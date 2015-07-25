using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using System.IO;

namespace Saving.Criteria
{
    public partial class AjaxSlPrincipalBalance : System.Web.UI.Page
    {
        protected String percent;
        private WebState state;
        private CommonLibrary.WsReport.Report wsReport;

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState();
            wsReport = WsUtil.Report;
            try
            {
                String app = Request["app"].ToString();
                String gid = Request["gid"].ToString();
                String rid = Request["rid"].ToString();
                percent = wsReport.GetStatus(state.SsWsPass, app, Session.SessionID, gid, rid);
                CopyPDF(percent);
            }
            catch (Exception ex)
            {
                percent = "ไม่มีสถานะการประมวลผล ... " + ex.Message;
            }
        }

        private void CopyPDF(String percent)
        {
            try
            {
                string[] per = percent.Split(',');
                int stat = int.Parse(per[0]);
                PDFUtil pdfUtil = new PDFUtil(Session);
                if (stat == 1 && pdfUtil.IsSendPDF)
                {
                    File.Copy(pdfUtil.SourceFile, pdfUtil.DesFile, true);
                }
            }
            catch { }
        }
    }
}
