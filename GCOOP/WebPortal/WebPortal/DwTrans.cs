using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WebPortal
{
    public class DwTrans:Sybase.DataWindow.Transaction
    {
        public DwTrans()
            : base(new System.ComponentModel.Container())
        {
            //this.components = new System.ComponentModel.Container();
            //this.SQLCA = new Sybase.DataWindow.Transaction(this.components);
            // Profile scocbtch@scoth
            this.Dbms = Sybase.DataWindow.DbmsType.Oracle10g;
            this.Password = "scocbtch";
            this.ServerName = "imm/gco";
            this.UserId = "scocbtch";
            this.AutoCommit = false;
            this.DbParameter = "PBCatalogOwner='scocbtch',TableCriteria=',scocbtch'";
        }
    }
}
