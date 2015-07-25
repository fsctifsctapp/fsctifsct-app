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

namespace WebService
{
    /// <summary>
    /// Summary description for Print
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Print : System.Web.Services.WebService
    {

        [WebMethod]
        public int PrintPDF(String wsPass, String xmlPrint, string pdfFileName)
        {
            PrintSvEn lnv_rpt = new PrintSvEn(wsPass);
            int li_return = lnv_rpt.PrintPDF(xmlPrint, pdfFileName);
            return li_return;
        }

        [WebMethod]
        public int ReportPDF_PrintServer(String wsPass, String xmlPrint, string pdfFileName)
        {
            PrintSvEn lnv_rpt = new PrintSvEn(wsPass);
            int li_return = lnv_rpt.PrintPDF_PrintServer(xmlPrint, pdfFileName);
            return li_return;
        }

        [WebMethod]
        public String GetPDFURL(String wsPass)
        {
            ReportSvEn lnv_rpt = new ReportSvEn(wsPass);
            String ls_return = lnv_rpt.GetPDFURL();
            return ls_return;
        }

    }
}
