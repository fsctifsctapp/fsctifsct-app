//รายชื่อ SESSION ทั้งหมดของระบบ
//ss_application
//ss_username
//ss_pagepermiss        เป็น DataTable สำหรับรวมรายชื่อสิทธิการใช้งาน
//ss_menugroup          กลุ่มเมนูที่ใช้อยู่ปัจจุบัน
//ss_menugroupdesc      กลุ่มเมนูที่ใช้อยู่ปัจจุบัน *(เป็นชื่อภาษาไทย)
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.SessionState;
using DBAccess;
using System.Data.OracleClient;
using System.Net;
using SecurityEngine;

namespace Saving.CmConfig
{
    /// <summary>
    /// ใช้สำหรับดูสถานะของผู้ใช้ และ Web Browser
    /// </summary>
    public class WebState
    {
        private HttpSessionState session;
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_application"] : ระบุว่าตอนนี้ใช้ application อะไรอยู่
        /// </summary>
        public String SsApplication
        {
            get { try { return session["ss_application"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_thaiapplication"] : ระบุว่าตอนนี้ใช้ application อะไรอยู่ภาษาไทย
        /// </summary>
        /// <remarks>
        /// จะถูก set ตั้งแต่ใน frame master เมื่อมีการเลือก Application และนำชื่อระบบภาษาไทยมาจาก WebService เก็บใน session
        /// </remarks>
        public String SsThaiApplication
        {
            get { try { return session["ss_thaiapplication"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_username"] : ระบุว่าตอนนี้ผู้ใช้คือ username อะไร
        /// </summary>
        public String SsUsername
        {
            get { try { return session["ss_username"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_menugroup"] : ระบุว่าเมนูด้านซ้ายคือ group code อะไร
        /// </summary>
        public String SsMenuGroup
        {
            get { try { return session["ss_menugroup"].ToString(); } catch { return null; } }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_menugroup"] : ระบุว่าเมนูด้านซ้ายชื่อ group(ภาษาไทย) ว่าอะไร
        /// </summary>
        public String SsMenuGroupDesc
        {
            get { try { return session["ss_menugroupdesc"].ToString(); } catch { return null; } }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_pagepermiss"] : DataTable เก็บข้อมูลเกี่ยวกับเมนูและสิทธิ์
        /// </summary>
        public DataTable SsPagePermiss
        {
            get
            {
                try
                {
                    return session["ss_pagepermiss"] as DataTable;

                }
                catch { return null; }
            }
        }
        /// <summary>
        /// ReadOnly : ตัวแปร Session["ss_userlevel"] : ระดับของผู้เข้าใช้งาน 1:Admin 2:Leader 3:User | if null default = 3
        /// </summary>
        public int SsUserLevel
        {
            get { try { return Convert.ToInt32(session["ss_userlevel"]); } catch { return 3; } }
        }
        /// <summary>
        /// ค่า Connection String ตั้งไว้เป็น Session
        /// </summary>
        public String SsConnectionString
        {
            get { try { return session["ss_connectionstring"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : วิธีการ Connect [Auto:1] [Manual:0]
        /// </summary>
        public ConnectMode SsConnectMode
        {
            get
            {
                try
                {
                    return (ConnectMode)session["ss_connectmode"];
                }
                catch { return ConnectMode.Auto; }
            }
        }
        /// <summary>
        /// ReadOnly : Password WebService + ConnectionString ใช้สำหรับส่งให้ Webservice
        /// </summary>
        public String SsWsPass
        {
            get { try { return session["ss_wspass"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : หมายเลขสหกรณ์
        /// </summary>
        public String SsCoopNo
        {
            get { try { return session["ss_coopno"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ชื่อสหกรณ์
        /// </summary>
        public String SsCoopName
        {
            get { try { return session["ss_coopname"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ชื่อเครื่องที่เข้าใช้งาน
        /// </summary>
        public String SsClientComputerName
        {
            get { try { return session["ss_computername"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : ip เครื่องที่เข้าใช้งาน
        /// </summary>
        public String SsClientIp
        {
            get { try { return session["ss_clientip"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : สถานะการปิดงานสิ้นวันของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseDayStatus
        {
            get { try { return session["ss_closedaystatus"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : สถานะการปิดงานสิ้นเดือนของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseMonthStatus
        {
            get { try { return session["ss_closemonthstatus"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : สถานะการปิดงานสิ้นปีของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseYearStatus
        {
            get { try { return session["ss_closeyearstatus"].ToString(); } catch { return ""; } }
        }
        /// <summary>
        /// ReadOnly : วันทำการ
        /// </summary>
        public DateTime SsWorkDate
        {
            get { try { return Convert.ToDateTime(session["ss_workdate"]); } catch { return new DateTime(1970, 1, 1); } }
        }
        /// <summary>
        /// ReadOnly : สาขาของสหกรณ์ Default=001
        /// </summary>
        public String SsBranchId
        {
            get { try { return session["ss_branchid"].ToString(); } catch { return ""; } }
        }
        public String SsPrinterSet
        {
            get
            {
                try
                {
                    return session["ss_printerset"].ToString();
                }
                catch { return ""; }
            }
        }
        public String SsUrl
        {
            get
            {
                try
                {
                    return session["ss_url"].ToString();
                }
                catch
                {
                    return "http://127.0.0.1/GCOOP/Saving/";
                }
            }
        }
        public String SsLoginIP
        {
            get
            {
                try
                {
                    return session["ss_loginip"].ToString();
                }
                catch (Exception ex)
                {
                    return "0.0.0.0";
                }
            }
        }
        private bool isLocalIp;
        public bool SsIsLocalIP
        {
            get { return isLocalIp; }
        }
        //------ REQUEST
        private HttpRequest request;
        private String currentPage;
        /// <summary>
        /// ReadOnly : บอกว่าหน้าจอปัจจุบันชื่ออะไร (เช่น Home.aspx)
        /// </summary>
        public String CurrentPage
        {
            get { return currentPage; }
        }
        private String currentPageName;
        /// <summary>
        /// ReadOnly : บอกชื่อหน้าจอปัจจุบันภาษาไทย ที่ได้จาก Table AMSECWINS ใช้สำหรับ Tag Title 
        /// (หากไม่มีข้อมูล Default คือ GCOOP - Isocare)
        /// </summary>
        public String CurrentPageName
        {
            get { return currentPageName; }
        }
        private String url;
        /// <summary>
        /// ReadOnly : บอก URL สำหรับ Root directory เช่น http://localhost:8080/GCOOP/Saving/
        /// </summary>
        public String Url
        {
            get { return url; }
        }
        private String appCompany;
        /// <summary>
        /// ReadOnly : บอกว่าโปรแกรมอะไร จาก บ.อะไร (default = GCOOP - Isocare)
        /// </summary>
        public String AppCompany
        {
            get { return appCompany; }
        }
        private String appTitle;
        /// <summary>
        /// ReadOnly : Title ของ หน้า Page
        /// </summary>
        public String AppTitle
        {
            get { return appTitle; }
        }
        //------ STATUS
        private Boolean isLogin;
        /// <summary>
        /// ReadOnly : บอกว่าอยู่ในสถานะ Login แล้วหรือไม่
        /// </summary>
        public Boolean IsLogin
        {
            get { return isLogin; }
        }
        private Boolean isReadable;
        /// <summary>
        /// ReadOnly : บอกว่ามีสิทธ์ดูหน้าปัจจุบันได้หรือไม่
        /// </summary>
        public Boolean IsReadable
        {
            get { return isReadable; }
        }
        private Boolean isWritable;
        /// <summary>
        /// ReadOnly : บอกว่ามีสิทธ์บันทึกข้อมูลหน้าปัจจุบันได้หรือไม่
        /// </summary>
        public Boolean IsWritable
        {
            get { return isWritable; }
        }
        private String siteLogo;
        public String SiteLogo
        {
            get { return siteLogo; }
        }
        private String siteTName;
        public String SiteTName
        {
            get { return siteTName; }
        }
        private String siteEName;
        public String SiteEName
        {
            get { return siteEName; }
        }
        private String siteLinkName;
        public String SiteLinkName
        {
            get { return siteLinkName; }
        }
        private String strConnectMode;
        public String StrConnectMode
        {
            get { return strConnectMode; }
        }
        private String reportLink;
        public String ReportLink
        {
            get { return reportLink; }
        }
		

        public WebState()
        {
            try
            {
                Page page = new System.Web.UI.Page();
                this.session = page.Session;
                this.request = page.Request;
				this.isLocalIp = true;
            }
            catch { }
        }

        /// <summary>
        /// สร้างตัวแปรสำหรับดูสถานะของ Web Application
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="Request"></param>
        /// 
        public WebState(HttpSessionState Session, HttpRequest Request)
        {
            this.session = Session;
            this.request = Request;
            this.isReadable = false;
            this.isWritable = false;
			this.isLocalIp = true;
            siteLogo = "img/band_black.jpg";
            //siteTName = "สหกรณ์ออมทรัพย์การสื่อสารแห่งประเทศไทย จำกัด";
            siteTName = this.SsCoopName;
            siteEName = "THE COMMUNICATIONS AUTHORITY OF THAILAND SAVINGS CO-OPERATIVE LTD.";
            siteLinkName = "http://www.postcatsavings.com/index.html";
            currentPageName = "";
            appCompany = "CAT SAVINGS CO-OPERATIVE LTD.";

            //Connect Mode
            try
            {
                if ((ConnectMode)session["ss_connectmode"] == ConnectMode.Manual)
                { session["ss_connectmode"] = ConnectMode.Manual; strConnectMode = "Manual"; }
                else { session["ss_connectmode"] = ConnectMode.Auto; strConnectMode = "Auto"; }
            }
            catch { session["ss_connectmode"] = ConnectMode.Auto; }

            //----------- REQUEST
            //ตั้งค่า Application
            try
            {
                //oldApp ใช้เพื่อเช็คว่ามีการเปลี่ยน application หรือไม่
                String oldApp;
                try
                {
                    oldApp = session["ss_application"].ToString();
                }
                catch { oldApp = null; }
                // เงื่อนไขที่ 1. ตรวจสอบว่ามี  Applicaions จาก Url หรือไม่
                try
                {
                    if (request.Url.Segments[3] == "Applications/" && request.Url.Segments[4] != null)
                    {
                        session["ss_application"] = request.Url.Segments[4].ToLower().Replace("/", "");
                    }
                }
                catch { }
                // เงื่อนไขที่ 2. ตรวจสอบว่าส่ง Request["app"] มาหรือไม่
                if (request["app"] != null)
                {
                    session["ss_application"] = request["app"].ToString().Trim();
                }
                // เงื่อนไขที่ 3. ตรวจสอบว่ามีค่า Session เดิมหรือไม่ถ้า application == null
                if (request["app"] == null && oldApp != null && SsApplication == null)
                {
                    session["ss_application"] = SsApplication;
                }
                //เช็คว่าเมนูมีการเปลี่ยน Application รึเปล่า ถ้าเปลี่นนก็ให้ Refresh Menu
                WsCommon.Common comm = new WsCommon.Common();
                Session["ss_thaiapplication"] = comm.GetApplicationThai(SsWsPass, SsApplication);
                if ((oldApp != null && SsApplication != oldApp) || (SsPagePermiss == null && SsApplication != null))
                {
                    RefreshMenu(SsApplication);
                }
            }
            catch
            {
                session["ss_application"] = null;
            }
            //Request["app"]
            //ตั้งค่า REQUEST + URL หลัก + CurrentPage
            try
            {
                url = String.Format("http://{0}/GCOOP/Saving/", request.Url.Authority);
                session["ss_url"] = url;
                currentPage = new System.IO.FileInfo(request.Url.AbsolutePath).Name.Trim();
                reportLink = url + "ReportDefault.aspx";
            }
            catch { }

            //----------- SESSION
            //Username & IsLOGIN
            try
            {
                isLogin = !((SsUsername == null) || (SsUsername.Trim() == ""));
            }
            catch
            {
                isLogin = false;
            }
            //ตั้งค่า MenuSub
            try
            {
                if (SsApplication != null)
                {
                    SetApplicationDetail(SsApplication);
                    RefreshMenu(SsApplication);
                    SetMenuSub();
                }
            }
            catch { }

            if (currentPageName == "")
            {
                if (SsMenuGroupDesc == null || SsMenuGroupDesc == "")
                {
                    appTitle = appCompany + " - " + SsThaiApplication;
                }
                else
                {
                    appTitle = "[ " + SsMenuGroupDesc + " ] - " + appCompany + " - " + SsThaiApplication;
                }

            }
            else
            {
                appTitle = "[ " + currentPageName + " ]" + " - " + SsThaiApplication + " - " + appCompany;
            }
            session["ss_loginip"] = Request.UserHostAddress;
            if (isLogin)
            {
                session.Timeout = 20;
            }
        }

        /// <summary>
        /// สำหรับ Login เข้าสู่ระบบโดยก่อนจะ Login ต้องเลือก Application ก่อน
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>URL สำหรับให้หน้า Login.aspx จะ Redirect ไปหน้าไหนตามแต่ระบบ</returns>
        public String Login(String username, String password, String application, String branchid, String printerSet)
        {
            WsCommon.Common comm = null;
            try
            {
                comm = new Saving.WsCommon.Common();
            }
            catch { }

            //wsPass = new Decrypt().setDecrypttionWs(comm.GetPasswordService()) + "+ ";
            //if (SsConnectMode == ConnectMode.Auto) { 
            //    session["ss_connectionstring"] = comm.GetConnectionString(wsPass); 
            //}

            ////set wspass เพื่อใช้ในการ เรียกใช้ Method ใน WebService
            //session["ss_wspass"] = wsPass.Trim() + SsConnectionString;

            String[] rs = comm.VerifyLogin(SsWsPass, username, password, application);
            String redir = "";

            if (rs[0].ToString() == "true")// Login ผ่าน 
            {
                session["ss_username"] = username.Trim();//Add username ลง Session ครั้งแรก
                session["ss_branchid"] = branchid;//Add branchid ลง Session ครั้งแรก
                session["ss_printerset"] = printerSet;
                try { session["ss_userlevel"] = Convert.ToInt32(rs[1].ToString()); }
                catch { session["ss_userlevel"] = 3; }
                isLogin = true;
                SetApplicationDetail(application);
                RefreshMenu(application);// ทำการ สร้าง หรือ refresh Menu 
                redir = url + "Default.aspx";
            }
            else
            {
                isLogin = false;
                //session.Remove("ss_wspass");
                //if (SsConnectMode == ConnectMode.Auto) { session.Remove("ss_connectionstring"); }
                throw new Exception("Can not verify Username or Password.");
            }
            return redir;

        }

        /// <summary>
        /// 
        /// </summary>
        public void Logout()
        {
            session.RemoveAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public String RefreshMenu(String application)
        {
            String redir = "";
            WsCommon.Common comm = null;

            try { comm = new Saving.WsCommon.Common(); }
            catch { String aa = "เชื่อต่อ WebService ไม่ได้"; }

            if (isLogin)
            {
                session["ss_application"] = application;
                session["ss_pagepermiss"] = comm.GetPermissMenu(SsWsPass, SsUsername, application);
                redir = url + "Default.aspx";
            }
            return redir;
        }

        private void SetMenuSub()
        {
            try
            {
                session["ss_menugroup"] = request["gcode"].Trim();
            }
            catch
            {
                if (SsMenuGroup != null) { }
                else
                {
                    session["ss_menugroup"] = null;
                }
            }
            session["ss_menugroupdesc"] = null;
            for (int i = 0; i < SsPagePermiss.Rows.Count; i++)
            {
                if (SsPagePermiss.Rows[i]["WIN_OBJECT"].ToString().Trim().ToLower() + ".aspx" == currentPage.ToLower().Trim())
                {
                    currentPageName = SsPagePermiss.Rows[i]["WIN_DESCRIPTION"].ToString().Trim();
                    session["ss_menugroup"] = SsPagePermiss.Rows[i]["GROUP_CODE"].ToString().Trim();
                    session["ss_menugroupdesc"] = SsPagePermiss.Rows[i]["GROUP_DESC"].ToString().Trim();
                    isReadable = true;
                    try
                    {
                        isWritable = Convert.ToInt32(SsPagePermiss.Rows[i]["SAVE_STATUS"]) == 1;
                    }
                    catch { }
                    break;
                }
            }
            //Tast TabPage
            if (currentPage.ToLower().Trim().Equals("tabpage.aspx"))
            {
                currentPageName = "เทส Tab";
                session["ss_menugroup"] = "A";
                session["ss_menugroupdesc"] = "สมาชิก";
                isWritable = true;

            }
            if (SsMenuGroupDesc == null && SsMenuGroup != null)
            {
                for (int i = 0; i < SsPagePermiss.Rows.Count; i++)
                {
                    if (SsPagePermiss.Rows[i]["GROUP_CODE"].ToString().Trim() == SsMenuGroup)
                    {
                        session["ss_menugroupdesc"] = SsPagePermiss.Rows[i]["GROUP_DESC"].ToString();
                        break;
                    }
                }
            }
        }

        public void SetApplicationDetail(String app)
        {
            if (session["ss_computername"] == null || session["ss_clientip"] == null)
            {
                String ipAdd = "127.0.0.1";
                try
                {
                    ipAdd = request.UserHostAddress;
                }
                catch { }
                String[] ipAndName = GetIPAddressAndName(ipAdd);
                session["ss_clientip"] = ipAndName[0];
                session["ss_computername"] = ipAndName[1];
            }

            String sql = "";
            DataTable dt;

            Sta ta = new Sta(SsConnectionString);
            try
            {
                sql = "select workdate,closeday_status,closemonth_status,closeyear_status from amappstatus where application='" + app + "'";

                dt = WebUtil.Query(sql);

                try { session["ss_workdate"] = Convert.ToDateTime(dt.Rows[0]["workdate"]); }
                catch { session["ss_workdate"] = new DateTime(1970, 1, 1); }
                try { session["ss_closedaystatus"] = dt.Rows[0]["closeday_status"].ToString().Trim(); }
                catch { session["ss_closedaystatus"] = "0"; }
                try { session["ss_closemonthstatus"] = dt.Rows[0]["closemonth_status"].ToString().Trim(); }
                catch { session["ss_closemonthstatus"] = "0"; }
                try { session["ss_closeyearstatus"] = dt.Rows[0]["closeyear_status"].ToString().Trim(); }
                catch { session["ss_closeyearstatus"] = "0"; }

                sql = "";
                dt.Clear();

                sql = "select coop_no,coop_name from cmcoopconstant where site='CAT'"; //default สหกรณ์ 000 จาก DB

                dt = WebUtil.Query(sql);
                try { session["ss_coopno"] = dt.Rows[0]["coop_no"].ToString().Trim(); }
                catch { session["ss_coopno"] = "000"; }
                try { session["ss_coopname"] = dt.Rows[0]["coop_name"].ToString().Trim(); }
                catch { session["ss_coopname"] = "ไม่มีการกำหนดชื่อสหกรณ์"; }
                ta.Close();
            }
            catch (Exception ex) { ta.Close(); throw ex; }
        }

        /// <summary>
        /// คืนค่าเป็น Array 2 ค่า<br />
        /// result[0] = IpAddress <br />
        /// result[1] = ClientName <br />
        /// </summary>
        /// <param name="ipp"></param>
        /// <returns></returns>
        private String[] GetIPAddressAndName(string ipp)
        {
            String[] ipAndName = new String[2];
            try
            {
                if (ipp == "127.0.0.1")
                {
                    ipAndName[0] = "127.0.0.1";
                    ipAndName[1] = Dns.GetHostName().ToString();
                    return ipAndName;
                }
                IPAddress[] ip = Dns.GetHostAddresses(ipp);
                IPHostEntry iph = Dns.GetHostEntry(ip[0]);
                String ipFormat = System.Configuration.ConfigurationManager.AppSettings["ipAddressFormat"].ToString().Replace("X", "");
                String comName = iph.HostName;
                ip = iph.AddressList;
                String ipAddress = "";
                for (int i = 0; i < ip.Length; i++)
                {
                    try
                    {
                        if (ip[i].ToString().Substring(0, ipFormat.Length) == ipFormat)
                        {
                            ipAddress = ip[i].ToString();
                            break;
                        }
                    }
                    catch { }
                }
                if (ipAddress == "") throw new Exception("NULL_IP");
                if (String.IsNullOrEmpty(comName)) throw new Exception("NULL_NAME");
                ipAndName[0] = ipAddress;
                ipAndName[1] = comName;
            }
            catch (Exception ex)
            {
                ipAndName[0] = ipp;
                ipAndName[1] = ("exc:" + ex.Message).Replace(" ", "_");
            }
            return ipAndName;
        }
    }
}
