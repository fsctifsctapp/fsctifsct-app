using System;
using System.Security.Cryptography;
using SecurityEngine;
using pbreport;

namespace WebServiceReport
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
            try
            {
                encryptPassword = new Encryption().EncryptAscii(password);
            }
            catch { }
            try
            {
                encryptConnectionString = new Encryption().EncryptStrBase64(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            }
            catch { }
            if (autoCheckPassword && !IsPass)
            {
                throw new Exception("Password Service ไม่ถูกต้อง");
            }
        }
    }
}