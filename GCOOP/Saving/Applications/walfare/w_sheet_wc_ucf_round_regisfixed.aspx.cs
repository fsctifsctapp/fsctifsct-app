using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_ucf_round_regisfixed : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private String pbl = "w_sheet_wc_walfare_paid.pbl";
        protected String jsAdd_cstype;

        public void InitJsPostBack()
        {
            jsAdd_cstype = WebUtil.JsPostBack(this, "jsAdd_cstype");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptopen_date", "deptopen_tdate");

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, state.SsCsType);

            }
            else
            {
               
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsAdd_cstype":
                    Add_ccstype();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                tDwMain.Eng2ThaiAllRow();
                String delData = "delete from wcucfroundregisfixed where cs_type = '" + state.SsCsType + "'";
                WebUtil.QuerySdt(delData);

                int[] rows = new int[DwMain.RowCount];
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    rows[i - 1] = i;
                }

                DwUtil.InsertDataWindow(DwMain, pbl, "wcucfroundregisfixed", rows);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

       
        private void Add_ccstype()
        {
            try
            {
                DwMain.InsertRow(0);
                for (int i = 1; i <= DwMain.RowCount; i++)
                {

                    DwMain.SetItemString(i, "cs_type", state.SsCsType);

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
    }
}