using System;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Globalization;
using WebService.Processing;

namespace WebService
{
    /// <summary>
    /// Summary description for Deposit
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Deposit : System.Web.Services.WebService
    {
        [WebMethod]//Doys
        public String BaseFormatAccountNo(String wsPass, String deptAccountNoFormat)
        {
            return new DepositSvEn(wsPass).BaseFormatAccountNo(deptAccountNoFormat);
        }

        [WebMethod]
        public bool CalIntEstimate(String wsPass, DateTime calToDate, String branchId, String xmlListMidGroup)
        {
            return new DepositSvEn(wsPass).CalIntEstimate(calToDate, branchId, xmlListMidGroup);
        }

        [WebMethod]
        public bool CancelCheque(String wsPass, String entryId, DateTime entryDate, String machineId, String branchId, int oldStatus, String xmlChqList)
        {
            return new DepositSvEn(wsPass).CancelCheque(entryId, entryDate, machineId, branchId, oldStatus, xmlChqList);
        }

        [WebMethod]
        public String CloseCancel(String wsPass, String deptAccountNo, String branchId, DateTime entryDate, String entryId, String apv_id)
        {
            return new DepositSvEn(wsPass).CloseCancel(deptAccountNo, branchId, entryDate, entryId, apv_id);
        }

        [WebMethod]
        public bool CloseDay(String wsPass, DateTime closeDate, DateTime workDate, String appName, String branchId, String entryId, String machineId)
        {
            return new DepositSvEn(wsPass).CloseDay(closeDate, workDate, appName, branchId, entryId, machineId);
        }

        [WebMethod]
        public bool CloseMonth(String wsPass, DateTime closeDate, String appName, short month, short year, String branchId, String entryId)
        {
            return new DepositSvEn(wsPass).CloseMonth(closeDate, appName, month, year, branchId, entryId);
        }

        [WebMethod]
        public bool CloseYear(String wsPass, short year, DateTime operateDate, String entryId, String machineId, String application, String branchId)
        {
            return new DepositSvEn(wsPass).CloseYear(year, operateDate, entryId, machineId, application, branchId);
        }

        [WebMethod]//Doys
        public String DDDWXMLBankBranch(String wsPass, String bankCode)
        {
            return new DepositSvEn(wsPass).DDDWXMLBankBranch(bankCode);
        }

        [WebMethod]
        public String DepositPost(String wsPass, String xmlSlipMaster, String xmlSlipChq, String xmlSlipItem)
        {
            return new DepositSvEn(wsPass).DepositPost(xmlSlipMaster, xmlSlipChq, xmlSlipItem);
        }

        [WebMethod]//Doys
        public String GetCardPerson(String wsPass, String membNo)
        {
            return new DepositSvEn(wsPass, false).GetCardPerson(membNo);
        }

        [WebMethod]//Doys
        public System.Data.DataTable GetChildDeptWith(String wsPass, String itemType)
        {
            return new DepositSvEn(wsPass, false).GetChildDeptWith(itemType);
        }

        [WebMethod]
        public String GetChequeList(String wsPass, DateTime deptDate, int clearStatus, String branchId)
        {
            return new DepositSvEn(wsPass).GetChequeList(deptDate, clearStatus, branchId);
        }

        [WebMethod]//Doys
        public String GetChildDeptWithXml(String wsPass, String itemType)
        {
            return new DepositSvEn(wsPass).GetChildDeptWithXml(itemType);
        }

        [WebMethod]//Doys
        public String GetDeptCodeMask(String wsPass)
        {
            return new DepositSvEn(wsPass, false).GetDeptCodeMask();
        }

        [WebMethod]//Doys
        public String GetNewAccountNames(String wsPass, String membNo)
        {
            return new DepositSvEn(wsPass, false).GetNewAccountNames(membNo);
        }

        [WebMethod]//Doys
        public String GetRecpPayTypeDesc(String wsPass, String recpPayCode)
        {
            return new DepositSvEn(wsPass, false).GetRecpPayTypeDesc(recpPayCode);
        }

        [WebMethod]//Doys
        public String GetDpDeptConstant(String wsPass, String column)
        {
            return new DepositSvEn(wsPass).GetDpDeptConstant(column);
        }

        [WebMethod]
        public String InitDepSlip(String wsPass, String deptaccountNo, String branchId, String entryId, DateTime entryDate, String machineId)
        {
            return new DepositSvEn(wsPass).InitDepSlip(deptaccountNo, branchId, entryId, entryDate, machineId);
        }

        [WebMethod]
        public String[] InitDeptSlipCalInt(String wsPass, String accNo, String branchId, String entryID, DateTime entryDate, String machineID, String xmlSlipMaster, String xmlSlipItem)
        {
            return new DepositSvEn(wsPass).InitDeptSlipCalInt(accNo, branchId, entryID, entryDate, machineID, xmlSlipMaster, xmlSlipItem);
        }

        [WebMethod]
        public String InitDeptSlipDetail(String wsPass, String deptType, String deptAccountNo, String branchID, DateTime entryDate, String slipType)
        {
            return new DepositSvEn(wsPass).InitDeptSlipDetail(deptType, deptAccountNo, branchID, entryDate, slipType);
        }

        [WebMethod]
        public String InitOpenSlip(String wsPass, String deptGroup, String personCode, String branchId, DateTime entryDate, String entryId, String machineId)
        {
            return new DepositSvEn(wsPass).InitOpenSlip(deptGroup, personCode, branchId, entryDate, entryId, machineId);
        }

        [WebMethod]
        public bool MultiDeposit(String wsPass, String xmlMain, String xmlDeptCash, String xmlDeptCheque, String branchId, String entryId, String machineId, DateTime operateDate)
        {
            return new DepositSvEn(wsPass).MultiDeposit(xmlMain, xmlDeptCash, xmlDeptCheque, branchId, entryId, machineId, operateDate);
        }

        [WebMethod]
        public String OpenAccount(String wsPass, String xmlMaster, String xmlCheque, String xmlCoDept, short period)
        {
            return new DepositSvEn(wsPass).OpenAccount(xmlMaster, xmlCheque, xmlCoDept, period);
        }

        [WebMethod]
        public String OperateCancel(String wsPass, String slipNo, String branchId, String entryId, String machineId, DateTime workDate, String apv_id)
        {
            return new DepositSvEn(wsPass).OperateCancel(slipNo, branchId, entryId, machineId, workDate, apv_id);
        }

        [WebMethod]
        public bool PastChequeByList(String wsPass, String entryId, DateTime entryDate, String machineId, String branchId, String xmlChqList)
        {
            return new DepositSvEn(wsPass).PastChequeByList(entryId, entryDate, machineId, branchId, xmlChqList);
        }

        [WebMethod]
        public String WithdrawClose(String wsPass, String xmlSlipMaster, String xmlSlipItem)
        {
            return new DepositSvEn(wsPass).WithdrawClose(xmlSlipMaster, xmlSlipItem);
        }

        [WebMethod]
        public String ViewAccountNoFormat(String wsPass, String deptAccountNo)
        {
            return new DepositSvEn(wsPass, false).ViewAccountNoFormat(deptAccountNo);
        }

        [WebMethod]
        public String ViewCardMemberFormat(String wsPass, String memberCard)
        {
            return new DepositSvEn(wsPass, false).ViewCardMemberFormat(memberCard);
        }

        [WebMethod]
        public String GetNewStartBkNo(String wsPass, String BookType, String BookGroup, String BranchID)
        {
            DepositSvEn dep = new DepositSvEn(wsPass);
            return dep.GetStartBkNo(BookType, BookGroup, BranchID);
        }

        [WebMethod]
        public int RegisBookNo(String wsPass, String regisXml, String branchID, String entryID, String machineID, DateTime workDate)
        {
            DepositSvEn dep = new DepositSvEn(wsPass);
            return dep.RegisBookNo(regisXml, branchID, entryID, machineID, workDate);
        }

        [WebMethod]
        public String PrintSlip(String wsPass, String slipNo, String branchId, String printSet)
        {
            return new WinPrintCalling(wsPass).CallWinPrint("ap_deposit", "PrintSlip", new String[3] { slipNo, branchId, printSet });
            //return new DepositSvEn(wsPass, false).PrintSlip(slipNo, branchId, printSet);
        }

        [WebMethod]
        public String RunNotePad()
        {
            ProcessStartInfo starter;
            Process Prc;

            //''Pass File Path And Arguments
            //starter = new ProcessStartInfo("C:\\Windows\\notepad.exe");
            //starter = new ProcessStartInfo("C:\\Windows\\system32\\notepad.exe");
            starter = new ProcessStartInfo("C:\\Users\\Doys\\Documents\\apprinting.exe");

            starter.CreateNoWindow = true;
            starter.RedirectStandardOutput = true;
            starter.UseShellExecute = false;

            //''Start Adobe Process
            Prc = new Process();
            Prc.StartInfo = starter;
            Prc.Start();
            //0015300000585
            return "5555";
        }

        [WebMethod]
        public String InitPrintBook(String wsPass, String deptAccountNo, String branchId)
        {
            return new DepositSvEn(wsPass).InitPrintBook(deptAccountNo, branchId);
        }

        [WebMethod]
        public String PrintBook(String wsPass, String deptAccountNo, String branchId, short seqNo, short pageNo, short lineNo, bool bf, String printSet)
        {
            String[] ss = new String[7];
            ss[0] = deptAccountNo;
            ss[1] = branchId;
            ss[2] = seqNo + "";
            ss[3] = pageNo + "";
            ss[4] = lineNo + "";
            ss[5] = bf.ToString();
            ss[6] = printSet;
            return new WinPrintCalling(wsPass).CallWinPrint("ap_deposit", "PrintBook", ss);
            //return new DepositSvEn(wsPass).PrintBook(deptAccountNo, branchId, seqNo, pageNo, lineNo, bf, printSet);
        }

        [WebMethod]
        public String GetCashType(String wsPass, String recPayType)
        {
            return new DepositSvEn(wsPass).GetCashType(recPayType);
        }

        [WebMethod]
        public String ClientServer()
        {
            String result = "";
            try
            {
                TcpClient tcpclnt = new TcpClient();
                result += "Connecting.....\n";
                tcpclnt.Connect("192.168.99.97", 8001);
                // use the ipaddress as in the server program
                result += "Connected\n";
                result += "Enter the string to be transmitted : \n";
                //String str = Console.ReadLine();
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("555555555++");
                result += "Transmitting.....\n";

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                result += "------------------------------------------------\n";
                for (int i = 0; i < k; i++)
                {
                    result += Convert.ToChar(bb[i]) + "\n";
                }
                tcpclnt.Close();
            }

            catch (Exception e)
            {
                result += "Error..... " + e.ToString();
            }
            return result;
        }

        [WebMethod]
        public Decimal IsEqualDept(String wsPass, String deptAccountNo, String branchId, decimal balance, String deptTypeCode, String slipType)
        {
            return new DepositSvEn(wsPass).IsEqualDept(deptAccountNo, branchId, balance, deptTypeCode, slipType);
        }

        [WebMethod]
        public int GetLoopCloseDay(String wsPass, DateTime closeDate)
        {
            return new DepositSvEn(wsPass).GetLoopCloseDay(closeDate);
        }

        [WebMethod]
        public int OperateEndDay(String wsPass, DateTime workDate, String branchId, String entryID, String machineId)
        {
            return new DepositSvEn(wsPass).OperateEndDay(workDate, branchId, entryID, machineId);
        }

        [WebMethod]
        public int ProcessUpInt(String wsPass, String branchOriginId, String entryId, String machineId, DateTime entryDate, DateTime upIntDate)
        {
            return new DepositSvEn(wsPass).ProcessUpInt(branchOriginId, entryId, machineId, entryDate, upIntDate);
        }

        [WebMethod]
        public int CalIntRemain(String wsPass, String branchId, DateTime calIntTo)
        {
            return new DepositSvEn(wsPass).CalIntRemain(branchId, calIntTo);
        }

        [WebMethod]
        public int IsCloseMonth(String wsPass, String appName, String branchId)
        {
            return new DepositSvEn(wsPass).IsCloseMonth(appName, branchId);
        }

        [WebMethod]
        public int IsCloseYear(String wsPass, String appName, String branchId)
        {
            return new DepositSvEn(wsPass).IsCloseYear(appName, branchId);
        }

        [WebMethod]
        public int PostIntNextDay(String wsPass, DateTime closeDate, DateTime workDate, String entryId, String branchId, String machineId)
        {
            return new DepositSvEn(wsPass).PostIntNextDay(closeDate, workDate, entryId, branchId, machineId);
        }

        [WebMethod]
        public int UpdateCloseDayStatus(String wsPass, DateTime closeDate, String appName, String branchId)
        {
            return new DepositSvEn(wsPass).UpdateCloseDayStatus(closeDate, appName, branchId);
        }

        [WebMethod]
        public int UpdateReportBalDay(String wsPass, DateTime entryDate, String branchId, String entryId)
        {
            return new DepositSvEn(wsPass).UpdateReportBalDay(entryDate, branchId, entryId);
        }

        [WebMethod]
        public bool IsEndYearDate(String wsPass, DateTime closeDate)
        {
            return new DepositSvEn(wsPass).IsEndYearDate(closeDate);
        }

        [WebMethod]
        public bool IsEndMonthDate(String wsPass, DateTime closeDate)
        {
            return new DepositSvEn(wsPass).IsEndMonthDate(closeDate);
        }

        [WebMethod]
        public int GenReportBalDay(String wsPass, DateTime closeDate, String branchId, String entryId)
        {
            return new DepositSvEn(wsPass).GenReportBalDay(closeDate, branchId, entryId);
        }

        [WebMethod]
        public String GetIntDisplay(String wsPass, String depttype)
        {
            return new DepositSvEn(wsPass).GetIntDisplay(depttype);
        }

        [WebMethod]
        public int InsertNewRate(String wsPass, String headxml, String ratexml, String entryid, DateTime entrydate)
        {
            return new DepositSvEn(wsPass).InsertNewRate(headxml, ratexml, entryid, entrydate);
        }

        [WebMethod]
        public String InitIntRateOneDate(String wsPass, String headxml)
        {
            return new DepositSvEn(wsPass).InitIntRateOneDate(headxml);
        }

        [WebMethod]
        public String InitDataForSequest(String wsPass, String deptacc, String branchid, String machine, String userid, DateTime date)
        {
            return new DepositSvEn(wsPass).InitDataForSequest(deptacc, branchid, machine, userid, date);
        }

        [WebMethod]
        public int UpdateSequest(String wsPass, String sequestxml)
        {
            return new DepositSvEn(wsPass).UpdateSequest(sequestxml);
        }

        [WebMethod]
        public int UpdateExtramem(String wsPass, String extramemxml)
        {
            return new DepositSvEn(wsPass).UpdateExtramem(extramemxml);
        }

        [WebMethod]
        public String GetExtramemDetail(String wsPass, String extramemno)
        {
            return new DepositSvEn(wsPass).GetExtramemDetail(extramemno);
        }

        [WebMethod]
        public Decimal[] GetPermissAmount(String wsPass, string userId, string branchId)
        {
            return new DepositSvEn(wsPass).GetPermissAmount(userId, branchId);
        }

        [WebMethod]
        public String GetApvList(String wsPass, Decimal deptMax, Decimal widthMax, String branchId)
        {
            return new DepositSvEn(wsPass).GetApvList(deptMax, widthMax, branchId);
        }

        [WebMethod]
        public String GetApvListDetail(String wsPass, String processId, String branchId)
        {
            return new DepositSvEn(wsPass).GetApvListDetail(processId, branchId);
        }

        [WebMethod]
        public int ApvPermiss(String wsPass, String processId, String status, Decimal avpAmt, String machineId, String apvId, DateTime workDate, String avpCode, String itemType, String branchId)
        {
            return new DepositSvEn(wsPass).ApvPermiss(processId, status, avpAmt, machineId, apvId, workDate, avpCode, itemType, branchId);
        }

        [WebMethod]
        public String AddApvTask(String wsPass, String nameReq, String application, String computerReq, String itemType, String itemTypeDesc, String docType, String member, DateTime dateReq, decimal amountReq, String deptAccNo, String accName, String sysApvCode, String systemCode, int numApv, String branchId)
        {
            return new DepositSvEn(wsPass).AddApvTask(nameReq, application, computerReq, itemType, itemTypeDesc, docType, member, dateReq, amountReq, deptAccNo, accName, sysApvCode, systemCode, numApv, branchId);
        }

        [WebMethod]
        public String[] CheckRightPermissionCancel(String wsPass, String userId, String branchId, String deptslipNo, DateTime adtmDate)
        {
            return new DepositSvEn(wsPass).CheckRightPermissionCancel(userId, branchId, deptslipNo, adtmDate);
        }

        [WebMethod]
        public String[] CheckRightPermissionDep(String wsPass, String userId, String branchId, String deptslipXml, short OpenAccount)
        {
            return new DepositSvEn(wsPass).CheckRightPermissionDep(userId, branchId, deptslipXml, OpenAccount);
        }

        [WebMethod]
        public String[] CheckApproved(String wsPass, String processId, String branchId)
        {
            return new DepositSvEn(wsPass).CheckApproved(processId, branchId);
        }

        [WebMethod]
        public String PrintBookFirstPage(String wsPass, String application, String deptAccountNo, String branchId, String entryId, String bookNo, String reason, String avpId, String printSet, int normFlag, DateTime workDate, int reprint)
        {
            //new DepositSvEn(wsPass).PrintBookFirstPage(accountNo, branchId, workDate, entryId, bookNo, reson, apvId, normFlag, prinSet);
            string[] args = new string[10];
            args[0] = deptAccountNo;
            args[1] = branchId;
            args[2] = entryId;
            args[3] = bookNo;
            args[4] = reason;
            args[5] = avpId;
            args[6] = printSet;
            args[7] = normFlag.ToString();
            args[8] = workDate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            args[9] = reprint.ToString();
            return new WinPrintCalling(wsPass).CallWinPrint(application, "PrintBookFirstPage", args);
        }

        [WebMethod]
        public DateTime GetDueDate(String wsPass, String deptType, DateTime fromDate)
        {
            return new DepositSvEn(wsPass).GetDueDate(deptType, fromDate);
        }

        [WebMethod]
        public int RunCloseDayProcess(String wsPass, String w_sheet_id, DateTime closeDate, DateTime workDate, String appName, String branchId, String entryId, String machine)
        {
            DpCloseDayProgress dp = new DpCloseDayProgress(wsPass, closeDate, workDate, appName, branchId, entryId, machine);
            int ii = Processing.Progressing.Add(dp, appName, w_sheet_id, true);
            return ii;
        }

        [WebMethod]
        public String GetNameMember(String wsPass, String memberNo, String branchId, short memberFlag)
        {
            return new DepositSvEn(wsPass).GetNameMember(memberNo, branchId, memberFlag);
        }

        [WebMethod]
        public int UpdateMaxBookSeqNo(String wsPass, String deptAccountNo, String branchId)
        {
            return new DepositSvEn(wsPass, false).UpdateMaxBookSeqNo(deptAccountNo, branchId);
        }

        [WebMethod]
        public short AddNewDepttype(String wsPass, String xmlNewDepttype, String branchId, DateTime workDate)
        {
             return new DepositSvEn(wsPass).AddNewDepttype(xmlNewDepttype, branchId, workDate);
        }

        [WebMethod]
        public String GenIntAdvance(String wsPass, String appName, String wsheetName, DateTime workDate, DateTime dateFrom, DateTime dateTo, String deptTypeFrom, String deptTypeTo, String branchId)
        {
            DpIntAdvanceProgress dp = new DpIntAdvanceProgress(wsPass, workDate, dateFrom, dateTo, deptTypeFrom, deptTypeTo, branchId);
            Processing.Progressing.Add(dp, appName, wsheetName, true);
            return "true";
        }
    }
}