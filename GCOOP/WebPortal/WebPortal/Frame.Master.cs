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

namespace WebPortal
{
    public partial class Frame : System.Web.UI.MasterPage
    {
        public String loginPanel;
        public String wrongPassword;

        protected void Page_Load(object sender, EventArgs e)
        {
            WebPortal.Itm.WptService im;
            String[] args;
            String getlogin;
            String page;
            String member_no;
            String username;


            wrongPassword = "";
            try
            {
                page = Request["page"].ToString();
            }
            catch
            {
                page = "";
            }
            switch (page)
            {
                case "Share": Label1.Text = "ข้อมูลหุ้น";
                    break;
                case "Deposit": Label1.Text = "ข้อมูลเงินฝาก";
                    break;
                case "Loan": Label1.Text = "ข้อมูลเงินกู้";
                    break;
                case "Loanpermission": Label1.Text = "สิทธิกู้";
                    break;
                case "Collateral": Label1.Text = "ค้ำประกัน";
                    break;
                case "Keeping": Label1.Text = "เรียกเก็บ";
                    break;
                case "Changepassword": Label1.Text = "เปลี่ยนรหัสผ่าน";
                    break;
                case "DivAvg": Label1.Text = "เงินปันผล-เฉลี่ยคืน";
                    break;
                case "Approve_Loan": Label1.Text = "การอนุมัติเงินกู้";
                    break;
                //case "Receipt": Label1.Text = "ใบเสร็จออนไลน์";
                //    break;
                //case "Calloan": Label1.Text = "ตารางคำนวณการผ่อนชำระ";
                //    break;
                //case "Sharebal": Label1.Text = "หนังสือยืนยันยอด";
                //    break;
                //case "Collateral_Check": Label1.Text = "สิทธิการค้ำประกัน";
                //    break;
                default: Label1.Text = "ข้อมูลสมาชิก";
                    break;
            }
            try
            {
                if (Request["LogSubmit"].ToString() == "Login")
                {
                    args = new String[3];
                    args[0] = "1234";
                    args[1] = Request["LogUsername"].ToString();
                    args[2] = Request["LogPassword"].ToString();
                    im = new WebPortal.Itm.WptService();
                    getlogin = im.GetLogin(args);
                    member_no = getlogin.Substring(0,6);
                    username = getlogin.Substring(6);
                    if (username == args[1])
                    {
                        Session["username"] = member_no;
                    }
                    else
                    {
                        wrongPassword = getlogin;
                    }
                }
                else
                {
                    //Label1.Text = err.ToString();
                    
                }


            }
            catch
            {
            }

        }

        protected void Page_LoadComplete()
        {
            try
            {
                if(Session["username"].ToString()=="")
                {
                    LiteralLogIn.Text =  "<div class=loginform>"+
                                    "<fieldset>"+
                                    "<p>"+
                                    "<label class=top for=username_1>"+
                                    "&nbsp;Username:&nbsp;<input type=textbox name=LogUsername size=18 MaxLength=8/>"+
                                    "</label>"+
                                    "<br />"+
                                    "</p>"+
                                    "<p>"+
                                    "<label class=top for=password_1>"+
                                    "&nbsp;Password:&nbsp;<input name=LogPassword type=password size=18 MaxLength=8/>"+
                                    "</label>"+
                                    "<br />"+
                                    "</p>"+
                                    "<p>"+
                                    "&nbsp;<input type=Submit name=LogSubmit Value=Login />"+
                                    "</p>"+
                                    "<p>"+
                                    wrongPassword +
                                    "<a id=A1 href=# onclick=PopUpWindow();return false;>ลืมรหัสผ่าน?</a>" +
                                    "</p><br>"+
                                    "</fieldset>"+
                                    "</div>";

                }else
                {
                    LiteralLogIn.Text = "";
                    pnl_logout.Visible = true;

                }
            }catch
            {
                    LiteralLogIn.Text = "<div class=loginform>"+
                                    "<fieldset>"+
                                    "<p>"+
                                    "<label class=top for=username_1>"+
                                    "&nbsp;Username:&nbsp;<input type=textbox name=LogUsername size=18 MaxLength=8/>" +
                                    "</label>"+
                                    "<br />"+
                                    "</p>"+
                                    "<p>"+
                                    "<label class=top for=password_1>"+
                                    "&nbsp;Password:&nbsp;<input name=LogPassword type=password size=18 MaxLength=8/>" +
                                    "</label>"+
                                    "<br />"+
                                    "</p>"+
                                    "<p>"+
                                    "&nbsp;<input type=Submit name=LogSubmit Value=Login />"+
                                    "</p>"+
                                    "<p>"+
                                    wrongPassword +
                                    "<a id=A1 href=# onclick=PopUpWindow();return false;>ลืมรหัสผ่าน?</a>" +
                                    "</p><br>"+
                                    "</fieldset>"+
                                    "</div>";
            }
        }




    }
}
