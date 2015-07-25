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
using DBAccess;
using System.Collections.Generic;

namespace WsWebPortal
{
    public class CWShare
    {
        private Double _shareStkAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double ShareStkAmt
        {
            get { return _shareStkAmt; }
            set { _shareStkAmt = value;}
        }
        private Double _shareBeginAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double ShareBeginAmt
        {
            get { return _shareBeginAmt; }
            set { _shareBeginAmt = value;}
        }
        private Double _periodShareAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double PeriodShareAmt
        {
            get { return _periodShareAmt; }
            set { _periodShareAmt = value;}
        }
        private Double _shareValue;
        /// <summary>
        /// 
        /// </summary>
        public Double ShareValue
        {
            get { return _shareValue; }
            set { _shareValue = value;}
        }
        private Double _periodBaseAmat;
        /// <summary>
        /// 
        /// </summary>
        public Double PeriodBaseAmat
        {
            get { return _periodBaseAmat; }
            set { _periodBaseAmat = value;}
        }
        private Int32 _lastPeriod;
        /// <summary>
        /// 
        /// </summary>
        public Int32 LastPeriod
        {
            get { return _lastPeriod; }
            set { _lastPeriod = value;}
        }

        public List<CWShare> GetShare(String memberNo)
        {
            List<CWShare> cList = new List<CWShare>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT     
                    SHSHAREMASTER.SHARESTK_AMT, SHSHAREMASTER.SHAREBEGIN_AMT, 
                    SHSHAREMASTER.LAST_PERIOD, SHSHAREMASTER.PERIODBASE_AMT, 
                    SHSHAREMASTER.PERIODSHARE_AMT, SHSHARETYPE.SHARE_VALUE
                  FROM         
                    SHSHAREMASTER, SHSHARETYPE
                  WHERE     
                    SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE AND 
                    (SHSHAREMASTER.MEMBER_NO = '" + memNo + "')";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWShare m = new CWShare();
                m.LastPeriod = dt.GetInt32("LAST_PERIOD");
                m.PeriodBaseAmat = dt.GetDouble("PERIODBASE_AMT");
                m.PeriodShareAmt = dt.GetDouble("PERIODSHARE_AMT");
                m.ShareValue = dt.GetDouble("SHARE_VALUE");
                m.ShareBeginAmt = dt.GetDouble("SHAREBEGIN_AMT");
                m.ShareStkAmt = dt.GetDouble("SHARESTK_AMT");
                cList.Add(m);
            }
            return cList;
        }

    }
}
