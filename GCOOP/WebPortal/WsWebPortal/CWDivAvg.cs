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
    public class CWDivAvg
    {
        private Int32 _divAvgYear;
        /// <summary>
        /// 
        /// </summary>
        public Int32 DivAvgYear
        {
            get { return _divAvgYear; }
            set { _divAvgYear = value;}
        }
        private Double _divAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double DivAmt
        {
            get { return _divAmt; }
            set { _divAmt = value;}
        }
        private Double _avgAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double AvgAmt
        {
            get { return _avgAmt; }
            set { _avgAmt = value;}
        }

        public List<CWDivAvg> GetDivAvg(String memberNo)
        {
            List<CWDivAvg> cList = new List<CWDivAvg>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT 
                        DIVAVG_YEAR,DIVIDEND_AMT,AVERAGE_AMT
                  FROM 
                        MBDIVAVGMAST
                  WHERE 
                        DIVAVG_YEAR = (SELECT MAX(DIVAVG_YEAR) 
                                       FROM MBDIVAVGMAST 
                                       WHERE MEMBER_NO='" + memNo + "') AND MEMBER_NO='" + memNo + "'";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWDivAvg m = new CWDivAvg();
                m.DivAvgYear = dt.GetInt32("DIVAVG_YEAR");
                m.DivAmt = dt.GetDouble("DIVIDEND_AMT");
                m.AvgAmt = dt.GetDouble("AVERAGE_AMT");
                cList.Add(m);
            }
            return cList;
        }

    }
}
