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
using Sybase.DataWindow.Web;

namespace Saving.Debug
{
    public partial class debug_page : System.Web.UI.Page
    {
        protected WebState state;

        private int sessionAmt;
        private String sessionDesc;
        private String sessionName;
        private String allSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState();
            sessionAmt = Session.Count;
            for (int i = 0; i < sessionAmt; i++)
            {
                try { sessionName = Session.Keys[i].ToString(); }
                catch { sessionName = "Not Found Name"; }
                try{sessionDesc = Session[i].ToString();}
                catch { sessionDesc = "Not Found Description"; }
                allSession = allSession + "<tr><td>" + i + "</td><td>" + sessionName + "</td><td>" + sessionDesc + "</td></tr>";
            }
            LtSessionList.Text = allSession;
        }
    }
}
