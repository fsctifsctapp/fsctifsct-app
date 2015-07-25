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
    public partial class w_sheet_wc_cri_total_age : PageWebSheet, WebSheet
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String PostPost;
        protected String jsbranch_id;
        protected String jsbranch_desc;

        public void InitJsPostBack()
        {
            PostPost = WebUtil.JsPostBack(this, "PostPost");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
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
            Decimal status = 0;
            status = dw_criteria.GetItemDecimal(1, "status");
            if (status == 1)
            {
                gid = "walfare_daily";
                rid = "walfare_daily99_22";
            }
            else if (status == 2)
            {
            gid = "walfare_daily";
            rid = "walfare_daily99_21";
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
            else if (eventArg == "jsbranch_id")
            {
                ChangeBranchId();
            }
            else if (eventArg == "jsbranch_desc")
            {
                ChangeBranchDesc();
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            string not_cstype, cs_type;

            not_cstype = "0";
            cs_type = state.SsCsType;
            try
            {
                dw_criteria.SaveDataCache();
                String cstype = dw_criteria.GetItemString(1, "as_cstype");
                DataWindowChild dc = dw_criteria.GetChild("branch_id");
                dc.SetFilter("cs_type='" + cstype + "'");
                dc.Filter();                
            }
            catch { }
            DwUtil.RetrieveDDDW(dw_criteria, "as_cstype", "criteria.pbl", null);
            DwUtil.RetrieveDDDW(dw_criteria, "branch_idd", "criteria.pbl", cs_type, not_cstype);
            dw_criteria.SetItemString(1, "as_cstype", state.SsCsType);
            dw_criteria.Modify("as_cstype.Protect=1");
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

            String ascstype = dw_criteria.GetItemString(1, "as_cstype");



            //String egroup_code = dw_criteria.GetItemString(1, "egroup_code");

            String coop_name = state.SsCoopName;
            String branch_id = state.SsBranchId;
            Decimal status = 0;
            status = dw_criteria.GetItemDecimal(1, "status");
            ReportHelper lnv_helper = new ReportHelper();

            if (status == 1)
            {
                lnv_helper.AddArgument(ascstype, ArgumentType.String);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
            }
            else
            {
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