using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SecurityEngine;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Applications.walfare.dlg
{
    public partial class w_dlg_wc_branch_info : PageWebDialog, WebDialog
    {
        protected String postselectbranch_id;
        protected String postselectbranch_desc;
        protected String postSaveInformation;

        public void InitJsPostBack()
        {
            postselectbranch_id = WebUtil.JsPostBack(this, "postselectbranch_id");
            postselectbranch_desc = WebUtil.JsPostBack(this, "postselectbranch_desc");
            postSaveInformation = WebUtil.JsPostBack(this, "postSaveInformation");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {                
                try
                {
                    string user_name = Request["username"];
                    DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_permission_all.pbl", null, state.SsCsType, user_name);
                    string pwdSet = DwMain.GetItemString(1, "password");
                    string pwd = new SecurityEngine.Decryption().DecryptAscii(pwdSet);
                    DwMain.SetItemString(1, "password", pwd);
                    DwMain.SetItemString(1, "confirm_password", pwd);
                    DwMain.SetItemString(1, "coopbranch_id_1", DwMain.GetItemString(1, "coopbranch_id"));
                    if (DwMain.RowCount == 0)
                    {
                        DwMain.InsertRow(0);
                        LtServerMessage.Text = WebUtil.ErrorMessage("<p align=\"center\">ไม่สามาทำรายการได้เนื่องจาก <br /> User Name นี้ไม่อยู่ใน สมาคม"+ state.SsCsDesc + "</p>");
                        return;
                    }
                    decimal user_type = DwMain.GetItemDecimal(1, "user_type");
                    if (user_type == 1)
                    {
                        DwUtil.RetrieveDataWindow(DwPermiss, "w_sheet_wc_permission_all.pbl", null, user_name);
                    }
                }
                catch { }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwPermiss);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postselectbranch_id")
            {
                Selectbranch_id();
            }
            else if (eventArg == "postselectbranch_desc")
            {
                Selectbranch_desc();
            }
            else if (eventArg == "postSaveInformation")
            {
                SaveData();
            }
        }

        public void WebDialogLoadEnd()
        {
             try
            {
                string coopbr = DwMain.GetItemString(1, "coopbranch_id_1");
                DwUtil.RetrieveDDDW(DwMain, "coopbranch_id", "w_sheet_wc_permission_all.pbl", coopbr, state.SsCsType);
            }catch{}

            DwMain.SaveDataCache();
            DwPermiss.SaveDataCache();
        }

        public void Selectbranch_id()
        {
            try
            {
                DwMain.SetItemString(1,"coopbranch_id", DwMain.GetItemString(1, "coopbranch_id_1"));
            }
            catch
            {

            }
        }
        public void Selectbranch_desc()
        {
            try
            {
                DwMain.SetItemString(1, "coopbranch_id_1", DwMain.GetItemString(1, "coopbranch_id"));
            }
            catch
            {

            }
        }
        public void SaveData()
        {
            try
            {
                if (DwMain.GetItemString(1, "password") == DwMain.GetItemString(1, "confirm_password"))
                {
                    string pwd = new SecurityEngine.Encryption().EncryptAscii(DwMain.GetItemString(1, "password"));
                    DwMain.SetItemString(1, "password", pwd);
                    DwUtil.UpdateDateWindow(DwMain, "w_sheet_wc_permission_all.pbl", "amsecusers");
                    decimal user_type = DwMain.GetItemDecimal(1, "user_type");
                    if (user_type == 1)
                    {
                        for (int i = 1; i <= DwPermiss.RowCount; i++)
                        {
                            try
                            {
                                DwPermiss.SetItemDecimal(i, "save_status", DwPermiss.GetItemDecimal(i, "check_flag"));
                            }
                            catch
                            {
                                DwPermiss.SetItemDecimal(i, "save_status", 0);
                            }
                        }
                        DwUtil.UpdateDateWindow(DwPermiss, "w_sheet_wc_permission_all.pbl", "amsecpermiss");
                    }
                    DwMain.SetItemString(1, "password", "");
                    DwMain.SetItemString(1, "confirm_password", "");
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรหัสผ่านให้ตรงกัน");
                }
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ <br />" + ex);
            }
        }
    }
}