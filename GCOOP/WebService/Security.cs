using System;
using System.Security.Cryptography;
using SecurityEngine;
using pbservice;
using System.Configuration;
using System.Web;
using Microsoft.Win32;
using System.IO;

namespace WebService
{
    public class Security
    {
        private String wsPass;
        private String password;
        private String encryptPassword;
        private String encryptConnectionString;

        public String WinPrintIP
        {
            get
            {
                String var = "";
                try
                {
                    n_cst_xmlconfig xmlConfig = new n_cst_xmlconfig();
                    var = xmlConfig.of_getconstantvalue("winprint.winprint_ip");
                }
                catch { }
                return var;
            }
        }

        public int WinPrintPort
        {
            get
            {
                int var = -1;
                try
                {
                    n_cst_xmlconfig xmlConfig = new n_cst_xmlconfig();
                    var = Convert.ToInt32(xmlConfig.of_getconstantvalue("winprint.winprint_port"));
                }
                catch { }
                return var;
            }
        }

        public String ConnectionString
        {
            get { return wsPass.Split('+')[1]; }
        }

        /// <summary>
        /// get:คือค่า path เป็น Folder GCOOP ตัวอย่าง: C:\CAT\GCOOP\
        /// </summary>
        public String PhysicalPath
        {
            get
            {
                string appPath = HttpContext.Current.Request.ApplicationPath;
                string physicalPath = HttpContext.Current.Request.MapPath(appPath);
                string physicalPathGCOOP = physicalPath.Substring(0, physicalPath.ToUpper().IndexOf("\\GCOOP\\") + 7);
                return physicalPathGCOOP;
            }
        }

        public bool IsPass
        {
            get
            {
                try { return wsPass.Split('+')[0] == password; }
                catch { return false; }
            }
        }

        public String Password
        {
            get { return password; }
        }

        public String EncryptPassword
        {
            get { return encryptPassword; }
        }

        public String EncryptConnectionString
        {
            get { return encryptConnectionString; }
        }

        public Security(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public Security(String wsPass, bool autoCheckPassword)
        {
            ConstructorEnding(wsPass, autoCheckPassword);
        }

        private void ConstructorEnding(String wsPass, bool autoCheckPassword)
        {
            try
            {
                string filePath = "C:\\TEMP\\gcoop_path.txt";
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(this.PhysicalPath);
                writer.Close();
            }
            catch { }
            //-------------- ท่อนปล่อย back door - security ต้องเอาออกภายหลัง
            if (wsPass == "x")
            {

                password = System.Configuration.ConfigurationManager.AppSettings["wsPass"].ToString();
                encryptPassword = new Encryption().EncryptAscii(password);
                encryptConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                this.wsPass = password + "+" + System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                autoCheckPassword = false;
                return;
            }
            //-------------- จบท่อนปล่อย back door - security
            password = System.Configuration.ConfigurationManager.AppSettings["wsPass"].ToString();
            try
            {
                this.wsPass = new Decryption().DecryptStrBase64(wsPass);
            }
            catch { this.wsPass = ""; }
            encryptPassword = new Encryption().EncryptAscii(password);
            encryptConnectionString = new Encryption().EncryptStrBase64(GetPrivateConStr());
            if (autoCheckPassword && !IsPass)
            {
                throw new Exception("Password Service ไม่ถูกต้อง");
            }
        }

        public String GetPrivateConStr()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
    }
}