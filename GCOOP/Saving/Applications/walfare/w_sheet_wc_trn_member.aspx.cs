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
    public partial class w_sheet_wc_trn_member : PageWebSheet, WebSheet
    {
        protected String jsinitAccNo;
        protected String jsbranch_id;
        protected String jsbranch_desc;
        protected String jsselect_option;
        private DwThDate tDwMain;
        private DwThDate tDWOption;

        public void InitJsPostBack()
        {
            jsinitAccNo = WebUtil.JsPostBack(this, "jsinitAccNo");
            jsbranch_id = WebUtil.JsPostBack(this, "jsbranch_id");
            jsbranch_desc = WebUtil.JsPostBack(this, "jsbranch_desc");
            jsselect_option = WebUtil.JsPostBack(this, "jsselect_option");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptopen_date", "deptopen_tdate");
            tDWOption = new DwThDate(DwOption, this);
            tDWOption.Add("deptopen_date", "deptopen_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
                DwOption.SetItemString(1, "branch_id", state.SsBranchId);
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
                case "jsinitAccNo":
                    InitAccNo();
                    break;
                case "jsbranch_id":
                    ChangeBranchId();
                    break;
                case "jsbranch_desc":
                    ChangeBranchDesc();
                    break;
                case "jsselect_option":
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string select_option;
                try{select_option = DwOption.GetItemString(1, "select_option");}
                catch 
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกรูปแบบการโอนย้าย"); 
                    return;
                }
                String XmlMain = DwMain.Describe("DataWindow.data.XML");
                string branch_id = state.SsBranchId;
                if (state.SsCsType == "1")
                {
                    try
                    {
                        branch_id = DwOption.GetItemString(1, "branch_id");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกศูนย์ประสานงานต้นทาง");
                        return;
                    }
                }
                bool result = false;
                if (select_option == "O")
                {
                    //try
                    //{
                    //    string tdeptopen_date = DwOption.GetItemString(1, "deptopen_tdate");
                    //    if (tdeptopen_date.Length == 8)
                    //    {
                    //        DateTime dt = DateTime.ParseExact(tdeptopen_date, "ddMMyyyy", WebUtil.TH);
                    //        DwOption.SetItemDateTime(1, "deptopen_date", dt);
                    //    }
                    //}
                    //catch { }
                    //DateTime open_date = DateTime.MinValue;
                    //decimal prncbal;
                    //try
                    //{
                    //    open_date = DwOption.GetItemDateTime(1, "deptopen_date");
                    //    prncbal = DwOption.GetItemDecimal(1, "prncbal");
                    //}
                    //catch
                    //{
                    //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาข้อมูลวันคุ้มครองและจำนวนเงินให้ครบ");
                    //    return;
                    //}
                   // WsUtil.Walfare.Trn
                    result = WsUtil.Walfare.TrnMemb_ChgBranch(state.SsWsPass, state.SsApplication, XmlMain, "w_sheet_wc_trn_memb.pbl", state.SsUsername, branch_id, state.SsCsType, state.SsWorkDate, 0);
                }
                else
                {
                    result = WsUtil.Walfare.TrnMemb(state.SsWsPass, state.SsApplication, XmlMain, "w_sheet_wc_trn_memb.pbl", state.SsUsername, branch_id, state.SsCsType);
                }
                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ " + ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            string not_cstype, cstype;
            if (state.SsCsType == "1")
            {
                not_cstype = "8";
                cstype = "%";
            }
            else
            {
                not_cstype = "0";
                cstype = state.SsCsType;
                DwOption.Modify("t_3.visible=0");
                DwOption.Modify("branch_id.visible=0");
            }
            try
            {
                DwUtil.RetrieveDDDW(DwOption, "branch_id", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);
                DwUtil.RetrieveDDDW(DwMain, "coopbranch_id", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);
            }
            catch { }            
            tDWOption.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
        }

        public void InitAccNo()
        {
            string deptaccount_no = HdDeptaccount_no.Value;
            try
            {
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    if (deptaccount_no == DwMain.GetItemString(i, "deptaccount_no"))
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("เลขฌาปนกิจ " + deptaccount_no + "เพิ่มแถวไปแล้ว ไม่สามารถเพิ่มอีกได้");
                        return;
                    }
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
            }
            string branch_id;
            if (state.SsCsType == "1")
            {
                branch_id = DwOption.GetItemString(1, "branch_id");
            }
            else
            {
                branch_id = state.SsBranchId;
            }
            String SQL_init = @"select wm.member_no as member_no, wm.wfaccount_name as full_name, wm.deptopen_date as deptopen_date,
                    wm.branch_id as branch_id, cb.coopbranch_desc as coopbranch_desc, wm.wftype_code as wftype_code
                    from wcdeptmaster wm left join cmucfcoopbranch cb on(wm.branch_id = cb.coopbranch_id) where deptaccount_no = '"
                   + deptaccount_no + "' and branch_id = '" + branch_id + "'";
            try
            {
                Sdt dt = WebUtil.QuerySdt(SQL_init);
                if (dt.Next())
                {
                    int row_count = DwMain.RowCount;
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(row_count + 1, "deptaccount_no", deptaccount_no);
                    DwMain.SetItemString(row_count + 1, "member_no", dt.GetString("member_no"));
                    DwMain.SetItemString(row_count + 1, "wfaccount_name", dt.GetString("full_name"));
                    DwMain.SetItemString(row_count + 1, "deptopen_tdate", dt.GetDateTh("deptopen_date"));
                    DwMain.SetItemDateTime(row_count + 1, "deptopen_date", dt.GetDate("deptopen_date"));
                    DwMain.SetItemString(row_count + 1, "wftype_code", dt.GetString("wftype_code"));

                    //DwMain.SetItemString(row_count + 1, "branch_id", dt.GetString("branch_id"));
                    //DwMain.SetItemString(row_count + 1, "branch_desc", dt.GetString("coopbranch_desc"));
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void ChangeBranchId()
        {
            int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwMain.SetItemString(row, "coopbranch_id", DwMain.GetItemString(row, "branch_id"));
            }
            catch { }
        }

        public void ChangeBranchDesc()
        {
            int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwMain.SetItemString(row, "branch_id", DwMain.GetItemString(row, "coopbranch_id"));
            }
            catch { }
        }

    }
}