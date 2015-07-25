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
using System.Text;
using Sybase.DataWindow.Web;

namespace Saving.CmConfig
{
    public class PageWebDialog : Page
    {
        protected WebState state;
        private List<DwThDate> tDwDates;
        protected DwTrans sqlca;
        private WebDialog webDialog;

        protected void ConnectSQLCA()
        {
            if (sqlca != null)
            {
                try
                {
                    sqlca.Disconnect();
                }
                catch { }
            }
            try
            {
                sqlca = new DwTrans();
                sqlca.Connect();
            }
            catch { }
        }

        protected void DisConnectSQLCA()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tDwDates = new List<DwThDate>();
            state = new WebState();
            webDialog = (WebDialog)this;

            //เริ่มต้นการทำงานของ WebDialog
            webDialog.InitJsPostBack();
            webDialog.WebDialogLoadBegin();
            if (IsPostBack)
            {
                String eventArg = "";
                try
                {
                    eventArg = Request["__EVENTARGUMENT"];
                    //eventArg = eventArg.Replace(",", "").Trim();
                }
                catch { }
                webDialog.CheckJsPostBack(eventArg);
            }
            //w_dlg_xx_xxxxxx
            //webPageType
            String url = String.Format("http://{0}/GCOOP/Saving/", Request.Url.Authority);

            String onLoad = "";
            onLoad += "<script type=\"text/javascript\" src=\"" + url + "js/DetectBrowser.js\"></script>\n";
            onLoad += "<script type=\"text/javascript\" src=\"" + url + "js/WebState.js\"></script>\n";
            onLoad += "<script type=\"text/javascript\" src=\"" + url + "js/Gcoop.js\"></script>\n";
            onLoad += "<script type=\"text/javascript\" src=\"" + url + "js/js.js\"></script>\n";
            onLoad += "<script type=\"text/javascript\">window.onload = function(){ Page_LoadComplete(); }</script>\n";
            onLoad += "<input id='webPageType' type='hidden' value='w_dlg_xx_xxxxxx' />";
            onLoad += SetDwThDateJavaScriptEvent();

            Type t = this.GetType();
            ClientScript.RegisterClientScriptBlock(t, "DsScript", onLoad);

            String lastFocus = "";
            try
            {
                lastFocus = Request["tempLastFocus"];
            }
            catch { }
            String dsTempElementEnter = "\n<!-- Doys แทรก Script เพื่อ  //-->\n";
            dsTempElementEnter += "<input type='text' id='tempElementEnter' name='tempElementEnter' value='fel' style='width:1px;height:1px;border:none;background-color:white;' />\n";
            dsTempElementEnter += "<input type='hidden' id='tempLastFocus' name='tempLastFocus' value='" + lastFocus + "' />\n\n";
            ClientScript.RegisterClientScriptBlock(t, "DsTempElementEnter", dsTempElementEnter);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                webDialog.WebDialogLoadEnd();
            }
            catch (Exception ex)
            {
                this.DisConnectSQLCA();
                throw ex;
            }
            this.DisConnectSQLCA();
        }

        public void AddThDate(DwThDate tDwDate)
        {
            tDwDates.Add(tDwDate);
        }

        protected int RestoreContextDw(WebDataWindowControl dwDeBug)
        {
            int first = 0;
            int last = 0;
            first = dwDeBug.RowCount;
            dwDeBug.RestoreContext();
            last = dwDeBug.RowCount;
            int ii = 0;
            while (last > first)
            {
                dwDeBug.DeleteRow(dwDeBug.RowCount);
                last--;
                ii++;
                if (ii > 10000)
                {
                    break;
                }
            }
            return dwDeBug.RowCount;
        }

        public String SetDwThDateJavaScriptEvent()
        {
            // ตั้งคา Javascript
            StringBuilder varColumnThDate = new StringBuilder("<script> \n\t");
            varColumnThDate.Append("var thDwColumnNameArray = new Array(); \n\t");
            varColumnThDate.Append("var enDwColumnNameArray = new Array(); \n\t");
            varColumnThDate.Append("var dwObjectJavaScriptArray = new Array(); \n\t");
            varColumnThDate.Append("var dwColumnNameArrayCount = 0;\n\t");

            int iii = 0;
            varColumnThDate.Append("function SetThDateJavaScript01(){ \n\t");
            for (int i = 0; i < tDwDates.Count; i++)
            {
                DwThDate dwTh = tDwDates[i];
                for (int j = 0; j < dwTh.Count; j++)
                {
                    WebDataWindowControl dwMain = dwTh.DwMain;
                    for (int k = 0; k < dwMain.RowCount; k++)
                    {
                        varColumnThDate.Append("thDwColumnNameArray[" + iii + "] = '" + dwTh.ThName[j] + "_" + k + "'; \n\t");
                        varColumnThDate.Append("enDwColumnNameArray[" + iii + "] = '" + dwTh.OriName[j] + "_" + k + "'; \n\t");
                        varColumnThDate.Append("dwObjectJavaScriptArray[" + iii + "] = obj" + dwTh.ID + "; \n\t");
                        iii++;
                    }
                }
            }
            varColumnThDate.Append("} \n\t");
            varColumnThDate.Append("dwColumnNameArrayCount = " + iii + "; \n\t");
            varColumnThDate.Append("</script>\n");
            return varColumnThDate.ToString();
        }
    }
}