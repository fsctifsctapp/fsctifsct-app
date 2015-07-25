using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using pbservice;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WinPrint.ap_deposit;

namespace WinPrint
{
    public partial class Form1 : Form
    {
        private Thread thread;
        private TcpListener myList;
        private Socket s;
        private String webServiceResult;
        private String ipAddress;
        private int port = 4112;
        private int iAccept;

        public Form1()
        {
            InitializeComponent();
            textBox3.Text = "";
            //Get IP Address
            ipAddress = "";
            String clientip = "";
            IPHostEntry ipEntry = null;
            String computername = Dns.GetHostName().ToString();
            try
            {
                TestInstant();
            }
            catch { }
            try
            {
                try
                {
                    ipEntry = Dns.GetHostByName(computername);// Dns.GetHostEntry(computername); //Dns.GetHostByName(computername);
                }
                catch (Exception ex) { ex.ToString(); }
                IPAddress[] addr = ipEntry.AddressList;
                if (addr.Length > 1)
                {
                    clientip = addr[0].ToString();
                }
                else
                {
                    for (int i = 0; i < addr.Length; i++)
                    {
                        clientip = clientip + addr[i].ToString();
                    }
                }
                ipAddress = clientip;
            }
            catch
            {
                ipAddress = "127.0.0.1";
            }

            n_cst_xmlconfig xmlConfig = new n_cst_xmlconfig();
            ipAddress = xmlConfig.of_getconstantvalue("winprint.winprint_ip");
            port = int.Parse(xmlConfig.of_getconstantvalue("winprint.winprint_port"));

            thread = new Thread(new ThreadStart(Running));
            thread.Start();
            textBox3.Text = "GCOOP Print server " + ipAddress + ":" + port + " started....\r\n";
        }

        private void Running()
        {
            while (true)
            {
                try
                {
                    webServiceResult = "";
                    IPAddress ipAd = IPAddress.Parse(ipAddress);

                    /* Initializes the Listener */
                    myList = new TcpListener(ipAd, port);

                    /* Start Listeneting at the specified port */
                    myList.Start();

                    s = myList.AcceptSocket();
                    iAccept++;
                    UpdateTextBoxMessage("[" + iAccept + "]Accepted from " + s.RemoteEndPoint + " ::  ");

                    byte[] b = new byte[20000000];
                    int k = s.Receive(b);
                    UnicodeEncoding unen = new UnicodeEncoding();
                    String receiveMessage = unen.GetString(b, 0, k);

                    String pManage = PrintManager(receiveMessage);
                    ASCIIEncoding asen = new ASCIIEncoding();

                    s.Send(asen.GetBytes(webServiceResult));
                    UpdateTextBoxMessage(pManage + "\r\n");

                    s.Close();
                    myList.Stop();
                }
                catch (Exception e)
                {
                    SetTextBoxMessage(e.ToString());
                    break;
                }
            }
            UpdateTextBoxMessage("\r\n------ Good bye ------");
        }

        private delegate void OutputUpdateDelegate(string data);
        private delegate void OutputSetDelegate(string data);

        private void UpdateTextBoxMessage(String data)
        {
            try
            {
                if (textBox3.InvokeRequired)
                {
                    textBox3.Invoke(new OutputUpdateDelegate(UpdateTextBoxDelegate), new object[] { data });
                }
            }
            catch { }
        }

        private void UpdateTextBoxDelegate(string data)
        {
            textBox3.Text += data;
            textBox3.SelectionStart = textBox3.TextLength;
            textBox3.ScrollToCaret();
        }

        private void SetTextBoxMessage(String data)
        {
            try
            {
                if (textBox3.InvokeRequired)
                {
                    textBox3.Invoke(new OutputSetDelegate(SetTextBoxDelegate), new object[] { data });
                }
            }
            catch { }
        }

        private void SetTextBoxDelegate(string data)
        {
            textBox3.Text += data;
        }

        private String PrintManager(String clientMessage)
        {
            try
            {
                String result = "";
                String[] message = clientMessage.Split(',');
                String commandCode = message[0];

                if (commandCode != "dp_slip" && commandCode != "dp_book" && commandCode != "printpdf")
                {
                    n_cst_dbconnectservice conSv = new n_cst_dbconnectservice();
                    String returnBack = "";
                    try
                    {
                        conSv.of_connectdb(message[1]);
                        WinPrintInterface winIntf = (WinPrintInterface)System.Activator.CreateInstance(Type.GetType("WinPrint." + message[2] + "." + commandCode));

                        String[] winPrintIntfArgs = message[3].Split('^');
                        winIntf.SetArgument(winPrintIntfArgs);

                        winIntf.SetTransMaual(conSv);

                        webServiceResult = "";
                        returnBack = winIntf.Run(ref webServiceResult);

                        conSv.of_disconnectdb();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            conSv.of_disconnectdb();
                        }
                        catch { }
                        throw ex;
                    }
                    return returnBack;
                }

                String connectionString = message[1];
                n_cst_dbconnectservice lnv_con = new n_cst_dbconnectservice();
                lnv_con.of_connectdb(connectionString);

                try
                {

                    if (commandCode == "dp_slip")
                    {
                        String slipNo = message[2];
                        String branchId = message[3];
                        String formSet = message[4];
                        String accountNo = message[5];
                        String pageIndexS = message[6];
                        String lineIndexS = message[7];
                        String isBFB = message[8];
                        String printSeqS = message[9];
                        short pageIndex = Convert.ToInt16(pageIndexS);
                        short lineIndex = Convert.ToInt16(lineIndexS);
                        short printSeq = Convert.ToInt16(printSeqS);
                        bool isBF = isBFB.ToLower() == "true";

                        n_cst_deposit_service lnv_dep = new n_cst_deposit_service();
                        lnv_dep.of_settrans(lnv_con);
                        lnv_dep.of_init();
                        lnv_dep.of_print_slip(slipNo, branchId, formSet);
                        result = "Print slip number " + slipNo + " at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        webServiceResult = result;
                    }
                    else if (commandCode == "dp_book")
                    {
                        String slipNo = message[2];
                        String branchId = message[3];
                        String formSet = message[4];
                        String accountNo = message[5];
                        String pageIndexS = message[6];
                        String lineIndexS = message[7];
                        String isBFB = message[8];
                        String printSeqS = message[9];
                        short pageIndex = Convert.ToInt16(pageIndexS);
                        short lineIndex = Convert.ToInt16(lineIndexS);
                        short printSeq = Convert.ToInt16(printSeqS);
                        bool isBF = isBFB.ToLower() == "true";

                        n_cst_deposit_service lnv_dep = new n_cst_deposit_service();
                        lnv_dep.of_settrans(lnv_con);
                        lnv_dep.of_init();
                        webServiceResult = "";
                        lnv_dep.of_print_book(accountNo, branchId, printSeq, pageIndex, lineIndex, isBF, formSet, ref webServiceResult);
                        result = "Print book account " + accountNo + " at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else if (commandCode == "reportpdf")
                    {
                        String pkApp = message[2];
                        String pkGid = message[3];
                        String pkRid = message[4];
                        String xmlCriteria = message[5];
                        String pdfFilename = message[6];
                        n_cst_reportservice lnv_print = new n_cst_reportservice();
                        lnv_print.of_settrans(ref lnv_con);
                        int li_return = lnv_print.of_report_print_pdf(pkApp, pkGid, pkRid, xmlCriteria, pdfFilename);
                        webServiceResult = Convert.ToString(li_return);
                        result = "Create PDF Report(" + pkApp + "," + pkGid + "," + pkRid + "," + pdfFilename + ",XML:[" + xmlCriteria + "]) return " + webServiceResult;
                    }
                    else if (commandCode == "printpdf")
                    {
                        String xmlPrint = message[2];
                        String pdfFilename = message[3];
                        n_cst_printservice lnv_print = new n_cst_printservice();
                        lnv_print.of_settrans(ref lnv_con);
                        int li_return = lnv_print.of_print_pdf(xmlPrint, pdfFilename);
                        webServiceResult = Convert.ToString(li_return);
                        result = "Create PDF Print(" + pdfFilename + ",XML:[" + xmlPrint + "]) return " + webServiceResult;
                    }
                    else
                    {
                        //แสดง command ที่ส่งมาออกไปเลยถ้าไม่มี script รองรับไว้.
                        result = "Command not support command(): " + clientMessage;
                    }
                    lnv_con.of_disconnectdb();
                }
                catch (Exception ex1)
                {
                    lnv_con.of_disconnectdb();
                    throw ex1;
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private void TestInstant()
        {
            //BaseProduct b = (BaseProduct)System.Activator.CreateInstance(Type.GetType("ConsoleApplication1.Product")
            //    , new object[] { typeof(string) },
            //    new object[] { "123" }
            //);
            //n_cst_deposit_service dep = System.Activator.CreateInstance("n_cst_deposit_service");
            //WinPrintInterface winIntf = (PrintSlip)System.Activator.CreateInstance(Type.GetType("WinPrint.ap_deposit.PrintSlip"));
            ////((WinPrintBase)winIntf).dbConnectService
            //winIntf.Run();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                s.Close();
            }
            catch { }
            try
            {
                myList.Stop();
            }
            catch { }
            try
            {
                thread.Abort();
            }
            catch { }
            try
            {
                thread = null;
            }
            catch { }
        }
    }
}