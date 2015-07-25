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
using System.Collections.Generic;

namespace WebService
{
    public class DepositConstant
    {
        public String Test1 = 1.ToString();
        public String Test2 = 2.ToString();

        public List<DepositConstant> GetDepositConstant()
        {
            return new List<DepositConstant>();
        }

        public void TestLinkList()
        {
            //GetDepositConstant()[0].
            return;
        }
    }
}