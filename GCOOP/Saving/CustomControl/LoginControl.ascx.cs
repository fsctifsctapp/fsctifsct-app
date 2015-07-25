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
using SecurityEngine;
using CommonLibrary;
using CommonLibrary.WsCommon;

namespace Saving.CustomControl
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        private WebState state;
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                this.visible = value;
                DivLogIn.Visible = value;
                TableLogIn.Visible = value;
            }
        }

        protected String focusControl;

        public void LoadBegin(WebState state)
        {
            this.state = state;
            focusControl = "";
            if (String.IsNullOrEmpty(state.SsUsername) && !String.IsNullOrEmpty(state.SsApplication))
            {
                if (!IsPostBack)
                {
                    Common comm = WsUtil.Common;
                    DataTable dt = comm.GetBranchId(state.SsWsPass, state.SsCsType);
                    if (dt.Rows.Count >= 0)
                    {
                        DdBranchId.DataSource = dt;
                        DdBranchId.DataTextField = "coopbranch_iddesc";
                        DdBranchId.DataValueField = "COOPBRANCH_ID";
                        DdBranchId.DataBind();
                    }
                }
            }
        }

        public bool LogIn(String username, String password, String branchId)
        {
            try
            {
                String branch = branchId;
                String isFinish = "";
                username = string.IsNullOrEmpty(username) ? "" : username;
                password = string.IsNullOrEmpty(password) ? "" : password;
                isFinish = state.Login(username, new Encryption().EncryptAscii(password), state.SsApplication, branch);
                try
                {
                    if (isFinish == "true")
                    {
                        HttpCookie ck = new HttpCookie("UserAccount");
                        ck["cs_type"] = state.SsCsType;
                        ck["coopbranch_id"] = branchId;
                        ck["username"] = username;
                        ck.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(ck);
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                LtLoginMessage.Text = WebUtil.ErrorMessage("เข้าสู่ระบบไม่สำเร็จ กรุณาตรวจสอบ ศูนย์ประสานงาน, ชื่อผู้ใช้, รหัสผ่าน");
            }
            return true;
        }

        public void LoadEnd()
        {
            this.Visible = String.IsNullOrEmpty(state.SsUsername) && !String.IsNullOrEmpty(state.SsApplication);
            if (Visible)
            {
                try
                {
                    if (!IsPostBack)
                    {
                        Common comm = WsUtil.Common;
                        DataTable dt = comm.GetBranchId(state.SsWsPass, state.SsCsType);
                        if (dt.Rows.Count >= 0)
                        {
                            DdBranchId.DataSource = dt;
                            DdBranchId.DataTextField = "coopbranch_iddesc";
                            DdBranchId.DataValueField = "COOPBRANCH_ID";
                            DdBranchId.DataBind();
                            DdBranchId.SelectedIndex = 0;
                        }
                        focusControl = TbUsername.ClientID;
                    }
                    else
                    {
                        focusControl = TbPassword.ClientID;
                    }
                    try
                    {
                        HttpCookie newCookie = Request.Cookies["UserAccount"];
                        String csType1 = state.SsCsType;
                        if (newCookie != null)
                        {
                            String csType = "";
                            try
                            {
                                csType = newCookie["cs_type"].ToString();
                            }
                            catch { }
                            if (csType == csType1)
                            {
                                String branchId = newCookie["coopbranch_id"];
                                String username = newCookie["username"];
                                DdBranchId.SelectedValue = branchId;
                                TbUsername.Text = username;
                                newCookie.Expires = DateTime.Now.AddMonths(1);
                                Response.Cookies.Add(newCookie);
                                focusControl = TbPassword.ClientID;
                            }
                        }
                    }
                    catch { }
                    focusControl = "\n<script>document.getElementById('" + focusControl + "').focus();</script>\n";
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bool isLogIn = this.LogIn(TbUsername.Text.Trim(), TbPassword.Text.Trim(), DdBranchId.SelectedValue);
            }
            catch (Exception ex)
            {
                LtLoginMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        protected void BtHome_Click(object sender, EventArgs e)
        {
            state.SsApplication = "";
            state.SsCsType = "";
            state.SsCsDesc = "";
            String url = state.SsUrl;
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect(url);
        }
    }
}