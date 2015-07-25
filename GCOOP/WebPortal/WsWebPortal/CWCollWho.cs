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
    public class CWCollWho
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
        private String _membName;
        /// <summary>
        /// 
        /// </summary>
        public String MembName
        {
            get { return _membName; }
            set { _membName = value; }
        }
        private String _membSurname;
        /// <summary>
        /// 
        /// </summary>
        public String MembSurname
        {
            get { return _membSurname; }
            set { _membSurname = value; }
        } 
        private Double _princBal;
        /// <summary>
        /// 
        /// </summary>
        public Double _PrincBal
        {
            get { return _princBal; }
            set { _princBal = value; }
        }

        public List<CWCollWho> GetCollwho(String memberNo)
        {
            List<CWCollWho> cList = new List<CWCollWho>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                SELECT     
                    LNCONTCOLL.LOANCONTRACT_NO, LNCONTCOLL.DESCRIPTION, 
                    LNCONTMASTER.PRINCIPAL_BALANCE, MBMEMBMASTER.MEMB_NAME, 
                    MBMEMBMASTER.MEMB_SURNAME
                FROM         
                    LNCONTCOLL, LNCONTMASTER,MBMEMBMASTER
                WHERE     
                    LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO AND 
                    LNCONTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO AND 
                    LNCONTCOLL.COLL_STATUS = 1 AND LNCONTMASTER.CONTRACT_STATUS = 1 AND
                    LNCONTCOLL.REF_COLLNO = '" + memNo+"' ORDER BY LNCONTMASTER.PRINCIPAL_BALANCE DESC";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWCollWho m = new CWCollWho();
                m.LnContNo = dt.GetString("LOANCONTRACT_NO");
                m.Description = dt.GetString("DESCRIPTION");
                m.MembName = dt.GetString("MEMB_NAME");
                m.MembSurname = dt.GetString("MEMB_SURNAME");
                cList.Add(m);
            }
            return cList;
        }

    }
}
