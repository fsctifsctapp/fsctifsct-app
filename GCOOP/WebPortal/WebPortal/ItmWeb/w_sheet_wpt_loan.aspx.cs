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
    public partial class w_sheet_wpt_loan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string member;
            member= Session["username"].ToString();
            DwLoanMem.Retrieve(member);
            DwLoan.Retrieve(member);
            try
            {
                if (Request["loancontract_no"] != null && Request["loancontract_no"].Trim() != "")
                {
                    DwLoanStm.Retrieve(Request["loancontract_no"].Trim());
                }
            }
            catch
            {
            }
        }
    }
}
