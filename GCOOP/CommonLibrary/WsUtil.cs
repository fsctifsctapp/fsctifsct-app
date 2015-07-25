using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CommonLibrary.WsWalfare;
using CommonLibrary.WsCommon;
using CommonLibrary.WsReport;

namespace CommonLibrary
{
    public class WsUtil
    {
        public static String Wshost
        {
            get
            {
                String ip = "";
                try
                {
                    WebState x = new WebState();
                    if (x.ClondUsing)
                    {
                        ip = SocketClient.WebServiceIp().Trim();
                        if (ip == "") throw new Exception();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    ip = ConfigurationManager.AppSettings["wshost_00"].ToString();
                }
                return ip;
            }
        }
        public static String WsProgress
        {
            get
            {
                String ip = ConfigurationManager.AppSettings["wsprogress_00"].ToString();// "wsprogress_00";
                return ip;
            }
        }
        public static String WsReport
        {
            get { return ConfigurationManager.AppSettings["wsreport_00"].ToString(); }
        }
        public static String SitePrefix
        {
            get { return ConfigurationManager.AppSettings["sitePrefix"].ToString(); }
        }

        public static Walfare Walfare
        {
            get
            {
                Walfare wf = new Walfare();
                //wf.Url = ConfigurationManager.AppSettings["wsUrl"].ToString() + "Walfare.asmx";
                wf.Url = "http://" + WsUtil.Wshost + "/" + WsUtil.SitePrefix + "/GCOOP/WebService/Walfare.asmx";
                return wf;
            }
        }

        public static Common Common
        {
            get
            {
                Common cm = new Common();
                //String url = ConfigurationManager.AppSettings["wsUrl"].ToString() + "Common.asmx";
                //cm.Url = url;
                cm.Url = "http://" + WsUtil.Wshost + "/" + WsUtil.SitePrefix + "/GCOOP/WebService/Common.asmx";
                return cm;
            }
        }

        public static Report Report
        {
            get
            {
                Report rp = new Report();
                //String url = ConfigurationManager.AppSettings["wsReportUrl"].ToString() + "Report.asmx";
                //rp.Url = url;
                rp.Url = "http://" + WsUtil.WsReport + "/" + WsUtil.SitePrefix + "/GCOOP/WebServiceReport/Report.asmx";
                return rp;
            }
        }
    }
}
