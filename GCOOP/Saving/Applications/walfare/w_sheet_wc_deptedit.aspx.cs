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

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_deptedit : PageWebSheet, WebSheet
    {
        protected String jsinitAccNo;
        protected String postPost;
        protected String postProvince;
        protected String other_postPost;
        protected String other_postProvince;
        protected String jsAddRelateRow;
        protected String jsDeleteRelateRow;
        private String pbl = "w_sheet_wc_membermaster.pbl";

        private DwThDate tDwMain;

        public void InitJsPostBack()
        {
            jsinitAccNo = WebUtil.JsPostBack(this, "jsinitAccNo");
            postPost = WebUtil.JsPostBack(this, "postPost");
            postProvince = WebUtil.JsPostBack(this, "postProvince");
            other_postPost = WebUtil.JsPostBack(this, "other_postPost");
            other_postProvince = WebUtil.JsPostBack(this, "other_postProvince");
            jsAddRelateRow = WebUtil.JsPostBack(this, "jsAddRelateRow");
            jsDeleteRelateRow = WebUtil.JsPostBack(this, "jsDeleteRelateRow");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("apply_date", "apply_tdate");
            tDwMain.Add("wfbirthday_date", "wfbirthday_tdate");
            tDwMain.Add("deptopen_date", "deptopen_tdate");
            tDwMain.Add("deptclose_date", "deptclose_tdate");
            tDwMain.Add("die_date", "die_tdate");
            tDwMain.Add("effective_date", "effective_tdate");            
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "membgroup_code", state.SsBranchId);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                DwMain.SetItemString(1, "province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "postcode", state.SsPostCode);
                DwMain.SetItemString(1, "other_ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "other_province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "other_postcode", state.SsPostCode);

                tDwMain.Eng2ThaiAllRow();

                String wftypeCode = DwUtil.GetString(DwMain, 1, "wftype_code", "");
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwStment);
                this.RestoreContextDw(DwCodept);
                this.RestoreContextDw(Dwtrn);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsinitAccNo":
                    JsinitAccNo();
                    break;
                case "postProvince":
                    JsPostProvince();
                    break;
                case "other_postProvince":
                    Jsother_PostProvince();
                    break;
                case "jsAddRelateRow":
                    JsAddRelateRow();
                    break;
                case "jsDeleteRelateRow":
                    JsDeleteRelateRow();
                    break;
                case "postPost":
                    try
                    {
                        DwMain.SetItemString(1, "postcode", "");
                    }
                    catch { }
                    break;
                case "other_postPost":
                    try
                    {
                        DwMain.SetItemString(1, "other_postcode", "");
                    }
                    catch { }
                    break;
            }
        }

        public void SaveWebSheet()
        {
            if (state.SsUserType == 2 && state.SsCsType == "8")
            {
                 LtServerMessage.Text = WebUtil.ErrorMessage("คุณไม่มีสิทธิในการแก้ไขข้อมูล กรุณาติดต่อ สมาคม");
                 return;
            }
            try
            {
                string tdeptopen_date = DwMain.GetItemString(1, "apply_tdate");
                if (tdeptopen_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(tdeptopen_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "apply_date", dt);
                }
            }
            catch { }
            try
            {
                string tbirth_date = DwMain.GetItemString(1, "wfbirthday_tdate");
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
                string teffective_date = DwMain.GetItemString(1, "effective_tdate");
                if (teffective_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(teffective_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "effective_date", dt);
                }
            }
            catch { }

            ///chec สถานะสมาชิก
            try
            {
                int wfmember_status = Convert.ToInt32(DwMain.GetItemDecimal(1, "wfmember_status"));

                if (wfmember_status == -1 && state.SsUserType != 1)
                {
                    try
                    {
                        string resigncause_code = DwMain.GetItemString(1, "resigncause_code");
                        switch (resigncause_code)
                        {
                            case "01":
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้ได้ทำการแจ้งลาออกแล้ว");
                                break;
                            case "02":
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้ได้ทำการแจ้งเสียชีวิตแล้ว");
                                break;
                            case "03":
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้ถูกยกเลิกไปแล้ว");
                                break;
                            case "04":
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ สมาชิกคนนี้สิ้นสุดสมาชิกภาพไปแล้ว");
                                break;
                        }
                    }
                    catch
                    { }
                }
                else
                {
                    string prename_code = DwMain.GetItemString(1, "prename_code");
                    if (prename_code == "00")
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุคำนำหน้า");
                        return;
                    }
                    string prename_desc = "";
                    String sqlpre = "select prename_desc from mbucfprename where prename_code='" + prename_code + "'";

                    ///format member_no
                    //string member_no = DwMain.GetItemString(1, "member_no").Trim();
                    //member_no = int.Parse(member_no).ToString("000000");
                    //DwMain.SetItemString(1, "member_no", member_no);

                    try{
                        string membNO;
                        if ((state.SsCsType == "6" && DwMain.GetItemString(1, "wftype_code") == "02") || (state.SsCsType == "1" && (DwMain.GetItemString(1, "wftype_code") == "12" || DwMain.GetItemString(1, "wftype_code") == "03" || DwMain.GetItemString(1, "wftype_code") == "08")) || (state.SsCsType == "8" && (DwMain.GetItemString(1, "wftype_code") == "03" || DwMain.GetItemString(1, "wftype_code") == "05" || DwMain.GetItemString(1, "wftype_code") == "06")))
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
                            membNO = int.Parse(DwMain.GetItemString(1, "member_no")).ToString("0000000"); ;
                        }
                        else
                        {
                            membNO = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no").Trim());
                        }
                        DwMain.SetItemString(1, "member_no", membNO);
                    }
                    catch { }

                    ///check Data
                    CheckData();

                    Sdt dtable = WebUtil.QuerySdt(sqlpre);
                    if (dtable.Next())
                    {
                        prename_desc = dtable.GetString("prename_desc");
                    }
                    string fullname = prename_desc + "" + DwMain.GetItemString(1, "deptaccount_name") + "  " + DwMain.GetItemString(1, "deptaccount_sname");
                    DwMain.SetItemString(1, "wfaccount_name", fullname);

                    String DWseq_no = "", Codept_CPS;
                    int rowCount = DwCodept.RowCount;
                    decimal foreigner_flag;
                    for (int i = 0; i < rowCount; i++)
                    {
                        try
                        {
                            DwCodept.GetItemString(i + 1, "name");
                            DWseq_no = DWseq_no + "'" + DwCodept.GetItemDecimal(i + 1, "seq_no") + "' ";
                            if (i != (rowCount - 1))
                            {
                                DWseq_no = DWseq_no + ",";
                            }
                            try
                            {
                                Codept_CPS = DwCodept.GetItemString(i + 1, "codept_id");
                            }
                            catch
                            {
                                Codept_CPS = "";
                            }
                            if ((state.SsCsType != "1") && (state.SsCsType != "6") && (state.SsCsType != "8") && (state.SsCsType != "2"))
                            {
                                foreigner_flag = DwCodept.GetItemDecimal(i + 1, "foreigner_flag");
                                if (foreigner_flag == 0)
                                {
                                    bool Chk_CodeptCPS = VerifyPeopleID(Codept_CPS);
                                    if (!Chk_CodeptCPS)
                                    {
                                        LtServerMessage.Text = WebUtil.ErrorMessage("บัตรประชาชนผู้รับผลประโยชน์คนที่ " + (i + 1) + " ไม่ถูกต้อง");
                                        return;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            for (int j = DwCodept.RowCount; j >= (i + 1); j--)
                            {
                                DwCodept.DeleteRow(j);
                            }
                            break;
                        }
                    }

                    rowCount = DwCodept.RowCount;
                    if (DWseq_no == "") DWseq_no = "0";
                    DWseq_no = DWseq_no.Remove(DWseq_no.Length - 1);
                    String SqlDel = "delete from wccodeposit where  deptaccount_no = '" + Session["deptaccount_no"].ToString() + "'";
                    WebUtil.QuerySdt(SqlDel);

                    String deptaccount_no = Session["deptaccount_no"].ToString();
                    for (int k = 1; k <= DwCodept.RowCount; k++)
                    {
                        DwCodept.SetItemDecimal(k, "seq_no", k);
                        DwCodept.SetItemString(k, "deptaccount_no", deptaccount_no);
                        DwCodept.SetItemString(k, "branch_id", state.SsBranchId);
                    }                    
                    try
                    {
                        int[] rows = new int[DwCodept.RowCount];
                        int ii = 0;
                        for (int i = 1; i <= DwCodept.RowCount; i++)
                        {
                            rows[ii] = i;
                            ii++;
                        }
                        DwUtil.InsertDataWindow(DwCodept, pbl, "wccodeposit", rows);
                        Hdrowcount.Value = Convert.ToString(DwCodept.RowCount);
                        int Def_RowCodet = 10 - DwCodept.RowCount;
                        for (int i = 1; i <= Def_RowCodet; i++)
                        {
                            DwCodept.InsertRow(0);
                            DwCodept.SetItemDecimal(i, "seq_no", i + Def_RowCodet);
                        }

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        return;
                    }
                    try
                    {
                        DwMain.SetItemString(1, "deptaccount_no", Session["deptaccount_no"].ToString());
                        String XmlCompare = DwMain.Describe("DataWindow.Data.XML");
                        String XmlMain = Session["xml_before_audit"].ToString();
                        string[] colid = new string[2];
                        colid[0] = "101";
                        colid[1] = "200";
                        int resu = WsUtil.Walfare.SvAuditEdit(state.SsWsPass, state.SsApplication, pbl, XmlMain, XmlCompare, state.SsUsername, state.SsBranchId, state.SsCsType, "d_dp_dept_edit_master", colid);
                        Session["xml_before_audit"] = null;

                        if (resu == 1)
                        {
                            DwUtil.UpdateDateWindow(DwMain, pbl, "wcdeptmaster");
                            DwUtil.UpdateDateWindow(DwCodept, pbl, "wccodeposit");
                            Session["deptaccount_no"] = null;
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                            DwMain.Reset();
                            DwMain.InsertRow(0);
                            DwCodept.Reset();
                            DwStment.Reset();
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้" + ex);
                    }
                }
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
            if(state.SsUserType == 1 || state.SsUserType == 3)
            {
                try
                {
                    DwMain.Modify("wftype_code.Protect=0");
                    DwMain.Modify("deptaccount_name.Protect=0");
                    DwMain.Modify("deptaccount_sname.Protect=0");
                    DwMain.Modify("apply_tdate.Protect=0");
                    DwMain.Modify("sex.Protect=0");
                    DwMain.Modify("wfbirthday_tdate.Protect=0");
                    DwMain.Modify("member_no.Protect=0");
                    DwMain.Modify("card_person.Protect=0");
                    DwMain.Modify("deptopen_tdate.Protect=0");
                    DwMain.Modify("effective_tdate.Protect=0");
                    DwMain.Modify("foreigner_flag.Protect=0");
                    DwMain.Modify("prename_code.Protect=0");
                    DwMain.Modify("deptclose_tdate.Protect=0");
                    DwMain.Modify("die_tdate.Protect=0");
                    DwMain.Modify("mate_name.Protect=0");
                    DwMain.Modify("manage_corpse_name.Protect=0");

                    DwMain.Modify("other_contact_address.Protect=0");
                    DwMain.Modify("other_ampher_code.Protect=0");
                    DwMain.Modify("other_province_code.Protect=0");
                    DwMain.Modify("other_postcode.Protect=0");
                    DwMain.Modify("contact_address.Protect=0");
                    DwMain.Modify("ampher_code.Protect=0");
                    DwMain.Modify("province_code.Protect=0");
                    DwMain.Modify("postcode.Protect=0");
                    DwMain.Modify("withdrawable_amt.Protect=0");
                    DwMain.Modify("phone.Protect=0");
                    DwMain.Modify("remark.Protect=0");

                    DwCodept.Modify("name.Protect=0");
                    DwCodept.Modify("codept_id.Protect=0");
                    DwCodept.Modify("codept_addre.Protect=0");
                    DwCodept.Modify("foreigner_flag.Protect=0");
                }
                catch { }
                try
                {
                    string backColor = DwMain.Describe("deptaccount_no.Background.Color");
                    DwMain.Modify("wftype_code.Background.Color=" + backColor);
                    DwMain.Modify("deptaccount_name.Background.Color=" + backColor);
                    DwMain.Modify("deptaccount_sname.Background.Color=" + backColor);
                    DwMain.Modify("apply_tdate.Background.Color=" + backColor);
                    DwMain.Modify("sex.Background.Color=" + backColor);
                    DwMain.Modify("wfbirthday_tdate.Background.Color=" + backColor);
                    DwMain.Modify("member_no.Background.Color=" + backColor);
                    DwMain.Modify("card_person.Background.Color=" + backColor);
                    DwMain.Modify("deptopen_tdate.Background.Color=" + backColor);
                    DwMain.Modify("effective_tdate.Background.Color=" + backColor);

                    DwMain.Modify("prename_code.Background.Color=" + backColor);
                    DwMain.Modify("deptclose_tdate.Background.Color=" + backColor);
                    DwMain.Modify("die_tdate.Background.Color=" + backColor);
                    DwMain.Modify("mate_name.Background.Color=" + backColor);
                    DwMain.Modify("manage_corpse_name.Background.Color=" + backColor);

                    DwMain.Modify("other_contact_address.Background.Color=" + backColor);
                    DwMain.Modify("other_ampher_code.Background.Color=" + backColor);
                    DwMain.Modify("other_province_code.Background.Color=" + backColor);
                    DwMain.Modify("other_postcode.Background.Color=" + backColor);
                    DwMain.Modify("contact_address.Background.Color=" + backColor);
                    DwMain.Modify("ampher_code.Background.Color=" + backColor);
                    DwMain.Modify("province_code.Background.Color=" + backColor);
                    DwMain.Modify("postcode.Background.Color=" + backColor);
                    DwMain.Modify("withdrawable_amt.Background.Color=" + backColor);
                    DwMain.Modify("phone.Background.Color=" + backColor);
                    DwMain.Modify("remark.Background.Color=" + backColor);

                    DwCodept.Modify("name.Background.Color=" + backColor);
                    DwCodept.Modify("codept_id.Background.Color=" + backColor);
                    DwCodept.Modify("codept_addre.Background.Color=" + backColor);
                    DwCodept.Modify("foreigner_flag.Background.Color=" + backColor);
                    DwCodept.Modify("b_delete.visible=1");
                }

                catch { }
            }
            else if (state.SsCsType == "2" && state.SsUserType == 2)
            {
                try
                {
                    DwMain.Modify("prename_code.Protect=0");
                    DwMain.Modify("other_contact_address.Protect=0");
                    DwMain.Modify("other_ampher_code.Protect=0");
                    DwMain.Modify("other_province_code.Protect=0");
                    DwMain.Modify("other_postcode.Protect=0");
                    DwMain.Modify("contact_address.Protect=0");
                    DwMain.Modify("ampher_code.Protect=0");
                    DwMain.Modify("province_code.Protect=0");
                    DwMain.Modify("postcode.Protect=0");
                    // DwMain.Modify("withdrawable_amt.Protect=0");
                    DwMain.Modify("phone.Protect=0");
                    DwMain.Modify("remark.Protect=0");
                }
                catch { }
                try
                {

                    string backColor = DwMain.Describe("deptaccount_no.Background.Color");
                    DwMain.Modify("prename_code.Background.Color=" + backColor);
                    DwMain.Modify("other_contact_address.Background.Color=" + backColor);
                    DwMain.Modify("other_ampher_code.Background.Color=" + backColor);
                    DwMain.Modify("other_province_code.Background.Color=" + backColor);
                    DwMain.Modify("other_postcode.Background.Color=" + backColor);
                    DwMain.Modify("contact_address.Background.Color=" + backColor);
                    DwMain.Modify("ampher_code.Background.Color=" + backColor);
                    DwMain.Modify("province_code.Background.Color=" + backColor);
                    DwMain.Modify("postcode.Background.Color=" + backColor);
                    //  DwMain.Modify("withdrawable_amt.Background.Color=" + backColor);
                    DwMain.Modify("phone.Background.Color=" + backColor);
                    DwMain.Modify("remark.Background.Color=" + backColor);

                }
                catch
                {
                }
            }
            else {
                try
                {
                }
                catch { }
            }

            //if (state.SsCsType == "1" && state.SsUserType != 1)
            //{
            //    DwMain.Modify("effective_tdate.Protect=1");
            //    string backColor = DwMain.Describe("resigncause_code.Background.Color");
            //    DwMain.Modify("effective_tdate.Background.Color=" + backColor);
            //}

            if (state.SsCsType == "1")
            {
                DwMain.Modify("effective_tdate.Protect=1");
                string backColor = DwMain.Describe("resigncause_code.Background.Color");
                DwMain.Modify("effective_tdate.Background.Color=" + backColor);
            }

            DwMain.SaveDataCache();
            DwStment.SaveDataCache();
            DwCodept.SaveDataCache();
            Dwtrn.SaveDataCache();
            //string member_status;
            //try{
            //    member_status = DwMain.GetItemString(1, "resigncause_code");
            //}catch{
            //    member_status = "00";
            //}
            //    if(member_status == "01" || member_status == "02"){
            //        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ทำรายการ ลาออก/เสียชีวิต ไปแล้วไม่สามาชิกแก้ไขข้อมูลได้");
            //    }
        }

        public void JsinitAccNo()
        {

            try
            {
                String AccNo = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
                if (AccNo != "" && AccNo != null)
                {
                    try
                    {
                        DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, AccNo, state.SsBranchId, state.SsCsType);
                        DwUtil.RetrieveDataWindow(DwStment, pbl, null, AccNo, state.SsBranchId);
                        DwUtil.RetrieveDataWindow(DwCodept, pbl, null, AccNo, state.SsBranchId);
                        DwUtil.RetrieveDataWindow(Dwtrn, pbl, null, AccNo, state.SsCsType);

                        try
                        {
                            DateTime ldtm_birth, ldtm_now;
                            int li_age;
                            ldtm_birth = DwMain.GetItemDateTime(1, "wfbirthday_date");
                            ldtm_now = DateTime.UtcNow;
                            li_age = ldtm_now.Year - ldtm_birth.Year;
                            DwMain.SetItemDecimal(1, "totalage", li_age);
                        }
                        catch { }

                        ///check สถานะสมาชิก
                        int wfmember_status = Convert.ToInt32(DwMain.GetItemDecimal(1, "wfmember_status"));

                        if (wfmember_status == -1)
                        {
                            try
                            {
                                string resigncause_code = DwMain.GetItemString(1, "resigncause_code");
                                switch (resigncause_code)
                                {
                                    case "01":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ทำการแจ้งลาออกแล้ว ไม่สามารถแก้ไขข้อมูลได้");
                                        break;
                                    case "02":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ทำการแจ้งเสียชีวิตแล้ว ไม่สามารถแก้ไขข้อมูลได้");
                                        break;
                                    case "03":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ถูกยกเลิกไปแล้ว ไม่สามารถแก้ไขข้อมูลได้");
                                        break;
                                    case "04":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้สิ้นสุดสมาชิกภาพไปแล้ว ไม่สามารถแก้ไขข้อมูลได้");
                                        break;
                                }
                            }
                            catch
                            { }
                        }
                        else
                        {
                            int RowCodet = DwCodept.RowCount;
                            int Def_RowCodet = 10 - RowCodet;
                            for (int i = 1; i <= Def_RowCodet; i++)
                            {
                                DwCodept.InsertRow(0);
                                DwCodept.SetItemDecimal(RowCodet + i, "seq_no", i + Def_RowCodet);
                            }
                            String Xml_codept = DwCodept.Describe("DataWindow.Data.XML");
                        }

                        Session["xml_before_audit"] = DwMain.Describe("DataWindow.Data.XML");
                        Session["deptaccount_no"] = DwMain.GetItemString(1, "deptaccount_no");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้");
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        WebSheetLoadEnd();
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้");
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    WebSheetLoadEnd();
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล");
            }
        }

        private void JsPostProvince()
        {
            DwMain.SetItemString(1, "ampher_code", "");
        }
        private void Jsother_PostProvince()
        {
            DwMain.SetItemString(1, "other_ampher_code", "");
        }

        public void JsAddRelateRow()
        {
            DwCodept.InsertRow(0);
            //HdRow.Value = Convert.ToString(DwCodept.RowCount);
            //LtServerMessage.Text = WebUtil.WarningMessage("เมื่อกรอกข้อมูลผู้รับผลประโยชน์เรียบร้อยแล้ว กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");

        }

        public void JsDeleteRelateRow()
        {
            int rowRelate = Convert.ToInt32(HdRow.Value);
            //HdSeq_no.Value = DwCodept.GetItemString(rowRelate, "seq_no");
            //LtServerMessage.Text = WebUtil.WarningMessage("คุณได้ลบข้อมูล " + DwCodept.GetItemString(rowRelate, "name") + " กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
            DwCodept.DeleteRow(rowRelate);
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

        private void CheckData()
        {
            try
            {
                string member_no = DwMain.GetItemString(1, "member_no");
                if (string.IsNullOrEmpty(member_no)) throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            catch
            {
                throw new Exception("กรุณากรอกเลขสมาชิกสหกรณ์");
            }
            try
            {
                string deptaccount_name = DwMain.GetItemString(1, "deptaccount_name");
                if (string.IsNullOrEmpty(deptaccount_name)) throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกชื่อผู้สมัคร");
            }
            try
            {
                string deptaccount_sname = DwMain.GetItemString(1, "deptaccount_sname");
                if (string.IsNullOrEmpty(deptaccount_sname)) throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            catch
            {
                throw new Exception("กรุณากรอกนามสกุลผู้สมัคร");
            }
            try
            {
                DateTime wfbirthday_date = DwMain.GetItemDateTime(1, "wfbirthday_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันเกิดให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }
            try
            {
                string card_person = DwMain.GetItemString(1, "card_person");
                if (DwMain.GetItemDecimal(1, "foreigner_flag") == 0)
                {
                    if (VerifyPeopleID(card_person) == false)
                    {
                        throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
                    }
                }
            }
            catch
            {
                throw new Exception("กรุณากรอกบัตรประจำตัวประชาชนให้ถูกต้อง");
            }
            try
            {
                DateTime apply_date = DwMain.GetItemDateTime(1, "apply_date");
            }
            catch
            {
                throw new Exception("กรุณากรอกวันสมัครให้ถูกต้อง ตามรูปแบบ dd/mm/yyyy");
            }

        }
    }
}