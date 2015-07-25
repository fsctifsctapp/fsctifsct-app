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
using System.Globalization;
using GcoopServiceCs;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_walfare_reqedit : PageWebSheet, WebSheet
    {
        //Session["Xml"] = null;
        private String XmlMain;
        private String XmlCompare;

        private string auditDocno;
        private int TauditStatus = 0;

        private String pbl = "w_sheet_wc_walfare_reqedit.pbl";
        private DwThDate tDwMain;
        protected String postPost;
        protected String postProvince;
        protected String other_postPost;
        protected String other_postProvince;
        protected String postInitcardPS;
        protected String postInitmembNo;
        protected String jsAddRelateRow;
        protected String jsDeleteRelateRow;
        protected String jsInitWalfareReq;
        protected String jsChangeMemtype;
        

        public void InitJsPostBack()
        {
            postPost = WebUtil.JsPostBack(this, "postPost");
            postProvince = WebUtil.JsPostBack(this, "postProvince");
            other_postPost = WebUtil.JsPostBack(this, "other_postPost");
            other_postProvince = WebUtil.JsPostBack(this, "other_postProvince");
            postInitcardPS = WebUtil.JsPostBack(this, "postInitcardPS");
            postInitmembNo = WebUtil.JsPostBack(this, "postInitmembNo");
            jsAddRelateRow = WebUtil.JsPostBack(this, "jsAddRelateRow");
            jsDeleteRelateRow = WebUtil.JsPostBack(this, "jsDeleteRelateRow");
            jsInitWalfareReq = WebUtil.JsPostBack(this, "jsInitWalfareReq");
            jsChangeMemtype = WebUtil.JsPostBack(this, "jsChangeMemtype");

            //-----------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("apply_date", "apply_tdate");
            tDwMain.Add("wfbirthday_date", "birthday_tdate");
            tDwMain.Add("deptopen_date", "deptopen_tdate");
        }

        public void WebSheetLoadBegin()
        {
            HdchkMembRow.Value = "false";
            HdchkcardPs.Value = "false";
            HdMember_no.Value = "";
            HdcardPs.Value = "";

            ///slip ใบเสร็จ
            HdSaveStatus.Value = "";
            HdFromMaster.Value = "";

            if (!IsPostBack)
            {
                Session["xml_before_audit"] = null;
                Session["deptslip_no"] = null;
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "deptaccount_no", state.SsBranchId);
                DwMain.SetItemString(1, "membgroup_code", state.SsBranchId);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                DwMain.SetItemString(1, "province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "postcode", state.SsPostCode);
                //DwMain.SetItemDateTime(1, "apply_date", state.SsWorkDate);

                tDwMain.Eng2ThaiAllRow();

                String wftypeCode = DwUtil.GetString(DwMain, 1, "wftype_code", "");
                if (wftypeCode != "")
                {
                    String sql = "select * from wcmembertype where wftype_code='" + wftypeCode + "' and cs_type = '" + state.SsCsType + "'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(1, "deptitemtype_desc", "FEE - ค่าธรรมเนียมสมัครใหม่");
                        DwSlip.SetItemDecimal(1, "amt", dt.GetDecimal("feeappl_amt"));
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(2, "deptitemtype_desc", "WFY - ค่าธรรมเนียมรายปี");
                        DwSlip.SetItemDecimal(2, "amt", dt.GetDecimal("feeperyear_amt"));
                        DwSlip.InsertRow(0);
                        DwSlip.SetItemString(3, "deptitemtype_desc", "WPF - เงินสงเคราะห์ศพล่วงหน้า");
                        DwSlip.SetItemDecimal(3, "amt", dt.GetDecimal("paybffuture_amt"));
                    }
                }
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
            else if (eventArg == "other_postProvince")
            {
                Jsother_PostProvince();
            }
            else if (eventArg == "postInitcardPS")
            {
                JsInitWalfareReq("card_person");
            }
            else if (eventArg == "postInitmembNo")
            {
                JsInitWalfareReq("member_no");
            }
            else if (eventArg == "jsAddRelateRow")
            {
                JsAddRelateRow();
            }
            else if (eventArg == "jsDeleteRelateRow")
            {
                JsDeleteRelateRow();
            }
            else if (eventArg == "jsInitWalfareReq")
            {
                JsInitWalfareReq("");
            }
            else if (eventArg == "jsChangeMemtype")
            {
                ChangeMemtype();
            }
            else if (eventArg == "postPost")
            {
                try
                {
                    DwMain.SetItemString(1, "postcode", "");
                }
                catch { }
            }
            else if (eventArg == "other_postPost")
            {
                try
                {
                    DwMain.SetItemString(1, "other_postcode", "");
                }
                catch { }
            }
        }

        public void SaveWebSheet()
        {
            
            string prename_code = DwMain.GetItemString(1, "prename_code");
            if (prename_code == "00")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุคำนำหน้า");
                return;
            }
            string prename_desc = "";
            String sql = "select prename_desc from mbucfprename where prename_code='" + prename_code + "'";
            Sdt dtable = WebUtil.QuerySdt(sql);
            if (dtable.Next())
            {
                prename_desc = dtable.GetString("prename_desc");
            }
            string fullname = prename_desc + "" + DwMain.GetItemString(1, "deptaccount_name") + "  " + DwMain.GetItemString(1, "deptaccount_sname");
            DwMain.SetItemString(1, "wfaccount_name", fullname);
            try
            {
                string tapply_date = DwMain.GetItemString(1, "apply_tdate");
                if (tapply_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tapply_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "apply_date", dt);
                }
            }
            catch { }

            try
            {
                string tbirth_date = DwMain.GetItemString(1, "birthday_tdate");
                if (tbirth_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tbirth_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "wfbirthday_date", dt);
                }
            }
            catch { }
            try
            {
                string tdeptopen_date = DwMain.GetItemString(1, "deptopen_tdate");
                if (tdeptopen_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tdeptopen_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "deptopen_date", dt);
                }
            }
            catch { }
            try
            {

                string membNO;
                if ((state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "02") || (state.SsCsType == "1" && (DwMain.GetItemString(1, "wftype_code") == "03" || DwMain.GetItemString(1, "wftype_code") == "08" || DwMain.GetItemString(1, "wftype_code") == "12")) || (state.SsCsType == "8" && (DwMain.GetItemString(1, "wftype_code") == "03" || DwMain.GetItemString(1, "wftype_code") == "05" || DwMain.GetItemString(1, "wftype_code") == "06")))
                {
                    if (state.SsBranchId != "0084")
                    {
                        membNO = "ส" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("00000");
                    }
                    else
                    {
                        membNO = "ส" + DwMain.GetItemString(1, "member_no");
                    }
                }
                else if (state.SsCsType == "1" && DwMain.GetItemString(1, "wftype_code") == "06")
                {
                    membNO = "บ" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("00000"); ;
                }
                else if (state.SsCsType == "1" && DwMain.GetItemString(1, "wftype_code") == "13")
                {
                    membNO = "ม" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("00000"); ;
                }
                else if (state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "03")
                {
                    membNO = "สF" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("0000"); ;
                }
                else if (state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "04")
                {
                    membNO = "สM" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("0000"); ;
                }
                else if (state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "05")
                {
                    membNO = "สS" + int.Parse(DwMain.GetItemString(1, "member_no")).ToString("0000"); ;
                }
                else if (state.SsBranchId == "0270")
                {
                    membNO = int.Parse(DwMain.GetItemString(1, "member_no")).ToString("00000000"); ;
                }
                else
                {
                    membNO = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no"));
                }
                DwMain.SetItemString(1, "member_no", membNO);
            }
            catch { }

            int approve_status = Convert.ToInt32(DwMain.GetItemDecimal(1, "approve_status"));

            try{
            /// check สถานะสมาชิก
                if (state.SsUserType == 1)
                {
                    if (HdActionStatus.Value == "add")
                    {
                        AddRelateRowReqEdit();
                    }
                    else if (HdActionStatus.Value == "del")
                    {
                        DeleteRelateRowReqEdit();
                    }
                    else
                    {
                        UpdateReqEdit();
                    }
                }
                else
                {
                    switch (approve_status)
                    {
                        case 1:
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้ได้รับการอนุมัติไปแล้ว");
                            break;
                        case -9:
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้ถูกยกเลิกใบคำขอสมัคร");
                            break;
                        default:
                            try
                            {
                                DateTime birthday = DwMain.GetItemDateTime(1, "wfbirthday_date");// (DateTime)dtMain.Rows[0]["Wfbirthday_date"];
                                string wftype_code = DwMain.GetItemString(1, "wftype_code");

                                String SqlMaxAge = "select * from wcmembertype where cs_type = '" + state.SsCsType + "' and wftype_code = '" + wftype_code + "'";
                                Sdt dtAge = WebUtil.QuerySdt(SqlMaxAge);

                                decimal birthday_year = birthday.Year;
                                decimal birthday_month = birthday.Month;
                                decimal birthday_day = birthday.Day;
                                if (state.SsCsType == "2" || state.SsCsType == "6")
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
                                    DateTime apply_date = DwMain.GetItemDateTime(1, "apply_date");// (DateTime)dtMain.Rows[0]["apply_date"];
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
                            if (DwMain.GetItemDecimal(1, "foreigner_flag") == 0)
                            {
                                if(!VerifyPeopleID(DwMain.GetItemString(1, "card_person"))){
                                    throw new Exception("เลขบัตรประชาชนไม่ถูกต้อง");
                                }
                            }
                            bool Pid;
                            decimal foreigner_flag;
                            for (int i = 0; i < DwRelate.RowCount; i++)
                            {
                                foreigner_flag = DwRelate.GetItemDecimal(i + 1, "foreigner_flag");
                                if (foreigner_flag == 0)
                                {
                                    Pid = VerifyPeopleID(DwRelate.GetItemString(i + 1, "codept_id"));
                                    if (Pid == false)
                                    {
                                        throw new Exception("เลขบัตรประชาชนผู้รับผลประโยชน์คนที่ " + (i + 1) + " ไม่ถูกต้อง");
                                    }
                                }
                            }
                            if ((state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "02") || (state.SsCsType == "1" && DwMain.GetItemString(1, "wftype_code") == "03"))
                            {
                            }
                            else
                            {
                                string memb_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no"));
                                DwMain.SetItemString(1, "member_no", memb_no);
                            }
                            if (HdActionStatus.Value == "add")
                            {
                                AddRelateRowReqEdit();
                            }
                            else if (HdActionStatus.Value == "del")
                            {
                                DeleteRelateRowReqEdit();
                            }
                            else
                            {
                                UpdateReqEdit();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            Session["xml_before_audit"] = null;
            TauditStatus = 0;

            DwMain.Reset();
            DwSlip.Reset();
            DwRelate.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "deptaccount_no", state.SsBranchId);
            DwMain.SetItemString(1, "membgroup_code", state.SsBranchId);
            DwMain.SetItemString(1, "branch_id", state.SsBranchId);
            DwMain.SetItemString(1, "province_code", state.SsProvinceCode);
            DwMain.SetItemString(1, "ampher_code", state.SsDistrictCode);
            DwMain.SetItemString(1, "postcode", state.SsPostCode);
            //DwMain.SetItemDateTime(1, "apply_date", state.SsWorkDate);


            String wftypeCode = DwUtil.GetString(DwMain, 1, "wftype_code", "");
            if (wftypeCode != "")
            {
                String wfsql = "select * from wcmembertype where wftype_code='" + wftypeCode + "' and cs_type = '" + state.SsCsType + "'";
                Sdt dt = WebUtil.QuerySdt(wfsql);
                if (dt.Next())
                {
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(1, "deptitemtype_desc", "FEE - ค่าธรรมเนียมสมัครใหม่");
                    DwSlip.SetItemDecimal(1, "amt", dt.GetDecimal("feeappl_amt"));
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(2, "deptitemtype_desc", "WFY - ค่าธรรมเนียมรายปี");
                    DwSlip.SetItemDecimal(2, "amt", dt.GetDecimal("feeperyear_amt"));
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(3, "deptitemtype_desc", "WPF - เงินสงเคราะห์ศพล่วงหน้า");
                    DwSlip.SetItemDecimal(3, "amt", dt.GetDecimal("paybffuture_amt"));
                }
            }
            
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "wftype_code", pbl, state.SsCsType);
                DwUtil.RetrieveDDDW(DwMain, "prename_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "province_code", pbl, null);
                try
                {
                    //DwMain.SetItemString(1, "postcode", "");
                    String pvCode = DwUtil.GetString(DwMain, 1, "province_code", "");
                    if (pvCode != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "ampher_code", pbl, pvCode);
                        String dtCode = DwUtil.GetString(DwMain, 1, "ampher_code", "");
                        if (dtCode != "")
                        {
                            if (DwUtil.GetString(DwMain, 1, "postcode", "") == "")
                            {
                                DataWindowChild dc = DwMain.GetChild("ampher_code");
                                int rPostCode = dc.FindRow("DISTRICT_CODE='" + dtCode + "'", 1, dc.RowCount);
                                String postCode = DwUtil.GetString(dc, rPostCode, "postcode", "");
                                DwMain.SetItemString(1, "postcode", postCode);
                            }
                        }
                        else
                        {
                            DwMain.SetItemString(1, "postcode", "");
                        }
                    }
                }
                catch { }

                DwUtil.RetrieveDDDW(DwMain, "other_province_code", pbl, null);
                try
                {
                    //DwMain.SetItemString(1, "postcode", "");
                    String pvCode = DwUtil.GetString(DwMain, 1, "other_province_code", "");
                    if (pvCode != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "other_ampher_code", pbl, pvCode);
                        String dtCode = DwUtil.GetString(DwMain, 1, "other_ampher_code", "");
                        if (dtCode != "")
                        {
                            if (DwUtil.GetString(DwMain, 1, "other_postcode", "") == "")
                            {
                                DataWindowChild dc = DwMain.GetChild("other_ampher_code");
                                int rPostCode = dc.FindRow("DISTRICT_CODE='" + dtCode + "'", 1, dc.RowCount);
                                String postCode = DwUtil.GetString(dc, rPostCode, "postcode", "");
                                DwMain.SetItemString(1, "other_postcode", postCode);
                            }
                        }
                        else
                        {
                            DwMain.SetItemString(1, "other_postcode", "");
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
            if (state.SsUserType == 1 || state.SsUserType == 3)
            {
                try
                {
                    DwMain.Modify("deptopen_tdate.Protect=0");
                }
                catch { }
                try
                {
                    string backColor = DwMain.Describe("postcode.Background.Color");
                    DwMain.Modify("deptopen_tdate.Background.Color=" + backColor);
                }
                catch { }
                try
                {
                    DwSlip.Modify("amt.Protect=0");
                }
                catch { }
            }            
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();
            DwRelate.SaveDataCache();            
        }

        private void JsPostProvince()
        {
            DwMain.SetItemString(1, "ampher_code", "");
        }

        private void Jsother_PostProvince()
        {
            DwMain.SetItemString(1, "other_ampher_code", "");
        }

        private void JsInitWalfareReq(string chk)
        {
            //String XmlDataImp = "";
            String ReqDocno = DwMain.GetItemString(1, "deptrequest_docno"); ;
            
            String card_person, memb_no, sql ;
            
            
            Sdt dt;

            try
            {
                switch (chk)
                {
                    case "card_person":
                        card_person = DwMain.GetItemString(1, "card_person");
                        sql = "Select deptrequest_docno From wcreqdeposit Where card_person ='" + card_person + "' And branch_id = '" + state.SsBranchId + "'";
                        dt = WebUtil.QuerySdt(sql);
                        ReqDocno = "";

                        if (dt.GetRowCount() == 0)
                        {
                            HdchkcardPs.Value = "false";
                        }
                        else if (dt.GetRowCount() > 1)
                        {
                            HdchkcardPs.Value = "true";
                            HdcardPs.Value = card_person;
                        }
                        else if (dt.GetRowCount() == 1)
                        {
                            if (dt.Next()) ReqDocno = dt.GetString("deptrequest_docno");
                            HdchkcardPs.Value = "false";
                        }
                        break;
                    case "member_no":
                        if (state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "02")
                        {
                            memb_no = DwMain.GetItemString(1, "member_no");
                        }
                        else
                        {
                            memb_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no"));
                        }
                        sql = "Select deptrequest_docno From wcreqdeposit Where member_no ='" + memb_no + "' And branch_id ='" + state.SsBranchId + "'";
                        dt = WebUtil.QuerySdt(sql);

                        ReqDocno = "";
                        if (dt.GetRowCount() == 0)
                        {
                            HdchkMembRow.Value = "false";
                        }
                        else if (dt.GetRowCount() > 1)
                        {
                            HdchkMembRow.Value = "true";
                            HdMember_no.Value = memb_no;
                        }
                        else if (dt.GetRowCount() == 1)
                        {
                            if (dt.Next()) ReqDocno = dt.GetString("deptrequest_docno");
                            HdchkMembRow.Value = "false";
                        }
                        break;
                }

                if (ReqDocno != "" && ReqDocno != null)
                {
                    try
                    {
                        DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, ReqDocno, state.SsBranchId);
                        DwUtil.RetrieveDataWindow(DwRelate, pbl, null, ReqDocno, state.SsBranchId);
                        DwUtil.RetrieveDataWindow(DwSlip, pbl, null, ReqDocno, state.SsBranchId);

                        Sdt dtprovince = WebUtil.QuerySdt("select province_code, ampher_code from wcreqdeposit where deptrequest_docno = '" + ReqDocno + "'");
                        if (dtprovince.Next())
                        {
                            DwMain.SetItemString(1, "province_code", dtprovince.GetString("province_code"));
                            DwMain.SetItemString(1, "ampher_code", dtprovince.GetString("ampher_code"));
                        }
                        
                        int approve_status = Convert.ToInt32(DwMain.GetItemDecimal(1, "approve_status"));

                        ///slip ใบสเร็จ
                        try
                        {
                            string sqlslipno = "select deptslip_no from wcdeptslip where deptaccount_no = '" + ReqDocno + "' and branch_id = '" + state.SsBranchId + "' and deptitemtype_code = 'WFF'";
                            Sdt dtslip = WebUtil.QuerySdt(sqlslipno);
                            if (dtslip.Next())
                            {
                                HdSlipNo.Value = dtslip.GetString("deptslip_no");
                                Session["deptslip_no"] = HdSlipNo.Value;
                                HdSaveStatus.Value = "1";
                            }
                            else
                            {
                                ///slip ใบเสร็จ From Master
                                string sqldeptNo = "select deptaccount_no from wcreqdeposit where deptrequest_docno = '" + ReqDocno + "' and branch_id = '" + state.SsBranchId + "'";
                                Sdt dtdeptNo = WebUtil.QuerySdt(sqldeptNo);
                                if (dtdeptNo.Next())
                                {
                                    string deptNo = dtdeptNo.GetString("deptaccount_no");
                                    string sqlslipno_deptno = "select deptslip_no from wcdeptslip where deptaccount_no = '" + deptNo + "' and branch_id = '" + state.SsBranchId + "' and deptitemtype_code = 'WFF'";
                                    Sdt dtslip_deptno = WebUtil.QuerySdt(sqlslipno_deptno);

                                    if (dtslip_deptno.Next())
                                    {
                                        HdSlipNo.Value = dtslip_deptno.GetString("deptslip_no");
                                        Session["deptslip_no"] = HdSlipNo.Value;
                                        HdSaveStatus.Value = "1";
                                        HdFromMaster.Value = "1";
                                    }
                                }                                     
                            }
                        }
                        catch
                        {

                        }

                        ///check สถานะสมาชิก
                        switch (approve_status)
                        {
                            case 1:
                                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้รับการอนุมัติแล้ว ไม่สามารถเปลี่ยนแปลงใบคำขอสมัครได้");
                                break;
                            case -9:
                                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ถูกยกเลิกใบคำขอสมัคร ไม่สามารถเปลี่ยนแปลงใบคำขอสมัครได้");
                                break;
                        }                       

                        ///XmlMain
                        Session["xml_before_audit"] = DwMain.Describe("DataWindow.Data.XML");

                        try
                        {
                            String Rname = DwRelate.GetItemString(1, "name");
                        }
                        catch {
                            try
                            {
                                DwRelate.DeleteRow(1);
                            }
                            catch { }
                        }

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ทำการไม่สำเร็จ" + ex);
                        LoadEnd();
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้กรุณากรอกข้อมูลให้ถูกต้อง");
                    LoadEnd();
                }                
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้กรุณากรอกข้อมูลให้ถูกต้อง");
                LoadEnd();
            }
        }

        public void JsAddRelateRow()
        {
            DwRelate.InsertRow(0);
            HdRow.Value = Convert.ToString(DwRelate.RowCount);
            LtServerMessage.Text = WebUtil.WarningMessage("เมื่อกรอกข้อมูลผู้รับผลประโยชน์เรียบร้อยแล้ว กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");

        }

        public void JsDeleteRelateRow()
        {
            int rowRelate = Convert.ToInt32(HdRow.Value);
            HdSeq_no.Value = DwRelate.GetItemString(rowRelate, "seq_no");
            LtServerMessage.Text = WebUtil.WarningMessage("คุณได้ลบข้อมูล " + DwRelate.GetItemString(rowRelate, "name") + " กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
            DwRelate.DeleteRow(rowRelate);
        }

        public void UpdateReqEdit()
        {
            try
            {
                DwMain.SetItemString(1, "entry_id", state.SsUsername);

                ///Call Audit
                XmlCompare = DwMain.Describe("DataWindow.Data.XML");
                XmlMain = Session["xml_before_audit"].ToString();
                string[] colid = new string[2];
                colid[0] = "000";
                colid[1] = "100";
                int resu = WsUtil.Walfare.SvAuditEdit(state.SsWsPass, state.SsApplication, pbl, XmlMain, XmlCompare, state.SsUsername, state.SsBranchId, state.SsCsType, "d_dp_reqdepoist_main_reqedit", colid); 
                //int resu = auditEdit(XmlMain, XmlCompare);

                if (resu == 1)
                {
                    DwUtil.UpdateDateWindow(DwMain, pbl, "wcreqdeposit");
                    DwUtil.UpdateDateWindow(DwRelate, pbl, "wcreqcodeposit");
                    DwUtil.UpdateDateWindow(DwSlip, pbl, "wcreqdetail");
                    updateSlip();
                    //String xmlRelate = DwRelate.Describe("DataWindow.Data.XML");
                    //WsUtil.Walfare.EditReqWalfareRelate(state.SsWsPass, state.SsApplication, pbl, xmlRelate); 
                    LoadEnd();

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                }
                else
                {
                    LoadEnd();
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void AddRelateRowReqEdit()
        {
            try
            {
                String docregno = DwMain.GetItemString(1, "deptrequest_docno");
                String sql = "Select max(seq_no) as max_seqno From wcreqcodeposit Where deptrequest_docno ='" + docregno + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    int last_seqno = dt.GetInt32("max_seqno");
                    int[] rowrelate = new int[] {DwRelate.RowCount};
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwRelate.SetItemDecimal(rowrelate[0], "seq_no", last_seqno + 1);
                    DwRelate.SetItemString(rowrelate[0], "deptrequest_docno", docregno);
                    DwRelate.SetItemString(rowrelate[0], "branch_id", state.SsBranchId);
                    DwUtil.InsertDataWindow(DwRelate, pbl, "wcreqcodeposit", rowrelate);


                    ///Call Audit
                    XmlCompare = DwMain.Describe("DataWindow.Data.XML");
                    XmlMain = Session["xml_before_audit"].ToString();
                    string[] colid = new string[2];
                    colid[0] = "000";
                    colid[1] = "100";
                    int resu = WsUtil.Walfare.SvAuditEdit(state.SsWsPass, state.SsApplication, pbl, XmlMain, XmlCompare, state.SsUsername, state.SsBranchId, state.SsCsType, "d_dp_reqdepoist_main_reqedit", colid); 
                    //int resu = auditEdit(XmlMain, XmlCompare);
                    if (resu == 1)
                    {
                        DwUtil.UpdateDateWindow(DwMain, pbl, "wcreqdeposit");
                        DwUtil.UpdateDateWindow(DwRelate, pbl, "wcreqcodeposit");
                        DwUtil.UpdateDateWindow(DwSlip, pbl, "wcreqdetail");
                        updateSlip();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else
                    {
                        LoadEnd();
                        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                    }
                }                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            HdActionStatus.Value = "";
        }

        public void DeleteRelateRowReqEdit()
        {
            try
            {
                String docregno = DwMain.GetItemString(1, "deptrequest_docno");
                int result = WsUtil.Walfare.DeleteWalRelateRowReqEdit(state.SsWsPass, state.SsApplication, docregno, HdSeq_no.Value);
                if (result == 1)
                {
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);

                    ///Call Audit
                    XmlCompare = DwMain.Describe("DataWindow.Data.XML");
                    XmlMain = Session["xml_before_audit"].ToString();
                    string[] colid = new string[2];
                    colid[0] = "000";
                    colid[1] = "100";
                    int resu = WsUtil.Walfare.SvAuditEdit(state.SsWsPass, state.SsApplication, pbl, XmlMain, XmlCompare, state.SsUsername, state.SsBranchId, state.SsCsType, "d_dp_reqdepoist_main_reqedit", colid); 
                    //int resu = auditEdit(XmlMain, XmlCompare);

                    if (resu == 1)
                    {
                        DwUtil.UpdateDateWindow(DwMain, pbl, "wcreqdeposit");
                        DwUtil.UpdateDateWindow(DwRelate, pbl, "wcreqcodeposit");
                        DwUtil.UpdateDateWindow(DwSlip, pbl, "wcreqdetail");
                        updateSlip();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else
                    {
                        LoadEnd();
                        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                    }
                }
                else
                {
                    LoadEnd();
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                }                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            HdActionStatus.Value = "";
        }

        public void LoadEnd()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);

            DwSlip.Reset();
            String wftypeCode = DwUtil.GetString(DwMain, 1, "wftype_code", "");

            if (wftypeCode != "")
            {
                String sql = "select * from wcmembertype where wftype_code='" + wftypeCode + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(1, "deptitemtype_desc", "FEE - ค่าธรรมเนียมสมัครใหม่");
                    DwSlip.SetItemDecimal(1, "amt", dt.GetDecimal("feeappl_amt"));
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(2, "deptitemtype_desc", "WFY - ค่าธรรมเนียมรายปี");
                    DwSlip.SetItemDecimal(2, "amt", dt.GetDecimal("feeperyear_amt"));
                    DwSlip.InsertRow(0);
                    DwSlip.SetItemString(3, "deptitemtype_desc", "WPF - เงินสงเคราะห์ศพล่วงหน้า");
                    DwSlip.SetItemDecimal(3, "amt", dt.GetDecimal("paybffuture_amt"));
                }
            }

            DwRelate.Reset();
        }

        public int auditEdit(String Mxml, String Cxml)
        {
            string Mvalue;
            string Cvalue;
                     
            String pblFull = WebUtil.PhysicalPath + "Saving\\DataWindow\\walfare\\" + pbl;

            ///XmlMain
            DataStore DtMain = new DataStore(pblFull, "d_dp_reqdepoist_main_reqedit");
            DtMain.ImportString(Mxml, FileSaveAsType.Xml);

            String[] columnName = new string[DtMain.ColumnCount];
            String[] columnType = new string[DtMain.ColumnCount];            

            ///XmlCompare
            DataStore DtCompare = new DataStore(pblFull, "d_dp_reqdepoist_main_reqedit");
            DtCompare.ImportString(Cxml, FileSaveAsType.Xml);  

            int Colcount = DtCompare.ColumnCount;
            for (int i = 0; i < Colcount; i++)
            {
                ///XmlMain
                columnName[i] = DtMain.Describe("#" + (i + 1) + ".Name");
                columnType[i] = DtMain.Describe(columnName[i] + ".ColType").ToLower();

                if (columnType[i].IndexOf("(") > 0)
                {
                    columnType[i] = columnType[i].Substring(0, columnType[i].IndexOf("("));
                }

                Mvalue = getDataFromDW(DtMain, columnName[i], columnType[i], 1);
                Mvalue = Mvalue.Trim();

                Cvalue = getDataFromDW(DtCompare, columnName[i], columnType[i], 1);
                Cvalue = Cvalue.Trim();

                if (Mvalue != Cvalue && columnName[i] != "entry_id")
                {
                    Sta ta = new Sta(state.SsConnectionString);
                    ta.Transection();
                    try
                    {
                        string member_no = DtMain.GetItemString(1, "member_no");
                        string wfaccount_name = DtMain.GetItemString(1, "wfaccount_name");
                        string card_person = DtMain.GetItemString(1, "card_person");

                        int result = saveAudit(ta, columnName[i], member_no, wfaccount_name, card_person, Mvalue, Cvalue);
                        if (result != 1)
                        {
                            Session["xml_before_audit"] = null;
                            TauditStatus = 0;

                            return 0;
                        }
                        ta.Commit();
                        ta.Close();
                    }
                    catch (Exception ex)
                    {
                        Session["xml_before_audit"] = null;
                        TauditStatus = 0;

                        ta.RollBack();
                        ta.Close();
                        throw ex;
                    }
                }
            }
            Session["xml_before_audit"] = null;
            TauditStatus = 0;

            return 1;
        }

        public String getDataFromDW(IDataStore Dts, string colName, string colType, int row)
        {
            String result = "null";

            if (colType == "char")
            {
                try
                {
                    result = Dts.GetItemString(row, colName);
                }
                catch { }
            }
            else if (colType == "decimal" || colType == "long" || colType == "number" || colType == "float" || colType == "double")
            {
                try
                {
                    result = Dts.GetItemDecimal(row, colName).ToString();
                }
                catch { }
            }
            else if (colType == "datetime")
            {
                //dataWindow.SetItemDateTime(r + 1, cName[c], DateTime.ParseExact(dt.Rows[r][c].ToString(), "yyyy-MM-dd HH:mm:ss", WebUtil.EN));
                try
                {
                    CultureInfo en = new CultureInfo("en-US");
                    result = "to_date('" + Dts.GetItemDateTime(row, colName).ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";
                }
                catch { }
            }
            return result;
        }

        public int saveAudit(Sta ta, string ColName, string member_no, string name, string card_person, string Oldvalue, string Newvalue)
        {
            String today = "to_date('" + DateTime.Now.ToString("yyyy-MM-d H:m:s", WebUtil.EN) + "', 'yyyy-mm-dd hh24:mi:ss')";

            if (TauditStatus == 0)
            {
                try
                {
                    DocumentControl dct = new DocumentControl();
                    auditDocno = dct.NewDocumentNo("AUDITMEMBER", 2554, ta);

                    String Msql = "INSERT INTO mbaudit (docno, member_no, approve_id, approve_date, \"NAME\", card_person, branch_id, cs_type) ";
                    Msql = Msql + "VALUES('" + auditDocno + "','" + member_no + "','" + state.SsUsername + "'," + today + ",'" + name + "','" + card_person + "','" + state.SsBranchId + "','" + state.SsCsType + "')";

                    ta.Exe(Msql);
                    TauditStatus = 1;
                }
                catch
                {
                    return 0;
                }
                
            }
            try
            {
                String sql = "Select col_id From cmauditcolumn Where engcol_name ='" + ColName.ToUpper() + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    String col_id = dt.GetString("col_id");
                    String Hsql = "INSERT INTO mbaudithistory (docno, col_id, old_value, new_value) VALUES('";
                    Hsql = Hsql + auditDocno + "','" + col_id + "','" + Oldvalue + "','" + Newvalue + "')";

                    ta.Exe(Hsql);
                }
                else
                {
                    return 0;
                }                              
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        private void updateSlip()
        {
            Sta ta = new Sta(state.SsConnectionString);
            ta.Transection();

            decimal prncslip_amt;
            string deptitemtype_code, sql;
            string deptslip_no = Session["deptslip_no"].ToString();
            Session["deptslip_no"] = null;
            for (int i = 0; i < DwSlip.RowCount; i++)
            {
                prncslip_amt = DwSlip.GetItemDecimal(i + 1, "amt");
                deptitemtype_code = DwSlip.GetItemString(i + 1, "deptitemtype_code");
                sql = "update wcdeptslipdet set prncslip_amt = " + prncslip_amt + " where deptitemtype_code = '" + deptitemtype_code + "' and deptslip_no = '" + deptslip_no + "'";
                try
                {
                    ta.Exe(sql);
                }
                catch { }
            }
            try
            {                
                ta.Commit();
                ta.Close();
            }
            catch
            {
                ta.RollBack();
                ta.Close();
            }

        }

        private void ChangeMemtype()
        {
            try
            {
                string wftype_code = DwMain.GetItemString(1, "wftype_code");

                String SQLcontant = "select * from wcmembertype where cs_type = '" + state.SsCsType + "' and wftype_code = '" + wftype_code + "'";
                Sdt dtContant = WebUtil.QuerySdt(SQLcontant);
                if (dtContant.Next())
                {
                    decimal Feeappl = dtContant.GetDecimal("feeappl_amt");
                    decimal Feeperyear = dtContant.GetDecimal("feeperyear_amt");
                    decimal Paybffuture = dtContant.GetDecimal("paybffuture_amt");
                    DateTime OpenDate = dtContant.GetDate("deptopen_date");

                    DwMain.SetItemDateTime(1, "deptopen_date", OpenDate);

                    DwSlip.SetItemDecimal(1, "amt", Feeappl);
                    DwSlip.SetItemDecimal(2, "amt", Feeperyear);
                    DwSlip.SetItemDecimal(3, "amt", Paybffuture);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถดึงค่าคงที่ของระบบได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
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
            int sumValue = 0;

            for (int i = 0; i < PID.Length - 1; i++)
            {
                sumValue += int.Parse(PID[i].ToString()) * (13 - i);
            }
            int v = (11 - (sumValue % 11)) % 10;
            String chk = PID[12].ToString();

            return PID[12].ToString() == v.ToString();
        }
    }    
}