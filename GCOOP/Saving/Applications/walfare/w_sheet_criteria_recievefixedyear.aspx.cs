﻿using System;
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
    public partial class w_sheet_criteria_recievefixedyear : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String reportchg;
        //private DwThDate tdw_criteria;
        //protected String PostPost;

        // protected String membtype_all;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            // PostPost = WebUtil.JsPostBack(this, "PostPost");

            // membtype_all = WebUtil.JsPostBack(this, "membtype_all");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            reportchg = WebUtil.JsPostBack(this, "reportchg");
            //tdw_criteria = new DwThDate(dw_criteria, this);
            // tdw_criteria.Add("as_startdate", "as_tstartdate");
            // tdw_criteria.Add("as_enddate", "as_tenddate");
        }

        public void WebSheetLoadBegin()
        {
            // DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
            DwUtil.RetrieveDDDW(dw_criteria, "branch_id", "criteria.pbl", null);
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_criteria);
            }
            else
            {
                dw_criteria.InsertRow(0);
                //  dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
                // dw_criteria.SetItemString(1, "start_docno", "");
                // dw_criteria.SetItemString(1, "end_docno", "");
                dw_criteria.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                dw_criteria.SetItemString(1, "for_year", Syear);

                //dw_criteria.SetItemDateTime(1, "as_startdate", new DateTime(DateTime.Today.Year, 1, 1));
                //  dw_criteria.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, 12, 31));

                //   tdw_criteria.Eng2ThaiAllRow();
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
            Decimal report = dw_criteria.GetItemDecimal(1, "report");
            if (report == 2)
            {
                gid = "walfare_daily";
                rid = "walfare_daily23_3";
            }
            else
            {
                gid = "walfare_daily";
                rid = "walfare_daily23_1";
                if (state.SsCsType == "1")
                {
                    gid = "walfare_daily";
                    rid = "walfare_daily23_2";
                }
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
                dw_criteria.SaveDataCache();
                //DwUtil.RetrieveDDDW(dw_criteria, "membtype_start", "criteria.pbl", state.SsCsType);
                //DwUtil.RetrieveDDDW(dw_criteria, "membtype_end", "criteria.pbl", state.SsCsType);
                // String cstype = dw_criteria.GetItemString(1, "as_cstype");
                // DataWindowChild dc = dw_criteria.GetChild("branch_id");
                //dc.SetFilter("cs_type='" + cstype + "'");
                // dc.Filter();
            }
            catch { }
        }
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            //String start_docno = dw_criteria.GetItemString(1, "start_docno");
            String for_year = dw_criteria.GetItemString(1, "for_year");
            String branch_id = dw_criteria.GetItemString(1, "branch_id");
            String ascstype = state.SsCsType;
            String recv_period = (Convert.ToInt32(for_year) - 1).ToString() + "12";
            // String start_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "as_tstartdate", null);
            //String end_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "as_tenddate", null);
            String coop_name = state.SsCoopName;
            //string start_memb = dw_criteria.GetItemString(1, "membtype_start");
            //string end_memb = dw_criteria.GetItemString(1, "membtype_end");
            //string order_by = dw_criteria.GetItemString(1, "order_by");
            //end_memb = end_memb + " and order by " + order_by;
            ReportHelper lnv_helper = new ReportHelper();

            // Double branch_all = dw_criteria.GetItemDouble(1, "branch_all");
            // Double membtype_all = dw_criteria.GetItemDouble(1, "membtype_all");


            //lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
            //lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
            //lnv_helper.AddArgument(branch_id, ArgumentType.String);
            lnv_helper.AddArgument(branch_id, ArgumentType.String);
            lnv_helper.AddArgument(recv_period, ArgumentType.String);
            lnv_helper.AddArgument(for_year, ArgumentType.String);
            lnv_helper.AddArgument(ascstype, ArgumentType.String);
            //lnv_helper.AddArgument(start_memb, ArgumentType.String);
            // lnv_helper.AddArgument(end_memb, ArgumentType.String);

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
                        pdfUtil.DesFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.desfile") + '-' + branch_id + "-" + dt.GetString("coopbranch_desc") + ".pdf";
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
