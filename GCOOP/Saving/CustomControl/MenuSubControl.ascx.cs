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
using System.Collections.Generic;

namespace Saving.CustomControl
{
    public partial class MenuSubControl : System.Web.UI.UserControl
    {
        private WebState state;
        protected String linkUrl = "";
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                divMenuSub.Visible = value;
            }
        }
        private String group;
        public String Group
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(group))
                    {
                        return state.SsMenuGroupDesc;
                    }
                    else
                    {
                        return group;
                    }
                }
                catch { return ""; }
            }
        }

        public void LoadBegin(WebState state)
        {
            this.state = state;
            linkUrl = state.SsUrl;
        }

        public void LoadEnd()
        {
            this.Visible = !String.IsNullOrEmpty(state.SsApplication) && !String.IsNullOrEmpty(state.SsUsername);
            if (this.Visible)
            {
                MenuSub ms = new MenuSub();
                try
                {
                    List<MenuSub> mss = ms.GetMenuSub(state.SsPagePermiss, state.SsMenuGroup);
                    RepeaterMenuSub.DataSource = mss;
                    RepeaterMenuSub.DataBind();
                    this.group = mss[0].Group;
                }
                catch { }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CssMenuSub.Visible = false;
        }
    }
}