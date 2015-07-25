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
using Saving.CustomControl;

namespace Saving
{
    public partial class Frame : System.Web.UI.MasterPage
    {
        protected WebState state;
        private DateTime dt001;

        protected String SavingUrl
        {
            get { return state.Url; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dt001 = DateTime.Now;
            state = new WebState(Session, Request, Application);
            //System.Configuration.ConfigurationManager.AppSettings["wsPass"].ToString();
            MenuBarControl1.LoadBegin(state);
            MenuSubControl1.LoadBegin(state);
            TopBarControl1.LoadBegin(state);
            ApplicationsControl1.LoadBegin(state);
            LoginControl1.LoadBegin(state);
            divMenuGroup.Attributes.Remove("class");
            divMenuGroup.Attributes.Add("class", "menubarGroupClose");
            SetWebSheetBegin();
        }

        protected void Page_LoadComplete()
        {
            LoginControl1.LoadEnd();
            ApplicationsControl1.LoadEnd();
            TopBarControl1.LoadEnd();
            MenuSubControl1.LoadEnd();
            MenuBarControl1.LoadEnd();

            TableMenuAndContent.Visible = !string.IsNullOrEmpty(state.SsApplication) && !string.IsNullOrEmpty(state.SsUsername);
            if (string.IsNullOrEmpty(state.SsApplication) || string.IsNullOrEmpty(state.SsUsername))
            {
                TdReport.InnerHtml = "";
            }

            SetWebSheetEnd();
            String title = "GCOOP - Isocare Systems.";
            if (!string.IsNullOrEmpty(state.SiteTName))
            {
                LbSiteNameThai.Text = state.SiteTName;
            }
            if (!string.IsNullOrEmpty(state.SiteEName))
            {
                LbSiteNameEnglish.Text = state.SiteEName;
            }
            else
            {
                LbSiteNameEnglish.Text = "SAVINGS CO-OPERATIVE LTD.";
            }
            LbSystemAndPage.Text = state.SsThaiApplication;
            if (!string.IsNullOrEmpty(state.CurrentPageName))
            {
                this.Page.Title = state.CurrentPageName + " : " + title;
                LbSystemAndPage.Text = state.SsThaiApplication + " &nbsp; - &nbsp; " + state.CurrentPageName;
            }
            else if (!string.IsNullOrEmpty(state.SsMenuGroupDesc))
            {
                this.Page.Title = state.SsMenuGroupDesc + " : " + title;
            }
            else if (!string.IsNullOrEmpty(state.SsApplication))
            {
                this.Page.Title = state.SsApplication.ToUpper() + " : " + title;
            }
            else
            {
                this.Page.Title = title;
            }
            TopBarControl1.SetLoadTime(dt001);
            LbSiteNameThai.Text = state.SsCsDesc.Trim() == "" ? "โปรแกรมฌาปนกิจสงเคราะห์" : state.SsCsDesc.Trim();
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
            if (!IsPostBack)
            {
                state.LogAct(state.SsUsername, "load", "เปิดหน้าจอ" + state.CurrentPageName, state.SsApplication, state.CurrentPage);
            }
            if (state.SsCsType == "")
            {
                Image1.ImageUrl = "~/img/band_black.jpg";
            }
            else
            {
                Image1.ImageUrl = "~/img/applications/" + state.SsCsType + ".png";
            }
        }

        private void SetWebSheetBegin()
        {
           

            WebSheet wSheet = null;
            PageWebSheet pwSheet = null;
            LtDwThDateJavaScript.Text = "";
            try
            {
                wSheet = (WebSheet)ContentPlace.Page;
                pwSheet = (PageWebSheet)ContentPlace.Page;
                if (pwSheet.WebSheetType != "CommonLibrary.PageWebSheet") return;
                pwSheet.state = this.state;
                pwSheet.LtServerMessage = ((Literal)ContentPlace.FindControl("LtServerMessage"));
                pwSheet.LtServerMessage.Text = "";
                //---------------------------------------------------------------------------
                if (!state.IsReadable)
                {
                    pwSheet.LtServerMessage.Text = WebUtil.PermissionDeny(PermissType.ReadDeny);
                    return;
                }
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
                        state.LogAct(state.SsUsername, "save", "พยายามบันทึก" + state.CurrentPageName, state.SsApplication, state.CurrentPage);
                        pwSheet.LtServerMessage.Text = WebUtil.PermissionDeny(PermissType.WriteDeny);
                        return;
                    }
                    state.LogAct(state.SsUsername, "save", "บันทึก" + state.CurrentPageName, state.SsApplication, state.CurrentPage);
                    try
                    {
                        wSheet.SaveWebSheet();
                    }
                    catch { }
                }
                else
                {
                    //เช็ค Event JsPostBack
                    try
                    {
                        if (IsPostBack)
                        {
                            state.LogAct(state.SsUsername, eventArg, "PostBack on " + state.CurrentPageName, state.SsApplication, state.CurrentPage);
                            wSheet.CheckJsPostBack(eventArg);
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}