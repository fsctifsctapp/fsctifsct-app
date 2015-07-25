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
using DBAccess;

namespace CommonLibrary
{
    public class MenuReport
    {
        #region Property

        private String application;
        private String pageLink;
        private String groupid;
        private String groupname;

        public String Application
        {
            get { return application; }
            set { application = value; }
        }
        public String PageLink
        {
            get { return pageLink; }
            set { pageLink = value; }
        }
        public String GroupID
        {
            get { return groupid; }
            set { groupid = value; }
        }
        public String GroupName
        {
            get { return groupname; }
            set { groupname = value; }
        }

        #endregion

        private List<MenuReport> Deprecated_GetMenuReport(String appl, String connStr)
        {

            List<MenuReport> menu = new List<MenuReport>();
            try
            {
                //Sta ta = new Sta(connStr);
                String sql = "";
                if (appl.ToLower() == "ap_deposit" || appl.ToUpper() == "ap_deposit")
                {
                    sql = @"select * from cmreportgroup where 
                        application=UPPER('" + appl + @"') 
                        or application=LOWER('" + appl + "')";
                }
                else
                {
                    sql = @"SELECT CMAPPLREPORT.APPLICATION,   
                         CMAPPLREPORT.GROUP_ID,   
                         CMAPPLREPORT.GROUP_NAME  
                         FROM CMAPPLREPORT  
                         WHERE cmapplreport.application = '" + appl + "'";
                }
                DataTable dt = WebUtil.Query(sql); //ta.Query(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MenuReport m = new MenuReport();
                    m.Application = dt.Rows[i]["APPLICATION"].ToString().Trim();
                    m.GroupID = dt.Rows[i]["GROUP_ID"].ToString();
                    m.GroupName = dt.Rows[i]["GROUP_NAME"].ToString();
                    m.PageLink = String.Format("~/ReportDefault.aspx?gid={0}", dt.Rows[i]["GROUP_ID"]);
                    menu.Add(m);
                }
            }
            catch (Exception ex)
            {
                String strEl = ex.ToString();
                //error จะให้ทำยังไง ?
            }
            return menu;
        }

        public List<MenuReport> GetMenuReport(String appl, String connStr)
        {
            List<MenuReport> menu = new List<MenuReport>();
            try
            {
                //Sta ta = new Sta(connStr);
                String sql = "";
                sql = @"select application, group_id, group_name from webreportgroup where 
                    used_flag = 1 and
                    ( application=UPPER('" + appl + @"') 
                    or application=LOWER('" + appl + @"') )
                    order by group_order";
                DataTable dt = WebUtil.Query(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MenuReport m = new MenuReport();
                    m.Application = dt.Rows[i]["APPLICATION"].ToString().Trim();
                    m.GroupID = dt.Rows[i]["GROUP_ID"].ToString();
                    m.GroupName = dt.Rows[i]["GROUP_NAME"].ToString();
                    m.PageLink = String.Format("~/ReportDefault.aspx?app={1}&gid={0}", dt.Rows[i]["GROUP_ID"], appl);
                    menu.Add(m);
                }
            }
            catch (Exception ex)
            {
                String strEl = ex.ToString();
                //error จะให้ทำยังไง ?
            }
            return menu;
        }
    }
}
