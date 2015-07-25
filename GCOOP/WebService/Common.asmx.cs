using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Configuration;
using pbservice;
using DBAccess;
using System.Globalization;
using Sybase.DataWindow;
using System.IO;
using WebService.Processing;
using System.Threading;
using Microsoft.Win32;
using GcoopServiceCs;

namespace WebService
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Common : System.Web.Services.WebService
    {
        [WebMethod]
        public String GetPasswordService()
        {
            return new Security("", false).EncryptPassword;
        }

        [WebMethod]
        public String GetConnectionString(String wsPass)
        {
            return new Security(wsPass).EncryptConnectionString;
        }

        [WebMethod]
        public String GetConstantValue(String wsPass, String param)
        {
            String var = "";
            try
            {
                n_cst_xmlconfig xmlConfig = new n_cst_xmlconfig();
                var = xmlConfig.of_getconstantvalue(param);
            }
            catch { }
            return var;
        }

        [WebMethod]
        public DataTable GetDataTable(String wsPass, String keyWord, String tName)
        {
            return new CommonSvEn(wsPass, false).GetDataTable(keyWord, tName);
        }

        [WebMethod]
        public String GetNewDocNo(String wsPass, String docCode)
        {
            CommonSvEn cm = new CommonSvEn(wsPass);
            return cm.GetNewDocNo(docCode);
        }

        [WebMethod]
        public String GetNewDocumentNo(String wsPass, String docCode)
        {
            Security sec = new Security(wsPass);
            String docNew = new GcoopServiceCs.DocumentControl().NewDocumentNo(GcoopServiceCs.DocumentTypeCode.INSAPPLDOCNO, 2554, sec.ConnectionString);
            return docNew;
        }

        [WebMethod]
        public String GetStatusApplication(String wsPass)
        {
            Security sec = new Security(wsPass);

            String outputMessage = "";
            try
            {
                //String appList = "shrlon,ap_deposit,app_finance,account,keeping,app_assist,hr,cmd,mis";
                String connString = sec.ConnectionString;


                Sta ta = new Sta(connString);
                //for (int i = 0; i < appList.Split(',').Length; i++)
                //{
                //String sql = "select description,closeday_status,workdate,application from amappstatus where application='" + appList.Split(',').GetValue(i) + "'";
                String sql = "select description,closeday_status,workdate,application from amappstatus where used_flag = 1 order by menu_order asc";
                Sdt dt = ta.Query(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime wdate = new DateTime();
                    wdate = Convert.ToDateTime(dt.Rows[i]["workdate"]);
                    outputMessage += (i + 1) + "," + dt.Rows[i]["application"].ToString().Trim() + "," + dt.Rows[i]["description"].ToString().Trim() + "," + dt.Rows[i]["closeday_status"].ToString().Trim() + "," + wdate.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) + "|";
                }
                //}
                ta.Close();
            }
            catch (Exception exc) { throw exc; }

            return outputMessage;
        }

        [WebMethod]
        public DataTable GetStatusApplicationData(String wsPass)
        {
            String connString = "";
            if (String.IsNullOrEmpty(wsPass))
            {
                Security sec = new Security("", false);
                connString = sec.GetPrivateConStr();
            }
            else
            {
                Security sec = new Security(wsPass);
                connString = sec.ConnectionString;
            }
            Sta ta = new Sta(connString);
            //String sql = "select description, closeday_status, workdate, application from amappstatus where used_flag = 1 order by menu_order asc";
            String sql = @"
                select 
	                cs_type,
	                'walfare' as application,
	                cs_name as description,
	                0 as closeday_status,
	                to_date('" + DateTime.Today.ToString("yyyy-MM-dd", new CultureInfo("en-US")) + @"', 'yyyy-mm-dd') as workdate
                from cmucfcremation order by cs_type";
            DataTable dt = ta.QueryDataTable(sql);
            dt.TableName = "AppStatus";
            return dt;
        }

        [WebMethod]
        public String GetApplicationThai(String wsPass, String app)
        {
            Security sec = new Security(wsPass, false);
            try
            {
                String connString = sec.ConnectionString;
                Sta ta = new Sta(connString);
                String sql = "select description from amappstatus where application='" + app + "'";
                Sdt dt = ta.Query(sql);
                ta.Close();
                return dt.Rows[0]["description"].ToString(); ;
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        [WebMethod]
        public String BaseFormatMemberNo(String wsPass, String memberNo)
        {
            new Security(wsPass, false);
            try
            {
                return int.Parse(memberNo).ToString("000000");

            }
            catch { return memberNo; }
        }

        [WebMethod]
        public DataTable GetBranchId(String wsPass, String csType)
        {
            Security sec = new Security(wsPass);
            //String connectionString = GetConnectionString(wsPass);
            String connectionString = sec.ConnectionString;
            Sta ta = new Sta(connectionString);
            String sql = @"SELECT CMUCFCOOPBRANCH.COOPBRANCH_ID,   
                             CMUCFCOOPBRANCH.COOPBRANCH_DESC,
                             CMUCFCOOPBRANCH.COOPBRANCH_ID || ' - ' || CMUCFCOOPBRANCH.COOPBRANCH_DESC as coopbranch_iddesc
                            FROM CMUCFCOOPBRANCH where cs_type='" + csType + "' order by COOPBRANCH_DESC asc";


            DataTable dt = ta.QueryDataTable(sql);
            dt.TableName = "COOPBRANCH";

            ta.Close();
            return dt;
        }

        [WebMethod]
        public String[] VerifyLogin(String wsPass, String username, String password, String application, String branchId)
        {
            Security sec = new Security(wsPass);

            String[] result = new String[3];
            Sta ta = new Sta(sec.ConnectionString);
            String sql = @"SELECT AMSECUSERS.USER_NAME, AMSECUSERS.PASSWORD, AMSECUSEAPPS.APPLICATION,AMSECUSERS.USER_LEVEL,AMSECUSERS.USER_TYPE
                           FROM AMSECUSERS, AMSECUSEAPPS
                           WHERE AMSECUSERS.USER_NAME = AMSECUSEAPPS.USER_NAME AND 
                           (AMSECUSERS.USER_NAME = '" + username.Trim() + @"') AND (AMSECUSEAPPS.APPLICATION = '" + application + @"') 
                           AND (AMSECUSERS.PASSWORD = '" + password.Trim() + "') AND (AMSECUSERS.COOPBRANCH_ID='" + branchId + "' OR AMSECUSERS.USER_TYPE=1)";
            Sdt dt = ta.Query(sql);
            ta.Close();
            if (dt.Rows.Count > 0) //Login ผ่าน
            {
                result[0] = "true";
                result[1] = dt.Rows[0]["USER_LEVEL"].ToString();
                result[2] = dt.Rows[0]["USER_TYPE"].ToString();
            }
            else
            {
                result[0] = "false";
                result[1] = "";
                result[2] = "";
            }
            return result;
        }

        [WebMethod]
        public DataTable GetPermissMenu(String wsPass, String username, String application)
        {
            //if (!new Security().CheckPassword(wsPass)) { throw new Exception("Webservice Password ไม่ถูกต้อง"); }
            Security security = new Security(wsPass);
            String sql = @"SELECT AMSECPERMISS.USER_NAME,   
                             AMSECPERMISS.APPLICATION,   
                             AMSECPERMISS.WINDOW_ID,   
                             AMSECPERMISS.SAVE_STATUS,   
                             AMSECWINS.WIN_OBJECT,   
                             AMSECWINS.WIN_DESCRIPTION,   
                             AMSECWINS.WIN_PARAMETER,   
                             AMSECWINS.WIN_TITLE,   
                             AMSECWINS.WIN_TOOLBAR,   
                             AMSECWINS.OPEN_TYPE,   
                             AMSECWINS.ICON_LABEL,   
                             AMSECWINS.ICON_PICTURE,   
                             AMSECWINS.GROUP_CODE,   
                             AMSECWINS.WIN_ORDER,   
                             AMSECWINSGROUP.GROUP_DESC,   
                             AMSECWINSGROUP.GROUP_ORDER  
                                FROM AMSECPERMISS,   
                             AMSECWINS,   
                             AMSECWINSGROUP  
                                WHERE ( AMSECWINS.APPLICATION = AMSECPERMISS.APPLICATION ) and  
                             ( AMSECWINS.WINDOW_ID = AMSECPERMISS.WINDOW_ID ) and  
                             ( AMSECWINS.APPLICATION = AMSECWINSGROUP.APPLICATION ) and  
                             ( trim(AMSECWINS.GROUP_CODE) = trim(AMSECWINSGROUP.GROUP_CODE) ) and  
                             ( ( amsecpermiss.user_name = '" + username + @"' ) AND  
                             ( amsecpermiss.application = '" + application + @"' ) AND  
                             ( amsecwins.used_flag = 1 ) AND  
                             ( amsecpermiss.check_flag in ( 1, 2 ) ) ) order by AMSECWINSGROUP.GROUP_ORDER, AMSECWINS.GROUP_CODE,AMSECWINS.WIN_ORDER asc";

            Sta ta = new Sta(security.ConnectionString);
            DataTable dt = ta.QueryDataTable(sql);
            dt.TableName = "AMSECALL";
            ta.Close();

            return dt;
        }

        [WebMethod]
        public String GetDDDWXml(String wsPass, String ddwobj)
        {
            CommonSvEn cm = new CommonSvEn(wsPass);
            return cm.GetDDDWXml(ddwobj);
        }

        [WebMethod]
        public String GetPrinterFormSets(String wsPass)
        {
            CommonSvEn cm = new CommonSvEn(wsPass, false);
            return cm.GetPrinterFormSets();
        }

        [WebMethod]
        public DataTable GetPrinterFormSetsData(String wsPass)
        {
            Security sec = new Security(wsPass, false);
            DataSet ds = new DataSet();
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string physicalPath = HttpContext.Current.Request.MapPath(appPath);
            string xmlConfigPath = sec.PhysicalPath + "XMLConfig\\print.formsets.xml";

            ds.ReadXml(xmlConfigPath);
            DataTable dt = ds.Tables[0];
            dt.TableName = "formsets";
            return dt;
        }

        [WebMethod]
        public String GetDefaultPrinterFormSets(String wsPass, String userName)
        {
            return new CommonSvEn(wsPass, false).GetDefaultPrinterFormSets(userName);
        }

        [WebMethod]
        public String GetDefaultPrinterFormSetsByIP(String wsPass, String comIp)
        {
            return new CommonSvEn(wsPass, false).GetDefaultPrinterFormSetsByIP(comIp);
        }

        [WebMethod]
        public String GetXmlDataStore(String wsPass, String application, String libralyList, String dwobjectName, params object[] ArgsList)
        {
            Security sec = new Security(wsPass);
            CommonSvEn cmds = new CommonSvEn(wsPass, false);
            String fullLibralyList = "";// cmds.GetConstantValue("common.saving_dw_path") + application + "\\" + libralyList;
            fullLibralyList = sec.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + libralyList;
            try
            {
                FileInfo fInfo = new FileInfo(fullLibralyList);
                if (!fInfo.Exists) throw new Exception();
            }
            catch
            {
                fullLibralyList = cmds.GetConstantValue("common.saving_dw_pathdev") + application + "\\" + libralyList;
            }


            DwTrans sqlca = new DwTrans(sec.ConnectionString);
            try
            {
                sqlca.Connect();
                DataStore dts = new DataStore(fullLibralyList, dwobjectName);
                dts.SetTransaction(sqlca);
                dts.Retrieve(ArgsList);
                if (dts.RowCount < 1)
                {
                    throw new Exception("ERROR:NOROWXML");
                }
                sqlca.Disconnect();
                return dts.Describe("DataWindow.Data.XML");
            }
            catch (Exception ex)
            {
                try { sqlca.Disconnect(); }
                catch { }
                throw ex;
            }
        }

        [WebMethod]
        public String GetTextDataStore(String wsPass, String application, String libralyList, String dwobjectName, params object[] ArgsList)
        {
            Security sec = new Security(wsPass);
            CommonSvEn cmds = new CommonSvEn(wsPass, false);
            String fullLibralyList = "";// cmds.GetConstantValue("common.saving_dw_path") + application + "\\" + libralyList;
            fullLibralyList = sec.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + libralyList;
            try
            {
                FileInfo fInfo = new FileInfo(fullLibralyList);
                if (!fInfo.Exists) throw new Exception();
            }
            catch
            {
                fullLibralyList = cmds.GetConstantValue("common.saving_dw_pathdev") + application + "\\" + libralyList;
            }
            DwTrans sqlca = new DwTrans(sec.ConnectionString);
            try
            {
                sqlca.Connect();
                DataStore dts = new DataStore(fullLibralyList, dwobjectName);
                dts.SetTransaction(sqlca);
                dts.Retrieve(ArgsList);
                sqlca.Disconnect();
                return dts.Describe("DataWindow.Data");
            }
            catch (Exception ex)
            {
                try { sqlca.Disconnect(); }
                catch { }
                throw ex;
            }
        }

        [WebMethod]
        public String GetProcessStatus(String wspass, String application, String w_sheet_id)
        {
            Security sec = new Security(wspass);
            string[] s = Progressing.GetStatus(application, w_sheet_id);

            string ss = "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} ";
            String result = String.Format(ss, s);
            try
            {
                if (s[0] == "1" || s[0] == "-1")
                {
                    Progressing.Remove(application, w_sheet_id);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return result;
        }

        [WebMethod]
        public String RemoveProcess(String wsPass, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            Progressing.Remove(application, w_sheet_id);
            return "Remove successfully";
        }

        [WebMethod]
        public String ReadThaiBath(String wsPass, decimal number)
        {
            return new CommonSvEn(wsPass).ReadThaiBath(number);
        }

        [WebMethod]
        public DateTime LastDayOfmonth(String wsPass, DateTime date)
        {
            return new CommonSvEn(wsPass).LastDayOfmonth(date);
        }

        [WebMethod]
        public String zPhysicalPathRegistry(String wsPass)
        {
            Security sec = new Security(wsPass);
            try
            {
                return Registry.LocalMachine.OpenSubKey("SOFTWARE\\GCOOP\\PhyPath").GetValue("path").ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public String zPhysicalPathXML(String wsPass)
        {
            Security sec = new Security(wsPass);
            try
            {
                return sec.WinPrintIP;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public int InsertDataWindow(String wsPass, String xmlData, String pblName, String table, String dwObjectName, String application, int[] rows)
        {
            Security sec = new Security(wsPass);
            DwHandle dwH = new DwHandle(xmlData, sec.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + pblName, dwObjectName);
            return dwH.InsertData(sec.ConnectionString, table, rows);
        }

        [WebMethod]
        public int UpdateDataWindow(String wsPass, String xmlData, String pblName, String table, String dwObjectName, String application, int[] rows)
        {
            Security sec = new Security(wsPass);
            DwHandle dwH = new DwHandle(xmlData, sec.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + pblName, dwObjectName);
            return dwH.UpdateData(sec.ConnectionString, table, rows);
        }

        
    }
}