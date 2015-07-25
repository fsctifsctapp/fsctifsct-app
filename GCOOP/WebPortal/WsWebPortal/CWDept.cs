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
    public class CWDept
    {
        private String _deptAccNo;
        /// <summary>
        /// 
        /// </summary>
        public String DeptAccNo
        {
            get { return _deptAccNo; }
            set { _deptAccNo = value; }
        }
        private String _deptAccName;
        /// <summary>
        /// 
        /// </summary>
        public String DeptAccName
        {
            get { return _deptAccName; }
            set { _deptAccName = value; }
        }
        private String _deptTypeDesc;
        /// <summary>
        /// 
        /// </summary>
        public String DeptTypeDesc
        {
            get { return _deptTypeDesc; }
            set { _deptTypeDesc = value; }
        }            
        private Double _prncbal;
        /// <summary>
        /// 
        /// </summary>
        public Double Prncbal
        {
            get { return _prncbal; }
            set { _prncbal = value; }
        }

        public List<CWDept> GetDept(String memberNo)
        {
            List<CWDept> cList = new List<CWDept>();
            String memNo = memberNo.Trim();             
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT    
                         DPDEPTMASTER.DEPTACCOUNT_NO, DPDEPTMASTER.DEPTACCOUNT_NAME, 
                         DPDEPTMASTER.PRNCBAL, DPDEPTTYPE.DEPTTYPE_DESC
                  FROM       
                         DPDEPTMASTER, DPDEPTTYPE
                  WHERE      
                         DPDEPTMASTER.DEPTTYPE_CODE = DPDEPTTYPE.DEPTTYPE_CODE AND (
                         DPDEPTMASTER.MEMBER_NO = '"+memNo+"')";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWDept m = new CWDept();
                m.DeptAccNo = dt.GetString("DEPTACCOUNT_NO");
                m.DeptAccName = dt.GetString("DEPTACCOUNT_NAME");
                m.Prncbal = dt.GetDouble("PRNCBAL");
                m.DeptTypeDesc = dt.GetString("DEPTTYPE_DESC");
                cList.Add(m);
            }
            return cList;
        }
        

    }
}
