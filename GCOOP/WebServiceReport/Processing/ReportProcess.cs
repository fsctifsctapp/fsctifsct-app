using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using pbreport;
using System.IO;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace WebServiceReport.Processing
{
    public class ReportProcess : MainProgress, Running
    {
        n_cst_reportservice svReport;
        n_cst_progresscontrol svProgress;
        n_cst_dbconnectservice svCon;
        Security security;

        String swPass;
        String pkApplication;
        String pkGroupID;
        String pkReportID;
        String userID;
        String xmlCriteria;
        string pdfFileName;
        bool winPrint = false;
        str_progress winProgress;

        #region Constructor

        public ReportProcess(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String xmlCriteria, string pdfFileName)
        {

            security = new Security(wsPass);
            this.connectString = security.ConnectionString;

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svProgress = new n_cst_progresscontrol();
            Progress = svProgress.of_get_progress();

            svReport = new n_cst_reportservice();
            svReport.of_settrans(ref svCon);
            svReport.of_setprogress(ref svProgress);

            this.pkApplication = pkApplication;
            this.pkGroupID = pkGroupID;
            this.pkReportID = pkReportID;
            this.userID = "unknow";
            this.xmlCriteria = xmlCriteria;
            this.pdfFileName = pdfFileName;

            this.winProgress = new str_progress();

            SetRunning(this);
        }

        public ReportProcess(String wsPass, String pkApplication, String pkGroupID, String pkReportID, String userID, String xmlCriteria, string pdfFileName)
        {
            security = new Security(wsPass);
            this.connectString = security.ConnectionString;

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svProgress = new n_cst_progresscontrol();
            Progress = svProgress.of_get_progress();

            svReport = new n_cst_reportservice();
            svReport.of_settrans(ref svCon);
            svReport.of_setprogress(ref svProgress);

            this.pkApplication = pkApplication;
            this.pkGroupID = pkGroupID;
            this.pkReportID = pkReportID;
            this.userID = userID;
            this.xmlCriteria = xmlCriteria;
            this.pdfFileName = pdfFileName;

            this.winProgress = new str_progress();

            SetRunning(this);
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~ReportProcess()
        {
            DisConnect();
        }

        #endregion

        private void ClearOldPDF()
        {
            try
            {
                n_cst_xmlconfig lnv_xmlconf = new n_cst_xmlconfig();
                String ls_path = lnv_xmlconf.of_getconstantvalue("reportservice.pdfpath");
                String ls_delete = lnv_xmlconf.of_getconstantvalue("reportservice.ws.pdfdeletetime");
                long deleteSecoundXML = Convert.ToInt64(ls_delete);

                DirectoryInfo drrInfo = new DirectoryInfo(ls_path);

                foreach (FileInfo ls_file in drrInfo.GetFiles())
                {
                    try
                    {
                        String ls_filename = ls_file.Name;
                        ls_filename = ls_filename.Substring(0, 14);
                        CultureInfo en = new CultureInfo("en-US");
                        DateTime fileTime = DateTime.ParseExact(ls_filename, "yyyyMMddHHmmss", en);
                        long totalDelete = Convert.ToInt64((DateTime.Now - fileTime).TotalMilliseconds / 1000);
                        if (totalDelete >= deleteSecoundXML) //too old.
                        {
                            ls_file.Delete();
                        }
                    }
                    catch { }
                }
                DisConnect();
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        #region Running Members

        public pbreport.str_progress GetProgress()
        {
            if (winPrint)
            {
                return winProgress;
            }
            else
            {
                svReport.of_getprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    int li_pdfmethod;
                    n_cst_xmlconfig lnv_xmlconf = new n_cst_xmlconfig();
                    li_pdfmethod = Convert.ToInt32(lnv_xmlconf.of_getconstantvalue("reportservice.ws.pdfmethod"));
                    if (li_pdfmethod == 1)
                    {
                        int li_return = svReport.of_report_print_pdf(pkApplication, pkGroupID, pkReportID, userID, xmlCriteria, pdfFileName);

                        try
                        {
                            ClearOldPDF();
                        }
                        catch (Exception ex)
                        { }
                        DisConnect();
                    }
                    else //pdfmethod == 2.
                    {
                        winPrint = true;
                        winProgress.progress_index = 0;
                        winProgress.progress_max = 1;
                        winProgress.progress_text = "WinPrint";
                        winProgress.status = 8;
                        winProgress.subprogress_index = 0;
                        winProgress.subprogress_max = 1;
                        winProgress.subprogress_text = "WinPrint";
                        winProgress.error_text = "";
                        try
                        {
                            string results = new WinPrintCalling(connectString).CallWinPrint("report_all", lnv_xmlconf.of_getconstantvalue("reportservice.ws.pdfwinprintcmd"), new String[7] { pkApplication, pkGroupID, pkReportID, xmlCriteria, pdfFileName, connectString, userID });
                            if (results != "1") throw new Exception("WinPrint Error");
                            winProgress.status = 1;
                        }
                        catch (Exception ex)
                        {
                            winProgress.error_text = ex.Message;
                            winProgress.status = -1;
                        }
                        try
                        {
                            ClearOldPDF();
                        }
                        catch (Exception ex)
                        { }
                        DisConnect();
                    }

                }
                catch (Exception ex)
                {
                    DisConnect();
                    throw ex;
                }
            }
        }

        #endregion
    }
}
