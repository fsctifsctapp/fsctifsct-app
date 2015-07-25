using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using DBAccess;
using System.IO;

namespace WinReport
{
    public partial class WinReportForm : Form
    {
        private Thread svThread;
        private Thread ctThread;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private delegate void OutputUpdateDelegate(string data);
        public void UpdateTextBoxMessage(String data)
        {
            try
            {
                if (tbText.InvokeRequired)
                {
                    tbText.Invoke(new OutputUpdateDelegate(UpdateTextBoxDelegate), new object[] { data });
                }
            }
            catch { }
        }
        public void UpdateTextBoxDelegate(string data)
        {
            tbText.AppendText(data);
        }
        private int port = 0;
        private int accessing = 0;
        private String exePBPath = "";
        private String reportPDFPath = "";

        public WinReportForm()
        {
            InitializeComponent();
            XmlConfigService x = new XmlConfigService();
            port = x.WinReportPort;// 4115;//int.Parse(xmlConfig.of_getconstantvalue("winprint.winprint_port"));
            exePBPath = x.WinReportExePBPath;
            reportPDFPath = x.ReportPDFPath;
            svThread = new Thread(new ThreadStart(Run));
            svThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                Thread.Sleep(3000);
                serverSocket = new TcpListener(port);
                clientSocket = default(TcpClient);
                int counter = 0;
                serverSocket.Start();
                UpdateTextBoxMessage("\r\n >>  Server Started \r\n");
                counter = 0;
                while (true)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    accessing = counter;
                    UpdateTextBoxMessage(" >>  " + counter.ToString("0000") + " Accessing ...."+DateTime.Now.ToString("dd/MM/yy HH:mm:ss")+" \r\n");
                    ctThread = new Thread(RunSocket);
                    ctThread.Start();
                }
                clientSocket.Close();
                serverSocket.Stop();
                UpdateTextBoxMessage(" >>  exit\r\n");
                break;
            }
        }

        private void RunSocket()
        {
            int accessingNow = accessing;
            while ((true))
            {
                int requestCount = 0;
                byte[] bytesFrom = new byte[20000000];
                string dataFromClient = null;
                Byte[] sendBytes = null;
                string serverResponse = null;
                requestCount = 0;
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("¶"));
                    String wsReturn = "";
                    String pManage = PrintManager(dataFromClient, ref wsReturn);
                    //this.UpdateTextBoxMessage(" >> " + "From client-" + clNo + dataFromClient + "\r\n");
                    serverResponse = wsReturn;
                    sendBytes = Encoding.UTF8.GetBytes(serverResponse + "¶");
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    this.UpdateTextBoxMessage(" >>  " + accessingNow.ToString("0000") + "" + pManage + "\r\n");
                }
                catch (Exception ex)
                {
                    this.UpdateTextBoxMessage(" >> " + accessingNow.ToString("0000") + "" + ex.ToString() + "\r\n");
                }
                break;
            }
        }

        private String PrintManager(String dataFromClient, ref String webServiceResult)
        {
            try
            {
                String[] message = dataFromClient.Split('`');
                String commandCode = message[0];
                if (commandCode == "reportpdf")
                {
                    String[] args = message[3].Split('´');
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(exePBPath, dataFromClient);
                    psi.RedirectStandardOutput = true;
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    psi.UseShellExecute = false;
                    System.Diagnostics.Process listFiles = System.Diagnostics.Process.Start(psi);
                    System.IO.StreamReader myOutput = listFiles.StandardOutput;
                    listFiles.WaitForExit(300000);
                    //C:\GCOOP_ALL\CAT\GCOOP\WebService\Report\PDF\
                    String filePath = reportPDFPath + args[4];
                    if (!File.Exists(filePath)) throw new Exception("สร้างรายงาน PDF ไม่สำเร็จ");
                    webServiceResult = "1";
                    return ">>  APP: " + args[0] + "  >>  REPORT: " + args[2] + "  >>  Result: " + 1;
                }
                else
                {
                    throw new Exception("?? ?? ??");
                }
            }
            catch (Exception ex)
            {
                webServiceResult = "-1";
                return ex.Message;
            }
        }

        private void MultiSocket_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                svThread.Abort();
            }
            catch { }
            try
            {
                ctThread.Abort();
            }
            catch { }
            try
            {
                clientSocket.Close();
            }
            catch { }
            try
            {
                serverSocket.Stop();
            }
            catch { }
            ctThread = null;
            svThread = null;
            clientSocket = null;
            serverSocket = null;
        }
    }
}
