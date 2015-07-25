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
using adminservice;

namespace Saving
{
    public partial class ChangePass : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            //รหัสผ่านที่ป้อนจากหน้าจอ.
            String userID, oldPass, newPass, confirmPass;
            WebState state = new WebState(Session,Request);
            userID = state.SsUsername;
            oldPass = TextBox1.Text.Trim();
            newPass = TextBox2.Text.Trim();
            confirmPass = TextBox3.Text.Trim();
            
            //service
            n_cst_adminservice svAdmin;
            n_cst_dbconnectservice svCon;
            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(state.SsConnectionString);
            svAdmin = new n_cst_adminservice();
            svAdmin.of_settrans(ref svCon);

            //ตรวจสอบความถูกต้อง
            int rv = svAdmin.of_verifyuserpassword(userID, oldPass, newPass, confirmPass);
            //คืนค่า 1 = รหัสผ่านถูกต้อง(อนุญาติให้ใช้รหัสผ่านนี้ได้)
            //คืนค่า 0 = สมาชิกยังไม่ได้ตั้งรหัสผ่านไว้
            //คืนค่า -1 = รหัสผ่านไม่ถูกต้อง
            if (rv == -1)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("รหัสผ่านเดิมไม่ถูกต้อง!");
                TextBox1.Text = "";
                TextBox1.Focus();
                svCon.of_disconnectdb();
                return;
            }else if(rv == -2){
                LtServerMessage.Text = WebUtil.ErrorMessage("ยืนยันรหัสผ่านไม่ตรงกัน!");
                TextBox3.Text = "";
                TextBox3.Focus();
                svCon.of_disconnectdb();
                return;
            }

            //บันทึก
            String errtext = "";
            rv = svAdmin.of_savepassword(userID, newPass, ref errtext);
            if (rv < 0)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกรหัสผ่านไม่สำเร็จ: "+errtext);
                svCon.of_disconnectdb();
                return;
            }

            //แจ้งผู้ใช้ว่าบันทึกเรียบร้อยแล้ว
            svCon.of_disconnectdb();
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
            TextBox3.Enabled = false;
            Button1.Enabled = false;
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกรหัสผ่านเรียบร้อยแล้ว กรุณาออกจากระบบ");
            
        }
    }
}
