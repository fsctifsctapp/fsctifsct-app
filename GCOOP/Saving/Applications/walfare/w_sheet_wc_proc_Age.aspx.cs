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
    public partial class w_sheet_wc_proc_Age : PageWebSheet, WebSheet
    {
       
        protected String jsupdateFee;

        public void InitJsPostBack()
        {
            jsupdateFee = WebUtil.JsPostBack(this, "jsupdateFee");
           

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
                DwOption.SetItemDateTime(1, "as_tstartdate", new DateTime(2011, 6, 1));
                DwOption.SetItemDateTime(1, "as_tenddate", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));


            }
            else
            {
                this.RestoreContextDw(DwOption);
               
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
               
                case "jsupdateFee":
                    UpdateFeeYear();
                    break;
            }
        }

        public void SaveWebSheet()
        {
           
        }

        public void WebSheetLoadEnd()
        {
            
            DwOption.SaveDataCache();
            DwUtil.RetrieveDDDW(DwOption, "as_tstartdate", "w_sheet_wc_membermaster.pbl", null);
            DwUtil.RetrieveDDDW(DwOption, "as_tenddate", "w_sheet_wc_membermaster.pbl", null);
            
        }

        private void UpdateFeeYear()
        {
            try
            {
                bool result = false;
                string cs_type = state.SsCsType;
                DateTime st_date = DwOption.GetItemDateTime(1, "as_tstartdate");
                DateTime end_date = DwOption.GetItemDateTime(1, "as_tenddate");

                result = WsUtil.Walfare.AgeChgProc(state.SsWsPass, state.SsApplication, cs_type, st_date, end_date);

                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ประมวณผลสำเร็จ");

                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

       
    }
}