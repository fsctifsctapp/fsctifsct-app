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
    public class CWRegister
    {
        private String _memberNo;
        /// <summary>
        /// 
        /// </summary>
        public String MemberNo{
            get { return _memberNo; }
            set { _memberNo = value; }
        }
        private String _membName;
        /// <summary>
        /// 
        /// </summary>
        public String MembName{
            get { return _membName; }
            set { _membName = value; }
        }
        private String _membSurname;
        /// <summary>
        /// 
        /// </summary>
        public String MembSurname{
            get { return _membSurname; }
            set { _membSurname = value; }
        }
        private String _cardPerson;
        /// <summary>
        /// 
        /// </summary>
        public String CardPerson{
            get { return _cardPerson; }
            set { _cardPerson = value; }
        }
        private String _userName;
        /// <summary>
        /// 
        /// </summary>
        public String UserName{
            get { return _userName; }
            set { _userName = value; }
        }
        private String _password;
        /// <summary>
        /// 
        /// </summary>
        public String Password{
            get { return _password; }
            set { _password = value; }
        }

        private String _emailAddress;
        /// <summary>
        /// 
        /// </summary>
        public String EmailAddress{
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
        private String _confPass;
        /// <summary>
        /// 
        /// </summary>
        public String ConfPass{
            get { return _confPass; }
            set { _confPass = value; }
        }
        private String _oldPass;
        /// <summary>
        /// 
        /// </summary>
        public String OldPass{
            get { return _oldPass; }
            set { _oldPass = value; }
        }        
        private String _newPass;
        /// <summary>
        /// 
        /// </summary>
        public String NewPass{
            get { return _newPass; }
            set { _newPass = value; }
        }

        public List<CWRegister> GetRegMem(String memberNo)
        {
            List<CWRegister> cList = new List<CWRegister>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT 
                        MEMBER_NO,MEMB_NAME,
                        MEMB_SURNAME,CARD_PERSON,
                        EMAIL_ADDRESS
                  FROM 
                        MBMEMBMASTER
                  WHERE 
                        MEMBER_NO='" + memNo + "'";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWRegister m = new CWRegister();
                m.MembName = dt.GetString("MEMB_NAME");
                m.MemberNo = dt.GetString("MEMBER_NO");
                m.MembSurname = dt.GetString("MEMB_SURNAME");
                m.CardPerson = dt.GetString("CARD_PERSON");
                m.EmailAddress = dt.GetString("EMAIL_ADDRESS");
                cList.Add(m);
            }
            return cList;
        }
        public Int32 InsertReg(String member,String name,String surname,String cardId,String email,String usn,String pwd)
        {

            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                    INSERT INTO 
                        WBMEMBMASTER(MEMBER_NO,MEMB_NAME,MEMB_SURNAME,CARD_PERSON,EMAIL_ADDRESS,USERNAME,PASSWORD) 
                    VALUES 
                        ('" + member + "','" + name + "','" + surname + "','" + cardId + "','" + email + "','" + usn + "','" + pwd + "')";
            Int32 ii = ta.Exe(sql);
            ta.Close();
            return ii;
        }
        public List<CWRegister> GetRegUser(String memberNo)
        {
            List<CWRegister> cList = new List<CWRegister>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT 
                        USERNAME,PASSWORD,EMAIL_ADDRESS
                  FROM 
                        WBMEMBMASTER
                  WHERE 
                        MEMBER_NO='" + memNo + "'";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWRegister m = new CWRegister();
                m.UserName = dt.GetString("USERNAME").Trim();
                m.Password = dt.GetString("PASSWORD").Trim();
                m.EmailAddress = dt.GetString("EMAIL_ADDRESS").Trim();
                cList.Add(m);
            }
            return cList;
        }
        public Int32 UpdateRegUser(String member, String usn, String pwd)
        {
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql= @"UPDATE WBMEMBMASTER SET USERNAME='"+usn+"',PASSWORD= '"+pwd+"'WHERE MEMBER_NO = '"+member+"'";
            Int32 ii= ta.Exe(sql);
            ta.Close();
            return  ii;
        }

    }
}
