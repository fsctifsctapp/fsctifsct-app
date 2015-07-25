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
    public partial class u_cri_wc_summary_all : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        private DwThDate tdw_criteria;
        protected String PostPost;
        protected String clr_editbox;
       // protected String membtype_all;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            clr_editbox = WebUtil.JsPostBack(this, "clr_editbox");
          // membtype_all = WebUtil.JsPostBack(this, "membtype_all");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            tdw_criteria = new DwThDate(dw_criteria, this);
            tdw_criteria.Add("as_startdate", "as_tstartdate");
            tdw_criteria.Add("as_enddate", "as_tenddate");
        }

        public void WebSheetLoadBegin()
        {
            DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
            DwUtil.RetrieveDDDW(dw_criteria, "branch_id", "criteria.pbl", null);
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_criteria);
            }
            else
            {
                dw_criteria.InsertRow(0);
                dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);
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
            else if (eventArg == "clr_editbox")
            {
                Clr_editbox();
            }
            
        }

       
        public void Clr_editbox()
        {
            Double branch_all = dw_criteria.GetItemDouble(1, "branch_all");
            if (branch_all != 1)
            {
                dw_criteria.SetItemString(1, "branch_id_1", "");
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_criteria, "membtype_start", "criteria.pbl", state.SsCsType);
                DwUtil.RetrieveDDDW(dw_criteria, "membtype_end", "criteria.pbl", state.SsCsType);
                string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();
                String cstype = state.SsCsType;
                DataWindowChild dc = dw_criteria.GetChild("as_cstype");
                dc.SetFilter("cs_type='" + cstype + "'");
                dc.Filter();
                DataWindowChild dcs = dw_criteria.GetChild("membtype_start");
                DataWindowChild dce = dw_criteria.GetChild("membtype_start");
                switch (branch_all)
                {
                    case "3":
                        dcs.SetFilter("wftype_code in('04', '05') ");
                        dcs.Filter();
                        dce.SetFilter("wftype_code in('04', '05') ");
                        dce.Filter();
                        break;
                    case "2":
                        dcs.SetFilter("wftype_code not in('04', '05') ");
                        dcs.Filter();
                        dce.SetFilter("wftype_code not in('04', '05') ");
                        dce.Filter();
                        break;
                }
            }
            catch { }
        }
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String ascstype = dw_criteria.GetItemString(1, "as_cstype");
            String branch_id = dw_criteria.GetItemString(1, "branch_id");

            String coop_name = state.SsCoopName;
            string start_memb = dw_criteria.GetItemString(1, "membtype_start");
            string end_memb = dw_criteria.GetItemString(1, "membtype_end");
            ReportHelper lnv_helper = new ReportHelper();

            string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();

            switch (branch_all)
            {
                case "1":
                    lnv_helper.AddArgument(branch_id, ArgumentType.String);
                    lnv_helper.AddArgument(ascstype, ArgumentType.String);
                    lnv_helper.AddArgument(start_memb, ArgumentType.String);
                    lnv_helper.AddArgument(end_memb, ArgumentType.String);
                    break;
                default:
                    lnv_helper.AddArgument(ascstype, ArgumentType.String);
                    lnv_helper.AddArgument(start_memb, ArgumentType.String);
                    lnv_helper.AddArgument(end_memb, ArgumentType.String);
                    break;                
            }
            
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
