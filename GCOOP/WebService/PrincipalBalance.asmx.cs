using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using WebService.Processing;
using pbservice;

namespace WebService
{
    /// <summary>
    /// Summary description for PrincipalBalance
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PrincipalBalance : System.Web.Services.WebService
    {

        [WebMethod]
        public int ItemCount(String wsPass, DateTime operateDate)
        {
            Security sec = new Security(wsPass);
            n_cst_principalbalance_new service = new n_cst_principalbalance_new();
            n_cst_dbconnectservice svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(sec.ConnectionString);
            service.of_settrans(svCon);
            int rv = service.of_count(operateDate);
            return rv;
        }

        [WebMethod]
        public string Run(String wsPass, DateTime operateDate)
        {
            String app = "shrlon";
            String w_sheet_id = "w_sheet_sl_principal_balance";
            try
            {
                Processing.Progressing.Remove(app, w_sheet_id);
            }
            catch
            {
            }
            Security sec = new Security(wsPass);
            PrincipalBalanceProcess pbp = new PrincipalBalanceProcess(sec.ConnectionString, operateDate);
            Processing.Progressing.Add(pbp, app, w_sheet_id);
            return "true";
        }

        [WebMethod]
        public String GetStatus(String wspass)
        {
            Security sec = new Security(wspass);
            string[] s = Progressing.GetStatus("shrlon", "w_sheet_sl_principal_balance");
            string ss = "{0},{1},{2},{3},{4},{5},{6},{7}"; //ห้ามคั่นด้วยคอมม่าเพราะจะตัดผิดเมื่อมีข้อความ error โผล่มา.
            String result = String.Format(ss, s);
            if (result.StartsWith("1"))
            {
                Processing.Progressing.Remove("shrlon", "w_sheet_sl_principal_balance");
            }
            return result;
        }

        [WebMethod]
        public String Stop(String wsPass, String app, String w_sheet_id)
        {
            Processing.Progressing.Remove(app, w_sheet_id);
            return "หยุดการประมวลผลเรียบร้อยแล้ว";
        }

    }
}
