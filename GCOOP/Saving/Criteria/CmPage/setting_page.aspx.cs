using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using CommonLibrary.WsCommon;

namespace Saving.CmPage
{
    public partial class setting_page : System.Web.UI.Page
    {
        private WebState state;

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState();
            
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            String tempSet = "";
            try { tempSet = state.SsPrinterSet; }
            catch { tempSet = ""; }
            if (tempSet == "0" || tempSet == "")
            {
            this.SetDDLPrinter();
            }
        }

        private void SetDDLPrinter()
        {
            try
            {
                try { DlPrinter.Items.Clear(); }
                catch { }
                Common comm = WsUtil.Common;
                DataTable dtFormSets = comm.GetPrinterFormSetsData(state.SsWsPass);
                String tempSet = "";
                int ii = -1;
                ListItem lt;
                for (int i = 0; i < dtFormSets.Rows.Count; i++)
                {
                    lt = new ListItem(dtFormSets.Rows[i]["formset_desc"].ToString(), dtFormSets.Rows[i]["formset_code"].ToString());
                    DlPrinter.Items.Add(lt);
                    try { tempSet = state.SsPrinterSet; }
                    catch { tempSet = ""; }
                    if (tempSet == "")
                    {
                        if (dtFormSets.Rows[i]["computer_ip"].ToString() == state.SsClientIp)
                        {
                            ii = ii >= 0 ? ii : i;
                            DlPrinter.SelectedIndex = ii;
                        }
                    }
                    else
                    {
                        if (dtFormSets.Rows[i]["formset_code"].ToString() == tempSet)
                        {
                            ii = ii >= 0 ? ii : i;
                            DlPrinter.SelectedIndex = ii;
                        }
                    }
                }
                if (ii < 0)
                {
                    lt = new ListItem("ยังไม่ได้กำหนดเครื่องพิมพ์", "0");
                    DlPrinter.Items.Insert(0, lt);
                    DlPrinter.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { LbServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        protected void b_save_Click(object sender, EventArgs e)
        {
            Session["ss_printerset"] = DlPrinter.SelectedValue;
            this.SetDDLPrinter();
        }
    }
}
