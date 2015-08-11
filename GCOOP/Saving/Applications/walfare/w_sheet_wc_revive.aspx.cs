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
    public partial class w_sheet_wc_revive : PageWebSheet, WebSheet
    {
        protected String jsinitAccNo;
        protected String jsResignCauseCode;
        private String pbl = "w_sheet_wc_inform.pbl";
        private DwThDate tDwMain;

        public void InitJsPostBack()
        {

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("die_date", "die_tdate");
            tDwMain.Add("deptclose_date", "deptclose_tdate");

            jsinitAccNo = WebUtil.JsPostBack(this, "jsinitAccNo");
            jsResignCauseCode = WebUtil.JsPostBack(this, "jsResignCauseCode");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                DwMain.SetItemString(1, "for_year", Syear);
                
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch(eventArg)
            {
                case "jsinitAccNo":
                     InitAccNo();
                     break; 
                case "jsResignCauseCode":
                     DwMain.SetItemString(1, "month", "");
                     DwMain.SetItemString(1, "year", "");
                     break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string tdeptclose_date = DwMain.GetItemString(1, "deptclose_tdate");
                if (tdeptclose_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tdeptclose_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "deptclose_date", dt);
                }
            }
            catch { }
            try
            {
                string tdie_date = DwMain.GetItemString(1, "die_tdate");
                if (tdie_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tdie_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "die_date", dt);
                }
            }
            catch { }

            try
            {
                string resigncause_code = DwMain.GetItemString(1, "resigncause_code");
                decimal reqchg_status = DwMain.GetItemDecimal(1, "reqchg_status");
                string period = "";
                if (resigncause_code.Trim() == "04" && reqchg_status == -9)
                {
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
                }
                string deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                DateTime deptclose_tdate = DwMain.GetItemTime(1, "deptclose_date");
                DateTime die_tdate = DwMain.GetItemTime(1, "die_date");                
                
                string branch_id = DwMain.GetItemString(1, "branch_id");
                string dpreqchg_doc = DwMain.GetItemString(1, "wcreqchg_dept_dpreqchg_doc");
                string for_year = DwMain.GetItemString(1, "for_year");

                bool Ck_return = WsUtil.Walfare.StatusMem(state.SsWsPass, state.SsApplication, deptaccount_no, deptclose_tdate, die_tdate, resigncause_code, reqchg_status, branch_id, period, state.SsUsername, state.SsCsType, dpreqchg_doc, for_year);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
            }

            catch (Exception ex) 
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                
            }

        }

        public void WebSheetLoadEnd()
        {
            
            DwUtil.RetrieveDDDW(DwMain, "branch_id", pbl, state.SsCsType);
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();

        }
     

        public void InitAccNo()
        {
            try
            {
                String AccNo = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
                String Branch_id = DwMain.GetItemString(1, "branch_id");
                DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, AccNo, Branch_id);

                //string Syear = Convert.ToString(DateTime.Today.Year + 543);
                //DwMain.SetItemString(1, "for_year", Syear);

                if (DwMain.RowCount < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล");
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                }
                else
                {
                    string Syear = Convert.ToString(DateTime.Today.Year + 543);
                    DwMain.SetItemString(1, "for_year", Syear);
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            
        }
        
    }
}