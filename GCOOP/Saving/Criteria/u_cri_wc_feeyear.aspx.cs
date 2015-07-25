﻿using System;
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
    public partial class u_cri_wc_feeyear : PageWebSheet, WebSheet
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String PostPost;
        protected String clr_editbox;
        protected String jsbranch_id;
        protected String jsbranch_desc;

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            clr_editbox = WebUtil.JsPostBack(this, "clr_editbox");
            jsbranch_id = WebUtil.JsPostBack(this, "jsbranch_id");
            jsbranch_desc = WebUtil.JsPostBack(this, "jsbranch_desc");
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
                dw_criteria.SetItemString(1, "branch_idd", state.SsBranchId);
                dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                dw_criteria.SetItemString(1, "year", Syear);
                // dw_criteria.SetItemDateTime(1, "as_startdate", new DateTime(2011, 6, 1));
                // dw_criteria.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
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
            else if (eventArg == "jsbranch_id")
            {
                ChangeBranchId();
            }
            else if (eventArg == "jsbranch_desc")
            {
                ChangeBranchDesc();
            }


        }

        /*public void Clr_editbox()
        {
            Double branch_all = dw_criteria.GetItemDouble(1, "branch_all");
            if (branch_all != 1)
            {
                dw_criteria.SetItemString(1, "branch_id_1", "");
            }
            dw_criteria.SetItemString(1, "membtype_start", "");
            dw_criteria.SetItemString(1, "membtype_end", "");
        }*/

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

            string not_cstype, cstype;

            not_cstype = "0";
            cstype = state.SsCsType;

            try
            {
                dw_criteria.Modify("bsearch.visible=0");
                DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "branch_idd", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);


            }
            catch { }

            dw_criteria.SaveDataCache();
        }

        public void ChangeBranchId()
        {
            //int row = Convert.ToInt32(HdRow.Value);
            try
            {
                dw_criteria.SetItemString(1, "branch_idd", dw_criteria.GetItemString(1, "branch_id"));
                // jjsselect_option();
            }
            catch { }
        }

        public void ChangeBranchDesc()
        {
            // int row = Convert.ToInt32(HdRow.Value);
            try
            {
                dw_criteria.SetItemString(1, "branch_id", dw_criteria.GetItemString(1, "branch_idd"));
                // jjsselect_option();
            }
            catch { }
        }

        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String ascstype = state.SsCsType;
            String branch_id = dw_criteria.GetItemString(1, "branch_id");
            String status = dw_criteria.GetItemString(1, "status");
            String year = dw_criteria.GetItemString(1, "year");
            Decimal yearr = Convert.ToInt32(year);
            String ym = yearr - 1 + "12";
            Decimal all = dw_criteria.GetItemDecimal(1, "all");
            String coop_name = state.SsCoopName;

            if (all == 1)
            {
                status = "%";
            }

            ReportHelper lnv_helper = new ReportHelper();

            //   string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();

            lnv_helper.AddArgument(ym, ArgumentType.String);
            lnv_helper.AddArgument(branch_id, ArgumentType.String);
            lnv_helper.AddArgument(status, ArgumentType.String);
            lnv_helper.AddArgument(ascstype, ArgumentType.String);
            lnv_helper.AddArgument(year, ArgumentType.String);


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