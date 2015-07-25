using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Data;
using Sybase.DataWindow.Web;
using Sybase.DataWindow;

namespace Saving.CmConfig
{
    public class WebUtil
    {
        public static CultureInfo TH
        {
            get
            {
                return new CultureInfo("th-TH");
            }
        }

        public static CultureInfo EN
        {
            get
            {
                return new CultureInfo("en-US");
            }
        }

        public static String DwDateTimeFormat
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["DwDateTimeFormat"].ToString(); }
        }

        public static String DwDateTimeCulture
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["DwDateTimeCulture"].ToString(); }
        }

        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            int temp = param.Length - length;
            string result = param.Substring(temp, length);
            //return the result of the operation
            return result;
        }

        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        public static string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = param.Substring(startIndex);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// สร้าง JavaScript Postback
        /// </summary>
        /// <param name="page"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String JsPostBack(System.Web.UI.Page page, String name)
        {
            return "<script>function " + name + "(){" + page.ClientScript.GetPostBackEventReference(page, name) + "}</script>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissType"></param>
        /// <returns></returns>
        public static String PermissionDeny(PermissType permissType)
        {
            if (permissType == PermissType.ReadDeny)
            {
                return "<font color=\"red\">คุณยังไม่ได้รับสิทธิ์การใช้งานหน้านี้</font>";
            }
            else if (permissType == PermissType.WriteDeny)
            {
                return "<font color=\"red\">คุณยังไม่ได้รับสิทธิ์การการบันทึกข้อมูลหน้านี้</font>";
            }
            else if (permissType == PermissType.LoginDeny)
            {
                return "<font color=\"red\">คุณยังไม่ได้ทำการ Login</font>";
            }
            return "";
        }

        public static String ErrorMessage(String message)
        {
            return "<div align=\"center\"><table><tr><td valign=\"top\"><img style\"text-align:center;\" src=\"" + new WebState().SsUrl + "img/error.gif\" /></td><td align=\"left\"><font color=\"red\">" + message + "</font></td></tr></table></div>";
        }

        public static String ErrorMessage(Exception ex)
        {
            if (ex.GetType().FullName == "System.Web.Services.Protocols.SoapException")
            {
                return WebUtil.ErrorMessage(WebUtil.SoapMessage((SoapException)ex));
            }
            else
            {
                return WebUtil.ErrorMessage(ex.Message);
            }
        }

        public static String CompleteMessage(String message)
        {
            return "<div align=\"center\"><table><tr><td valign=\"top\"><img style\"text-align:center;\" src=\"" + new WebState().SsUrl + "img/complete.gif\" /></td><td align=\"left\"><font color=\"green\">" + message + "</font></td></tr></table></div>";
        }

        public static String WarningMessage(String message)
        {
            return "<div align=\"center\"><table><tr><td valign=\"top\"><img style\"text-align:center;\" src=\"" + new WebState().SsUrl + "img/warning.gif\" /></td><td align=\"left\"><font color=\"orange\">" + message + "</font></td></tr></table></div>";
        }

        public static String WarningMessage(Exception ex)
        {
            if (ex.GetType().FullName == "System.Web.Services.Protocols.SoapException")
            {
                return WebUtil.WarningMessage(WebUtil.SoapMessage((SoapException)ex));
            }
            else
            {
                return WebUtil.WarningMessage(ex.Message);
            }
        }

        public static String StringFormat(String str, String formats)
        {
            String formatted = "";
            int tempStr = 0;
            try
            {
                tempStr = Convert.ToInt32(str);
            }
            catch { }


            formatted = tempStr.ToString(formats);

            return formatted;
        }

        public static String SoapMessage(SoapException ex)
        {
            try
            {
                String prefix = "[SW]";
                String message = "";
                String exMessage = ex.Message;
                String pbException = "Sybase.PowerBuilder.PBThrowableE: ";
                String soapException = "System.Web.Services.Protocols.SoapException: ";
                String sysException = "System.Exception: ";
                String at = "\n   at ";
                int indexOfStart = 0;
                int indexOfPbService = exMessage.IndexOf(pbException);
                int indexOfSystemEx = exMessage.IndexOf(sysException);
                if (indexOfPbService > 0)
                {
                    prefix = "[PB]";
                    indexOfStart = indexOfPbService + pbException.Length;
                }
                else if (indexOfSystemEx > 0)
                {
                    prefix = "[SY]";
                    indexOfStart = indexOfSystemEx + sysException.Length;
                }
                else
                {
                    indexOfStart = exMessage.IndexOf(soapException) + soapException.Length;
                }
                message = exMessage.Substring(indexOfStart);
                message = message.Substring(0, message.IndexOf(at));
                message = message.Replace("Server was unable to process request. --->", "");
                return prefix + message;
            }
            catch
            {

                return ex.Message.Replace("Server was unable to process request. --->", "");
            }
        }

        //ตอนนี้มีให้ใช้กับ Report Master (รอเปลี่ยนเป็น state.SsThaiApplicaion)
        public static String GetApplicationThai(String app)
        {
            String rs = "";
            if (app == "shrlon") rs = "หุ้นหนี้";
            else if (app == "ap_deposit") rs = "เงินฝาก";
            else if (app == "app_finance") rs = "การเงิน";
            else if (app == "app_assist") rs = "สวัสดิการ";
            else if (app == "account") rs = "บัญชี";
            else if (app == "keeping") rs = "จัดเก็บ";
            else if (app == "hr") rs = "บริหารงานบุคคล";
            else if (app == "mis") rs = "การลงทุน";
            else if (app == "app_assist") rs = "สวัสดิการ";
            else if (app == "cmd") rs = "พัสดุครุภัณฑ์";
            return rs;
        }
        /// <summary>
        /// แปลงค่า String ให้เป็น int โดยหากเจอ Exception จะ return 0 
        /// </summary>
        /// <param name="number">ตัวเลขในรูปแบบ String</param>
        /// <returns>คืนค่าเป็นตัวเลข หากเกิด Exception จะคืนค่า 0</returns>
        public static int ParseInt(String number)
        {
            try
            {
                return int.Parse(number);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// เช็คว่า String ที่ได้อยู่ในรูปแบบ XML หรือไม่
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool IsXML(String xml)
        {
            try
            {
                return xml.ToLower().IndexOf("<?xml") >= 0;
            }
            catch
            {
                return false;
            }
        }

        //กำหนดค่าของ UI
        public static String BGselectedTab() { return "rgb(211,213,255)"; }
        public static String BGmenuTab() { return "rgb(200,235,255)"; }
        public static String GenHeadTabMenu(String nameTab, int tabAmt, int selectedTab)
        {
            String tdWidth = (100 / tabAmt).ToString();
            String[] tabList = nameTab.Split(',');

            String output = "<table style=\"width: 100%; border: solid 1px; margin-top: 5px;\">" +
                "<tr align=\"center\" class=\"dwtab\">";

            for (int i = 0; i < tabList.Length; i++)
            {
                if (selectedTab == (i + 1))
                {
                    output += "<td style=\"background-color:" + BGselectedTab() + "; cursor: pointer;\" id=\"stab" + (i + 1) + "\" width=\"25%\"" +
                              "onclick=\"showTabPage(" + (i + 1) + ");\" width=\"" + tdWidth + "%\">" +
                              "" + tabList.GetValue(i) + "</td>";
                }
                else
                {
                    output += "<td style=\"background-color:" + BGmenuTab() + "; cursor: pointer;\" id=\"stab" + (i + 1) + "\" width=\"25%\"" +
                              "onclick=\"showTabPage(" + (i + 1) + ");\" width=\"" + tdWidth + "%\">" +
                              "" + tabList.GetValue(i) + "</td>";
                }
            }

            output += "</tr></table>";
            return output;
        }

        public static String GenJavaScriptTabPage(int tabAmt, int selectedTab)
        {
            String output = "<script type=\"text/javascript\">" +
                            "function showTabPage(tab){var i = 1;var tabamount = " + tabAmt + ";" +
                            "for (i = 1; i <= tabamount; i++) {" +
                //"document.getElementById(\"tab\" + i).style.visibility = \"hidden\";" +
                            "document.getElementById(\"stab\" + i).style.backgroundColor = \"" + BGmenuTab() + "\";" +
                            "if (i == tab) {" +
                //"document.getElementById(\"tab\" + i).style.visibility = \"visible\";" +
                            "document.getElementById(\"stab\" + i).style.backgroundColor = \"" + BGselectedTab() + "\";" +
                            "}}}" +
                            "</script>";

            return output;
        }

        public static String GetConnectionElement(String elementName)
        {
            String result = "";
            try
            {
                String connectionString = new System.Web.UI.Page().Session["ss_connectionstring"].ToString();
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

        public static String EncryptType(int type)
        {
            switch (type)
            {
                case 1: return ""; break;
                case 2: return ""; break;
                case 3: return ""; break;
                default: return ""; break;
            }
        }

        public static DataTable XmlToDataTable(String xml, String tbName)
        {
            System.IO.StringReader strReader = new System.IO.StringReader(xml);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.ReadXml(strReader);
            dt.TableName = tbName;
            return dt;
        }

        public static DataTable Query(String sql)
        {
            WebState state = new WebState();
            Saving.WsCommon.Common cm = new Saving.WsCommon.Common();
            System.Data.DataTable dt = cm.GetDataTable(state.SsWsPass, sql, "Sdt");
            return dt;
        }

        public static String ConvertDateThaiToEng(WebDataWindowControl dwObj, String thaiDateColumn, String thDate)
        {
            String thaiDate = "";
            if (thDate == null || thDate == "") { thaiDate = dwObj.GetItemString(1, thaiDateColumn); }
            else { thaiDate = thDate; }
            thaiDate = thaiDate.Replace("/", "");
            DateTime engdate = DateTime.ParseExact(thaiDate, "ddMMyyyy", WebUtil.EN);
            String str_date = engdate.Day.ToString("00");
            String str_month = engdate.Month.ToString("00");
            String str_year = (Convert.ToInt32(engdate.Year.ToString()) - 543).ToString("0000");
            thaiDate = str_month + "/" + str_date + "/" + str_year;
            return thaiDate;
        }

        public static void RetrieveDDDW(WebDataWindowControl dwObj, String columnName, String libraryList, params object[] args)
        {
            WsCommon.Common comSrv = new WsCommon.Common();
            WebState state = new WebState();

            String dwobjectName = dwObj.Describe(columnName + ".dddw.name");
            String strDS = comSrv.GetTextDataStore(state.SsWsPass, state.SsApplication, libraryList, dwobjectName, args);
            if (!string.IsNullOrEmpty(strDS))
            {
                DataWindowChild dwChild = dwObj.GetChild(columnName);
                dwChild.Reset();
                dwChild.ImportString(strDS, FileSaveAsType.Text);
            }
            comSrv.Dispose();
        }
    }
}