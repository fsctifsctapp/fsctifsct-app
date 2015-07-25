using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Flash
{
    public partial class Home1 : System.Web.UI.Page
    {
        WebState state;
        String[] appMainlist = null;
        String[] appExpandlist = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //encrypt("}I#%ap:");
            state = new WebState(Session, Request);
            try
            {
                if (Request["cmd"].ToString() == "logout")
                {
                    state.Logout();
                }
            }
            catch { }



            //appMainlist = new String[5];
            //appMainlist[0] = "shrlon";
            //appMainlist[1] = "ap_deposit";
            //appMainlist[2] = "app_finance";
            //appMainlist[3] = "account";
            //appMainlist[4] = "keeping";

            //appExpandlist = new String[4];
            //appExpandlist[0] = "app_assist";
            //appExpandlist[1] = "hr";
            //appExpandlist[2] = "cmd";
            //appExpandlist[3] = "mis";

            //String leftoutput = "";
            //String rightoutput = "";
            //int LiconHspace = 145;
            //int RiconHspace = 145;
            //for (int i = 0; i < appMainlist.Length; i++)
            //{
            //    state.SetApplicationDetail(appMainlist.GetValue(i).ToString());
            //    state = new WebState(Session, Request);
            //    String clsday = state.SscloseDayStatus;
            //    String outputclsday = "";
            //    if (clsday == "1") { outputclsday = "ปิดงานสิ้นวันเรียบร้อยแล้ว"; }
            //    else if (clsday == "0") { outputclsday = "ยังไม่ได้ปิดงานสิ้นวัน"; }

            //    leftoutput = leftoutput + "<div class=\"leftdescription\" style=\"margin-top: " + LiconHspace + "px\"><span id=\"ltext1_" + (i + 1) + "\" style=\"visibility: visible;\" onclick=\"pageopen('main" + (i + 1) + "')\"><span class=\"style1\">ระบบ"
            //                        + state.GetApplicationThai(appMainlist.GetValue(i).ToString()) + "</span><br /><br />" + state.SsworkDate + "<br />สถานะปิดงานสิ้นวัน :<br />" + outputclsday + "</span></div>";
            //    LiconHspace += 110;
            //}
            ////ltr_leftside.Text = leftoutput;

            //for (int i = 0; i < appExpandlist.Length; i++)
            //{
            //    state.SetApplicationDetail(appExpandlist.GetValue(i).ToString());
            //    state = new WebState(Session, Request);
            //    String clsday = state.SscloseDayStatus;
            //    String outputclsday = "";
            //    if (clsday == "1") { outputclsday = "ปิดงานสิ้นวันเรียบร้อยแล้ว"; }
            //    else if (clsday == "0") { outputclsday = "ยังไม่ได้ปิดงานสิ้นวัน"; }
            //    rightoutput = rightoutput + "<div class=\"rithtdescription\" style=\"margin-top: " + RiconHspace + "px\"><span id=\"rtext1_" + (i + 1) + "\" style=\"visibility: visible;\" onclick=\"pageopen('expand" + (i + 1) + "')\"><span class=\"style1\">ระบบ"
            //                    + state.GetApplicationThai(appMainlist.GetValue(i).ToString()) + "</span><br /><br />" + state.SsworkDate + "<br />สถานะปิดงานสิ้นวัน :<br />" + outputclsday + "</span>";
            //    RiconHspace += 110;
            //}
            //ltr_rightside.Text = rightoutput;

        }


        WebUtil ut = new WebUtil();
        private String str_encrypted = "", keynum = "98765432123456789", keychar = "chakrit_op";
        private int sourceLen;


        private String encrypt(String strsource)
        {

            sourceLen = strsource.Length;

            int asciiVal, asciiKeynum, asciiKeychar;
            int intKey = 0, intChar = 0;
            String tempVal, tempKey, tempChar;

            for (int i = 0; i < sourceLen; i++)
            {

                //แปลง Source String เป็น รหัส Ascii ทีละ 1 ตัว
                tempVal = strsource.Substring(i, i + 1);
                asciiVal = StrToAsciiCode(tempVal);

                //แปลง Keynum String เป็น รหัส Ascii ทีละ 1 ตัว
                tempKey = keynum.Substring(intKey, intKey + 1);
                asciiKeynum = StrToAsciiCode(tempKey);

                //แปลง  Keychar String เป็น รหัส Ascii ทีละ 1 ตัว
                tempChar = keychar.Substring(intChar, intChar + 1);
                asciiKeychar = StrToAsciiCode(tempChar);



                asciiVal += asciiKeynum;
                asciiVal -= asciiKeychar;
                // Added this section to ensure that ASCII Values stay within 0 to 255 range
                do
                {
                    if (asciiVal > 255)
                    {
                        asciiVal = asciiVal - 255;
                    }
                } while (asciiVal > 255);


                str_encrypted += asciiVal.ToString("000");

                intKey++;
                intChar++;

                if (i > keynum.Length) { intChar = 0; };
                if (i > keychar.Length) { intChar = 0; };


            }
            return str_encrypted;
        }


        private int StrToAsciiCode(String strkey)
        {
            System.Text.Encoding ascii = System.Text.Encoding.ASCII;
            Byte[] encodedBytes = ascii.GetBytes(strkey);
            return Convert.ToInt32(encodedBytes.GetValue(0));
        }
    }
}
