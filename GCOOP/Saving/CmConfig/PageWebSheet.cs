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
    public class PageWebSheet : Page
    {
        public WebState state;
        private List<DwThDate> tDwDates;
        protected DwTrans sqlca;
        public Literal LtServerMessage;
        protected String initJavaScript;

        public String WebSheetType
        {
            get { return "Saving.CmConfig.PageWebSheet"; }
        }

        //------------------------------------------------------------------

        public void AddThDate(DwThDate tDwDate)
        {
            tDwDates.Add(tDwDate);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tDwDates = new List<DwThDate>();
            String SaveWebSheet = WebUtil.JsPostBack(this, "SaveWebSheet");
            String saveJavaScript = SaveWebSheet + @"
            <script>
            function MenubarSave(){
                var isValid;
                try{
                    isValid = Validate();
                }catch(err){
                    alert('ไม่พบการ Validate');
                    return;
                }
                if(!isValid) return;
                SaveWebSheet();
            }
            </script>";
            initJavaScript = "";
            Type t = this.GetType();
            String lastFocus = "";
            try
            {
                lastFocus = Request["tempLastFocus"];
            }
            catch { }

            String dsTempElementEnter = "\n<!-- Doys แทรก Script เพื่อ  //-->\n";
            dsTempElementEnter += saveJavaScript;
            dsTempElementEnter += "<input type='text' id='tempElementEnter' name='tempElementEnter' value='fel' style='width:1px;height:1px;border:none;background-color:white;' />\n";
            dsTempElementEnter += "<input type='hidden' id='tempLastFocus' name='tempLastFocus' value='" + lastFocus + "' />\n\n";
            ClientScript.RegisterClientScriptBlock(t, "DsTempElementEnter", dsTempElementEnter);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            String s = sender.ToString();
        }

        public void SetDwThDateJavaScriptEvent(Literal ltDwThDateJavaScript)
        {
            // ตั้งคา Javascript
            StringBuilder varColumnThDate = new StringBuilder("<script> \n\t");
            varColumnThDate.Append("var thDwColumnNameArray = new Array(); \n\t");
            varColumnThDate.Append("var enDwColumnNameArray = new Array(); \n\t");
            varColumnThDate.Append("var dwObjectJavaScriptArray = new Array(); \n\t");
            varColumnThDate.Append("var dwColumnNameArrayCount = 0;\n\t");

            int iii = 0;
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
            varColumnThDate.Append("dwColumnNameArrayCount = " + iii + "; \n\t");
            varColumnThDate.Append("</script>\n");
            //Type t = this.GetType();
            //ClientScript.RegisterClientScriptBlock(t, "DwColumnNameArray", varColumnThDate.ToString());
            ltDwThDateJavaScript.Text = varColumnThDate.ToString();
        }

        public void ConnectSQLCA()
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

        public void DisConnectSQLCA()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
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
    }
}
