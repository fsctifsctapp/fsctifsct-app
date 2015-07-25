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
    public partial class w_sheet_wc_group_paid_cancel : PageWebSheet, WebSheet
    {
        protected String SelectCode;
        protected String PostSearch;
        protected String PeriodChange;
        private DwThDate tDwCri;

        public void InitJsPostBack()
        {
            SelectCode = WebUtil.JsPostBack(this, "SelectCode");
            PostSearch = WebUtil.JsPostBack(this, "PostSearch");
            PeriodChange = WebUtil.JsPostBack(this, "PeriodChange");

            //-----------------------------------
            tDwCri = new DwThDate(DWCri, this);
            tDwCri.Add("inform_date", "inform_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DWCri.InsertRow(0);

                DWCri.SetItemString(1, "branch_id", state.SsBranchId);

                DateTime informdate = new DateTime((DateTime.Today.Year - 1), 12, 31);
                DWCri.SetItemDateTime(1, "inform_date", informdate);
            }
            else
            {
                this.RestoreContextDw(DWCri);
                this.RestoreContextDw(DwMain);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "SelectCode":
                    JSSelectCode();
                    break;
                case "PostSearch":
                    Search();
                    break;
                case "PeriodChange":
                    PeriodChanGe();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String xmlMain = DwMain.Describe("DataWindow.Data.XML");
            bool resu = false;
            try
            {
                try
                {
                    string tinform_date = DwMain.GetItemString(1, "inform_tdate");
                    if (tinform_date.Length == 8)
                    {
                        DateTime dt = DateTime.ParseExact(tinform_date, "ddMMyyyy", WebUtil.TH);
                        DwMain.SetItemDateTime(1, "inform_date", dt);
                    }
                }
                catch { }
                DateTime inform_date = DateTime.MinValue;
                try
                {
                    inform_date = DWCri.GetItemDateTime(1, "inform_date");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่ลาออก");
                    return;
                }
                
                string branch_id = DWCri.GetItemString(1, "branch_id");
                int period = Convert.ToInt32(DWCri.GetItemDecimal(1, "period"));
                resu = WsUtil.Walfare.SaveGroupPaidCancel(state.SsWsPass, state.SsApplication, "w_sheet_wc_walfare_paid.pbl", xmlMain, state.SsUsername, branch_id, inform_date, period);
                if (resu)
                {
                    DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_walfare_paid.pbl", tDwCri, branch_id);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("" + ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(DWCri,"branch_id", "w_sheet_wc_walfare_paid.pbl", state.SsCsType);
            DWCri.SaveDataCache();
            DwMain.SaveDataCache();
            tDwCri.Eng2ThaiAllRow();
        }

        private void JSSelectCode()
        {
           // decimal status_post;
            switch (HdSelectCode.Value)
            {
                case "approve":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        //status_post = DwMain.GetItemDecimal(i + 1 , "status_post");
                        //if (status_post != -9)
                        //{
                        DwMain.SetItemDecimal(i + 1, "status", 1);
                        //}
                    }
                    break;
                case "wait":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        //status_post = DwMain.GetItemDecimal(i + 1, "status_post");
                        //if (status_post != -9)
                        //{
                        DwMain.SetItemDecimal(i + 1, "status", 8);
                        //}
                    }
                    break;
                case "cancle":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        //status_post = DwMain.GetItemDecimal(i + 1, "status_post");
                        //if (status_post != -9)
                        //{
                            DwMain.SetItemDecimal(i + 1, "status", 0);
                        //}
                    }
                    break;
            }
            HdSelectCode.Value = "";
        }

        private void Search()
        {
            try
            {
                string branch_id = DWCri.GetItemString(1, "branch_id");
                Decimal period = DWCri.GetItemDecimal(1, "period");
                String periodS = Convert.ToString(period);
                DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_walfare_paid.pbl", tDwCri, branch_id, periodS);
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขา และ รายการ");
            }
        }

        private void PeriodChanGe()
        {
            try
            {
                Decimal period =  DWCri.GetItemDecimal(1, "period");
                String Speriod = period.ToString();
                String Syear = Speriod.Substring(0, 4);
                int year = Convert.ToInt32(Syear)-543;
                DateTime informdate = new DateTime(year, 12, 31);
                DWCri.SetItemDateTime(1, "inform_date", informdate);
                Search();
            }
            catch { }
        }
    }
}