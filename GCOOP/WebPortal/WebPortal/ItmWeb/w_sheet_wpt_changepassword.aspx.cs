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

namespace WebPortal.ItmWeb
{
    public partial class w_sheet_wpt_changepassword : System.Web.UI.Page
    {
        public string[] args;
        protected void Page_Load(object sender, EventArgs e)
        {
            String member;
            member = Session["username"].ToString();
            DwUser.Retrieve(member);
        }


        protected void save_Click(object sender, EventArgs e)
        {
            WebPortal.Itm.WptService im;
            String memberno;
            String msgs;
            String usn, oldpws, newpwd, confpwd;
            try
            {
                memberno = Session["username"].ToString();
                usn = username.Text;
                oldpws = current.Text;
                newpwd = newpass.Text;
                confpwd = confirm.Text;
                im = new WebPortal.Itm.WptService();
                
                msgs = im.ChangePassword(memberno, usn, oldpws, newpwd, confpwd);
                //altmsg.Text = msgs; 
                MessageBox(msgs);
            }
            catch
            {
            }
        }
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

    }
}
