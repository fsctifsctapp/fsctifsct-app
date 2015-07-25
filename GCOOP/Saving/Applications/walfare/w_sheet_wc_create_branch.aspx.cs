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
    public partial class w_sheet_wc_create_branch : PageWebSheet, WebSheet
    {
        private string pbl = "w_sheet_wc_permission_all.pbl";

        protected String jsRetrieveBranch;
        protected String jsBranchChk;
        protected String postPost;
        protected String postProvince;

        public void InitJsPostBack()
        {
            jsRetrieveBranch = WebUtil.JsPostBack(this, "jsRetrieveBranch");
            jsBranchChk = WebUtil.JsPostBack(this, "jsBranchChk");
            postPost = WebUtil.JsPostBack(this, "postPost");
            postProvince = WebUtil.JsPostBack(this, "postProvince");
        }

        public void WebSheetLoadBegin()
        {            
            if (!IsPostBack)
            {               
                DwMain.InsertRow(0);
                
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
                case "jsRetrieveBranch":
                    RetrieveBranch();
                    break;

                case "postProvince":
                    JsPostProvince();
                    break;

                case "jsBranchChk":
                    JsChkBranch();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string coop_id = DwMain.GetItemString(1, "coopbranch_id");
                if (coop_id.Trim().Length != 4)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรหัสสหกรณ์เป็ฯตัวเอง 4 หลัก");
                }
                else
                {

                    if (HdStatusSave.Value == "edit")
                    {
                        try
                        {
                            bool result = false;
                            String XmlMain = DwMain.Describe("DataWindow.data.XML");
                            string branch_id = state.SsBranchId;
                            string cs_type = state.SsCsType;

                            result = WsUtil.Walfare.Update_Coopbranch(state.SsWsPass, state.SsApplication, XmlMain, pbl, cs_type, branch_id);

                            if (result)
                            {
                                string coop_desc = DwMain.GetItemString(1, "coopbranch_desc");
                                LtServerMessage.Text = WebUtil.CompleteMessage("อัพเดทข้อมูล " + coop_id+" " + coop_desc + " สำเร็จ");

                            }

                        }
                        catch (Exception ex)
                        {
                           
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + DwMain.GetItemString(1, "coopbranch_id") + "' and cs_type = '" + state.SsCsType + "'";
                            Sdt dt = WebUtil.QuerySdt(sql);
                            if (dt.GetRowCount() > 0)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากรหัสสหกรณ์นี้มีอยู่แล้ว");
                            }
                            else
                            {
                                DwMain.SetItemSqlString(1, "cs_type", state.SsCsType);
                                DwUtil.InsertDataWindow(DwMain, pbl, "cmucfcoopbranch");

                                if (state.SsCsType == "8")
                                {
                                    bool result = false;
                                    String XmlMain = DwMain.Describe("DataWindow.data.XML");
                                    result = WsUtil.Walfare.BranchDoc_New(state.SsWsPass, state.SsApplication, XmlMain, pbl, state.SsCsType, "WCFEEYEARSLIP");
                                }
                                
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

                                
                                DwMain.Reset();
                                DwMain.InsertRow(0);
                            }
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้กรุณาตรวจสอบข้อมูลให้ถูกต้อง");
                        }
                    }
                }
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้กรุณาตรวจสอบข้อมูลให้ถูกต้อง"); }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "province_code", pbl, null);
                DwMain.Modify("use_status.visible=0");
                if (HdStatusSave.Value == "edit")
                {
                    DwMain.Modify("use_status.visible=1");
                }

                try
                {
                   //// DwMain.SetItemString(1, "postcode", "");
                    String pvCode = DwUtil.GetString(DwMain, 1, "province_code", "");
                    if (pvCode != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "district_code", pbl, pvCode);
                        String dtCode = DwUtil.GetString(DwMain, 1, "district_code", "");
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
            DwMain.SaveDataCache();
        }

        public void RetrieveBranch()
        {
            try
            {
                String branch_id = DwMain.GetItemString(1, "coopbranch_id");
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, branch_id, state.SsCsType);
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้กรูณาตรวจสอบรหัสสหกรณ์ให้ถูกต้อง");
                DwMain.Reset();
                DwMain.InsertRow(0);
                WebSheetLoadEnd();
            }
            
        }
        private void JsPostProvince()
        {
            DwMain.SetItemString(1, "district_code", "");
        }

        public void JsChkBranch()
        {
            HdStatusSave.Value = "";
            DwMain.SetItemString(1, "coopbranch_desc", "");
            DwMain.SetItemString(1, "province_code", "");
            DwMain.SetItemString(1, "district_code", "");
            DwMain.SetItemString(1, "postcode", "");
            DwMain.SetItemString(1, "section", "");
            DwMain.SetItemDecimal(1, "area_desc", 0);
            DwMain.SetItemString(1, "account_no", "");
            DwMain.SetItemString(1, "bank_name", "");
            DwMain.SetItemString(1, "bank_branch", "");
            DwMain.SetItemString(1, "account_name", "");

            String dtCs_type = String.Empty, cstype_desc = String.Empty;
            String branch_id = DwMain.GetItemString(1, "coopbranch_id");

            if (state.SsCsType == "1" || state.SsCsType == "3")
            {
                cstype_desc = "ณาปนกิจ สอ.ครูไทย";
                String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "' and cs_type in('1','3')";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dtCs_type = dt.GetString("cs_type");
                    if (dtCs_type == state.SsCsType)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้ว กดปุ่ม < แก้ไข > เพื่อตรวจสอบหรือแก้ไขข้อมูล");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้วใน " + cstype_desc);
                        DwMain.Modify("coopbranch_desc.protect=1");
                        DwMain.Modify("province_code.protect=1");
                        DwMain.Modify("district_code.protect=1");
                        DwMain.Modify("postcode.protect=1");
                        DwMain.Modify("section.protect=1");
                        DwMain.Modify("area_desc.protect=1");
                        DwMain.Modify("account_no.protect=1");
                        DwMain.Modify("bank_name.protect=1");
                        DwMain.Modify("bank_branch.protect=1");
                        DwMain.Modify("account_name.protect=1");
                    }
                }
                else
                    LtServerMessage.Text = WebUtil.CompleteMessage("รหัส สหกรณ์ " + branch_id + " ยังไม่ถูกใช้");
            }

            else if (state.SsCsType == "2" || state.SsCsType == "4" || state.SsCsType == "5" || state.SsCsType == "7")
            {
                String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "' and cs_type in('2','4','5','7')";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dtCs_type = dt.GetString("cs_type");
                    if(dtCs_type == "2") cstype_desc = "ณาปนกิจ สอ.ทหาร";
                    if(dtCs_type == "4") cstype_desc = "ณาปนกิจ สอ.ราชการไทย";
                    if (dtCs_type == "5") cstype_desc = "ณาปนกิจ สอ.รัฐวิสาหกิจไทย";
                    if (dtCs_type == "7") cstype_desc = "ณาปนกิจ สอ.สถานประกอบการ";
                    if (dtCs_type == state.SsCsType)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้ว กดปุ่ม < แก้ไข > เพื่อตรวจสอบหรือแก้ไขข้อมูล");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้วใน " + cstype_desc);
                        DwMain.Modify("coopbranch_desc.protect=1");
                        DwMain.Modify("province_code.protect=1");
                        DwMain.Modify("district_code.protect=1");
                        DwMain.Modify("postcode.protect=1");
                        DwMain.Modify("section.protect=1");
                        DwMain.Modify("area_desc.protect=1");
                        DwMain.Modify("account_no.protect=1");
                        DwMain.Modify("bank_name.protect=1");
                        DwMain.Modify("bank_branch.protect=1");
                        DwMain.Modify("account_name.protect=1");
                    }
                }
                else
                    LtServerMessage.Text = WebUtil.CompleteMessage("รหัส สหกรณ์ " + branch_id + " ยังไม่ถูกใช้");
            }

            else if (state.SsCsType == "8")
            {
                String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "' and cs_type = '8'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dtCs_type = dt.GetString("cs_type");
                    LtServerMessage.Text = WebUtil.WarningMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้ว กดปุ่ม < แก้ไข > เพื่อตรวจสอบหรือแก้ไขข้อมูล");
                   
                }
                else
                    LtServerMessage.Text = WebUtil.CompleteMessage("รหัส สหกรณ์ " + branch_id + " ยังไม่ถูกใช้");
            }

            else if (state.SsCsType == "6")
            {
                String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "' and cs_type = '6'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dtCs_type = dt.GetString("cs_type");
                    LtServerMessage.Text = WebUtil.WarningMessage("รหัสสหกรณ์ " + branch_id + " มีอยู่แล้ว กดปุ่ม < แก้ไข > เพื่อตรวจสอบหรือแก้ไขข้อมูล");

                }
                else
                    LtServerMessage.Text = WebUtil.CompleteMessage("รหัส สหกรณ์ " + branch_id + " ยังไม่ถูกใช้");
            }

        }
    }
}