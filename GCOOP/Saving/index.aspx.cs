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
using DBAccess;
using System.Globalization;
using CommonLibrary;
using SecurityEngine;
using CommonLibrary.WsCommon;

namespace Saving
{
    public partial class Index : System.Web.UI.Page
    {
        private WebState state;
        protected String genJavaScript = "";
        protected String genExcJavaScript = "";
        protected String genJavaScriptInnerHTML = "";

        private String connString = "";
        private String connItem = "";
        private String wsPass = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState();
            try
            {
                if (Request["cmd"].ToString() == "logout")
                {
                    HfAppClicked.Value = "";
                    HfAppEng.Value = "";
                    state.Logout();
                }
                else if (Request["cmd"].ToString() == "switch")
                {
                    Session["ss_menugroup"] = null;
                    HfAppClicked.Value = "";
                    HfAppEng.Value = "";
                    state.SetApplicationDetail(state.SsApplication);
                }
            }
            catch { }
            //Label1.Text = "DB: " + strDBConnect + " [Manual Connect]";
            GenListDB();
            SetConnString();
            GetStatusApplication();
        }

        private void GetStatusApplication()
        {
            PnlSelectDBError.Visible = false;
            PnlSelectDB.Visible = true;
            LtServerMessage.Text = "";
            String locationUrl = "../Saving/Login.aspx?app=";
            String setMainOutput = "";
            String setExpandOutput = "";
            String setLeftOutput = "";
            String setRightOutput = "";
            genJavaScript = "<script type=\"text/javascript\">function pageopen(sys){";
            genJavaScriptInnerHTML = "<script type=\"text/javascript\">" +
                                     "function on_click(appno){" +
                                     "var detail = document.getElementById(\"divShowDetail\");" +
                                     "var appOpen = document.getElementById(\"HfAppClicked\");" +
                                     "var hf_appEng = document.getElementById(\"HfAppEng\");";

            genJavaScriptInnerHTML += "try{if(appOpen.value==appno){" +
                                      "window.location = \"" + locationUrl + "\"+hf_appEng.value;}" +
                                      "}catch(Err){alert(\"error!!\")}";

            genExcJavaScript = "";

            Common wscom = WsUtil.Common;

            String appList = "";


            try
            {
                appList = wscom.GetStatusApplication(wsPass);
                //จำนวน item ต่อ column
                int rowpercol = 6;
                int m = 1;
                int x = 1;
                int appno = 1;
                for (int i = 0; i < appList.Split('|').Length - 1; i++)
                {
                    //set ของ Application
                    String tempAppList = appList.Split('|').GetValue(i).ToString();

                    //Status ของ Application นั้นๆ
                    String index = tempAppList.Split(',').GetValue(0).ToString();
                    String appEng = tempAppList.Split(',').GetValue(1).ToString();
                    String appThai = tempAppList.Split(',').GetValue(2).ToString();
                    String closeDay = tempAppList.Split(',').GetValue(3).ToString();
                    String workDate = tempAppList.Split(',').GetValue(4).ToString();

                    String clsdayStatus = "";
                    String today = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
                    if (workDate == today)
                    {
                        if (closeDay == "0")
                        {
                            clsdayStatus = "<span style='color: Green;'>ปิดงานสิ้นวันแล้ว</span>";
                        }
                        else if (closeDay == "1")
                        {
                            clsdayStatus = "<span style='color: Green;'>ปิดงานสิ้นวันแล้ว<span>";
                        }
                    }
                    else if (workDate != today)
                    {
                        if (closeDay == "0")
                        {
                            clsdayStatus = "<span style='color: Red;'>วันทำการไม่ตรงกับวันปัจจุบัน</span>";
                        }
                        else if (closeDay == "1")
                        {
                            clsdayStatus = "<span style='color: Red;'>ยังไม่ได้ปิดงานสิ้นวัน</span>";
                        }
                    }

                    
                    String defaultButton = "img/home/button1.gif";
                    String mouseOver = "img/home/button1_hover.gif";
                    String mouseOut = "img/home/button1.gif";



                    if (i < rowpercol)
                    {
                        setLeftOutput += "<tr><td id='tdMenu" + i + "' onclick=\"on_click('" + i + "');\" " +
                                         "onmouseover=\"document.getElementById('tdMenu" + i + "').style.background = 'url(" + mouseOver + ") no-repeat';\" " +
                                         "onmouseout=\"document.getElementById('tdMenu" + i + "').style.background = 'url(" + mouseOut + ") no-repeat';\"  " +
                                         "align='center' style='background: url(" + defaultButton + ") " +
                                         "no-repeat;size:auto; padding:15px 1px 15px 1px; cursor:pointer;'>" +
                                         //"&nbsp;&nbsp;<span onclick=\"on_click('" + i + "');\" " +
                                         "&nbsp;&nbsp;<span onclick=\"\" " +
                                         "class=\"style1\">" + appThai + "</span></td></tr>";
                        genJavaScriptInnerHTML += "if(appno==\"" + i + "\"){" +
                                                  "appOpen.value = \"" + i + "\";" +
                                                  "hf_appEng.value = \"" + appEng + "\";" +
                                                  "detail.innerHTML =\"<center>" +
                                                  "<span class='styleDetail'>" + appThai + "</span><br />" +
                                                  "วันทำการ : " + workDate + "<br>สถานะ เปิด/ปิด งาน :<br>" + clsdayStatus + "<br><br>" +
                                                  "<span style='text-decoration: underline; cursor:pointer;' onclick='pageopen(" + i + ")'>" +
                                                  "เข้าสู่ระบบ</span></center>\";}";
                        genJavaScript += "if (sys ==" + i + ") { window.location = \"" + locationUrl + appEng + "\"; }";
                        appno++;
                    }
                    else if (i >= rowpercol)
                    {
                        setRightOutput += "<tr><td id='tdMenu" + i + "' onclick=\"on_click('" + i + "');\" " +
                                         "onmouseover=\"document.getElementById('tdMenu" + i + "').style.background = 'url(" + mouseOver + ") no-repeat';\" " +
                                         "onmouseout=\"document.getElementById('tdMenu" + i + "').style.background = 'url(" + mouseOut + ") no-repeat';\"  " +
                                         "align='center' style='background: url(" + defaultButton + ") " +
                                         "no-repeat;size:auto; padding:15px 1px 15px 1px; cursor:pointer;'>" +
                                         //"&nbsp;&nbsp;<span onclick=\"on_click('" + i + "');\" " +
                                         "&nbsp;&nbsp;<span onclick=\"\" " +
                                         "class=\"style1\">" + appThai + "</span></td></tr>";
                        genJavaScriptInnerHTML += "if(appno==\"" + i + "\"){" +
                                                  "appOpen.value = \"" + i + "\";" +
                                                  "hf_appEng.value = \"" + appEng + "\";" +
                                                  "detail.innerHTML =\"<center>" +
                                                  "<span class='styleDetail'>" + appThai + "</span><br />" +
                                                  "วันทำการ : " + workDate + "<br>สถานะ เปิด/ปิด งาน :<br>" + clsdayStatus + "<br><br>" +
                                                  "<span style='text-decoration: underline; cursor:pointer;' onclick='pageopen(" + i + ")'>" +
                                                  "เข้าสู่ระบบ</span></center>\";}";
                        genJavaScript += "if (sys ==" + i + ") { window.location = \"" + locationUrl + appEng + "\"; }";
                        appno++;

                    }
                }
                genJavaScriptInnerHTML += "}</script>";
                genJavaScript += "}</script>";
                LtLeft.Text = setLeftOutput;
                LtRight.Text = setRightOutput;
                //LtMainMenu.Text = setMainOutput;
                //LtExpandMenu.Text = setExpandOutput;
            }
            catch (Exception exc)
            {

                genJavaScript = "";
                String outputHTML = "<table style=\"width: 40%;text-align: center;\" border=\"0\">" +
                                    "<tr><td><br />" +
                    //ข้อความ
                                    "<span style=\"color: Red;\">เกิดข้อผิดพลาด!<br><br>กรุณาเลือก Database ใหม่<div id='countdown'></div></span>" +
                                    "</td></tr></table><br>";
                String refreshtime = "61";
                genExcJavaScript = "<script type='text/javascript'>var milisec=0;var seconds=" + refreshtime + ";document.getElementById('countdown').innerHTML ='Auto refresh in (" + (Convert.ToInt32(refreshtime) - 1).ToString() + ")';function display(){if(milisec<=0){milisec=9;seconds-=1;}if (seconds<=-1){milisec=0;seconds+=1;location.reload(true);}else{milisec-=1;document.getElementById('countdown').innerHTML ='Auto refresh in ('+seconds+')';setTimeout('display()',100);}} display();</script> ";
                LtLeft.Text = "";
                LtRight.Text = "";
                //LtMainMenu.Text = "";
                //LtExpandMenu.Text = "";
                LtServerMessage.Text = WebUtil.ErrorMessage("<center>" + exc.Message + "<br><br>" + outputHTML + "</center >");
                PnlSelectDBError.Visible = true;
                PnlSelectDB.Visible = false;
                Label1.Text = "";

            }
        }

        protected void DlSelectDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (connString != "000" && connString != "")
            {
                Session["ss_connectionstring"] = connString;
                Session["ss_connectmode"] = ConnectMode.Manual;
                String strDBConnect = WebUtil.GetConnectionElement("User ID") + "@" + WebUtil.GetConnectionElement("Data Source");
                Label1.Text = "DB: " + strDBConnect + " [Manual Connect]";
                state = new WebState(Session, Request);
                SetConnString();
                GetStatusApplication();
            }
        }

        private void SetConnString()
        {
            Common wscom = WsUtil.Common;
            wsPass = new Decryption().DecryptAscii(wscom.GetPasswordService()) + "+ ";
            if (state.SsConnectMode == ConnectMode.Auto)
            {
                String wsPassEnCrypt = new Encryption().EncryptStrBase64(wsPass);
                Session["ss_connectionstring"] = new Decryption().DecryptStrBase64(wscom.GetConnectionString(wsPassEnCrypt));
                String strDBConnect = WebUtil.GetConnectionElement("User ID") + "@" + WebUtil.GetConnectionElement("Data Source");
                Label1.Text = "DB: " + strDBConnect + " [Auto Connect]";
            }
            else
            {
                if (connString != "")
                {
                    Session["ss_connectionstring"] = connString;
                }
            }

            //else { Session["ss_connectionstring"] = connString; }
            //set wspass เพื่อใช้ในการ เรียกใช้ Method ใน WebService
            Session["ss_wspass"] = new Encryption().EncryptStrBase64(wsPass.Trim() + Session["ss_connectionstring"].ToString());
            wsPass = Session["ss_wspass"].ToString();// wsPass.Trim() + Session["ss_connectionstring"].ToString();
        }

        private void GenListDB()
        {
            //Get Connection String จาก Saving
            int conDbList = ConfigurationManager.ConnectionStrings.Count;
            try
            {
                connString = DlSelectDB.SelectedValue;
                connItem = DlSelectDB.SelectedItem.ToString();
            }
            catch { connString = ""; connItem = ""; };
            try
            {
                if (connString == "000")
                {
                    connString = DlSelectDBError.SelectedValue;
                    connItem = DlSelectDBError.SelectedItem.ToString();
                }
            }
            catch { connString = ""; connItem = ""; };
            DlSelectDB.Items.Clear();
            DlSelectDBError.Items.Clear();
            ListItem[] list = new ListItem[conDbList];
            list[0] = new ListItem();
            list[0].Text = "-- เลือกเพื่อเปลี่ยน Database --";
            list[0].Value = "000";
            DlSelectDB.Items.Add(list[0]);
            DlSelectDBError.Items.Add(list[0]);
            for (int i = 1; i < list.Length; i++)
            {
                list[i] = new ListItem();
                list[i].Text = ConfigurationManager.ConnectionStrings[i].Name;
                list[i].Value = ConfigurationManager.ConnectionStrings[i].ConnectionString;
                DlSelectDB.Items.Add(list[i]);
                DlSelectDBError.Items.Add(list[i]);
            }
            //จบการ set ค่า เพื่อ add ConnectionString จาก Saving ใส่ใน DropDown;List

        }
    }

}
