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
    public partial class u_cri_rdocno_montly : PageWebSheet, WebSheet
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
            //tdw_criteria.Add("as_startdate", "as_tstartdate");
            //tdw_criteria.Add("as_enddate", "as_tenddate");
        }

        public void WebSheetLoadBegin()
        {
            DwUtil.RetrieveDDDW(dw_criteria, "sgroup_code", "criteria.pbl", null);
            //DwUtil.RetrieveDDDW(dw_criteria, "egroup_code", "criteria.pbl", null);
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_criteria);
            }
            else
            {
                dw_criteria.InsertRow(0);
                //dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                dw_criteria.SetItemString(1, "sgroup_code", state.SsBranchId);
                //dw_criteria.SetItemString(1, "egroup_code", state.SsBranchId);
                //dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);

                //dw_criteria.SetItemDateTime(1, "as_startdate", new DateTime(DateTime.Today.Year, 1, 1));
                //dw_criteria.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, 12, 31));

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
            String ascstype = state.SsCsType;//dw_criteria.GetItemString(1, "as_cstype");
            String branch_id = state.SsBranchId;
            String mth = dw_criteria.GetItemString(1, "month");
            mth = (mth.Length == 1) ? "0"+mth : mth;

            String period = dw_criteria.GetItemString(1, "year") + mth;

            String sgroup_code = dw_criteria.GetItemString(1, "sgroup_code");
            //String egroup_code = dw_criteria.GetItemString(1, "egroup_code");

            String coop_name = state.SsCoopName;
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(period, ArgumentType.String);
            lnv_helper.AddArgument(sgroup_code, ArgumentType.String);
            lnv_helper.AddArgument(sgroup_code, ArgumentType.String);
            lnv_helper.AddArgument(ascstype, ArgumentType.String);


            

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
