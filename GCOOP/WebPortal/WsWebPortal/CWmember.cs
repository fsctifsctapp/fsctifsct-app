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
    public class CWmember
    {

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
        public String MembSurname
        {
            get { return _membSurname; }
            set { _membSurname = value; } 
        }
        private String _memberNo;
        public String MemberNo
        {
            get { return _memberNo; }
            set { _memberNo = value; }
        }
        private DateTime _memberDate;
        public DateTime MemberDate
        {
            get { return _memberDate; }
            set { _memberDate = value; }
        }
        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public List<CWmember> GetMember(String memberNo)
        {
            List<CWmember> cList = new List<CWmember>();
            String memNo = memberNo.Trim();             
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
            SELECT     
                MEMB_NAME, MEMB_SURNAME, 
                MEMBER_DATE, MEMBER_NO, 
                BIRTH_DATE
            FROM         
                MBMEMBMASTER
            WHERE     
                MEMBER_NO = '"+memNo+"'";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWmember m = new CWmember();
                m.MembName = dt.GetString("MEMB_NAME");
                m.MembSurname = dt.GetString("MEMB_SURNAME");
                m.MemberDate = dt.GetDate("MEMBER_DATE");
                m.MemberNo = dt.GetString("MEMBER_NO");
                m.BirthDate = dt.GetDate("BIRTH_DATE");
                cList.Add(m);
            }
            return cList;
        }
        
         
          
    }
}
