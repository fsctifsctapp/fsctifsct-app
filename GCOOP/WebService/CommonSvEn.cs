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
using pbservice;
using DBAccess;

namespace WebService
{
    public class CommonSvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_doccontrolservice svDoc;
        private n_cst_printservice printSrv;
        private n_cst_thailibservice svThaiLib;
        private n_cst_utility svUtil;
        private Security security;
        private n_cst_datetimeservice svDatetime;

        public CommonSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public CommonSvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                svCon = new n_cst_dbconnectservice();
                svCon.of_connectdb(security.ConnectionString);
                svDoc = new n_cst_doccontrolservice();
                svDoc.of_settrans(svCon);
                svUtil = new n_cst_utility();
                svUtil.of_settrans(svCon);
                svThaiLib = new n_cst_thailibservice();
                svDatetime = new n_cst_datetimeservice();
               
            }
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~CommonSvEn()
        {
            DisConnect();
        }

        public DataTable GetDataTable(String keyWord, String tName)
        {
            try
            {
                Sta ta = new Sta(security.ConnectionString);
                DataTable dt = null;
                try
                {
                    dt = ta.QueryDataTable(keyWord);
                    dt.TableName = tName;
                }
                catch (Exception ex)
                {
                    ta.Close();
                    throw ex;
                }
                ta.Close();
                DisConnect();
                return dt;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetNewDocNo(String docCode)
        {
            try
            {
                string doc = svDoc.of_getnewdocno(docCode, "000");
                this.DisConnect();
                return doc;
            }
            catch (Exception ex)
            {
                this.DisConnect();
                throw ex;
            }
        }

        public String GetDDDWXml(String ddwobj)
        {
            String strXml = "";
            try
            {
                strXml = svUtil.of_getdddwxml(ddwobj);
                this.DisConnect();
            }
            catch (Exception ex)
            {
                this.DisConnect();
                throw ex;
            }
            return strXml;
        }

        public String GetPrinterFormSets()
        {
            try
            {
                printSrv = new n_cst_printservice();
                printSrv.of_reloadsetting();
                DisConnect();
                return printSrv.of_getformsets();
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetDefaultPrinterFormSets(String userName)
        {
            try
            {
                printSrv = new n_cst_printservice();
                printSrv.of_reloadsetting();
                DisConnect();
                return printSrv.of_getdefaultformset(userName);
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetDefaultPrinterFormSetsByIP(String comIp)
        {
            try
            {
                printSrv = new n_cst_printservice();
                printSrv.of_reloadsetting();
                DisConnect();
                return printSrv.of_getdefaultformset_bycomid(comIp);
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetConstantValue(String code)
        {
            String result = "";
            try
            {
                n_cst_xmlconfig xmlConfig = new n_cst_xmlconfig();
                result = xmlConfig.of_getconstantvalue(code);
                DisConnect();
            }
            catch { DisConnect(); }
            return result;
        }

        public String ReadThaiBath(decimal number)
        {
            try
            {
                DisConnect();
                return svThaiLib.of_readthaibaht(number);
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public DateTime LastDayOfmonth(DateTime date)
        {
            try
            {
                DisConnect();
                return svDatetime.of_lastdayofmonth(date);
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
            throw new Exception("NO - Function");
        }

    }
}