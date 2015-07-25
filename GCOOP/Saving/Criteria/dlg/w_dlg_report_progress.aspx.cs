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

namespace Saving.Applications.keeping.dlg
{
    public partial class w_dlg_report_progress : PageWebDialog, WebDialog
    {

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;

        #region WebDialog Members

        public void InitJsPostBack()
        {
            app = Request["app"].ToString();
            gid = Request["gid"].ToString();
            rid = Request["rid"].ToString();
            pdf = Request["pdf"].ToString();
        }

        public void WebDialogLoadBegin()
        {
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void WebDialogLoadEnd()
        {
            
        }

        #endregion

    }
}
