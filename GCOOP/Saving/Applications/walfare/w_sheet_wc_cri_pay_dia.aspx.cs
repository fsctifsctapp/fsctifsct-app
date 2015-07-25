using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Sybase.DataWindow;
using DBAccess;
using System.Data;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_cri_pay_dia : PageWebSheet, WebSheet
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String PostPost;
       // protected String clr_editbox;

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
           // clr_editbox = WebUtil.JsPostBack(this, "clr_editbox");
        }

        public void WebSheetLoadBegin()
        {
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_criteria);
            }
            else
            {
                dw_criteria.InsertRow(0);
                //dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                // dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);
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


            gid = "walfare_daily";
            rid = "walfare_daily99_1";

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
               
                DwUtil.RetrieveDDDW(dw_criteria, "year_mm", "criteria.pbl", null);
               
            }
            catch { }
            dw_criteria.SaveDataCache();
        }
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String ascstype = state.SsCsType;


            String coop_name = state.SsCoopName;
            string start_ym = dw_criteria.GetItemString(1, "year_mm");
           
            ReportHelper lnv_helper = new ReportHelper();



            lnv_helper.AddArgument(start_ym, ArgumentType.String);
            lnv_helper.AddArgument(start_ym, ArgumentType.String);
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
                    //String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "'";
                    //Sdt dt = WebUtil.QuerySdt(sql);
                    //if (dt.Next())
                    // {
                    pdfUtil.IsSendPDF = true;
                    pdfUtil.DesFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.desfile") + ascstype + '-' + ".pdf";
                    //}
                }
                catch { pdfUtil.IsSendPDF = false; }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }


        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        #endregion


    }
}