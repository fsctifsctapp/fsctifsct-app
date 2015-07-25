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
    public partial class ApplicationsControl : System.Web.UI.UserControl
    {
        private WebState state;
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if (visible)
                {
                    MenuApplications m = new MenuApplications();
                    Repeater1.DataSource = m.GetMenuApplication(state.SsWsPass);
                    Repeater1.DataBind();
                }
            }
        }

        public void LoadBegin(WebState state)
        {
            this.state = state;
            if (string.IsNullOrEmpty(state.SsApplication))
            {
                MenuApplications m = new MenuApplications();
                Repeater1.DataSource = m.GetMenuApplication(state.SsWsPass);
                Repeater1.DataBind();
            }
        }

        public void LoadEnd()
        {
            bool forceShow = false;
            if (string.IsNullOrEmpty(state.SsApplication))
            {
                this.Visible = true;
            }
            else if (forceShow)
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}