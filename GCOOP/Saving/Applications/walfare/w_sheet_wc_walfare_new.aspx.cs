
using CommonLibrary;
using System;
using DBAccess;
using Sybase.DataWindow;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_walfare_new : PageWebSheet, WebSheet
    {
        private String pbl = "w_sheet_wc_walfare_new.pbl";

        private DwThDate tDwMain;

        protected String postPost;
        protected String postProvince;
        protected String postAddRowDwRelate;
        protected String postDelRowDwRelate;

        public void InitJsPostBack()
        {
            postPost = WebUtil.JsPostBack(this, "postPost");
            postProvince = WebUtil.JsPostBack(this, "postProvince");
            postAddRowDwRelate = WebUtil.JsPostBack(this, "postAddRowDwRelate");
            postDelRowDwRelate = WebUtil.JsPostBack(this, "postDelRowDwRelate");
            //-----------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("apply_date", "apply_tdate");
            tDwMain.Add("wfbirthday_date", "wfbirthday_tdate");
            tDwMain.Add("deptopen_date", "deptopen_tdate");
        }

        public void WebSheetLoadBegin()
        {
            //HdIsOpen.Value = "false";
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "membgroup_code", state.SsBranchId);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                DwMain.SetItemString(1, "province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "postcode", state.SsPostCode);
                DwMain.SetItemDateTime(1, "apply_date", state.SsWorkDate);
                DwMain.SetItemString(1, "depttype_code", "01");
                DwMain.SetItemDecimal(1, "approve_status", 8);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                DwMain.SetItemString(1, "member_type", "01");
                DwMain.SetItemString(1, "carreer", "0");

                String wftypeCode = DwUtil.GetString(DwMain, 1, "wftype_code", "");
                int payStatus = DwUtil.GetInt(DwMain, 1, "pay_status", 0);
                if (wftypeCode != "")
                {
                    String sql = "select * from wcmembertype where wftype_code='" + wftypeCode + "'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(1, "deptitemtype_code", "FEE");
                        DwSlip.SetItemDecimal(1, "amt", dt.GetDecimal("feeappl_amt"));
                        DwSlip.SetItemDecimal(1, "status_pay", payStatus);
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(2, "deptitemtype_code", "WFY");
                        DwSlip.SetItemDecimal(2, "amt", dt.GetDecimal("feeperyear_amt"));
                        DwSlip.SetItemDecimal(2, "status_pay", payStatus);
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(3, "deptitemtype_code", "WPF");
                        DwSlip.SetItemDecimal(3, "amt", dt.GetDecimal("paybffuture_amt"));
                        DwSlip.SetItemDecimal(3, "status_pay", payStatus);
                    }
                    //dt.Dispose();
                    dt = WebUtil.QuerySdt("select * from wcdeptconstant");
                    if (dt.Next())
                    {
                        DateTime deptOpenDate = dt.GetDate("deptopen_date");
                        DwMain.SetItemDateTime(1, "deptopen_date", deptOpenDate);
                    }
                }

                String sql2 = "select * from wcdeptconstant where branch_id = '" + state.SsBranchId + "' ";
                Sdt dt2 = WebUtil.QuerySdt(sql2);
                if (dt2.Next())
                {
                    DwMain.SetItemDateTime(1, "deptopen_date", dt2.GetDate("deptopen_date"));
                }
                tDwMain.Eng2ThaiAllRow();
                DwRelate.InsertRow(0);
                DwRelate.InsertRow(0);
                DwRelate.InsertRow(0);
                DwRelate.InsertRow(0);
                DwRelate.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwRelate);
                this.RestoreContextDw(DwSlip);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postProvince")
            {
                JsPostProvince();
            }
            else if (eventArg == "postAddRowDwRelate")
            {
                JsPostRowDwRelate();
            }
            else if (eventArg == "postDelRowDwRelate")
            {
                JsPostDelRowRelate();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                for (int i = DwRelate.RowCount; i > 0; i--)
                {
                    String nameRelate = DwUtil.GetString(DwRelate, i, "name", "").Trim();
                    if (nameRelate == "")
                    {
                        DwRelate.DeleteRow(i);
                    }
                }
                try
                {
                    String birthDateThai = DwUtil.GetString(DwMain, 1, "wfbirthday_tdate", "").Trim();// DwMain.GetItemString(1, "wfbirthday_tdate");
                    if (birthDateThai.Length == 8)
                    {
                        DwMain.SetItemDateTime(1, "wfbirthday_date", DateTime.ParseExact(birthDateThai, "ddMMyyyy", WebUtil.TH));
                    }
                }
                catch { }
                String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");
                String xmlDwRelate = DwRelate.Describe("DataWindow.Data.XML");
                String xmlDwSlip = DwSlip.Describe("DataWindow.Data.XML");
                WsUtil.Walfare.SaveReqWalfareNew(state.SsWsPass, state.SsApplication, pbl, xmlDwMain, xmlDwRelate, xmlDwSlip);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "wftype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "prename_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "province_code", pbl, null);
                try
                {
                    DwMain.SetItemString(1, "postcode", "");
                    String pvCode = DwUtil.GetString(DwMain, 1, "province_code", "");
                    if (pvCode != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "ampher_code", pbl, pvCode);
                        String dtCode = DwUtil.GetString(DwMain, 1, "ampher_code", "");
                        if (dtCode != "")
                        {
                            DataWindowChild dc = DwMain.GetChild("ampher_code");
                            int rPostCode = dc.FindRow("DISTRICT_CODE='" + dtCode + "'", 1, dc.RowCount);
                            String postCode = DwUtil.GetString(dc, rPostCode, "postcode", "");
                            DwMain.SetItemString(1, "postcode", postCode);
                        }
                    }
                }
                catch { }
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwSlip, "deptitemtype_code", pbl);
            }
            catch { }
            DwMain.SaveDataCache();
            DwRelate.SaveDataCache();
            DwSlip.SaveDataCache();
        }

        private void JsPostProvince()
        {
            //HdIsOpen.value = "true";
            DwMain.SetItemString(1, "ampher_code", "");
        }

        private void JsPostRowDwRelate()
        {
            DwRelate.InsertRow(0);
        }

        private void JsPostDelRowRelate()
        {
            int ii = int.Parse(HdDeleteRow.Value);
            DwRelate.DeleteRow(ii);
        }
    }
}