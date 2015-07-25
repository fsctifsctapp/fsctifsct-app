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
    public class CWWhocoll
    {
        private String _lnContNo;
        /// <summary>
        /// 
        /// </summary>
        public String LnContNo
        {
            get { return _lnContNo; }
            set { _lnContNo = value; }
        }
        private String _description;
        /// <summary>
        /// 
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private Double _prncBal;
        /// <summary>
        /// 
        /// </summary>
        public Double PrncBal
        {
            get { return _prncBal; }
            set { _prncBal = value; }
        }

        public List<CWWhocoll> GetWhocoll(String memberNo)
        {
            List<CWWhocoll> cList = new List<CWWhocoll>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT     
                        LNCONTCOLL.LOANCONTRACT_NO, LNCONTCOLL.DESCRIPTION, 
                        LNCONTMASTER.PRINCIPAL_BALANCE
                  FROM         
                        LNCONTCOLL, LNCONTMASTER
                  WHERE     
                        LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO AND 
                        (LNCONTCOLL.REF_COLLNO <> '" + memNo + "') AND (LNCONTMASTER.MEMBER_NO = '" + memNo + "') AND (LNCONTMASTER.CONTRACT_STATUS > 0) AND (LNCONTMASTER.PRINCIPAL_BALANCE <> 0)ORDER BY LNCONTMASTER.PRINCIPAL_BALANCE DESC";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWWhocoll m = new CWWhocoll();
                m.LnContNo = dt.GetString("LOANCONTRACT_NO");
                m.Description = dt.GetString("DESCRIPTION");
                m.PrncBal = dt.GetDouble("PRINCIPAL_BALANCE");
                cList.Add(m);
            }
            return cList;
        }

    }
}
