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
using DBAccess;
using pbservice;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Globalization;

namespace WebService
{
    public class PrintSvEn
    {

        private n_cst_dbconnectservice svCon;
        private n_cst_printservice svPrint;

        private Security security;

        public PrintSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public PrintSvEn(String wsPass, bool autoConnect)
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
                svPrint = new n_cst_printservice();
                svPrint.of_settrans(ref svCon);
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

        ~PrintSvEn()
        {
            DisConnect();
        }

        /***********************************************************/

        private void ClearOldPDF()
        {
            try
            {
                n_cst_xmlconfig lnv_xmlconf = new n_cst_xmlconfig();
                String ls_path = lnv_xmlconf.of_getconstantvalue("printservice.pdfpath");
                String ls_delete = lnv_xmlconf.of_getconstantvalue("printservice.ws.pdfdeletetime");
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
                //DisConnect();
            }
            catch (Exception ex)
            {
                //DisConnect();
                throw ex;
            }
        }

        public int PrintPDF(String xmlPrint, String pdfFileName)
        {
            //return PDFFileName
            try
            {
                int li_pdfmethod;
                n_cst_xmlconfig lnv_xmlconf = new n_cst_xmlconfig();
                li_pdfmethod = Convert.ToInt32(lnv_xmlconf.of_getconstantvalue("printservice.ws.pdfmethod"));
                if (li_pdfmethod == 1)
                {
                    try
                    {
                        ClearOldPDF();
                    }
                    catch (Exception ex)
                    { }
                    int li_return = svPrint.of_print_pdf(xmlPrint, pdfFileName);
                    DisConnect();
                    return li_return;
                }
                else //pdfmethod == 2.
                {
                    DisConnect();
                    return PrintPDF_PrintServer(xmlPrint, pdfFileName);
                }

            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int PrintPDF_PrintServer(String xmlPrint, string pdfFileName)
        {

            TcpClient tcpclnt = new TcpClient();
            try
            {
                n_cst_xmlconfig lnv_xmlconf = new n_cst_xmlconfig();
                String result = "";
                String[] ss = new String[10];
                ss[0] = lnv_xmlconf.of_getconstantvalue("printservice.ws.pdfwinprintcmd");  //"printpdf";  //commandCode
                ss[1] = security.ConnectionString;
                ss[2] = xmlPrint;
                ss[3] = pdfFileName;
                String sender = String.Format("{0},{1},{2},{3}", ss);
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
                return Convert.ToInt32(result); // 1 or -1.
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

        public String GetPDFURL()
        {
            try
            {
                String ls_url = svPrint.of_getprintpdfurl();
                DisConnect();
                return ls_url;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

    }
}