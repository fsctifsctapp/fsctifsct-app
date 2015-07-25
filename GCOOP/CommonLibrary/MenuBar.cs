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

namespace CommonLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuBar
    {
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagePermiss"></param>
        /// <returns></returns>
        public List<MenuBar> GetMenuBar(Object pagePermiss)
        {
            List<MenuBar> menu = new List<MenuBar>();
            DataTable dt = new DataTable();
            String oldValue = "";
            String newValue = "";
            String application = "";
            try
            {
                dt = pagePermiss as DataTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    newValue = dt.Rows[i]["GROUP_CODE"].ToString().Trim();
                    if (newValue != oldValue)
                    {
                        MenuBar m = new MenuBar();
                        m.Name = dt.Rows[i]["GROUP_DESC"].ToString().Trim();
                        m.Application = dt.Rows[i]["APPLICATION"].ToString().Trim();
                        m.PageLink = String.Format("~/Default.aspx?gcode={0}&app={1}", newValue,m.Application);
                        //m.PageLink = String.Format("~/Default.aspx?gcode={0}&app={1}", newValue, dt.Rows[i]["APPLICATION"].ToString().Trim());
                        m.Picture = "";
                        menu.Add(m);
                        oldValue = newValue;
                    }
                }
            }
            catch { }
            return menu;
        }
    }
}
