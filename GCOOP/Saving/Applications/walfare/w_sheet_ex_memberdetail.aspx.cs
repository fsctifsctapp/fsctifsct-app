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
using SecurityEngine;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_ex_memberdetail : PageWebSheet, WebSheet
    {
        protected String app = "";
        protected String gid = "";
        protected String rid = "";
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
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
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
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
                String ascstypee = state.SsCsType;
                jjjj.Value = ascstypee;
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "membgroup_code", state.SsBranchId);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                DwMain.SetItemString(1, "province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "postcode", state.SsPostCode);
                DwMain.SetItemString(1, "other_ampher_code", state.SsDistrictCode);
                DwMain.SetItemString(1, "other_province_code", state.SsProvinceCode);
                DwMain.SetItemString(1, "other_postcode", state.SsPostCode);
                // DwMain.SetItemString(1, "card_person_1", "");
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
                case "runProcess":
                    RunProcess();
                    break;
                case "popupReport":
                    PopupReport();
                    break;
                //case "jsAddRelateRow":
                //   JsAddRelateRow();
                //     break;
                //case "jsDeleteRelateRow":
                //  JsDeleteRelateRow();
                //break;
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

        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwMain.SaveDataCache();
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
            if (state.SsUserType == 1 || state.SsUserType == 3)
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

                    DwCodept.Modify("name.Protect=0");
                    DwCodept.Modify("codept_id.Protect=0");
                    DwCodept.Modify("codept_addre.Protect=0");
                    DwCodept.Modify("foreigner_flag.Protect=0");
                }
                catch { }
                try
                {
                    string backColor = DwMain.Describe("postcode.Background.Color");
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

                    DwCodept.Modify("name.Background.Color=" + backColor);
                    DwCodept.Modify("codept_id.Background.Color=" + backColor);
                    DwCodept.Modify("codept_addre.Background.Color=" + backColor);
                    DwCodept.Modify("foreigner_flag.Background.Color=" + backColor);
                    DwCodept.Modify("b_delete.visible=1");
                }

                catch { }
            }
            else
            {
                try
                {

                }
                catch
                {
                }
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
                state.SsWsPass = new Encryption().EncryptStrBase64("1234+" + Session["ss_connectionstring"].ToString());
                String AccNo = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no_1"));
                //String AccNo = DwMain.GetItemString(1, "search_all");
                String cstypee = state.SsCsType;
                if (AccNo != "" && AccNo != null)
                {
                    try
                    {
                        DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, AccNo, cstypee);
                        DwUtil.RetrieveDataWindow(DwStment, pbl, null, AccNo);
                        DwUtil.RetrieveDataWindow(DwCodept, pbl, null, AccNo);
                        DwUtil.RetrieveDataWindow(Dwtrn, pbl, null, AccNo, cstypee);

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
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ทำการแจ้งลาออกแล้ว");
                                        break;
                                    case "02":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ได้ทำการแจ้งเสียชีวิตแล้ว");
                                        break;
                                    case "03":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้ถูกยกเลิกไปแล้ว");
                                        break;
                                    case "04":
                                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกคนนี้สิ้นสุดสมาชิกภาพไปแล้ว");
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
                        Session["deptaccount_no_1"] = DwMain.GetItemString(1, "deptaccount_no_1");
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

        /*public void JsAddRelateRow()
        {
            DwCodept.InsertRow(0);
            //HdRow.Value = Convert.ToString(DwCodept.RowCount);
            //LtServerMessage.Text = WebUtil.WarningMessage("เมื่อกรอกข้อมูลผู้รับผลประโยชน์เรียบร้อยแล้ว กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");

        }*/

        /*public void JsDeleteRelateRow()
        {
            int rowRelate = Convert.ToInt32(HdRow.Value);
            //HdSeq_no.Value = DwCodept.GetItemString(rowRelate, "seq_no");
            //LtServerMessage.Text = WebUtil.WarningMessage("คุณได้ลบข้อมูล " + DwCodept.GetItemString(rowRelate, "name") + " กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
            DwCodept.DeleteRow(rowRelate);
        }*/

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


        /*  private void CheckData()
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

          }*/
        #region Report Process
        private void RunProcess()
        {
            state.SsWsPass = new Encryption().EncryptStrBase64("1234+" + Session["ss_connectionstring"].ToString());
            app = "walfare";
            gid = "walfare_daily";
            rid = "walfare_daily22_5";
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            //String start_docno = dw_criteria.GetItemString(1, "start_docno");
            String deptacc = DwMain.GetItemString(1, "deptaccount_no");
            String branch_id = DwMain.GetItemString(1, "branch_id");
            String ascstype = state.SsCsType;
            // jjjj.Value = "54889589589";
            // String branch_id = dw_criteria.GetItemString(1, "as_branchid");
            // String membno = dw_criteria.GetItemString(1, "as_deptaccno");
            //  membno = int.Parse(membno).ToString("000000");


            //String egroup_code = dw_criteria.GetItemString(1, "egroup_code");

            String coop_name = state.SsCoopName;
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(branch_id, ArgumentType.String);
            lnv_helper.AddArgument(ascstype, ArgumentType.String);
            lnv_helper.AddArgument(deptacc, ArgumentType.String);
            //lnv_helper.AddArgument(ascstype, ArgumentType.String);




            //-------------------------------------------------------

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();
            try
            {
                CommonLibrary.WsReport.Report lws_report = WsUtil.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, Session.SessionID, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
                PDFUtil pdfUtil = new PDFUtil(Session);
                pdfUtil.SourceFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.sourcefile") + pdfFileName;
                try
                {
                    String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        pdfUtil.IsSendPDF = true;
                        pdfUtil.DesFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.desfile") + ascstype + '-' + branch_id + "-" + dt.GetString("coopbranch_desc") + ".pdf";
                    }
                }
                catch { pdfUtil.IsSendPDF = false; }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }
        #endregion

        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        //#endregion
    }
}