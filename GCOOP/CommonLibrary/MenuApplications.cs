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
using CommonLibrary.WsCommon;

namespace CommonLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuApplications
    {
        private String _trStart;
        public String TrStart
        {
            get { return _trStart; }
            set { _trStart = value; }
        }

        private String _trEnd;
        public String TrEnd
        {
            get { return _trEnd; }
            set { _trEnd = value; }
        }

        private String _workDate;
        public String WorkDate
        {
            get { return _workDate; }
            set { _workDate = value; }
        }

        private String _pageLink;
        /// <summary>
        /// 
        /// </summary>
        public String PageLink
        {
            get { return _pageLink; }
            set { _pageLink = value; }
        }

        private String _picture;
        /// <summary>
        /// 
        /// </summary>
        public String Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private String _pictureCss;
        public String PictureCss
        {
            get { return _pictureCss; }
            set { _pictureCss = value; }
        }

        private String _name;
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _application;
        /// <summary>
        /// 
        /// </summary>
        public String Application
        {
            get { return _application; }
            set { _application = value; }
        }

        private String _csType;
        public String CsType
        {
            get { return _csType; }
            set { _csType = value; }
        }

        private String _csDesc;
        public String CsDesc
        {
            get { return _csDesc; }
            set { _csDesc = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagePermiss"></param>
        /// <returns></returns>
        public List<MenuApplications> GetMenuApplication(Object wsPass)
        {
            List<MenuApplications> menu = new List<MenuApplications>();
            int rows = 0;
            DataTable dt;
            try
            {
                String newPass = (String)wsPass;
                Common cmd = WsUtil.Common;
                dt = cmd.GetStatusApplicationData(newPass);
                rows = dt.Rows.Count;
            }
            catch { dt = null; }
            for (int i = 0; i < 18; i++)
            {
                MenuApplications m = new MenuApplications();
                if (i % 6 == 0)
                {
                    m.TrStart = "<tr>";
                }
                else
                {
                    m.TrStart = "";
                }
                m.TrEnd = i == 5 || i == 11 || i == 17 ? "</tr>" : "";
                try
                {
                    if (dt == null) throw new Exception();
                    if (rows < i) throw new Exception();
                    bool isClose = true;
                    string stat = "";
                    try
                    {
                        isClose = Convert.ToInt32(dt.Rows[i]["closeday_status"]) == 1;
                    }
                    catch { isClose = true; }
                    stat = isClose ? "<font color=\"red\">[ปิด]</font>" : "<font color=\"#33CC33\">[เปิด]</font>";
                    m.WorkDate = "วันที่: " + Convert.ToDateTime(dt.Rows[i]["workdate"]).ToString("dd/MM/yyyy", WebUtil.TH) + " " + stat;
                    m.Name = dt.Rows[i]["description"].ToString();
                    m.Application = dt.Rows[i]["application"].ToString();
                    m.Picture = "~/img/applications/" + (i+1) + ".png";
                    m.PictureCss = "imApplication";
                    m.CsType = dt.Rows[i]["cs_type"].ToString();
                    m.CsDesc = dt.Rows[i]["description"].ToString();
                }
                catch
                {
                    m.WorkDate = "";
                    m.Name = "";
                    m.Application = "";
                    m.PageLink = "";
                    m.Picture = "~/img/white.gif";
                    m.PictureCss = "imApplicationNone";
                    m.CsType = "";
                    m.CsDesc = "";
                }
                menu.Add(m);
            }
            return menu;
        }
    }
}
