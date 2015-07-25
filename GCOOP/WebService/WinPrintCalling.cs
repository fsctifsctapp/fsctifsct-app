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
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace WebService
{
    public class WinPrintCalling
    {
        private Security security;

        public WinPrintCalling(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public WinPrintCalling(String wsPass, bool checkPermission)
        {
            ConstructorEnding(wsPass, checkPermission);
        }

        private void ConstructorEnding(String wsPass, bool checkPermission)
        {
            security = new Security(wsPass);
        }

        public String CallWinPrint(String application, String className, String[] args)
        {
            TcpClient tcpclnt = new TcpClient();
            try
            {
                String result = "";
                String[] ss = new String[10];
                ss[0] = className;
                ss[1] = security.ConnectionString;
                ss[2] = application;
                String ss3 = "";
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i > 0)
                        {
                            ss3 += "^";
                        }
                        ss3 += string.IsNullOrEmpty(args[i]) ? "" : args[i];
                    }
                    ss[3] = ss3;
                }
                else
                {
                    ss[3] = "";
                }
                String sender = String.Format("{0},{1},{2},{3}", ss);
                tcpclnt.Connect(security.WinPrintIP, security.WinPrintPort);
                Stream stm = tcpclnt.GetStream();
                //ASCIIEncoding asen = new ASCIIEncoding();
                UnicodeEncoding asen = new UnicodeEncoding();
                byte[] ba = asen.GetBytes(sender);
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[1000];
                int k = stm.Read(bb, 0, 1000);
                for (int i = 0; i < k; i++)
                {
                    result += Convert.ToChar(bb[i]);
                }
                tcpclnt.Close();
                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    tcpclnt.Close();
                }
                catch { }
                throw ex;
            }
        }
    }
}
