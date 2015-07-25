using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace WebPortal.ItmWeb
{
    public partial class w_sheet_wpt_deposit : System.Web.UI.Page
    {
        //private DwTrans sqlca;
        protected void Page_Load(object sender, EventArgs e)
        {
            String member;
            member = Session["username"].ToString();
            //sqlca = new DwTrans();
            //sqlca.Connect();
           // DwDeptMem.SetTransaction(sqlca);
            DwDeptMem.Retrieve(member);
            //DwDept.SetTransaction(sqlca);
            DwDept.Retrieve(member);

            try
            {
                if (Request["deptaccount_no"] != null && Request["deptaccount_no"].Trim() != "")
                {
                    DwDeptStm.Retrieve(Request["deptaccount_no"].Trim());
                }
                //DwDeptStm.SetTransaction(sqlca);
                //DwDeptStm.Retrieve("0102002829");
            }
            catch
            {
            }
            //sqlca.Disconnect();

        }


    }
}
