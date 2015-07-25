using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CommonLibrary
{
    public enum ConnectMode
    {
        /// <summary>
        /// การติดต่อฐานข้อมูล Auto คือการรับ Connection String จาก WebService
        /// </summary>
        Auto = 1,
        /// <summary>
        /// การติดต่อฐานข้อมูล Manual คือการรับ Connection String จากการคีย์มือ
        /// </summary>
        Manual = 0,
    }
}
