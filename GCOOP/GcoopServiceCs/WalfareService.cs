
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecurityEngine;
using Sybase.DataWindow;
using DBAccess;
using GcoopServiceCs.WfStruct;
using System.Data;
using System.Globalization;

namespace GcoopServiceCs
{
    public class WalfareService
    {
        private WsSecurity sec;
        private String application;
        private String auditDocno = "";

        public WalfareService(String wsPass, String application)
        {
            sec = new WsSecurity(wsPass);
            this.application = application;
        }

        //--------------------------------------------------------

        private int PermissAll(UserAccount userAcc)
        {
            int result = 0;
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                result = PermissAll(userAcc, ta);
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return 1;
        }

        private int PermissAll(UserAccount userAcc, Sta ta)
        {
            
            if (!userAcc.AdminOption)
            {
                String sql1 = "delete from amsecpermiss where application='" + application + "' and user_name='" + userAcc.UserName + 
                    "' and window_id in(select window_id from amsecwins where group_code <> 'A')";
                ta.Exe(sql1);
            }
            String adminCmd = userAcc.AdminOption ? " 1 = 1 " : " group_code = 'A' ";
            String sqlQ1 = "select * from amsecwins where application='" + application + "' and " + adminCmd + 
                            " and window_id not in(select window_id from amsecpermiss where application='" + application + 
                            "' and user_name='" + userAcc.UserName + "') order by group_code, win_order";
            Sdt dt = ta.Query(sqlQ1);
            int iii = 0;
            while (dt.Next())
            {
                String sql = "insert into amsecpermiss(user_name, application, window_id, save_status, check_flag)values('{0}', '{1}', '{2}', {3}, {4})";
                sql = string.Format(sql, userAcc.UserName, application, dt.GetString("window_id"), 1, 1); //tar
                ta.Exe(sql);
                iii++;
                if (iii > 2000) break;
            }
            return 1;
        }

        //--------------------------------------------------------

        private void ToPhycalPathPbl(ref String pbl)
        {
            pbl = sec.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + pbl;
        }

        public int[] SaveReqWalfareNew(String pbl, String xmlDwMain, String xmlDwRelate, String xmlDwSlip)
        {
            this.ToPhycalPathPbl(ref pbl);
            int[] resu = new int[2];

            DataStore dwMain = new DataStore(pbl, "d_dp_reqdepoist_main");
            dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);

            DataStore dwRelate = new DataStore(pbl, "d_dp_reqcodeposit");
            dwRelate.ImportString(xmlDwRelate, FileSaveAsType.Xml);

            DataStore dwSlip = new DataStore(pbl, "d_wf_reqdetail");
            dwSlip.ImportString(xmlDwSlip, FileSaveAsType.Xml);

            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                DocumentControl dct = new DocumentControl();
                String deptRequestNo = dct.NewDocumentNo("WCAPPLDOCNO", 2554, ta);
                String branch_id = dwMain.GetItemString(1, "branch_id");

                decimal immediappFlag = dwMain.GetItemDecimal(1, "immediapp_flag");
                dwMain.SetItemString(1, "deptrequest_docno", deptRequestNo);
                String branchId = dwMain.GetItemString(1, "branch_id");

                String prenameCode = dwMain.GetItemString(1, "prename_code");
                String sqlPrename = "select prename_desc from mbucfprename where prename_code='" + prenameCode + "'";
                Sdt dtPrename = ta.Query(sqlPrename);
                if (dtPrename.Next())
                {
                    dwMain.SetItemString(1, "wfaccount_name", dtPrename.GetString(0) + dwMain.GetItemString(1, "deptaccount_name") + " " + dwMain.GetItemString(1, "deptaccount_sname"));
                }
                if (immediappFlag == 1)
                {
                    String sqlLastAccNo = "select max(last_accno) as maxlast_accno from cmucfcoopbranch where coopbranch_id = '" + branchId + "'";
                    Sdt dtLastAccNo = ta.Query(sqlLastAccNo);
                    String lastAccNo = "";
                    if (dtLastAccNo.Next())
                    {
                        String sqlUpdateLastAccNo = "update cmucfcoopbranch set last_accno = " + (dtLastAccNo.GetInt32(0) + 1) + " where coopbranch_id = '" + branchId + "'";
                        ta.Exe(sqlUpdateLastAccNo);
                        lastAccNo = (dtLastAccNo.GetInt32(0) + 1).ToString("000000");
                    }
                    dwMain.SetItemString(1, "deptaccount_no", lastAccNo);
                }
                else
                {
                    dwMain.SetItemString(1, "deptaccount_no", "");
                }

                String sql = new DwHandle(dwMain).SqlInsertSyntax("WCREQDEPOSIT", 1);
                resu[0] = ta.Exe(sql);

                int rlCount = dwRelate.RowCount;
                for (int i = 1; i <= rlCount; i++)
                {
                    dwRelate.SetItemString(i, "deptrequest_docno", deptRequestNo);
                    dwRelate.SetItemString(i, "branch_id", branch_id);
                    dwRelate.SetItemDecimal(i, "seq_no", i);
                    String sqlRelate = new DwHandle(dwRelate).SqlInsertSyntax("WCREQCODEPOSIT", i);
                    ta.Exe(sqlRelate);
                }

                for (int i = 1; i <= dwSlip.RowCount; i++)
                {
                    dwSlip.SetItemDecimal(i, "seq_no", i);
                    dwSlip.SetItemString(i, "branch_id", branch_id);
                    dwSlip.SetItemString(i, "wftype_code", dwMain.GetItemString(1, "wftype_code"));
                    dwSlip.SetItemString(i, "deptrequest_docno", deptRequestNo);
                    dwSlip.SetItemString(i, "depttype_code", dwMain.GetItemString(1, "depttype_code"));

                    //decimal statusPay = dwSlip.GetItemDecimal(i, "status_pay");
                    //if (statusPay == 0)
                    //{
                    //    dwSlip.SetItemDecimal(i, "status_pay", 8);
                    //}
                    string sqlSlip = new DwHandle(dwSlip).SqlInsertSyntax("WCREQDETAIL", i);
                    ta.Exe(sqlSlip);
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return resu;
        }

        public String SaveReqWalfareNewLight(String pbl, DateTime workDate, DataTable dtMain, DataTable dtSlip, DataTable dtRelate)
        {
            //if (CheckDuplicate(pbl, dtMain) != true)
            //{
            //    throw new Exception("บุคคลนี้มีใบคำขอสมัครสมาชิกอยู่แล้ว");
            //}
            if (CheckData(pbl, dtMain) != true)
            {
                throw new Exception("กรูณากรอกข้อมูลให้ถูกต้อง");
            }

            this.ToPhycalPathPbl(ref pbl);
            String resu = "";

            DataStore dwMain = new DataStore(pbl, "d_dp_reqdepoist_main");
            //dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
            DwHandle.ImportData(dtMain, dwMain);

            DataStore dwSlip = new DataStore(pbl, "d_wf_reqdetail");
            //dwSlip.ImportString(xmlDwSlip, FileSaveAsType.Xml);
            DwHandle.ImportData(dtSlip, dwSlip);

            DataStore dwRelate = new DataStore(pbl, "d_dp_reqcodeposit");
            //dwRelate.ImportString(xmlDwRelate, FileSaveAsType.Xml);
            DwHandle.ImportData(dtRelate, dwRelate);

            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                DocumentControl dct = new DocumentControl();
                CultureInfo th = new CultureInfo("th-TH");
                int WorkYear = workDate.Year + 543;
                String deptRequestNo = dct.NewDocumentNo("WCAPPLDOCNO", WorkYear, ta);
                String branch_id = dwMain.GetItemString(1, "branch_id");

                decimal immediappFlag = dwMain.GetItemDecimal(1, "immediapp_flag");
                dwMain.SetItemString(1, "deptrequest_docno", deptRequestNo);
                String branchId = dwMain.GetItemString(1, "branch_id");

                String prenameCode = dwMain.GetItemString(1, "prename_code");
                String sqlPrename = "select prename_desc from mbucfprename where prename_code='" + prenameCode + "'";
                Sdt dtPrename = ta.Query(sqlPrename);
                if (dtPrename.Next())
                {
                    dwMain.SetItemString(1, "wfaccount_name", dtPrename.GetString(0) + dwMain.GetItemString(1, "deptaccount_name") + " " + dwMain.GetItemString(1, "deptaccount_sname"));
                }
                if (immediappFlag == 1)
                {
                    String sqlLastAccNo = "select max(last_accno) as maxlast_accno from cmucfcoopbranch where coopbranch_id = '" + branchId + "'";
                    Sdt dtLastAccNo = ta.Query(sqlLastAccNo);
                    String lastAccNo = "";
                    if (dtLastAccNo.Next())
                    {
                        String sqlUpdateLastAccNo = "update cmucfcoopbranch set last_accno = " + (dtLastAccNo.GetInt32(0) + 1) + " where coopbranch_id = '" + branchId + "'";
                        ta.Exe(sqlUpdateLastAccNo);
                        lastAccNo = (dtLastAccNo.GetInt32(0) + 1).ToString("000000");
                    }
                    dwMain.SetItemString(1, "deptaccount_no", lastAccNo);
                }
                else
                {
                    dwMain.SetItemString(1, "deptaccount_no", "");
                }

                String sql = new DwHandle(dwMain).SqlInsertSyntax("WCREQDEPOSIT", 1);
                int ire = ta.Exe(sql);

                int rlCount = dwRelate.RowCount;
                for (int i = 1; i <= rlCount; i++)
                {
                    dwRelate.SetItemString(i, "deptrequest_docno", deptRequestNo);
                    dwRelate.SetItemString(i, "branch_id", branch_id);
                    dwRelate.SetItemDecimal(i, "seq_no", i);
                    String sqlRelate = new DwHandle(dwRelate).SqlInsertSyntax("WCREQCODEPOSIT", i);
                    ta.Exe(sqlRelate);
                }

                ///slip
                decimal amt = 0;
                decimal[] prncslip_amt = new decimal[dwSlip.RowCount];
                string[] deptitemtype_code = new string[dwSlip.RowCount];
                string[] deptitemtype_desc = new string[dwSlip.RowCount];
                ///end slip
                for (int i = 1; i <= dwSlip.RowCount; i++)
                {
                    dwSlip.SetItemDecimal(i, "seq_no", i);
                    dwSlip.SetItemString(i, "branch_id", branch_id);
                    dwSlip.SetItemString(i, "wftype_code", dwMain.GetItemString(1, "wftype_code"));
                    dwSlip.SetItemString(i, "deptrequest_docno", deptRequestNo);
                    dwSlip.SetItemString(i, "depttype_code", dwMain.GetItemString(1, "depttype_code"));

                    string sqlSlip = new DwHandle(dwSlip).SqlInsertSyntax("WCREQDETAIL", i);
                    ta.Exe(sqlSlip);

                    ///slip
                    amt = amt + dwSlip.GetItemDecimal(i, "amt");
                    prncslip_amt[i - 1] = dwSlip.GetItemDecimal(i, "amt");
                    deptitemtype_code[i - 1] = dwSlip.GetItemString(i, "deptitemtype_code");
                    deptitemtype_desc[i - 1] = dwSlip.GetItemString(i, "deptitemtype_desc");
                    ///end slip
                }
                ///slip
                string user_name = dwMain.GetItemString(1, "entry_id");
                string deptslip_no = dct.NewDocumentNo("WCSLIPDOCNO", WorkYear, ta);
                String sqlinsert = @"insert into wcdeptslip(deptslip_no, deptaccount_no, deptslip_date, depttype_code, deptitemtype_code, recppaytype_code,
                            deptslip_amt, cash_type, entry_id, entry_date,item_status, sliptype_code, branch_id) values('" + deptslip_no + "', '" + deptRequestNo + @"',
                            to_date('" + DateTime.Today.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), '01', 'WFF', 'CSH', " + amt + ", 'CSH', '" + user_name + @"', 
                            to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 'dd/mm/yyyy HH24:mi:ss') , 1, 'WPX', '" + branchId + "')";
                ta.Exe(sqlinsert);

                String sqlinsertdet = "";
                for (int j = 0; j <= 2; j++)
                {
                    sqlinsertdet = @"insert into wcdeptslipdet(deptslip_no, seq_no, depttype_code, deptitemtype_code, prncslip_amt, slip_desc, branch_id)
                    values('" + deptslip_no + "', " + (j + 1) + ", '01', '" + deptitemtype_code[j] + "', '" + prncslip_amt[j] + "', '" + deptitemtype_desc[j] + "', '" + branchId + "')";

                    ta.Exe(sqlinsertdet);
                }
                ///end slip

                ta.Commit();
                ta.Close();
                resu = deptRequestNo;
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return resu;
        }

        public int[] UpdateReqWalfareNewLight(DataTable dtMain, DataTable dtSlip, DataTable dtRelate)
        {
            if (CheckData("w_sheet_wc_walfare_new.pbl", dtMain) != true)
            {
                throw new Exception("กรูณากรอกข้อมูลให้ถูกต้อง");
            }
            int[] resu = new int[2];
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                Sdt sdtMain = new Sdt(dtMain, true);
                if (sdtMain.Next())
                {
                    String branchId = sdtMain.GetString("branch_id");
                    String deptrequest_docno = sdtMain.GetString("deptrequest_docno");

                    String wftype_code = sdtMain.GetString("wftype_code");
                    String member_no = sdtMain.GetString("member_no");
                    String sex = sdtMain.GetString("sex");
                    String prename_code = sdtMain.GetString("prename_code");
                    String deptaccount_name = sdtMain.GetString("deptaccount_name");
                    String deptaccount_sname = sdtMain.GetString("deptaccount_sname");
                    String card_person = sdtMain.GetString("card_person");
                    DateTime wfbirthday_date = sdtMain.GetDate("wfbirthday_date");
                    String contact_address = sdtMain.GetString("contact_address");
                    String ampher_code = sdtMain.GetString("ampher_code");
                    String province_code = sdtMain.GetString("province_code");
                    DateTime apply_date = sdtMain.GetDate("apply_date");
                    String postcode = sdtMain.GetString("postcode");
                    String hometel = sdtMain.GetString("hometel");
                    String remark = sdtMain.GetString("remark");
                    String mate_name = sdtMain.GetString("mate_name");
                    String manage_corpse_name = sdtMain.GetString("manage_corpse_name");
                    decimal foreigner_flag = sdtMain.GetDecimal("foreigner_flag");
                    String updateMain = @"
                    update wcreqdeposit set
                        wftype_code = '{0}',
                        member_no = '{1}',
                        sex = '{2}',
                        prename_code = '{3}',
                        deptaccount_name = '{4}',
                        deptaccount_sname = '{5}',
                        card_person = '{6}',
                        wfbirthday_date = {7},
                        contact_address = '{8}',
                        ampher_code = '{9}',
                        province_code = '{10}',
                        apply_date = {11},
                        postcode = '{12}',
                        hometel = '{13}',
                        remark = '{14}',
                        mate_name = '{15}',
                        manage_corpse_name = '{16}',
                        foreigner_flag = {17}
                    where deptrequest_docno='" + deptrequest_docno + "' and branch_id='" + branchId + "' ";
                    CultureInfo en = new CultureInfo("en-US");
                    string birthDate = "to_date('" + wfbirthday_date.ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";
                    string appDate = "to_date('" + apply_date.ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";
                    updateMain = String.Format(updateMain, wftype_code, member_no, sex, prename_code, deptaccount_name, deptaccount_sname
                        , card_person, birthDate, contact_address, ampher_code, province_code, appDate, postcode, hometel, remark, mate_name
                        , manage_corpse_name, foreigner_flag);
                    ta.Exe(updateMain);

                    String updateSlip = "update wcreqdetail set amt = {0} where deptrequest_docno='" + deptrequest_docno + "' and seq_no = {1} and branch_id='" + branchId + "'";
                    Sdt sdtSlip = new Sdt(dtSlip, true);
                    while (sdtSlip.Next())
                    {
                        String sqlUp = string.Format(updateSlip, sdtSlip.GetDecimal("amt"), sdtSlip.GetDecimal("seq_no"));
                        ta.Exe(sqlUp);
                    }

                    String delete = "delete from WCREQCODEPOSIT where deptrequest_docno='" + deptrequest_docno + "' and branch_id='" + branchId + "' ";
                    ta.Exe(delete);
                    String insert = "insert into WCREQCODEPOSIT(deptrequest_docno, seq_no, \"NAME\", codept_id, codept_addr, branch_id, foreigner_flag)values('{0}', {1}, '{2}', '{3}', '{4}', '{5}', {6})";
                    Sdt sdtOther = new Sdt(dtRelate, true);
                    while (sdtOther.Next())
                    {
                        String sqlIn = string.Format(insert, deptrequest_docno, sdtOther.GetInt32("seq_no"), sdtOther.GetString("name"), sdtOther.GetString("codept_id"), sdtOther.GetString("codept_addr"), branchId, sdtOther.GetDecimal("foreigner_flag"));
                        ta.Exe(sqlIn);
                    }
                    ta.Commit();
                    ta.Close();
                }
                else throw new Exception("dtMain no row");
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return resu;
        }

        public int EditReqWalfareRelate(String pbl, String xmlRelate)
        {
            this.ToPhycalPathPbl(ref pbl);
            int result;
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();

            DataStore dwRelate = new DataStore(pbl, "d_dp_reqcodeposit_reqedit");
            dwRelate.ImportString(xmlRelate, FileSaveAsType.Xml);

            int rlCount = dwRelate.RowCount;
            try
            {
                for (int i = 1; i <= rlCount; i++)
                {
                    //dwRelate.SetItemString(i, "deptrequest_docno", deptRequestNo);
                    //dwRelate.SetItemString(i, "branch_id", branch_id);
                    //dwRelate.SetItemDecimal(i, "seq_no", i);
                    String sqlRelate = new DwHandle(dwRelate).SqlUpdateSyntax("WCREQCODEPOSIT", i);
                    ta.Exe(sqlRelate);
                }
                ta.Commit();
                ta.Close();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
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
            return result;
        }

        public int DeleteReqWalfareRelate(String docregno, String seq_no)
        {
            int result = 0;

            Sta ta = new Sta(sec.ConnectionString);
            try
            {
                String sql = "Delete From wcreqcodeposit Where deptrequest_docno='" + docregno + "' and seq_no='" + seq_no + "'";
                ta.Exe(sql);
                ta.Close();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }

            return result;
        }

        public int NewUserAccount(UserAccount userAcc)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                int ii = 0;
                userAcc.Password = new SecurityEngine.Encryption().EncryptAscii(userAcc.Password);
                String sql1 = "insert into amsecusers(user_name, full_name, description, password, coopbranch_id, user_level, user_type)values('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6})";
                sql1 = string.Format(sql1, userAcc.UserName, userAcc.FullName, userAcc.Description, userAcc.Password, userAcc.CoopBranchId, 3, 1);
                String sql2 = "insert into amsecuseapps(user_name, application)values('" + userAcc.UserName + "', '" + application + "')";
                ii = ta.Exe(sql1);
                ii = ta.Exe(sql2);

                PermissAll(userAcc, ta);

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
                if (ex.Message.IndexOf("ORA-00001") >= 0)
                {
                    throw new Exception("ไม่สามารถบันทึกข้อมูลได้เนื่องจากมี Username: " + userAcc.UserName + " ซ้ำแล้ว");
                }
                throw ex;
            }
            return 1;
        }

        public int PermissUsers(String pbl, String xmlDwMain)
        {
            this.ToPhycalPathPbl(ref pbl);
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                DataStore dwMain = new DataStore(pbl, "d_wc_permission_user");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                for (int i = 1; i <= dwMain.RowCount; i++)
                {
                    String userName = dwMain.GetItemString(i, "user_name");
                    decimal uPermiss = dwMain.GetItemDecimal(i, "user_permiss");
                    decimal check_flag = dwMain.GetItemDecimal(i, "check_flag");
                    decimal paid_flag = dwMain.GetItemDecimal(i, "paid_flag");
                    decimal editreqregis_flag = dwMain.GetItemDecimal(i, "editreqregis_flag");
                    decimal memb_flag = dwMain.GetItemDecimal(i, "memb_flag");
                    decimal resign_flag = dwMain.GetItemDecimal(i, "resign_flag");
                    decimal user_type = dwMain.GetItemDecimal(i, "user_type");
                    UserAccount uAcc = new UserAccount();
                    uAcc.UserName = userName;
                    uAcc.AdminOption = dwMain.GetItemDecimal(i, "user_type") == 1;
                    //ta.Exe("delete from amsecpermiss where user_name='" + userName + "'");
                    if (uPermiss == 1)
                    {
                        PermissAll(uAcc, ta);
                    }
                    if (uAcc.AdminOption)
                    {
                        ta.Exe("update amsecusers set user_type=1 , user_level=3 where user_name='" + uAcc.UserName + "'");                        
                    }
                    else
                    {
                        ta.Exe("update amsecusers set user_type=" + user_type + " , user_level=3 where user_name='" + uAcc.UserName + "'");
                    }
                    ta.Exe("update amsecpermiss set check_flag = " + check_flag + " where user_name ='" + uAcc.UserName + "' and window_id = 'WC00000010'");
                    ta.Exe("update amsecpermiss set check_flag = " + editreqregis_flag + " where user_name ='" + uAcc.UserName + "' and window_id = 'WC-A000010'");
                    ta.Exe("update amsecpermiss set check_flag = " + memb_flag + " where user_name ='" + uAcc.UserName + "' and window_id = 'WC-A000060'");
                    ta.Exe("update amsecpermiss set check_flag = " + resign_flag + " where user_name ='" + uAcc.UserName + "' and window_id = 'WC-A000070'");
                    ta.Exe("update amsecpermiss set check_flag = " + paid_flag + " where user_name ='" + uAcc.UserName + "' and window_id = 'WC-A000090'");

                }
                ta.Commit();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return 1;
        }

        public bool SavePaidAdd(String pbl, String xmlDwList)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwList = new DataStore(pbl, "d_wc_paid_group_add");
                dwList.ImportString(xmlDwList, FileSaveAsType.Xml);
                String sqlList, sqldel;
                string period, wfmember_no;
                for (int i = 1; i <= dwList.RowCount; i++)
                {
                    period = dwList.GetItemString(i, "recv_period");
                    wfmember_no = dwList.GetItemString(i, "wfmember_no");
                    sqldel = "delete from wcrecievemonth where wfmember_no = '" + wfmember_no + "' and recv_period = '" + period + "' and wcitemtype_code = 'FEE'";
                    ta.Exe(sqldel);
                }
                for (int i = 1; i <= dwList.RowCount; i++)
                {
                    sqlList = new DwHandle(dwList).SqlInsertSyntax("wcrecievemonth", i);
                    ta.Exe(sqlList);
                }
                ta.Commit();
                ta.Close();
                return true;
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
        }

        public string SavePaid(String pbl, String xmlDwMain, String xmlDwSlip, String reqDocNo)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            String slipNo = "";
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wf_paid_slip_master");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                DataStore dwSlip = new DataStore(pbl, "d_wf_paid_slip_item");
                dwSlip.ImportString(xmlDwSlip, FileSaveAsType.Xml);
                DataStore dwStm = new DataStore(pbl, "d_wf_paid_statement");
                int year = (dwMain.GetItemDateTime(1, "operate_date").Year) + 543;
                slipNo = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", year, ta);
                dwMain.SetItemString(1, "deptslip_no", slipNo);
                dwMain.SetItemString(1, "deptitemtype_code", dwSlip.GetItemString(1, "deptitemtype_code"));
                dwMain.SetItemString(1, "depttype_code", "01");
                String sqlInsertMaster = new DwHandle(dwMain).SqlInsertSyntax("WCDEPTSLIP", 1);
                String deptAccountNo = dwMain.GetItemString(1, "deptaccount_no");
                String branchId = dwMain.GetItemString(1, "branch_id");
                int ii1 = ta.Exe(sqlInsertMaster);
                int ii2 = 0;
                int ii3 = 0;
                int seq_stm = 0;
                decimal principal = 0;
                String sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptAccountNo + "' and branch_id='" + branchId + "'";
                Sdt dt = ta.Query(sqlSeqStm);
                if (dt.Next())
                {
                    seq_stm = dt.GetInt32(0);
                }
                else
                {
                    seq_stm = 0;
                }
                String sqlPrncBal = "select prncbal, withdrawable_amt from wcdeptmaster where deptaccount_no='" + deptAccountNo + "' and branch_id='" + branchId + "'";
                Sdt dtPrnc = ta.Query(sqlPrncBal);
                if (dtPrnc.Next())
                {
                    principal = dtPrnc.GetDecimal(0);
                }
                else
                {
                    principal = 0;
                }
                for (int i = 1; i <= dwSlip.RowCount; i++)
                {
                    dwSlip.SetItemString(i, "deptslip_no", slipNo);
                    dwSlip.SetItemDecimal(i, "seq_no", i);
                    String sqlSlip = new DwHandle(dwSlip).SqlInsertSyntax("WCDEPTSLIPDET", i);
                    ii2 = ta.Exe(sqlSlip);
                    seq_stm++;
                    dwStm.InsertRow(0);
                    dwStm.SetItemString(i, "deptaccount_no", deptAccountNo);
                    dwStm.SetItemDecimal(i, "seq_no", seq_stm);
                    dwStm.SetItemString(i, "deptitemtype_code", dwSlip.GetItemString(i, "deptitemtype_code"));
                    dwStm.SetItemString(i, "ref_docno", slipNo);
                    dwStm.SetItemDecimal(i, "deptitem_amt", dwSlip.GetItemDecimal(i, "prncslip_amt"));
                    dwStm.SetItemString(i, "entry_id", dwMain.GetItemString(1, "entry_id"));
                    dwStm.SetItemDateTime(i, "entry_date", dwMain.GetItemDateTime(1, "operate_date"));
                    dwStm.SetItemDateTime(i, "operate_date", dwMain.GetItemDateTime(1, "operate_date"));
                    dwStm.SetItemDecimal(i, "sign_flag", dwSlip.GetItemDecimal(1, "sign_flag"));
                    dwStm.SetItemString(i, "branch_id", branchId);
                    String deptitemTypeCode = dwSlip.GetItemString(i, "deptitemtype_code");
                    if (deptitemTypeCode == "WPF")
                    {
                        principal += dwSlip.GetItemDecimal(i, "prncslip_amt");
                    }
                    else if (deptitemTypeCode == "WPD")
                    {
                        decimal withdrawableAmt = dtPrnc.GetDecimal(1);
                        decimal slipAmt = dwSlip.GetItemDecimal(i, "prncslip_amt");
                        if (slipAmt > withdrawableAmt)
                        {
                            throw new Exception("ไม่สามารถใส่จำนวนเกิน " + withdrawableAmt.ToString("#,##0.00") + ".-");
                        }
                        decimal total = withdrawableAmt - slipAmt;
                        String sql = "update wcdeptmaster set withdrawable_amt=" + total + " where deptaccount_no='" + deptAccountNo + "' and branch_id='" + branchId + "'";
                        int ii = ta.Exe(sql);
                    }
                    dwStm.SetItemDecimal(i, "prncbal", principal);
                    ii3 = ta.Exe(new DwHandle(dwStm).SqlInsertSyntax("WCDEPTSTATEMENT", i));
                }
                ta.Exe("update wcdeptmaster set effective_date = to_date('01/01/" + Convert.ToString(year) + "', 'dd/mm/yyyy') where deptaccount_no='" + deptAccountNo + "' and branch_id='" + branchId + "'");
                ta.Exe("update wcdeptmaster set prncbal=" + principal + " where deptaccount_no='" + deptAccountNo + "' and branch_id='" + branchId + "'");
                if (reqDocNo != "")
                {
                    int ii4 = ta.Exe("update wcreqdeposit set pay_status=1 where deptrequest_docno='" + reqDocNo + "'");
                    int ii5 = ta.Exe("update wcreqdetail set status_pay=1 where deptrequest_docno='" + reqDocNo + "'");
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return slipNo;
        }

        public int SvAuditEdit(string pbl, String Mxml, String Cxml, string UserNanme, string Branch_id, string Cs_type, string dwName, string[] colid)
        {
            if (CheckDataEdit(pbl, Cxml, dwName) != true)
            {
                throw new Exception("กรูณากรอกข้อมูลให้ถูกต้อง");
            }
            try
            {
                this.ToPhycalPathPbl(ref pbl);

                string Mvalue;
                string Cvalue;
                int TauditStatus = 0;

                ///XmlMain
                DataStore DtMain = new DataStore(pbl, dwName);
                DtMain.ImportString(Mxml, FileSaveAsType.Xml);

                String[] columnName = new string[DtMain.ColumnCount];
                String[] columnType = new string[DtMain.ColumnCount];

                ///XmlCompare
                DataStore DtCompare = new DataStore(pbl, dwName);
                DtCompare.ImportString(Cxml, FileSaveAsType.Xml);

                int Colcount = DtCompare.ColumnCount;

                Sta ta = new Sta(sec.ConnectionString);
                ta.Transection();
                try
                {
                    for (int i = 0; i < Colcount; i++)
                    {
                        ///XmlMain
                        columnName[i] = DtMain.Describe("#" + (i + 1) + ".Name");
                        columnType[i] = DtMain.Describe(columnName[i] + ".ColType").ToLower();

                        if (columnType[i].IndexOf("(") > 0)
                        {
                            columnType[i] = columnType[i].Substring(0, columnType[i].IndexOf("("));
                        }

                        Mvalue = getDataFromDW(DtMain, columnName[i], columnType[i], 1);
                        Mvalue = Mvalue.Trim();

                        Cvalue = getDataFromDW(DtCompare, columnName[i], columnType[i], 1);
                        Cvalue = Cvalue.Trim();

                        if (Mvalue != Cvalue && columnName[i] != "entry_id")
                        {
                            string member_no, wfaccount_name, card_person;
                            try
                            {
                                member_no = DtMain.GetItemString(1, "member_no");
                            }
                            catch
                            {
                                member_no = "null";
                            }
                            try
                            {

                                wfaccount_name = DtMain.GetItemString(1, "wfaccount_name");
                            }
                            catch
                            {
                                wfaccount_name = "null";
                            }
                            try
                            {
                                card_person = DtMain.GetItemString(1, "card_person");
                            }
                            catch
                            {
                                card_person = "null";
                            }


                            int result = saveAudit(ta, columnName[i], member_no, UserNanme, wfaccount_name, card_person, Branch_id, Cs_type, Mvalue, Cvalue, TauditStatus, colid);
                            TauditStatus = 1;
                        }
                    }
                    ta.Commit();
                    ta.Close();
                }
                catch (Exception ex)
                {
                    ta.RollBack();
                    ta.Close();
                    throw ex;
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getDataFromDW(IDataStore Dts, string colName, string colType, int row)
        {
            String result = "null";

            if (colType == "char")
            {
                try
                {
                    result = Dts.GetItemString(row, colName);
                }
                catch { }
            }
            else if (colType == "decimal" || colType == "long" || colType == "number" || colType == "float" || colType == "double")
            {
                try
                {
                    result = Dts.GetItemDecimal(row, colName).ToString();
                }
                catch { }
            }
            else if (colType == "datetime")
            {
                //dataWindow.SetItemDateTime(r + 1, cName[c], DateTime.ParseExact(dt.Rows[r][c].ToString(), "yyyy-MM-dd HH:mm:ss", WebUtil.EN));
                try
                {
                    CultureInfo th = new CultureInfo("th-TH");
                    //result = "to_date('" + Dts.GetItemDateTime(row, colName).ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";
                    result = Dts.GetItemDateTime(row, colName).ToString("dd/MM/yyyy", th);
                }
                catch { }
            }
            return result;
        }

        public int saveAudit(Sta ta, string ColName, string member_no, string UserNanme, string name, string card_person, string Branch_id, string Cs_type, string Oldvalue, string Newvalue, int TauditStatus, string[] colid)
        {
            CultureInfo en = new CultureInfo("en-US");
            String today = "to_date('" + DateTime.Now.ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";


            if (TauditStatus < 1)
            {
                DocumentControl dct = new DocumentControl();
                auditDocno = dct.NewDocumentNo("AUDITMEMBER", 2554, ta);
                String Msql = "INSERT INTO mbaudit (docno, member_no, approve_id, approve_date, \"NAME\", card_person, branch_id, cs_type) ";
                Msql = Msql + "VALUES('" + auditDocno + "','" + member_no + "','" + UserNanme + "'," + today + ",'" + name + "','" + card_person + "','" + Branch_id + "','" + Cs_type + "')";

                ta.Exe(Msql);
            }

            String sql = "Select col_id From cmauditcolumn Where engcol_name ='" + ColName.ToUpper() + "' and col_id between '" + colid[0] + "' and '" + colid[1] + "'";
            Sdt dt = ta.Query(sql);
            if (dt.Next())
            {
                String col_id = dt.GetString("col_id");
                String Hsql = "INSERT INTO mbaudithistory (docno, col_id, old_value, new_value) VALUES('";
                Hsql = Hsql + auditDocno + "','" + col_id + "','" + Oldvalue + "','" + Newvalue + "')";

                ta.Exe(Hsql);
            }

            return 1;
        }

        public string SetCheckDuplicateDT(String pbl, DataTable dtMain, string[] listDB, string cs_type, int user_type)
        {
            this.ToPhycalPathPbl(ref pbl);

            DataStore dwMain = new DataStore(pbl, "d_dp_reqdepoist_main");
            //dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
            DwHandle.ImportData(dtMain, dwMain);

            return CheckDuplicate(dwMain, listDB, cs_type, user_type);
//            Sta ta = new Sta(sec.ConnectionString);
//            ta.Transection();
//            try
//            {
//                string member_no = dwMain.GetItemString(1, "member_no");
//                string prename_code = dwMain.GetItemString(1, "prename_code");
//                string deptaccount_name = dwMain.GetItemString(1, "deptaccount_name");
//                string deptaccount_sname = dwMain.GetItemString(1, "deptaccount_sname");
//                //DateTime wfbirthday_date = dwMain.GetItemDateTime(1, "wfbirthday_date");
//                string card_person = dwMain.GetItemString(1, "card_person");
//                string branch_id = dwMain.GetItemString(1, "branch_id");


//                String sql_full = "select * from wcreqdeposit where member_no = '" + member_no + "' and prename_code = '" + prename_code + @"' and deptaccount_name = '"
//                    + deptaccount_name + "' and deptaccount_sname = '" + deptaccount_sname + "' and card_person = '" + card_person + "' and branch_id = '" + branch_id + @"'
//                    and approve_status <> -9";

//                Sdt dtchk_full = ta.Query(sql_full);
//                if (dtchk_full.Next())
//                {
//                    ta.Close();
//                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
//                }

//                String sql_notMbNo = "select * from wcreqdeposit where prename_code = '" + prename_code + @"' and deptaccount_name = '"
//                    + deptaccount_name + "' and deptaccount_sname = '" + deptaccount_sname + "' and card_person = '" + card_person + "' and branch_id = '" + branch_id + @"'
//                    and approve_status <> -9";

//                Sdt dtchk_notMbNo = ta.Query(sql_notMbNo);
//                if (dtchk_notMbNo.Next())
//                {
//                    ta.Close();
//                    throw new Exception("ไม่สามารถทำรายการได้ ชื่อ " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
//                }

//                String sql_notCardPS = "select * from wcreqdeposit where member_no = '" + member_no + "' and prename_code = '" + prename_code + @"' and deptaccount_name = '"
//                    + deptaccount_name + "' and deptaccount_sname = '" + deptaccount_sname + "' and branch_id = '" + branch_id + @"' and approve_status <> -9";

//                Sdt dtchk_notCardPS = ta.Query(sql_notCardPS);
//                if (dtchk_notCardPS.Next())
//                {
//                    ta.Close();
//                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
//                }

//                String sql_MbNo = "select * from wcreqdeposit where member_no = '" + member_no + "' and branch_id = '" + branch_id + @"' and approve_status <> -9";

//                Sdt dtchk_MbNo = ta.Query(sql_MbNo);
//                if (dtchk_MbNo.Next())
//                {
//                    ta.Close();
//                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " มีอยู่แล้ว กรุณาตรวจสอบ");
//                }

//                String sql_CardPS = "select * from wcreqdeposit where card_person = '" + card_person + "' and branch_id = '" + branch_id + @"' and approve_status <> -9";

//                Sdt dtchk_CardPS = ta.Query(sql_CardPS);
//                if (dtchk_CardPS.Next())
//                {
//                    ta.Close();
//                    throw new Exception("ไม่สามารถทำรายการได้ เลขประจำตัวประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
//                }

//                ta.Close();
//            }
//            catch (Exception ex)
//            {
//                ta.RollBack();
//                ta.Close();
//                throw ex;
//            }

//            return "";
        }

        public Boolean CheckData(String pbl, DataTable dtMain)
        {
            this.ToPhycalPathPbl(ref pbl);

            DataStore dwMain = new DataStore(pbl, "d_dp_reqdepoist_main");
            //dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
            DwHandle.ImportData(dtMain, dwMain);

            try
            {
                string member_no = dwMain.GetItemString(1, "member_no");
                if (string.IsNullOrEmpty(member_no)) throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            catch
            {
                throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            try
            {
                string deptaccount_name = dwMain.GetItemString(1, "deptaccount_name");
                if (string.IsNullOrEmpty(deptaccount_name)) throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            try
            {
                string deptaccount_sname = dwMain.GetItemString(1, "deptaccount_sname");
                if (string.IsNullOrEmpty(deptaccount_sname)) throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            try
            {
                DateTime wfbirthday_date = dwMain.GetItemDateTime(1, "wfbirthday_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันเกิดให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }
            try
            {
                string card_person = dwMain.GetItemString(1, "card_person");
                if (dwMain.GetItemDecimal(1, "foreigner_flag") == 0)
                {
                    if (VerifyPeopleID(card_person) == false)
                    {
                        throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
                    }
                }
            }
            catch
            {
                throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
            }
            try
            {
                DateTime apply_date = dwMain.GetItemDateTime(1, "apply_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันสมัครให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }

            return true;
        }

        public Boolean CheckDataEdit(String pbl, String xmlDwMain, string dwName)
        {
            this.ToPhycalPathPbl(ref pbl);

            DataStore dwMain = new DataStore(pbl, dwName);
            dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
            //DwHandle.ImportData(dtMain, dwMain);

            try
            {
                string member_no = dwMain.GetItemString(1, "member_no");
                if (string.IsNullOrEmpty(member_no)) throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            catch
            {
                throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            try
            {
                string deptaccount_name = dwMain.GetItemString(1, "deptaccount_name");
                if (string.IsNullOrEmpty(deptaccount_name)) throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            try
            {
                string deptaccount_sname = dwMain.GetItemString(1, "deptaccount_sname");
                if (string.IsNullOrEmpty(deptaccount_sname)) throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            try
            {
                DateTime wfbirthday_date = dwMain.GetItemDateTime(1, "wfbirthday_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันเกิดให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }
            try
            {
                string card_person = dwMain.GetItemString(1, "card_person");
                if (dwMain.GetItemDecimal(1, "foreigner_flag") == 0)
                {
                    if (VerifyPeopleID(card_person) == false)
                    {
                        throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
                    }
                }
            }
            catch
            {
                throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
            }
            try
            {
                DateTime apply_date = dwMain.GetItemDateTime(1, "apply_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันสมัครให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }

            return true;
        }

        private Boolean VerifyPeopleID(String PID)
        {
            //ตรวจสอบว่าทุก ๆ ตัวอักษรเป็นตัวเลข
            PID = PID.Trim();
            if (PID.ToCharArray().All(c => char.IsNumber(c)) == false)
            {
                return false;
            }
            //ตรวจสอบว่าข้อมูลมีทั้งหมด 13 ตัวอักษร
            if (PID.Trim().Length != 13)
            {
                return false;
            }
            //int sumValue = 0;

            //for (int i = 0; i < PID.Length - 1; i++)
            //{
            //    sumValue += int.Parse(PID[i].ToString()) * (13 - i);
            //}
            //int v = (11 - (sumValue % 11)) % 10;
            //String chk = PID[12].ToString();

            //return PID[12].ToString() == v.ToString();
            return true;
        }

        public Boolean GenSlip(DateTime workDate, string user_name)
        {            
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();

            String sqlreq, sqldetail, sqlinsert, sqlinsertdet;
            sqlreq = "select * from wcreqdeposit where deptopen_date = to_date('01012012', 'ddmmyyyy') order by deptrequest_docno";
            Sdt dtreq, dtdetail ;
            dtreq  = ta.Query(sqlreq);

            int i = 0, amt = 0;
            string deptslip_no;
            String [] deptrequest_docno = new String[dtreq.GetRowCount()];
            String[] deptitemtype_code = new String[] { "FEE", "WFY", "WPF" };
            String [] slip_desc = new String[] {"ค่าธรรมเนียมสมัครใหม่", "ค่าธรรมเนียมรายปี", "เงินสงเคราะห์ศพล่วงหน้า"};

            int[] prncslip_amt = new int[3];

            CultureInfo th = new CultureInfo("th-TH");
            int WorkYear = Convert.ToInt32(workDate.ToString("yyyy", th));
            DocumentControl dct = new DocumentControl();

            while (dtreq.Next())
            {
                deptrequest_docno[i] =  dtreq.GetString("deptrequest_docno");
                
                deptslip_no = dct.NewDocumentNo("WCSLIPDOCNO", WorkYear, ta);
                sqldetail = "select amt, deptitemtype_code from wcreqdetail where deptrequest_docno = '" + deptrequest_docno[i] + "'";
                dtdetail = ta.Query(sqldetail);
                amt = 0;
                while(dtdetail.Next()){
                    amt = dtdetail.GetInt32("amt") + amt;
                    switch (dtdetail.GetString("deptitemtype_code"))
                    {
                        case "FEE":
                            prncslip_amt[0] = dtdetail.GetInt32("amt");
                            break;
                        case "WFY":
                            prncslip_amt[1] = dtdetail.GetInt32("amt");
                            break;
                        case "WPF":
                            prncslip_amt[2] = dtdetail.GetInt32("amt");
                            break;
                    }
                }
                sqlinsert = @"insert into wcdeptslip(deptslip_no, deptaccount_no, deptslip_date, depttype_code, deptitemtype_code, recppaytype_code,
                            deptslip_amt, cash_type, entry_id, entry_date,item_status, sliptype_code, branch_id) values('" + deptslip_no + "', '" + deptrequest_docno[i] + @"',
                            to_date('" + DateTime.Today.ToString("dd/MM/yyyy") +"', 'dd/mm/yyyy'), '01', 'WFF', 'CSH', " + amt + ", 'CSH', '" + user_name + @"', 
                            to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")+ "', 'dd/mm/yyyy HH24:mi:ss') , 1, 'WPX', '" + dtreq.GetString("branch_id") + "')";
                try
                {
                    ta.Exe(sqlinsert);                   
                }
                catch (Exception ex)
                {
                    ta.Close();
                    throw ex;
                }
                for (int j = 0; j <= 2; j++)
                {
                    sqlinsertdet = @"insert into wcdeptslipdet(deptslip_no, seq_no, depttype_code, deptitemtype_code, prncslip_amt, slip_desc, branch_id)
                    values('" + deptslip_no + "', " + (j+1) + ", '01', '" + deptitemtype_code[j] + "', '" + prncslip_amt[j] + "', '" + slip_desc[j] + "', '" + dtreq.GetString("branch_id") + "')";
                    try
                    {
                        ta.Exe(sqlinsertdet);
                    }
                    catch (Exception ex)
                    {
                        ta.Close();
                        throw ex;
                    }
                }
            }
            try
            {
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean UpdateReqDocno()
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();

            String sqlSelect_dup, sqlSelect_master, sqlSelect_req, update_reg;
            Sdt dtSelect_dup, dtMaster, dtReq;
            sqlSelect_dup = @"select member_no, branch_id , count(member_no) as count_member_no
                                        from wcreqdeposit
                                        where branch_id = branch_id
                                        and deptaccount_no is null
                                        and deptopen_date <> to_date('01012012', 'ddmmyyyy')
                                        group by member_no, branch_id
                                        having count(member_no)>1
                                        ";

            dtSelect_dup = ta.Query(sqlSelect_dup);
            string member_no, branch_id;
            int count_member_no;
            while (dtSelect_dup.Next())
            {
                member_no = dtSelect_dup.GetString("member_no");
                branch_id = dtSelect_dup.GetString("branch_id");
                count_member_no = dtSelect_dup.GetInt32("count_member_no");

                sqlSelect_master = @"select deptaccount_no from wcdeptmaster
                                    where member_no = '" + member_no + "' and branch_id = '" + branch_id + @"'
                                    order by deptaccount_no";
                sqlSelect_req = @"select deptrequest_docno from wcreqdeposit
                                    where member_no = '" + member_no + "' and branch_id = '" + branch_id + @"'
                                    order by deptrequest_docno";
                dtMaster = ta.Query(sqlSelect_master);
                dtReq = ta.Query(sqlSelect_req);
                while (dtMaster.Next() && dtReq.Next())
                {
                    update_reg = "update wcreqdeposit set deptaccount_no = '" + dtMaster.GetString("deptaccount_no") + @"'
                                where deptrequest_docno = '" + dtReq.GetString("deptrequest_docno") + "'";

                    ta.Exe(update_reg);
                }
            }
            try
            {
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }


            return true;
        }

        public string SetCheckDuplicate(String pbl, String xmlDwMain, string[] listDB, string cs_type, int user_type)
        {
            this.ToPhycalPathPbl(ref pbl);
            
            DataStore dwMain = new DataStore(pbl, "d_dp_reqdepoist_main");
            dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
            //DwHandle.ImportData(dtMain, dwMain);
            return CheckDuplicate(dwMain, listDB, cs_type, user_type);
        }

        private string CheckDuplicate(IDataStore dwMain, string[] listDB, string cs_type, int user_type)
        {
            bool otrher_cremation_flag = false;
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                string member_no = dwMain.GetItemString(1, "member_no").Trim();
                string prename_code = dwMain.GetItemString(1, "prename_code");
                string deptaccount_name = dwMain.GetItemString(1, "deptaccount_name");
                string deptaccount_sname = dwMain.GetItemString(1, "deptaccount_sname");
                //DateTime wfbirthday_date = dwMain.GetItemDateTime(1, "wfbirthday_date");
                string card_person = dwMain.GetItemString(1, "card_person").Trim();
                string branch_id = dwMain.GetItemString(1, "branch_id").Trim();
                string wftype_code = dwMain.GetItemString(1, "wftype_code");

                ///check ว่าเป็นสหกรณ์ในกลุุ่มอาชีพครูหรือไม่
                //String chkftsc = @"select coopbranch_id from cmucfcoopbranch where coopbranch_id = '" + branch_id + "' and cs_type = '1'";
                //Sdt dtchkftsc = ta.Query(chkftsc);
                //bool resuchkftsc = false;
                //if (dtchkftsc.Next())
                //{
                //    resuchkftsc = true;
                //}

                ///check ว่าเป็นสมาคม ชสอ หรือไม่ เพื่อทำการตรวจสอบว่าสมาชิกได้สมัครสมาคมของกลุ่มอาชีพหรือยัง (ยกเว้นสมาคมครูไทยจะไม่ตรวจเนื่องจากข้อมูลยังไม่สมบูรณ์)
                if (cs_type == "8" && wftype_code.Trim() == "01")
                {
                    Sta[] taDBFist = new Sta[listDB.Length];
                    try
                    {
                        bool ishave = false;
                        for (int i = 0; i < listDB.Length; i++)
                        {
                            taDBFist[i] = new Sta(new SecurityEngine.Decryption().DecryptStrBase64(listDB[i]));

//                             String SQLCardPS = "select * from wcreqdeposit where card_person = '" + card_person + @"' and approve_status <> -9 and wftype_code = '01' or
//                                            (card_person = '" + card_person + @"' and approve_status <> -9 and wftype_code in('02','03', '04', '05') and branch_id 
//                                            in(select coopbranch_id from cmucfcoopbranch where cs_type = '1'))  or (card_person = '" + card_person +
//                                            @"' and approve_status <> -9 and wftype_code in('01', '02', '03', '04', '05', '06') and branch_id 
//                                            in(select coopbranch_id from cmucfcoopbranch where cs_type = '3'))  or (card_person = '" + card_person +
//                                            @"' and approve_status <> -9 and wftype_code in('01', '03') and branch_id 
//                                            in(select coopbranch_id from cmucfcoopbranch where cs_type = '4')) or (card_person = '" + card_person +
//                                            @"' and approve_status <> -9 and wftype_code in('01', '02') and branch_id 
//                                            in(select coopbranch_id from cmucfcoopbranch where cs_type = '5'))";
                           
                            //String SQLCardPS = "select r.deptrequest_docno from wcreqdeposit r, wcmembertype b where r.card_person = '" + card_person
                            //    + @"' and r.approve_status <> -9 and r.wftype_code = b.wftype_code and b.registfsct_status = 1 group by r.deptrequest_docno";
                            //Sdt dtChkDBFistCardPS = taDBFist[i].Query(SQLCardPS);

                            String SQLCardPS = "select r.deptrequest_docno from wcreqdeposit r, wcmembertype b where r.card_person = '" + card_person
                                + @"' and r.approve_status <> -9 and r.wftype_code = b.wftype_code and b.registfsct_status = 1 group by r.deptrequest_docno";
                            Sdt dtChkDBFistCardPS = taDBFist[i].Query(SQLCardPS);
                            if (dtChkDBFistCardPS.Next())
                            {
                                String SQLdie_chk = "select deptaccount_no from wcdeptmaster where card_person = '" + card_person +"' and deptclose_status = 1 and resigncause_code = '02' ";
                                Sdt dtSQLdie_chk = taDBFist[i].Query(SQLdie_chk);
                                if (dtSQLdie_chk.Next())
                                {
                                    ta.Close();
                                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีสถานะเป็น เสียชีวิต ในกองวิชาชีพ");
                                }
                                else
                                {
                                    ishave = true;
                                    break;
                                }
                            }
                            taDBFist[i].Close();
                        }
                        if (!ishave)
                        {
                            for (int i = 0; i < listDB.Length; i++)
                            {
                                try
                                {
                                    taDBFist[i].Close();
                                }
                                catch { }
                            }

                            if (user_type == 1)
                            {
                                otrher_cremation_flag = true;
                            }
                            else
                            {
                                ta.Close();
                                throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ไม่ได้สมัครสมาชิกฯ ในกองวิชาชีพ");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        for (int i = 0; i < listDB.Length; i++)
                        {
                            try
                            {
                                taDBFist[i].Close();
                            }
                            catch { }
                        }
                        throw new Exception( ex.Message);
                    }

                }

                String sql_full = "select * from wcreqdeposit where trim(card_person) = '" + card_person + "' and approve_status > 0";

                Sdt dtchk_full = ta.Query(sql_full);
                if (dtchk_full.Next())
                {
                    if (dtchk_full.GetDecimal("approve_status") == 8)
                    {
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ ชื่อ "  + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีใบสมัคร ที่รอการอนุมัติอยู่ กรุณาตรวจสอบ");
                    }
                    string resuCode = checkDupMaster(dtchk_full.GetString("deptaccount_no"));
                    if (checkDupMaster(dtchk_full.GetString("deptaccount_no")) != "")
                    {
                        string resuStr = checkResignCauseCode(resuCode);
                        if (resuStr != "no")
                        {
                            ta.Close();
                            if (cs_type == "8" && resuCode.Trim() == "04")
                            {
                                if (user_type == 1)
                                {
                                    return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว ต้องการบันทึกหรือไม่";
                                }
                                else
                                {
                                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว กรุณาติดต่อ สส.ชสอ.");
                                }
                            }
                            else
                            {
                                //throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                                return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว คุณต้องการทำรายการต่อหรือไม่";
                            }                     
                        }
                    }
                    ta.Close();
                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                }

                String sql_notMbNo = "select * from wcreqdeposit where prename_code = '" + prename_code + @"' and deptaccount_name = '"
                    + deptaccount_name + "' and deptaccount_sname = '" + deptaccount_sname + "' and card_person = '" + card_person + @"' and approve_status > 0";

                Sdt dtchk_notMbNo = ta.Query(sql_notMbNo);
                if (dtchk_notMbNo.Next())
                {
                    if (dtchk_notMbNo.GetDecimal("approve_status") == 8)
                    {
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ ชื่อ " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                    }
                    string resuCode = checkDupMaster(dtchk_notMbNo.GetString("deptaccount_no"));
                    if (checkDupMaster(dtchk_notMbNo.GetString("deptaccount_no")) != "")
                    {
                        string resuStr = checkResignCauseCode(resuCode);
                        if (resuStr != "no")
                        {
                            ta.Close();
                            if (cs_type == "8" && resuCode.Trim() == "04")
                            {
                                if (user_type == 1)
                                {
                                    return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว ต้องการบันทึกหรือไม่";
                                }
                                else
                                {
                                    throw new Exception("เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว กรุณาติดต่อ สส.ชสอ.");
                                }
                            }
                            else
                            {
                                //throw new Exception("ไม่สามารถทำรายการได้ ชื่อ " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                                return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว คุณต้องการทำรายการต่อหรือไม่";
                            }
                        }
                    }
                    ta.Close();
                    throw new Exception("ไม่สามารถทำรายการได้ ชื่อ " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                }

                String sql_notCardPS = "select * from wcreqdeposit where member_no = '" + member_no + "' and prename_code = '" + prename_code + @"' and deptaccount_name = '"
                    + deptaccount_name + "' and deptaccount_sname = '" + deptaccount_sname + "' and branch_id = '" + branch_id + @"' and approve_status > 0";

                Sdt dtchk_notCardPS = ta.Query(sql_notCardPS);
                if (dtchk_notCardPS.Next())
                {
                    if (dtchk_notCardPS.GetDecimal("approve_status") == 8)
                    {
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
                    }
                    string resuCode = checkDupMaster(dtchk_notCardPS.GetString("deptaccount_no"));
                    if (checkDupMaster(dtchk_notCardPS.GetString("deptaccount_no")) != "")
                    {
                        string resuStr = checkResignCauseCode(resuCode);
                        if (resuStr != "no")
                        {
                            ta.Close();
                            if (cs_type == "8" && resuCode.Trim() == "04")
                            {
                                if (user_type == 1)
                                {
                                    return "ลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว ต้องการบันทึกหรือไม่";
                                }
                                else
                                {
                                    throw new Exception("เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว กรุุุณาติดต่อ สส.ชสอ.");
                                }
                            }
                            else
                            {
                                //throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
                                return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว คุณต้องการทำรายการต่อหรือไม่";
                            }
                        }
                    }
                    ta.Close();
                    throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
                }

                if ((!(cs_type == "2" && (wftype_code == "03" || wftype_code == "04" || wftype_code == "05" || wftype_code == "06"))) && !(cs_type == "4" && wftype_code == "04") && !(cs_type == "5" && wftype_code == "03") && !(cs_type == "1" && wftype_code == "06" || wftype_code == "08" || wftype_code == "10" || wftype_code == "13") && !(cs_type == "6" && (wftype_code == "02" || wftype_code == "03" || wftype_code == "04" || wftype_code == "05"))) //tar  member_no ซ้ำได้
                {
                    String sql_MbNo = "select * from wcreqdeposit where member_no = '" + member_no + "' and branch_id = '" + branch_id + @"' and approve_status > 0 and branch_id not in('2222', '8000')";

                    Sdt dtchk_MbNo = ta.Query(sql_MbNo);
                    if (dtchk_MbNo.Next())
                    {
                        if (dtchk_MbNo.GetDecimal("approve_status") == 8)
                        {
                            ta.Close();
                            throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " มีอยู่แล้ว กรุณาตรวจสอบ");
                        }
                        string resuCode = checkDupMaster(dtchk_MbNo.GetString("deptaccount_no"));
                        if (checkDupMaster(dtchk_MbNo.GetString("deptaccount_no")) != "")
                        {
                            string resuStr = checkResignCauseCode(resuCode);
                            if (resuStr != "no")
                            {
                                ta.Close();
                                if (cs_type == "8" && resuCode.Trim() == "04")
                                {
                                    if (user_type == 1)
                                    {
                                        return "ลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว ต้องการบันทึกหรือไม่่"; ;
                                    }
                                    else
                                    {
                                        throw new Exception("เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว กรุณาติดต่อ สส.ชสอ.");
                                    }
                                }
                                else
                                {
                                    //throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
                                    return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว คุณต้องการทำรายการต่อหรือไม่";
                                }
                            }
                        }
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " มีอยู่แล้ว กรุณาตรวจสอบ");
                    }
                }

                String sql_CardPS = "select * from wcreqdeposit where card_person = '" + card_person.Trim() + "' and approve_status > 0";

                Sdt dtchk_CardPS = ta.Query(sql_CardPS);
                if (dtchk_CardPS.Next())
                {
                    if (dtchk_CardPS.GetDecimal("approve_status") == 8)
                    {
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ เลขประจำตัวประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                    }
                    string deptaccount_no = dtchk_CardPS.GetString("deptaccount_no").Trim();
                    if (deptaccount_no != "")
                    {
                        string resuCode = checkDupMaster(deptaccount_no);
                        if (checkDupMaster(deptaccount_no) != "")
                        {
                            string resuStr = checkResignCauseCode(resuCode);
                            if (resuStr != "no")
                            {
                                ta.Close();
                                if (cs_type == "8" && resuCode.Trim() == "04")
                                {
                                    if (user_type == 1)
                                    {
                                        return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว ต้องการบันทึกหรือไม่";
                                    }
                                    else
                                    {
                                        throw new Exception("เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว กรุณาติดต่อ สส.ชสอ.");
                                    }
                                }
                                else
                                {
                                    //throw new Exception("ไม่สามารถทำรายการได้ เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " มีอยู่แล้ว กรุณาตรวจสอบ");
                                    return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ได้ทำการแจ้ง" + resuStr + "แล้ว คุณต้องการทำรายการต่อหรือไม่";
                                }
                            }
                        }
                    }
                    else
                    {
                        ta.Close();
                        throw new Exception("ไม่สามารถทำรายการได้ เกิดข้อผิดพลาดในการตรวจสอบ เลขประจำตัวประชาชน กรุณาลองบันทึกใหม่อีกครั้ง");
                    }
                    ta.Close();
                    throw new Exception("ไม่สามารถทำรายการได้ เลขประจำตัวประชาชน " + card_person + " มีอยู่แล้ว กรุณาตรวจสอบ");
                }
                if (otrher_cremation_flag)
                {
                    ta.Close();
                    return "เลขสมาชิกสหกรณ์ " + member_no + " " + deptaccount_name + "   " + deptaccount_sname + " บัตรประชาชน " + card_person + " ไม่ได้สมัครสมาชิกฯ ในกองวิชาชีพ คุณต้องการบันทึกใช่หรือไม่?";
                }

                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }

            return "";
        }

        private string checkDupMaster(string deptaccount_no)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();

            string result = "";
            string sql = "select * from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "'";
            Sdt dt = ta.Query(sql);
            if (dt.Next())
            {
                try
                {
                    result = dt.GetString("resigncause_code").Trim();
                }
                catch
                {
                    result = "";
                }
            }
            return result;
        }

        private string checkResignCauseCode(string resigncause_code)
        {
            string result;
            switch (resigncause_code)
            {
                case "01":
                    result = "ลาออก";
                    break;
                case "02":
                    result = "เสียชีวิต";
                    break;
                case "03":
                    result = "ยกเลิก";
                    break;
                case "04":
                    result = "ลิ้นสุด";
                    break;
                default:
                    result = "no";
                    break;                    
            }
            return result;
        }

        public Boolean SaveWaitGroupPaid(string pbl, String xmlDwMain, int period)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {                
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_wait_paid_group");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                dwMain.SetFilter("status = 2 or cancal_status = 1");
                dwMain.Filter();
                string deptaccount_no;
                Decimal status,cantus;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                    status = dwMain.GetItemDecimal(i + 1, "status");
                    cantus = dwMain.GetItemDecimal(i + 1, "cancal_status");

                    if (status == 2 && cantus == 0)
                    {

                        ta.Exe("update wcrecievemonth set status_post = " + status + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                    }

                    if (cantus == 1)
                    {
                        status = 9;
                        ta.Exe("update wcrecievemonth set status_post = " + status + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                    }
                    }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean SaveGroupPaid(string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                int fee_count;
                if (fee_flag == 0)
                {
                    fee_count = 0;
                }
                else
                {
                    fee_count = 1;
                }
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_paid_group");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                dwMain.SetFilter("status in(1,9)");
                dwMain.Filter();
                DataStore dwStm = new DataStore(pbl, "d_wf_paid_statement");

                int year = DateTime.Today.Year + 543;
                // string slipNo = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", year, ta);
                string slipNo, deptaccount_no, recppaytype_code, cash_type, sliptype_code;
                string deptslip_date = DateTime.Today.ToString("dd/MM/yyyy"), operate_date = DateTime.Today.ToString("dd/MM/yyyy");
                decimal deptslip_amt;
                String sqlSlip, sqlSeqStm, sqlinsertdet, sqlPrncBal, sqlChk, sqlChkWcreceive;
                Sdt dt, dtPrnc, dtChk, dtChkWcreceive;
                int seq_stm;
                string[] deptitemtype_code = new string[] { "WPF", "WFY" };
                decimal[] prncslip_amt = new decimal[2];
                string[] deptitemtype_desc = new string[] { "เงินสงเคราะห์ศพล่วงหน้า", "ค่าธรรมเนียมรายปี" };
                prncslip_amt[1] = fee;
                decimal principal = 0;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                    if (dwMain.GetItemDecimal(i + 1, "status") == 1)
                    {                        
                        sqlChkWcreceive = "select * from wcrecievemonth where wfmember_no = '" + deptaccount_no + "' and recv_period = " + period + " and status_post <> 1 and wcitemtype_code = 'FEE'";
                        dtChkWcreceive = ta.Query(sqlChkWcreceive);
                        if (!dtChkWcreceive.Next())
                        {
                            throw new Exception("เลขฌาปนกิจ " + deptaccount_no + " ได้ทำรายการไปแล้ว ไม่สามารถทำรายการได้อีก");
                        }
                        slipNo = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", year, ta);

                        recppaytype_code = "CSH";
                        deptslip_amt = dwMain.GetItemDecimal(i + 1, "fee_year");
                        prncslip_amt[0] = deptslip_amt;
                        cash_type = "CSH";
                        sliptype_code = "WPX";
                        /*
                        sqlChk = "select wfmember_no from wcrecievemonth where wfmember_no = '" + deptaccount_no + "' and wcitemtype_code = 'FEE' and status_post in(2, 8) and fee_year <> 0 and recv_period <" + period;
                        dtChk = ta.Query(sqlChk);
                        if (dtChk.Next())
                        {
                            throw new Exception("เลขฌาปนกิจ " + dtChk.GetString("wfmember_no") + " กรุณาทำการชำระเงินของรายการ(งวด)ก่อนหน้านี้ให้เรียบร้อยก่อนทำรายการงวดนี้");
                        }
                        */
                        sqlSlip = @"insert into wcdeptslip(deptslip_no, deptaccount_no, deptslip_date, depttype_code, deptitemtype_code, recppaytype_code, 
                                deptslip_amt, cash_type, entry_id, entry_date, operate_date, item_status, sliptype_code, branch_id, recv_period) 
                                values('" + slipNo + "', '" + deptaccount_no + "', to_date('" + deptslip_date + "', 'dd/mm/yyyy'), '01', '" + deptitemtype_code[0] + @"', 
                                '" + recppaytype_code + "', " + deptslip_amt + ", '" + cash_type + "', '" + entry_id + "', to_date('" + operate_date + @"', 'dd/mm/yyyy'), 
                                to_date('" + operate_date + "', 'dd/mm/yyyy'), 1, '" + sliptype_code + "', '" + branch_id + "', '" + period + "')";

                        try
                        {
                            ta.Exe(sqlSlip);
                        }
                        catch (Exception ex)
                        {
                            ta.Close();
                            throw ex;
                        }

                        for (int j = 0; j <= fee_count; j++)
                        {
                            sqlinsertdet = @"insert into wcdeptslipdet(deptslip_no, seq_no, depttype_code, deptitemtype_code, prncslip_amt, slip_desc, branch_id)
                    values('" + slipNo + "', " + (j + 1) + ", '01', '" + deptitemtype_code[j] + "', '" + prncslip_amt[j] + "', '" + deptitemtype_desc[j] + "', '" + branch_id + "')";

                            try
                            {
                                ta.Exe(sqlinsertdet);
                            }
                            catch (Exception ex)
                            {
                                ta.Close();
                                throw ex;
                            }
                        }
                        if (statement_flag == 1)
                        {
                            sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dt = ta.Query(sqlSeqStm);
                            if (dt.Next())
                            {
                                seq_stm = dt.GetInt32(0);
                                seq_stm++;
                            }
                            else
                            {
                                seq_stm = 0;
                            }
                            sqlPrncBal = "select prncbal, withdrawable_amt from wcdeptmaster where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dtPrnc = ta.Query(sqlPrncBal);
                            if (dtPrnc.Next())
                            {
                                principal = dtPrnc.GetDecimal(0);
                            }
                            else
                            {
                                principal = 0;
                            }
                            principal += prncslip_amt[0];

                            for (int k = 0; k <= fee_count; k++)
                            {
                                dwStm.InsertRow(0);
                                dwStm.SetItemString(k + 1, "deptaccount_no", deptaccount_no);
                                dwStm.SetItemDecimal(k + 1, "seq_no", seq_stm);
                                dwStm.SetItemString(k + 1, "deptitemtype_code", deptitemtype_code[k]);
                                dwStm.SetItemString(k + 1, "ref_docno", slipNo);
                                dwStm.SetItemDecimal(k + 1, "deptitem_amt", prncslip_amt[k]);
                                dwStm.SetItemDecimal(k + 1, "prncbal", principal);
                                dwStm.SetItemString(k + 1, "entry_id", entry_id);
                                dwStm.SetItemDateTime(k + 1, "entry_date", DateTime.Today);
                                dwStm.SetItemDateTime(k + 1, "operate_date", DateTime.Today);
                                dwStm.SetItemDecimal(k + 1, "statement_status", 1);
                                dwStm.SetItemDecimal(k + 1, "item_status", 1);
                                //dwStm.SetItemDecimal(i, "sign_flag", dwSlip.GetItemDecimal(1, "sign_flag"));
                                dwStm.SetItemString(k + 1, "branch_id", branch_id);
                                seq_stm++;
                                try
                                {
                                    ta.Exe(new DwHandle(dwStm).SqlInsertSyntax("WCDEPTSTATEMENT", k + 1));
                                }
                                catch (Exception ex)
                                {
                                    ta.Close();
                                    throw ex;
                                }
                            }
                        }
                        string period_year = (period.ToString()).Substring(0, 4);
                        period_year = (Convert.ToInt32(period_year) - 542).ToString();
                        String SQLMaster = @"update wcdeptmaster set effective_date = to_date('01/01/" + period_year + @"', 'dd/mm/yyyy')";

                        sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                        dt = ta.Query(sqlSeqStm);
                        if (dt.Next())
                        {
                            if (statement_flag == 1)
                            {
                                seq_stm = dt.GetInt32(0);
                                SQLMaster = SQLMaster + ", prncbal=" + principal + ", laststmseq_no = " + seq_stm;
                            }
                            //ta.Exe("update wcdeptmaster set prncbal=" + principal + " where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'");
                            //ta.Exe("update wcrecievemonth set status_post = " + dwMain.GetItemDecimal(i + 1, "status") + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                        }
                        SQLMaster = SQLMaster + " where deptaccount_no='" + deptaccount_no +
                                "' and branch_id='" + branch_id + "'";

                        ta.Exe(SQLMaster);
                    }
                    ta.Exe("update wcrecievemonth set status_post = " + dwMain.GetItemDecimal(i + 1, "status") + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean SaveGroupPaidNew(string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag) //สำหรับ สส.ชสอ ให้ศูนย์สามารถออกใบเสร็จได้เลย หลังจากชำระกลุ่มแล้ว โดยไม่ต้องรอสมาคมอนุมัติ
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                int fee_count;
                if (fee_flag == 0)
                {
                    fee_count = 0;
                }
                else
                {
                    fee_count = 1;
                }
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_wait_paid_group");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                dwMain.SetFilter("status = 2 or cancal_status = 1");
                dwMain.Filter();

                DataStore dwStm = new DataStore(pbl, "d_wf_paid_statement");

                int year = DateTime.Today.Year + 543;
                // string slipNo = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", year, ta);
                string slipNo, slipNoBranch, deptaccount_no, recppaytype_code, cash_type, sliptype_code;
                string deptslip_date = DateTime.Today.ToString("dd/MM/yyyy"), operate_date = DateTime.Today.ToString("dd/MM/yyyy");
                decimal deptslip_amt;
                String sqlSlip, sqlSlipBranch, sqlSeqStm, sqlinsertdet, sqlPrncBal, sqlChk, sqlChkWcreceive;
                Sdt dt, dtPrnc, dtChk, dtChkWcreceive;
                int seq_stm;
                string[] deptitemtype_code = new string[] { "WPF", "WFY" };
                decimal[] prncslip_amt = new decimal[2];
                string[] deptitemtype_desc = new string[] { "เงินสงเคราะห์ศพล่วงหน้า", "ค่าธรรมเนียมรายปี" };
                prncslip_amt[1] = fee;
                decimal principal = 0, cantus = 0, status = 0;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                    cantus = dwMain.GetItemDecimal(i + 1, "cancal_status");
                    status = dwMain.GetItemDecimal(i + 1, "status");
                    if (status == 2 && cantus == 0)
                    {
                        sqlChkWcreceive = "select * from wcrecievemonth where wfmember_no = '" + deptaccount_no + "' and recv_period = " + period + " and status_post <> 1 and wcitemtype_code = 'FEE'";
                        dtChkWcreceive = ta.Query(sqlChkWcreceive);
                        if (!dtChkWcreceive.Next())
                        {
                            throw new Exception("เลขฌาปนกิจ " + deptaccount_no + " ได้ทำรายการไปแล้ว ไม่สามารถทำรายการได้อีก");
                        }
                        slipNo = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", year, ta);
                        //slipNoBranch = new DocumentControlBranch().NewDocumentNo("WCFEEYEARSLIP", branch_id, "8", year, ta);
                        recppaytype_code = "CSH";
                        deptslip_amt = dwMain.GetItemDecimal(i + 1, "fee_year");
                        prncslip_amt[0] = deptslip_amt;
                        cash_type = "CSH";
                        sliptype_code = "WPX";
                        /*
                        sqlChk = "select wfmember_no from wcrecievemonth where wfmember_no = '" + deptaccount_no + "' and wcitemtype_code = 'FEE' and status_post in(2, 8) and fee_year <> 0 and recv_period <" + period;
                        dtChk = ta.Query(sqlChk);
                        if (dtChk.Next())
                        {
                            throw new Exception("เลขฌาปนกิจ " + dtChk.GetString("wfmember_no") + " กรุณาทำการชำระเงินของรายการ(งวด)ก่อนหน้านี้ให้เรียบร้อยก่อนทำรายการงวดนี้");
                        }
                        */
                        sqlSlip = @"insert into wcdeptslip(deptslip_no, deptaccount_no, deptslip_date, depttype_code, deptitemtype_code, recppaytype_code, 
                                deptslip_amt, cash_type, entry_id, entry_date, operate_date, item_status, sliptype_code, branch_id, recv_period) 
                                values('" + slipNo + "', '" + deptaccount_no + "', to_date('" + deptslip_date + "', 'dd/mm/yyyy'), '01', '" + deptitemtype_code[0] + @"', 
                                '" + recppaytype_code + "', " + deptslip_amt + ", '" + cash_type + "', '" + entry_id + "', to_date('" + operate_date + @"', 'dd/mm/yyyy'), 
                                to_date('" + operate_date + "', 'dd/mm/yyyy'), 1, '" + sliptype_code + "', '" + branch_id + "', '" + period + "')";

//                        sqlSlipBranch = @"insert into wcdeptslipbranch(deptslipbranch_no, deptslip_no, cs_type)
//                                          values('" + slipNoBranch + "','" + slipNo + "','8')";
                        try
                        {
                            ta.Exe(sqlSlip);
                         //  ta.Exe(sqlSlipBranch);
                        }
                        catch (Exception ex)
                        {
                            ta.Close();
                            throw ex;
                        }

                        for (int j = 0; j <= fee_count; j++)
                        {
                            sqlinsertdet = @"insert into wcdeptslipdet(deptslip_no, seq_no, depttype_code, deptitemtype_code, prncslip_amt, slip_desc, branch_id)
                    values('" + slipNo + "', " + (j + 1) + ", '01', '" + deptitemtype_code[j] + "', '" + prncslip_amt[j] + "', '" + deptitemtype_desc[j] + "', '" + branch_id + "')";

                            try
                            {
                                ta.Exe(sqlinsertdet);
                            }
                            catch (Exception ex)
                            {
                                ta.Close();
                                throw ex;
                            }
                        }
                        if (statement_flag == 1)
                        {
                            sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dt = ta.Query(sqlSeqStm);
                            if (dt.Next())
                            {
                                seq_stm = dt.GetInt32(0);
                                seq_stm++;
                            }
                            else
                            {
                                seq_stm = 0;
                            }
                            sqlPrncBal = "select prncbal, withdrawable_amt from wcdeptmaster where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dtPrnc = ta.Query(sqlPrncBal);
                            if (dtPrnc.Next())
                            {
                                principal = dtPrnc.GetDecimal(0);
                            }
                            else
                            {
                                principal = 0;
                            }
                            principal += prncslip_amt[0];

                            for (int k = 0; k <= fee_count; k++)
                            {
                                dwStm.InsertRow(0);
                                dwStm.SetItemString(k + 1, "deptaccount_no", deptaccount_no);
                                dwStm.SetItemDecimal(k + 1, "seq_no", seq_stm);
                                dwStm.SetItemString(k + 1, "deptitemtype_code", deptitemtype_code[k]);
                                dwStm.SetItemString(k + 1, "ref_docno", slipNo);
                                dwStm.SetItemDecimal(k + 1, "deptitem_amt", prncslip_amt[k]);
                                dwStm.SetItemDecimal(k + 1, "prncbal", principal);
                                dwStm.SetItemString(k + 1, "entry_id", entry_id);
                                dwStm.SetItemDateTime(k + 1, "entry_date", DateTime.Today);
                                dwStm.SetItemDateTime(k + 1, "operate_date", DateTime.Today);
                                dwStm.SetItemDecimal(k + 1, "statement_status", 1);
                                dwStm.SetItemDecimal(k + 1, "item_status", 1);
                                //dwStm.SetItemDecimal(i, "sign_flag", dwSlip.GetItemDecimal(1, "sign_flag"));
                                dwStm.SetItemString(k + 1, "branch_id", branch_id);
                                seq_stm++;
                                try
                                {
                                    ta.Exe(new DwHandle(dwStm).SqlInsertSyntax("WCDEPTSTATEMENT", k + 1));
                                }
                                catch (Exception ex)
                                {
                                    ta.Close();
                                    throw ex;
                                }
                            }
                        }/*
                        string period_year = (period.ToString()).Substring(0, 4);
                        period_year = (Convert.ToInt32(period_year) - 542).ToString();
                        String SQLMaster = @"update wcdeptmaster set effective_date = to_date('01/01/" + period_year + @"', 'dd/mm/yyyy')";

                        sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                        dt = ta.Query(sqlSeqStm);
                        if (dt.Next())
                        {
                            if (statement_flag == 1)
                            {
                                seq_stm = dt.GetInt32(0);
                                SQLMaster = SQLMaster + ", prncbal=" + principal + ", laststmseq_no = " + seq_stm;
                            }
                            //ta.Exe("update wcdeptmaster set prncbal=" + principal + " where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'");
                            //ta.Exe("update wcrecievemonth set status_post = " + dwMain.GetItemDecimal(i + 1, "status") + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                        }
                        SQLMaster = SQLMaster + " where deptaccount_no='" + deptaccount_no +
                                "' and branch_id='" + branch_id + "'";

                        ta.Exe(SQLMaster); */
                    }
                    if (cantus == 1)
                    {
                        status = 9;
                       // ta.Exe("update wcrecievemonth set status_post = " + status + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                    }

                    ta.Exe("update wcrecievemonth set status_post = " + status + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean ChgEffective(string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_paid_group");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                dwMain.SetFilter("status in(1,9)");
                dwMain.Filter();

                DataStore dwStm = new DataStore(pbl, "d_wf_paid_statement");

                int year = DateTime.Today.Year + 543;
                string deptaccount_no;
                string deptslip_date = DateTime.Today.ToString("dd/MM/yyyy"), operate_date = DateTime.Today.ToString("dd/MM/yyyy");
                String sqlSeqStm, sqlPrncBal, sqlChkWcreceive;
                Sdt dt, dtPrnc, dtChkWcreceive;
                int seq_stm;
                string[] deptitemtype_code = new string[] { "WPF", "WFY" };
                decimal[] prncslip_amt = new decimal[2];
                string[] deptitemtype_desc = new string[] { "เงินสงเคราะห์ศพล่วงหน้า", "ค่าธรรมเนียมรายปี" };
                prncslip_amt[1] = fee;
                decimal principal = 0;
                string period_mm = (period.ToString()).Substring(4,2);
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                    if (period_mm == "12")
                    {

                        if (dwMain.GetItemDecimal(i + 1, "status") == 1)
                        {
                            sqlChkWcreceive = "select * from wcrecievemonth where wfmember_no = '" + deptaccount_no + "' and recv_period = " + period + " and status_post <> 1 and wcitemtype_code = 'FEE'";
                            dtChkWcreceive = ta.Query(sqlChkWcreceive);
                            if (!dtChkWcreceive.Next())
                            {
                                throw new Exception("เลขฌาปนกิจ " + deptaccount_no + " ได้ทำรายการไปแล้ว ไม่สามารถทำรายการได้อีก");
                            }

                            sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dt = ta.Query(sqlSeqStm);
                            if (dt.Next())
                            {
                                seq_stm = dt.GetInt32(0);
                                seq_stm++;
                            }
                            else
                            {
                                seq_stm = 0;
                            }
                            sqlPrncBal = "select prncbal, withdrawable_amt from wcdeptmaster where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dtPrnc = ta.Query(sqlPrncBal);
                            if (dtPrnc.Next())
                            {
                                principal = dtPrnc.GetDecimal(0);
                            }
                            else
                            {
                                principal = 0;
                            }
                            principal += prncslip_amt[0];

                            string period_year = (period.ToString()).Substring(0, 4);
                            period_year = (Convert.ToInt32(period_year) - 542).ToString();
                            String SQLMaster = @"update wcdeptmaster set effective_date = to_date('01/01/" + period_year + @"', 'dd/mm/yyyy')";

                            sqlSeqStm = "select max(seq_no) as sss from wcdeptstatement where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                            dt = ta.Query(sqlSeqStm);
                            if (dt.Next())
                            {
                                if (statement_flag == 1)
                                {
                                    seq_stm = dt.GetInt32(0);
                                    SQLMaster = SQLMaster + ", prncbal=" + principal + ", laststmseq_no = " + seq_stm;
                                }
                                //ta.Exe("update wcdeptmaster set prncbal=" + principal + " where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'");
                                //ta.Exe("update wcrecievemonth set status_post = " + dwMain.GetItemDecimal(i + 1, "status") + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                            }
                            SQLMaster = SQLMaster + " where deptaccount_no='" + deptaccount_no +
                                    "' and branch_id='" + branch_id + "'";

                            ta.Exe(SQLMaster);
                        }
                    }
                    ta.Exe("update wcrecievemonth set status_post = " + dwMain.GetItemDecimal(i + 1, "status") + " where wfmember_no ='" + deptaccount_no + "' and wcitemtype_code = 'FEE' and recv_period = " + period);
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean TrnMemb(String XmlMain, string pbl, string Username, string oldbranch_id, string cs_type)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_trn_memb");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);

                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    string ReqchgNo = new DocumentControl().NewDocumentNo("WCCHGDOCNO", DateTime.Today.Year + 543, ta);
                    string deptaccount_no = dwMain.GetItemString(i + 1, "deptaccount_no");
                    string today = "'" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                    string die_date = "'" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                    string inform_date = "'" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                    string resigncause_code = "01";

                    Decimal approve_flag = 1;//DwMain.GetItemDecimal(1, "approve_flag");
                    string member_no = dwMain.GetItemString(1, "member_no").Trim();
                    decimal quantitymem_amt = 0;
                    Sdt dt = ta.Query("select count(*) as quantitymem_amt from wcdeptmaster where deptclose_status = 0");
                    if (dt.Next())
                    {
                        quantitymem_amt = dt.GetDecimal("quantitymem_amt");
                    }
                    Decimal cremateestpay_amt = 0;
                    string wftype_code = dwMain.GetItemString(1, "wftype_code");
                    Decimal withdrawable_amt = 0;
                    Decimal wfe_amt = 0;
                    Decimal wfe_amt2 = 0;
                    decimal die_status = 1;
                    string remark = "โอนย้ายสมาชิกไปสหกรณ์อื่น";

                    ///สร้างใบคำขอลาออก
                    String sql = @"INSERT INTO wcreqchg_dept (dpreqchg_doc, deptaccount_no, approve_flag, remark, entry_date, entry_id, die_date, inform_date, resigncause_code,";
                    if (approve_flag == 1)
                    {
                        sql = sql + " approve_date,";
                    }
                    sql = sql + " reqchg_status, member_no, quantitymem_amt, cremateestpay_amt, wftype_code, die_status, branch_id, withdrawable_amt, wfe_amt, wfe_amt2)";
                    sql = sql + "VALUES ('" + ReqchgNo + "','" + deptaccount_no + "'," + approve_flag + ",'" + remark + "', to_date(" + today + ",'" + Username + "', to_date(" + die_date + ", to_date(" + inform_date + ",'" + resigncause_code + "',";
                    if (approve_flag == 1)
                    {
                        sql = sql + "to_date(" + today + ",";
                    }
                    sql = sql + "" + 1 + ",'" + member_no + "'," + quantitymem_amt + "," + cremateestpay_amt + ",'" + wftype_code + "'," + die_status + ",'" + oldbranch_id + "'," + withdrawable_amt + "," + wfe_amt + "," + wfe_amt2 + ")";
                    try
                    {
                        int req = ta.Exe(sql);
                    }
                    catch
                    {
                        throw new Exception("ไม่สามารถทำการแจ้งเสียชีวิต/ลาออกได้");
                    }


                    ///Update ทะเบียนเดิม ให้ลาออก
                    String msql = "update wcdeptmaster set deptclose_status=" + 1 +
                            @", deptclose_date=to_date(" + today +
                            @", lastaccess_date=to_date(" + today +
                            @", last_process_date=to_date(" + today +
                            @", wfmember_status=" + -1 +
                            @", die_date=to_date(" + die_date +
                        //@", wfcarcass_seq='" + quantitymem_amt + "'" +
                            @", resigncause_code='" + resigncause_code + "'";
                    if (approve_flag == 1)
                    {
                        msql = msql + ", apply_date=  to_date(" + today;
                    }
                    msql = msql + @", withdrawable_amt=" + withdrawable_amt +
                    @", cremateapp_amt=" + cremateestpay_amt;
                    if (approve_flag == 1)
                    {
                        msql = msql + ", kpreceive_status=" + 1;
                    }
                    else
                    {
                        msql = msql + ", kpreceive_status=" + -1;
                    }
                    msql = msql + " where deptaccount_no='" + deptaccount_no + "' and branch_id='" + oldbranch_id + "'";
                    try
                    {
                        ta.Exe(msql);
                    }
                    catch
                    {
                        throw new Exception("ไม่สามารถ Update ทะเบียนเดิมได้");
                    }

                    string branch_id = "", new_memb_no = "";

                    ///Check branch_id and member_no(new)
                    try
                    {
                        branch_id = dwMain.GetItemString(i + 1, "branch_id");
                        new_memb_no = int.Parse(dwMain.GetItemString(i + 1, "new_memb_no")).ToString("000000");
                    }
                    catch
                    {
                        throw new Exception("กรุูณาระบุ \"ศูนย์ประสานงาน และ เลขสมาชิกสหกรณ์\" ที่ต้องการโอนย้ายไป");
                    }
                    ///เลยใบคำขอ และเลขสลิป
                    string deptrequest_docno = new DocumentControl().NewDocumentNo("WCAPPLDOCNO", DateTime.Today.Year + 543, ta);
                    string deptslip_no = new DocumentControl().NewDocumentNo("WCSLIPDOCNO", DateTime.Today.Year, ta);

                    ///new deptaccount_no from cmshrlondoccontrol
                    string getNewAcc_no = "select last_documentno from cmshrlondoccontrol where document_code = 'WFGENMEMNO'";
                    Sdt dtDocControl = ta.Query(getNewAcc_no);
                    string NewAcc_no = "";
                    if (dtDocControl.Next())
                    {
                        int Acc_no = Convert.ToInt32(dtDocControl.GetString("last_documentno")) + 1;
                        NewAcc_no = int.Parse(Convert.ToString(Acc_no)).ToString("000000");
                        ta.Exe("update cmshrlondoccontrol set last_documentno = '" + NewAcc_no + "' where document_code = 'WFGENMEMNO'");
                    }
                    else
                    {
                        throw new Exception("ไม่สามารถสร้างเลขฌาปนกิจได้");
                    }

                    /// ใบสมัคร
                    String SqlInsertReq = @"insert into wcreqdeposit(deptrequest_docno, depttype_code, wftype_code, member_no, prename_code, 
                                            deptaccount_name, deptaccount_sname, card_person, remark, deptaccount_no, membgroup_code, wfbirthday_date, 
                                            contact_address, hometel, ampher_code, province_code, member_type, wfrecieve_no, approve_status, apply_date, 
                                            entry_id, entry_date, postcode, wfaccount_name, sex, branch_id, pay_status, approve_date, approve_id) 
                                            select '" + deptrequest_docno + @"', depttype_code, wftype_code, " + new_memb_no + @", prename_code, deptaccount_name,
                                            deptaccount_sname, card_person, remark, '" + NewAcc_no + @"', membgroup_code, wfbirthday_date, 
                                            contact_address, phone, ampher_code, province_code, member_type, '" + deptslip_no + "', 1, to_date(" + today + ", '"
                                            + Username + "', to_date(" + today + ", postcode, wfaccount_name, sex, '" + branch_id + "', 1, to_date(" + today + ", '"
                                            + Username + "' from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "'";
                    try
                    {
                        ta.Exe(SqlInsertReq);
                    }
                    catch
                    {
                        throw new Exception("ไม่สามารถสร้างใบสมัครใหม่ได้");
                    }

                    /// ใบสมัครผู้รับผลประโยชน์
                    String GetcoDept = "select * from wccodeposit where deptaccount_no = '" + deptaccount_no + "' order by seq_no";
                    Sdt dtGetcoDept = ta.Query(GetcoDept);
                    int j = 1;
                    while (dtGetcoDept.Next())
                    {
                        String SqlInsertReqCo = @"insert into wcreqcodeposit(deptrequest_docno, seq_no, {0}NAME{0}, codept_addr, codept_id, branch_id)
                                                select '" + deptrequest_docno + "', " + j + ", {0}NAME{0}, codept_addre, codept_id, '" + branch_id + @"'
                                                from wccodeposit where deptaccount_no = '" + deptaccount_no + "' and seq_no = " + j;
                        SqlInsertReqCo = string.Format(SqlInsertReqCo, "\"");
                        j++;
                        try
                        {
                            ta.Exe(SqlInsertReqCo);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถบันทึกผู้รับผลประโยชน์(ใบสมัคร)ได้");
                        }
                    }

                    ///ใบสมัคร รายการชำระ
                    string[] deptitemtype_code = new string[] { "FEE", "WFY", "WPF" };
                    string[] deptitemtype_desc = new string[] { "ค่ำสมัครใหม่", "ค่าบำรุงรายปี", "เงินสงเคราะห์ล่วงหน้า" };
                    String SqlMembType = "select * from wcmembertype where wftype_code = '" + wftype_code + "' and cs_type = '" + cs_type + "'";
                    Sdt dtMembType = ta.Query(SqlMembType);
                    if (dtMembType.Next())
                    {
                        double[] pay_amt = new double[] { dtMembType.GetDouble("feeappl_amt"), dtMembType.GetDouble("feeperyear_amt"), dtMembType.GetDouble("paybffuture_amt") };

                        for (int k = 0; k <= 2; k++)
                        {
                            String SqlInsertReqDet = @"insert into wcreqdetail(deptrequest_docno, seq_no, depttype_code, wftype_code, deptitemtype_code, apply_amt, installable,
                                                    minimuminstall, status_pay, amt,branch_id) Values('" + deptrequest_docno + "', " + (k + 1) + ", '01', '" + wftype_code + "', '"
                                                    + deptitemtype_code[k] + @"', 0, 0, 0, 1, " + pay_amt[k] + ", '" + branch_id + "')";
                            try
                            {
                                ta.Exe(SqlInsertReqDet);
                            }
                            catch
                            {
                                throw new Exception("ไม่สามารถบันทึกรายการรับชำระ(ใบสมัคร)ได้");
                            }
                        }


                        ///ทะเบียน
                        String SqlGetContant = "select deptopen_date from wcdeptconstant where cs_type = '" + cs_type + "'";
                        Sdt dtGetContant = ta.Query(SqlGetContant);
                        if (dtGetContant.Next())
                        {
                            string Sdeptopen_date = dtGetContant.GetDateEn("deptopen_date");
                            String SqlCreateMaster = @"insert into wcdeptmaster(deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, member_no, prename_code, wfaccount_name,
                                            deptaccount_name, deptaccount_sname, deptclose_status, card_person, prncbal, laststmseq_no, lastaccess_date, last_process_date,
                                            membgroup_code, wfmember_status, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, phone,
                                            deptrequest_docno, apply_date, branch_id, wpf_amt)
                                            select '" + NewAcc_no + @"', wftype_code, depttype_code, deptopen_date, effective_date, '" + new_memb_no + @"', 
                                            prename_code, wfaccount_name,deptaccount_name, deptaccount_sname, 0, card_person, " + pay_amt[2] + @", 3, to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss') 
                                            , to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss'), membgroup_code, 1, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, 
                                            phone, '" + deptrequest_docno + "', to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 'dd/mm/yyyy HH24:mi:ss'),'" + branch_id + "', " + pay_amt[2] + " from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "'";
                            ta.Exe(SqlCreateMaster);

                            ///สลิป

                            double amt = pay_amt.Sum();
                            String sqlinsertslip = @"insert into wcdeptslip(deptslip_no, deptaccount_no, deptslip_date, depttype_code, deptitemtype_code, recppaytype_code,
                            deptslip_amt, cash_type, entry_id, entry_date,item_status, sliptype_code, branch_id) values('" + deptslip_no + "', '" + deptrequest_docno + @"',
                            to_date('" + DateTime.Today.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), '01', 'WFF', 'CSH', " + amt + ", 'CSH', '" + Username + @"', 
                            to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 'dd/mm/yyyy HH24:mi:ss') , 1, 'WPX', '" + branch_id + "')";
                            ta.Exe(sqlinsertslip);

                            ///สลิก detail
                            String sqlinsertdet = "";
                            for (int h = 0; h <= 2; h++)
                            {
                                sqlinsertdet = @"insert into wcdeptslipdet(deptslip_no, seq_no, depttype_code, deptitemtype_code, prncslip_amt, slip_desc, branch_id)
                                values('" + deptslip_no + "', " + (h + 1) + ", '01', '" + deptitemtype_code[h] + "', '" + pay_amt[h] + "', '" + deptitemtype_desc[h] + "', '" + branch_id + "')";
                                ta.Exe(sqlinsertdet);
                            }
                        }
                        else
                        {
                            throw new Exception("ไม่สามารถสร้างทะเบียนสมาชิกได้");
                        }
                    }
                    else
                    {
                        throw new Exception("ไม่สามารถดึงค่าเงินได้");
                    }


                    ///ผู้รับผลประโยชน์(ทะเบียน)
                    dtGetcoDept.ReStart();
                    int g = 1;
                    while (dtGetcoDept.Next())
                    {
                        String SqlInsertCoDept = @"insert into wccodeposit(deptaccount_no, seq_no, {0}NAME{0}, codept_addre, codept_id, branch_id)
                                                select '" + NewAcc_no + "', " + g + ", {0}NAME{0}, codept_addre, codept_id, '" + branch_id + @"' from wccodeposit
                                                where deptaccount_no = '" + deptaccount_no + "' and seq_no = " + g;
                        SqlInsertCoDept = string.Format(SqlInsertCoDept, "\"");
                        g++;
                        try
                        {
                            ta.Exe(SqlInsertCoDept);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถบันทึกผู้รับผลประโยชน์(ทะเบียน)ได้");
                        }
                    }

                    ///Statement (ทะเบียน)
                    for (int l = 0; l <= 2; l++)
                    {
                        double[] Spay_amt = new double[] { dtMembType.GetDouble("feeappl_amt"), dtMembType.GetDouble("feeperyear_amt"), dtMembType.GetDouble("paybffuture_amt") };
                        String SqlInsertStatement = @"insert into wcdeptstatement(deptaccount_no , seq_no, deptitemtype_code, operate_date, ref_docno, deptitem_amt, prncbal,
                                            item_status, entry_id, entry_date,statement_status, branch_id) 
                                            Values('" + NewAcc_no + "', " + (l + 1) + ", '" + deptitemtype_code[l] + "', to_date('" + DateTime.Today.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), '" + deptslip_no +
                                             "'," + Spay_amt[l] + "," + Spay_amt[l] + ", 1, '" + Username + "', to_date('" + DateTime.Today.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), 1, '" + branch_id + "')";
                        try
                        {
                            ta.Exe(SqlInsertStatement);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถบันทึก Statement ได้");
                        }
                    }
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Pay_die_member(String XmlMain, string pbl, string Username, string cs_type) //บันทึกการจ่ายเงินคนตายประจำเดือน by tar 12/01/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_chg_die_detail");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                string deptaccount_no, member_no, branch_id, die_date, sele, sele_month, for_year, yearmm;
                String SQLdie_mem;
                for (int i = 1; i <= dwMain.RowCount; i++)
                {
                    
                    sele = dwMain.GetItemString(i, "sele");
                    if (sele == "1")
                    {
                        sele_month = dwMain.GetItemString(i, "sele_mon");
                        if (sele_month == "00")
                        {
                            throw new Exception("กรุณาเลือกเดือนประกาศ");
                        }
                        else
                        {
                            for_year = dwMain.GetItemString(i, "for_year");
                            yearmm = for_year + sele_month;
                            deptaccount_no = dwMain.GetItemString(i, "deptaccount_no");
                            member_no = dwMain.GetItemString(i, "member_no");
                            branch_id = dwMain.GetItemString(i, "branch_id");
                            die_date = dwMain.GetItemString(i, "die_date");


                            SQLdie_mem = @"insert into wcpaydiemember(deptaccount_no, member_no,yearmm, branch_id, pay_status)
                                    values('" + deptaccount_no + "', '" + member_no + "','" + yearmm + "', '" + branch_id + "',8)";


                            ta.Exe(SQLdie_mem);
                        }
                    }
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean infrom_die_mm_save(String XmlMain, string pbl, string Username, string cs_type, string member_no, string deptaccount_no, string branch_id_in,string resignChg) //บันทึกการจ่ายเงินคนตายประจำเดือน by tar 12/01/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_inform_die_sele_month");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                string for_year = String.Empty
                     , sele_mon = String.Empty
                     , yearmm = String.Empty;
                Decimal die_age = 0 ,
                        pay_flag = 0;
                String SQLdie_mem = String.Empty,
                       SQLmem_close = String.Empty;

                for_year = dwMain.GetItemString(1, "for_year");
                sele_mon = dwMain.GetItemString(1, "sele_mon");
                pay_flag = dwMain.GetItemDecimal(1, "pay_flag");
                yearmm = for_year + sele_mon;
                if (sele_mon == "00")
                {
                    throw new Exception("กรุณาระบุ เดือนประกาศ");
                   
                }
                if (pay_flag == 3)
                    throw new Exception("กรุณาระบุ สถานะการชำระเงินคงสถาพ");

                if (resignChg == "02")
                {
                    try
                    {
                        die_age = dwMain.GetItemDecimal(1, "die_age");
                    }
                    catch
                    {
                        throw new Exception("กรุณาระบุ อายุของผู้เสียชีวิต");
                    }
                    if (die_age == 0)
                        throw new Exception("กรุณาระบุ อายุของผู้เสียชีวิต");

                    SQLdie_mem = @"insert into wcpaydiemember(deptaccount_no, member_no,yearmm, branch_id, pay_status, mem_age)
                                    values('" + deptaccount_no + "', '" + member_no + "','" + yearmm + "', '" + branch_id_in + "',8, '" + die_age + "')";
                    ta.Exe(SQLdie_mem);
                }

             

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean AgeChgProc(String cs_type, DateTime st_date, DateTime end_date)
        {
            
           
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                string sqlselect = @"select deptaccount_no,
                                    wfbirthday_date
                                    from wcdeptmaster
                                    where branch_id in (select coopbranch_id from cmucfcoopbranch where cs_type = '" + cs_type + @"')
                                    and deptclose_status = 0 and (deptopen_date between to_date('" + st_date.ToString("ddMMyyyy") + @"', 'ddmmyyyy') and to_date('" + end_date.ToString("ddMMyyyy") + @"', 'ddmmyyyy'))
                                    ";
                //'0210','0011','0032','0172','0070','0026', '0005','0082','0088','0024','0201','0119', '0017','0009','0053','0067','0001','0014'

                Sdt dtSelect = ta.Query(sqlselect);
                while (dtSelect.Next())
                {
                    DateTime ldtm_now = DateTime.MinValue, wfbirthday_date = DateTime.MinValue;
                    String deptaccount_no = String.Empty;
                    //Decimal total_age = 0;
                    int li_age = 0;

                    deptaccount_no = dtSelect.GetString("deptaccount_no");
                    wfbirthday_date = dtSelect.GetDate("wfbirthday_date");

                    
                    ldtm_now = DateTime.UtcNow;
                    li_age = ldtm_now.Year - wfbirthday_date.Year;
                    //total_age = Math.Round(li_age, 0);

                    if (li_age <= 999)
                    {
                        String agee = li_age.ToString();
                        String SqlUpdateAge = "update wcdeptmaster set total_age =  '" + agee + @"'
                                            where deptaccount_no = '" + deptaccount_no + "' ";

                        ta.Exe(SqlUpdateAge);
                    }

                 }


                ta.Commit();
                ta.Close();

                return true;
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }

        }

        public Boolean Update_Fee_Year(String XmlMain, string pbl, string cs_type, string branch_id) //อัพเดจ ยอดเรียกเก็บรายปี by tar 01/03/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_cri_ucf_paid");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);


                string for_year = dwMain.GetItemString(1, "for_year");
                string mmyyyy = dwMain.GetItemString(1, "mmyyyy");


                String UpFee = @"update wcrecievemonth 
                                 set fee_year = (select item_amt from wcucfrecievefixedyear where cs_type = '" + cs_type + @"' 
                                 and deptopen_date in(select deptopen_date from wcdeptmaster where deptaccount_no = wcrecievemonth.wfmember_no and rownum =1 ))
                                 where recv_period = '" + mmyyyy + @"' and
                                 wcitemtype_code = 'FEE' and
                                 branch_id = '" + branch_id + @"'
                                 and status_post <> 1";
                    ta.Exe(UpFee);
                   

               
                
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Update_Coopbranch(String XmlMain, string pbl, string cs_type, string branch_id) //
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_create_branch");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                string  coopbranch_id, coopbranch_desc, province_code, district_code, postcode, section, account_no, bank_name, bank_branch, account_name;
                Decimal area_desc, use_status;
                String userr = "guest";

                use_status = dwMain.GetItemDecimal(1, "use_status");
                 
                 coopbranch_id = dwMain.GetItemString(1, "coopbranch_id");
                 coopbranch_desc = dwMain.GetItemString(1, "coopbranch_desc");

                 try
                 {
                     province_code = dwMain.GetItemString(1, "province_code");
                 }
                 catch
                 {
                     province_code = "";
                 }
                 try
                 {
                     district_code = dwMain.GetItemString(1, "district_code");
                 }
                 catch
                 {
                     district_code = "";
                 }
                 try
                 {
                     postcode = dwMain.GetItemString(1, "postcode");
                 }
                 catch
                 {

                    postcode = "";
                 }
                 try
                 {
                     section = dwMain.GetItemString(1, "section");
                 }
                 catch
                 {
                     section = "";
                 }
                 try
                 {
                     area_desc = dwMain.GetItemDecimal(1, "area_desc");
                 }
                 catch
                 {
                     area_desc = 0;
                 }
                 try
                 {
                     account_no = dwMain.GetItemString(1, "account_no");
                 }
                 catch
                 {
                     account_no = "";
                 }
                 try
                 {
                     bank_name = dwMain.GetItemString(1, "bank_name");
                 }
                 catch
                 {
                     bank_name = "";
                 }
                 try
                 {
                     bank_branch = dwMain.GetItemString(1, "bank_branch");
                 }
                 catch
                 {
                     bank_branch = "";
                 }
                 
                 try
                 {
                     account_name = dwMain.GetItemString(1, "account_name");
                 }
                 catch
                 {
                     account_name = "";
                 }


                 String UpCoop = @" update CMUCFCOOPBRANCH 
                                 set COOPBRANCH_DESC = '" + coopbranch_desc + "',DISTRICT_CODE = '" + district_code + @"'
                                 , PROVINCE_CODE = '" + province_code + "', POSTCODE = '" + postcode + @"'
                                 , SECTION =  '" + section + "', AREA_DESC = '" + area_desc + @"'
                                 , ACCOUNT_NO =  '" + account_no + "', BANK_NAME = '" + bank_name + @"'
                                 , BANK_BRANCH =  '" + bank_branch + "', ACCOUNT_NAME = '" + account_name + @"'
                                 , USE_STATUS = '" + use_status + @"'
                                 where COOPBRANCH_ID = '" + coopbranch_id + @"' and CS_TYPE = '" + cs_type + "' ";
                ta.Exe(UpCoop);

                if (use_status == 0)
                {
                    String deleUser = @"delete from amsecusers where coopbranch_id = '" + coopbranch_id + "' and user_name <> '" + userr + "'";
                    ta.Exe(deleUser);
                }



                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean BranchDoc_New(String XmlMain, string pbl, string cs_type, string document_code) //
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_create_branch");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                string coopbranch_id;
                coopbranch_id = dwMain.GetItemString(1, "coopbranch_id");

                if (document_code == "WCFEEYEARSLIP")
                {
                    String insertBranchDocno = @"insert into cmbranchdoccontrol (branch_id, document_code, document_name, 
                                              last_documentno, document_prefix, document_length,
                                              document_format, document_year, clear_type,
                                              system_code, cs_type)
                                        values('" + coopbranch_id + @"', 'WCFEEYEARSLIP', 'เลขที่ใบเสร็จต่ออายุสมาชิก',
                                               0, '" + coopbranch_id + @"', 11,
                                               'PPPPYYRRRRR', to_char(sysdate,'yyyy')+543, 1,
                                               'ALL', '" + cs_type + @"')";

                    ta.Exe(insertBranchDocno);
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }
        
        public Boolean Pay_die_member_save(String XmlMain, string pbl, string Username, string cs_type) //บันทึกคนตายประจำเดือน by tar 10/01/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_chg_die_pay_detail");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
               // int count = 0;
                for (int i = 1; i <= dwMain.RowCount; i++)
                {

                    decimal pay_status = dwMain.GetItemDecimal(i, "pay_status");
                    //if (pay_status == 1)
                    //{
                        string deptaccount_no = dwMain.GetItemString(i, "deptaccount_no");
                        string m_inform = dwMain.GetItemString(i, "m_inform");
                        string free_manage = dwMain.GetItemString(i, "free_manage");
                        string m_manage_die = dwMain.GetItemString(i, "m_manage_die");
                        string pay_die = dwMain.GetItemString(i, "pay_die");
                        string m_payable_full = dwMain.GetItemString(i, "m_payable_full");
                        string prncbal = dwMain.GetItemString(i, "prncbal");
                        DateTime pay_date = dwMain.GetItemDate(i, "pay_date");
                        string pay_tdate = dwMain.GetItemString(i, "pay_tdate");
                        string pay_dateEN = pay_tdate.Substring(0, 2) + "/" + pay_tdate.Substring(2, 2) + "/" + Convert.ToString(Convert.ToInt32(pay_tdate.Substring(4, 4)) - 543);
                        String pay_datee = pay_date.ToString("dd/mm/yyyy");
                        String SQLdie_mem = @"update wcpaydiemember set m_inform = '" + m_inform + "',free_manage = '" + free_manage + "',m_manage_die = '" + m_manage_die + "',pay_die = '" + pay_die + "',m_payable_full = '" + m_payable_full + "',prncbal = '" + prncbal + "',pay_id  = '" + Username + "',entry_date = sysdate,pay_status  = '" + pay_status + "' where deptaccount_no =  '" + deptaccount_no + "' ";
                        ta.Exe(SQLdie_mem);
                    if (pay_status == 1)
                    {
                        String SQLpay = @"update wcpaydiemember set pay_date = to_date('" + pay_dateEN + "','dd/mm/yyyy') where deptaccount_no =  '" + deptaccount_no + "' ";
                        ta.Exe(SQLpay);
                    }

                       
                    //}
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Memo_Admin_Save(String XmlMain, string pbl, string Username, string cs_type, string branch_id) //MEMO by tar 09/02/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                string eDateTH, eDateEN, memo_detail, memo_type, money_type, remark, bank_code, MEMO_docno, text_detail;
                decimal memo_cnt;
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_memo_fin");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                // int count = 0;
                 eDateTH = dwMain.GetItemString(1, "memo_date");
                 eDateEN = eDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(eDateTH.Substring(6, 4)) - 543);


                 memo_detail = dwMain.GetItemString(1, "memo_detail");
                 memo_type = dwMain.GetItemString(1, "memo_type");
                 //money_type = dwMain.GetItemString(1, "money_type");
                 memo_cnt = dwMain.GetItemDecimal(1, "memo_cnt");
                 try
                 {
                     money_type = dwMain.GetItemString(1, "money_type");
                 }
                 catch
                 {
                     money_type = " ";
                 }

                 try
                 {
                     text_detail = dwMain.GetItemString(1, "text_detail");
                 }
                 catch
                 {
                     text_detail = " ";
                 }

                 try
                 {
                     remark = dwMain.GetItemString(1, "remark");
                 }
                 catch
                 {
                     remark = " ";
                 }

                 try
                 {
                     bank_code = dwMain.GetItemString(1, "bank_code");
                 }
                 catch
                 {
                     bank_code = " ";
                 }
                 if (memo_type == "02")
                 {
                     bank_code = " ";
                     money_type = " ";
                 }
                 if (money_type == "01")
                 {
                     bank_code = " ";
                    
                 }
                 if (memo_type == "03")
                 {
                     bank_code = " ";
                     money_type = " ";
                 }

                 MEMO_docno = new DocumentControl().NewDocumentNo("WCMEMODOCNO", DateTime.Today.Year + 543, ta);

                String SQLMemo = @"insert into CMMEMODETAIL
                                (memo_id, MEMO_DATE, BRANCH_ID
                                , ENTRY_ID, entry_date, memo_detail, memo_type, money_type,
                                memo_count, bank_code, REMARK, CS_TYPE, text_detail)
                                values('" + MEMO_docno + "',to_date('" + eDateEN + "', 'dd/mm/yyyy'), '" + branch_id +
                                @"', '" + Username + "', sysdate, '" + memo_detail + "', '" + memo_type + "', '" + money_type +
                                @"', '" + memo_cnt + "', '" + bank_code + "', '" + remark + "', '" + cs_type + "', '" + text_detail + "')";
                    ta.Exe(SQLMemo);
                    


                    //}
                
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Edit_bank_branch(String XmlDwIn, string pbl, string Username, string cs_type ,string branch_id) //เพิ่มแก้ไข บัญชีธนาคารศูนย์ by tar 13/01/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore DwIn = new DataStore(pbl, "d_wc_edit_bank");
                DwIn.ImportString(XmlDwIn, FileSaveAsType.Xml);

                for (int i = 1; i <= DwIn.RowCount; i++)
                {
                    string account_name = DwIn.GetItemString(i, "account_name");
                    string bank_name = DwIn.GetItemString(i, "bank_name");
                    string bank_branch = DwIn.GetItemString(i, "bank_branch");
                    string account_no = DwIn.GetItemString(i, "account_no");
                    string accno = account_no.Replace("-","");
                    if (accno.Length != 10)
                    {
                        throw new Exception("  ไม่สามารถบันทึกได้ เลขที่บัญชีเกินหรือไม่ครบ 10 หลัก  ");
                    }
                    String SQLtrn = @"update cmucfcoopbranch set account_name = '" + account_name + "',bank_name = '" + bank_name + "',bank_branch = '" + bank_branch + "',account_no =  '" + accno + "' where coopbranch_id =  '" + branch_id + "'  and cs_type = '" + cs_type + "'";
                    ta.Exe(SQLtrn);


                }



                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {

                ta.RollBack();
                ta.Close();
                throw ex;
                //throw new Exception("กรุณาเลือกสถานะอนุมัติและกรอกเลขสมาชิก สอ.ใหม่ ก่อนทำการกดบันทึก");

            }
            return true;
        }

        public Boolean TrnMemb_Out(String XmlMain, string pbl, string Username, string oldbranch_id, string cs_type) //แจ้งย้ายออก by tar 8/12/2014
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_trn_memb_new");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                for (int i = 0; i < dwMain.RowCount; i++)
                {
     
                    string branch_id = dwMain.GetItemString(i + 1, "branch_id");
                    string deptaccount_no = dwMain.GetItemString(i + 1, "deptaccount_no");

                    String SQLreqNo = "select deptrequest_docno from wcreqdeposit where deptaccount_no = '" + deptaccount_no + "'";
                    Sdt dtReqNo = ta.Query(SQLreqNo);
                    string deptrequest_docno = "";
                    if (dtReqNo.Next())
                    {
                        deptrequest_docno = dtReqNo.GetString("deptrequest_docno");
                    }

                    string trn_docno = new DocumentControl().NewDocumentNo("WCTRNMEMB", DateTime.Today.Year + 543, ta);
                    ///insert report Transfer
                    String SQLtrn = @"insert into wctransfermember(trn_docno, deptaccount_no, entry_id, entry_date, old_branch_id, new_branch_id,trn_status)
                                    values('" + trn_docno + "', '" + deptaccount_no + "', '" + Username + "', to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                        + "', 'dd/mm/yyyy HH24:mi:ss'), '" + oldbranch_id + "', '" + branch_id + "',9)";

                  
                    ta.Exe(SQLtrn);
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean TrnMemb_In(String XmlDwIn, string pbl, string Username, string cs_type) //แจ้งย้ายเข้า by tar 11/12/2014
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore DwIn = new DataStore(pbl, "d_wc_trn_in_new");
                DwIn.ImportString(XmlDwIn, FileSaveAsType.Xml);
                int count = 0;
                for (int i = 1; i <= DwIn.RowCount; i++)
                {
                    decimal trn_status = DwIn.GetItemDecimal(i, "trn_status");
                    string trn_docno = DwIn.GetItemString(i, "trn_docno");
                    string new_memb_no = DwIn.GetItemString(i, "new_memb_no");

                    if (trn_status == 8 && new_memb_no != null)
                    {
                        String SQLtrn = @"update wctransfermember set trn_status = '" + trn_status + "',new_mem_no = '" + new_memb_no + "',entry_id_in = '" + Username + "',entry_date_in = sysdate where trn_docno =  '" + trn_docno + "' ";
                        ta.Exe(SQLtrn);
                        count++;
                    }
                  
                }
                if (count == 0) {
                    ta.RollBack();
                    ta.Close();
                    throw new Exception("กรุณาเลือกสถานะอนุมัติ ก่อนทำการกดบันทึก");
                    
                }
              ta.Commit();
               ta.Close();
            }
            catch (Exception ex)
            {

                ta.RollBack();
                ta.Close();
                throw new Exception("กรุณาเลือกสถานะอนุมัติและกรอกเลขสมาชิก สอ.ใหม่ ก่อนทำการกดบันทึก");
                throw ex;
                
                
            }
            return true;
        }

        public Boolean TrnMemb_ChgBranch_confirm(String XmlMain, string pbl, string Username, string cs_type, DateTime deptopen_date, decimal prncbal)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_trn_admin_confirm");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                int count = 0;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    decimal trn_status = dwMain.GetItemDecimal(i + 1, "trn_status");
                    if (trn_status == 1)
                    {
                        count++;
                        //string wftype_code = dwMain.GetItemString(1, "wftype_code");
                        string trn_docno = dwMain.GetItemString(i + 1, "trn_docno");

                        string branch_id = dwMain.GetItemString(i + 1, "coopbranch_id_new");
                        string oldbranch_id = dwMain.GetItemString(i + 1, "coopbranch_id");
                        string new_memb_no = dwMain.GetItemString(i + 1, "new_mem_no");
                        string deptaccount_no = dwMain.GetItemString(i + 1, "deptaccount_no");

                        String SQLreqNo = "select deptrequest_docno from wcreqdeposit where deptaccount_no = '" + deptaccount_no + "'";
                        Sdt dtReqNo = ta.Query(SQLreqNo);
                        string deptrequest_docno = "";
                        if (dtReqNo.Next())
                        {
                            deptrequest_docno = dtReqNo.GetString("deptrequest_docno");
                        }

                        ///insert New master
                        ///

                        String SqlCreateMaster = @"insert into wcdeptmaster(deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, member_no, prename_code, wfaccount_name,
                                    deptaccount_name, deptaccount_sname, deptclose_status, card_person, prncbal, laststmseq_no, lastaccess_date, last_process_date,
                                    membgroup_code, wfmember_status, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, phone,
                                    deptrequest_docno, apply_date, branch_id, wpf_amt)
                                    select deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, '" + new_memb_no + @"', 
                                    prename_code, wfaccount_name,deptaccount_name, deptaccount_sname, 0, card_person, prncbal, laststmseq_no, to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss') 
                                    , to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss'), membgroup_code, 1, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, 
                                    phone, '" + deptrequest_docno + "', apply_date,'" + branch_id + "', " + prncbal + " from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                        ta.Exe(SqlCreateMaster);

                        ///update codeposit --> branch_id
                        String SQLcodept = @"update wccodeposit set branch_id = '" + branch_id + "' where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                        ta.Exe(SQLcodept);

                        ///update statement --> branch_id
                        String SQLstatement = @"update wcdeptstatement set branch_id = '" + branch_id + "' where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                        ta.Exe(SQLstatement);

                        ///update wcreqchg_dept --> branch_id,member_no
                        String SQLchg = @"update wcreqchg_dept set branch_id = '" + branch_id + "',member_no = '" + new_memb_no + "'  where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                        ta.Exe(SQLchg);

                        

                        ///delete old master
                        String SQLdelOldMaster = @"delete from wcdeptmaster where branch_id = '" + oldbranch_id + "' and deptaccount_no = '" + deptaccount_no + "'";
                        ta.Exe(SQLdelOldMaster);


                        ///update report Transfer
                        String SQLtrn = @"update wctransfermember set trn_status = '" + trn_status + "',confirm_id = '" + Username + "',confirm_date = sysdate where trn_docno =  '" + trn_docno + "' ";

                        ///update recievemonth where status_post = 8
                        String SQLRecvMonth = "update wcrecievemonth set branch_id = '" + branch_id + "' where wfmember_no = '" + deptaccount_no +
                                                "' and branch_id = '" + oldbranch_id + "'";
                        ta.Exe(SQLRecvMonth);

                        ta.Exe(SQLtrn);
                    }
                }

                if (count == 0)
                {
                    ta.RollBack();
                    ta.Close();
                    throw new Exception("กรุณาเลือกสถานะอนุมัติ ก่อนทำการกดบันทึก");

                }

                    ta.Commit();
                    ta.Close();
                
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Edit_deptacc_no(string branch_id, string cs_type) //แก้เลขณาปนกิจซ้ำ by tar 04/06/2015
        {
            String  update_accno = "", SeleLastAcc = "";
            Decimal  up_acc = 0, SeleLastAccInt = 0;
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                String SqlSeleLastAcc = @"select last_documentno from cmshrlondoccontrol where trim(document_code) = 'WFGENMEMNO' ";
                Sdt LastAccSelect = ta.Query(SqlSeleLastAcc);

                if (LastAccSelect.Next())
                {
                    SeleLastAcc = LastAccSelect.GetString("last_documentno");
                    SeleLastAccInt = Convert.ToInt32(SeleLastAcc);
                    
                }

                String sqlselect = @"select * from wcdeptmaster 
                                    where deptaccount_no in (
									                        select deptaccount_no from wcdeptmaster
                                                            where branch_id in (select coopbranch_id from cmucfcoopbranch where cs_type = '" + cs_type + @"')
									                        and DEPTCLOSE_STATUS <> -9  group by deptaccount_no
									                        having   count(deptaccount_no)  > 1)
                                    order by deptaccount_no";
                Sdt dtSelect = ta.Query(sqlselect);
                int i = 1;
                while (dtSelect.Next())
                {
                    String DEPTACCOUNT_NO = String.Empty,
                           BRANCH_ID = String.Empty,
                           WFTYPE_CODE = String.Empty,
                           DEPTTYPE_CODE = String.Empty,
                           MEMBER_NO = String.Empty,
                           PRENAME_CODE = String.Empty,
                           WFACCOUNT_NAME = String.Empty,
                           DEPTACCOUNT_NAME = String.Empty,
                           DEPTACCOUNT_SNAME = String.Empty,
                           DEPT_OBJECTIVE = String.Empty,
                           DEPTPASSBOOK_NO = String.Empty,
                           CONDFORWITHDRAW = String.Empty,
                           CARD_PERSON = String.Empty,
                           MEMBGROUP_CODE = String.Empty,
                           REMARK = String.Empty,
                           WFDEPTACCOUNT_NO = String.Empty,
                           SEX = String.Empty,
                           MATE_NAME = String.Empty,
                           CARREER = "accnoChg",
                           MEMBER_TYPE = String.Empty,
                           CONTACT_ADDRESS = String.Empty,
                           AMPHER_CODE = String.Empty,
                           PROVINCE_CODE = String.Empty,
                           POSTCODE = String.Empty,
                           PHONE = String.Empty,
                           EXPENSE_CODE = String.Empty,
                           EXPENSE_BRANCH = String.Empty,
                           EXPENSE_ACCID = String.Empty,
                           RESIGNCAUSE_CODE = String.Empty,
                           DEPTREQUEST_DOCNO = String.Empty,
                           KPRECV_PERIOD = String.Empty,
                           KEEPMEM_NO = String.Empty,
                           MANAGE_CORPSE_NAME = String.Empty,
                           OTHER_CONTACT_ADDRESS = String.Empty,
                           OTHER_AMPHER_CODE = String.Empty,
                           OTHER_PROVINCE_CODE = String.Empty,
                           OTHER_POSTCODE = String.Empty;
                    
                    Decimal DEPTCLOSE_STATUS = 0,
                            DEPTMONTH_STATUS = 0,
                            DEPTMONTH_AMT = 0,
                            MONTHINT_STATUS = 0,
                            MONTHINTPAY_METH = 0,
                            SPCINT_RATE_STATUS = 0,
                            SPCINT_RATE = 0,
                            BEGINBAL = 0,
                            PRNCBAL = 0,
                            ACCUINT_AMT = 0,
                            INTARREAR_AMT = 0,
                            ACCUINTPAY_AMT = 0,
                            ACCUTAXPAY_AMT = 0,
                            LASTREC_NO_CARD = 0,
                            LASTPAGE_NO_CARD = 0,
                            LASTLINE_NO_CARD = 0,
                            WFMEMBER_STATUS = 0,
                            WFCARCASS_SEQ = 0,
                            APPLYFEE_STATUS = 0,
                            WITHDRAWABLE_AMT = 0,
                            CREMATEAPP_AMT = 0,
                            DIE_STATUS = 0,
                            KPRECEIVE_STATUS = 0,
                            WFPRINCIPAL_ARREAR = 0,
                            MARIAGE_STATUS = 0,
                            MISDEPTTKB_BALANCE = 0,
                            MISDEPTKP_BALANCE = 0,
                            WFE_AMT = 0,
                            WFE_AMT2 = 0,
                            FEE_AMT = 0,
                            WPF_AMT = 0,
                            WFY_AMT = 0,
                            FOREIGNER_FLAG = 0,
                            LASTSTMSEQ_NO = 0;
                    
                    DateTime DEPTOPEN_DATE = DateTime.MinValue,
                             DEPTCLOSE_DATE = DateTime.MaxValue,
                             LASTCALINT_DATE = DateTime.MinValue,
                             LASTACCESS_DATE = DateTime.MinValue,
                             LAST_PROCESS_DATE = DateTime.MinValue,
                             DIE_DATE = DateTime.MinValue,
                             MBCHGTYPE_DATE = DateTime.MinValue,
                             WFBIRTHDAY_DATE = DateTime.MinValue,
                             APPLY_DATE = DateTime.MinValue,
                             APPROVE_DATE = DateTime.MinValue,
                             EFFECTIVE_DATE = DateTime.MinValue;

                    DEPTACCOUNT_NO = dtSelect.GetString("deptaccount_no");
                    try { WFTYPE_CODE = dtSelect.GetString("WFTYPE_CODE"); } catch { WFTYPE_CODE = ""; }
                    DEPTTYPE_CODE = dtSelect.GetString("DEPTTYPE_CODE");
                    DEPTOPEN_DATE = dtSelect.GetDate("DEPTOPEN_DATE");
                    MEMBER_NO = dtSelect.GetString("MEMBER_NO");
                    PRENAME_CODE = dtSelect.GetString("PRENAME_CODE");
                    WFACCOUNT_NAME = dtSelect.GetString("WFACCOUNT_NAME");
                    DEPTACCOUNT_NAME = dtSelect.GetString("DEPTACCOUNT_NAME");
                    DEPTACCOUNT_SNAME = dtSelect.GetString("DEPTACCOUNT_SNAME");
                    DEPTCLOSE_STATUS = dtSelect.GetDecimal("DEPTCLOSE_STATUS");
                    DEPTCLOSE_DATE = dtSelect.GetDate("DEPTCLOSE_DATE");
                    //DEPT_OBJECTIVE = dtSelect.GetString("DEPT_OBJECTIVE");
                    //DEPTPASSBOOK_NO = dtSelect.GetString("DEPTPASSBOOK_NO");
                    //CONDFORWITHDRAW = dtSelect.GetString("CONDFORWITHDRAW");
                    CARD_PERSON = dtSelect.GetString("CARD_PERSON");
                    //DEPTMONTH_STATUS = dtSelect.GetDecimal("DEPTMONTH_STATUS");
                    //DEPTMONTH_AMT = dtSelect.GetDecimal("DEPTMONTH_AMT");
                    //MONTHINT_STATUS = dtSelect.GetDecimal("MONTHINT_STATUS");
                    //MONTHINTPAY_METH = dtSelect.GetDecimal("MONTHINTPAY_METH");
                    //SPCINT_RATE_STATUS = dtSelect.GetDecimal("SPCINT_RATE_STATUS");
                    //SPCINT_RATE = dtSelect.GetDecimal("SPCINT_RATE");
                    //BEGINBAL = dtSelect.GetDecimal("BEGINBAL");
                    PRNCBAL = dtSelect.GetDecimal("PRNCBAL");
                    //LASTCALINT_DATE = dtSelect.GetDate("LASTCALINT_DATE");
                    //ACCUINT_AMT = dtSelect.GetDecimal("ACCUINT_AMT");
                    //INTARREAR_AMT = dtSelect.GetDecimal("INTARREAR_AMT");
                    //ACCUINTPAY_AMT = dtSelect.GetDecimal("ACCUINTPAY_AMT");
                    //ACCUTAXPAY_AMT = dtSelect.GetDecimal("ACCUTAXPAY_AMT");
                    //LASTSTMSEQ_NO = dtSelect.GetDecimal("LASTSTMSEQ_NO");
                   // LASTREC_NO_CARD = dtSelect.GetDecimal("LASTREC_NO_CARD");
                    //LASTPAGE_NO_CARD = dtSelect.GetDecimal("LASTPAGE_NO_CARD");
                    //LASTLINE_NO_CARD = dtSelect.GetDecimal("LASTLINE_NO_CARD");
                    LASTACCESS_DATE = dtSelect.GetDate("LASTACCESS_DATE");
                    LAST_PROCESS_DATE = dtSelect.GetDate("LAST_PROCESS_DATE");
                    MEMBGROUP_CODE = dtSelect.GetString("MEMBGROUP_CODE");
                    WFMEMBER_STATUS = dtSelect.GetDecimal("WFMEMBER_STATUS");
                    REMARK = dtSelect.GetString("REMARK");
                    DIE_DATE = dtSelect.GetDate("DIE_DATE");
                    //WFDEPTACCOUNT_NO = dtSelect.GetString("WFDEPTACCOUNT_NO");
                    //WFCARCASS_SEQ = dtSelect.GetDecimal("WFCARCASS_SEQ");
                    SEX = dtSelect.GetString("SEX");
                    MATE_NAME = dtSelect.GetString("MATE_NAME");
                    //MBCHGTYPE_DATE = dtSelect.GetDate("MBCHGTYPE_DATE");
                    //CARREER = dtSelect.GetString("CARREER");
                    WFBIRTHDAY_DATE = dtSelect.GetDate("WFBIRTHDAY_DATE");
                    MEMBER_TYPE = dtSelect.GetString("MEMBER_TYPE");
                    APPLYFEE_STATUS = dtSelect.GetDecimal("APPLYFEE_STATUS");
                    CONTACT_ADDRESS = dtSelect.GetString("CONTACT_ADDRESS");
                    AMPHER_CODE = dtSelect.GetString("AMPHER_CODE");
                    PROVINCE_CODE = dtSelect.GetString("PROVINCE_CODE");
                    POSTCODE = dtSelect.GetString("POSTCODE");
                    PHONE = dtSelect.GetString("PHONE");
                    EXPENSE_CODE = dtSelect.GetString("EXPENSE_CODE");
                    EXPENSE_BRANCH = dtSelect.GetString("EXPENSE_BRANCH");
                    EXPENSE_ACCID = dtSelect.GetString("EXPENSE_ACCID");
                    RESIGNCAUSE_CODE = dtSelect.GetString("RESIGNCAUSE_CODE");
                    DEPTREQUEST_DOCNO = dtSelect.GetString("DEPTREQUEST_DOCNO");
                    APPLY_DATE = dtSelect.GetDate("APPLY_DATE");
                    WITHDRAWABLE_AMT = dtSelect.GetDecimal("WITHDRAWABLE_AMT");
                    //CREMATEAPP_AMT = dtSelect.GetDecimal("CREMATEAPP_AMT");
                    DIE_STATUS = dtSelect.GetDecimal("DIE_STATUS");
                    //KPRECEIVE_STATUS = dtSelect.GetDecimal("KPRECEIVE_STATUS");
                    //KPRECV_PERIOD = dtSelect.GetString("KPRECV_PERIOD");
                    //WFPRINCIPAL_ARREAR = dtSelect.GetDecimal("WFPRINCIPAL_ARREAR");
                    MARIAGE_STATUS = dtSelect.GetDecimal("MARIAGE_STATUS");
                    //MISDEPTTKB_BALANCE = dtSelect.GetDecimal("MISDEPTTKB_BALANCE");
                    //MISDEPTKP_BALANCE = dtSelect.GetDecimal("MISDEPTKP_BALANCE");
                    BRANCH_ID = dtSelect.GetString("BRANCH_ID");
                    APPROVE_DATE = dtSelect.GetDate("APPROVE_DATE");
                    //WFE_AMT = dtSelect.GetDecimal("WFE_AMT");
                    //WFE_AMT2 = dtSelect.GetDecimal("WFE_AMT2");
                    FEE_AMT = dtSelect.GetDecimal("FEE_AMT");
                    WPF_AMT = dtSelect.GetDecimal("WPF_AMT");
                    WFY_AMT = dtSelect.GetDecimal("WFY_AMT");
                    //KEEPMEM_NO = dtSelect.GetString("KEEPMEM_NO");
                    MANAGE_CORPSE_NAME = dtSelect.GetString("MANAGE_CORPSE_NAME");
                    EFFECTIVE_DATE = dtSelect.GetDate("EFFECTIVE_DATE");
                    //FOREIGNER_FLAG = dtSelect.GetDecimal("FOREIGNER_FLAG");
                    OTHER_CONTACT_ADDRESS = dtSelect.GetString("OTHER_CONTACT_ADDRESS");
                    OTHER_AMPHER_CODE = dtSelect.GetString("OTHER_AMPHER_CODE");
                    OTHER_PROVINCE_CODE = dtSelect.GetString("OTHER_PROVINCE_CODE");
                    OTHER_POSTCODE = dtSelect.GetString("OTHER_POSTCODE");




                    
                    
                    
                    
                    
                    up_acc = SeleLastAccInt + i;
                    update_accno = up_acc.ToString();

                    //insert CARREER
                   String sqlInsertMaster = @"insert into WCDEPTMASTER(DEPTACCOUNT_NO, WFTYPE_CODE, DEPTTYPE_CODE, DEPTOPEN_DATE, MEMBER_NO
                                                                      ,PRENAME_CODE, WFACCOUNT_NAME, DEPTACCOUNT_NAME, DEPTACCOUNT_SNAME,DEPTCLOSE_STATUS
                                                                      ,DEPTCLOSE_DATE, CARD_PERSON, PRNCBAL, LASTACCESS_DATE, LAST_PROCESS_DATE
                                                                      ,MEMBGROUP_CODE, WFMEMBER_STATUS, REMARK, DIE_DATE, SEX 
                                                                      ,MATE_NAME, CARREER, WFBIRTHDAY_DATE, MEMBER_TYPE, APPLYFEE_STATUS, CONTACT_ADDRESS
                                                                      ,AMPHER_CODE, PROVINCE_CODE, POSTCODE, PHONE, EXPENSE_CODE
                                                                      ,EXPENSE_BRANCH, EXPENSE_ACCID, RESIGNCAUSE_CODE, DEPTREQUEST_DOCNO, APPLY_DATE
                                                                      ,WITHDRAWABLE_AMT, DIE_STATUS, MARIAGE_STATUS, BRANCH_ID, APPROVE_DATE
                                                                      ,FEE_AMT, WPF_AMT, WFY_AMT, MANAGE_CORPSE_NAME, EFFECTIVE_DATE 
                                                                      ,OTHER_CONTACT_ADDRESS, OTHER_AMPHER_CODE, OTHER_PROVINCE_CODE, OTHER_POSTCODE       
                                                          )values('" + update_accno + "', '" + WFTYPE_CODE + "', '" + DEPTTYPE_CODE + "', to_date('" + DEPTOPEN_DATE.ToString("ddMMyyyy") + "', 'ddmmyyyy'), '" + MEMBER_NO + @"'
                                                                 ,'" + PRENAME_CODE + "', '" + WFACCOUNT_NAME + "', '" + DEPTACCOUNT_NAME + "', '" + DEPTACCOUNT_SNAME + "', '" + DEPTCLOSE_STATUS + @"'  
                                                                 ,to_date('" + DEPTCLOSE_DATE.ToString("ddMMyyyy") + "', 'ddmmyyyy'), '" + CARD_PERSON + "', '" + PRNCBAL + "', to_date('" + LASTACCESS_DATE.ToString("ddMMyyyy") + "', 'ddmmyyyy'), to_date('" + LAST_PROCESS_DATE.ToString("ddMMyyyy") + @"', 'ddmmyyyy')
                                                                 ,'" + MEMBGROUP_CODE + "', '" + WFMEMBER_STATUS + "', '" + REMARK + "', to_date('" + DIE_DATE.ToString("ddMMyyyy") + "', 'ddmmyyyy') , '" + SEX + @"'
                                                                 ,'" + MATE_NAME + "', '" + CARREER + "', to_date('" + WFBIRTHDAY_DATE.ToString("ddMMyyyy") + "', 'ddmmyyyy'), '" + MEMBER_TYPE + "', '" + APPLYFEE_STATUS + "', '" + CONTACT_ADDRESS + @"'
                                                                 ,'" + AMPHER_CODE + "', '" + PROVINCE_CODE + "', '" + POSTCODE + "', '" + PHONE + "', '" + EXPENSE_CODE + @"'
                                                                 ,'" + EXPENSE_BRANCH + "', '" + EXPENSE_ACCID + "', '" + RESIGNCAUSE_CODE + "', '" + DEPTREQUEST_DOCNO + "',to_date('" + APPLY_DATE.ToString("ddMMyyyy") + @"', 'ddmmyyyy')
                                                                 ,'" + WITHDRAWABLE_AMT + "', '" + DIE_STATUS + "', '" + MARIAGE_STATUS + "', '" + BRANCH_ID + "', to_date('" + APPROVE_DATE.ToString("ddMMyyyy") + @"', 'ddmmyyyy')
                                                                 ,'" + FEE_AMT + "', '" + WPF_AMT + "', '" + WFY_AMT + "', '" + MANAGE_CORPSE_NAME + "', to_date('" + EFFECTIVE_DATE.ToString("ddMMyyyy") + @"', 'ddmmyyyy')
                                                                 ,'" + OTHER_CONTACT_ADDRESS + "', '" + OTHER_AMPHER_CODE + "', '" + OTHER_PROVINCE_CODE + "', '" + OTHER_POSTCODE + @"'
                                                                )";
                   ta.Exe(sqlInsertMaster);


                   if (DEPTCLOSE_STATUS == 0)
                   {
                       // update DEPTCLOSE_DATE = null, DIE_DATE = NULL
                       String sqlUpdateMaster = @"update wcdeptmaster set DEPTCLOSE_DATE = null,DIE_DATE = null    where deptaccount_no = '" + update_accno + "' and branch_id = '" + BRANCH_ID + "'";
                       ta.Exe(sqlUpdateMaster);
                   }
                   if (APPROVE_DATE == DateTime.MinValue)
                   {
                        String sqlUpdateMaster1 = @"update wcdeptmaster set APPROVE_DATE = null where deptaccount_no = '" + update_accno + "' and branch_id = '" + BRANCH_ID + "'";
                        ta.Exe(sqlUpdateMaster1);

                   }

                   String sqlUpStm = @"update wcdeptstatement set deptaccount_no = '" + update_accno + "'   where deptaccount_no = '" + DEPTACCOUNT_NO + "' and branch_id = '" + BRANCH_ID + "'";
                   ta.Exe(sqlUpStm);

                   String sqlUpCodeposit = @"update wccodeposit set deptaccount_no = '" + update_accno + "' where deptaccount_no = '" + DEPTACCOUNT_NO + "' and branch_id = '" + BRANCH_ID + "'";
                   ta.Exe(sqlUpCodeposit);

                   String sqlUpRcvMon = @"update wcrecievemonth set wfmember_no = '" + update_accno + "' where wfmember_no = '" + DEPTACCOUNT_NO + "' and branch_id = '" + BRANCH_ID + "'";
                   ta.Exe(sqlUpRcvMon);

                   String sqlUpDeptSlip = @"update wcdeptslip set deptaccount_no = '" + update_accno + "' where deptaccount_no = '" + DEPTACCOUNT_NO + "' and branch_id = '" + BRANCH_ID + "'";
                   ta.Exe(sqlUpDeptSlip);

                   String sqlUpOldAccNo = @"update wcdeptmaster set DEPTCLOSE_STATUS = -9, WFMEMBER_STATUS = -1, RESIGNCAUSE_CODE = '03' where deptaccount_no = '" + DEPTACCOUNT_NO + "' and branch_id = '" + BRANCH_ID + "'";
                   ta.Exe(sqlUpOldAccNo);

                    i++;
                }
                String sqlUpDoccontrol = @"update cmshrlondoccontrol set last_documentno = '" + update_accno + "' where trim(document_code) = 'WFGENMEMNO' ";
                ta.Exe(sqlUpDoccontrol);

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {

                ta.RollBack();
                ta.Close();
                throw ex;


            }
            return true;
        }

        public Boolean TrnMemb_ChgBranch(String XmlMain, string pbl, string Username, string oldbranch_id, string cs_type, DateTime deptopen_date, decimal prncbal)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_trn_memb");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    string wftype_code = dwMain.GetItemString(1, "wftype_code");
                    string branch_id = dwMain.GetItemString(i + 1, "branch_id");
                    //string new_memb_no = int.Parse(dwMain.GetItemString(i + 1, "new_memb_no")).ToString("000000");
                    string new_memb_no = dwMain.GetItemString(i + 1, "new_memb_no");
                    string deptaccount_no = dwMain.GetItemString(i + 1, "deptaccount_no");

                    String SQLreqNo = "select deptrequest_docno from wcreqdeposit where deptaccount_no = '" + deptaccount_no + "'";
                    Sdt dtReqNo = ta.Query(SQLreqNo);
                    string deptrequest_docno = "";
                    if (dtReqNo.Next())
                    {
                        deptrequest_docno = dtReqNo.GetString("deptrequest_docno");
                    }

                    ///insert New master
                    ///

                    String SqlCreateMaster = @"insert into wcdeptmaster(deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, member_no, prename_code, wfaccount_name,
                                    deptaccount_name, deptaccount_sname, deptclose_status, card_person, prncbal, laststmseq_no, lastaccess_date, last_process_date,
                                    membgroup_code, wfmember_status, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, phone,
                                    deptrequest_docno, apply_date, branch_id, wpf_amt)
                                    select deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, '" + new_memb_no + @"', 
                                    prename_code, wfaccount_name,deptaccount_name, deptaccount_sname, 0, card_person, prncbal, laststmseq_no, to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss') 
                                    , to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"', 'dd/mm/yyyy HH24:mi:ss'), membgroup_code, 1, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, 
                                    phone, '" + deptrequest_docno + "', apply_date,'" + branch_id + "', " + prncbal + " from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                    ta.Exe(SqlCreateMaster);

                    ///update codeposit --> branch_id
                    String SQLcodept = @"update wccodeposit set branch_id = '" + branch_id + "' where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                    ta.Exe(SQLcodept);

                    ///update statement --> branch_id
                    String SQLstatement = @"update wcdeptstatement set branch_id = '" + branch_id + "' where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                    ta.Exe(SQLstatement);

                    ///update wcreqchg_dept --> branch_id,member_no
                    String SQLchg = @"update wcreqchg_dept set branch_id = '" + branch_id + "',member_no = '" + new_memb_no + "'  where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + oldbranch_id + "'";
                    ta.Exe(SQLchg);

                    ///delete old master
                    String SQLdelOldMaster = @"delete from wcdeptmaster where branch_id = '" + oldbranch_id + "' and deptaccount_no = '" + deptaccount_no + "'";
                    ta.Exe(SQLdelOldMaster);

                    string trn_docno = new DocumentControl().NewDocumentNo("WCTRNMEMB", DateTime.Today.Year + 543, ta);

                    if (cs_type == "8")
                    {
                        ///insert report Transfer
                        String SQLtrn = @"insert into wctransfermember(trn_docno, deptaccount_no, entry_id, entry_date, old_branch_id, new_branch_id,trn_status,entry_id_in,new_mem_no,entry_date_in,confirm_id,confirm_date)
                                    values('" + trn_docno + "', '" + deptaccount_no + "', '" + Username + "', to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                            + "', 'dd/mm/yyyy HH24:mi:ss'), '" + oldbranch_id + "', '" + branch_id + "',1, '" + Username + "', '" + new_memb_no + "',sysdate, '" + Username + "',sysdate)";
                        ta.Exe(SQLtrn);
                    }
                    else
                    {
                        ///insert report Transfer
                        String SQLtrn = @"insert into wctransfermember(trn_docno, deptaccount_no, entry_id, entry_date, old_branch_id, new_branch_id)
                                    values('" + trn_docno + "', '" + deptaccount_no + "', '" + Username + "', to_date('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                            + "', 'dd/mm/yyyy HH24:mi:ss'), '" + oldbranch_id + "', '" + branch_id + "')";

                        ta.Exe(SQLtrn);
                    }
                    ///update recievemonth where status_post = 8
                    String SQLRecvMonth = "update wcrecievemonth set branch_id = '" + branch_id + "' where wfmember_no = '" + deptaccount_no +
                                            "' and branch_id = '" + oldbranch_id + "'";
                    ta.Exe(SQLRecvMonth);

                    
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean Add_FeeYear(String XmlMain, string pbl, string Username, string cs_type, string branch_id) //เพิ่มรายการเรียกเก็บ by tar 19/01/2015
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_feeyearlast_add");
                dwMain.ImportString(XmlMain, FileSaveAsType.Xml);
                int count = 0;
                for (int i = 1; i <= dwMain.RowCount; i++)
                {
                    string add_status = dwMain.GetItemString(i, "add_status");
                    if (add_status == "1")
                    {
                        count++;
                        string deptaccount_no = dwMain.GetItemString(i, "deptaccount_no");
                        string member_no = dwMain.GetItemString(i, "member_no");
                        string wfaccount_name = dwMain.GetItemString(i, "wfaccount_name");
                        DateTime operate_date = dwMain.GetItemDate(i, "operate_date");
                        string recv_period = dwMain.GetItemString(i, "recv_period");
                        string item_amt = dwMain.GetItemString(i, "item_amt");
                        
                        string depttype_code = dwMain.GetItemString(i, "depttype_code");
                        string wcitemtype_code = "FEE";
                        ///insert New master
                        ///

                        String SqlCreateFee = @"insert into wcrecievemonth(wfmember_no, member_no, recv_period, depttype_code, operate_date, status_post, fee_year, wfaccount_name,branch_id, wcitemtype_code)
                                    values('" + deptaccount_no + "', '" + member_no + "', '" + recv_period + "', '" + depttype_code + "', to_date('" + operate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),8,'" + item_amt + "','" + wfaccount_name + "','" + branch_id + "','" + wcitemtype_code + "')";
                                    
                        ta.Exe(SqlCreateFee);

                       
                    }
                }

                if (count == 0)
                {
                    ta.RollBack();
                    ta.Close();
                    throw new Exception("กรุณาเลือกสถานะอนุมัติ ก่อนทำการกดบันทึก");

                }

                ta.Commit();
                ta.Close();

            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean SaveGroupPaidCancel(string pbl, String xmlMain, string entry_id, string branch_id, DateTime inform_date, int period)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_paid_group_cancel");
                dwMain.ImportString(xmlMain, FileSaveAsType.Xml);
                string ReqchgNo, deptaccount_no, today, die_date, Sinform_date, resigncause_code, member_no, wftype_code, remark;
                String SqlChkResign = "", SqlUpdateResign = "";
                Sdt dtChkRsing;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    if (dwMain.GetItemDecimal(i + 1, "status") == 1)
                    {
                        deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                        ReqchgNo = new DocumentControl().NewDocumentNo("WCCHGDOCNO", DateTime.Today.Year + 543, ta);
                        today = "'" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        die_date = "'" + inform_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        Sinform_date = "'" + inform_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        resigncause_code = "04";

                        Decimal approve_flag = 1;
                        member_no = dwMain.GetItemString(1, "member_no").Trim();
                        decimal quantitymem_amt = 0;
                        Sdt dt = ta.Query("select count(*) as quantitymem_amt from wcdeptmaster where deptclose_status = 0");
                        if (dt.Next())
                        {
                            quantitymem_amt = dt.GetDecimal("quantitymem_amt");
                        }
                        Decimal cremateestpay_amt = 0;
                        wftype_code = dwMain.GetItemString(1, "wftype_code");
                        Decimal withdrawable_amt = 0;
                        Decimal wfe_amt = 0;
                        Decimal wfe_amt2 = 0;
                        decimal die_status = 1;
                        remark = "สิ้นสุดสมาชิกภาพ";

                        SqlChkResign = "select * from wcreqchg_dept where deptaccount_no = '" + deptaccount_no + "' and approve_flag = 0";
                        dtChkRsing = ta.Query(SqlChkResign);
                        if (dtChkRsing.Next())
                        {
                            SqlUpdateResign = "update wcreqchg_dept set approve_flag = " + approve_flag + ", remark = '" + remark + @"',
                                                entry_date = to_date(" + today + ", entry_id = '" + entry_id + @"', die_date = 
                                                to_date(" + die_date + ", inform_date = to_date(" + Sinform_date + @", resigncause_code = 
                                                '" + resigncause_code + "', reqchg_status = 1, quantitymem_amt = " + quantitymem_amt +
                                                ", cremateestpay_amt = " + cremateestpay_amt + ", die_status = " + die_status +
                                                ", withdrawable_amt = " + withdrawable_amt + ", wfe_amt = " + wfe_amt + ", wfe_amt2 = " +
                                                wfe_amt2 + ", approve_date = to_date(" + today + " where deptaccount_no = '" + deptaccount_no + "'";
                            ta.Exe(SqlUpdateResign);
                        }
                        else
                        {
                            String sql = @"INSERT INTO wcreqchg_dept (dpreqchg_doc, deptaccount_no, approve_flag, remark, entry_date, entry_id, die_date, inform_date, 
                                            resigncause_code,";
                            if (approve_flag == 1)
                            {
                                sql = sql + " approve_date,";
                            }
                            sql = sql + " reqchg_status, member_no, quantitymem_amt, cremateestpay_amt, wftype_code, die_status, branch_id, withdrawable_amt, wfe_amt, wfe_amt2)";
                            sql = sql + "VALUES ('" + ReqchgNo + "','" + deptaccount_no + "'," + approve_flag + ",'" + remark + "', to_date(" + today + ",'" + entry_id + 
                                  "', to_date(" + die_date + ", to_date(" + Sinform_date + ",'" + resigncause_code + "',";
                            if (approve_flag == 1)
                            {
                                sql = sql + "to_date(" + today + ",";
                            }
                            sql = sql + "" + 1 + ",'" + member_no + "'," + quantitymem_amt + "," + cremateestpay_amt + ",'" + wftype_code + "'," + die_status + ",'" + 
                                  branch_id + "'," + withdrawable_amt + "," + wfe_amt + "," + wfe_amt2 + ")";

                            try
                            {
                                int req = ta.Exe(sql);
                            }
                            catch
                            {
                                throw new Exception("ไม่สามารถทำการแจ้งเสียชีวิต/ลาออกได้");
                            }
                        }
                        String msql = "update wcdeptmaster set deptclose_status=" + 1 +
                            @", deptclose_date=to_date(" + today +
                            @", lastaccess_date=to_date(" + today +
                            @", last_process_date=to_date(" + today +
                            @", wfmember_status=" + -1 +
                            @", die_date=to_date(" + die_date +
                            //@", wfcarcass_seq='" + quantitymem_amt + "'" +
                            @", resigncause_code='" + resigncause_code + "'";
                        if (approve_flag == 1)
                        {
                            msql = msql + ", apply_date=  to_date(" + today;
                        }
                        msql = msql + @", withdrawable_amt=" + withdrawable_amt +
                        @", cremateapp_amt=" + cremateestpay_amt;
                        if (approve_flag == 1)
                        {
                            msql = msql + ", kpreceive_status=" + 1;
                        }
                        else
                        {
                            msql = msql + ", kpreceive_status=" + -1;
                        }
                        msql = msql + " where deptaccount_no='" + deptaccount_no + "' and branch_id='" + branch_id + "'";
                        try
                        {
                            ta.Exe(msql);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถ Update ทะเบียนได้");
                        }

                        String usql = @"update wcrecievemonth set status_post = -9 where wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "' and wcitemtype_code = 'FEE' and recv_period = " + period;
                        try
                        {
                            ta.Exe(usql);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถเปลี่ยนสถานะได้");
                        }
                    }
                    else if (dwMain.GetItemDecimal(i + 1, "status") == 0)
                    {
                        deptaccount_no = dwMain.GetItemString(i + 1, "wfmember_no");
                        String sql = @"update wcrecievemonth set status_post = 8 where wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "' and wcitemtype_code = 'FEE' and recv_period = " + period;
                        try
                        {
                            ta.Exe(sql);
                        }
                        catch
                        {
                            throw new Exception("ไม่สามารถเปลี่ยนสถานะได้");
                        }
                    }
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean GenStatement(DateTime deptopen_date, decimal prncbal, String groupBranch)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            string pbl = "w_sheet_wc_walfare_paid.pbl";
            this.ToPhycalPathPbl(ref pbl);
            try
            {
                DataStore dwStm = new DataStore(pbl, "d_wf_paid_statement");
                String SQLGetdata = "select * from wcdeptmaster where deptopen_date = to_date('" + deptopen_date.ToString("dd/MM/yyyy") +
                                    "', 'dd/mm/yyyy') and deptaccount_no not in(select deptaccount_no from wcdeptstatement branch_id in(" + groupBranch + @") 
                                    group by deptaccount_no) and branch_id in(" + groupBranch + ")";
                Sdt dtGetdata = ta.Query(SQLGetdata);
                string deptaccount_no, branch_id, Sopen_date;
                decimal[] prncslip_amt = new decimal[3];
                String[] deptitemtype_code = new String[] { "FEE", "WFY", "WPF" };

                int seq_stm;
                prncslip_amt[0] = 20;
                prncslip_amt[1] = 20;
                while (dtGetdata.Next())
                {
                    deptaccount_no = dtGetdata.GetString("deptaccount_no");
                    seq_stm = 1;
                    branch_id = dtGetdata.GetString("branch_id");
                    Sopen_date = dtGetdata.GetDate("deptopen_date").ToString("dd/MM/yyyy");
                    prncslip_amt[2] = prncbal;
                    //switch (Sopen_date)
                    //{
                    //    case "01/01/2012":
                    //        prncslip_amt[2] = 3000;
                    //        break;
                    //    case "01/04/2012":
                    //        prncslip_amt[2] = 2250;
                    //        break;
                    //}
                    for (int i = 1; i <= 3; i++)
                    {
                        dwStm.InsertRow(0);
                        dwStm.SetItemString(i, "deptaccount_no", deptaccount_no);
                        dwStm.SetItemDecimal(i, "seq_no", seq_stm++);
                        dwStm.SetItemString(i, "deptitemtype_code", deptitemtype_code[i - 1]);
                        dwStm.SetItemString(i, "ref_docno", "CNV");
                        dwStm.SetItemDecimal(i, "deptitem_amt", prncslip_amt[i - 1]);
                        dwStm.SetItemString(i, "entry_id", "admin");
                        dwStm.SetItemDateTime(i, "entry_date", DateTime.Now);
                        dwStm.SetItemDateTime(i, "operate_date", DateTime.Today);
                        //dwStm.SetItemDecimal(i, "sign_flag", dwSlip.GetItemDecimal(1, "sign_flag"));
                        dwStm.SetItemString(i, "branch_id", branch_id);

                        ta.Exe(new DwHandle(dwStm).SqlInsertSyntax("WCDEPTSTATEMENT", i));
                    }
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }

            return true;
        }

        public Boolean GenCodept(DateTime deptopen_date, String groupBranch)
        {

            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                String SQLdel = "delete from wccodeposit where deptaccount_no in (select deptaccount_no from wcdeptmaster where deptopen_date = to_date('"
                                    + deptopen_date.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') and branch_id in(" + groupBranch + "))";
                ta.Exe(SQLdel);
                String SQLInsert_Select = "insert into wccodeposit(deptaccount_no, seq_no,\"NAME\", codept_addre, codept_id, branch_id)" +
                                        "select m.deptaccount_no,c.seq_no," + " c.\"NAME\", c.codept_addr, c.codept_id, c.branch_id " +
                                        @"from wcreqcodeposit c left join wcreqdeposit r
                                            on(c.deptrequest_docno = r.deptrequest_docno)  
                                            left join wcdeptmaster m
                                            on(r.deptaccount_no = m.deptaccount_no)
                                        where r.branch_id in(" + groupBranch + @") and c.branch_id in(" + groupBranch + @") and m.branch_id in(" + groupBranch + @")
                                            and m.deptopen_date = to_date('" + deptopen_date.ToString("dd/MM/yyyy") + @"', 'dd/mm/yyyy')
                                            and r.approve_status <> -9";
                ta.Exe(SQLInsert_Select);

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public Boolean StatusMem(string deptaccount_no, DateTime deptclose_tdate, DateTime die_tdate, string resigncause_code, decimal reqchg_status, string branch_id, string period, string user, string cs_type, string dpreqchg_doc, string for_year)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {

                if (reqchg_status != -9)
                {
                    String SQL1 = "UPDATE wcreqchg_dept SET inform_date = to_date('" + deptclose_tdate.ToString("dd/MM/yyyy")
                        + "', 'dd/mm/yyyy'),die_date = to_date('" + die_tdate.ToString("dd/MM/yyyy")
                        + "', 'dd/mm/yyyy'),resigncause_code = '" + resigncause_code + "' "
                        + "WHERE deptaccount_no = '" + deptaccount_no + "' AND branch_id = '" + branch_id + "'";
                    ta.Exe(SQL1);
                    String SQL2 = "UPDATE wcdeptmaster SET deptclose_date = to_date('" + deptclose_tdate.ToString("dd/MM/yyyy")
                        + "', 'dd/mm/yyyy'),die_date = to_date('" + die_tdate.ToString("dd/MM/yyyy")
                        + "', 'dd/mm/yyyy'),resigncause_code = '" + resigncause_code + "' "
                        + "WHERE deptaccount_no = '" + deptaccount_no + "' AND branch_id = '" + branch_id + "'";
                    ta.Exe(SQL2);
                }
                else
                {
                    String SQLChkData = @"select resigncause_code from wcdeptmaster 
                    WHERE deptaccount_no = '" + deptaccount_no + "' AND branch_id = '" + branch_id + "'";
                    Sdt dtchk = ta.Query(SQLChkData);
                    if (dtchk.Next())
                    {
                        if (dtchk.GetString("resigncause_code").Trim() != resigncause_code.Trim())
                        {
                            ta.RollBack();
                            ta.Close();
                            throw new Exception("กรุณาเลือกสาเหตุการออกให้ถูกต้อง");
                        }
                    }

                    String SQL1 = "UPDATE  wcreqchg_dept SET approve_flag = -9,reqchg_status = " + reqchg_status + "  "
                        + "WHERE deptaccount_no = '" + deptaccount_no + "' AND branch_id = '" + branch_id + "'";
                    ta.Exe(SQL1);
                    String SQL2 = "UPDATE wcdeptmaster SET deptclose_status = 0,"
                        + "wfmember_status = 1,die_status= 0,"
                    + " deptclose_date = null, die_date = null,"
                    + "resigncause_code = null,withdrawable_amt= 0,"
                    + "cremateapp_amt=null,kpreceive_status=null,"
                    + "wfe_amt= null,wfe_amt2= null  "
                    + "WHERE deptaccount_no = '" + deptaccount_no + "' AND branch_id = '" + branch_id + "'";
                    ta.Exe(SQL2);
                    String SQL5 = @"INSERT INTO WCMEMBERRETURN(DPREQCHG_DOC,DEPTACCOUNT_NO,RETURN_FLAG,RESIGNCAUSE_CODE,RETURN_FORYEAR
                                                               ,ENTRY_DATE,ENTRY_ID,BRANCH_ID,CS_TYPE) 
                                    VALUES('" + dpreqchg_doc + "','" + deptaccount_no + "',1,'" + resigncause_code + "','" + for_year + @"'
                                           ,SYSDATE,'" + user + "','" + branch_id + "','" + cs_type + "')"; //Tar
                    ta.Exe(SQL5);
                    if (resigncause_code.Trim() == "04" && reqchg_status == -9)
                    {
                        String SQL4 = "SELECT * FROM wcrecievemonth WHERE recv_period = '" + period
                            + "' AND wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                        Sdt dt = ta.Query(SQL4);
                        if (dt.Next())
                        {
                            String SQL3 = "UPDATE wcrecievemonth SET status_post = 8 WHERE recv_period = '" + period
                                + "' AND wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                            ta.Exe(SQL3);
                        }
                        else
                        {
                            ta.RollBack();
                            ta.Close();
                            throw new Exception("ไม่สามารถยกเลิกใบคำขอได้ เนื่องจากระบุเดือนและปีไม่ถูกต้อง");
                        }
                    }
                }
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }

        public bool CancelApprovePaid(string deptaccount_no, string branch_id, string slip_no, string period, string entry_id, decimal slip_amt, string Seffective_date)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();

            try
            {
                String MEffecDate = "update wcdeptmaster set effective_date = to_date('" + Seffective_date + "', 'ddmmyyyy') where deptaccount_no = '" + deptaccount_no
                        + "' and branch_id = '" + branch_id + "'";
                ta.Exe(MEffecDate);

                ///CheckStatement
                String SlStatement = "select * from wcdeptstatement where ref_docno = '"
                    + slip_no + "' and deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                Sdt dtSl = ta.Query(SlStatement);
                if (dtSl.Next())
                {
                    ///Select prncball from master
                    String SMprncBal = "select prncbal from wcdeptmaster where deptaccount_no = '" + deptaccount_no
                        + "' and branch_id = '" + branch_id + "'";
                    Sdt dtSM = ta.Query(SMprncBal);
                    decimal prncbal = 0;
                    if (dtSM.Next())
                    {
                        prncbal = dtSM.GetDecimal("prncbal") - slip_amt;
                    }
                    else
                    {
                        throw new Exception("ไม่สามารถ Get ค่าเงินคงเหลือได้");
                    }
                    ///UpdateMaster
                    String MPrncBal = "update wcdeptmaster set prncbal = " + prncbal + " where deptaccount_no = '" + deptaccount_no
                        + "' and branch_id = '" + branch_id + "'";
                    ta.Exe(MPrncBal);
                    ///UpdateStatement
                    String UpStatement = "update wcdeptstatement set item_status = -9, statement_status = -9 where ref_docno = '"
                        + slip_no + "' and deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    ta.Exe(UpStatement);

                    String MaxStatement = "select max(seq_no) as seq_no from wcdeptstatement where deptaccount_no = '"
                        + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    Sdt dtMax = ta.Query(MaxStatement);
                    decimal seq_no = 0;
                    if (dtMax.Next())
                    {
                        seq_no = dtMax.GetDecimal("seq_no") + 1;
                    }
                    else
                    {
                        throw new Exception("ไม่สามารถสร้างลำดับ Statement ได้");
                    }
                    ///InsertStatement
                    String IsStatement = @"Insert into wcdeptstatement(deptaccount_no, seq_no, deptitemtype_code, ref_docno, deptitem_amt, 
                                entry_id, entry_date, operate_date, branch_id, item_status, statement_status, prncbal) 
                                Values('" + deptaccount_no + "', " + seq_no + ", 'RFE', '" + slip_no + "', " + slip_amt + ", '" + entry_id +
                                "', to_date('" + DateTime.Today.ToString("ddMMyyyy") + "', 'ddmmyyyy'), to_date('" +
                                DateTime.Today.ToString("ddMMyyyy") + "', 'ddmmyyyy'), '" + branch_id + "', 1, 1, " + prncbal + ")";
                    ta.Exe(IsStatement);
                }
                ///UpdateSlip
                String UpSlip = "Update wcdeptslip set item_status = -9 where deptslip_no = '" + slip_no + "' and deptaccount_no = '" + deptaccount_no
                    + "' and branch_id = '" + branch_id + "'";
                ta.Exe(UpSlip);

                String SQL4 = "SELECT * FROM wcrecievemonth WHERE recv_period = '" + period
                            + "' AND wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                Sdt dt = ta.Query(SQL4);
                if (dt.Next())
                {
                    String SQL3 = "UPDATE wcrecievemonth SET status_post = 8 WHERE recv_period = '" + period
                        + "' AND wfmember_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    ta.Exe(SQL3);
                }
                else
                {
                    throw new Exception("ไม่สามารถยกเลิกใบคำขอได้ เนื่องจากระบุเดือนและปีไม่ถูกต้อง");
                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;

            }
            return true;
        }

        public Boolean GenNewCsType(string cs_type)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            int i = 1;
            try
            {

                string sql = "select * from wcdeptmaster where Branch_Id in(select coopbranch_Id from cmucfcoopbranch where cs_type=" + cs_type 
                    + ") and deptclose_status = 0 order by  deptopen_date,deptaccount_no,branch_id";

                Sdt dt = ta.Query(sql);

                string deptaccount_no, branch_id;
                while (dt.Next())
                {   //ทะเบียน
                    deptaccount_no = dt.GetString("deptaccount_no");
                    branch_id = dt.GetString("branch_id");
                    String SqlCreateMaster = @"insert into wcdeptmaster_copy(deptaccount_no, wftype_code, depttype_code, deptopen_date, effective_date, member_no, prename_code, wfaccount_name,
                                            deptaccount_name, deptaccount_sname, deptclose_status, card_person, prncbal, laststmseq_no, lastaccess_date, last_process_date,
                                            membgroup_code, wfmember_status, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, phone,
                                            deptrequest_docno, apply_date, branch_id, wpf_amt,cremateapp_amt,wfcarcass_seq,deptclose_date,mate_name,carreer,applyfee_status,withdrawable_amt,mariage_status,wfe_amt,wfe_amt2,fee_amt,wfy_amt,keepmem_no,manage_corpse_name,foreigner_flag,resigncause_code,die_date,die_status,remark,kprecv_period,kpreceive_status)
                                            select '" + int.Parse(i.ToString()).ToString("000000") + @"', wftype_code, depttype_code, deptopen_date, effective_date, member_no, 
                                            prename_code, wfaccount_name,deptaccount_name, deptaccount_sname,deptclose_status, card_person, prncbal, laststmseq_no, lastaccess_date 
                                            , last_process_date, membgroup_code,wfmember_status, sex, wfbirthday_date, member_type, contact_address, ampher_code, province_code, postcode, 
                                            phone, deptrequest_docno, apply_date,branch_id, wpf_amt,cremateapp_amt,wfcarcass_seq,deptclose_date,mate_name,carreer,applyfee_status,withdrawable_amt,mariage_status,wfe_amt,wfe_amt2,fee_amt,wfy_amt,keepmem_no,manage_corpse_name,foreigner_flag,resigncause_code,die_date,die_status,remark,kprecv_period,kpreceive_status from wcdeptmaster where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    ta.Exe(SqlCreateMaster);
                    //ผู้รับประโยชน์
                    String SqlInsertCoDept = @"insert into wccodeposit_copy(deptaccount_no,seq_no,{0}NAME{0},codept_addre,codept_id,branch_id)
                                                select '" + int.Parse(i.ToString()).ToString("000000") + @"',seq_no,{0}NAME{0},codept_addre,codept_id,branch_id from wccodeposit
                                                where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    SqlInsertCoDept = string.Format(SqlInsertCoDept, "\"");
                    ta.Exe(SqlInsertCoDept);
                    //รายการเคลื่อนไหว Statement
                    String SqlDeptStatement = @"insert into wcdeptstatement_copy(deptaccount_no,seq_no,deptitemtype_code,operate_date,ref_docno,deptitem_amt,prncbal,
                                            item_status, entry_id, entry_date,statement_status, branch_id)
                                                select '" + int.Parse(i.ToString()).ToString("000000") + @"',seq_no,deptitemtype_code,operate_date,ref_docno,deptitem_amt,prncbal,item_status,entry_id, entry_date,statement_status, branch_id from wcdeptstatement
                                                where deptaccount_no = '" + deptaccount_no + "' and branch_id = '" + branch_id + "'";
                    SqlDeptStatement = string.Format(SqlDeptStatement, "\"");
                    ta.Exe(SqlDeptStatement);
                    i++;

                }
                ta.Commit();
                ta.Close();
                return true;
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
        }

        public Boolean SavePersonChgStatus(string pbl, String xmlDwMain)
        {
            Sta ta = new Sta(sec.ConnectionString);
            ta.Transection();
            try
            {
                this.ToPhycalPathPbl(ref pbl);
                DataStore dwMain = new DataStore(pbl, "d_wc_chkother_cs_search_detail");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                string deptaccount_no, coopbranch_id;
                Decimal status ;
                for (int i = 0; i < dwMain.RowCount; i++)
                {
                    deptaccount_no = dwMain.GetItemString(i + 1, "deptaccount_no");
                    coopbranch_id = dwMain.GetItemString(i + 1, "coopbranch_id");
                    status = dwMain.GetItemDecimal(i + 1, "status");

                    if (status == 1)
                    {

                        String SQLup = @"update wcdeptmaster set deptclose_status = 0, deptclose_date = null, wfmember_status = 1, 
                                        die_date = null, resigncause_code = null, die_status = null 
                                        where deptaccount_no ='" + deptaccount_no + "' and branch_id = '" + coopbranch_id + "' ";
                        ta.Exe(SQLup);
                    }

                }

                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            return true;
        }
    }
}