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
using CommonLibrary;

namespace Saving.CustomControl
{
    public partial class MenuBarControl : System.Web.UI.UserControl
    {
        private WebState state;

        public void LoadBegin(WebState state)
        {
            this.state = state;
        }

        public void LoadEnd()
        {
            if (state.SsPagePermiss != null)
            {
                MenuBar mb = new MenuBar();
                RepeaterMenuBar.DataSource = mb.GetMenuBar(state.SsPagePermiss);
                RepeaterMenuBar.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CssMenuBar.Visible = false;
        }
    }
}