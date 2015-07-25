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
using DBAccess;

namespace WebServiceReport
{
    public class WinPrintCalling
    {
        private Security security;
        private string connectionString;

        public WinPrintCalling(String wsPass)
        {
            this.connectionString = wsPass;
            ConstructorEnding(wsPass, true);
        }

        public WinPrintCalling(String wsPass, bool checkPermission)
        {
            ConstructorEnding(wsPass, checkPermission);
        }

        private void ConstructorEnding(String wsPass, bool checkPermission)
        {
            security = new Security(wsPass, false);
        }

        public String CallWinPrint(String application, String className, String[] args)
        {
            return CallWinPrint3(application, className, args);
        }

        //เตรียมลบ ไม่ได้ใช้แล้ว : 2011-10-20
        public String CallWinPrint2(String application, String className, String[] args)
        {
            TcpClient tcpclnt = new TcpClient();
            try
            {
                XmlConfigService x = new XmlConfigService();
                String winReportIP = x.WinReportIP;
                int winReportPort = x.WinReportPort;
                String result = "";
                String[] ss = new String[10];
                ss[0] = className;
                ss[1] = connectionString;
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
                tcpclnt.Connect(winReportIP, winReportPort);
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

        private String CallWinPrint3(String application, String className, String[] args)
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                XmlConfigService x = new XmlConfigService();
                int port = x.WinReportPort;
                string ip = x.WinReportIP;
                String result = "";
                String[] ss = new String[10];
                ss[0] = className;
                ss[1] = connectionString;
                ss[2] = application;
                String ss3 = "";
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i > 0)
                        {
                            ss3 += "´";
                        }
                        ss3 += string.IsNullOrEmpty(args[i]) ? "" : args[i];
                    }
                    ss[3] = ss3;
                }
                else
                {
                    ss[3] = "";
                }
                String sender = String.Format("{0}`{1}`{2}`{3}", ss);
                //*****************************************************
                clientSocket.Connect(ip, port);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes(sender + "¶");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                byte[] inStream = new byte[100000];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                result = System.Text.Encoding.UTF8.GetString(inStream);
                result = result.Substring(0, result.IndexOf("¶"));
                try
                {
                    serverStream.Close();
                }
                catch { }
                try
                {
                    clientSocket.Close();
                }
                catch { }
                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    clientSocket.Close();
                }
                catch { }
                throw ex;
            }
        }
    }
}
