using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Sybase.DataWindow;
using DBAccess;
using System.Data;
using GcoopServiceCs;
namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_paid_add : PageWebSheet, WebSheet
    {
        private string pbl = "w_sheet_wc_walfare_paid.pbl";
        protected String jsChangeDeptNo;
        protected String jsAddRow;

        private DwThDate tDwList;
        public void InitJsPostBack()
        {
            jsChangeDeptNo = WebUtil.JsPostBack(this,"jsChangeDeptNo");
            jsAddRow = WebUtil.JsPostBack(this, "jsAddRow");

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);                
            }
            else
            {
                this.RestoreContextDw(DwCri);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsChangeDeptNo":
                    ChangeDeptNo();
                    break;
                case "jsAddRow":
                    AddRow();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                for (int i = 1; i <= DwList.RowCount; i++)
                {
                    try
                    {
                        string toperate_date = DwList.GetItemString(1, "operate_tdate");
                        if (toperate_date.Length == 8)
                        {
                            DateTime dt = DateTime.ParseExact(toperate_date, "ddMMyyyy", WebUtil.TH);
                            DwList.SetItemDateTime(i, "operate_date", dt);
                        }
                    }
                    catch { }
                }
                String xmlList = DwList.Describe("DataWindow.data.XML");
                bool result = WsUtil.Walfare.SavePaidAdd(state.SsWsPass, state.SsApplication, pbl, xmlList);
                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            tDwList.Eng2ThaiAllRow();
            DwCri.SaveDataCache();
            DwList.SaveDataCache();
        }

        private void ChangeDeptNo()
        {
            try
            {
                string deptaccount_no = int.Parse(DwCri.GetItemString(1, "deptaccount_no")).ToString("000000");
                DwUtil.RetrieveDataWindow(DwCri, pbl, null, deptaccount_no, state.SsCsType);
                if (DwCri.RowCount > 0)
                {
                    decimal deptclose_status = DwCri.GetItemDecimal(1, "deptclose_status");
                    if (deptclose_status == 0)
                    {
                        DwUtil.RetrieveDataWindow(DwList, pbl, tDwList, deptaccount_no);
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("สามาชิกคนนี้ได้ทำการลาออก/ชีวิตแล้ว ไม่สามารถทำรายการได้");
                        DwList.Reset();
                    }
                }
                else
                {
                    DwCri.InsertRow(0);
                    DwList.Reset();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void AddRow()
        {
            try
            {
                string deptaccount_no = DwCri.GetItemString(1, "deptaccount_no");
                string member_no = DwCri.GetItemString(1, "member_no");
                string wfaccount_name = DwCri.GetItemString(1, "wfaccount_name");
                string branch_id = DwCri.GetItemString(1, "branch_id");

                DwList.InsertRow(0);
                int rowcount = DwList.RowCount;
                DwList.SetItemString(rowcount, "wfmember_no", deptaccount_no);
                DwList.SetItemString(rowcount, "member_no", member_no);
                DwList.SetItemString(rowcount, "depttype_code", "01");
                DwList.SetItemDecimal(rowcount, "status_post", 8);
                DwList.SetItemString(rowcount, "wfaccount_name", wfaccount_name);
                DwList.SetItemString(rowcount, "wcitemtype_code", "FEE");
                DwList.SetItemString(rowcount, "branch_id", branch_id);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}