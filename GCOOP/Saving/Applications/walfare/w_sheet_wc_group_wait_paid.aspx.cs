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
    public partial class w_sheet_wc_group_wait_paid : PageWebSheet, WebSheet
    {
        private DwThDate tDwCtrl;
        protected String SelectCode;
        protected String PeriodChange;

        public void InitJsPostBack()
        {
            SelectCode = WebUtil.JsPostBack(this, "SelectCode");
            PeriodChange = WebUtil.JsPostBack(this, "PeriodChange");
            tDwCtrl = new DwThDate(DWCtrl, this);
            tDwCtrl.Add("deptopen_date", "deptopen_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DWCtrl.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DWCtrl);
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
                case "PeriodChange":
                    JSPeriodChange();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String xmlMain = DwMain.Describe("DataWindow.Data.XML");
            bool resu = false;
            decimal Dperiod = 0;
            int period = 0;
            int statement_flag = 1, fee_flag = 1;
            decimal fee = 20;  
            try
            {
                if (state.SsCsType == "8")
                {
                     Dperiod = DWCtrl.GetItemDecimal(1, "period");
                     period = Convert.ToInt32(Dperiod);
                    if (state.SsCsType == "7") fee = 50;
                    switch (period)
                    {
                        case 255512:
                            switch (state.SsCsType)
                            {
                                case "2":
                                    fee = 40;
                                    break;
                                case "3":
                                    fee = 40;
                                    break;
                                case "4":
                                    fee = 20;
                                    break;
                                case "5":
                                    fee = 20;
                                    break;
                                case "6":
                                    fee = 20;
                                    break;
                                case "7":
                                    fee = 50;
                                    break;
                                case "8":
                                    fee = 20;
                                    break;
                            }
                            statement_flag = 0;
                            break;
                        case 255511:
                            fee_flag = 0;
                            break;
                        case 255612:
                            switch (state.SsCsType)
                            {
                                case "2":
                                    fee = 40;
                                    break;
                                case "3":
                                    fee = 40;
                                    break;
                                case "4":
                                    fee = 20;
                                    break;
                                case "5":
                                    fee = 20;
                                    break;
                                case "6":
                                    fee = 20;
                                    break;
                                case "7":
                                    fee = 50;
                                    break;
                                case "8":
                                    fee = 20;
                                    break;
                            }
                            statement_flag = 0;
                            break;

                        case 255712:
                            switch (state.SsCsType)
                            {
                                case "1":
                                    fee = 40;
                                    break;
                                case "2":
                                    fee = 40;
                                    break;
                                case "3":
                                    fee = 40;
                                    break;
                                case "4":
                                    fee = 20;
                                    break;
                                case "5":
                                    fee = 20;
                                    break;
                                case "6":
                                    fee = 20;
                                    break;
                                case "7":
                                    fee = 50;
                                    break;
                                case "8":
                                    fee = 20;
                                    break;
                            }
                            statement_flag = 0;
                            break;
                    }
                    resu = WsUtil.Walfare.SaveGroupPaidNew(state.SsWsPass, state.SsApplication, "w_sheet_wc_walfare_paid.pbl", xmlMain, state.SsUsername, state.SsBranchId, statement_flag, period, fee, fee_flag);
                    if (resu)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการสำเร็จ");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ");
                    }
                }
                else
                {
                    Dperiod = DWCtrl.GetItemDecimal(1, "period");
                    period = Convert.ToInt32(Dperiod);

                    resu = WsUtil.Walfare.SaveWaitGroupPaid(state.SsWsPass, state.SsApplication, "w_sheet_wc_walfare_paid.pbl", xmlMain, period);
                    if (resu)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการสำเร็จ");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ");
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("" + ex);
            }
            JSPeriodChange();
        }

        public void WebSheetLoadEnd()
        {
            tDwCtrl.Eng2ThaiAllRow();
            DWCtrl.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void JSSelectCode()
        {
            switch (HdSelectCode.Value)
            {
                case "approve":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemDecimal(i+1, "status", 2);
                    }
                    break;
                case "wait":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "status", 8);
                    }
                    break;
                case "cancle":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "cancal_status", 1); //9
                    }
                    break;                    
            }
            HdSelectCode.Value = "";
        }

        private void JSPeriodChange()
        {
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            try
            {
                decimal period;
                try
                {
                    period = DWCtrl.GetItemDecimal(1, "period");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกรายการก่อนทำรายการ");
                    return;
                }
                //DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_walfare_paid.pbl", null, state.SsBranchId, period);
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();

                String SQLcon = "", member_no, member_nos, member_noe, deptaccount_no, deptaccount_name, deptaccount_sname, st_tdate, end_tdate, deptacc_st, deptacc_end;
                string st_DateEN, end_DateEN;
                DateTime deptopen_date;
                try
                {
                    member_no = WebUtil.MemberNoFormat(DWCtrl.GetItemString(1, "member_no"));
                }
                catch { member_no = ""; }

                try
                {
                    member_nos = DWCtrl.GetItemString(1, "member_nos");
                    member_noe = DWCtrl.GetItemString(1, "member_noe");
                }
                catch
                {
                    member_nos = "";
                    member_noe = "";
                }
                try
                {
                    deptaccount_no = WebUtil.MemberNoFormat(DWCtrl.GetItemString(1, "deptaccount_no"));
                }
                catch { deptaccount_no = ""; }
                try
                {
                    deptaccount_name = DWCtrl.GetItemString(1, "deptaccount_name");
                }
                catch { deptaccount_name = ""; }
                try
                {
                    deptaccount_sname = DWCtrl.GetItemString(1, "deptaccount_sname");
                }
                catch { deptaccount_sname = ""; }
                try
                {
                    deptopen_date = DWCtrl.GetItemDateTime(1, "deptopen_date");
                }
                catch { deptopen_date = DateTime.MinValue; }

                try
                {
                    st_tdate = DWCtrl.GetItemString(1, "st_tdate");
                   // st_DateEN = st_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(st_tdate.Substring(6, 4)) - 543);
                    end_tdate = DWCtrl.GetItemString(1, "end_tdate");
                   // end_DateEN = end_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(end_tdate.Substring(6, 4)) - 543);
                }
                catch 
                { 
                    st_tdate ="";
                    end_tdate = "";
                }

                try
                {
                    deptacc_st = DWCtrl.GetItemString(1, "deptacc_st");
                    deptacc_end = DWCtrl.GetItemString(1, "deptacc_end");
                }
                catch 
                {
                    deptacc_st = "";
                    deptacc_end = "";
                }

                


                SQLcon = SQLcon + " and wcrecievemonth.recv_period = '" + period + "'";
                if (member_no != "")
                {
                    SQLcon = SQLcon + " and trim(wcdeptmaster.member_no) = '" + member_no.Trim() + "'";
                }
                if (deptaccount_no != "")
                {
                    SQLcon = SQLcon + " and trim(wcrecievemonth.wfmember_no) = '" + deptaccount_no.Trim() + "'";
                }

                if (deptaccount_name != "")
                {
                    SQLcon = SQLcon + " and wcdeptmaster.deptaccount_name like '%" + deptaccount_name + "%'";
                }

                if (deptaccount_sname != "")
                {
                    SQLcon = SQLcon + " and wcdeptmaster.deptaccount_sname like '%" + deptaccount_sname + "%'";
                }
                if (deptopen_date != DateTime.MinValue)
                {
                    SQLcon = SQLcon + " and wcdeptmaster.deptopen_date = to_date('" + deptopen_date.ToString("ddMMyyyy") + "', 'ddmmyyyy')";
                }

                if (st_tdate != "" && end_tdate != "")
                {
                    st_DateEN = st_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(st_tdate.Substring(6, 4)) - 543);
                    end_DateEN = end_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(end_tdate.Substring(6, 4)) - 543);
                    SQLcon = SQLcon + " and wcdeptmaster.deptopen_date between to_date('" + st_DateEN + "','dd/mm/yyyy') and to_date('" + end_DateEN + "','dd/mm/yyyy')";
                }

                if (deptacc_st != "" && deptacc_end != "")
                {
                    SQLcon = SQLcon + " and trim(wcrecievemonth.wfmember_no) between '" + deptacc_st.Trim() + "' and '" + deptacc_end.Trim() + "'";
                }

                if (member_nos != "" && member_noe != "")
                {
                    SQLcon = SQLcon + " and trim(wcrecievemonth.member_no) between '" + member_nos.Trim() + "' and '" + member_noe.Trim() + "'";
                }
                String sqlSost = "";
                String sort_sele = DWCtrl.GetItemString(1, "sort_sele");
                if (sort_sele == "1")
                {
                    sqlSost = "order by wfmember_no";
                }
                else
                {
                    sqlSost = "order by member_no";
                }

                String SQL;
                if (SQLcon == "")
                {
                    SQL = SQLBegin + " and wcrecievemonth.branch_id = '" + state.SsBranchId + "' " + sqlSost ;
                }
                else
                {
                    SQL = SQLBegin + SQLcon + " and wcrecievemonth.branch_id = '" + state.SsBranchId + "'" + sqlSost;

                }
                try
                {
                    DwMain.SetSqlSelect(SQL);
                    DwMain.Retrieve();
                    SQLCA.Disconnect();
                }
                catch
                {
                    SQLCA.Disconnect();
                    LtServerMessage.Text = WebUtil.ErrorMessage("แก้ข้อผิดพลาด ไม่สามารถค้นหาข้อมูลได้");
                }
            }
            catch (Exception ex)
            {
                SQLCA.Disconnect();
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}