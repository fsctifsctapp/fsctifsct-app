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
    public partial class w_sheet_wc_cri_rdocno_admin : PageWebSheet, WebSheet
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String PostPost;
        protected String clr_editbox;
        //private DwThDate tdw_criteria;

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            clr_editbox = WebUtil.JsPostBack(this, "clr_editbox");
            //tdw_criteria = new DwThDate(dw_criteria, this);
            //tdw_criteria.Add("as_startdate", "as_tstartdate");
            //tdw_criteria.Add("as_enddate", "as_tenddate");
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
                dw_criteria.SetItemString(1, "branch_id_e", state.SsBranchId);
                //dw_criteria.SetItemDateTime(1, "as_startdate", new DateTime(2011, 6, 1));
                //dw_criteria.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));

                //tdw_criteria.Eng2ThaiAllRow();
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
                    rid = "walfare_daily99_10";
                    break;
                case "8":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_11";
                    break;
                case "7":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_12";
                    break;
                case "6":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_13";
                    break;
                case "5":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_14";
                    break;
                case "9":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_15";
                    break;
                case "10":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_16";
                    break;
                case "11":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_17";
                    break;
                case "12":
                    gid = "walfare_daily";
                    rid = "walfare_daily99_20";
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
            //if (branch_all != 1)
            //{
            //    dw_criteria.SetItemString(1, "branch_id_1", "");
            //}
            //dw_criteria.SetItemString(1, "membtype_start", "");
            //dw_criteria.SetItemString(1, "membtype_end", "");
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            try
            {
                dw_criteria.SaveDataCache();
                String Cs_typee = state.SsCsType;
                DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "branch_id", "criteria.pbl", Cs_typee);
                DwUtil.RetrieveDDDW(dw_criteria, "branch_id_e", "criteria.pbl", Cs_typee);
                DwUtil.RetrieveDDDW(dw_criteria, "as_startdate", "criteria.pbl", Cs_typee);
                DwUtil.RetrieveDDDW(dw_criteria, "as_enddate", "criteria.pbl", Cs_typee);

              //  string branch_all = dw_criteria.GetItemDecimal(1, "branch_all").ToString();
                //String cstype = state.SsCsType;
                //DataWindowChild dc = dw_criteria.GetChild("as_cstype");
                //dc.SetFilter("cs_type='" + cstype + "'");
                //dc.Filter();
                //DataWindowChild dcs = dw_criteria.GetChild("membtype_start");
                //DataWindowChild dce = dw_criteria.GetChild("membtype_end");
                //switch (branch_all)
                //{
                //    case "1":
                //        break;
                //    case "2":
                //        dcs.SetFilter("wftype_code not in('04', '05') ");
                //        dcs.Filter();
                //        dce.SetFilter("wftype_code not in('04', '05') ");
                //        dce.Filter();
                //        break;
                //    case "3":
                //        dcs.SetFilter("wftype_code in('04', '05') ");
                //        dcs.Filter();
                //        dce.SetFilter("wftype_code in('04', '05') ");
                //        dce.Filter();
                //        break;
                //    case "4":
                //        //dcs.SetFilter("wftype_code not in('04', '05') ");
                        //dcs.Filter();
                        //dce.SetFilter("wftype_code not in('04', '05') ");
                        //dce.Filter();
                    //   break;
             // }
            }
            catch { }
        } 
        #region Report Process
        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String ascstype = state.SsCsType;
            String branch_id = dw_criteria.GetItemString(1, "branch_id");
            String branch_id_e = dw_criteria.GetItemString(1, "branch_id_e");
            String coop_name = state.SsCoopName;
           // String start_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "as_tstartdate", null);
           // String end_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "as_tenddate", null);
            String start_date = dw_criteria.GetItemString(1, "as_startdate");
            String end_date = dw_criteria.GetItemString(1, "as_enddate");
            string branch_allr = dw_criteria.GetItemDecimal(1, "branch_all").ToString();
            String as_logotus = dw_criteria.GetItemDecimal(1, "as_logotus").ToString();
            ReportHelper lnv_helper = new ReportHelper();
            if (branch_allr == "5" || branch_allr == "9" || branch_allr == "10" || branch_allr == "11")
            {
                lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(ascstype, ArgumentType.String);

            }
            else if (branch_allr == "12")
            {
                lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(ascstype, ArgumentType.String);
                lnv_helper.AddArgument(as_logotus, ArgumentType.Number);
            }
            else
            {
                lnv_helper.AddArgument(start_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(end_date, ArgumentType.DateTime);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(branch_id_e, ArgumentType.String);
                lnv_helper.AddArgument(ascstype, ArgumentType.String);
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