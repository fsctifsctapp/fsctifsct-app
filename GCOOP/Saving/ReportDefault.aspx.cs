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
using System.Collections.Generic;

namespace Saving
{
    public partial class ReportDefault : System.Web.UI.Page
    {
        private String deprecated_criobj = "";

        private String app = "";
        private String gid = "";
        private WebState state;

        private void Deprecated_Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            try
            {
                gid = Request["gid"].ToString();
            }
            catch { }
            try
            {
                deprecated_criobj = Request["criobj"].ToString();
            }
            catch { deprecated_criobj = ""; }
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if ((gid != null) && (gid != "") && deprecated_criobj == "")
            {
                String output = "";
                List<MenuSubReport> sub = new List<MenuSubReport>();
                sub = new MenuSubReport().GetMenuSubReport(state.SsApplication, gid,state.SsConnectionString);
                for (int i = 0; i < sub.Count; i++)
                {
                    output += "<tr><td class=\"tdpoint\" width=\"15px\"><img style=\" visibility:hidden;\" id=\"p_row" + i 
                            + "\" alt=\"\" src=\"img/arrow.ico\" /></td><td style=\"background-color:Transparent\" id=\"t_row" + i 
                            + "\" onmouseover=\"showPointer(" + i + ");\" onmouseout=\"hindPointer(" + i
                            + ");\" onclick=\"window.location='" + sub[i].PageLink+ "'\">" + sub[i].ReportName + "</td></tr>";
                    
                        //alert('" + sub[i].ReportId  + "');
                }
                ltr_submenu.Text = output;
            }
            try
            {
                if (deprecated_criobj != null && deprecated_criobj != "" && app != null && app != "")
                {
                    ltr_submenu.Text = "";
                    Deprecated_ChangePanel(app,deprecated_criobj.Trim().ToLower());
                    //lbl_cnstmenu.Text = pagename;
                }
            }
            catch(Exception ex) {LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
        private void Deprecated_ChangePanel(String app, String uo)
        {
            uo = "Applications/" + app + "/report/" + uo + ".ascx";
            try
            {
                pnl_cnst.Controls.Clear();
            }
            catch { }
            Control ct = LoadControl(uo);
            pnl_cnst.Controls.Add(ct);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            try
            {
                gid = Request["gid"].ToString();
            }
            catch { }
            if ((gid != null) && (gid != ""))
            {
                String output = "";
                List<MenuSubReport> sub = new List<MenuSubReport>();
                sub = new MenuSubReport().GetMenuSubReport(app, gid, state.SsConnectionString);
                for (int i = 0; i < sub.Count; i++)
                {
                    output += "<tr><td class=\"tdpoint\" width=\"15px\"><img style=\" visibility:hidden;\" id=\"p_row" + i
                            + "\" alt=\"\" src=\"img/arrow.ico\" /></td><td style=\"background-color:Transparent\" id=\"t_row" + i
                            + "\" onmouseover=\"showPointer(" + i + ");\" onmouseout=\"hindPointer(" + i
                            + ");\" onclick=\"window.location='" + sub[i].PageLink + "'\">" + sub[i].ReportName + "</td></tr>";

                    //alert('" + sub[i].ReportId  + "');
                }
                ltr_submenu.Text = output;
            }
        }
    }
}
