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
using pbservice;
using System.IO;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace WebService.Processing
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
        String xmlCriteria;
        string pdfFileName;

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
            this.xmlCriteria = xmlCriteria;
            this.pdfFileName = pdfFileName;

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

        public pbservice.str_progress GetProgress()
        {
            svReport.of_getprogress(ref svProgress);
            return svProgress.of_get_progress();
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
                        int li_return = svReport.of_report_print_pdf(pkApplication, pkGroupID, pkReportID, xmlCriteria, pdfFileName);
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
                        DisConnect();

                        TcpClient tcpclnt = new TcpClient();
                        try
                        {
                            String result = "";
                            String[] ss = new String[10];
                            ss[0] = lnv_xmlconf.of_getconstantvalue("reportservice.ws.pdfwinprintcmd");  //"reportpdf";  //commandCode
                            ss[1] = connectString;
                            ss[2] = pkApplication;
                            ss[3] = pkGroupID;
                            ss[4] = pkReportID;
                            ss[5] = xmlCriteria;
                            ss[6] = pdfFileName;
                            String sender = String.Format("{0},{1},{2},{3},{4},{5},{6}", ss);
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
                            try
                            {
                                ClearOldPDF();
                            }
                            catch (Exception ex)
                            { }
                            DisConnect();
                        }
                        catch (Exception ex)
                        {
                            DisConnect();
                            try
                            {
                                tcpclnt.Close();
                            }
                            catch { }
                            throw ex;
                        }
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
