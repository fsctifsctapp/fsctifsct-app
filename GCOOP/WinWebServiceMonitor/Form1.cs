using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Net.Sockets;
using System.Collections;
using System.DirectoryServices;
using System.IO;

namespace WinWebServiceMonitor
{
    public partial class Form1 : Form
    {
        private Thread svThread;
        private Thread ctThread;
        private Thread atThread;
        private String wsIpAddress;
        private String wsSite;
        private String appPoolName;
        private String mode = "FIX";
        private int maxVirtualBytes = 0;
        private int maxPrivateBytes = 0;
        private int status = -1;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private int port = 0;
        private int hit = 0;
        private int accessing = 0;
        private delegate void OutputUpdateTextBoxDelegate(string data);
        public void UpdateTextBoxMessage(String data)
        {
            try
            {
                if (TbText.InvokeRequired)
                {
                    TbText.Invoke(new OutputUpdateTextBoxDelegate(UpdateTextBoxMessageDelegate), new object[] { data });
                }
            }
            catch { }
        }
        public void UpdateTextBoxMessageDelegate(string data)
        {
            TbText.AppendText(data);
        }

        private delegate void OutputUpdateStatusLabel(string data);
        public void UpdateStatusLabel(String data)
        {
            try
            {
                if (TbText.InvokeRequired)
                {
                    TbText.Invoke(new OutputUpdateStatusLabel(UpdateStatusLabelDelegate), new object[] { data });
                }
            }
            catch { }
        }
        public void UpdateStatusLabelDelegate(string data)
        {
            TbText.AppendText(data);
        }

        private delegate void OutputUpdateStatusTextBox(string data);
        public void UpdateStatusTextBox(String data)
        {
            try
            {
                if (TbStatus.InvokeRequired)
                {
                    TbStatus.Invoke(new OutputUpdateStatusTextBox(UpdateStatusTextBoxDelegate), new object[] { data });
                }
            }
            catch { }
        }
        public void UpdateStatusTextBoxDelegate(string data)
        {
            TbStatus.Text = data;
        }

        /// <summary>
        /// get:คือค่า path เป็น Folder GCOOP ตัวอย่าง: C:\\CAT\GCOOP\
        /// </summary>
        public String PhysicalPath
        {
            get
            {
                string appPath = Application.ExecutablePath;// HttpContext.Current.Request.ApplicationPath;
                string physicalPath = appPath;// HttpContext.Current.Request.MapPath(appPath);
                string physicalPathGCOOP = physicalPath.Substring(0, physicalPath.ToUpper().IndexOf("\\GCOOP\\") + 7);
                return physicalPathGCOOP;
            }
        }

        public Form1()
        {
            InitializeComponent();

            try
            {
                string filePath = "C:\\TEMP\\gcoop_path.txt";
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(this.PhysicalPath);
                writer.Close();
            }
            catch { }

            try
            {
                String[] args = Program.args;
                if (args.Length == 7)
                {
                    TbIp.Text = args[0];
                    TbPort.Text = args[1];
                    TbSite.Text = args[2];
                    TbAppPool.Text = args[3];
                    TbPrivate.Text = args[4];
                    TbVirtual.Text = args[5];
                    mode = args[6] == "A" ? "AUTO" : "FIX";
                    TbMode.Text = mode;
                    if (mode == "FIX")
                    {
                        TbStatus.Text = args[6];
                        this.status = int.Parse(args[6]);
                    }
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
            try
            {
                atThread.Abort();
            }
            catch { }
            atThread = null;
            ctThread = null;
            svThread = null;
            clientSocket = null;
            serverSocket = null;
        }

        private void BtStart_Click(object sender, EventArgs e)
        {
            if (BtStart.Text == "Start")
            {
                StartAppForce();
                svThread = new Thread(new ThreadStart(RunThreadStart));
                svThread.Start();
            }
            else
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
                try
                {
                    atThread.Abort();
                }
                catch { }
                atThread = null;
                ctThread = null;
                svThread = null;
                clientSocket = null;
                serverSocket = null;
                BtStart.Text = "Start";
            }
        }

        public void RunThreadStart()
        {
            while (true)
            {
                Thread.Sleep(3000);
                serverSocket = new TcpListener(port);
                clientSocket = default(TcpClient);
                int counter = 0;
                serverSocket.Start();
                UpdateTextBoxMessage(" >>  Server Started \r\n");
                counter = 0;
                while (true)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    accessing = counter;
                    //String ip = clientSocket.Client.RemoteEndPoint.ToString();
                    //UpdateTextBoxMessage(" >>  " + counter.ToString("0000") + " Accessing .... \r\n");
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
                byte[] bytesFrom = new byte[20000];
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
                    String wsReturn = "";//Doys
                    //*****//String pManage = PrintManager(clientSocket.Client.RemoteEndPoint.ToString(), dataFromClient, ref wsReturn);
                    wsReturn = CheckWebServiceMonitor(false);
                    //this.UpdateTextBoxMessage(" >> " + "From client-" + clNo + dataFromClient + "\r\n");
                    serverResponse = wsReturn;
                    sendBytes = Encoding.UTF8.GetBytes(serverResponse + "¶");
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    //this.UpdateTextBoxMessage(" >>  " + accessingNow.ToString("0000") + "::::" + pManage + "\r\n");
                }
                catch (Exception ex)
                {
                    this.UpdateTextBoxMessage(" >> " + accessingNow.ToString("0000") + "" + ex.ToString() + "\r\n");
                }
                break;
            }
        }

        public void StartAppForce()
        {
            try
            {
                TbText.Text = "";
                wsIpAddress = TbIp.Text.Trim();
                wsSite = TbSite.Text.Trim();
                String url = "";// "http://" + wsIpAddress + "/" + wsSite + "/GCOOP/WebService/Common.asmx";
                appPoolName = TbAppPool.Text.Trim();
                port = int.Parse(TbPort.Text.Trim());
                maxPrivateBytes = int.Parse(TbPrivate.Text);
                maxVirtualBytes = int.Parse(TbVirtual.Text);

                int iii = 0;
                while (GetPrivateBytes() < 100000)
                {
                    WsCommon.Common cm = new WsCommon.Common();
                    cm.Url = url = "http://" + wsIpAddress + "/" + wsSite + "/GCOOP/WebService/Common.asmx";
                    cm.GetStatusApplicationData("x");
                    cm.GetStatusApplicationData("x");
                    cm.Dispose();

                    try
                    {
                        url = "http://" + wsIpAddress + "/" + wsSite + "/GCOOP/WebService/Shrlon.asmx";
                        WsShrlon.Shrlon sl = new WsShrlon.Shrlon();
                        sl.Url = url;
                        sl.OfInitListInRcv("x", "CSH", new DateTime(2011, 1, 1));
                        sl.OfInitListInRcv("x", "CSH", new DateTime(2011, 2, 1));
                        sl.Dispose();
                    }
                    catch { }
                    CheckWebServiceMonitor(true);
                    iii++;
                    if (iii > 100) break;
                    //break;
                }
                CheckWebServiceMonitor(true);
                BtStart.Text = "Stop";
            }
            catch (Exception ex)
            {
                UpdateStatus("Error: " + ex.Message);
            }
        }

        private int GetPrivateBytes()
        {
            int p = 0, v = 0;
            Process[] m_arrSysProcesses = Process.GetProcesses();
            for (int i = 0; i < m_arrSysProcesses.Length; i++)
            {
                Process proc = m_arrSysProcesses[i];
                String appPoool = GetProcessOwner(proc.Id);
                if (proc.ProcessName.ToLower() == "w3wp" && appPoool.ToLower() == appPoolName.ToLower())
                {
                    return (proc.PrivateMemorySize / 1024);
                }
            }
            return 0;
        }

        private String CheckWebServiceMonitor(bool upLabel)
        {
            String status = "", appPool = "-";
            int p = 0, v = 0;
            Process[] m_arrSysProcesses = Process.GetProcesses();
            for (int i = 0; i < m_arrSysProcesses.Length; i++)
            {
                Process proc = m_arrSysProcesses[i];
                String appPoool = GetProcessOwner(proc.Id);
                if (proc.ProcessName.ToLower() == "w3wp" && appPoool.ToLower() == appPoolName.ToLower())
                {
                    appPool = appPoool;
                    p = proc.PrivateMemorySize / 1024;
                    v = proc.VirtualMemorySize / 1024;
                    status = "Status:: Private Bytes: " + (proc.PrivateMemorySize / 1024).ToString("#,##0") + " KB  Virtual Bytes: " + (proc.VirtualMemorySize / 1024).ToString("#,##0") + " KB";
                }
            }
            if (status != "")
            {
                if (upLabel)
                {
                    UpdateStatus(status);
                    try
                    {
                        TbStatus.Text = this.status.ToString();
                    }
                    catch { }
                }
                else
                {
                    UpdateTextBox(status);
                }
                if (this.status != 0 && mode == "AUTO")
                {
                    this.status = 1;
                }
            }
            else if (this.status != 0 && mode == "AUTO")
            {
                this.status = -1;
            }
            if (mode == "AUTO")
            {
                if ((p >= maxPrivateBytes || v >= maxVirtualBytes) && this.status != 0)
                {
                    this.status = 0;
                    AutoRestartApplicationPool();
                }
                try
                {
                    UpdateStatusTextBox(this.status.ToString());
                }
                catch { }
            }
            return p + "," + v + "," + this.status + "," + appPool;
        }

        private string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user 
                    //return argList[1] + "\\" + argList[0];
                    return argList[0];
                }
            }
            return "NO OWNER";
        }

        private void AddMessageLine(String text)
        {
            TbText.AppendText(text + "\r\n");
        }

        private void UpdateStatus(String text)
        {
            hit++;
            try
            {
                LbStatus.Text = text + "   Hit: " + hit.ToString("#,##0");
            }
            catch
            {
                UpdateStatusLabel(text + "   Hit: " + hit.ToString("#,##0"));
            }
        }

        private void UpdateTextBox(String text)
        {
            hit++;
            try
            {
                TbText.AppendText(text + "   Hit: " + hit.ToString("#,##0") + "\r\n");
            }
            catch
            {
                UpdateTextBoxMessage(text + "   Hit: " + hit.ToString("#,##0") + "\r\n");
            }
        }

        /// <summary>
        /// Get a list of available Application Pools
        /// </summary>
        /// <returns></returns>
        public List<string> HentAppPools()
        {

            List<string> list = new List<string>();
            DirectoryEntry W3SVC = new DirectoryEntry("IIS://LocalHost/w3svc", "", "");

            foreach (DirectoryEntry Site in W3SVC.Children)
            {
                if (Site.Name == "AppPools")
                {
                    foreach (DirectoryEntry child in Site.Children)
                    {
                        list.Add(child.Name);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Recycle an application pool
        /// </summary>
        /// <param name="IIsApplicationPool"></param>
        public void RecycleAppPool(string IIsApplicationPool)
        {
            ManagementScope scope = new ManagementScope(@"\\localhost\root\MicrosoftIISv2");
            scope.Connect();
            ManagementObject appPool = new ManagementObject(scope, new ManagementPath("IIsApplicationPool.Name='W3SVC/AppPools/" + IIsApplicationPool + "'"), null);
            appPool.InvokeMethod("Recycle", null, null);
        }

        private void AutoRestartApplicationPool()
        {
            try
            {
                atThread.Abort();
            }
            catch { }
            atThread = null;
            atThread = new Thread(new ThreadStart(RunAutoReAppPool));
            atThread.Start();
        }

        private void RunAutoReAppPool()
        {
            while (true)
            {
                Thread.Sleep(20000);
                try
                {
                    RecycleAppPool(appPoolName);
                    String url = "";
                    int iii = 0;

                    while (GetPrivateBytes() < 100000)
                    {
                        try
                        {
                            url = "http://" + wsIpAddress + "/" + wsSite + "/GCOOP/WebService/Common.asmx";
                            WsCommon.Common cm = new WsCommon.Common();
                            cm.Url = url;
                            cm.GetStatusApplicationData("x");
                            cm.GetStatusApplicationData("x");
                            cm.Dispose();
                        }
                        catch { }

                        try
                        {
                            url = "http://" + wsIpAddress + "/" + wsSite + "/GCOOP/WebService/Shrlon.asmx";
                            WsShrlon.Shrlon sl = new WsShrlon.Shrlon();
                            sl.Url = url;
                            sl.OfInitListInRcv("x", "CSH", new DateTime(2011, 1, 1));
                            sl.OfInitListInRcv("x", "CSH", new DateTime(2011, 2, 1));
                            sl.Dispose();
                        }
                        catch { }
                        iii++;
                        if (iii > 300) break;
                    }

                    this.status = 1;
                }
                catch { }
                break;
            }
        }

        private void BtReApp_Click(object sender, EventArgs e)
        {
            RecycleAppPool(appPoolName);
        }
    }
}
