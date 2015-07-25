using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DBAccess
{
    public class XmlConfigService
    {
        private DataTable dtXmlConfig;

        public DataTable XmlServiceData
        {
            get
            {
                String path = "";
                try
                {
                    string filePath = "C:\\TEMP\\gcoop_path.txt";
                    if (File.Exists(filePath))
                    {
                        StreamReader reader = new StreamReader(filePath);
                        path = reader.ReadLine() + @"XMLConfig\xmlconf.constmap.xml";
                        reader.Close();
                    }
                }
                catch { }
                try
                {
                    if (File.Exists(path))
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(path);
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                    }
                }
                catch { }
                return null;
            }
        }

        private void InitDataTableXmlConfig()
        {
            if (dtXmlConfig == null)
            {
                dtXmlConfig = this.XmlServiceData;
            }
        }

        private String GetXmlConfigDataString(String code)
        {
            InitDataTableXmlConfig();
            try
            {
                for (int i = 0; i < dtXmlConfig.Rows.Count; i++)
                {
                    String forCode = dtXmlConfig.Rows[i]["config_code"].ToString();
                    if (forCode == code)
                    {
                        return dtXmlConfig.Rows[i]["config_value"].ToString();
                    }

                }
            }
            catch { }
            return "";
        }

        private int GetXmlConfigDataInt(String code)
        {
            try
            {
                String v = GetXmlConfigDataString(code);
                int i = int.Parse(v);
                return i;
            }
            catch { return 0; }
        }

        /// <summary>
        /// คืนค่า path ของ PDF ex: C:\GCOOP_ALL\CAT\GCOOP\WebService\Report\PDF\
        /// </summary>
        public String ReportPDFPath { get { return GetXmlConfigDataString("reportservice.pdfpath"); } }

        public bool ClondUsing { get { return GetXmlConfigDataInt("server.cloud_using") == 1; } }

        public String ClondIP { get { return GetXmlConfigDataString("server.clond_ip"); } }

        public int ClondPort { get { return GetXmlConfigDataInt("server.clond_port"); } }

        public String WinPrintIP { get { return GetXmlConfigDataString("winprint.winprint_ip"); } }

        public int WinPrintPort { get { return GetXmlConfigDataInt("winprint.winprint_port"); } }

        public String WinReportIP { get { return GetXmlConfigDataString("winreport.winreport_ip"); } }

        public int WinReportPort { get { return GetXmlConfigDataInt("winreport.winreport_port"); } }

        public String WinReportExePBPath { get { return GetXmlConfigDataString("winreport.winreport_exepb_path"); } }

        public String WinLogIP { get { return GetXmlConfigDataString("winlog.winlog_ip"); } }

        public int WinLogPort { get { return GetXmlConfigDataInt("winlog.winlog_port"); } }

        public bool WinLogUsing { get { return GetXmlConfigDataInt("winlog.winlog_using") == 1; } }

        public String WinLogConnectionString { get { return GetXmlConfigDataString("winlog.winlog_connectionstring"); } }
    }
}
