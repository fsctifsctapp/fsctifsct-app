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
using CommonLibrary;

namespace Saving
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected String arg;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                arg = Request["aspxerrorpath"];
                String[] temp = arg.Split('/');
                if (temp[5].Split('.').GetValue(1).ToString().ToLower() == "aspx")
                {
                    LtError.Text = WebUtil.ErrorMessage("404 Page Not Found --> " + temp[5]);
                }
                else
                {
                    LtError.Text = WebUtil.ErrorMessage("Stack Error!");
                }
            }
            catch (Exception ex) { LtError.Text = ex.ToString(); }
        }
    }
}
