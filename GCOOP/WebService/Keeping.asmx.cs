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
    /// Summary description for Keeping
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Keeping : System.Web.Services.WebService
    {
        [WebMethod]
        public int RunRcvProcess(String wsPass, String xml, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            KpRcvProcess kp = new KpRcvProcess(sec.ConnectionString, xml);
            return Processing.Progressing.Add(kp, application, w_sheet_id, true);
        }

        [WebMethod]
        public int RunPostProcess(String wsPass, String xml, String Application, String CurrentPage)
        {
            Security sec = new Security(wsPass);
            KpPostProgress kpPost = new KpPostProgress(sec.ConnectionString, xml);
            return Processing.Progressing.Add(kpPost, Application, CurrentPage, true);
        }

        [WebMethod]
        public int RunPostEmrcont(String wsPass, short year, short month, DateTime apvdate, string userid, String app, String w_sheet)
        {
            Security sec = new Security(wsPass);
            KpPostErmcont kpEmr = new KpPostErmcont(sec.ConnectionString, year, month, apvdate, userid);
            return Processing.Progressing.Add(kpEmr, app, w_sheet, true);
        }

        [WebMethod]
        public int RunGenDiskProcess(String wsPass, String as_rcvpriod, DateTime adtm_receipt, String as_diskcode, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            KpGendiskProcess kp = new KpGendiskProcess(sec.ConnectionString, as_rcvpriod, adtm_receipt, as_diskcode);
            return Processing.Progressing.Add(kp, application, w_sheet_id, true);
        }

        [WebMethod]
        public String CancelProcess(String app, String w_sheet_id)
        {
            Progressing.Cancel(app, w_sheet_id);
            return "true";
        }

        [WebMethod]
        public int InitReceiptReturn(String wsPass, String as_memno, String as_recvperiod, ref String as_xmlhead, ref String as_xmlreceipt)
        {

            KeepingSvEn kp = new KeepingSvEn(wsPass);
            return kp.InitReceiptReturn(as_memno, as_recvperiod, ref as_xmlhead, ref as_xmlreceipt);
        }

        [WebMethod]
        public int SaveReceiptReturn(String wsPass, String as_xmlhead, String as_xmlreceipt, DateTime adtm_returndate, String as_userid, String as_branchid)
        {

            KeepingSvEn kp = new KeepingSvEn(wsPass);
            return kp.SaveReceiptReturn(as_xmlhead, as_xmlreceipt, adtm_returndate, as_userid, as_branchid);
        }
        //a
        // decodeจากแผ่น 
        [WebMethod]
        public String decode(String wsPass, String as_recvperiod, DateTime adtm_receipt, String as_xmldata)
        {
            KeepingSvEn kp = new KeepingSvEn(wsPass);
            return kp.decode(as_recvperiod, adtm_receipt, as_xmldata);
        }
        //a
        // cutkeepจากแผ่น
        [WebMethod]
        public int cutkeep(String wsPass, String as_recvperiod, String as_xmldata)
        {
            KeepingSvEn kp = new KeepingSvEn(wsPass);
            return kp.cutkeep(as_recvperiod, as_xmldata);
        }
        //EGAT
        //Mai InitPauseLoanKeep หน้าจองดกู้แบบกำหนดระยะเวลา
        [WebMethod]
        public int InitPauseLoanKeep(String wsPass, ref str_keep astr_keep)
        {
            KeepingSvEn Kp = new KeepingSvEn(wsPass);
            return Kp.InitPauseLoanKeep(ref astr_keep);
        }

        //Mai SavePauseLoanKeep หน้าจองดกู้แบบกำหนดระยะเวลา
        //[WebMethod]
        //public int SavePauseLoanKeep(String wsPass, str_keep astr_keep)
        //{
        //    KeepingSvEn Kp = new KeepingSvEn(wsPass);
        //    return Kp.SavePauseLoanKeep(astr_keep);
        //}

        //Mai หน้าจอ IMP & EXP แผ่นดิสก์
        [WebMethod]
        public int DiskProcess(String wsPass, ref str_keep astr_keep)
        {
            KeepingSvEn Kp = new KeepingSvEn(wsPass);
            return Kp.DiskProcess(ref astr_keep);
        }

        //MAI
        //progressbar ประมวลปันผล
        [WebMethod]
        public int RunPauseLoanProcess(String wsPass, str_keep astr_keep, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            KpPauseLoan kp = new KpPauseLoan(sec.ConnectionString, astr_keep);
            return Processing.Progressing.Add(kp, application, w_sheet_id, true);
        }

        //Mai InitRemainPay หน้าจอชำระยอดค้างซองเงินเดือน
        [WebMethod]
        public int InitRemainPay(String wsPass, ref str_keep astr_keep)
        {
            KeepingSvEn Kp = new KeepingSvEn(wsPass);
            return Kp.InitRemainPay(ref astr_keep);
        }

        //Mai SavePauseLoanKeep หน้าจองดกู้แบบกำหนดระยะเวลา
        [WebMethod]
        public int SaveRemainPay(String wsPass, str_keep astr_keep)
        {
            KeepingSvEn Kp = new KeepingSvEn(wsPass);
            return Kp.SaveRemainPay(astr_keep);
        }

    }
}
