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
using DBAccess;

namespace Saving.CmConfig
{
    public class AppFunction
    {
        public static String GetItemTypeGroup(Sta ta, String itemType)
        {
            try
            {
                String sql = @"
                select		DEPTITEM_GROUP
                from		DPUCFDEPTITEMTYPE
                where		DEPTITEMTYPE_CODE		= '" + itemType + "' ";
                Sdt dt = ta.Query(sql);
                if (!dt.Next())
                    throw new Exception();
                return dt.GetString("DEPTITEM_GROUP").Trim();
            }
            catch
            {
                return "";
            }
        }
    }
}
