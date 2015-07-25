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

namespace Saving.Flash
{
    public partial class Index : System.Web.UI.Page
    {
        private WebState state;
        protected String genJavaScript = "";
        protected String genExcJavaScript = "";

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
                    state.Logout();
                }
                else if (Request["cmd"].ToString() == "switch")
                {
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
            String setMainOutput = "";
            String setExpandOutput = "";
            genJavaScript = "<script type=\"text/javascript\">function pageopen(sys){";
            genExcJavaScript = "";

            Common wscom = WsUtil.Common;
            wscom.Discover();

            String appList = "";

            try
            {
                appList = wscom.GetStatusApplication(wsPass);
                int m = 1;
                int x = 1;
                for (int i = 0; i < appList.Split('|').Length - 1; i++)
                {
                    String tempAppList = appList.Split('|').GetValue(i).ToString();

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
                            clsdayStatus = "<span style=\"color: Green;\">ปิดงานสิ้นวันแล้ว</span>";
                        }
                        else if (closeDay == "1")
                        {
                            clsdayStatus = "<span style=\"color: Green;\">ปิดงานสิ้นวันแล้ว<span>";
                        }
                    }
                    else if (workDate != today)
                    {
                        if (closeDay == "0")
                        {
                            clsdayStatus = "<span style=\"color: Red;\">วันทำการไม่ตรงกับวันปัจจุบัน</span>";
                        }
                        else if (closeDay == "1")
                        {
                            clsdayStatus = "<span style=\"color: Red;\">ยังไม่ได้ปิดงานสิ้นวัน</span>";
                        }
                    }

                    String locationUrl = "../Login.aspx?app=";
                    if (i < 6)
                    {

                        setMainOutput += "<tr><td class=\"seticon\" width=\"55%\" " +
                                        "style=\"padding:12px 0 12px 0; cursor:pointer; \">" +
                                        "<img id=\"main" + m + "\" alt=\"\" src=\"Main" + m + ".png\" " +
                                        "onclick=\"pageopen('main" + m + "');\" /></td><td style=\"text-align: " +
                                        "right; padding:0 10px 0 0;\"><span class=\"style1\">" + appThai + "</span>" +
                                        "<br><br>วันทำการ : " + workDate + "<br>สถานะ เปิด/ปิด งาน :" +
                                        "<br>" + clsdayStatus + "</td></tr>";
                        genJavaScript += "if (sys == \"main" + m + "\") { window.location = \"" + locationUrl + appEng + "\"; }";
                        m++;
                    }
                    else if (i >= 6)
                    {
                        setExpandOutput += "<tr><td style=\"text-align: left; " +
                                        "padding:0 0 0 10px;\"><span class=\"style1\">" + appThai + "</span>" +
                                        "<br><br>วันทำการ : " + workDate + "<br>สถานะ เปิด/ปิด งาน :" +
                                        "<br>" + clsdayStatus + "</td><td class=\"seticon\" width=\"55%\" " +
                                        "style=\"padding:12px 0 12px 0; cursor:pointer;\">" +
                                        "<img id=\"add" + x + "\" alt=\"\" src=\"Expand" + x + ".png\" " +
                                        "onclick=\"pageopen('expand" + x + "');\"/></td></tr>";
                        genJavaScript += "if (sys == \"expand" + x + "\") { window.location = \"" + locationUrl + appEng + "\"; }";
                        x++;

                    }
                }
                genJavaScript += "}</script>";
                LtMainMenu.Text = setMainOutput;
                LtExpandMenu.Text = setExpandOutput;
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
                LtMainMenu.Text = "";
                LtExpandMenu.Text = "";
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
