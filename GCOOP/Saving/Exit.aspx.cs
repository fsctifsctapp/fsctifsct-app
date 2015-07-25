using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Saving
{
    public partial class Exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CommonLibrary.WebState state = new CommonLibrary.WebState();
                state.LogAct(state.SsUsername, "logout", "ออกจากระบบ", state.SsApplication, "LOGOUT");
                Session.Abandon();
                Session.RemoveAll();
            }
            catch { }
        }
    }
}
