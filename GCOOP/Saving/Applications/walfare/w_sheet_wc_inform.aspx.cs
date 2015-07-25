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
    public partial class w_sheet_wc_inform : PageWebSheet, WebSheet
    {
        private String pbl = "w_sheet_wc_inform.pbl";
        protected String jsInitinform;
        protected String jsResignChange;
        protected String jsInitinformOld;
        protected String jssetDataResignChange;

        private DwThDate tDwMain;
        public void InitJsPostBack()
        {
            jsInitinform = WebUtil.JsPostBack(this, "jsInitinform");
            jsResignChange = WebUtil.JsPostBack(this, "jsResignChange");
            jsInitinformOld = WebUtil.JsPostBack(this, "jsInitinformOld");
            jssetDataResignChange = WebUtil.JsPostBack(this, "jssetDataResignChange");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("inform_date", "inform_tdate");
            tDwMain.Add("die_date", "die_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "inform_date", DateTime.Today);
                DwMain.SetItemDateTime(1, "die_date", DateTime.Today);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsInitinform":
                    //DwMain.SetItemString(1, "inform_tdate", DateTime.Now.ToString("ddMMyyyy"));
                    Initinform();
                    break;
                case "jsResignChange":
                    SetDatetime();
                    DwMain.SetItemDecimal(1, "quantitymem_amt", 0);
                    ResignChange();
                    break;
                case "jsInitinformOld":
                    InitinformOld();
                    break;
                case "jssetDataResignChange":
                    try
                    {
                        string wftyep_code = DwMain.GetItemString(1, "wftype_code");
                        String sql = "select cremate_amt from wcmembertype where wftype_code = " + wftyep_code;
                        Sdt dt = WebUtil.QuerySdt(sql);
                        if (dt.Next())
                        {
                            setDataResignChange(dt.GetDecimal("cremate_amt"));
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                    break;
            }
        }

        public void SaveWebSheet()
        {
            if (HdStatus.Value == "1")
            {
                try
                {
                    try
                    {
                        DwMain.GetItemString(1, "resigncause_code");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุสาเหตุการออก");
                    }
                    String informDateThai, dieDateThai;
                    try
                    {
                        informDateThai = DwUtil.GetString(DwMain, 1, "inform_tdate", "").Trim();
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุวันที่แจ้ง");
                        return;
                    }
                    if (informDateThai.Length == 8)
                    {
                        DwMain.SetItemDateTime(1, "inform_date", DateTime.ParseExact(informDateThai, "ddMMyyyy", WebUtil.TH));
                    }
                    dieDateThai = DwUtil.GetString(DwMain, 1, "die_tdate", "").Trim();
                    if (dieDateThai.Length == 8)
                    {
                        DwMain.SetItemDateTime(1, "die_date", DateTime.ParseExact(dieDateThai, "ddMMyyyy", WebUtil.TH));
                    }
                    String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");

                    if (HdModeSave.Value != "edit")
                    {
                        String sql = "select * from wcreqchg_dept where deptaccount_no = '" + DwMain.GetItemString(1, "deptaccount_no") + "' and branch_id = '" + state.SsBranchId + "' and reqchg_status <> -9";
                        Sdt dtchkUniqe = WebUtil.QuerySdt(sql);
                        if (dtchkUniqe.Next())
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกคนนี้มีใบคำขอลากออกแล้ว ไม่สามารถทำรายการได้");
                        }
                        else
                        {
                            ///Save Insert
                            int result = Saveinform("insert");
                            if (result == 1)
                            {
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                            }
                            else
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สมารถทำรายการได้");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            ///Save Edit
                            int result = Saveinform("edit");
                            if (result == 1)
                            {
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                            }
                            else
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สมารถทำรายการได้");
                            }
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้ " + ex);
                        }
                    }

                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("" + ex); }
                HdStatus.Value = "";
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามาบันทึกข้อมูลได้ เนื่องจากใบคำขอแจ้งลาออก/เสียชีวิตนี้ได้รับการอนุมัติแล้ว");
            }
            HdModeSave.Value = "";
        }

        public void WebSheetLoadEnd()
        {
            if (state.SsUserType == 1 || state.SsUserType == 3)
            {
                try
                {
                    DwMain.Modify("inform_tdate.Protect=0");
                }
                catch { }
                try
                {
                    string backColor = DwMain.Describe("acc_name.Background.Color");
                    DwMain.Modify("inform_tdate.Background.Color=" + backColor);
                }

                catch { }
            }
            else
            {
            }
            //try
            //{
            //    DwUtil.RetrieveDDDW(DwMain, "resigncause_code", pbl, null);
            //}
            //catch
            //{
            //}
            //this.SetFocus(DwMain);
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        private void SetDatetime()
        {
            String informDateThai = "", dieDateThai = "";
            try
            {
                informDateThai = DwUtil.GetString(DwMain, 1, "inform_tdate", "").Trim();
            }
            catch
            {
            }
            if (informDateThai.Length == 8)
            {
                DwMain.SetItemDateTime(1, "inform_date", DateTime.ParseExact(informDateThai, "ddMMyyyy", WebUtil.TH));
            }
            try
            {
                dieDateThai = DwUtil.GetString(DwMain, 1, "die_tdate", "").Trim();
            }
            catch
            {
            }
            if (dieDateThai.Length == 8)
            {
                DwMain.SetItemDateTime(1, "die_date", DateTime.ParseExact(dieDateThai, "ddMMyyyy", WebUtil.TH));
            }
        }
        private void Initinform()
        {
            int accNo;
            String deptaccount_no, acc_no, sql;
            String wfaccount_name = "";
            String member_no = "";
            String wcmembertype_desc = "";
            String wftype_code = "";
            String wftype_desc = "";
            Decimal cremate_amt = 0;
            int result = 0;
            Sdt dt;
            DwMain.SetItemString(1, "cs_type", state.SsCsType);
            try
            {
                accNo = Convert.ToInt32(DwMain.GetItemString(1, "deptaccount_no"));
                acc_no = Convert.ToString(accNo);
                deptaccount_no = WebUtil.MemberNoFormat(acc_no);
                bool MasterResult = chkMaster(deptaccount_no);
                if (!MasterResult)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากสมาชิกถูกยกเลิกทะเบียน");
                    return;
                }
                result = chkReqcng_Dept(deptaccount_no);
                if (result == 0)
                {
                    sql = @"select a.*,b.wcmembertype_desc,b.cremate_amt,b.paybffuture_amt 
                    from wcdeptmaster a, wcmembertype b  
                    where a.wftype_code = b.wftype_code and 
                    a.deptaccount_no='" + deptaccount_no + @"' and 
                    a.branch_id='" + state.SsBranchId + "'";
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        deptaccount_no = dt.GetString("deptaccount_no");
                        wfaccount_name = dt.GetString("wfaccount_name");
                        member_no = dt.GetString("member_no");
                        wftype_code = dt.GetString("wftype_code");
                        wftype_desc = dt.GetString("wcmembertype_desc");
                        wcmembertype_desc = dt.GetString("wcmembertype_desc");
                        cremate_amt = dt.GetDecimal("cremate_amt");

                        //DateTime now = DateTime.Now;
                        //String ddMM = DateTime.Now.ToString("ddMM");
                        //int yyyy = now.Year + 543;
                        //string today = "" + ddMM + "" + yyyy;
                        //string Sdeptopen_date = "";

                        //try
                        //{
                        //    string tinform_date = DwMain.GetItemString(1, "inform_tdate");
                        //    if (tinform_date.Length == 8)
                        //    {
                        //        DateTime informdate = DateTime.ParseExact(tinform_date, "ddMMyyyy", WebUtil.TH);
                        //        DwMain.SetItemDateTime(1, "inform_date", informdate);
                        //    }

                        //DateTime Ddeptopen_date = DwMain.GetItemDateTime(1, "inform_tdate");
                        //Sdeptopen_date = Ddeptopen_date.ToString("ddMMyyyy");

                        //}
                        //catch 
                        //{ 
                        //    Sdeptopen_date = today;
                        //    DwMain.SetItemString(1, "inform_tdate", Sdeptopen_date);
                        //}

                        DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

                        //DwMain.SetItemString(1, "die_tdate", today);
                        DwMain.SetItemString(1, "wftype_code", wftype_code);
                        DwMain.SetItemString(1, "wftype_desc", wftype_desc);
                        DwMain.SetItemString(1, "acc_name", wfaccount_name);
                        DwMain.SetItemString(1, "member_no", member_no);

                        setDataResignChange(cremate_amt);
                        HdStatus.Value = "1";
                    }
                    else
                    {
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        DwMain.SetItemDateTime(1, "inform_date", DateTime.Today);
                        DwMain.SetItemDateTime(1, "die_date", DateTime.Today);
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากไม่มีเลขทะเบียนสมาชิก " + deptaccount_no);
                    }
                }
                else
                {
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwMain.SetItemDateTime(1, "inform_date", DateTime.Today);
                    DwMain.SetItemDateTime(1, "die_date", DateTime.Today);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากเลขทะเบียนสมาชิก " + deptaccount_no + " มีใบคำขอลาออกแล้ว");
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิก");
            }

        }
        private void ResignChange()
        {
            String resignChg = "";
            try
            {
                resignChg = DwMain.GetItemString(1, "resigncause_code");
                switch (resignChg)
                {
                    case "01":
                        setDataResignChange(0);
                        break;
                    case "02":
                        Initinform();
                        break;
                }
            }
            catch { }

        }

        private void setDataResignChange(Decimal cremate_amt)
        {
            String sql2, sql3;
            Decimal total_memb = 0;
            Decimal percent1 = 0;
            Decimal percent2 = 0;
            decimal die_status = 0;
            string Sdeptopen_date = "";

            Sdt dt2, dt3;
            //int chkdie_status = chkDiestatus();

            sql2 = "select * from wcucffeepay_rate";
            dt2 = WebUtil.QuerySdt(sql2);
            if (dt2.Next())
            {
                percent1 = dt2.GetDecimal("percent_fee");
                percent2 = dt2.GetDecimal("percent_fee2");
                die_status = dt2.GetDecimal("die_status");
            }
            try
            {
                DateTime Ddeptopen_date = DwMain.GetItemDateTime(1, "inform_date");
                Sdeptopen_date = Ddeptopen_date.ToString("dd/MM/yyyy");
            }
            catch
            {
                Sdeptopen_date = DateTime.Now.ToString("dd/MM/yyyy");
            }

            total_memb = DwMain.GetItemDecimal(1, "quantitymem_amt");

            if (total_memb == 0)
            {
                sql3 = "select count(*) as total_memb from wcdeptmaster where wfmember_status = 1 and deptopen_date <= to_date('" + Sdeptopen_date + "','dd/mm/yyyy')";
                dt3 = WebUtil.QuerySdt(sql3);
                if (dt3.Next()) total_memb = Convert.ToDecimal(dt3.GetString("total_memb"));

            }

            if (die_status == 1)
            {
                cremate_amt = cremate_amt * total_memb;
            }
            else
            {
                cremate_amt = cremate_amt * 1;
            }

            Decimal pt1 = percent1 * cremate_amt;
            Decimal pt2 = percent2 * cremate_amt;
            Decimal withdrawable_amt = cremate_amt - (pt1 + pt2);

            DwMain.SetItemDecimal(1, "cremateestpay_amt", cremate_amt);
            DwMain.SetItemDecimal(1, "wfe_amt", pt1);
            DwMain.SetItemDecimal(1, "wfe_amt2", pt2);
            DwMain.SetItemDecimal(1, "quantitymem_amt", total_memb);
            DwMain.SetItemDecimal(1, "withdrawable_amt", withdrawable_amt);
        }

        private int Saveinform(string SaveMode)
        {
            Sta ta = new Sta(state.SsConnectionString);
            ta.Transection();
            try
            {

                Decimal die_status = 0;
                String remark;

                DocumentControl dct = new DocumentControl();
                String ReqchgNo = dct.NewDocumentNo("WCCHGDOCNO", DateTime.Now.Year, ta);

                String die_code = DwMain.GetItemString(1, "resigncause_code");
                if (die_code == "02")
                {
                    die_status = 1;
                }
                else
                {
                    die_status = 0;
                }
                Int32 deptaccount_no_int = Convert.ToInt32(DwMain.GetItemString(1, "deptaccount_no"));
                String deptaccount_no = Convert.ToString(deptaccount_no_int);
                try
                {
                    remark = DwMain.GetItemString(1, "remark");
                }
                catch
                {
                    remark = "";
                }
                String today = "'" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                String die_date = "'" + DwMain.GetItemDateTime(1, "die_date").ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                String inform_date = "'" + DwMain.GetItemDateTime(1, "inform_date").ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                String resigncause_code = DwMain.GetItemString(1, "resigncause_code");

                Decimal approve_flag = 0;//DwMain.GetItemDecimal(1, "approve_flag");
                String member_no = DwMain.GetItemString(1, "member_no").Trim();
                Decimal quantitymem_amt = DwMain.GetItemDecimal(1, "quantitymem_amt");
                Decimal cremateestpay_amt = DwMain.GetItemDecimal(1, "cremateestpay_amt");
                String wftype_code = DwMain.GetItemString(1, "wftype_code");
                Decimal withdrawable_amt = DwMain.GetItemDecimal(1, "withdrawable_amt");
                Decimal wfe_amt = DwMain.GetItemDecimal(1, "wfe_amt");
                Decimal wfe_amt2 = DwMain.GetItemDecimal(1, "wfe_amt2");

                if (SaveMode != "edit")
                {
                    String sql = @"INSERT INTO wcreqchg_dept (dpreqchg_doc, deptaccount_no, approve_flag, remark, entry_date, entry_id, die_date, inform_date, resigncause_code,";
                    if (approve_flag == 1)
                    {
                        sql = sql + " approve_date,";
                    }
                    sql = sql + " reqchg_status, member_no, quantitymem_amt, cremateestpay_amt, wftype_code, die_status, branch_id, withdrawable_amt, wfe_amt, wfe_amt2)";
                    sql = sql + "VALUES ('" + ReqchgNo + "','" + WebUtil.MemberNoFormat(deptaccount_no) + "'," + approve_flag + ",'" + remark + "', to_date(" + today + ",'" + state.SsUsername + "', to_date(" + die_date + ", to_date(" + inform_date + ",'" + resigncause_code + "',";
                    if (approve_flag == 1)
                    {
                        sql = sql + "to_date(" + today + ",";
                    }
                    sql = sql + "" + 8 + ",'" + member_no + "'," + quantitymem_amt + "," + cremateestpay_amt + ",'" + wftype_code + "'," + die_status + ",'" + state.SsBranchId + "'," + withdrawable_amt + "," + wfe_amt + "," + wfe_amt2 + ")";

                    ta.Exe(sql);
                }
                else
                {
                    string regchg_docno_old = DwMain.GetItemString(1, "dpreqchg_doc");
                    String sql_update = "UPDATE wcreqchg_dept SET inform_date = to_date(" + inform_date + ", die_date = to_date(" + die_date + ", resigncause_code = '" + resigncause_code + @"', 
                    remark = '" + remark + "', quantitymem_amt =" + quantitymem_amt + ", cremateestpay_amt =" + cremateestpay_amt + @", die_status = " + die_status + @",
                    wfe_amt =" + wfe_amt + ", wfe_amt2 =" + wfe_amt2 + ", withdrawable_amt =" + withdrawable_amt + @" where dpreqchg_doc = 
                    '" + regchg_docno_old + @"' and branch_id = '" + state.SsBranchId + "'";

                    ta.Exe(sql_update);
                }
                /*
                String msql = "update wcdeptmaster set deptclose_status=" + 1 +
                            @", deptclose_date=to_date(" + today +
                            @", lastaccess_date=to_date(" + today +
                            @", last_process_date=to_date(" + today +
                            @", wfmember_status=" + -1 +
                            @", die_date=to_date(" + die_date +
                            @", wfcarcass_seq='" + quantitymem_amt + "'" +
                            @", resigncause_code='" + resigncause_code + "'";
                if (approve_flag == 1)
                {
                    msql = msql + ", apply_date=  to_date(" + today;
                }
                msql = msql + @", withdrawable_amt=" + withdrawable_amt +
                @", cremateapp_amt=" + cremateestpay_amt;
                if (approve_flag == 1)
                {
                    msql = msql + ", kpreceive_status=" + 1;
                } 
                else
                {
                    msql = msql + ", kpreceive_status=" + -1;
                }
                msql = msql + " where deptaccount_no='" + WebUtil.MemberNoFormat(deptaccount_no) + "' and branch_id='" + state.SsBranchId + "'";
                ta.Exe(msql);
                */
                ta.Commit();
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.RollBack();
                ta.Close();
                throw ex;
            }
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemDateTime(1, "inform_date", DateTime.Today);
            DwMain.SetItemDateTime(1, "die_date", DateTime.Today);
            return 1;
        }

        private int chkReqcng_Dept(String acc_no)
        {
            String deptaccount_no = "";
            Sdt dt;
            try
            {
                String sql = "select deptaccount_no from wcreqchg_dept where deptaccount_no='" + acc_no + "' and branch_id='" + state.SsBranchId + "' and reqchg_status <> -9";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    deptaccount_no = dt.GetString("deptaccount_no").Trim();
                }
                if (deptaccount_no == acc_no && HdModeSave.Value != "edit")
                {
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        private bool chkMaster(string deptaccount_no)
        {
            try
            {
                String sql = "select deptaccount_no from wcdeptmaster where deptaccount_no='" + deptaccount_no + "' and branch_id='" + state.SsBranchId + "' and resigncause_code = '03'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void InitinformOld()
        {
            try
            {
                String dpreqchg_doc = DwMain.GetItemString(1, "dpreqchg_doc");
                DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, dpreqchg_doc, state.SsBranchId, state.SsCsType);


                decimal approve_flag = DwMain.GetItemDecimal(1, "approve_flag");
                if (approve_flag == 1)
                {
                    HdStatus.Value = "0";
                    LtServerMessage.Text = WebUtil.WarningMessage("ใบคำขอลาออก/แจ้งเสียชีวิตนี้ได้รับการอนุมัติไปแล้วไม่สามารถแก้ไขได้");
                }
                else
                {
                    HdStatus.Value = "1";
                    HdModeSave.Value = "edit";
                }


            }
            catch { }
        }

    }
}