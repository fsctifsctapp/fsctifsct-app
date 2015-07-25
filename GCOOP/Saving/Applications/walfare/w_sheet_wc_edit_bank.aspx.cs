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
    public partial class w_sheet_wc_edit_bank : PageWebSheet, WebSheet
    {
        protected String jsbranch_id;
        protected String jsbranch_desc;
        protected String jsselect_option;
        private DwThDate tDwMain;
        // private DwThDate tDWOption;


        public void InitJsPostBack()
        {
            jsbranch_id = WebUtil.JsPostBack(this, "jsbranch_id");
            jsbranch_desc = WebUtil.JsPostBack(this, "jsbranch_desc");
            jsselect_option = WebUtil.JsPostBack(this, "jsselect_option");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptopen_date", "deptopen_tdate");

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
                DwOption.SetItemString(1, "branch_idd", state.SsBranchId);
                DwOption.SetItemString(1, "branch_id", state.SsBranchId);
                //DwIn.Visible = false;
                //DwMain.Visible = false;
                jjsselect_option();
            }
            else
            {
                this.RestoreContextDw(DwOption);
                this.RestoreContextDw(DwMain);

            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsbranch_id":
                    ChangeBranchId();
                    break;
                case "jsbranch_desc":
                    ChangeBranchDesc();
                    break;
                case "jsselect_option":
                    jjsselect_option();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string select_option;
                try { select_option = DwOption.GetItemString(1, "branch_id"); }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกศูนย์ประสานงาน");
                    return;
                }
                String XmlMain = DwMain.Describe("DataWindow.data.XML");
                //string branch_id = state.SsBranchId;

                bool result = false;

                result = WsUtil.Walfare.Edit_bank_branch(state.SsWsPass, state.SsApplication, XmlMain, "w_sheet_wc_trn_memb.pbl", state.SsUsername, state.SsCsType, select_option);

                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                //  LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก สถานะอนุมัติและเลขกรอก สอ.ใหม่ ก่อนทำการกดบันทึก");
            }
        }

        public void WebSheetLoadEnd()
        {
            string not_cstype, cstype;
            if (state.SsCsType == "1")
            {
                not_cstype = "8";
                cstype = "%";
            }
            else
            {
                not_cstype = "0";
                cstype = state.SsCsType;

            }
            try
            {
                DwUtil.RetrieveDDDW(DwOption, "branch_idd", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);
                DwUtil.RetrieveDDDW(DwMain, "coopbranch_id", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);
                DwUtil.RetrieveDDDW(DwMain, "coopbranch_id_new", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);

            }
            catch { }
            // tDWOption.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
        }



        public void ChangeBranchId()
        {
            //int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwOption.SetItemString(1, "branch_idd", DwOption.GetItemString(1, "branch_id"));
                jjsselect_option();
            }
            catch { }
        }

        public void ChangeBranchDesc()
        {
            // int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwOption.SetItemString(1, "branch_id", DwOption.GetItemString(1, "branch_idd"));
                jjsselect_option();
            }
            catch { }
        }

        public void jjsselect_option()
        {
            DwMain.Visible = true;
            DwOption.Visible = true;
            string cs_t = state.SsCsType;
            string branch = DwOption.GetItemString(1, "branch_id");
            DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_trn_memb.pbl", null, branch, cs_t);

        }

    }
}