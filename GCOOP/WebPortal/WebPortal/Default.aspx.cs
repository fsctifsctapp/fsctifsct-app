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

namespace WebPortal
{
    public partial class Default : System.Web.UI.Page
    {
        public String wrongPassword;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["cmd"] == "LogOut")
            {
                //Label2.Text:="clear session";
                Session["username"] = "";
            }
            try
            {
                if (Request["register"].ToString() == "สมัครสมาชิก")
                {
                    WebPortal.Itm.WptService im;
                    String member, cardid, status;
                    member = Request["memberno"].ToString(); ;
                    cardid = Request["cardno"].ToString();


                    im = new WebPortal.Itm.WptService();
                    status = im.CheckMember(member,cardid);
                    if (status == "-1")
                    {
                        MessageBox("หมายเลขสมาชิกหรือหมายเลขบัตรประชาชนไม่ถูกต้อง");
                        //wrongPassword = "*** หมายเลขสมาชิกหรือหมายเลขบัตรประชาชนไม่ถูกต้อง ***";
                        //Response.Write("<script>alert('หมายเลขสมาชิกหรือเลขที่บัตรประชาชนไม่ถูกต้อง')</script>");
                    }
                    else if (status == "0")
                    {
                        MessageBox("หมายเลขสมาชิกนี้ได้สมัครเป็นสมาชิกแล้ว");
                        //wrongPassword = "*** หมายเลขสมาชิกนี้ได้สมัครเป็นสมาชิกแล้ว ***";
                    }
                    else
                    {
                        //MessageBox(status);
                        Response.Redirect("ItmWeb/w_sheet_wpt_register.aspx?id=" + status);
                    }
                }
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


        protected void Page_LoadComplete()
        {
            try
            {
                String member = Session["username"].ToString();
                if (Session["username"].ToString() == "")
                {
                    RegMem.Text = "<table style=width: 100%;>" +
                                "<tr>" +
                                "<td class=style1 colspan='2'><img  src=\"img/bg_bullet_half_2.gif\" />สมัครเข้าใช้งานระบบ</td>" +
                                "</tr>" +
                                 "<tr>" +
                                "<td align=center colspan=2>&nbsp;</td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style7 align=center colspan=2></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style3 align=right>&nbsp; หมายเลขสมาชิก :</td>" +
                                "<td><input type=textbox name=membno  MaxLength=6></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style3 align=right>&nbsp; หมายเลขบัตรประชาชน :</td>" +
                                "<td><input type=textbox name=cardno  MaxLength=13></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8><input type=Submit name=register  value=สมัครสมาชิก Width=86px /></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8>&nbsp;"+
                                "</tr>" +
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8>&nbsp;" +
                                "</tr>" +
                                "</table>";

                }
                else
                {
                    //Label2.Text = Session["username"].ToString();
                    RegMem.Text = "";
                }
            }
            catch
            {
                //Label2.Text = Session["username"].ToString();
                RegMem.Text = "<table style=width: 100%;>" +
                                "<tr>" +
                                "<td class=style1 colspan=2><img src=\"img/bg_bullet_half_2.gif\" />สมัครเข้าใช้งานระบบ</td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td align=center colspan=2>&nbsp;</td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style7 align=center colspan=2></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style3 align=right>&nbsp; หมายเลขสมาชิก :</td>" +
                                "<td><input type=textbox name=memberno runat=server MaxLength=6></asp:TextBox></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style3 align=right>&nbsp; หมายเลขบัตรประชาชน :</td>" +
                                "<td><input type=textbox name=cardno  MaxLength=13></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8><input type=Submit name=register  value=สมัครสมาชิก  Width=86px /></td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8>&nbsp;"+
                                "</tr>"+
                                "<tr>" +
                                "<td class=style8>&nbsp;</td>" +
                                "<td class=style8>&nbsp;" +
                                "</tr>" +
                                "</table>";
            }


        }

        

    }
}
