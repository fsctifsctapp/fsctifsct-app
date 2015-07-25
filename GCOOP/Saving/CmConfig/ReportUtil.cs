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

namespace Saving.CmConfig
{
    public class ReportUtil
    {
        /// <summary>
        /// คืนค่า depttype แบบ aray 0 = min 1 = max
        /// </summary>
        /// <returns></returns>
        public static string[] GetMinMaxDepttype()
        {
            DataTable dt = WebUtil.Query("select min(depttype_code) as min, max(depttype_code) as max from dpdepttype ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxLoantype()
        {
            DataTable dt = WebUtil.Query("select min(loantype_code) as min, max(loantype_code) as max from lnloantype ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }
        public static string[] GetMinMaxMembgroup()
        {
            DataTable dt = WebUtil.Query("select min(membgroup_code) as min, max(membgroup_code) as max from mbucfmembgroup ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxRappaytype()
        {
            DataTable dt = WebUtil.Query("select min(recppaytype_code) as min, max(recppaytype_code) as max from dpucfrecppaytype ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxUsertype()
        {
            DataTable dt = WebUtil.Query("select min(user_name) as min, max(user_name) as max from amsecusers ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxMembno()
        {
            DataTable dt = WebUtil.Query("select min(member_no) as min, max(member_no) as max from mbmembmaster ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxMembsubgroup()
        {
            DataTable dt = WebUtil.Query("select min(subgroup_code) as min, max(subgroup_code) as max from mbucfmbsubgroup ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static DateTime[] GetAdtmBeginYear(string start_date)
        {
            DataTable dt = WebUtil.Query("select max(Accstart_Date) as max_begin from cmaccountyear where Accstart_Date <= to_date('" + start_date + "','dd/mm/yyyy')");
            if (dt.Rows.Count > 0)
            {
                return new DateTime[1] { Convert.ToDateTime(dt.Rows[0][0]) };

            }
            else { return null; }
        }

        public static string[] GetMinMaxMembloanobjective()
        {
            DataTable dt = WebUtil.Query("select min(loanobjective_code) as min, max(loanobjective_code) as max from lnucfloanobjective ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }

        public static string[] GetMinMaxMembentryid()
        {
            DataTable dt = WebUtil.Query("select min(user_name) as min, max(user_name) as max from amsecusers ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }
    }
}
