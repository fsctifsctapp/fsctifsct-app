using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using DBAccess;
using System.Collections.Generic;

namespace WsWebPortal
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WptService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<CWmember> GetMember(String memberNo)
        {
            CWmember cList = new CWmember();
            return cList.GetMember(memberNo);       
        }

        [WebMethod]
        public List<CWApprvLoan> GetApprvLoan(String memberNo)
        {
            CWApprvLoan cList = new CWApprvLoan();
            return cList.GetApprvLoan(memberNo);
        }

        [WebMethod]
        public List<CWCollWho> GetCollwho(String memberNO)
        {
            CWCollWho cList = new CWCollWho();
            return cList.GetCollwho(memberNO);
        }

        [WebMethod]
        public List<CWDept> GetDept(String memberNo)
        {
            CWDept cList = new CWDept();
            return cList.GetDept(memberNo);
        }

        [WebMethod]
        public List<CWDeptStm> GetDeptStm(String deptAccNo)
        {
            CWDeptStm cList = new CWDeptStm();
            return cList.GetDeptStm(deptAccNo);
        }

        [WebMethod]
        public List<CWDivAvg> GetDivAvg(String memberNo)
        {
            CWDivAvg cList = new CWDivAvg();
            return cList.GetDivAvg(memberNo);
        }

        [WebMethod]
        public List<CWKeeping> GetKeeping(string memberNo)
        {
            CWKeeping cList = new CWKeeping();
            return cList.GetKeeping(memberNo);
        }

        [WebMethod]
        public List<CWLoan> GetLoan(String memberNo)
        {
            CWLoan cList = new CWLoan();
            return cList.GetLoan(memberNo);
        }

        [WebMethod]
        public List<CWLoanStm> GetLoanStm(String lnContNo)
        {
            CWLoanStm cList = new CWLoanStm();
            return cList.GetLoanStm(lnContNo);
        }

        [WebMethod]
        public List<CWRegister> GetRegMem(String memberNO)
        {
            CWRegister cList = new CWRegister();
            return cList.GetRegMem(memberNO);
        }

        [WebMethod]
        public Int32 InsertReg(String member, String name, String surname, String cardId, String email, String usn, String pwd)
        {
            CWRegister cList = new CWRegister();
            return cList.InsertReg(member, name, surname, cardId, email, usn, pwd);
        }

        [WebMethod]
        public List<CWRegister> GetRegUser(String memberNo)
        {
            CWRegister cList = new CWRegister();
            return cList.GetRegMem(memberNo);
        }

        [WebMethod]
        public Int32 UpdateRegUser(String member, String usn, String pwd)
        {
            CWRegister cList = new CWRegister();
            return cList.UpdateRegUser(member,usn,pwd);
        }

        [WebMethod]
        public List<CWShare> GetShare(String memberNo)
        {
            CWShare cList = new CWShare();
            return cList.GetShare(memberNo);
        }

        [WebMethod]
        public List<CWShareStm> GetShareStm(String memberNo)
        {
            CWShareStm cList = new CWShareStm();
            return cList.GetShareStm(memberNo);
        }

        [WebMethod]
        public List<CWWhocoll> GetWhocoll(String memberNo)
        {
            CWWhocoll cList = new CWWhocoll();
            return cList.GetWhocoll(memberNo);
        }

        [WebMethod]
        public String CheckMember(String memberNo,String cardPerson)
        {           
            try
            {
                String memNo = "";
                String checkMemNo = "";
                String re;
                Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                String sql = @"
                    SELECT 
                        MEMBER_NO,MEMB_NAME,MEMB_SURNAME,
                        CARD_PERSON,EMAIL_ADDRESS
                     FROM 
                        MBMEMBMASTER
                     WHERE 
                        MEMBER_NO='" + memberNo + "' AND 	CARD_PERSON= '" + cardPerson + "'";
                Sdt dt = ta.Query(sql);
                ta.Close();
                if (dt.Next())
                {
                    memNo = dt.GetString("MEMBER_NO");
                }
                else
                {
                    return re = "-1";
                }
                if(memNo != "" )
                {
                    String sqlMem = @"
                            SELECT 
                                MEMBER_NO,MEMB_NAME,MEMB_SURNAME,
                                CARD_PERSON,EMAIL_ADDRESS
                            FROM 
                                WBMEMBMASTER
                            WHERE 
                                MEMBER_NO='" + memNo + "' ";
                    dt = ta.Query(sqlMem);
                    if (dt.Next())
                    {
                        checkMemNo = dt.GetString("MEMBER_NO");
                    }
                    else
                    {
                        return re = memNo;
                    }
                    if (checkMemNo != "") return re = "0";
                    else return re = memberNo;
                }
                else
                {
                    return re = "-1";
                }

            }
            catch(Exception ex)
            {
                return ex.ToString();
            }


        }

        [WebMethod]
        public String GetLogin(String[] arr)
        {
            String password = "";
            String username = "";
            String webcode = "";
            String member = "";
            String re = "";
            try
            {
                username = arr[1].Trim();
                password = arr[2].Trim();
                if (username == "") return re = "กรุณาป้อนชื่อผู้ใช้";
                if (password == "") return re = "กรุณาป้อนรหัสผ่าน";
                Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                String sql = @"
                       select 
                          password,member_no
                       from 
                          wbmembmaster 
                       where 
                          username = '" + username + "' and password = '" + password + "'";
                Sdt dt = ta.Query(sql);
                ta.Close();
                if (dt.Next())
                {
                    webcode = dt.GetString("password").Trim();
                    member = dt.GetString("member_no").Trim();
                }
                else
                {
                    return re = "ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้อง";
                }
                if (webcode != "")
                {
                    if (password == webcode) return re = member+username;
                }
                else
                {
                    return  "ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้อง";
                }
                return "ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้อง";

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public String GetUserPassword(String memberNo, String emailAddr)
        {
            try
            {
                String memNo = memberNo.Trim();
                String email = emailAddr.Trim();
                String username = "";
                String password = "";
                String re = "";
                Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                String sql = @"
                        select 
                            member_no,email_address,username,password 
                        from 
                            wbmembmaster 
                        where member_no='"+memNo+"' and email_address='"+email+"'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    memNo = dt.GetString("member_no").Trim();
                    email = dt.GetString("email_address").Trim();
                    username = dt.GetString("username").Trim();
                    password = dt.GetString("password").Trim();
                    re = memNo +":"+ email +":"+ username +":"+ password;
                }
                else
                {
                    re = "email ไม่ถูกต้อง";
                }
                return re;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

        }

        [WebMethod]
        public String ChangePassword(String memberNO, String user, String oldpwd, String newpwd, String confpwd)
        {
            try
            {
                String memNo = memberNO.Trim();
                String username = user.Trim();
                String current = oldpwd.Trim();
                String pwdnew = newpwd.Trim();
                String confirm = confpwd.Trim();
                String pwdold = "";
                if (username == "") return "กรุณาป้อน Username ด้วย";
                if (current == "") return "กรุณาป้อน Password เดิมด้วย";
                if (pwdnew == "") return "ป้อน Password ใหม่ด้วย";
                if (confirm == "") return "ยืนยัน Password ใหม่ด้วย";
                if (pwdnew != confirm) return "Password ตัวใหม่กับที่ยืนยันไม่ตรงกัน";
                Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                String sql = @"
                        select 
                            password 
                        from 
                            wbmembmaster 
                        where 
                            member_no = '"+memNo+"' ";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    pwdold = dt.GetString("password").Trim(); 
                }
                else
                {
                    return "รหัสผ่านไม่ถูกต้อง";           
                }
                if (pwdold != "")
                {
                    if (pwdold != current) return  "ป้อน Password เดิมไม่ถูกต้อง";
                }
                String sqlUpdate = String.Format("update wbmembmaster  set password = '{0}',username='{2}' where member_no = '{1}'",pwdnew,memNo,username);
                ta.Exe(sqlUpdate);
                ta.Close();
                return  "ปรับปรุง Username และ  Password เรียบร้อยแล้ว";

            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

        }

    }
}
