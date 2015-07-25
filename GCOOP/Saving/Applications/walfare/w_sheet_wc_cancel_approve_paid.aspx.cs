using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.Data;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_cancel_approve_paid : PageWebSheet, WebSheet
    {
        private string pbl = "w_sheet_wc_walfare_paid.pbl";
        private DwThDate tDwMain;
        protected String jsPostDeptaccountNo;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("effective_date", "effective_tdate");
            tDwMain.Add("deptopen_date", "deptopen_tdate");

            jsPostDeptaccountNo = WebUtil.JsPostBack(this, "jsPostDeptaccountNo");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);

            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDeptaccountNo":
                    PostDeptaccountNo();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                decimal select_flag = 0, slip_amt = 0, count = 0;
                string slip_no = "";
                for (int i = 1; i <= DwDetail.RowCount; i++)
                {
                    count += DwDetail.GetItemDecimal(i, "select_flag");
                    select_flag = DwDetail.GetItemDecimal(i, "select_flag");
                    if (select_flag == 1)
                    {
                        slip_amt = DwDetail.GetItemDecimal(i, "deptslip_amt");
                        slip_no = DwDetail.GetItemString(i, "deptslip_no");
                    }
                }
                if (count != 1)
                {
                    throw new Exception("กรุณาเลือกรายการใบเสร็จที่ต้องการยกเลิก 1 รายการ");
                }
                string period = "";
                try
                {
                    string month = DwMain.GetItemString(1, "month");
                    string year = DwMain.GetItemString(1, "year");
                    if (month.Trim() == "" || year.Trim() == "")
                    {
                        throw new Exception("กรุณาระบุ เดือน และ ปี ที่ต้องการยกเลิก การสิ้นสุด");
                    }
                    period = year + month;
                }
                catch
                {
                    throw new Exception("กรุณาระบุ เดือน และ ปี ที่ต้องการยกเลิก การสิ้นสุด");
                }
                DateTime deptopen_date = DwMain.GetItemDateTime(1, "deptopen_date");
                DateTime effective_date = DwMain.GetItemDateTime(1, "effective_date");
                DateTime new_effective_date = new DateTime(effective_date.Year - 1, 1, 1);

                string Seffective_date = "";
                if (deptopen_date > new_effective_date)
                {
                    Seffective_date = deptopen_date.ToString("ddMMyyyy");
                }
                else
                {
                    Seffective_date = new_effective_date.ToString("ddMMyyyy");
                }
                string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                string branch_id = DwMain.GetItemString(1, "branch_id");
                bool Ck_return = WsUtil.Walfare.CancelApprovePaid(state.SsWsPass, state.SsApplication, deptaccount_no, branch_id, slip_no, period, state.SsUsername, slip_amt, Seffective_date);
                if (Ck_return)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    PostDeptaccountNo();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้");
                }
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(DwMain, "branch_id", pbl, state.SsCsType);
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void PostDeptaccountNo()
        {
            try
            {
                String deptaccount_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
                String Branch_id = DwMain.GetItemString(1, "branch_id");
                DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, deptaccount_no, Branch_id);
                if (DwMain.RowCount < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล");
                    DwMain.InsertRow(0);
                }
                else
                {
                    DwUtil.RetrieveDataWindow(DwDetail, pbl, null, deptaccount_no, Branch_id);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }

        }
    }
}