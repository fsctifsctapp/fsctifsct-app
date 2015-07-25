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
using SecurityEngine;
using CommonLibrary.WsCommon;

namespace Saving
{
    public partial class Login : System.Web.UI.Page
    {
        private WebState state;

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState();
            try
            {
                String app = Request["app"];
                state.SetApplicationDetail(app);
            }
            catch { }
            state = new WebState(Session, Request);
            LogdedIn();
            //SetDDLBranch();
            //SetDDLPrinter();
            if (state.SsConnectMode == ConnectMode.Manual) { LtConnectMode.Text = "<span style=\"color:red; font-size:smaller;\">ต่อ Database :<br>" + WebUtil.GetConnectionElement("User ID") + "@" + WebUtil.GetConnectionElement("Data Source") + "<br>แบบ Manual</span>"; };
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SetDDLBranch();
            SetDDLPrinter();
        }

        protected void b_login_Click(object sender, EventArgs e)
        {
            try
            {
                String url = "";
                String branch = DlBranchId.SelectedValue;
                String printerSet = DlPrinter.SelectedValue;
                if (txt_password.Text == "")
                {
                    url = state.Login(txt_username.Text, new Encryption().EncryptAscii("1234"), state.SsApplication, branch, printerSet);
                }
                else
                {
                    url = state.Login(txt_username.Text, new Encryption().EncryptAscii(txt_password.Text), state.SsApplication, branch, printerSet);
                }
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                LbServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        protected void LogdedIn()
        {
            try
            {
                if (state.IsLogin)
                {
                    String url = state.Url + "Default.aspx?millisecond=" + DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour;
                    Response.Redirect(url);
                }
            }
            catch { }
        }

        private void SetDDLBranch()
        {
            try
            {
                Common comm = WsUtil.Common;
                DataTable dt = comm.GetBranchId(state.SsWsPass, state.SsCsType);
                DlBranchId.DataSource = dt;
                DlBranchId.DataTextField = "COOPBRANCH_DESC";
                DlBranchId.DataValueField = "COOPBRANCH_ID";
                DlBranchId.DataBind();
            }
            catch (Exception ex) { LbServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void SetDDLPrinter()
        {
            try
            {
                Common comm = WsUtil.Common;
                DataTable dtFormSets = comm.GetPrinterFormSetsData(state.SsWsPass);
                int ii = -1;
                ListItem lt;
                for (int i = 0; i < dtFormSets.Rows.Count; i++)
                {
                    lt = new ListItem(dtFormSets.Rows[i]["formset_desc"].ToString(), dtFormSets.Rows[i]["formset_code"].ToString());
                    DlPrinter.Items.Add(lt);
                    if (dtFormSets.Rows[i]["computer_ip"].ToString() == state.SsClientIp)
                    {
                        ii = ii >= 0 ? ii : i;
                        DlPrinter.SelectedIndex = ii;
                    }
                }
                if (ii < 0)
                {
                    lt = new ListItem("ยังไม่ได้กำหนดเครื่องพิพม์", "0");
                    DlPrinter.Items.Insert(0, lt);
                    DlPrinter.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { LbServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
    }
}
