using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccess;
using Sybase.DataWindow;

namespace GcoopServiceCs
{
    public class InsuranceService
    {
        private String connectionString;

        public InsuranceService(String connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// การ Init ค่าให้ DataWindow 2 ตัว คือ DwMain และ DwLoan<br />
        /// 1. สร้าง DataStore ตั้งชื่อว่า dwMain นำ XML จาก argument มา import ใส่<br />
        /// 2. ไป select ข้อมูลจากทะเบียนสมาชิกใส่ Sdt แล้วนำค่าต่างๆ ไปยิงใส่ dwMain<br />
        /// 3. ไป select ข้อมูลหุ้นแล้วยิงใส่ dwMain<br />
        /// 4. สร้าง DataStore สำหรับข้อมูลเงินกู้ dwLoan เพื่อหายอด sum ของเงินกู้ และ export xml เพื่อส่งไปให้หน้า UI<br />
        /// 5. เมื่อยิงค่าตั้งต้นของ dwMain ครบแล้วให้ export xml ของ dwMain ส่งไปให้หน้า UI 
        /// </summary>
        /// <param name="pbl">pbl แบบ fullpath</param>
        /// <param name="xmlInsuRequest">xml จาก DwMain</param>
        /// <returns>คืนค่า Array String รูปแบบ XML 2 ตัวคือ DwMain(d_as_insureqnew) และ DwLoan(d_as_loandetail)</returns>
        public String[] InitInsuRequestNew(String pbl, String xmlInsuRequest)
        {
            String[] resu = new String[2];
            resu[0] = "";
            resu[1] = "";
            Sta ta = new Sta(connectionString);
            try
            {
                //1. สร้าง DataStore ตั้งชื่อว่า dwMain นำ XML จาก argument มา import ใส่
                DataStore dwMain = new DataStore();
                dwMain.LibraryList = pbl;
                dwMain.DataWindowObject = "d_as_insureqnew";
                dwMain.Reset();
                dwMain.ImportString(xmlInsuRequest, FileSaveAsType.Xml);
                String memberNo = dwMain.GetItemString(1, "member_no");

                //2. ไป select ข้อมูลจากทะเบียนสมาชิกใส่ Sdt แล้วนำค่าต่างๆ ไปยิงใส่ dwMain
                String sql = @"
                    SELECT MBMEMBMASTER.MEMBER_NO,   
                             MBMEMBMASTER.MEMB_NAME,   
                             MBMEMBMASTER.MEMB_SURNAME,   
                             MBMEMBMASTER.MEMBGROUP_CODE,   
                             MBUCFPRENAME.PRENAME_SHORT,   
                             MBUCFMEMBGROUP.MEMBGROUP_DESC,   
                             MBMEMBMASTER.BIRTH_DATE,   
                             MBMEMBMASTER.MEMBER_DATE,  
                             MBMEMBMASTER.card_person
                    FROM MBMEMBMASTER,   
                             MBUCFMEMBGROUP,   
                             MBUCFPRENAME  
                    WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  
                             ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
                             ( ( MBMEMBMASTER.MEMBER_NO = '" + memberNo + "' ) )";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    // 2.1 เริ่มนำจาก Sdt dt มายิงให้กับ dwMain (*Sdt inherit มาจาก DataTable)
                    dwMain.SetItemString(1, "full_name", dt.GetString("PRENAME_SHORT") + dt.GetString("memb_name") + " " + dt.GetString("memb_surname"));
                    dwMain.SetItemString(1, "membgroup_code", dt.GetString("MEMBGROUP_CODE"));
                    dwMain.SetItemDateTime(1, "birth_date", dt.GetDate("birth_date"));
                    dwMain.SetItemString(1, "person_card", dt.GetString("card_person"));
                    int year = DateTime.Today.Year - dt.GetDate("birth_date").Year;
                    try
                    {
                        //3. ไป select ข้อมูลหุ้นแล้วยิงใส่ dwMain
                        String sqlShare = @"
                        select	sum( shsharemaster.sharestk_amt * shsharetype.share_value ) as summy
                        from	shsharemaster, shsharetype
                        where	( shsharetype.sharetype_code = shsharemaster.sharetype_code ) and
		                        ( ( shsharemaster.member_no = '" + memberNo + "' ) )";
                        Sdt dtSumShare = ta.Query(sqlShare);
                        if (dtSumShare.Next())
                        {
                            //3.1 ยิงค่าใส่ dwMain
                            dwMain.SetItemDecimal(1, "share_amt", dtSumShare.GetDecimal(0));
                        }
                        else throw new Exception();
                    }
                    catch { dwMain.SetItemDecimal(1, "share_amt", 0m); }
                    try
                    {
                        //4. สร้าง DataStore สำหรับข้อมูลเงินกู้ dwLoan เพื่อหายอด sum ของเงินกู้ และ export xml เพื่อส่งไปให้หน้า UI
                        DataStore dwLoan = new DataStore(pbl, "d_as_loandetail");
                        DwTrans sqlca = new DwTrans(connectionString);
                        sqlca.Connect();
                        try
                        {
                            dwLoan.SetTransaction(sqlca);
                            dwLoan.Retrieve(memberNo);
                            if (dwLoan.RowCount > 0)
                            {
                                //4.1 หายอด sum เพื่อยิงใส่ dwMain
                                decimal loanAmt = Convert.ToDecimal(dwLoan.Describe("evaluate( 'sum( principal_balance  for all )', " + dwLoan.RowCount + " )"));
                                dwMain.SetItemDecimal(1, "loan_amt", loanAmt);
                                //4.2 export xml ใส่ใน array ช่อง 1 เพื่อส่งกลับให้ UI
                                resu[1] = dwLoan.Describe("DataWindow.Data.XML");
                            }
                            else throw new Exception();
                        }
                        catch (Exception exLoan)
                        {
                            sqlca.Disconnect();
                            throw exLoan;
                        }
                        sqlca.Disconnect();
                    }
                    catch
                    {
                        dwMain.SetItemDecimal(1, "loan_amt", 0m);
                        resu[1] = "";
                    }

                    //5. เมื่อยิงค่าตั้งต้นของ dwMain ครบแล้วให้ export xml ของ dwMain ส่งไปให้หน้า UI 
                    resu[0] = dwMain.Describe("DataWindow.Data.XML");
                    ta.Close();
                }
                else
                {
                    throw new Exception("ไม่มีสมาชิก " + memberNo + " อยู่ในระบบ");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }

        public int SaveInsuRequestNew(String pbl, DateTime workDate, String xmlInsuRequest)
        {
            Sta ta = new Sta(connectionString);
            try
            {
                ta.Transection();
                DataStore dwMain = new DataStore(pbl, "d_as_insureqnew");
                dwMain.ImportString(xmlInsuRequest, FileSaveAsType.Xml);
                string docNo = "";
                docNo = new DocumentControl().NewDocumentNo(DocumentTypeCode.INSAPPLDOCNO, workDate.Year + 543, ta);
                dwMain.SetItemString(1, "insreqdoc_no", docNo);
                
                String sqlInsert = new DwHandle(dwMain).SqlInsertSyntax("INSREQNEW", 1);
                String sqlUpdate = new DwHandle(dwMain).SqlUpdateSyntax("INSREQNEW", 1);
                
                ta.Exe(sqlInsert);
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }
        // a init.ใบคำขอปิดประกัน
        public String[] InitInsuReqResign(String pbl, String xmlInsuReqResign)
        {
            String[] resu = new String[1];
            resu[0] = "";
            // resu[1] = "";
            Sta ta = new Sta(connectionString);
            try
            {
                //1. สร้าง DataStore ตั้งชื่อว่า dwMain นำ XML จาก argument มา import ใส่
                DataStore dwMain = new DataStore();
                dwMain.LibraryList = pbl;
                dwMain.DataWindowObject = "d_ins_mainresign";
                dwMain.Reset();
                dwMain.ImportString(xmlInsuReqResign, FileSaveAsType.Xml);
                String memberNo = dwMain.GetItemString(1, "member_no");

                //2. ไป select ข้อมูลจากทะเบียนสมาชิกใส่ Sdt แล้วนำค่าต่างๆ ไปยิงใส่ dwMain
                String sqlStr = @"    SELECT MBMEMBMASTER.MEMBER_NO,   
                             MBMEMBMASTER.MEMB_NAME,   
                             MBMEMBMASTER.MEMB_SURNAME,   
                             MBMEMBMASTER.MEMBGROUP_CODE,   
                             MBUCFPRENAME.PRENAME_SHORT,   
                             MBUCFMEMBGROUP.MEMBGROUP_DESC,   
                             MBMEMBMASTER.BIRTH_DATE,   
                             MBMEMBMASTER.MEMBER_DATE,  
                             MBMEMBMASTER.MEMBER_STATUS,  
                             MBMEMBMASTER.card_person,
                                MBUCFMEMBTYPE.MEMBTYPE_CODE,
                                MBUCFMEMBTYPE.MEMBTYPE_DESC
                    FROM MBMEMBMASTER,   
                             MBUCFMEMBGROUP,   
                             MBUCFPRENAME  ,MBUCFMEMBTYPE
                    WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  
                             ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and   ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and  
                             ( ( MBMEMBMASTER.MEMBER_NO = '" + memberNo + "' ) )";

                Sdt dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    // 2.1 เริ่มนำจาก Sdt dt มายิงให้กับ dwMain (*Sdt inherit มาจาก DataTable)

                    dwMain.SetItemString(1, "member_no", dt.GetString("MEMBER_NO"));
                    dwMain.SetItemString(1, "memb_name", dt.GetString("MEMB_NAME"));
                    dwMain.SetItemString(1, "memb_surname", dt.GetString("MEMB_SURNAME"));
                    dwMain.SetItemDate(1, "member_date", dt.GetDate("MEMBER_DATE"));
                    dwMain.SetItemString(1, "membgroup_code", dt.GetString("MEMBGROUP_CODE"));
                    dwMain.SetItemString(1, "membgroup_desc", dt.GetString("MEMBGROUP_DESC"));
                    dwMain.SetItemString(1, "membtype_code", dt.GetString("MEMBTYPE_CODE"));
                    dwMain.SetItemString(1, "membtype_desc", dt.GetString("MEMBTYPE_DESC"));
                    dwMain.SetItemDecimal(1, "member_status", dt.GetDecimal("MEMBER_STATUS"));


                    //3. เมื่อยิงค่าตั้งต้นของ dwMain ครบแล้วให้ export xml ของ dwMain ส่งไปให้หน้า UI 
                    resu[0] = dwMain.Describe("DataWindow.Data.XML");
                    ta.Close();
                }
                else
                {
                    throw new Exception("ไม่มีสมาชิก " + memberNo + " อยู่ในระบบ");
                }

            }
            catch (Exception ex)
            {
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }
        // a  save.ใบคำขอปิดประกัน
        public int SaveInsuReqResign(String pbl, DateTime workDate, String xmlInsuReqResign, String xmlInsuReqResigndel)
        {
            Sta ta = new Sta(connectionString);
            String sqlUpdate = "", sqlInsert = "";
            Decimal REQRESIGN_STATUS = 1, INSCOST_BALANCE;
            String
           INSTYPE_CODE,
           MEMBER_NO,
           INSGROUPDOC_NO,
           LEVEL_CODE,
           OPERATE_DATE,
           MBDEAD_DATE,
           SENDDOC_DATE,

           INSSTK_BLANCE,
           INSRESIGN_CASE,
           REMARK,
           ENTRY_DATE,
           ENTRY_ID,

           COOPBRANCH_ID,
           AFINE_BLANCE,
           CLOSEINS_DATE,
           CLOSEINS_ID,
           COMPACCEPT_DATE,
           APVIMMEDIATE_FLAG;
            try
            {
                ta.Transection();
                DataStore dwMain = new DataStore(pbl, "d_ins_mainresign");
                DataStore dwdetail = new DataStore(pbl, "d_ins_resign_det");
                dwMain.ImportString(xmlInsuReqResign, FileSaveAsType.Xml);
                dwdetail.ImportString(xmlInsuReqResigndel, FileSaveAsType.Xml);

                //dwMain.SetItemString(1, "insreqresign_no", docNo);
                //sqlInsert = new DwHandle(dwMain).SqlInsertSyntax("INSREQRESIGN", 1);
                //ta.Exe(sqlInsert);
                int row = dwdetail.RowCount;
                for (int i = 1; i <= row; i++)
                {
                    String INSREQRESIGN_NO = "";
                    INSREQRESIGN_NO = new DocumentControl().NewDocumentNo(DocumentTypeCode.INSCLDOCNO, workDate.Year + 543, ta);
                    INSTYPE_CODE = dwdetail.GetItemString(i, "instype_code");
                    MEMBER_NO = dwdetail.GetItemString(i, "member_no");
                    INSGROUPDOC_NO = dwdetail.GetItemString(i, "insgroupdoc_no");
                    INSRESIGN_CASE = dwdetail.GetItemString(i, "insresign_case");
                    INSCOST_BALANCE = dwdetail.GetItemDecimal(i, "inscost_blance");
                    Decimal operate_flag = dwdetail.GetItemDecimal(i, "operate_flag");
                    if (operate_flag == 1)
                    {

                        sqlInsert = @" INSERT INTO INSREQRESIGN  
                                                 ( INSREQRESIGN_NO,   
                                                   INSTYPE_CODE,   
                                                   MEMBER_NO,   
                                                   INSGROUPDOC_NO,   
                                                   LEVEL_CODE,   
                                                   OPERATE_DATE,   
                                                   MBDEAD_DATE,   
                                                   SENDDOC_DATE,   
                                                   INSCOST_BALANCE,   
                                                   INSSTK_BLANCE,   
                                                   INSRESIGN_CASE,   
                                                   REMARK,   
                                                   ENTRY_DATE,   
                                                   ENTRY_ID,   
                                                   REQRESIGN_STATUS,   
                                                   COOPBRANCH_ID,   
                                                   AFINE_BLANCE,   
                                                   CLOSEINS_DATE,   
                                                   CLOSEINS_ID,   
                                                   COMPACCEPT_DATE,   
                                                   APVIMMEDIATE_FLAG  )  
                                          VALUES ( '" + INSREQRESIGN_NO + @"',   
                                                   '" + INSTYPE_CODE + @"',   
                                                   '" + MEMBER_NO + @"',   
                                                   '" + INSGROUPDOC_NO + @"',   
                                                   null,   
                                                   null,   
                                                   null,   
                                                   null,   
                                                   " + INSCOST_BALANCE + @",   
                                                   null,   
                                                   '" + INSRESIGN_CASE + @"',   
                                                   null,   
                                                   null,   
                                                   null,   
                                                   " + REQRESIGN_STATUS + @",   
                                                   null,   
                                                  null,    
                                                   null,   
                                                   null,    
                                                  null,    
                                                   null )";
                        ta.Exe(sqlInsert);
                        dwdetail.SetItemDecimal(i, "insmemb_status", 0);
                        sqlUpdate = new DwHandle(dwdetail).SqlUpdateSyntax("INSGROUPMASTER", i);
                        ta.Exe(sqlUpdate);
                

                    }

                }


               ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }
        // a save.อนุมัติสมัครประกัน
        public int SaveAppinsnew(String member_no)
        {
            Sta ta = new Sta(connectionString);
            try
            {
                ta.Transection();               
                String sqlStr, instype_code, level_code, insgroupdoc_no, remark, insreqdoc_no,
                    kpmembno, marrige_name, insgroupno_ref, person_card;
                DateTime insgroup_date, insreq_date, inswd_date, resign_date, process_date,
                         mbdead_date, senddoc_date, compaccept_date, startsafe_date, endsafe_date;
                Decimal inscost_blance, insperod_payment, insstk_blance, last_period, insmemb_status, last_stm_no,
                        inspayment_arrear, inspayment_status, loan_amt, share_amt, misspay_amt, afine_blance, loanreq_amt, first_period, inskpothers_status,
                        insmemb_type, company_no, periodpay_amt, inspayment_type, insreturn_amt, inspayment_amt, insgroup_id;
               
                Sdt dt1 = new Sdt();
                Sdt dt2 = new Sdt();

                sqlStr = @"  SELECT INSREQDOC_NO,    MEMBER_NO,     MEMBGROUP_CODE,    INREQ_DATE,      INSAPROVE_DATE,   INSREQCOST_AMT,   INSAPCOST_AMT,   EX_STATUS1,   
                                EX_STATUS2,      EX_STATUS3,     EX_STATUS4,       LOAN_AMT,        SHARE_AMT,         ENTRY_ID,        ENTRY_DATE,      INSREQ_STATUS, 
                                INSGROUPDOC_NO,  APPROVE_ID,    INSTYPE_CODE,      INSPERIOD_PAYMENT,   COOPBRANCH_ID,   BIRTH_DATE,   MEMBER_DATE,   AGE, 
                                INSLEVEL_CODE,   LOANREQ_AMT,   MARRIGE_NAME,      INSGROUPNO_REF,   INSMEMB_TYPE,   PERSON_CARD,   INSPAYMENT_ARREAR,  
                                INSPAYMENT_AMT  
                         FROM INSREQNEW  
                         WHERE MEMBER_NO = '" + member_no + "'   ";
                dt1 = ta.Query(sqlStr);
                dt1.Next();
                try
                {
                    insreqdoc_no = dt1.GetString("INSREQDOC_NO");
                }
                catch { insreqdoc_no = "0000000"; }
                insgroupdoc_no = member_no;//dt1.GetString("INSGROUPDOC_NO");
                try
                {
                    instype_code = dt1.GetString("INSTYPE_CODE");
                }
                catch { instype_code = "01"; };
                try
                {
                    level_code = dt1.GetString("INSLEVEL_CODE");
                }
                catch { level_code = ""; }
                try
                {
                    person_card = dt1.GetString("PERSON_CARD");
                }
                catch { person_card = ""; }
                try
                {
                    inscost_blance = dt1.GetDecimal("INSREQCOST_AMT");
                }
                catch { inscost_blance = 0; }
                try
                {
                    loan_amt = dt1.GetDecimal("LOAN_AMT");
                }
                catch { loan_amt = 0; }
                try
                {
                    share_amt = dt1.GetDecimal("SHARE_AMT");
                }
                catch { share_amt = 0; }
                try
                {
                    loanreq_amt = dt1.GetDecimal("LOANREQ_AMT");
                }
                catch { loanreq_amt = 0; }
                try
                {
                    inspayment_arrear = dt1.GetDecimal("INSPAYMENT_ARREAR");
                }
                catch { inspayment_arrear = 0; }
                try
                {
                    inspayment_amt = dt1.GetDecimal("INSPAYMENT_AMT");
                }
                catch { inspayment_amt = 0; }
                try
                {
                    periodpay_amt = dt1.GetDecimal("INSPERIOD_PAYMENT");
                }
                catch { periodpay_amt = 0; }
                try
                {
                    insreq_date = dt1.GetDate("INREQ_DATE");
                }
                catch { insreq_date = DateTime.Now; }
                try
                {
                    marrige_name = dt1.GetString("MARRIGE_NAME");
                }
                catch { marrige_name = ""; }
                try
                {
                    insmemb_type = dt1.GetDecimal("INSMEMB_TYPE");
                }
                catch { insmemb_type = 1; }
                try
                {
                    insgroup_date = dt1.GetDate("INSAPROVE_DATE");
                }
                catch { insgroup_date = DateTime.Now; }

                sqlStr = "select max(insgroup_id)as insgroup_id  from insgroupmaster";
                dt2 = ta.Query(sqlStr);
                dt2.Next();
                insgroup_id = dt2.GetDecimal("insgroup_id") + 1;
                sqlStr = @"insert into insgroupmaster (member_no,  insreqdoc_no,  insgroupdoc_no, instype_code,
                                                   level_code ,  person_card , inscost_blance , loan_amt ,
                                                   share_amt , loanreq_amt,  inspayment_arrear ,  inspayment_amt ,  
                                                   periodpay_amt , insreq_date , marrige_name ,insmemb_type,
                                                   insgroup_date ,insgroup_id) 
                                            values( '" + member_no + @"',
                                                    '" + insreqdoc_no + @"', 
                                                   '" + insgroupdoc_no + @"', 
                                                    '" + instype_code + @"',
                                                    '" + level_code + @"',
                                                    '" + person_card + @"',
                                                   " + inscost_blance + @",
                                                  " + loan_amt + @",
                                                  " + share_amt + @",
                                                  " + loanreq_amt + @",
                                                   " + inspayment_arrear + @",
                                                    " + inspayment_amt + @",
                                                  " + periodpay_amt + @",
                                                 to_date('" + insreq_date.ToString("dd/MM/yyyy") + @"', 'dd/mm/yyyy'),

                                                  '" + marrige_name + @"',
                                                   " + insmemb_type + @",
                                                 to_date('" + insgroup_date.ToString("dd/MM/yyyy") + @"', 'dd/mm/yyyy'),
                                                   " + insgroup_id + @")";
              
                //             sqlStr = @"  INSERT INTO INSGROUPMASTER  
                //         ( MEMBER_NO,    INSTYPE_CODE,   LEVEL_CODE,   INSGROUPDOC_NO,   INSGROUP_DATE,   INSREQ_DATE,   INSWD_DATE,   INSCOST_BLANCE,   INSPEROD_PAYMENT,   INSSTK_BLANCE,   
                //           LAST_PERIOD,   INSMEMB_STATUS,   RESIGN_DATE,   LAST_STM_NO,   INSPAYMENT_ARREAR,   INSPAYMENT_STATUS,   LOAN_AMT,   SHARE_AMT,   REMARK,   PROCESS_DATE,   
                //           MISSPAY_AMT,   INSREQDOC_NO,   MBDEAD_DATE,   SENDDOC_DATE,   COMPACCEPT_DATE,   AFINE_BLANCE,   LOANREQ_AMT,   FIRST_PERIOD,   STARTSAFE_DATE,   ENDSAFE_DATE,   
                //           INSKPOTHERS_STATUS,   KPMEMBNO,   MARRIGE_NAME,   INSMEMB_TYPE,   INSGROUPNO_REF,   PERSON_CARD,   COMPANY_NO,   PERIODPAY_AMT,   INSPAYMENT_TYPE,   INSRETURN_AMT,   
                //           INSPAYMENT_AMT,   INSGROUP_ID )  
                //  VALUES (  'member_no',  'instype_code',  'level_code',  'insgroupdoc_no',  insgroup_date,   insreq_date,  inswd_date,   
                //   inscost_blance,  insperod_payment,   insstk_blance,    last_period,   insmemb_status,  resign_date,  last_stm_no,   
                // inspayment_arrear,  inspayment_status,  loan_amt,   share_amt,'remark',  process_date,   
                // misspay_amt,   'insreqdoc_no',  mbdead_date,  senddoc_date,   compaccept_date,   
                //   afine_blance,   
                // loanreq_amt,   
                // first_period,   
                // startsafe_date,   
                //endsafe_date,   
                // inskpothers_status,   
                // 'kpmembno',   
                //   'marrige_name',   
                // insmemb_type,   
                // 'insgroupno_ref',   
                // 'person_card',   
                // company_no,   
                // periodpay_amt,   
                // inspayment_type,   
                //insreturn_amt,   
                // inspayment_amt,   
                //  insgroup_id )  ";
                ta.Exe(sqlStr);
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }
        
        //-- By AOI 01.04.2011 
        public String[] InitInsuSliReq(String pbl, String xmlInsuReqResign)
        {
            String[] resu = new String[2];
            resu[0] = "";
            resu[1] = "";
            Sta ta = new Sta(connectionString);
            try
            {
                //1. สร้าง DataStore ตั้งชื่อว่า dwMain นำ XML จาก argument มา import ใส่
                DataStore dwMain = new DataStore();
                dwMain.LibraryList = pbl;
                dwMain.DataWindowObject = "d_ins_slip_operate_main";
                dwMain.Reset();
                dwMain.ImportString(xmlInsuReqResign, FileSaveAsType.Xml);
                String memberNo = dwMain.GetItemString(1, "member_no");

                //2. ไป select ข้อมูลจากทะเบียนสมาชิกใส่ Sdt แล้วนำค่าต่างๆ ไปยิงใส่ dwMain
                String sql = @"
                    SELECT MBMEMBMASTER.MEMBER_NO,   
                             MBMEMBMASTER.MEMB_NAME,   
                             MBMEMBMASTER.MEMB_SURNAME,   
                             MBMEMBMASTER.MEMBGROUP_CODE,   
                             MBUCFPRENAME.PRENAME_SHORT,   
                             MBUCFMEMBGROUP.MEMBGROUP_DESC,   
                             MBMEMBMASTER.BIRTH_DATE,   
                             MBMEMBMASTER.MEMBER_DATE,  
                             MBMEMBMASTER.card_person
                    FROM MBMEMBMASTER,   
                             MBUCFMEMBGROUP,   
                             MBUCFPRENAME  
                    WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  
                             ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
                             ( ( MBMEMBMASTER.MEMBER_NO = '" + memberNo + "' ) )";

                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    // 2.1 เริ่มนำจาก Sdt dt มายิงให้กับ dwMain (*Sdt inherit มาจาก DataTable)
                    dwMain.SetItemString(1, "prename_desc", dt.GetString("PRENAME_SHORT"));
                    dwMain.SetItemString(1, "memb_name", dt.GetString("MEMB_NAME"));
                    dwMain.SetItemString(1, "memb_surname", dt.GetString("MEMB_SURNAME"));
                    dwMain.SetItemString(1, "membgroup_code", dt.GetString("MEMBGROUP_CODE"));

                    try
                    {
                        //4. สร้าง DataStore สำหรับข้อมูลเงินกู้ dwLoan เพื่อหายอด sum ของเงินกู้ และ export xml เพื่อส่งไปให้หน้า UI
                        DataStore dwLoan = new DataStore(pbl, "d_ins_slip_operate_etc");
                        DwTrans sqlca = new DwTrans(connectionString);
                        sqlca.Connect();
                        try
                        {
                            dwLoan.SetTransaction(sqlca);
                            dwLoan.Retrieve(memberNo);
                            if (dwLoan.RowCount > 0)
                            {
                                //4.1 หายอด sum เพื่อยิงใส่ dwMain
                                //decimal loanAmt = Convert.ToDecimal(dwLoan.Describe("evaluate( 'sum( principal_balance  for all )', " + dwLoan.RowCount + " )"));
                                //dwMain.SetItemDecimal(1, "loan_amt", loanAmt);
                                //4.2 export xml ใส่ใน array ช่อง 1 เพื่อส่งกลับให้ UI
                                resu[1] = dwLoan.Describe("DataWindow.Data.XML");

                                //------- AOI เพิ่มเติม ตัวเลขยอดเงินต้องรวมไว้เรียบร้อย 05.04.2011-09.52 copy มาตอน 05.04.2011-10.02
                                Decimal cost = 0;
                                for (int r = 0; r < dwLoan.RowCount; r++)
                                {
                                    cost = (cost == 0) ? dwLoan.GetItemDecimal(r + 1, "insperod_payment") : cost + dwLoan.GetItemDecimal(r + 1, "insperod_payment");

                                }
                                dwMain.SetItemDecimal(1, "slip_amt", cost);
                                //----------------- End 05.04.2011-09.52

                            }
                            else throw new Exception();
                        }
                        //{
                        //    dwLoan.SetTransaction(sqlca);
                        //    dwLoan.Retrieve(memberNo);
                        //    if (dwLoan.RowCount > 0)
                        //    {
                        //        //4.1 หายอด sum เพื่อยิงใส่ dwMain
                        //        //decimal loanAmt = Convert.ToDecimal(dwLoan.Describe("evaluate( 'sum( principal_balance  for all )', " + dwLoan.RowCount + " )"));
                        //        //dwMain.SetItemDecimal(1, "loan_amt", loanAmt);
                        //        //4.2 export xml ใส่ใน array ช่อง 1 เพื่อส่งกลับให้ UI
                        //        resu[1] = dwLoan.Describe("DataWindow.Data.XML");
                        //    }
                        //    else throw new Exception();
                        //}
                        catch (Exception exLoan)
                        {
                            sqlca.Disconnect();
                            throw exLoan;
                        }
                        sqlca.Disconnect();
                    }
                    catch
                    {
                        // dwMain.SetItemDecimal(1, "loan_amt", 0m);
                        resu[1] = "";
                    }
                    // ---- สร้างตัว Detail




                    //3. เมื่อยิงค่าตั้งต้นของ dwMain ครบแล้วให้ export xml ของ dwMain ส่งไปให้หน้า UI 
                    resu[0] = dwMain.Describe("DataWindow.Data.XML");
                    ta.Close();


                }
                else
                {
                    throw new Exception("ไม่มีสมาชิก " + memberNo + " อยู่ในระบบ");

                }
            }
            catch (Exception ex)
            {
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }

        //        public String[] InitInsuSliDet(String pbl, String xmlInsuReqResign,String memberno)
        //        {
        //            String[] resu = new String[1];
        //            resu[0] = "";
        //            // resu[1] = "";
        //            Sta ta = new Sta(connectionString);
        //            try
        //            {
        //                //1. สร้าง DataStore ตั้งชื่อว่า dwMain นำ XML จาก argument มา import ใส่
        //                DataStore dwMain = new DataStore();
        //                dwMain.LibraryList = pbl;
        //                dwMain.DataWindowObject = "d_ins_slip_operate_etc";
        //                dwMain.Reset();
        //                dwMain.ImportString(xmlInsuReqResign, FileSaveAsType.Xml);
        //                //String memberno = dwMain.GetItemString(1, "memberno");

        //                //2. ไป select ข้อมูลจากทะเบียนสมาชิกใส่ Sdt แล้วนำค่าต่างๆ ไปยิงใส่ dwMain
        ////                String sql = @"select Slipitemtype_code ,Slipitem_Desc,item_payamt,item_balance 
        ////from cmshrlonslipdet where slip_no in(select cmshrlonslip.slip_no from cmshrlonslip where member_no ='" + memberno + "')";
        //                String sql = @"select insgroupmaster.member_no,insgroupmaster.instype_code,insgroupmaster.insperod_payment,insgroupmaster.inspayment_arrear 
        //from insgroupmaster left join insurencetype on insgroupmaster.instype_code = insurencetype.instype_code
        //where insgroupmaster.member_no ='" + memberno + "'";


        //                Sdt dt = ta.Query(sql);
        //                if (dt.Next())
        //                {
        //                    // 2.1 เริ่มนำจาก Sdt dt มายิงให้กับ dwMain (*Sdt inherit มาจาก DataTable)
        //                    dwMain.SetItemString(1, "insgroupmaster_instype_code", dt.GetString("instype_code"));
        //                    dwMain.SetItemString(1, "insurencetype_inscompany_name", dt.GetString("inscompany_name"));
        //                    dwMain.SetItemDecimal(1, "insgroupmaster_insperod_payment", dt.GetDecimal("insperod_payment"));
        //                    dwMain.SetItemDecimal(1, "insgroupmaster_inspayment_arrear", dt.GetDecimal("inspayment_arrear"));

        //                    //3. เมื่อยิงค่าตั้งต้นของ dwMain ครบแล้วให้ export xml ของ dwMain ส่งไปให้หน้า UI 
        //                    resu[0] = dwMain.Describe("DataWindow.Data.XML");
        //                    ta.Close();
        //                }
        //                else
        //                {
        //                    throw new Exception("ไม่มีข้อมูล " + memberno + " อยู่ในระบบ");

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                try
        //                {
        //                    ta.Close();
        //                }
        //                catch { }
        //                throw ex;
        //            }
        //            return resu;
        //        }

        // by aoi 01.04.2011
        //-- 05.04.2011 แก้เรื่อง commit trnasection
        public int SaveInsuSliReq(String pbl, DateTime workDate, String xmlInsuReqResign, String xmlInsuSlipDet)
        {
            Sta ta = new Sta(connectionString);
            ta.Transection();
            try
            {

                DataStore dwMain = new DataStore(pbl, "d_ins_slip_operate_main");
                dwMain.ImportString(xmlInsuReqResign, FileSaveAsType.Xml);

                System.Globalization.CultureInfo den = new System.Globalization.CultureInfo("en-US");
                String crrDate = DateTime.Now.ToString("dd/MM/yyyy");
                string[] str = crrDate.Split('/');
                DateTime d = Convert.ToDateTime(new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[1]), Convert.ToInt32(str[0])), den);




                string membno = dwMain.GetItemString(1, "member_no");
                string entryid = dwMain.GetItemString(1, "entry_id");
                string docNo = "";
                string slipNo = "";
                docNo = new DocumentControl().NewDocumentNo(DocumentTypeCode.INSCLDOCNO, workDate.Year + 543, ta);
                dwMain.SetItemString(1, "document_no", docNo);
                dwMain.SetItemString(1, "slip_no", docNo);
                dwMain.SetItemDateTime(1, "entry_date", DateTime.Now);
                dwMain.SetItemDateTime(1, "slip_date", d);
                dwMain.SetItemDateTime(1, "operate_date", d);

                slipNo = docNo;

                String sqlInsert = new DwHandle(dwMain).SqlInsertSyntax("CMSHRLONSLIP", 1);
                ta.Exe(sqlInsert);





                DataStore dwDetail = new DataStore(pbl, "d_ins_slip_operate_etc");
                dwDetail.ImportString(xmlInsuSlipDet, FileSaveAsType.Xml);

                int rw = dwDetail.RowCount;
                int[] k = new int[rw];
                for (int m = 0; m < rw; m++)
                {
                    Decimal chkBox = dwDetail.GetItemDecimal(m + 1, "totalcost");
                    if (chkBox == 1)
                    {
                        //-- แก้ไขเพิ่มเติม 03.04.2011-10.44
                        String typecode = dwDetail.GetItemString(m + 1, "instype_code");
                        String docCode = dwDetail.GetItemString(m + 1, "insgroupdoc_no");
                        String descrpt = dwDetail.GetItemString(m + 1, "inscompany_name");

                        Decimal pay = dwDetail.GetItemDecimal(m + 1, "insperod_payment");
                        Decimal remain = dwDetail.GetItemDecimal(m + 1, "inspayment_arrear");
                        Decimal insstk_balance = dwDetail.GetItemDecimal(m + 1, "insstk_blance");
                        Decimal stm_no = dwDetail.GetItemDecimal(m + 1, "last_stm_no");
                        Decimal priod_no = dwDetail.GetItemDecimal(m + 1, "last_period");
                        Decimal xRem = remain - pay; //--- ยอดคงเหลือค้างชำระ
                        Decimal grpID = dwDetail.GetItemDecimal(m + 1, "insgroup_id");
                        Decimal ldk_balance = dwDetail.GetItemDecimal(m + 1, "insstk_blance");



                        ldk_balance = ldk_balance + pay;

                        dwDetail.SetItemDecimal(m + 1, "inspayment_arrear", xRem);
                        dwDetail.SetItemDecimal(m + 1, "insstk_blance", insstk_balance + pay);
                        dwDetail.SetItemDecimal(m + 1, "last_period", priod_no);
                        dwDetail.SetItemDecimal(m + 1, "last_stm_no", stm_no + 1);


                        //==============================================================================
                        // insgroupmaster
                        String sqlUpdate = @"UPDATE insgroupmaster set
                            inspayment_arrear = " + xRem + @", 
                            insgroup_date = to_date('" + crrDate + @"','dd/mm/yyyy'),
                            insreq_date = to_date('" + crrDate + @"','dd/mm/yyyy'),
                            insstk_blance = nvl(insstk_blance,0)+" + pay + @",
                            last_period = nvl(last_period,0)+1,
                            last_stm_no = nvl(last_stm_no,0) + 1 WHERE member_no = '" + membno + @"'  
                            and instype_code='" + typecode + @"' 
                            and insgroupdoc_no='" + docCode + "'";
                        ta.Exe(sqlUpdate);



                        //==============================================================================
                        // cmshrlonslipdet
                        Decimal nxtNo = NextNo("cmshrlonslipdet", "slip_no", slipNo, ta);

                        sqlInsert = @"insert into cmshrlonslipdet(slip_no,slipitemtype_code,seq_no,shrlontype_code,
                                    operate_flag,Slipitem_desc,item_payamt,item_balance)
                                    values('" + slipNo + @"',
                                    'INA'," + nxtNo + @",
                                    '" + typecode + @"',
                                    1,'" + descrpt + @"',
                                    " + pay + "," + xRem + ")";

                        ta.Exe(sqlInsert);


                        //==============================================================================
                        //   insgroupstatement;


                        decimal xseq_no = NextNo("insgroupstatement", "insgroup_id", grpID.ToString(), ta);
                        String dsql = @"insert into insgroupstatement (member_no,instype_code,seq_no,insitemtype_code,
                        insperiod_payment,insprince_balance,operate_date,insgroupslip_date,entry_date,
                        entry_id,insgroupdoc_no,insgroup_id,refdoc_no)
                        values('" + membno + "','" + typecode + "'," + xseq_no + ",'IPX'," + pay + @",
                        " + ldk_balance + @",
                        to_date('" + crrDate + @"','dd/mm/yyyy'),
                        to_date('" + crrDate + @"','dd/mm/yyyy'),
                        to_date('" + crrDate + @"','dd/mm/yyyy'),
                        '" + entryid + @"',
                        '" + docCode + "'," + grpID + ",'" + slipNo + "')";

                        ta.Exe(dsql);


                    }

                }


                //===============================End Edit
                ta.Commit();
                ta.Close();
                //---- 01.04.2011 Save Detail
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }

        private Decimal NextNo(String tbl, String where, String vle, Sta ta)
        {

            String sql = @"select  nvl(max(seq_no),0) +1 nextNo from " + tbl + " where " + where + " = '" + vle + "'";
            Decimal seqNo = 0;
            Sdt dtDtl = ta.Query(sql);
            dtDtl.Next();
            seqNo = dtDtl.GetDecimal("nextNo");
            return seqNo;
        }
       
        public int SaveSlipDet(String pbl, DateTime workDate, String xmlInsuReqRequest, String slipno)
        {
            Sta ta = new Sta(connectionString);
            try
            {
                ta.Transection();
                DataStore dwDetail = new DataStore(pbl, "d_ins_slip_operate_etc");
                dwDetail.ImportString(xmlInsuReqRequest, FileSaveAsType.Xml);
                string docNo = "";
                docNo = new DocumentControl().NewDocumentNo(DocumentTypeCode.INSCLDOCNO, workDate.Year + 543, ta);
                dwDetail.SetItemString(1, "seq_no", docNo);
                dwDetail.SetItemString(1, "slip_no", slipno);

                String sqlInsert = new DwHandle(dwDetail).SqlInsertSyntax("cmshrlonslipdet", 1);
                String sqlUpdate = new DwHandle(dwDetail).SqlUpdateSyntax("cmshrlonslipdet", 1);


                ta.Exe(sqlInsert);
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }
        //---- 31.03.2011 Save Detail

        //by kae ยกเลิกปิดประกัน
        public int SaveCancelresign(String pbl, DateTime workDate, String xmlInsuCancelResign)
        {
            Sta ta = new Sta(connectionString);
            try
            {
                ta.Transection();
                DataStore dwMain = new DataStore(pbl, "d_ins_approve_listnew");
                dwMain.ImportString(xmlInsuCancelResign, FileSaveAsType.Xml);
                String sqlUpdate = "";
                // DwUtil.UpdateDateWindow(dw_search, "as_appinsresign.pbl", "INSREQRESIGN");
                int row = dwMain.RowCount;
                for (int i = 1; i <= row; i++)
                {
                    string doc_no;

                    decimal status = dwMain.GetItemDecimal(i, "reqresign_status");
                    try
                    {
                        doc_no = dwMain.GetItemString(i, "insgroupdoc_no");
                    }
                    catch { doc_no = ""; }
                    string member_no = dwMain.GetItemString(i, "member_no");
                    if (status == 8)
                    {

                        String sql = "UPDATE insgroupmaster set insmemb_status= 1,insresign_case ='05' where INSGROUPMASTER.member_no='" + member_no + "' and INSGROUPMASTER.insgroupdoc_no='" + doc_no + "'";
                        sqlUpdate = new DwHandle(dwMain).SqlUpdateSyntax("INSREQRESIGN", i);
                        ta.Exe(sqlUpdate);
                        

                    }
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return 1;
        }
    }
}
