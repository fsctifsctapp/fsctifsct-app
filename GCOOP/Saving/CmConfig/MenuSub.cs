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
using System.IO;
using System.Web.SessionState;


namespace Saving.CmConfig
{

    public class MenuSub
    {
        private String _application;

        public String Application
        {
            get { return _application; }
            set { _application = value; }
        }
        private String _pageLink;

        public String PageLink
        {
            get { return _pageLink; }
            set { _pageLink = value; }
        }
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private String _icon;
        public String Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public List<MenuSub> GetMenuSub(Object pagePermiss, Object menuGroup)
        {

            List<MenuSub> menu = new List<MenuSub>();
            try
            {
                DataTable dt = pagePermiss as DataTable;
                String groups = menuGroup.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (groups == dt.Rows[i]["GROUP_CODE"].ToString().Trim())
                    {
                        MenuSub m = new MenuSub();
                        m.Application = dt.Rows[i]["APPLICATION"].ToString().Trim();
                        m.Name = dt.Rows[i]["WIN_DESCRIPTION"].ToString();
                        m.Icon = dt.Rows[i]["ICON_PICTURE"].ToString();
                        //String aa = getSubFolder(m.Application, dt.Rows[i]["WIN_OBJECT"].ToString(), appurl.ToString());
                        //String aa = getSubFolder(m.Application, "WebForm1", appurl.ToString());

                        m.PageLink = String.Format("~/Applications/{0}/{1}.aspx?app={0}", m.Application, dt.Rows[i]["WIN_OBJECT"]);

                        //m.PageLink = String.Format("~/Applications/{0}/{1}", m.Application, getSubFolder(m.Application, dt.Rows[i]["WIN_OBJECT"].ToString()));
                        menu.Add(m);
                    }
                }
            }
            catch { }
            return menu;
        }

        private String getSubFolder(String app, String winObject)
        {

            String[] filePaths = null;
            String filename = "";
            String subfolder = "";
            try
            {
                String mappath = new System.Web.UI.Page().Server.MapPath("");
                String repath = "";
                String[] tempStr = mappath.Split('\\');
                for (int t = 0; t < tempStr.Length; t++)
                {
                    if (tempStr.GetValue(t).ToString().ToLower() == "applications")
                    {
                        break;
                    }
                    repath += tempStr.GetValue(t).ToString()+"\\";
                }

                filePaths = Directory.GetFiles(repath + "Applications\\" + app + "\\", "*.aspx", SearchOption.AllDirectories);
            }
            catch { }

            for (int i = 0; i < filePaths.Length; i++)
            {
                DirectoryInfo dif = new DirectoryInfo(filePaths.GetValue(i).ToString());
                String[] arrPath = dif.ToString().Split('\\');

                filename = dif.Name;

                if (winObject + ".aspx" == filename)
                {
                    for (int j = 0; j < arrPath.Length; j++)
                    {
                        if (arrPath.GetValue(j).ToString() == app)
                        {
                            for (int k = j + 1; k < arrPath.Length; k++)
                            {
                                try
                                {
                                    String chksplit = arrPath.GetValue(k).ToString().Split('.').GetValue(1).ToString();
                                }
                                catch
                                {
                                    subfolder += arrPath.GetValue(k).ToString() + "/";
                                }
                            }
                        }
                    }
                }
            }

            if (subfolder == "")
            {
                return winObject + ".aspx";
            }
            else
            {
                return subfolder + winObject + ".aspx";
            }
        }
    }
}
