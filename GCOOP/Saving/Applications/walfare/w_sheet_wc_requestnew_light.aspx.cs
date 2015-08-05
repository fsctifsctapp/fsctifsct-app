using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using System.Data;
using DBAccess;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_requestnew_light : PageWebSheet, WebSheet
    {
        private string pbl = "w_sheet_wc_walfare_new.pbl";
        protected string postTest;
        protected string postRequestDocNo;
        protected string postMemberNo;
        protected string postCardPerson;
        protected string postSaveSheet;
        protected string postwfmembertype;

        public void InitJsPostBack()
        {
            postTest = WebUtil.JsPostBack(this, "postTest");
            postRequestDocNo = WebUtil.JsPostBack(this, "postRequestDocNo");
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            postCardPerson = WebUtil.JsPostBack(this, "postCardPerson");
            postSaveSheet = WebUtil.JsPostBack(this, "postSaveSheet");
            postwfmembertype = WebUtil.JsPostBack(this, "postwfmembertype");
        }

        public void WebSheetLoadBegin()
        {
            //WAP5400021
            HdExtraSearchCode.Value = "";
            HdExtraSearchMode.Value = "";
            HdSaveStatus.Value = "";
            if (!IsPostBack)
            {
                UcMain1.InitUcMain(state);
                UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                UcOther1.InitUcOther(state.SsBranchId);

                HdForceSave.Value = "";
            }
            else
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRequestDocNo")
            {
                JsPostRequestDocNo();
            }
            else if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }
            else if (eventArg == "postCardPerson")
            {
                JsPostCardPerson();
            }
            else if (eventArg == "postSaveSheet")
            {
                SaveWebSheet();
            }
            else if (eventArg == "")
            {
                ChangeMembType();
            }
        }

        public void SaveWebSheet()
        {            
            DataTable dtMain = UcMain1.GetDataTable();
            DataTable dtOther = UcOther1.GetDataTable();
            string wftype_code = dtMain.Rows[0]["wftype_code"].ToString();

            ///สมาชิกสมทบ สสธท
            if ((state.SsCsType == "6" && wftype_code == "02") || (state.SsCsType == "1" && (wftype_code == "03" || wftype_code == "08" || wftype_code == "10" || wftype_code == "12")) || (state.SsCsType == "8" && (wftype_code == "03" || wftype_code == "05" || wftype_code == "06")) || (state.SsCsType == "2" && wftype_code == "06"))
            {
                if (state.SsBranchId != "0084" && state.SsBranchId != "0266")
                {
                    dtMain.Rows[0]["member_no"] = "ส" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("00000");
                }
                else
                {
                    dtMain.Rows[0]["member_no"] = "ส" + dtMain.Rows[0]["member_no"].ToString();
                }
            }
           

            else if (state.SsCsType == "1" && wftype_code == "06")
            {
                dtMain.Rows[0]["member_no"] = "บ" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("00000");
            }
            else if (state.SsCsType == "1" && wftype_code == "13")
            {
                dtMain.Rows[0]["member_no"] = "ม" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("00000");
            }
            else if (state.SsCsType == "6" && wftype_code == "03")
            {
                dtMain.Rows[0]["member_no"] = "สF" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("0000");
            }
            else if (state.SsCsType == "6" && wftype_code == "04")
            {
                dtMain.Rows[0]["member_no"] = "สM" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("0000");
            }
            else if (state.SsCsType == "6" && wftype_code == "05")
            {
                dtMain.Rows[0]["member_no"] = "สS" + int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("0000");
            }
            else if(state.SsBranchId == "0270"){
                dtMain.Rows[0]["member_no"] = int.Parse(dtMain.Rows[0]["member_no"].ToString()).ToString("0000000");
            }
            else
            {
                dtMain.Rows[0]["member_no"] = WebUtil.MemberNoFormat(dtMain.Rows[0]["member_no"].ToString());
            }

           

            /// check อายุสูงสุดสมาชิก วันเกิดและวันที่คีย์สมัครในแต่ละรอบ
            dtMain.Rows[0]["entry_date"] = DateTime.Now;
            try
            {
                DateTime birthday = (DateTime)dtMain.Rows[0]["Wfbirthday_date"];

                String SqlMaxAge = "select * from wcmembertype where cs_type = '" + state.SsCsType + "' and wftype_code = '" + wftype_code + "'";
                Sdt dtAge = WebUtil.QuerySdt(SqlMaxAge);

                decimal birthday_year = birthday.Year;
                decimal birthday_month = birthday.Month;
                decimal birthday_day = birthday.Day;
                if (state.SsCsType == "6")
                {
                    if (DateTime.Today.Month < birthday_month)
                    {
                        birthday_year = birthday_year + 1;
                    }
                    else if (DateTime.Today.Month == birthday_month)
                    {
                        if (DateTime.Today.Day < birthday_day)
                        {
                            birthday_year = birthday_year + 1;
                        }
                    }
                }
                if (dtAge.Next())
                {
                    decimal minAge = dtAge.GetDecimal("min_age");
                    decimal maxAge = dtAge.GetDecimal("max_age");
                    if ((DateTime.Today.Year - birthday_year) > maxAge)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ สมาชิกคนนี้อายุเกินกำหนด");
                        return;
                    }
                    if ((DateTime.Today.Year - birthday_year) < minAge)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ สมาชิกคนนี้อายุน้อยกว่ากำหนด");
                        return;
                    }

                    ///วันที่คีย์สมัครในแต่ละรอบ
                    DateTime apply_date = (DateTime)dtMain.Rows[0]["apply_date"];
                    DateTime applyperiod_date = dtAge.GetDate("applyperiod_date");

                    if ((apply_date > applyperiod_date))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ เนื่องจากวันสมัคร เกินกำหนดการรับสมัคร");
                        return;
                    }
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดกรุณาตรวจสอบวันเกิด และ วันสมัคร");
                return;
            }

            ///คำนำหน้า
            string prename = Convert.ToString(dtMain.Rows[0]["prename_code"]);
            if (prename.Trim() == "00")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุคำนำหน้า");
                return;
            }
            string sex = Convert.ToString(dtMain.Rows[0]["sex"]);
            if (sex.Trim() == "")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุเพศ");
                return;
            }

            //return;
            string member_no = Convert.ToString(dtMain.Rows[0]["member_no"] );
            for (int i = dtOther.Rows.Count; i > 0; i--)
            {
                try
                {
                    String nameRelate = dtOther.Rows[i - 1]["name"].ToString().Trim();
                    if (nameRelate == "") throw new Exception();
                    int del_flag = Convert.ToInt32(dtOther.Rows[i - 1]["del_flag"]);
                    if (del_flag == 1) throw new Exception();
                }
                catch
                {
                    dtOther.Rows[i - 1].Delete();
                }
            }
            try{
                bool Pid;
                int foreigner_flag;
                for (int i = 0; i < dtOther.Rows.Count; i++)
                {
                    dtOther.Rows[i]["seq_no"] = i + 1;
                    foreigner_flag = Convert.ToInt32(dtOther.Rows[i]["foreigner_flag"]);
                    if (foreigner_flag == 0)
                    {
                        Pid = VerifyPeopleID(dtOther.Rows[i]["codept_id"].ToString());
                        if (Pid == false)
                        {
                            throw new Exception("เลขบัตรประชาชนผู้รับผลประโยชน์คนที่ " + (i + 1) + " ไม่ถูกต้อง");
                        }
                    }
                }
                //return;
                if (HdSaveType.Value == "Insert")
                {
                    try
                    {
                        string Schk_out_return = "";
                        if (HdForceSave.Value == "")
                        {
                            string[] listDB = WebUtil.GetConnectionstrings();
                            Schk_out_return = WsUtil.Walfare.CheckDuplicateDT(state.SsWsPass, state.SsApplication, pbl, dtMain, listDB, state.SsCsType, state.SsUserType);
                            
                            if (Schk_out_return == "")
                            {
                                HdForceSave.Value = "2";
                            }
                        }
                        if (HdForceSave.Value == "2")
                        {
                            String docNo = WsUtil.Walfare.SaveReqWalfareNewLight(state.SsWsPass, state.SsApplication, state.SsWorkDate, pbl, dtMain, UcSlip1.GetDataTable(), dtOther);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

                            string sqlslipno = "select deptslip_no from wcdeptslip where deptaccount_no = '" + docNo + "' and branch_id = '" + state.SsBranchId + "'";
                            Sdt dtslip = WebUtil.QuerySdt(sqlslipno);
                            if (dtslip.Next())
                            {
                                HdSlipNo.Value = dtslip.GetString("deptslip_no");
                                HdSaveStatus.Value = "1";
                            }

                            //UcMain1.Retrieve(state, docNo);
                            //UcSlip1.Retrieve(state, docNo);
                            //UcOther1.Retrieve(state, docNo);
                            //HdSaveType.Value = "Edit";
                            UcMain1.InitUcMain(state);
                            UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                            UcOther1.InitUcOther(state.SsBranchId);
                            HdSaveType.Value = "Insert";

                            HdForceSave.Value = "";
                        }
                        else
                        {
                            HdForceSave.Value = "1";
                            if(Schk_out_return != ""){
                            HdMassage.Value = Schk_out_return;
                            }
                        }
                        
                        
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                else if (HdSaveType.Value == "Edit")
                {
                    //LtServerMessage.Text = WebUtil.ErrorMessage("บันสมัครนี้ถูกบันทึกไปแล้วไม่สามารถบันทึกซ้ำได้ หากต้องการแก้ไขกรุณาใช้เมนู แก้ไขใบสมัคร");
                    //return;
                    try
                    {
                        String docNo = dtMain.Rows[0]["deptrequest_docno"].ToString();

                        DateTime DEntry_date = (DateTime)dtMain.Rows[0]["entry_date"];
                        string SEntry_date = DEntry_date.ToString("dd/MM/yyyy", WebUtil.EN);
                        String sql = "select * from wcreqdeposit where deptrequest_docno = '" + docNo + "'";
                        Sdt dt = WebUtil.QuerySdt(sql);
                        string DBDEnter_date = "";
                        if (dt.Next())
                        {
                            DBDEnter_date = dt.GetDate("entry_date").ToString("dd/MM/yyyy", WebUtil.EN);
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้ กรุณาใช้หน้าแก้ไขใบสมัครแทน");
                        }
                        if (SEntry_date == DBDEnter_date)
                        {

                            WsUtil.Walfare.UpdateReqWalfareNewLight(state.SsWsPass, state.SsApplication, dtMain, UcSlip1.GetDataTable(), dtOther);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลที่แก้ไขแล้วสำเร็จ");

                            //UcMain1.Retrieve(state, docNo);
                            //UcSlip1.Retrieve(state, docNo);
                            //UcOther1.Retrieve(state, docNo);
                            //HdSaveType.Value = "Edit";
                            UcMain1.InitUcMain(state);
                            UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                            UcOther1.InitUcOther(state.SsBranchId);
                            HdSaveType.Value = "Insert";
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถแก้ไขใบคำขอสมัครนี้ได้ เนื่องจากใบคำขอนี้บันทึกไปแล้วมากกว่า 1 วัน กรุณาใช้หน้าจอแก้ไขใบสมัคร");
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
            }catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            UcMain1.CheckAdminStatus();
            UcSlip1.CheckAdminStatus();
        }

        private void JsPostRequestDocNo()
        {
            String docNo = HdRequestDocNo.Value;
            UcMain1.Retrieve(state, docNo);
            UcSlip1.Retrieve(state, docNo);
            UcOther1.Retrieve(state, docNo);
            HdSaveType.Value = "Edit";
        }

        private void JsPostMemberNo()
        {
            String member = UcMain1.GetMemberNo();
            member = WebUtil.MemberNoFormat(member);
            String sql = "select deptrequest_docno from WCREQDEPOSIT where member_no='" + member + "' and branch_id='" + state.SsBranchId + "'";
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Rows.Count == 1)
            {
                dt.Next();
                HdRequestDocNo.Value = dt.GetString(0).Trim();
                JsPostRequestDocNo();
            }
            else if (dt.Rows.Count > 1)
            {
                UcMain1.InitUcMain(state);
                UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                UcOther1.InitUcOther(state.SsBranchId);
                HdSaveType.Value = "Insert";
                HdExtraSearchMode.Value = "member";
                HdExtraSearchCode.Value = member;
            }
            else
            {
                UcMain1.InitUcMain(state);
                UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                UcOther1.InitUcOther(state.SsBranchId);
                HdSaveType.Value = "Insert";
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขสมาชิก " + member);
            }
        }

        private void JsPostCardPerson()
        {
            String card = UcMain1.GetCardPerson();
            //card = WebUtil.MemberNoFormat(card);
            String sql = "select deptrequest_docno from WCREQDEPOSIT where card_person='" + card + "' and branch_id='" + state.SsBranchId + "'";
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Rows.Count == 1)
            {
                dt.Next();
                HdRequestDocNo.Value = dt.GetString(0).Trim();
                JsPostRequestDocNo();
            }
            else if (dt.Rows.Count > 1)
            {
                UcMain1.InitUcMain(state);
                UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                UcOther1.InitUcOther(state.SsBranchId);
                HdSaveType.Value = "Insert";
                HdExtraSearchMode.Value = "card";
                HdExtraSearchCode.Value = card;
            }
            else
            {
                UcMain1.InitUcMain(state);
                UcSlip1.InitUcSlip(state.SsBranchId, "01", 1);
                UcOther1.InitUcOther(state.SsBranchId);
                HdSaveType.Value = "Insert";
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขบัตรประชาชน " + card);
            }
        }

        private Boolean VerifyPeopleID(String PID)
        {
            PID = PID.Trim();
            //ตรวจสอบว่าทุก ๆ ตัวอักษรเป็นตัวเลข
            if (PID.ToCharArray().All(c => char.IsNumber(c)) == false)
            {
                return false;
            }
            //ตรวจสอบว่าข้อมูลมีทั้งหมด 13 ตัวอักษร
            if (PID.Trim().Length != 13)
            {
                return false;
            }
            //int sumValue = 0;

            //for (int i = 0; i < PID.Length - 1; i++)
            //{
            //    sumValue += int.Parse(PID[i].ToString()) * (13 - i);
            //}
            //int v = (11 - (sumValue % 11)) % 10;
            //String chk = PID[12].ToString();

            //return PID[12].ToString() == v.ToString();
            return true;
        }

        private void ChangeMembType()
        {

            UcMain1.wftype_code_SelectedIndexChanged();

            DataTable dtMain = UcMain1.GetDataTable();
            string wftype_code = dtMain.Rows[0]["wftype_code"].ToString();

            UcSlip1.InitUcSlip(state.SsBranchId, wftype_code, 1);
        }
    }
}