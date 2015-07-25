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
    public class CWDeptStm
    {
        private Double _deptItemAmt;
        /// <summary>
        /// 
        /// </summary>
        public Double DeptItemAmt
        {
            get { return _deptItemAmt; }
            set { _deptItemAmt = value; }
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
        private Int32 _seqNo;
        /// <summary>
        /// 
        /// </summary>
        public Int32 SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        private String _deptTypeCode;
        /// <summary>
        /// 
        /// </summary>
        public String DeptTypeCode
        {
            get { return _deptTypeCode; }
            set { _deptTypeCode = value; }
        }
        private String _deptAccNo;
        /// <summary>
        /// 
        /// </summary>
        public String DeptAccNo
        {
            get { return _deptAccNo; }
            set { _deptAccNo = value; }
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

        public List<CWDeptStm> GetDeptStm(String deptAccNo) 
        {
            List<CWDeptStm> cList = new List<CWDeptStm>();
            String accNo = deptAccNo.Trim();             
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT     
                          DPDEPTSTATEMENT.OPERATE_DATE, DPDEPTSTATEMENT.DEPTITEM_AMT, 
                          DPDEPTSTATEMENT.PRNCBAL,DPUCFDEPTITEMTYPE.DEPTITEMTYPE_CODE, 
                          DPDEPTSTATEMENT.DEPTACCOUNT_NO, DPDEPTSTATEMENT.SEQ_NO
                  FROM         
                          DPDEPTSTATEMENT, DPUCFDEPTITEMTYPE
                  WHERE     
                          DPDEPTSTATEMENT.DEPTITEMTYPE_CODE = DPUCFDEPTITEMTYPE.DEPTITEMTYPE_CODE AND 
                          (DPDEPTSTATEMENT.DEPTACCOUNT_NO = '"+accNo+"')";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWDeptStm m = new CWDeptStm();
                m.OprDate = dt.GetDate("OPERATE_DATE");
                m.DeptItemAmt = dt.GetDouble("DEPTITEM_AMT");
                m.PrncBal = dt.GetDouble("PRNCBAL");
                m.DeptTypeCode = dt.GetString("DEPTITEMTYPE_CODE");
                m.DeptAccNo = dt.GetString("DEPTACCOUNT_NO");
                m.SeqNo = dt.GetInt32("SEQ_NO");
                cList.Add(m);
            }
            return cList;
        
        }

   
    }
}
