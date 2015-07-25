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
    public partial class w_sheet_wc_criteria_sumary_all : PageWebSheet, WebSheet
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String PostPost;
        protected String clr_editbox;

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            clr_editbox = WebUtil.JsPostBack(this, "clr_editbox");
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
                dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);
                dw_criteria.SetItemDateTime(1, "as_startdate", new DateTime(2011, 6, 1));
                dw_criteria.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
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

            string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();
            switch (branch_all)
            {
                case "1":
                    gid = "walfare_daily";
                    rid = "walfare_daily22_1";
                    break;
                case "2":
                    gid = "walfare_daily";
                    rid = "walfare_daily22_2";
                    break;
                case "3":
                    gid = "walfare_daily";
                    rid = "walfare_daily22_3";
                    break;
                case "4":
                    gid = "walfare_daily";
                    rid = "walfare_daily22_4";
                    break;
            } 
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
            dw_criteria.SetItemString(1, "membtype_start", "");
            dw_criteria.SetItemString(1, "membtype_end", "");
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                dw_criteria.SaveDataCache();
                DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "branch_id", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "membtype_start", "criteria.pbl", state.SsCsType);
                DwUtil.RetrieveDDDW(dw_criteria, "membtype_end", "criteria.pbl", state.SsCsType);
                DwUtil.RetrieveDDDW(dw_criteria, "as_startdate", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "as_enddate", "criteria.pbl", null);
                string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();
                String cstype = state.SsCsType;
                DataWindowChild dc = dw_criteria.GetChild("as_cstype");
                dc.SetFilter("cs_type='" + cstype + "'");
                dc.Filter();
                DataWindowChild dcs = dw_criteria.GetChild("membtype_start");
                DataWindowChild dce = dw_criteria.GetChild("membtype_end");
                switch (branch_all)
                {
                    case "1":
                        break;
                    case "2":
                        dcs.SetFilter("wftype_code not in('04', '05') ");
                        dcs.Filter();
                        dce.SetFilter("wftype_code not in('04', '05') ");
                        dce.Filter();
                        break;
                    case "3":
                        dcs.SetFilter("wftype_code in('04', '05') ");
                        dcs.Filter();
                        dce.SetFilter("wftype_code in('04', '05') ");
                        dce.Filter();
                        break;
                    case "4":
                        //dcs.SetFilter("wftype_code not in('04', '05') ");
                        //dcs.Filter();
                        //dce.SetFilter("wftype_code not in('04', '05') ");
                        //dce.Filter();
                        break;
                }
            }
            catch { }
        }
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String ascstype = state.SsCsType;
            String branch_id = dw_criteria.GetItemString(1, "branch_id");

            String coop_name = state.SsCoopName;
            string start_memb = dw_criteria.GetItemString(1, "membtype_start");
            string end_memb = dw_criteria.GetItemString(1, "membtype_end");
            String start_date = dw_criteria.GetItemString(1, "as_startdate");
            String end_date = dw_criteria.GetItemString(1, "as_enddate");
            ReportHelper lnv_helper = new ReportHelper();

            string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();

            switch (branch_all)
            {
                case "1":
                    lnv_helper.AddArgument(branch_id, ArgumentType.String);
                    lnv_helper.AddArgument(ascstype, ArgumentType.String);
                    lnv_helper.AddArgument(start_memb, ArgumentType.String);
                    lnv_helper.AddArgument(end_memb, ArgumentType.String);
                    lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                    lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
                    break;
                default:
                    lnv_helper.AddArgument(ascstype, ArgumentType.String);
                    lnv_helper.AddArgument(start_memb, ArgumentType.String);
                    lnv_helper.AddArgument(end_memb, ArgumentType.String);
                    lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                    lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
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