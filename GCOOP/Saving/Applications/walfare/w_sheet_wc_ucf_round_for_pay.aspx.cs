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
    public partial class w_sheet_wc_ucf_round_for_pay : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private String pbl = "w_sheet_wc_walfare_paid.pbl";
        protected String jsPostForyear;
        protected String jsAdd_cstype;
        protected String jsupdateFee;

        public void InitJsPostBack()
        {
            jsPostForyear = WebUtil.JsPostBack(this, "jsPostForyear");
            jsAdd_cstype = WebUtil.JsPostBack(this, "jsAdd_cstype");
            jsupdateFee = WebUtil.JsPostBack(this, "jsupdateFee");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptopen_date", "deptopen_tdate");
            
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);

             
            }
            else
            {
                this.RestoreContextDw(DwOption);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostForyear":
                    GetData();
                    break;
                case "jsAdd_cstype":
                    Add_ccstype();
                    break;
                case "jsupdateFee":
                    UpdateFeeYear();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                tDwMain.Eng2ThaiAllRow();
                string for_year = DwOption.GetItemString(1, "for_year");
                String delData = "delete from wcucfrecievefixedyear where for_year = '" + for_year + "' and cs_type = '" + state.SsCsType + "'";
                WebUtil.QuerySdt(delData);

                int[] rows = new int[DwMain.RowCount];
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    rows[i - 1] = i;
                }

                DwUtil.InsertDataWindow(DwMain, pbl, "wcucfrecievefixedyear", rows);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

              // Decimal aaa = Convert.ToInt32(for_year);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void UpdateFeeYear()
        {
            try
            {
                bool result = false;
                String XmlMain = DwOption.Describe("DataWindow.data.XML");
                string branch_id = state.SsBranchId;
                string cs_type = state.SsCsType;

                result = WsUtil.Walfare.Update_Fee_Year(state.SsWsPass, state.SsApplication, XmlMain, pbl, cs_type, branch_id);

                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("อัพเดทสำเร็จ");

                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void GetData()
        {
            try
            {
                string for_year = DwOption.GetItemString(1, "for_year");
                DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, for_year, state.SsCsType);
                Decimal yearr = Convert.ToInt32(for_year);
                DwOption.SetItemString(1, "mmyyyy", yearr-1 + "12");
               
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void Add_ccstype()
        {
            try
            {
                DwMain.InsertRow(0);
                string for_year1 = DwOption.GetItemString(1, "for_year");
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    DwMain.SetItemString(i, "cs_type", state.SsCsType);
                    DwMain.SetItemString(i, "for_year", for_year1);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        
         }
    }
}