using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsWalfare;
using System.Data;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_create_username : PageWebSheet, WebSheet
    {
        protected String jsselectBranch;
        public void InitJsPostBack()
        {
            jsselectBranch = WebUtil.JsPostBack(this, "jsselectBranch");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                String sql = "select * from cmucfcoopbranch where cs_type='" + state.SsCsType + "' order by coopbranch_desc";
                DataTable dt = WebUtil.Query(sql);
                DdCoopBranchId.DataSource = dt;
                DdCoopBranchId.DataValueField = "coopbranch_id";
                DdCoopBranchId.DataTextField = "coopbranch_desc";
                DdCoopBranchId.DataBind();
                DdCoopBranchId.SelectedValue = state.SsBranchId;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsselectBranch":
                    JSselectBranch();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                UserAccount userAccc = new UserAccount();
                userAccc.UserName = TbUsername.Text.Trim();
                userAccc.Password = TbPassword.Text.Trim();
                userAccc.FullName = TbFullName.Text.Trim();
                userAccc.Description = TbDescription.Text.Trim();
                userAccc.CoopBranchId = DdCoopBranchId.SelectedValue.Trim();
                Walfare wf = WsUtil.Walfare;
                if (TbConfirmPassword.Text.Trim() == userAccc.Password)
                {
                    wf.NewUserAccount(state.SsWsPass, state.SsApplication, userAccc);
                    wf.Dispose();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรหัสให้ตรงกัน");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
        }
        public void JSselectBranch()
        {
            string BranchId = HdBranchId.Value;

            //String sql = "select * from cmucfcoopbranch where cs_type='" + state.SsCsType + "' where coopbranch_desc = '" + BranchId + "'";
            //DataTable dt = WebUtil.Query(sql);
            DdCoopBranchId.SelectedValue = BranchId;
        }
    }
}