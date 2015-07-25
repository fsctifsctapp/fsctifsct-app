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
    public class CWLoanStm
    {
        private String _lnContNo;
        /// <summary>
        /// 
        /// </summary>
        public String LncontNo
        {
            get { return _lnContNo; }
            set { _lnContNo = value; }
        }
        private String _branchId;
        /// <summary>
        /// 
        /// </summary>
        public String BranchId
        {
            get { return _branchId; }
            set { _branchId = value; }
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
        private String _refDocNo;
        /// <summary>
        /// 
        /// </summary>
        public String RefDocNo
        {
            get { return _refDocNo; }
            set { _refDocNo = value; }
        }         
        private String _moneyTypeCode;
        /// <summary>
        /// 
        /// </summary>
        public String MoneyTypeCode
        {
            get { return _moneyTypeCode; }
            set { _moneyTypeCode = value; }
        }          
        private String _entryId;
        /// <summary>
        /// 
        /// </summary>
        public String EntryId
        {
            get { return _entryId; }
            set { _entryId = value; }
        }         
        private Int32 _seqNO;
        /// <summary>
        /// 
        /// </summary>
        public Int32 SeqNO
        {
            get { return _seqNO; }
            set { _seqNO = value; }
        }    
        private Int32 _period;
        /// <summary>
        /// 
        /// </summary>
        public Int32 Period
        {
            get { return _period; }
            set { _period = value; }
        } 
        private Int32 _itemStatus;
        /// <summary>
        /// 
        /// </summary>
        public Int32 ItemStatus
        {
            get { return _itemStatus; }
            set { _itemStatus = value; }
        }
        private DateTime _slipDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime SlipDate
        {
            get { return _slipDate; }
            set { _slipDate = value; }
        }
        private DateTime _oprDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime OprDate
        {
            get { return _oprDate; }
            set { _oprDate = value; }
        }
        private DateTime _calintForm;
        /// <summary>
        /// 
        /// </summary>
        public DateTime CalintForm
        {
            get { return _calintForm; }
            set { _calintForm = value; }
        }
        private DateTime _calintTo;
        /// <summary>
        /// 
        /// </summary>
        public DateTime CalintTo
        {
            get { return _calintTo; }
            set { _calintTo = value; }
        }
        private DateTime _entryDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime EntryDate
        {
            get { return _entryDate; }
            set { _entryDate = value; }
        }
        private Double _prncPayment;
        /// <summary>
        /// 
        /// </summary>
        public Double PrncPayment
        {
            get { return _prncPayment; }
            set { _prncPayment = value; }
        }
        private Double _interestPayment;
        /// <summary>
        /// 
        /// </summary>
        public Double InterestPayment
        {
            get { return _interestPayment; }
            set { _interestPayment = value; }
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
        private Double _bfintarrearAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double BfintarrearAmt
        {
            get { return _bfintarrearAmt; }
            set { _bfintarrearAmt = value; }
        }
        private Double _interestPeriod;
        /// <summary>
        /// 
        /// </summary>
        public Double InterestPeriod
        {
            get { return _interestPeriod; }
            set { _interestPeriod = value; }
        }
        private Double _interestArrear;
        /// <summary>
        /// 
        /// </summary>
        public Double InterestArrear
        {
            get { return _interestArrear; }
            set { _interestArrear = value; }
        }
        private Double _interestReturn;
        /// <summary>
        /// 
        /// </summary>
        public Double InterestReturn
        {
            get { return _interestReturn; }
            set { _interestReturn = value; }
        }

        public List<CWLoanStm> GetLoanStm(String lnContNo)
        {
            List<CWLoanStm> cList = new List<CWLoanStm>();
            String contNo = lnContNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
               SELECT     
                      LOANCONTRACT_NO, COOPBRANCH_ID, SEQ_NO, 
                      LOANITEMTYPE_CODE, SLIP_DATE, OPERATE_DATE,
                      REF_DOCNO, PERIOD,PRINCIPAL_PAYMENT, 
                      INTEREST_PAYMENT, PRINCIPAL_BALANCE, 
                      CALINT_FROM, CALINT_TO, BFINTARREAR_AMT, 
                      INTEREST_PERIOD, INTEREST_ARREAR, 
                      INTEREST_RETURN, MONEYTYPE_CODE, 
                      ITEM_STATUS, ENTRY_ID, ENTRY_DATE
                FROM         
                      LNCONTSTATEMENT
                WHERE 
                      (LOANCONTRACT_NO = '" + contNo + "')";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWLoanStm m = new CWLoanStm();
                m.LncontNo = dt.GetString("LOANCONTRACT_NO");
                m.BranchId = dt.GetString("COOPBRANCH_ID");
                m.SeqNO = dt.GetInt32("SEQ_NO");
                m.LnTypeCode = dt.GetString("LOANITEMTYPE_CODE");
                m.SlipDate = dt.GetDate("SLIP_DATE");
                m.OprDate = dt.GetDate("OPERATE_DATE");
                m.RefDocNo = dt.GetString("REF_DOCNO");
                m.Period = dt.GetInt32("PERIOD");
                m.PrncPayment = dt.GetDouble("PRINCIPAL_PAYMENT");
                m.PrncBal= dt.GetDouble("PRINCIPAL_BALANCE");
                m.CalintForm = dt.GetDate("CALINT_FROM");
                m.CalintTo = dt.GetDate("CALINT_TO");
                m.BfintarrearAmt = dt.GetDouble("BFINTARREAR_AMT");
                m.InterestPeriod= dt.GetDouble("INTEREST_PERIOD");
                m.InterestArrear = dt.GetDouble("INTEREST_ARREAR");
                m.InterestReturn = dt.GetDouble("INTEREST_RETURN");
                m.MoneyTypeCode = dt.GetString("MONEYTYPE_CODE");
                m.EntryId = dt.GetString("ENTRY_ID");
                m.EntryDate = dt.GetDate("ENTRY_DATE");
                cList.Add(m);
            }
            return cList;
        }

    }
}
