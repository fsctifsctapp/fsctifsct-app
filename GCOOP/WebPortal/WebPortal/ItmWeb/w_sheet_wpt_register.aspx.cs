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
    public partial class w_sheet_wpt_register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String member;
            member = Request["id"].ToString();
            DwRegister.Retrieve(member);
        }

        protected void regmem_Click(object sender, EventArgs e)
        {
            //WebPortal.Itm.WptService im;
            //Int32 ii;
            //im = new WebPortal.Itm.WptService();
            //ii = im.InsertReg("017067","ประจักษ์","ถามูลแสน (ก 45)","3400100054553","may", "may", "may");
            //DwRegister.InsertRow(1);

        }


    }
}
