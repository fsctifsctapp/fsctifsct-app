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
    public class MenuSubReport
    {
        private String application;
        private String groupid;
        private String reportid;
        private String reportname;
        private String criteriaobject;
        private String creatorobject;
        private String datawindowobject;
        private String reporttype;
        private String pageLink;
        
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
        public String GroupId
        {
            get { return groupid; }
            set { groupid = value; }
        }
        public String ReportId
        {
            get { return reportid; }
            set { reportid = value; }
        }
        public String ReportName
        {
            get { return reportname; }
            set { reportname = value; }
        }
        public String CriteriaObject
        {
            get { return criteriaobject; }
            set { criteriaobject = value; }
        }
        public String CreatorObject
        {
            get { return creatorobject; }
            set { creatorobject = value; }
        }
        public String DatawindowObject
        {
            get { return datawindowobject; }
            set { datawindowobject = value; }
        }
        public String ReportType
        {
            get { return reporttype; }
            set { reporttype = value; }
        }

        private List<MenuSubReport> Deprecated_GetMenuSubReport(String appl, String gid, String connStr)
        {

            List<MenuSubReport> menu = new List<MenuSubReport>();
            try
            {
                String sql = "";
                if (appl.ToLower() == "ap_deposit" || appl.ToUpper() == "ap_deposit")
                {
                    sql = @"select GROUP_ID,REPORT_ID,
                        REPORT_NAME,UO_CLASSNAME,REPORT_TYPE from cmreportdetail 
                        where group_id='" + gid + "' and used_flag=1";
                }
                else
                {
                    sql = @"  SELECT CMAPPLREPORTDET.APPLICATION,   
                             CMAPPLREPORTDET.GROUP_ID,   
                             CMAPPLREPORTDET.REPORT_ID,   
                             CMAPPLREPORTDET.REPORT_NAME,   
                             CMAPPLREPORTDET.UO_OBJECT,   
                             CMAPPLREPORTDET.REPORT_TYPE  
                             FROM CMAPPLREPORTDET  
                             WHERE ( cmapplreportdet.application = '"+appl+@"' ) AND  
                             ( cmapplreportdet.group_id ='" + gid + "')    ";
                }

                DataTable dt = WebUtil.Query(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MenuSubReport m = new MenuSubReport();
                    try{m.Application = dt.Rows[i]["APPLICATION"].ToString().Trim();}
                    catch { m.Application = appl; }
                    m.GroupId = dt.Rows[i]["GROUP_ID"].ToString();
                    m.ReportId = dt.Rows[i]["REPORT_ID"].ToString();
                    m.ReportName = dt.Rows[i]["REPORT_NAME"].ToString();
                    m.ReportType = dt.Rows[i]["REPORT_TYPE"].ToString();
                    try 
                    {
                        m.CriteriaObject = dt.Rows[i]["REPORT_CRIOBJECT"].ToString();
                        //m.PageLink = String.Format("~/Applications/{0}/report/{1}.aspx", appl, dt.Rows[i]["UO_OBJECT"].ToString());
                        m.PageLink = String.Format("ReportDefault.aspx?app={0}&uoobj={1}", appl, dt.Rows[i]["REPORT_CRIOBJECT"].ToString());
                    
                    }
                    catch 
                    {
                        m.CriteriaObject = dt.Rows[i]["UO_CLASSNAME"].ToString();
                        //m.PageLink = String.Format("~/Applications/{0}/report/{1}.aspx", appl, dt.Rows[i]["UO_CLASSNAME"].ToString());
                        m.PageLink = String.Format("ReportDefault.aspx?app={0}&uoobj={1}", appl, dt.Rows[i]["UO_CLASSNAME"].ToString());
                    }
                     
                    
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

        public List<MenuSubReport> GetMenuSubReport(String appl, String gid, String connStr)
        {

            List<MenuSubReport> menu = new List<MenuSubReport>();
            try
            {
                String sql = "";
                sql = @"SELECT REPORT_ID,REPORT_NAME,REPORT_CRIOBJECT  
                    FROM WEBREPORTDETAIL  
                    WHERE ( GROUP_ID = '" + gid + @"' ) 
                    AND ( USED_FLAG = 1 )
                    ORDER BY REPORT_ID, REPORT_NAME";
                DataTable dt = WebUtil.Query(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MenuSubReport m = new MenuSubReport();
                    m.Application = appl;
                    m.GroupId = gid;
                    m.ReportId = dt.Rows[i]["REPORT_ID"].ToString();
                    m.ReportName = dt.Rows[i]["REPORT_NAME"].ToString();
                    m.CriteriaObject = dt.Rows[i]["REPORT_CRIOBJECT"].ToString();
                    m.PageLink = String.Format("Criteria/{0}.aspx?app={1}&gid={2}&rid={3}", m.CriteriaObject, m.Application, m.GroupId, m.ReportId);
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
