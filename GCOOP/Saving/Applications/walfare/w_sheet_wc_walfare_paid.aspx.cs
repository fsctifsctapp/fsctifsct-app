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
    public partial class w_sheet_wc_walfare_paid : PageWebSheet, WebSheet
    {
        private string pbl = "w_sheet_wc_walfare_paid.pbl";
        private DwThDate tDwMain;
        protected String postDeptAccountNo;
        protected String postAddRowSlip;
        protected String postDeleteSlip;
        protected String postSlipCode;
        protected String postRecppaytype_code;
        protected String postBank;

        public void InitJsPostBack()
        {
            postDeptAccountNo = WebUtil.JsPostBack(this, "postDeptAccountNo");
            postAddRowSlip = WebUtil.JsPostBack(this, "postAddRowSlip");
            postDeleteSlip = WebUtil.JsPostBack(this, "postDeleteSlip");
            postSlipCode = WebUtil.JsPostBack(this, "postSlipCode");
            postRecppaytype_code = WebUtil.JsPostBack(this, "postRecppaytype_code");
            postBank = WebUtil.JsPostBack(this, "postBank");
            //---------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptslip_date", "deptslip_tdate");
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            ///slip ใบเสร็จ
            HdSaveStatus.Value = "";

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSlip);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postDeptAccountNo")
            {
                JsPostDeptAccountNo();
            }
            else if (eventArg == "postAddRowSlip")
            {
                JsPostAddRowSlip();
            }
            else if (eventArg == "postDeleteSlip")
            {
                JsPostDeleteSlip();
            }
            else if (eventArg == "postSlipCode")
            {
                JsPostSlipCode();
            }else if( eventArg == "postRecppaytype_code"){
                string recppaytype_code;
                try{
                    recppaytype_code = DwMain.GetItemString(1, "recppaytype_code");
                }catch{
                    recppaytype_code = "";
                }
                if(recppaytype_code != "TRN"){
                    DwMain.SetItemString(1, "expense_bank", " ");
                    DwMain.SetItemString(1, "expense_branch", "");
                }
            }
            else if (eventArg == "postBank")
            {
                
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string toperate_date = DwMain.GetItemString(1, "operate_tdate");
                if (toperate_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(toperate_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "operate_tdate", dt);
                }
            }
            catch { }

            try
            {
                string tdeptslip_date = DwMain.GetItemString(1, "deptslip_tdate");
                if (tdeptslip_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tdeptslip_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "deptslip_tdate", dt);
                }
            }
            catch { }

            try
            {
                DwMain.SetItemString(1, "recppaytype_code", "CSH");
                String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");
                String xmlDwSlip = DwSlip.Describe("DataWindow.Data.XML");
                String slip_no = WsUtil.Walfare.SavePaid(state.SsWsPass, state.SsApplication, pbl, xmlDwMain, xmlDwSlip, HdReqDocno.Value);
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการชำระสำเร็จ");

                HdSlipNo.Value = slip_no;
                HdSaveStatus.Value = "1";
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
                DwUtil.RetrieveDDDW(DwMain, "sliptype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "recppaytype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwSlip, "deptitemtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "expense_bank", pbl, null);
                String slipTypeCode = DwUtil.GetString(DwMain, 1, "sliptype_code", "").Trim();
                if (slipTypeCode != "")
                {
                    DataWindowChild dc = DwSlip.GetChild("deptitemtype_code");
                    dc.SetFilter("sign_flag=1");
                    dc.Filter();
                }
                String bankcode = DwMain.GetItemString(1, "expense_bank");
                if (bankcode != "")
                {
                    DwUtil.RetrieveDDDW(DwMain, "expense_branch", pbl, bankcode);
                }
            }
            catch { }
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();
        }

        private void JsPostDeptAccountNo()
        {
            HdReqDocno.Value = "";
            try
            {
                String deptAccNo = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "deptaccount_no", deptAccNo);
                DwMain.SetItemString(1, "sliptype_code", "WPX");
                String sql1 = @"
                    SELECT 
                        *
                    FROM 
                        WCDEPTMASTER
                    INNER JOIN MBUCFPRENAME ON WCDEPTMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE
                    WHERE WCDEPTMASTER.deptaccount_no='" + deptAccNo + "' and WCDEPTMASTER.branch_id='" + state.SsBranchId + "'";
                Sdt dt = WebUtil.QuerySdt(sql1);
                if (dt.Next())
                {
                    DwMain.SetItemString(1, "recppaytype_code", "CSH");
                    DwMain.SetItemDateTime(1, "deptslip_date", DateTime.Today);
                    DwMain.SetItemDateTime(1, "operate_date", DateTime.Today);
                    DwMain.SetItemString(1, "prename_desc", dt.GetString("prename_desc"));
                    DwMain.SetItemString(1, "deptaccount_name", dt.GetString("deptaccount_name"));
                    DwMain.SetItemString(1, "deptaccount_sname", dt.GetString("deptaccount_sname"));
                    DwMain.SetItemString(1, "member_no", dt.GetString("member_no"));
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                    tDwMain.Eng2ThaiAllRow();
                    //String sql2 = "select * from wcreqdetail where deptrequest_docno='" + dt.GetString("deptrequest_docno") + "'";
                    //Sdt dt2 = WebUtil.QuerySdt(sql2);
                    DwSlip.Reset();
                    String sqlReq = "select deptrequest_docno, pay_status from wcreqdeposit where member_no='" + dt.GetString("member_no") + "' and branch_id='" + state.SsBranchId + "'";
                    Sdt dtReq = WebUtil.QuerySdt(sqlReq);
                    if (dtReq.Next())
                    {
                        if (dtReq.GetInt32(1) != 1)
                        {
                            HdReqDocno.Value = dtReq.GetString(0);
                            String sqlReqDet = "select * from wcreqdetail left join wcucfdeptitemtype on wcreqdetail.deptitemtype_code = wcucfdeptitemtype.deptitemtype_code where wcreqdetail.deptrequest_docno='" + HdReqDocno.Value + "' order by seq_no";
                            Sdt dtReqDet = WebUtil.QuerySdt(sqlReqDet);
                            while (dtReqDet.Next())
                            {
                                JsPostAddRowSlip();
                                int row = DwSlip.RowCount;
                                DwSlip.SetItemString(row, "deptitemtype_code", dtReqDet.GetString("deptitemtype_code"));
                                DwSlip.SetItemString(row, "slip_desc", dtReqDet.GetString("deptitemtype_desc"));
                                DwSlip.SetItemDecimal(row, "prncslip_amt", dtReqDet.GetDecimal("amt"));
                            }
                        }
                        else
                        {
                            JsPostAddRowSlip();
                        }
                    }
                    else
                    {
                        JsPostAddRowSlip();
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsPostAddRowSlip()
        {
            DwSlip.InsertRow(0);
            int row = DwSlip.RowCount;
            DwSlip.SetItemString(row, "branch_id", state.SsBranchId);
            DwSlip.SetItemDecimal(row, "sign_flag", 1);
        }

        private void JsPostDeleteSlip()
        {
            int row = int.Parse(HdRowSlip.Value);
            DwSlip.DeleteRow(row);
        }

        private void JsPostSlipCode()
        {
            DwSlip.Reset();
            JsPostAddRowSlip();
            String sliptype_code = DwUtil.GetString(DwMain, 1, "sliptype_code", "");
            String deptaccountNo = DwUtil.GetString(DwMain, 1, "deptaccount_no", "");
            if (sliptype_code == "WPD")
            {
                DwSlip.SetItemDecimal(1, "sign_flag", -1);
                String sql = "select DEPTITEMTYPE_DESC from WCUCFDEPTITEMTYPE where DEPTITEMTYPE_CODE='" + sliptype_code + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                String desc = "";
                if (dt.Next())
                {
                    desc = dt.GetString(0);
                }
                else
                {
                    return;
                }
                sql = "select withdrawable_amt from wcdeptmaster where deptaccount_no='" + deptaccountNo + "' and branch_id='" + state.SsBranchId + "'";
                dt = WebUtil.QuerySdt(sql);
                decimal withAmt = 0;
                if (dt.Next())
                {
                    withAmt = dt.GetDecimal(0);
                    DwSlip.SetItemDecimal(1, "prncslip_amt", withAmt);
                }
                else
                {
                    return;
                }
                DwSlip.SetItemString(1, "deptitemtype_code", "WPD");
                DwSlip.SetItemString(1, "slip_desc", desc);
            }
        }

        private void JspostFilterBranch()
        {
            try
            {
                String bankcode = DwMain.GetItemString(1, "expense_bank");
                DwUtil.RetrieveDDDW(DwMain, "expense_branch", pbl, bankcode);
                //DataWindowChild dc = DwMain.GetChild("asnslippayout_bankbranch_id");
                //dc.SetFilter("bank_code ='" + bankcode + "'");
                //dc.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }        
    }
}