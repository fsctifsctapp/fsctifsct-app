using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using pbservice;
using WebService.Processing;

namespace WebService
{
    /// <summary>
    /// Summary description for LoanAssist
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoanAssist : System.Web.Services.WebService
    {

        //may ประมวลผลจัดเก็บ
        [WebMethod]
        public int RunKeepProcess(String wsPass, String xmlprocdata, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            LsKeepProcess sl = new LsKeepProcess(sec.ConnectionString, xmlprocdata);
            return Processing.Progressing.Add(sl, application, w_sheet_id);
        }
        //may ประมวลผ่านรายการผลจัดเก็บ
        [WebMethod]
        public int RunKeepPostProcess(String wsPass, String xmlprocdata, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            LsKeepPostProcess sl = new LsKeepPostProcess(sec.ConnectionString, xmlprocdata);
            return Processing.Progressing.Add(sl, application, w_sheet_id);
        }
        //may ยกเลิกใบเสร็จ
        [WebMethod]
        public int InitCancelRecieve(String wsPass, String member_no, String recvperiod, String xmlhead, String xmlrecept)
        {
            return new LoanAssistSvEn(wsPass).InitCancelRecieve(member_no, recvperiod, xmlhead, xmlrecept);
        }

        //may บันทึกยกเลิกใบเสร็จ
        [WebMethod]
        public int SaveCancelRecieve(String wsPass, String xmlhead, String xmlreceipt, DateTime adtm_returndate, String as_userid, String as_branchid)
        {
            return new LoanAssistSvEn(wsPass).SaveCancelRecieve(xmlhead, xmlreceipt, adtm_returndate, as_userid, as_branchid);
        }
        // a ยกเลิกสัญญาเงินกู้
        [WebMethod]
        public String InitReqContCancel(String wsPass, String as_contno)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.InitReqContCancel(as_contno);

        }
        //a บันทึก ยกเลิกสัญญาเงินกู้
        [WebMethod]
        public int SaveReqContCancel(String wsPass, String as_xmlcontccl, String as_cancelid, DateTime adtm_cancel)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.SaveReqContCancel(as_xmlcontccl, as_cancelid, adtm_cancel);
        }
        //a รับชำระ
        [WebMethod]
        public String[] InitSlipPayIn(String wsPass, String ls_memno, String ls_sliptype, DateTime ldtm_slipdate, DateTime ldtm_opedate)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.InitSlipPayIn(ls_memno, ls_sliptype, ldtm_slipdate, ldtm_opedate);
        }
        //a
        [WebMethod]
        public String InitSlipPayInCalInt(String wsPass, String as_xmlloan, String as_sliptype, DateTime datecal)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.InitSlipPayInCalInt(as_xmlloan, as_sliptype, datecal);
        }
        //a บันทึกรับชำระ
        [WebMethod]
        public int SaveSlipPayIn(String wsPass, String xml_sliphead, String xml_sliplon, String ls_entry_id, String ls_branch_id)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.SaveSlipPayIn(xml_sliphead, xml_sliplon, ls_entry_id, ls_branch_id);
        }
        //a initlisจ่าย
        [WebMethod]
        public String InitListLnRcv(String wsPass)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.InitListLnRcv();
        }
        //a จ่ายเงินกู้
        [WebMethod]
        public str_sliplspayout InitLnRcv(String wsPass, str_sliplspayout strSlipPayOut)
        {
            return new LoanAssistSvEn(wsPass).InitLnRcv(strSlipPayOut);
        }
        //a save จ่ายเงินกู้
        [WebMethod]
        public int SaveLnRcv(String wsPass, str_sliplspayout strSlipPayOut)
        {
            LoanAssistSvEn ls = new LoanAssistSvEn(wsPass);
            return ls.SaveLnRcv(strSlipPayOut);
        }
        // Boom กำหนดค่าเริ่มต้นใบคำขอกู้
        [WebMethod]
        public short InitLoanReq(String wsPass, ref str_lsloanright strLoanright)
        {
            return new LoanAssistSvEn(wsPass).InitLoanReq(ref strLoanright);
        }

        // Boom บันทึกใบคำขอกู้
        [WebMethod]
        public short SaveLoanReq(String wsPass, str_lsloanright strLoanright)
        {
            return new LoanAssistSvEn(wsPass).SaveReqLoan(strLoanright);
        }

        // Boom สร้างเลขที่สัญญา
        [WebMethod]
        public String GenNewContNo(String wsPass, String strLoantype)
        {
            return new LoanAssistSvEn(wsPass).GenNewContNo(strLoantype);
        }

        //may โอนหนี้ให้ผู้ค้ำ
        [WebMethod]
        public str_lntrncoll InitLnTrnColl(String wsPass, str_lntrncoll astr_trncoll)
        {
            return new LoanAssistSvEn(wsPass).InitLnTrnColl(astr_trncoll);
        }

        //may โอนหนี้ให้ผู้ค้ำ (เฉลี่ยให้ผู้ค้ำ)
        [WebMethod]
        public String[] InitLnTrnCollRecalTrn(String wsPass, String as_xmlmast, String as_xmltrndet)
        {
            return new LoanAssistSvEn(wsPass).InitLnTrnCollRecalTrn(as_xmlmast, as_xmltrndet);
        }

        //may บันทึก โอนหนี้ให้ผู้ค้ำ
        [WebMethod]
        public int SaveLnTrnColl(String wsPass, str_lntrncoll astr_trncoll)
        {
            return new LoanAssistSvEn(wsPass).SaveLnTrnColl(astr_trncoll);
        }
    }
}
