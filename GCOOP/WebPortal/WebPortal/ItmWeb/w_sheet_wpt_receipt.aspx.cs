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
using System.Drawing.Printing;
using System.Drawing;

namespace WebPortal.ItmWeb
{
    public partial class w_sheet_wpt_receipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String member;
            member = Session["username"].ToString();




        }


    }
}
