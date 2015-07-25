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
    public class CWLoan
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
        private String _lnTypeCode;
        /// <summary>
        /// 
        /// </summary>
        public String LnTypeCode
        {
            get { return _lnTypeCode; }
            set { _lnTypeCode = value; }
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
        private Double _periodPayment;
        /// <summary>
        /// 
        /// </summary>
        public Double PeriodPayment
        {
            get { return _periodPayment; }
            set { _periodPayment = value; }
        }
        private Double _lnReqAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double LnReqAmt
        {
            get { return _lnReqAmt; }
            set { _lnReqAmt = value; }
        }   
        private DateTime _startContDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartContDate
        {
            get { return _startContDate; }
            set { _startContDate = value; }
        }

        public List<CWLoan> GetLoan(String memberNo)
        {
            List<CWLoan> cList = new List<CWLoan>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT     
                      LNCONTMASTER.LOANCONTRACT_NO, LNCONTMASTER.PRINCIPAL_BALANCE, 
                      LNCONTMASTER.PERIOD_PAYAMT,LNCONTMASTER.LOANREQUEST_AMT, 
                      LNCONTMASTER.STARTCONT_DATE, LNLOANTYPE.LOANTYPE_CODE
                  FROM         
                      LNCONTMASTER, LNLOANTYPE
                  WHERE     
                      LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE AND 
                      (LNCONTMASTER.MEMBER_NO = '"+memNo+"') AND (LNCONTMASTER.CONTRACT_STATUS > 0) AND (LNCONTMASTER.PRINCIPAL_BALANCE > 0)";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWLoan m = new CWLoan();
                m.LnContNo = dt.GetString("LOANCONTRACT_NO");
                m.PrncBal = dt.GetDouble("PRINCIPAL_BALANCE");
                m.PeriodPayment = dt.GetDouble("PERIOD_PAYAMT");
                m.LnReqAmt = dt.GetDouble("LOANREQUEST_AMT");
                m.StartContDate = dt.GetDate("STARTCONT_DATE");
                m.LnTypeCode = dt.GetString("LOANTYPE_CODE");
                cList.Add(m);
            }
            return cList;
        }
    }
}
