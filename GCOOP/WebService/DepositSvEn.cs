using System;
using System.Data;
using System.Configuration;
using DBAccess;
using pbservice;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace WebService
{
    public class DepositSvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_deposit_service svDep;
        private n_cst_deposit_utility svDepUtil;
        private Security security;
        //private String index2;

        public DepositSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public DepositSvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                svCon = new n_cst_dbconnectservice();
                svCon.of_connectdb(security.ConnectionString);
                svDep = new n_cst_deposit_service();
                svDep.of_settrans(svCon);
                svDep.of_init();
                svDepUtil = new n_cst_deposit_utility();
                svDepUtil.of_settrans(svCon);
            }
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~DepositSvEn()
        {
            DisConnect();
        }

        //------------------------------------------------------------------//

        public String BaseFormatAccountNo(String deptAccountNoFormat)
        {
            try
            {
                String result = svDep.of_analizeaccno(deptAccountNoFormat.Replace("-", ""));
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool CalIntEstimate(DateTime calToDate, String branchId, String xmlListMidGroup)
        {
            try
            {
                int ii = svDep.of_calint_estimate(calToDate, branchId, xmlListMidGroup);
                DisConnect();
                return 1 == ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool CancelCheque(String entryId, DateTime entryDate, String machineId, String branchId, int oldStatus, String xmlChqList)
        {
            try
            {
                String errorMessage = "";
                short oldStat = Convert.ToInt16(oldStatus);
                svDep.of_cancelchq_bylist(entryId, entryDate, machineId, branchId, xmlChqList, oldStat, ref errorMessage);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String CloseCancel(String deptAccountNo, String branchId, DateTime entryDate, String entryId, String apv_id)
        {
            String slipNo = "";
            try
            {
                //svDep.of_cancel_close(deptAccountNo, branchId, entryDate, apv_id, ref slipNo);
                svDep.of_cancel_close(deptAccountNo, branchId, entryDate, entryId, apv_id, ref slipNo);
                DisConnect();
                return slipNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool CloseDay(DateTime closeDate, DateTime workDate, String appName, String branchId, String entryId, String machineId)
        {
            try
            {
                svDep.of_close_day(closeDate, workDate, appName, branchId, entryId, machineId);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool CloseMonth(DateTime closeDate, String appName, short month, short year, String branchId, String entryId)
        {
            try
            {
                svDep.of_close_month(closeDate, appName, month, year, branchId, entryId);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool CloseYear(short year, DateTime operateDate, String entryId, String machineId, String application, String branchId)
        {
            try
            {
                int ii = svDep.of_close_year(year, operateDate, entryId, machineId, application, branchId);
                DisConnect();
                return ii == 1;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String DDDWXMLBankBranch(String bankCode)
        {
            try
            {
                String result = "";
                svDepUtil.of_dddwbankbranch(bankCode, ref result);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String DepositPost(String xmlSlipMaster, String xmlSlipChq, String xmlSlipItem)
        {
            try
            {
                String slipNo = "";
                svDep.of_deposit(xmlSlipMaster, xmlSlipChq, xmlSlipItem, ref slipNo);
                DisConnect();
                return slipNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetCardPerson(String membNo)
        {
            String conStr = security.ConnectionString;
            String result = "";
            Sta ta = new Sta(conStr);
            try
            {
                String sql = @"
                SELECT        CARD_PERSON
                FROM          MBMEMBMASTER LEFT OUTER JOIN
                              MBUCFPRENAME ON MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE
                WHERE        (MBMEMBMASTER.MEMBER_NO = '" + membNo + "')";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    result = dt.GetString(0);
                }
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return result;
        }

        public DataTable GetChildDeptWith(String itemType)
        {
            String conStr = security.ConnectionString;
            Sta ta = new Sta(conStr);
            try
            {
                String itemCode = "";
                String where = "";
                if (itemType == "+")
                {
                    itemCode = "DEP";
                }
                else if (itemType == "-")
                {
                    itemCode = "WID";
                }
                else if (itemType == "/")
                {
                    itemCode = "CLS";
                }
                if (itemCode != "")
                {
                    where = "and group_itemtpe='" + itemCode + "'";
                }
                else
                {
                    where = "and 1 = 2";
                }
                String sql = @"
                SELECT RECPPAYTYPE_CODE,   
                     RECPPAYTYPE_DESC,   
                     RECPPAYTYPE_SORT,   
                     GROUP_ITEMTPE,   
                     MONEYTYPE_SUPPORT,   
                     RECPPAYTYPE_FLAG,   
                     ACTIVE_FLAG  
                FROM DPUCFRECPPAYTYPE       
                WHERE 1 = 1 " + where + " and active_flag=1 order by RECPPAYTYPE_CODE";
                DataTable dt = ta.QueryDataTable(sql);
                dt.TableName = "deptwith";
                ta.Close();
                return dt;
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
        }

        public String GetChildDeptWithXml(String itemType)
        {
            //try
            //{
            //    String errMess = "";
            //    int result = 0;
            //    String xml = "";
            //    result = svDepUtil.of_getchild_deptwith(itemType, ref xml);
            //    if (result != 1)
            //    {
            //        throw new Exception(errMess);
            //    }
            //    DisConnect();
            //    return xml;

            //}
            //catch (Exception e)
            //{
            //    DisConnect();
            //    throw e;
            //}
            return "";
        }

        public String GetChequeList(DateTime deptDate, int clearStatus, String branchId)
        {
            try
            {
                String xmlChequeList = "";
                short stat = Convert.ToInt16(clearStatus);
                svDep.of_get_chqlist(deptDate, stat, branchId, ref xmlChequeList);
                DisConnect();
                return xmlChequeList;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetDpDeptConstant(String column)
        {
            Sta ta = new Sta(security.ConnectionString);
            String result = "";
            try
            {
                Sdt dt = ta.Query("select " + column + " from DPDEPTCONSTANT");
                if (!dt.Next())
                {
                    ta.Close();
                    throw new Exception("ไม่มีข้อมูล column " + column);
                }
                result = dt.GetString(0);
            }
            catch { }
            ta.Close();
            return result;
        }

        public String GetDeptCodeMask()
        {
            Sta ta = new Sta(security.ConnectionString);
            try
            {
                String sql = "SELECT deptcode_mask  FROM DPDEPTCONSTANT";
                Sdt dt = ta.Query(sql);
                if (!dt.Next()) throw new Exception("ไม่มีข้อมูลการแสดงผลเลขที่บัญชี");
                String deptcodeMask = dt.GetString(0);
                ta.Close();
                return deptcodeMask;
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
        }

        public String GetNewAccountNames(String membNo)
        {
            String conStr = security.ConnectionString;
            String result = "";
            Sta ta = new Sta(conStr);
            try
            {
                String sql = @"
                    SELECT        MBUCFPRENAME.PRENAME_DESC || MBMEMBMASTER.MEMB_NAME || ' ' || MBMEMBMASTER.MEMB_SURNAME AS ACCOUNT_NAME
                    FROM          MBMEMBMASTER LEFT OUTER JOIN
                                  MBUCFPRENAME ON MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE
                    WHERE        (MBMEMBMASTER.MEMBER_NO = '" + membNo + "')";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    result = dt.GetString(0);
                }
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return result;
        }

        public String GetRecpPayTypeDesc(String recpPayCode)
        {
            String conStr = security.ConnectionString;
            String result = "";
            Sta ta = new Sta(conStr);
            try
            {
                String sql = "SELECT RECPPAYTYPE_DESC FROM DPUCFRECPPAYTYPE WHERE RECPPAYTYPE_CODE = '" + recpPayCode + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    result = dt.GetString(0);
                }
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return result;
        }

        public String InitDepSlip(String deptaccountNo, String branchId, String entryId, DateTime entryDate, String machineId)
        {
            //--------------------  ส่วน Doys เขียนเอง  ----------------------------
            Sta ta = new Sta(security.ConnectionString);
            try
            {
                bool isValid = true;
                bool isClose = false;
                String sql = "select * from dpdeptmaster where deptaccount_no = '" + deptaccountNo + "'";
                Sdt dt = ta.Query(sql);
                isValid = dt.Next();
                if (!isValid) throw new Exception("ไม่มีบัญชีเงินฝากเลขที่ " + ViewAccountNoFormat(deptaccountNo));
                isClose = dt.GetInt32("deptclose_status") == 1;
                if (isClose) throw new Exception("บัญชีเลขที่ " + ViewAccountNoFormat(deptaccountNo) + " ได้ทำการปิดบัญชีแล้ว");
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            ////--------------------  ส่วน PBService ----------------------------
            //String xmlSlipMaster = "";
            //String errMessage = "";
            //int result = svDep.of_init_deptslip(deptaccountNo, branchId, entryId, entryDate, machineId, ref xmlSlipMaster, ref errMessage);
            //if (result != 1)
            //{
            //    throw new Exception(errMessage);
            //}
            //return xmlSlipMaster;
            try
            {
                String xmlSlipMaster = "";
                String errMessage = "";
                int result = svDep.of_init_deptslip(deptaccountNo, branchId, entryId, entryDate, machineId, ref xmlSlipMaster, ref errMessage);
                DisConnect();
                return xmlSlipMaster;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] InitDeptSlipCalInt(String accNo, String branchId, String entryID, DateTime entryDate, String machineID, String xmlSlipMaster, String xmlSlipItem)
        {
            try
            {
                String[] result = new String[2];
                String errorMessage = "";
                int re = svDep.of_init_deptslip_calint(accNo, branchId, entryID, entryDate, machineID, ref xmlSlipMaster, ref xmlSlipItem, ref errorMessage);
                result[0] = xmlSlipMaster;
                result[1] = xmlSlipItem;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitDeptSlipDetail(String deptType, String deptAccountNo, String branchID, DateTime entryDate, String slipType)
        {
            try
            {
                String errorMessage = "";
                String xmlSlipItem = "";
                int re = svDep.of_init_deptslip_det(deptType, deptAccountNo, branchID, entryDate, slipType, ref xmlSlipItem, ref errorMessage);
                DisConnect();
                return xmlSlipItem;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitOpenSlip(String deptGroup, String personCode, String branchId, DateTime entryDate, String entryId, String machineId)
        {
            try
            {
                String result = "";
                String errorMessage = "";
                int re = svDep.of_init_openslip(deptGroup, personCode, branchId, entryDate, entryId, machineId, ref result, ref errorMessage);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool MultiDeposit(String xmlMain, String xmlDeptCash, String xmlDeptCheque, String branchId, String entryId, String machineId, DateTime operateDate)
        {
            try
            {
                svDep.of_multi_deposit(xmlMain, xmlDeptCash, xmlDeptCheque, branchId, entryId, machineId, operateDate);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String OpenAccount(String xmlMaster, String xmlCheque, String xmlCoDept, short period)
        {
            //try
            //{
            //    String deptAccountNo = "";
            //    String errorMessage = "";
            //    int re = svDep.of_openaccount(xmlMaster, xmlCheque, xmlCoDept, period, ref deptAccountNo, ref errorMessage);
            //    if (re != 1)
            //    {
            //        throw new Exception(errorMessage);
            //    }
            //    DisConnect();
            //    return deptAccountNo;
            //}
            //catch (Exception e)
            //{
            //    DisConnect();
            //    throw e;
            //}
            try
            {
                String deptAccountNo = "";
                String errorMessage = "";
                int re = svDep.of_openaccount(xmlMaster, xmlCheque, xmlCoDept, period, ref deptAccountNo, ref errorMessage);
                DisConnect();
                return deptAccountNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String OperateCancel(String slipNo, String branchId, String entryId, String machineId, DateTime workDate, String apv_id)
        {
            String RetslipNo = "";
            try
            {
                //String errMessage = "";
                int re = svDep.of_operate_cancel(slipNo, branchId, entryId, machineId, workDate, apv_id, ref RetslipNo);
                DisConnect();
                return RetslipNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool PastChequeByList(String entryId, DateTime entryDate, String machineId, String branchId, String xmlChqList)
        {
            try
            {
                String errorMessage = "";
                svDep.of_pastchq_bylist(entryId, entryDate, machineId, branchId, xmlChqList, ref errorMessage);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String WithdrawClose(String xmlSlipMaster, String xmlSlipItem)
        {
            try
            {
                String slipNo = "";
                int result = svDep.of_withdraw_close(xmlSlipMaster, xmlSlipItem, ref slipNo);
                DisConnect();
                return slipNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String ViewAccountNoFormat(String deptAccountNo)
        {
            Sta ta = new Sta(security.ConnectionString);
            Sdt dt = ta.Query("select deptcode_mask from DPDEPTCONSTANT");
            if (!dt.Next())
            {
                ta.Close();
                throw new Exception("ไม่สามารถติดต่อกับ Database ได้");
            }
            else
            {
                ta.Close();
            }
            String format = dt.GetString(0).ToUpper();//"X-XX-XXXXXXX";
            char[] fc = format.ToCharArray();
            char[] ac = deptAccountNo.ToCharArray();
            String accNo = "";
            int j = 0;
            for (int i = 0; i < fc.Length; i++)
            {
                if (fc[i] != 'X')
                {
                    accNo += fc[i].ToString();
                }
                else
                {
                    try
                    {
                        accNo += ac[j++];
                    }
                    catch { accNo += ""; }
                }
            }
            return accNo;
        }

        public String ViewCardMemberFormat(String memberCard)
        {
            String format = "X-XXXX-XXXXX-XX-X";//"X-XX-XXXXXXX";
            char[] fc = format.ToCharArray();
            char[] ac = memberCard.ToCharArray();
            String cardNo = "";
            int j = 0;
            for (int i = 0; i < fc.Length; i++)
            {
                if (fc[i] != 'X')
                {
                    cardNo += fc[i].ToString();
                }
                else
                {
                    try
                    {
                        cardNo += ac[j++];
                    }
                    catch { cardNo += ""; }
                }
            }
            return cardNo;
        }

        public String GetStartBkNo(String BookType, String BookGroup, String BranchID)
        {
            try
            {
                String StartBkNo = "";
                int result = 0;
                result = svDep.of_get_new_startbook_no(BookType, BookGroup, BranchID, ref StartBkNo);
                DisConnect();
                return StartBkNo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int RegisBookNo(String RegisXml, String BranchID, String EntryID, String MachineID, DateTime WorkDate)
        {
            try
            {
                int re = svDep.of_register_book(RegisXml, BranchID, EntryID, MachineID, WorkDate);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public void Running()
        {
            //ProcessStartInfo starter;
            //Process Prc;
            ////''Pass File Path And Arguments
            //starter = new ProcessStartInfo("C:\\Windows\\notepad.exe", "C:\\cmdx.txt");
            //starter.CreateNoWindow = true;
            //starter.RedirectStandardOutput = true;
            //starter.UseShellExecute = false;

            ////''Start Adobe Process
            //Prc = new Process();
            //Prc.StartInfo = starter;
            //Prc.Start();
            ////0015300000585
            ////svDep.of_print_slip("0015300000585", "000", "DEPOSIT_DOYS");
            //System.Diagnostics.Process.Start("C:\\Windows\\notepad.exe");

            System.Diagnostics.Process.Start("C:\\printing.bat");
        }

        public String InitPrintBook(String deptAccountNo, String branchId)
        {
            try
            {
                String xmlPrintForm = "";
                svDep.of_init_printbook(deptAccountNo, branchId, ref xmlPrintForm);
                DisConnect();
                return xmlPrintForm;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String PrintSlip(String slipNo, String branchId, String printSet)
        {
            TcpClient tcpclnt = new TcpClient();
            try
            {
                String result = "";
                String[] ss = new String[10];
                ss[0] = "dp_slip";
                ss[1] = security.ConnectionString;
                ss[2] = slipNo;
                ss[3] = branchId;
                ss[4] = printSet;
                ss[5] = "deptAccountNo";
                ss[6] = "0";
                ss[7] = "0";
                ss[8] = "false";
                ss[9] = "0"; //printSeqS = message[9];

                String sender = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", ss);

                tcpclnt.Connect(security.WinPrintIP, security.WinPrintPort);
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(sender);
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                for (int i = 0; i < k; i++)
                {
                    result += Convert.ToChar(bb[i]);
                }
                tcpclnt.Close();
                return result;// 1 == svDep.of_print_slip(slipNo, branchId, pringSet);
            }
            catch (Exception ex)
            {
                try
                {
                    tcpclnt.Close();
                }
                catch { }
                throw ex;
            }
        }

        public String PrintBook(String deptAccountNo, String branchId, short seqNo, short pageNo, short lineNo, bool bf, String printSet)
        {
            TcpClient tcpclnt = new TcpClient();
            try
            {
                String result = "";
                String[] ss = new String[10];
                ss[0] = "dp_book";
                ss[1] = security.ConnectionString;
                ss[2] = "slipNo";
                ss[3] = branchId;
                ss[4] = printSet;
                ss[5] = deptAccountNo;
                ss[6] = pageNo + "";
                ss[7] = lineNo + "";
                ss[8] = bf.ToString();
                ss[9] = seqNo + ""; //printSeqS = message[9];
                String sender = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", ss);
                tcpclnt.Connect(security.WinPrintIP, security.WinPrintPort);
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(sender);
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                for (int i = 0; i < k; i++)
                {
                    result += Convert.ToChar(bb[i]);
                }
                tcpclnt.Close();
                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    tcpclnt.Close();
                }
                catch { }
                throw ex;
            }
        }

        public String GetCashType(String recPayType)
        {
            try
            {
                String result = svDep.of_get_cashtype(recPayType);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Decimal IsEqualDept(String deptAccountNo, String branchId, decimal balance, String deptTypeCode, String slipType)
        {
            try
            {
                decimal deptAmt = 0;
                int ii = svDep.of_is_equal_dept(deptAccountNo, branchId, balance, deptTypeCode, slipType, ref deptAmt);
                DisConnect();
                return ii == 1 ? deptAmt : 0;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int GetLoopCloseDay(DateTime closeDate)
        {
            try
            {
                int loop = svDep.of_get_loopcloseday(closeDate);
                DisConnect();
                return loop;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int OperateEndDay(DateTime workDate, String branchId, String entryID, String machineId)
        {
            try
            {
                int ii = svDep.of_operate_endday(workDate, branchId, entryID, machineId);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int ProcessUpInt(String branchOriginId, String entryId, String machineId, DateTime entryDate, DateTime upIntDate)
        {
            try
            {
                int ii = svDep.of_process_upint(branchOriginId, entryId, machineId, entryDate, upIntDate);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int CalIntRemain(String branchId, DateTime calIntTo)
        {
            try
            {
                int ii = svDep.of_calint_remain(branchId, calIntTo);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int IsCloseMonth(String appName, String branchId)
        {
            try
            {
                int ii = svDep.of_is_close_month(appName, branchId);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int IsCloseYear(String appName, String branchId)
        {
            try
            {
                int ii = svDep.of_is_close_year(appName, branchId);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int PostIntNextDay(DateTime closeDate, DateTime workDate, String entryId, String branchId, String machineId)
        {
            try
            {
                String errorMessage = "";
                int ii = svDep.of_postint_nextday(closeDate, workDate, entryId, branchId, machineId, ref errorMessage);
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int UpdateCloseDayStatus(DateTime closeDate, String appName, String branchId)
        {
            try
            {
                int ii = svDep.of_update_closedaystatus(closeDate, appName, branchId);
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int UpdateReportBalDay(DateTime entryDate, String branchId, String entryId)
        {
            try
            {
                int ii = svDep.of_updatereport_balday(entryDate, branchId, entryId);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool IsEndYearDate(DateTime closeDate)
        {
            try
            {
                bool b = svDep.of_is_endyear_date(closeDate);
                DisConnect();
                return b;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool IsEndMonthDate(DateTime closeDate)
        {
            try
            {
                bool b = svDep.of_is_endmonth_date(closeDate);
                DisConnect();
                return b;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int GenReportBalDay(DateTime closeDate, String branchId, String entryId)
        {
            try
            {
                int ii = svDep.of_genreport_balday(closeDate, branchId, entryId);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetIntDisplay(String depttype)
        {
            try
            {
                String intdisplay = "";
                svDep.of_get_intdisplay(depttype, ref intdisplay);
                DisConnect();
                return intdisplay;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int InsertNewRate(String headxml, String ratexml, String entryid, DateTime entrydate)
        {
            try
            {
                svDep.of_insert_newrate_int(headxml, ratexml, entryid, entrydate);
                DisConnect();
                return 1;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitIntRateOneDate(String headxml)
        {
            String ratexml = "";
            try
            {
                svDep.of_initintrate_onedate(headxml, ref ratexml);
                DisConnect();
                return ratexml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitDataForSequest(String deptacc, String branchid, String machine, String userid, DateTime date)
        {
            String sequestxml = "";
            try
            {
                svDep.of_init_data_for_sequest(deptacc, branchid, machine, userid, date, ref sequestxml);
                DisConnect();
                return sequestxml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int UpdateSequest(String sequestxml)
        {
            try
            {
                svDep.of_update_sequest(sequestxml);
                DisConnect();
                return 1;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int UpdateExtramem(String extramemxml)
        {
            try
            {
                svDep.of_update_extramem(extramemxml);
                DisConnect();
                return 1;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetExtramemDetail(String extramemno)
        {
            String extramemxml = "";
            try
            {
                svDep.of_get_extramem_detail(extramemno, ref extramemxml);
                DisConnect();
                return extramemxml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] CheckRightPermissionCancel(String userId, String branchId, String deptslipNo, DateTime adtmDate)
        {
            try
            {
                string apvCode = "", codeDesc = "";
                int re = svDep.of_check_right_permission(userId, branchId, deptslipNo, adtmDate, ref apvCode, ref codeDesc);
                String[] result = new String[2];
                result[0] = re == 1 ? "true" : apvCode;
                result[1] = codeDesc == null ? "" : codeDesc;
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] CheckRightPermissionDep(String userId, String branchId, String deptslipXml, short OpenAccount)
        {
            try
            {
                string apvCode = "", codeDesc = "";
                int re = svDep.of_check_right_permission(userId, branchId, deptslipXml, OpenAccount, ref apvCode, ref codeDesc);
                String[] result = new String[2];
                result[0] = re == 1 ? "true" : apvCode;
                result[1] = codeDesc == null ? "" : codeDesc;
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Decimal[] GetPermissAmount(string userId, string branchId)
        {
            Decimal widthMax = 0;
            Decimal deptMax = 0;

            try
            {
                Decimal[] result = new Decimal[2];
                svDep.of_get_permiss_amount(userId, branchId, ref widthMax, ref deptMax);
                result[0] = widthMax;
                result[1] = deptMax;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetApvList(Decimal deptMax, Decimal widthMax, String branchId)
        {
            String listxml = "";
            try
            {
                svDep.of_get_apvlist(deptMax, widthMax, branchId, ref listxml);
                DisConnect();
                return listxml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetApvListDetail(String processId, String branchId)
        {
            String listxml = "";
            try
            {
                svDep.of_get_apvlist_detail(processId, branchId, ref listxml);
                DisConnect();
                return listxml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int ApvPermiss(String processId, String status, Decimal avpAmt, String machineId, String apvId, DateTime workDate, String avpCode, String itemType, String branchId)
        {
            try
            {
                svDep.of_apv_permiss(processId, status, avpAmt, machineId, apvId, workDate, avpCode, itemType, branchId);
                DisConnect();
                return 1;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String AddApvTask(String nameReq, String application, String computerReq, String itemType, String itemTypeDesc, String docType, String member, DateTime dateReq, decimal amountReq, String deptAccNo, String accName, String sysApvCode, String systemCode, int numApv, String branchId)
        {
            try
            {
                short numOfApv = Convert.ToInt16(numApv);
                String result = svDep.of_addapv_task(nameReq, application, computerReq, itemType, itemTypeDesc, docType, member, dateReq, amountReq, deptAccNo, accName, sysApvCode, systemCode, numOfApv, branchId);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] CheckApproved(String processId, String branchId)
        {
            String nameapprove = "";
            try
            {
                string re = svDep.of_check_approved(processId, branchId, ref nameapprove);
                String[] result = new String[2];
                result[0] = re;
                result[1] = nameapprove;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public DateTime GetDueDate(String deptType, DateTime fromDate)
        {
            try
            {
                DateTime result = svDep.of_getduedate(deptType, fromDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetNameMember(String memberNo, String branchId, short memberFlag)
        {
            try
            {
                String result = svDep.of_get_namemember(memberNo, branchId, memberFlag);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String xxxPrintBook()
        {
            //svDep.of_print_book_firstpage();
            return "";
        }

        public int UpdateMaxBookSeqNo(String deptAccountNo, String branchId)
        {
            Sta ta = new Sta(security.ConnectionString);
            try
            {
                String maxSql = @"
                SELECT max(seq_no) as max_seqno
                FROM 
                    DPDEPTSTATEMENT,
                    DPUCFDEPTITEMTYPE
                WHERE ( DPUCFDEPTITEMTYPE.DEPTITEMTYPE_CODE = DPDEPTSTATEMENT.DEPTITEMTYPE_CODE) 
                AND ( ( dpdeptstatement.deptaccount_no = '" + deptAccountNo + @"' ) 
                AND ( DPDEPTSTATEMENT.FORPRNBK_FLAG = 1 ) 
                AND ( DPDEPTSTATEMENT.BRANCH_ID = '" + branchId + @"' ))
                AND prntopb_status = '1'";

                Sdt dt = ta.Query(maxSql);
                if (!dt.Next())
                {
                    throw new Exception("ไม่พบข้อมูลรายการพิมพ์สมุด");
                }
                int re = dt.GetInt32(0);
                int ii = ta.Exe("update dpdeptmaster set lastrec_no_pb = " + re + " where deptaccount_no='" + deptAccountNo + "'");
                ta.Close();
                return re;
            }
            catch (Exception ex)
            {
                ta.Close();
                DisConnect();
                throw ex;
            }
        }

        public short AddNewDepttype(String xmlNewDepttype, String branchId, DateTime workDate)
        {
            try
            {
                short result = svDep.of_add_newdepttype(xmlNewDepttype, branchId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
    }
}