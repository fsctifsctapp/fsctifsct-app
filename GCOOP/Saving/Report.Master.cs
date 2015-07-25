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
using System.Collections.Generic;
using System.Globalization;

namespace Saving
{
    public partial class Report : System.Web.UI.MasterPage
    {
        #region MasterPage

        private String titlePage;
        public String TitlePage
        {
            get { return titlePage; }
        }
        private String jsIncl;
        public String JsIncl
        {
            get { return jsIncl; }
        }
        protected WebState state;
        private String application;
        private String username;

        protected String siteLogo;
        protected String siteTName;
        protected String siteEName;
        protected String siteLinkName;
        protected String siteArgument;

        private String currentPage = null;
        private String app;
        private String gid;

        protected void Deprecated_Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            titlePage = state.AppTitle;
            if (state.SsApplication == null)
            {
                Response.Redirect(state.Url + "Flash/index.aspx");
            }
            else if (!state.IsLogin && state.CurrentPage.ToLower() != "login.aspx")
            {
                Response.Redirect(state.Url + "Flash/index.aspx");
            }
            //--- ตั้งค่า Javascript
            try
            {
                HApplication.Value = state.SsApplication;
                HUrl.Value = state.Url;
                HCurrentPage.Value = state.CurrentPage;
            }
            catch { }
            jsIncl = "<script type=\"text/javascript\" src=\"" + state.Url + "js/js.js\"></script>\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/WebState.js\"></script>\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/Gcoop.js\"></script>\n";

            try
            {
                gid = Request["gid"].ToString();
            }
            catch { }

            if (!state.IsLogin)
            {

            }
            else
            {
                setCurrentMenu(state.SsApplication, gid, state.SsConnectionString);
                setPageLabel();
                setReportMenu(state.SsApplication, state.SsConnectionString);
            }
            // ตั้งค่า sitename sitelogo //
            ImgSiteLogo.ImageUrl = state.SiteLogo;
            siteTName = state.SiteTName;
            siteEName = state.SiteEName;
            siteLinkName = state.SiteLinkName;

            //SETWEBSHEET w_sheet..........
            //SetWebSheetBegin();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            state = new WebState(Session, Request, Application);
            titlePage = state.AppTitle;
            if (state.SsApplication == null)
            {
                Response.Redirect(state.Url + "Flash/index.aspx");
            }
            else if (!state.IsLogin && state.CurrentPage.ToLower() != "login.aspx")
            {
                Response.Redirect(state.Url + "Flash/index.aspx");
            }

            //--- Page Arguments
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

            //--- ตั้งค่า Javascript
            try
            {
                HApplication.Value = app;
                HUrl.Value = state.Url;
                HCurrentPage.Value = state.CurrentPage;
            }
            catch { }
            jsIncl = "\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/DetectBrowser.js\"></script>\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/js.js\"></script>\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/WebState.js\"></script>\n";
            jsIncl += "<script type=\"text/javascript\" src=\"" + state.Url + "js/Gcoop.js\"></script>\n";

            //--- Create Report Group Menus
            if (state.IsLogin)
            {
                setCurrentMenu(app, gid, state.SsConnectionString);
                setPageLabel();
                //setReportMenu(state.SsApplication, state.SsConnectionString);
            }

            // ตั้งค่า sitename sitelogo //
            ImgSiteLogo.ImageUrl = state.SiteLogo;
            siteTName = state.SiteTName;
            siteEName = state.SiteEName;
            siteLinkName = state.SiteLinkName;

            //SETWEBSHEET w_sheet..........
            SetWebSheetBegin();

        }

        protected void Page_LoadComplete()
        {
            if (state.SsCsType == "")
            {
                ImgSiteLogo.ImageUrl = "~/img/band_black.jpg";
            }
            else
            {
                ImgSiteLogo.ImageUrl = "~/img/applications/" + state.SsCsType + ".png";
            }

            if (!string.IsNullOrEmpty(state.SiteTName))
            {
                siteTName = state.SiteTName;
            }
            if (!string.IsNullOrEmpty(state.SiteEName))
            {
                siteEName = state.SiteEName;
            }
            else
            {
                siteEName = "SAVINGS CO-OPERATIVE LTD.";
            }
            SetWebSheetEnd();
            siteTName = state.SsCsDesc.Trim() == "" ? "โปรแกรมฌาปนกิจสงเคราะห์" : state.SsCsDesc.Trim();
        }

        #endregion

        private void setPageLabel()
        {
            username = state.SsUsername;
            application = state.SsApplication;
            if (state.CurrentPageName.Equals(""))
            {
                ltr_headmainpage.Text = state.SsThaiApplication;
            }
            else
            {
                ltr_headmainpage.Text = state.SsThaiApplication + "  -  <span style=\" font-size:14px;\">" + state.CurrentPageName + "</span>";
            }
            LbAppName.Text = "รายงาน " + state.SsThaiApplication;
            LbWorkDateLoginBy.Text = "วันทำการ : " + state.SsWorkDate.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) + " [ " + username + " ]";
        }

        private void setReportMenu(String app, String connStr)
        {
            new MenuReport().GetMenuReport(app, connStr);
        }

        private void setCurrentMenu(String app, String curr, String connStr)
        {
            List<MenuReport> menu = new MenuReport().GetMenuReport(app, connStr);
            for (int i = 0; i < menu.Count; i++)
            {
                if (menu[i].GroupID.Equals(curr))
                {
                    currentPage = menu[i].GroupName;
                    ltr_headmainpage.Text += currentPage;
                }
            }
        }

        private void SetWebSheetBegin()
        {
            WebSheet wSheet = null;
            PageWebSheet pwSheet = null;
            siteArgument = "";              //Frame.Master:=>LtDwThDateJavaScript.Text = "";
            try
            {
                wSheet = (WebSheet)ContentPlace.Page;
                pwSheet = (PageWebSheet)ContentPlace.Page;
                if (pwSheet.WebSheetType != "CommonLibrary.PageWebSheet") return;
                pwSheet.state = this.state;
                pwSheet.LtServerMessage = ((Literal)ContentPlace.FindControl("LtServerMessage"));
                pwSheet.LtServerMessage.Text = "";

                //---------------------------------------------------------------------------

                try
                {
                    wSheet.InitJsPostBack();
                }
                catch { }
                try
                {
                    wSheet.WebSheetLoadBegin();
                }
                catch { }
                String eventArg = "";
                try { eventArg = Request["__EVENTARGUMENT"]; }
                catch { }
                if (eventArg == "SaveWebSheet")
                {
                    //เตรียมเช็คสิทธิ์เซฟบรรทัดนี้อีกรอบ --------------------------------------------------------
                    if (!state.IsWritable)
                    {
                        pwSheet.LtServerMessage.Text = WebUtil.PermissionDeny(PermissType.WriteDeny);
                        return;
                    }
                    try
                    {
                        wSheet.SaveWebSheet();
                    }
                    catch { }
                }
                else
                {
                    //เช็ค Event JsPostBack
                    try { if (IsPostBack) wSheet.CheckJsPostBack(eventArg); }
                    catch { }
                }
            }
            catch { }
        }

        private void SetWebSheetEnd()
        {
            WebSheet wSheet = null;
            PageWebSheet pwSheet = null;
            try
            {
                wSheet = (WebSheet)ContentPlace.Page;
                pwSheet = (PageWebSheet)ContentPlace.Page;
                if (pwSheet.WebSheetType != "CommonLibrary.PageWebSheet") return;
                try
                {
                    wSheet.WebSheetLoadEnd();
                }
                catch { }
            }
            catch { }
            try
            {
                pwSheet.DisConnectSQLCA();
            }
            catch { }
            try
            {
                pwSheet.SetDwThDateJavaScriptEvent(LtDwThDateJavaScript);
            }
            catch { }
        }
    }
}
