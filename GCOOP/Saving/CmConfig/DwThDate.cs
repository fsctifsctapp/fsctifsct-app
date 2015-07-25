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
using Sybase.DataWindow.Web;
using System.Collections.Generic;
using System.Globalization;

namespace Saving.CmConfig
{
    public class DwThDate
    {
        private WebDataWindowControl dwMain;
        private CultureInfo th;
        private List<String> oriName;
        private List<String> thName;
        private String id;
        private String clientId;
        private int count;

        public int Count { get { return count; } }

        public List<String> OriName { get { return oriName; } }

        public List<String> ThName { get { return thName; } }

        public String ID { get { return id; } }

        public String ClientID { get { return clientId; } }

        public WebDataWindowControl DwMain { get { return dwMain; } }

        private void BeforeConstructorEnd(WebDataWindowControl dwMain)
        {
            this.dwMain = dwMain;
            this.count = 0;
            this.thName = new List<string>();
            this.oriName = new List<string>();
            this.th = new CultureInfo("th-TH");
            this.id = dwMain.ID;
            this.clientId = dwMain.ClientID;
        }

        public DwThDate(WebDataWindowControl dwMain)
        {
            this.dwMain = dwMain;
            this.count = 0;
            this.thName = new List<string>();
            this.oriName = new List<string>();
            this.th = new CultureInfo("th-TH");
        }

        public DwThDate(WebDataWindowControl dwMain, PageWebSheet w_sheet_xx_xxxxxxx)
        {
            BeforeConstructorEnd(dwMain);
            w_sheet_xx_xxxxxxx.AddThDate(this);
        }

        public DwThDate(WebDataWindowControl dwMain, PageWebDialog w_dlg_xx_xxxxxxx)
        {
            BeforeConstructorEnd(dwMain);
            w_dlg_xx_xxxxxxx.AddThDate(this);
        }

        public void Add(String OriginColumnName, String ThColumnName)
        {
            this.thName.Add(ThColumnName);
            this.dwMain.Modify(ThColumnName + ".EditMask.Mask='xx/xx/xxxx'");
            this.oriName.Add(OriginColumnName);
            this.count++;
        }

        public int Eng2ThaiAllRow()
        {
            int iii = 0;
            for (int i = 1; i < dwMain.RowCount + 1; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    try
                    {
                        DateTime dt = dwMain.GetItemDate(i, oriName[j]);
                        dwMain.SetItemString(i, thName[j], dt.ToString("ddMMyyyy", th));
                    }
                    catch { }
                    iii++;
                }
            }
            dwMain.ResetUpdateStatus();
            dwMain.SaveDataCache();
            return iii;
        }
    }
}