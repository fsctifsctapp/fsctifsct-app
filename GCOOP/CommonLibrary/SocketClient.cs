using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CommonLibrary
{
    public class SocketClient
    {
        public static String RestartPDFServer()
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                WebState state = new WebState();

                String result = "";
                String sender = "WebServiceReport";// String.Format("{0}`{1}`{2}`{3}", ss);
                //*****************************************************
                clientSocket.Connect(state.SsWsReport, 4113);
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

        public static String WriteLoging(String hit)
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                WebState state = new WebState();

                String result = "";
                String sender = hit;
                String winIp = state.WinLogIP;
                int winPort = state.WinLogPort;
                //*****************************************************
                clientSocket.Connect(winIp, winPort);
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

        public static String WebServiceIp()
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                WebState state = new WebState();

                String result = "";
                String sender = "A";
                String winIp = state.ClondIP;
                int winPort = state.ClondPort;
                //*****************************************************
                clientSocket.Connect(winIp, winPort);
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
