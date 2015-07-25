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
using System.Collections.Generic;
using DBAccess;

namespace WsWebPortal
{
    public class CWApprvLoan
    {
        private String _lnReqDocNo;
        /// <summary>
        /// 
        /// </summary>
        public String LnReqDocNo
        {
            get { return _lnReqDocNo; }
            set { _lnReqDocNo = value; } 
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
        private String _lnTypeDesc;
        /// <summary>
        /// 
        /// </summary>
        public String LnTypeDesc
        {
            get { return _lnTypeDesc; }
            set { _lnTypeDesc = value; } 
        }
        private String _lnContNO;
        /// <summary>
        /// 
        /// </summary>
        public String LnContNO
        {
            get { return _lnContNO; }
            set { _lnContNO = value; } 
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
        private Double _lnAppvAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double LnAppvAmt
        {
            get { return _lnAppvAmt; }
            set { _lnAppvAmt = value; } 
        }

        private Int32 _lnReqStatus;
        /// <summary>
        /// 
        /// </summary>
        public Int32 LnReqStatus
        {
            get { return _lnReqStatus; }
            set { _lnReqStatus = value; } 
        }
        private DateTime _appvDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime AppvDate
        {
            get { return _appvDate; }
            set { _appvDate = value; }
        }

        public List<CWApprvLoan> GetApprvLoan(String memberNo)
        {
            List<CWApprvLoan> cList = new List<CWApprvLoan>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                SELECT      
                     LNREQLOAN.LOANREQUEST_DOCNO,   
                     LNREQLOAN.MEMBER_NO,   
                     LNREQLOAN.LOANTYPE_CODE,   
                     LNREQLOAN.LOANREQUEST_DATE,   
                     LNREQLOAN.LOANREQUEST_AMT,   
                     LNREQLOAN.LOANPAYMENT_TYPE,   
                     LNREQLOAN.PERIOD_PAYAMT,   
                     LNREQLOAN.PERIOD_PAYMENT,   
                     LNREQLOAN.CONTRACT_TIME,   
                     LNREQLOAN.BUYSHARE_AMT,   
                     LNREQLOAN.SUM_CLEAR,   
                     LNREQLOAN.CONTRACTINT_TYPE,   
                     LNREQLOAN.CONTRACTINT_TIME,   
                     LNREQLOAN.CONTRACT_INTEREST,   
                     LNREQLOAN.LOANOBJECTIVE_CODE,   
                     LNREQLOAN.OD_FLAG,   
                     LNREQLOAN.REMARK,   
                     LNREQLOAN.APPROVE_DATE,   
                     LNREQLOAN.APPROVE_ID,   
                     LNREQLOAN.LOANAPPROVE_AMT,   
                     LNREQLOAN.LOANCONTRACT_NO,   
                     LNREQLOAN.LOANREQUEST_STATUS,   
                     LNREQLOAN.EXPENSE_BANK,   
                     LNREQLOAN.EXPENSE_BRANCH,   
                     LNREQLOAN.EXPENSE_ACCID,   
                     LNREQLOAN.BUYSHARE_FLAG,   
                     LNREQLOAN.EXPENSE_CODE,   
                     LNREQLOAN.RECEIVEPERIOD_FLAG,   
                     LNREQLOAN.REF_REGISTERNO,    
		             LNREQLOAN.LOANTYPE_CODE,
                     LNLOANTYPE.LOANTYPE_DESC  
                FROM 
                     LNREQLOAN,      
                     LNLOANTYPE  
               WHERE
                     LNREQLOAN.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE  and  
                     LNREQLOAN.MEMBER_NO = '" + memNo + "'";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWApprvLoan m = new CWApprvLoan();
                m.LnTypeCode = dt.GetString("LOANTYPE_CODE");
                m.LnReqDocNo = dt.GetString("LOANREQUEST_DOCNO");
                m.LnReqAmt = dt.GetDouble("LOANREQUEST_AMT");
                m.LnAppvAmt = dt.GetDouble("LOANAPPROVE_AMT");
                m.LnReqStatus = dt.GetInt32("LOANREQUEST_STATUS");
                m.AppvDate = dt.GetDate("APPROVE_DATE");
                m.LnContNO = dt.GetString("LOANCONTRACT_NO");
                m.LnTypeDesc = dt.GetString("LOANTYPE_DESC");
                cList.Add(m);
            }
            return cList;
        }

    }
}
