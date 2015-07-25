using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using pbreport;
using WebServiceReport.Processing;

namespace WebServiceReport
{
    /// <summary>
    /// Summary description for Report
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Report : System.Web.Services.WebService
    {
        #region No Process Unit (Normal)

        [WebMethod]
        public String ReportXML(String dbServer, String dbUser, String dbPass, String pkApplication, String pkGroupID, String pkReportID, String xmlCriteria)
        {
            //wsPass ให้ส่งเป้น connectionString มา.
            //ตัวอย่าง connectionString:("Data Source=webcenter3/gcoop;Persist Security Info=True;User ID=scobkcat;Password=scobkcat;")
            n_cst_dbconnectservice svCon;
            n_cst_reportservice svPrint;
            String dbConnect = "Data Source="+dbServer+";Persist Security Info=True;User ID="+dbUser+";Password="+dbPass+";";
            try
            {
                svCon = new n_cst_dbconnectservice();
                svCon.of_connectdb(dbConnect);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            try
            {
                svPrint = new n_cst_reportservice();
                svPrint.of_settrans(ref svCon);
                String ls_return = svPrint.of_report_xml(pkApplication, pkGroupID, pkReportID, xmlCriteria);
                svCon.of_disconnectdb();
                return ls_return;
            }
            catch (Exception ex)
            {
                try
                {
                    svCon.of_disconnectdb();
                }
                catch { }
                throw ex;
            }

        }

        [WebMethod]
        public int ReportPDF(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String xmlCriteria, string pdfFileName)
        {
            ReportSvEn lnv_rpt = new ReportSvEn(wsPass);
            int li_return = lnv_rpt.ReportPDF(pkApplication, pkGroupID, pkReportID, xmlCriteria, pdfFileName);
            return li_return;
        }

        [WebMethod]
        public int ReportPDF_PrintServer(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String xmlCriteria, string pdfFileName)
        {
            ReportSvEn lnv_rpt = new ReportSvEn(wsPass);
            int li_return = lnv_rpt.ReportPDF_PrintServer(pkApplication, pkGroupID, pkReportID, xmlCriteria, pdfFileName);
            return li_return;
        }

        [WebMethod]
        public String GetPDFURL(String wsPass)
        {
            ReportSvEn lnv_rpt = new ReportSvEn(wsPass);
            String ls_return = lnv_rpt.GetPDFURL();
            return ls_return;
        }

        [WebMethod]
        public str_webreportdetail GetReportDetail(String wsPass, String pkApplication, String pkGroupID, String pkReportID)
        {
            ReportSvEn lnv_rpt = new ReportSvEn(wsPass);
            str_webreportdetail lstr = lnv_rpt.GetReportDetail(pkApplication, pkGroupID, pkReportID);
            return lstr;
        }

        #endregion

        #region Processing Unit

        [WebMethod]
        public string Run(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String xmlCriteria, string pdfFileName)
        {
            String w_sheet_id = pkGroupID + "_" + pkReportID;
            try
            {
                Processing.Progressing.Remove(pkApplication, w_sheet_id);
            }
            catch
            {
            }
            Security sec = new Security(wsPass);
            ReportProcess pbp = new ReportProcess(wsPass, pkApplication, pkGroupID, pkReportID, xmlCriteria, pdfFileName);
            Processing.Progressing.Add(pbp, pkApplication, w_sheet_id);
            return "true";
        }

        [WebMethod]
        public string RunWithID(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String userID, String xmlCriteria, string pdfFileName)
        {
            String w_sheet_id = pkGroupID + "_" + pkReportID;
            try
            {
                Processing.Progressing.Remove(pkApplication, w_sheet_id);
            }
            catch
            {
            }
            Security sec = new Security(wsPass);
            ReportProcess pbp = new ReportProcess(wsPass, pkApplication, pkGroupID, pkReportID, userID, xmlCriteria, pdfFileName);
            Processing.Progressing.Add(pbp, pkApplication, w_sheet_id);
            return "true";
        }

        [WebMethod]
        public String GetStatus(String wspass, String pkApplication, String pkGroupID, String pkReportID)
        {
            Security sec = new Security(wspass);
            string[] s = Progressing.GetStatus(pkApplication, pkGroupID + "_" + pkReportID);
            string ss = "{0},{1},{2},{3},{4},{5},{6},{7}"; //ห้ามคั่นด้วยคอมม่าเพราะจะตัดผิดเมื่อมีข้อความ error โผล่มา.
            String result = String.Format(ss, s);
            if (result.StartsWith("1"))
            {
                //Processing.Progressing.Remove("shrlon", "w_sheet_sl_principal_balance");
                Processing.Progressing.Remove(pkApplication, pkGroupID + "_" + pkReportID);
            }
            return result;
        }

        [WebMethod]
        public String Stop(String wsPass, String pkApplication, String pkGroupID, String pkReportID)
        {
            Processing.Progressing.Remove(pkApplication, pkGroupID + "_" + pkReportID);
            return "หยุดการประมวลผลเรียบร้อยแล้ว";
        }

        #endregion
    }
}
