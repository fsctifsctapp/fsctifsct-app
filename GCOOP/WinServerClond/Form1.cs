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
using System.IO;
using DBAccess;

namespace WinServerClond
{
    public partial class Form1 : Form
    {
        private String wsTopIpAddress = "";

        private DataTable data;
        private Thread thread;

        private Thread svThread;
        private Thread ctThread;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private int port = 0;
        private int accessing = 0;

        private delegate void OutputUpdateDataDelegate(int row, int pBytes, int vBytes, int status, String appPool);
        public void UpdateData(int row, int pBytes, int vBytes, int status, String appPool)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new OutputUpdateDataDelegate(UpdateDataDelegate), new object[] { row, pBytes, vBytes, status, appPool });
                }
            }
            catch { }
        }
        public void UpdateDataDelegate(int row, int pBytes, int vBytes, int status, String appPool)
        {
            dataGridView1["PrivateBytes", row].Value = pBytes;
            dataGridView1["VirtualBytes", row].Value = vBytes;
            dataGridView1["Status", row].Value = status;
            dataGridView1["AppPool", row].Value = appPool;
        }

        private delegate void OutputUpdateTopIP();
        public void UpdateTopIP()
        {
            try
            {
                if (LbTopIP.InvokeRequired)
                {
                    LbTopIP.Invoke(new OutputUpdateTopIP(UpdateTopIpDelegate), new object[] { });
                }
            }
            catch { }
        }
        public void UpdateTopIpDelegate()
        {
            LbTopIP.Text = "Top ip: " + wsTopIpAddress;
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

            XmlConfigService x = new XmlConfigService();
            port = x.ClondPort;

            DataTable dt01 = new DataTable("TB000");
            dt01.Columns.Add("IP", Type.GetType("System.String"));
            dt01.Columns.Add("Port", Type.GetType("System.Int32"));
            dt01.Columns.Add("AppPool", Type.GetType("System.String"));
            dt01.Columns.Add("PrivateBytes", Type.GetType("System.Int32"));
            dt01.Columns.Add("VirtualBytes", Type.GetType("System.Int32"));
            dt01.Columns.Add("Status", Type.GetType("System.Int32"));

            try
            {
                DataTable dt00 = GetX();
                for (int i = 0; i < dt00.Rows.Count; i++)
                {
                    int useFlag = Convert.ToInt32(dt00.Rows[i][2]);
                    if (useFlag == 1)
                    {
                        DataRow dr = dt01.NewRow();
                        dr["IP"] = dt00.Rows[i][0].ToString();
                        dr["Port"] = Convert.ToInt32(dt00.Rows[i][1]);
                        dr["Status"] = "0";
                        dt01.Rows.Add(dr);
                    }
                }
            }
            catch { }
            dataGridView1.DataSource = dt01;
        }

        private void BtStart_Click(object sender, EventArgs e)
        {
            if (BtStart.Text == "Start")
            {
                data = dataGridView1.DataSource as DataTable;
                dataGridView1.ReadOnly = true;

                thread = new Thread(new ThreadStart(Run));
                thread.Start();

                svThread = new Thread(new ThreadStart(RunThreadStart));
                svThread.Start();

                BtStart.Text = "Stop";
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
                ctThread = null;
                svThread = null;
                clientSocket = null;
                serverSocket = null;

                try
                {
                    thread.Abort();
                }
                catch { }
                thread = null;
                BtStart.Text = "Start";
            }
        }

        private void Run()
        {
            while (true)
            {
                String topIp = "";
                int topStatus = 0;
                int topPrivate = 0;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    try
                    {
                        String ipAddr = data.Rows[i]["IP"].ToString();
                        int portAddr = Convert.ToInt32(data.Rows[i]["Port"]);
                        String resu = CallWinWebServiceMonotor(ipAddr, portAddr, "A");
                        String[] res = resu.Split(',');
                        int pBytes = int.Parse(res[0]);
                        int vBytes = int.Parse(res[1]);
                        int status = int.Parse(res[2]);
                        String appPool = res[3];

                        if (status > topStatus)
                        {
                            topIp = data.Rows[i]["IP"].ToString();
                            topStatus = status;
                            topPrivate = pBytes;
                        }
                        else if (pBytes > topPrivate && status >= topStatus)
                        {
                            topIp = data.Rows[i]["IP"].ToString();
                            topPrivate = pBytes;
                            topStatus = status;
                        }
                        UpdateData(i, pBytes, vBytes, status, appPool);
                    }
                    catch 
                    {
                        UpdateData(i, 0, 0, -9, "-");
                    }
                }
                wsTopIpAddress = topIp;
                UpdateTopIP();
                Thread.Sleep(5000);
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
                //UpdateTextBoxMessage(" >>  Server Started \r\n");
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
                //UpdateTextBoxMessage(" >>  exit\r\n");
                break;
            }
        }

        private void RunSocket()
        {
            int accessingNow = accessing;
            while ((true))
            {
                int requestCount = 0;
                byte[] bytesFrom = new byte[10000];
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
                    wsReturn = wsTopIpAddress;//CheckWebServiceMonitor();
                    //this.UpdateTextBoxMessage(" >> " + "From client-" + clNo + dataFromClient + "\r\n");
                    serverResponse = wsReturn;
                    sendBytes = Encoding.UTF8.GetBytes(serverResponse + "¶");
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    //this.UpdateTextBoxMessage(" >>  " + accessingNow.ToString("0000") + "::::" + pManage + "\r\n");
                }
                catch 
                {
                    //this.UpdateTextBoxMessage(" >> " + accessingNow.ToString("0000") + "" + ex.ToString() + "\r\n");
                }
                break;
            }
        }

        public static String CallWinWebServiceMonotor(String ip, int port, String text)
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                String result = "";
                String sender = text;
                String winIp = ip;
                int winPort = port;
                //*****************************************************
                clientSocket.ReceiveTimeout = 5000;
                clientSocket.SendTimeout = 5000;
                clientSocket.Connect(winIp, winPort);
                NetworkStream serverStream = clientSocket.GetStream();
                serverStream.ReadTimeout = 5000;
                serverStream.WriteTimeout = 5000;
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
            ctThread = null;
            svThread = null;
            clientSocket = null;
            serverSocket = null;

            try
            {
                thread.Abort();
            }
            catch { }

            thread = null;
        }

        private DataTable GetX()
        {
            String path = "";
            try
            {
                string filePath = "C:\\TEMP\\gcoop_path.txt";
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);
                    path = reader.ReadLine() + @"XMLConfig\server.clond.xml";
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
}
