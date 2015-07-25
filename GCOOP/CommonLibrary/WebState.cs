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
using CommonLibrary.WsCommon;
//using adminservice;

namespace CommonLibrary
{
    /// <summary>
    /// ใช้สำหรับดูสถานะของผู้ใช้ และ Web Browser
    /// </summary>
    public class WebState
    {
        private HttpSessionState Session;
        private HttpApplicationState ApplicationState;
        private XmlConfigService xmlConfig;

        public bool WinLogUsing
        {
            get { return xmlConfig.WinLogUsing; }
        }

        public String WinLogIP
        {
            get { return xmlConfig.WinLogIP; }
        }

        public int WinLogPort
        {
            get { return xmlConfig.WinLogPort; }
        }

        public bool ClondUsing
        {
            get { return xmlConfig.ClondUsing; }
        }

        public String ClondIP
        {
            get { return xmlConfig.ClondIP; }
        }

        public int ClondPort
        {
            get { return xmlConfig.ClondPort; }
        }

        public String SsDbProfile
        {
            get
            {
                try
                {
                    string resu = "";
                    resu = this.GetConnectionElement("User ID") + "@" + this.GetConnectionElement("Data Source");
                    if (resu.Trim() == "@") throw new Exception();
                    return resu;
                }
                catch
                {
                    return "";
                }
            }
        }

        public DateTime SsLastClick
        {
            get { return (DateTime)Session["ss_lastclick"]; }
            set { Session["ss_lastclick"] = value; }
        }

        /// <summary>
        /// get, set : ระบุว่าตอนนี้ใช้ application อะไรอยู่
        /// </summary>
        public String SsApplication
        {
            get
            {
                try
                {
                    if (Request == null) return Session["ss_application"].ToString();
                    String oldApp = "";
                    try
                    {
                        oldApp = Session["ss_application"].ToString();
                    }
                    catch { }

                    // เงื่อนไขที่ 1. ตรวจสอบว่ามี  Applicaions จาก Url หรือไม่
                    try
                    {
                        if (Request.Url.Segments[3] == "Applications/" && Request.Url.Segments[4] != null)
                        {
                            Session["ss_application"] = Request.Url.Segments[4].ToLower().Replace("/", "");
                        }
                    }
                    catch { }

                    // เงื่อนไขที่ 2. ตรวจสอบว่าส่ง Request["app"] มาหรือไม่
                    if (Request["app"] != null)
                    {
                        Session["ss_application"] = Request["app"].ToString().Trim();
                    }

                    // เงื่อนไขที่ 3. ตรวจสอบว่ามีค่า Session เดิมหรือไม่ถ้า application == null
                    return Session["ss_application"].ToString();
                }
                catch
                {
                    Session["ss_application"] = "";
                    return "";
                }
            }
            set
            {
                Session["ss_application"] = value;
            }
        }

        /// <summary>
        /// get, set : ระบุว่าตอนนี้ใช้ cs_type อะไรอยู่
        /// </summary>
        public String SsCsType
        {
            get
            {
                try
                {
                    if (Request == null) return Session["ss_cstype"].ToString().Trim();
                    String oldApp = "";
                    try
                    {
                        oldApp = Session["ss_cstype"].ToString().Trim();
                    }
                    catch { }

                    // เงื่อนไขที่ 1. ตรวจสอบว่ามี  Applicaions จาก Url หรือไม่
                    try
                    {
                        //if (Request.Url.Segments[3] == "Applications/" && Request.Url.Segments[4] != null)
                        //{
                        //    Session["ss_cstype"] = Request.Url.Segments[4].ToLower().Replace("/", "");
                        //}
                    }
                    catch { }

                    // เงื่อนไขที่ 2. ตรวจสอบว่าส่ง Request["app"] มาหรือไม่
                    if (Request["cstype"] != null)
                    {
                        this.SsConnectMode = ConnectMode.Manual;
                        try
                        {
                            Common wscom = WsUtil.Common;
                            String wsPass = new Decryption().DecryptAscii(wscom.GetPasswordService()) + "+ ";
                            this.SsConnectMode = ConnectMode.Manual;
                            this.SsConnectionString = ConfigurationManager.ConnectionStrings[Convert.ToInt32(Request["cstype"])].ConnectionString;
                            this.SsWsPass = new Encryption().EncryptStrBase64(wsPass.Trim() + this.SsConnectionString);
                        }
                        catch { }
                        Session["ss_cstype"] = Request["cstype"].ToString().Trim();
                    }
                    // เงื่อนไขที่ 3. ตรวจสอบว่ามีค่า Session เดิมหรือไม่ถ้า application == null
                    return Session["ss_cstype"].ToString().Trim();
                }
                catch
                {
                    Session["ss_cstype"] = "";
                    return "";
                }
            }
            set
            {
                Session["ss_cstype"] = value;
            }
        }

        /// <summary>
        /// get, set : ระบุว่าตอนนี้ใช้ cs_type อะไรอยู่
        /// </summary>
        public String SsCsDesc
        {
            get
            {
                try
                {
                    if (Request == null) return Session["ss_csdesc"].ToString().Trim();
                    String oldApp = "";
                    try
                    {
                        oldApp = Session["ss_csdesc"].ToString().Trim();
                    }
                    catch { }

                    // เงื่อนไขที่ 1. ตรวจสอบว่ามี  Applicaions จาก Url หรือไม่
                    try
                    {
                        //if (Request.Url.Segments[3] == "Applications/" && Request.Url.Segments[4] != null)
                        //{
                        //    Session["ss_cstype"] = Request.Url.Segments[4].ToLower().Replace("/", "");
                        //}
                    }
                    catch { }

                    // เงื่อนไขที่ 2. ตรวจสอบว่าส่ง Request["app"] มาหรือไม่
                    if (Request["csdesc"] != null)
                    {

                        Session["ss_csdesc"] = Request["csdesc"].ToString().Trim();
                    }
                    // เงื่อนไขที่ 3. ตรวจสอบว่ามีค่า Session เดิมหรือไม่ถ้า application == null
                    return Session["ss_csdesc"].ToString().Trim();
                }
                catch
                {
                    Session["ss_csdesc"] = "";
                    return "";
                }
            }
            set
            {
                Session["ss_csdesc"] = value;
            }
        }

        /// <summary>
        /// get : ระบุว่าตอนนี้ใช้ application อะไรอยู่ภาษาไทย
        /// </summary>
        /// <remarks>
        /// จะถูก set ตั้งแต่ใน frame master เมื่อมีการเลือก Application และนำชื่อระบบภาษาไทยมาจาก WebService เก็บใน session
        /// </remarks>
        public String SsThaiApplication
        {
            get { try { return Session["ss_thaiapplication"].ToString(); } catch { return ""; } }
            set { Session["ss_thaiapplication"] = value; }
        }

        /// <summary>
        /// get, set : ระบุว่าตอนนี้ผู้ใช้คือ username อะไร
        /// </summary>
        public String SsUsername
        {
            get
            {
                try { return Session["ss_username"].ToString(); }
                catch { return ""; }
            }
            set
            {
                Session["ss_username"] = value;
            }
        }

        /// <summary>
        /// get, set : ระบุว่าตอนนี้คือ group_code อะไร
        /// </summary>
        public String SsMenuGroup
        {
            get
            {
                try { return Session["ss_menugroup"].ToString(); }
                catch { return null; }
            }
            set
            {
                Session["ss_menugroup"] = value;
            }
        }

        /// <summary>
        /// get, set : ระบุว่าเมนูด้านซ้ายชื่อ group(ภาษาไทย) ว่าอะไร
        /// </summary>
        public String SsMenuGroupDesc
        {
            get
            {
                try { return Session["ss_menugroupdesc"].ToString(); }
                catch { return null; }
            }
            set
            {
                Session["ss_menugroupdesc"] = value;
            }
        }

        /// <summary>
        /// get, set : DataTable เก็บข้อมูลเกี่ยวกับเมนูและสิทธิ์
        /// </summary>
        public DataTable SsPagePermiss
        {
            get
            {
                try
                {
                    return Session["ss_pagepermiss"] as DataTable;

                }
                catch { return null; }
            }
            set
            {
                Session["ss_pagepermiss"] = value;
            }
        }

        /// <summary>
        /// get, set : ระดับของผู้เข้าใช้งาน 1:Admin 2:Leader 3:User | if null default = 3
        /// </summary>
        public int SsUserLevel
        {
            get { try { return Convert.ToInt32(Session["ss_userlevel"]); } catch { return 3; } }
            set { Session["ss_userlevel"] = value; }
        }

        public int SsUserType
        {
            get { try { return Convert.ToInt32(Session["ss_usertype"]); } catch { return 2; } }
            set { Session["ss_usertype"] = value; }
        }

        /// <summary>
        /// ค่า Connection String ตั้งไว้เป็น Session
        /// </summary>
        public String SsConnectionString
        {
            get { try { return Session["ss_connectionstring"].ToString(); } catch { return ""; } }
            set
            {
                Session["ss_connectionstring"] = value;
            }
        }

        /// <summary>
        /// get, set : วิธีการ Connect [Auto:1] [Manual:0]
        /// </summary>
        public ConnectMode SsConnectMode
        {
            get
            {
                ConnectMode cm;
                try
                {
                    if ((ConnectMode)Session["ss_connectmode"] == ConnectMode.Manual)
                    {
                        cm = ConnectMode.Manual;
                    }
                    else
                    {
                        cm = ConnectMode.Auto;
                    }
                }
                catch
                {
                    cm = ConnectMode.Auto;
                }
                return cm;
            }
            set
            {
                Session["ss_connectmode"] = value;
            }
        }

        /// <summary>
        /// ReadOnly : Password WebService + ConnectionString ใช้สำหรับส่งให้ Webservice
        /// </summary>
        public String SsWsPass
        {
            get { try { return Session["ss_wspass"].ToString().Trim(); } catch { return ""; } }
            set
            {
                Session["ss_wspass"] = value;
            }
        }

        /// <summary>
        /// get, set : หมายเลขสหกรณ์
        /// </summary>
        public String SsCoopNo
        {
            get
            {
                try { return Session["ss_coopno"].ToString(); }
                catch { return ""; }
            }
            set
            {
                Session["ss_coopno"] = value;
            }
        }

        /// <summary>
        /// get, set : ชื่อสหกรณ์
        /// </summary>
        public String SsCoopName
        {
            get
            {
                try
                {
                    return Session["ss_coopname"].ToString();
                }
                catch { return ""; }
            }
            set { Session["ss_coopname"] = value; }
        }

        /// <summary>
        /// get, set : ชื่อเครื่องที่เข้าใช้งาน
        /// </summary>
        public String SsClientComputerName
        {
            get
            {
                try
                {
                    String pcName = "";
                    try
                    {
                        pcName = Session["ss_clientcomputername"].ToString();
                    }
                    catch { }
                    string ipAdd = this.SsClientIp;
                    if (string.IsNullOrEmpty(pcName) && !string.IsNullOrEmpty(ipAdd))
                    {
                        String[] ipAndName = GetIPAddressAndName(ipAdd);
                        Session["ss_computername"] = ipAndName[1];
                        return ipAndName[1];
                    }
                    else
                    {
                        return Session["ss_computername"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// ReadOnly : ip เครื่องที่เข้าใช้งาน
        /// </summary>
        public String SsClientIp
        {
            get
            {
                try
                {
                    string bfIpAdd = "";
                    try
                    {
                        bfIpAdd = Session["ss_clientip"].ToString();
                    }
                    catch { }
                    if (string.IsNullOrEmpty(bfIpAdd))
                    {
                        String ipAdd = "127.0.0.1";
                        try
                        {
                            ipAdd = Request.UserHostAddress;
                        }
                        catch { }
                        String[] ipAndName = GetIPAddressAndName(ipAdd);
                        Session["ss_clientip"] = ipAndName[0];
                        return ipAndName[0];
                    }
                    else
                    {
                        return bfIpAdd;
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// get, set : สถานะการปิดงานสิ้นวันของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseDayStatus
        {
            get { try { return Session["ss_closedaystatus"].ToString(); } catch { return ""; } }
            set { Session["ss_closedaystatus"] = value; }
        }

        /// <summary>
        /// get, set : สถานะการปิดงานสิ้นเดือนของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseMonthStatus
        {
            get { try { return Session["ss_closemonthstatus"].ToString(); } catch { return ""; } }
            set { Session["ss_closemonthstatus"] = value; }
        }

        /// <summary>
        /// get, set : สถานะการปิดงานสิ้นปีของระบบ 0=ปิดแล้ว 1=ยังไม่ได้ปิด
        /// </summary>
        public String SsCloseYearStatus
        {
            get
            {
                try { return Session["ss_closeyearstatus"].ToString(); }
                catch { return ""; }
            }
            set
            {
                Session["ss_closeyearstatus"] = value;
            }
        }

        /// <summary>
        /// get, set : วันทำการ
        /// </summary>
        public DateTime SsWorkDate
        {
            get
            {
                try
                {
                    String application = this.SsApplication;
                    DateTime resu = new DateTime(1370, 1, 1);
                    if (application != "")
                    {
                        try
                        {
                            if (Session["ss_workdate"] != null)
                            {
                                DateTime workDate = Convert.ToDateTime(Session["ss_workdate"]);
                                resu = workDate;
                            }
                            else throw new Exception();
                        }
                        catch
                        {
                            DataTable dt = WsUtil.Common.GetDataTable(this.SsWsPass, "select workdate from amappstatus where application='" + application + "'", "SDT");
                            if (dt.Rows.Count > 0)
                            {
                                resu = Convert.ToDateTime(dt.Rows[0][0]);
                                Session["ss_workdate"] = resu;
                            }
                        }
                    }
                    return resu;
                }
                catch { return new DateTime(1370, 1, 1); }
            }
            set { Session["ss_workdate"] = value; }
        }

        /// <summary>
        /// get, set : สาขาของสหกรณ์ Default=001
        /// </summary>
        public String SsBranchId
        {
            get { try { return Session["ss_branchid"].ToString(); } catch { return ""; } }
            set { Session["ss_branchid"] = value; }
        }

        /// <summary>
        /// รหัสประเภทศูนย์บริการ(รหัสสหกรณ์)
        /// </summary>
        public String SsBranchType
        {
            get { try { return Session["ss_branchtype"].ToString(); } catch { return ""; } }
            set { Session["ss_branchtype"] = value; }
        }

        /// <summary>
        /// รหัสจังหวัดศูนย์บริการ(รหัสสหกรณ์)
        /// </summary>
        public String SsProvinceCode
        {
            get { try { return Session["ss_coopprovincecode"].ToString(); } catch { return ""; } }
            set { Session["ss_coopprovincecode"] = value; }
        }

        /// <summary>
        /// รหัสอำเภอศูนย์บริการ(รหัสสหกรณ์)
        /// </summary>
        public String SsDistrictCode
        {
            get { try { return Session["ss_coopdistrictcode"].ToString(); } catch { return ""; } }
            set { Session["ss_coopdistrictcode"] = value; }
        }

        /// <summary>
        /// รหัสไปรษณีย์ศูนย์บริการ(รหัสสหกรณ์)
        /// </summary>
        public String SsPostCode
        {
            get { try { return Session["ss_cooppostcode"].ToString(); } catch { return ""; } }
            set { Session["ss_cooppostcode"] = value; }
        }

        public String SsPrinterSet
        {
            get
            {
                try
                {
                    return Session["ss_printerset"].ToString();
                }
                catch { return "0"; }
            }
            set
            {
                Session["ss_printerset"] = value;
            }
        }

        /// <summary>
        /// get, set: คืนค่า Url จาก Browser ที่เรียกเข้ามา เช่น http://localhost/GCOOP/Saving/
        /// </summary>
        public String SsUrl
        {
            get
            {
                try
                {
                    if (Request == null) throw new Exception("NoRequest");
                    string thisPage = Request.Url.AbsolutePath;
                    string prefix = this.SitePrefix;
                    if (thisPage.ToLower().IndexOf("gcoop/savingdelphi") > 0)
                    {
                        Session["ss_url"] = String.Format("http://{0}/{1}/GCOOP/SavingDelphi/", Request.Url.Authority, prefix);
                    }
                    else if (thisPage.ToLower().IndexOf("gcoop/saving") > 0)
                    {
                        Session["ss_url"] = String.Format("http://{0}/{1}/GCOOP/Saving/", Request.Url.Authority, prefix);
                    }
                    else
                    {
                        Session["ss_url"] = String.Format("http://{0}/{1}/GCOOP/Saving/", Request.Url.Authority, prefix);
                    }
                    return Session["ss_url"].ToString();
                    //currentPage = new System.IO.FileInfo(Request.Url.AbsolutePath).Name.Trim();
                    //reportLink = url + "ReportDefault.aspx";
                }
                catch (Exception ex)
                {
                    if (ex.Message == "NoRequest")
                    {
                        try
                        {
                            return Session["ss_url"].ToString();
                        }
                        catch { return "http://127.0.0.1/GCOOP/Saving/"; }
                    }
                    Session["ss_url"] = "http://127.0.0.1/GCOOP/Saving/";
                    return "http://127.0.0.1/GCOOP/Saving/";
                }
            }
            set { Session["ss_url"] = value; }
        }

        public String SsWsHost
        {
            get { return ConfigurationManager.AppSettings["wshost_00"].ToString(); }
        }

        public String SsWsReport
        {
            get { return ConfigurationManager.AppSettings["wsreport_00"].ToString(); }
        }

        public String SsLoginIP
        {
            get
            {
                try
                {
                    return Request.UserHostAddress;
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
        private HttpRequest Request;

        /// <summary>
        /// get : บอกว่าหน้าจอปัจจุบันชื่ออะไร (เช่น Home.aspx)
        /// </summary>
        public String CurrentPage
        {
            get
            {
                try
                {
                    return new System.IO.FileInfo(Request.Url.AbsolutePath).Name.Trim();
                }
                catch { return ""; }
            }
        }

        private String currentPageName;
        /// <summary>
        /// get : บอกชื่อหน้าจอปัจจุบันภาษาไทย ที่ได้จาก Table AMSECWINS ใช้สำหรับ Tag Title 
        /// (หากไม่มีข้อมูล Default คือ GCOOP - Isocare)
        /// </summary>
        public String CurrentPageName
        {
            get { return currentPageName; }
        }

        /// <summary>
        /// get : บอก URL สำหรับ Root directory เช่น http://localhost:8080/GCOOP/Saving/
        /// </summary>
        public String Url
        {
            get { return SsUrl; }
        }

        /// <summary>
        /// ReadOnly : บอกว่าโปรแกรมอะไร จาก บ.อะไร (default = GCOOP - Isocare)
        /// </summary>
        public String AppCompany
        {
            get { return "ELECTRICITY GENERATING AUTHORITY OF THAILAND."; }
        }

        /// <summary>
        /// ReadOnly : Title ของ หน้า Page
        /// </summary>
        public String AppTitle
        {
            get
            {
                String appTitle;
                if (currentPageName == "")
                {
                    if (this.SsMenuGroupDesc == null || this.SsMenuGroupDesc == "")
                    {
                        appTitle = this.AppCompany + " - " + SsThaiApplication;
                    }
                    else
                    {
                        appTitle = "[ " + SsMenuGroupDesc + " ] - " + this.AppCompany + " - " + SsThaiApplication;
                    }
                }
                else
                {
                    appTitle = "[ " + currentPageName + " ]" + " - " + SsThaiApplication + " - " + this.AppCompany;
                }
                return appTitle;
            }
        }

        //------ STATUS
        /// <summary>
        /// get : บอกว่าอยู่ในสถานะ Login แล้วหรือไม่
        /// </summary>
        public Boolean IsLogin
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(SsUsername);
                }
                catch
                {
                    return false;
                }
            }
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

        public String SiteLogo
        {
            get { return "img/band_black.jpg"; }
        }

        public String SiteTName
        {
            get
            {
                return this.SsCoopName;
            }
        }

        public String SiteEName
        {
            get { return ConfigurationManager.AppSettings["siteEName"].ToString(); }
        }

        public String SitePrefix
        {
            get { return ConfigurationManager.AppSettings["sitePrefix"].ToString(); }
        }

        public String SitePrefixDB
        {
            get { return ConfigurationManager.AppSettings["sitePrefixDB"].ToString(); }
        }

        public String SiteLinkName
        {
            get { return ConfigurationManager.AppSettings["siteLinkName"].ToString(); }
        }

        /// <summary>
        /// get, set: คืนค่า Url Report จาก Browser ที่เรียกเข้ามา เช่น http://localhost/GCOOP/Saving/ReportDefault.aspx
        /// </summary>
        public String ReportLink
        {
            get
            {
                return SsUrl + "ReportDefault.aspx";
            }
        }

        public WebState()
        {
            try
            {
                Page page = new System.Web.UI.Page();
                this.Session = page.Session;
                this.Request = null;
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
            ConstuctorEnding(Session, Request, null);
        }

        public WebState(HttpSessionState Session, HttpRequest Request, HttpApplicationState ApplicationState)
        {
            this.ApplicationState = ApplicationState;
            ConstuctorEnding(Session, Request, ApplicationState);
            //new AdminWebState(this);
        }

        public void ConstuctorEnding(HttpSessionState Session, HttpRequest Request, HttpApplicationState ApplicationState)
        {
            xmlConfig = new XmlConfigService();
            this.Session = Session;
            this.Request = Request;
            this.isReadable = false;
            this.isWritable = false;
            this.isLocalIp = true;
            this.SsLastClick = DateTime.Now;
            currentPageName = "";

            if (this.CurrentPage == "w_sheet_ex_memberdetail.aspx")
            {
                this.Session["ss_wspass"] = "ma7Efq7aeQOo/5F0a50th2MP1KEjtVuySyUcsCgLPTkxa0onak+KrmqAtEqirwftbFOWCb+slq8luUzzvQM9+jLT/AVhRYk7nKvaD20vpud4rC0jeYqMyE5divZ8kLp/ey5WNtKy9ag=";
                Login("guest", "guest", "walfare", "0210");
            }

            //ตั้งค่า MenuSub
            try
            {
                if (!string.IsNullOrEmpty(SsApplication))
                {
                    SetApplicationDetail(SsApplication);
                    RefreshMenu(SsApplication);
                    SetMenuSub();
                }
            }
            catch { }
            try
            {
                String csType = this.SsCsType;
                String csDesc = this.SsCsDesc;
            }
            catch { }
            if (this.IsLogin)
            {
                Session.Timeout = 20;
            }
            try
            {
                if (Request["exitCommand"] == "Home")
                {
                    this.SsApplication = "";
                }
            }
            catch { }
        }

        /// <summary>
        /// สำหรับ Login เข้าสู่ระบบโดยก่อนจะ Login ต้องเลือก Application ก่อน
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>URL สำหรับให้หน้า Login.aspx จะ Redirect ไปหน้าไหนตามแต่ระบบ</returns>
        public String Login(String username, String password, String application, String branchid, String printerSet)
        {
            Common comm = null;
            try
            {
                comm = WsUtil.Common;
            }
            catch { }
            String[] rs = comm.VerifyLogin(SsWsPass, username, password, application, branchid);
            String redir = "";

            if (rs[0].ToString() == "true")// Login ผ่าน
            {
                Session["ss_username"] = username.Trim();//Add username ลง Session ครั้งแรก
                Session["ss_branchid"] = branchid;//Add branchid ลง Session ครั้งแรก
                Session["ss_printerset"] = printerSet;
                String sqlCoopBranch = "select * from cmucfcoopbranch where coopbranch_id='" + branchid + "'";
                //TEST001 - 001
                Sdt dt = WebUtil.QuerySdt(sqlCoopBranch);
                if (dt.Next())
                {
                    this.SsBranchType = dt.GetString("coopbranch_type").Trim();
                    this.SsProvinceCode = dt.GetString("province_code").Trim();
                    this.SsDistrictCode = dt.GetString("district_code").Trim();
                    this.SsPostCode = dt.GetString("post_code").Trim();
                }
                try { Session["ss_userlevel"] = Convert.ToInt32(rs[1].ToString()); }
                catch { Session["ss_userlevel"] = 3; }

                try { Session["ss_usertype"] = Convert.ToInt32(rs[2].ToString()); }
                catch { Session["ss_usertype"] = 2; }
                //isLogin = true;
                SetApplicationDetail(application);
                RefreshMenu(application);// ทำการ สร้าง หรือ refresh Menu
                redir = SsUrl + "Default.aspx";
                LogAct(SsUsername, "login", "เข้าระบบสำเร็จ", SsApplication, "LOGIN");
            }
            else
            {
                LogAct(username, "login", "พยายามเข้าระบบ", SsApplication, "LOGIN");
                throw new Exception("Can not verify Username or Password.");
            }
            return redir;
        }

        public String Login(String username, String password, String application, String branchid)
        {
            Common comm = null;
            try
            {
                comm = WsUtil.Common;
            }
            catch { }
            String[] rs = comm.VerifyLogin(SsWsPass, username, password, application, branchid);
            String redir = "";
            if (rs[0].ToString() == "true")// Login ผ่าน 
            {
                String sqlCoopBranch = "select * from cmucfcoopbranch where coopbranch_id='" + branchid + "'";
                //TEST001 - 001
                Sdt dt = WebUtil.QuerySdt(sqlCoopBranch);
                if (dt.Next())
                {
                    this.SsBranchType = dt.GetString("coopbranch_type").Trim();
                    this.SsProvinceCode = dt.GetString("province_code").Trim();
                    this.SsDistrictCode = dt.GetString("district_code").Trim();
                    this.SsPostCode = dt.GetString("post_code").Trim();
                }
                try { Session["ss_userlevel"] = Convert.ToInt32(rs[1].ToString()); }
                catch { Session["ss_userlevel"] = 3; }

                try { Session["ss_usertype"] = Convert.ToInt32(rs[2].ToString()); }
                catch { Session["ss_usertype"] = 2; }

                this.SsUsername = username;//Add username ลง Session ครั้งแรก
                this.SsBranchId = branchid;//Add branchid ลง Session ครั้งแรก
                try
                {
                    this.SsUserLevel = Convert.ToInt32(rs[1].ToString());
                }
                catch
                {
                    this.SsUserLevel = 3;
                }
                SetApplicationDetail(application);
                RefreshMenu(application);//ทำการ สร้าง หรือ refresh Menu 
                redir = SsUrl + "Default.aspx";
                this.SsPrinterSet = comm.GetDefaultPrinterFormSets(this.SsWsPass, this.SsUsername);
                LogAct(SsUsername, "login", "เข้าระบบสำเร็จ", SsApplication, "LOGIN");
                redir = "true";
            }
            else
            {
                LogAct(username, "login", "พยายามเข้าระบบ", SsApplication, "LOGIN");
                throw new Exception("เข้าสู่ระบบไม่สำเร็จ กรุณาตรวจสอบ ศูนย์ประสานงาน, ชื่อผู้ใช้, รหัสผ่าน");
            }
            return redir;
        }

        public Int32 LogAct(String userID, String actionID, String actionDesc, String application, String windowID)
        {
            //try
            //{
            //    n_cst_adminservice svAdmin;
            //    n_cst_dbconnectservice svCon;
            //    svCon = new n_cst_dbconnectservice();
            //    svCon.of_connectdb(this.SsConnectionString);
            //    svAdmin = new n_cst_adminservice();
            //    svAdmin.of_settrans(ref svCon);
            //    String errtext = "";
            //    Int32 rv = svAdmin.of_registeract(userID, actionID, actionDesc, application, windowID, SsClientIp, ref errtext);
            //    svCon.of_disconnectdb();
            //    return rv;
            //}
            //catch { return -1; }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Logout()
        {
            LogAct(SsUsername, "logout", "ออกจากระบบ", SsApplication, "LOGOUT");
            Session.Abandon();
            Session.RemoveAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public String RefreshMenu(String application)
        {
            String redir = "";
            Common comm = null;

            try { comm = WsUtil.Common; }
            catch { return ""; }

            if (this.IsLogin)
            {
                this.SsApplication = application;
                this.SsPagePermiss = comm.GetPermissMenu(SsWsPass, SsUsername, application);
                if (string.IsNullOrEmpty(this.SsMenuGroup))
                {
                    try
                    {
                        this.SsMenuGroup = this.SsPagePermiss.Rows[0]["GROUP_CODE"].ToString().Trim();
                        this.SsMenuGroupDesc = this.SsPagePermiss.Rows[0]["GROUP_DESC"].ToString().Trim();
                    }
                    catch
                    {
                        this.SsMenuGroup = null;
                        this.SsMenuGroupDesc = null;
                    }
                }
            }
            return "";
        }

        private void SetMenuSub()
        {
            try
            {
                this.SsMenuGroup = Request["gcode"].Trim();
            }
            catch
            {
                if (this.SsMenuGroup != null) { }
                else
                {
                    this.SsMenuGroup = null;
                }
            }
            this.SsMenuGroupDesc = null;
            string currentPage = this.CurrentPage.ToLower().Trim();
            for (int i = 0; i < SsPagePermiss.Rows.Count; i++)
            {
                if (this.SsPagePermiss.Rows[i]["WIN_OBJECT"].ToString().Trim().ToLower() + ".aspx" == currentPage)
                {
                    this.currentPageName = SsPagePermiss.Rows[i]["WIN_DESCRIPTION"].ToString().Trim();
                    this.SsMenuGroup = SsPagePermiss.Rows[i]["GROUP_CODE"].ToString().Trim();
                    this.SsMenuGroupDesc = SsPagePermiss.Rows[i]["GROUP_DESC"].ToString().Trim();
                    this.isReadable = true;
                    try
                    {
                        this.isWritable = Convert.ToInt32(this.SsPagePermiss.Rows[i]["SAVE_STATUS"]) == 1;
                    }
                    catch { }
                    break;
                }
            }
            //Tast TabPage
            if (CurrentPage.ToLower().Trim().Equals("tabpage.aspx"))
            {
                currentPageName = "เทส Tab";
                this.SsMenuGroup = "A";
                this.SsMenuGroupDesc = "สมาชิก";
                isWritable = true;

            }
            if (!string.IsNullOrEmpty(SsMenuGroupDesc) && !string.IsNullOrEmpty(SsMenuGroup))
            {
                for (int i = 0; i < SsPagePermiss.Rows.Count; i++)
                {
                    if (SsPagePermiss.Rows[i]["GROUP_CODE"].ToString().Trim() == SsMenuGroup)
                    {
                        this.SsMenuGroupDesc = SsPagePermiss.Rows[i]["GROUP_DESC"].ToString();
                        break;
                    }
                }
            }
        }

        public void SetApplicationDetail(String app)
        {
            String sql = "";
            String sitePrefix = System.Configuration.ConfigurationManager.AppSettings["sitePrefix"].ToString();
            DataTable dt;
            try
            {
                sql = "select workdate, closeday_status, closemonth_status, closeyear_status from amappstatus where application='" + app + "'";
                dt = WebUtil.Query(sql);
                try
                {
                    this.SsCloseDayStatus = dt.Rows[0]["closeday_status"].ToString().Trim();
                }
                catch
                {
                    this.SsCloseDayStatus = "0";
                }
                try
                {
                    this.SsCloseMonthStatus = dt.Rows[0]["closemonth_status"].ToString().Trim();
                }
                catch
                {
                    this.SsCloseMonthStatus = "0";
                }
                try
                {
                    this.SsCloseYearStatus = dt.Rows[0]["closeyear_status"].ToString().Trim();
                }
                catch
                {
                    this.SsCloseYearStatus = "0";
                }

                dt.Dispose();

                sql = "select coop_no,coop_name from cmcoopconstant where site='" + this.SitePrefixDB + "'"; //default สหกรณ์ 000 จาก DB
                dt = WebUtil.Query(sql);
                try
                {
                    this.SsCoopNo = dt.Rows[0]["coop_no"].ToString().Trim();
                }
                catch
                {
                    this.SsCoopNo = "000";
                }
                try
                {
                    this.SsCoopName = dt.Rows[0]["coop_name"].ToString().Trim();
                }
                catch
                {
                    this.SsCoopName = "ไม่มีการกำหนดชื่อสหกรณ์";
                }
                this.SsThaiApplication = WsUtil.Common.GetApplicationThai(this.SsWsPass, app);
            }
            catch (Exception ex) { throw ex; }
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
                String ipFormat = "192.168.99.xxx";// System.Configuration.ConfigurationManager.AppSettings["ipAddressFormat"].ToString().Replace("X", "");
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

        public void ResetState()
        {
        }

        public HttpSessionState GetSessionState()
        {
            return Session;
        }

        public HttpApplicationState GetApplicationState()
        {
            return ApplicationState;
        }

        public String GetConnectionElement(String elementName)
        {
            String result = "";
            try
            {
                String connectionString = this.SsConnectionString;
                String[] conArray = connectionString.Split(';');
                for (int i = 0; i < conArray.Length; i++)
                {
                    if (conArray[i].IndexOf(elementName) == 0)
                    {
                        String[] ar2 = conArray[i].Split('=');
                        result = ar2[1].Trim();
                        break;
                    }
                }
            }
            catch { }
            return result;
        }
    }
}