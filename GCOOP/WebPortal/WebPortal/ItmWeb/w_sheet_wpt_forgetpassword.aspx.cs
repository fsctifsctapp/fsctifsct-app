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
using System.Net.Mail;

namespace WebPortal.ItmWeb
{
    public partial class w_sheet_wpt_forgetpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  
            
        }

        protected void send_Click(object sender, EventArgs e)
        {
            try
            {
                WebPortal.Itm.WptService im;
                String member, email_addr;
                String username, password;
                String getstr;
                string[] str;
                im = new WebPortal.Itm.WptService();
                getstr = im.GetUserPassword(memberno.Text, email.Text);
                if (getstr == "email ไม่ถูกต้อง")
                {
                    Label1.Text = getstr;
                }
                str = getstr.Split(':');
                member = str[0];
                email_addr = str[1];
                username = str[2];
                password = str[3];
                Label1.Text = member + email_addr + username + password;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("coopertives@gmail.com", "เจ้าหน้าที่สหกรณ์ฯ");
                mail.To.Add(new MailAddress(email_addr, member));
                mail.Subject = "สอ.กสท.แจ้ง  username password";
                string body;
                body = "สหกรณ์ออมทรัพย์การสื่อสารแห่งประเทศไทย จำกัด แจ้ง username และ password หมายเลขสมาชิก" +
                        member + "\n" + "\n" + "Username =  " + username + "\n" + "Password =  " + password;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                //smtp.Host = "smtp.live.com";

                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("coopertives@gmail.com", "admincoop");
                //smtp.Credentials = new System.Net.NetworkCredential("badbaz_maji@hotmail.com", "yentafo");
                try
                {
                    smtp.Send(mail);
                    Label1.Text = "ส่งข้อมูลผ่านทางอีเมล์ของท่านเรียบร้อยแล้ว";
                }
                catch 
                {
                    Label1.Text = "ส่งข้อมูลผ่านทางอีเมล์ของท่านไม่สำเร็จ";
                }
            }
            catch 
            {
                Label1.Text = "ส่งข้อมูลผ่านทางอีเมล์ของท่านไม่สำเร็จ";
            }

            
        }


    }
}
