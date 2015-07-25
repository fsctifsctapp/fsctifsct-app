using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Globalization;
using DBAccess;

namespace WinLog
{
    public partial class Form1 : Form
    {
        private delegate void OutputUpdateDelegate(string data);
        private Thread svThread;
        private Thread ctThread;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        public void UpdateTextBoxMessage(String data)
        {
            try
            {
                if (TbText.InvokeRequired)
                {
                    TbText.Invoke(new OutputUpdateDelegate(UpdateTextBoxDelegate), new object[] { data });
                }
            }
            catch { }
        }
        public void UpdateTextBoxDelegate(string data)
        {
            TbText.AppendText(data);
        }
        private String GCOOPPath = "";
        private String LogPath = "";
        private int port = 0;
        private int accessing = 0;
        private String connectionString = "";

        public Form1()
        {
            InitializeComponent();
            String path = "";
            try
            {
                string filePath = "C:\\TEMP\\gcoop_path.txt";
                StreamReader writer = new StreamReader(filePath);
                path = writer.ReadToEnd();
                writer.Close();
                //path = path.Trim() + "WinStatus\\config.xml";
            }
            catch { }
            GCOOPPath = path.Trim();
            LogPath = GCOOPPath + "WinLog\\log\\";
            try
            {
                String filePath = GCOOPPath + "WinLog\\connection.txt";
                StreamReader read = new StreamReader(filePath);
                connectionString = read.ReadToEnd().Trim();
                read.Close();
            }
            catch { }
            try
            {
                String filePath = LogPath + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                StreamReader read = new StreamReader(filePath);
                TbText.Text = read.ReadToEnd();
                read.Close();
            }
            catch { }
            try
            {
                String filePath = GCOOPPath + "WinLog\\port.txt";
                StreamReader read = new StreamReader(filePath);
                port = Convert.ToInt32(read.ReadToEnd().Trim());
                read.Close();
            }
            catch { }
            svThread = new Thread(new ThreadStart(Run));
            svThread.Start();
        }

        public void WriteEndLine(String xxx)
        {
            try
            {
                String filePath = LogPath + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                StreamWriter writer = new StreamWriter(filePath, true);
                try
                {
                    writer.WriteLine(xxx);
                }
                catch (Exception ex)
                {
                    UpdateTextBoxMessage(ex.Message + "\r\n");
                }
                writer.Close();
            }
            catch (Exception ex)
            {
                UpdateTextBoxMessage("BIG ERROR: " + ex.ToString() + "\r\n");
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

        private void Run()
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
                    String pManage = PrintManager(clientSocket.Client.RemoteEndPoint.ToString(), dataFromClient, ref wsReturn);
                    //this.UpdateTextBoxMessage(" >> " + "From client-" + clNo + dataFromClient + "\r\n");
                    serverResponse = wsReturn;
                    sendBytes = Encoding.UTF8.GetBytes(serverResponse + "¶");
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    this.UpdateTextBoxMessage(" >>  " + accessingNow.ToString("0000") + "::::" + pManage + "\r\n");
                }
                catch (Exception ex)
                {
                    this.UpdateTextBoxMessage(" >> " + accessingNow.ToString("0000") + "" + ex.ToString() + "\r\n");
                }
                break;
            }
        }

        private String PrintManager(String ip, String dataFromClient, ref String webServiceResult)
        {
            Sta ta = new Sta(connectionString);
            try
            {
                String resu = ip + "   " + dataFromClient;
                webServiceResult = resu;
                String[] arr = resu.Replace("   ", "ɵ").Split('ɵ');

                //DateTime hit_date = DateTime.ParseExact(arr[1], "yyyy-MM-dd", new CultureInfo("en-US"));
                String hidDateTime = "to_date('" + arr[1] + "', 'yyyy-mm-dd')";
                String hit_time = arr[2];
                String client_ip = arr[3];
                String username = arr[4];
                String url = arr[5];
                String method = arr[6];
                String jspostback = arr[7];
                String webservice = arr[8];
                int wsram = 0;
                String webservicereport = arr[10];
                int wsrram = 0;
                String sql = @"insert into hitlog
                        (server_ip,             hit_date,               hit_time,                       client_ip, 
                        username,               url,                    method,                         jspostback,
                        webservice,             webservice_ram,         webservicereport,               webservicereport_ram)
                        values
                        ('" + ip + "',          " + hidDateTime + ",    '" + hit_time + "',             '" + client_ip + @"',
                        '" + username + "',     '" + url + @"',         '" + method + @"',              '" + jspostback + @"',
                        '" + webservice + "',   '" + wsram + "',        '" + webservicereport + "',     '" + wsrram + "')";

                
                int iii = ta.Exe(sql);
                ta.Close();
                return resu;
            }
            catch (Exception ex)
            {
                ta.Close();
                return ex.Message;
            }
        }
    }
}
