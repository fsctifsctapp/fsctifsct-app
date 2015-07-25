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
using SecurityEngine;
using CommonLibrary.WsCommon;

namespace Saving.CustomControl
{
    public partial class TopBarControl : System.Web.UI.UserControl
    {
        private WebState state;

        public void LoadBegin(WebState state)
        {
            this.state = state;
            GetListDb();
            CbDbProfile.Checked = false;
            if (String.IsNullOrEmpty(state.SsWsPass))
            {
                ChangedWsPass(0);
            }
        }

        public void LoadEnd()
        {
            LbApplication.Text = string.IsNullOrEmpty(state.SsApplication) ? "" : "| &nbsp; " + state.SsApplication.ToUpper() + " &nbsp; ";
            LbWorkDate.Text = state.SsWorkDate == null ? "" : "| &nbsp; DATE: " + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.TH) + " &nbsp; ";
            LbUsername.Text = string.IsNullOrEmpty(state.SsUsername) ? "" : "| &nbsp; USER: " + state.SsUsername.ToUpper() + " &nbsp; ";
            LbBranch.Text = string.IsNullOrEmpty(state.SsBranchId) ? "" : "| &nbsp; BRANCH: " + state.SsBranchId.Trim() + " &nbsp; ";
            LbIpAddress.Text = string.IsNullOrEmpty(state.SsClientIp) ? "" : "| &nbsp; IP: " + state.SsClientIp.ToUpper() + " &nbsp; ";
            LbDbProfile.Text = state.SsDbProfile;
        }

        private void GetListDb()
        {
            if (IsPostBack) return;

            int conDbList = ConfigurationManager.ConnectionStrings.Count;
            ListItem[] list = new ListItem[conDbList];
            list[0] = new ListItem();
            list[0].Text = "Auto";
            list[0].Value = "Auto";
            DdDbProfile.Items.Add(list[0]);
            for (int i = 1; i < list.Length; i++)
            {
                list[i] = new ListItem();
                list[i].Text = ConfigurationManager.ConnectionStrings[i].Name;
                list[i].Value = i.ToString();
                DdDbProfile.Items.Add(list[i]);
            }
            if (state.SsConnectMode == ConnectMode.Manual)
            {
                for (int i = 1; i < list.Length; i++)
                {
                    if (state.SsConnectionString == ConfigurationManager.ConnectionStrings[i].ConnectionString)
                    {
                        DdDbProfile.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void ChangedWsPass(int i)
        {
            try
            {
                Common wscom = WsUtil.Common;
                String wsPass = new Decryption().DecryptAscii(wscom.GetPasswordService()) + "+ ";
                if (i <= 0)
                {
                    state.SsConnectMode = ConnectMode.Auto;
                    String wsPassEnCrypt = new Encryption().EncryptStrBase64(wsPass);
                    state.SsConnectionString = new Decryption().DecryptStrBase64(wscom.GetConnectionString(wsPassEnCrypt));
                    //String strDBConnect = WebUtil.GetConnectionElement("User ID") + "@" + WebUtil.GetConnectionElement("Data Source");
                }
                else
                {
                    state.SsConnectMode = ConnectMode.Manual;
                    state.SsConnectionString = ConfigurationManager.ConnectionStrings[i].ConnectionString;
                }
                state.SsApplication = "";
                state.SsUsername = "";
                state.SsWsPass = new Encryption().EncryptStrBase64(wsPass.Trim() + state.SsConnectionString);
            }
            catch (Exception ex)
            {
                state.SsConnectMode = ConnectMode.Auto;
                state.SsConnectionString = "";
                state.SsWsPass = "";
                state.SsApplication = "";
                state.SsUsername = "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CssTopBar.Visible = false;
        }

        protected void DdDbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangedWsPass(DdDbProfile.SelectedIndex);
        }

        public void SetLoadTime(DateTime startTime)
        {
            TimeSpan timeEnd = DateTime.Now - startTime;
            LbLoadTime.Text = "| &nbsp; Load: " + timeEnd.TotalSeconds.ToString("0.00");
        }
    }
}