using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.IO;

namespace Saving.Criteria
{
    public partial class u_cri_rdate_rmembno_slip_paid : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        private DwThDate tdw_criteria;
        protected String PostPost;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            tdw_criteria = new DwThDate(dw_criteria, this);
            tdw_criteria.Add("start_date", "start_tdate");
            tdw_criteria.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            //DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
            //DwUtil.RetrieveDDDW(dw_criteria, "branch_id", "criteria.pbl", null);
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_criteria);
            }
            else
            {
                dw_criteria.InsertRow(0);
                //dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                //dw_criteria.SetItemString(1, "start_docno", "");
                //dw_criteria.SetItemString(1, "end_docno", "");
                //dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);

                dw_criteria.SetItemDateTime(1, "start_date", new DateTime(DateTime.Today.Year, 1, 1));
                dw_criteria.SetItemDateTime(1, "end_date", new DateTime(DateTime.Today.Year, 12, 31));
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                dw_criteria.SetItemString(1, "pay_year", Syear);

                tdw_criteria.Eng2ThaiAllRow();
            }
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                gid = Request["gid"].ToString();
            }
            catch { }
            try
            {
                rid = Request["rid"].ToString();
            }
            catch { }

            //Report Name.
            try
            {
                Sta ta = new Sta(state.SsConnectionString);
                String sql = "";
                sql = @"SELECT REPORT_NAME  
                    FROM WEBREPORTDETAIL  
                    WHERE ( GROUP_ID = '" + gid + @"' ) AND ( REPORT_ID = '" + rid + @"' )";
                Sdt dt = ta.Query(sql);
                ReportName.Text = dt.Rows[0]["REPORT_NAME"].ToString();
                ta.Close();
            }
            catch
            {
                ReportName.Text = "[" + rid + "]";
            }
            //rid += Session.SessionID;
            //Link back to the report menu.
            LinkBack.PostBackUrl = String.Format("~/ReportDefault.aspx?app={0}&gid={1}", app, gid);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "runProcess")
            {
                RunProcess();
            }
            else if (eventArg == "popupReport")
            {
                PopupReport();
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                String cstype = dw_criteria.GetItemString(1, "as_cstype");
                DataWindowChild dc = dw_criteria.GetChild("branch_id");
                dc.SetFilter("cs_type='" + cstype + "'");
                dc.Filter();
            }
            catch { }
        }
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            //String start_docno = dw_criteria.GetItemString(1, "start_docno");
            String start_membno = dw_criteria.GetItemString(1, "start_membno");
            String end_membno = dw_criteria.GetItemString(1, "end_membno");

            String start_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "start_tdate", null);
            String end_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "end_tdate", null);
            String slip_type = "WPF";// dw_criteria.GetItemString(1, "slip_type");
            String pay_year = dw_criteria.GetItemString(1, "pay_year");

            string branch_id = state.SsBranchId;
            string ascstype = state.SsCsType;
            ReportHelper lnv_helper = new ReportHelper();
            
            lnv_helper.AddArgument(start_membno, ArgumentType.String);
            lnv_helper.AddArgument(end_membno, ArgumentType.String);
            lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
            lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
            lnv_helper.AddArgument(slip_type, ArgumentType.String);
            lnv_helper.AddArgument(branch_id, ArgumentType.String);
            lnv_helper.AddArgument(ascstype, ArgumentType.String);
            lnv_helper.AddArgument(pay_year, ArgumentType.String);
            //-------------------------------------------------------

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();
            try
            {
                CommonLibrary.WsReport.Report lws_report = WsUtil.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, Session.SessionID, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
                PDFUtil pdfUtil = new PDFUtil(Session);
                pdfUtil.SourceFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.sourcefile") + pdfFileName;
                try
                {
                    String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        pdfUtil.IsSendPDF = true;
                        pdfUtil.DesFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.desfile") + ascstype + '-' + branch_id + "-" + dt.GetString("coopbranch_desc") + ".pdf";
                    }
                }
                catch { pdfUtil.IsSendPDF = false; }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }
        #endregion

        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        #endregion
    }
}
