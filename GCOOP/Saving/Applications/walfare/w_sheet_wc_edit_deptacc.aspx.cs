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
    public partial class w_sheet_wc_edit_deptacc : PageWebSheet, WebSheet
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
                //String sql = "select * from cmucfcoopbranch where cs_type='" + state.SsCsType + "' order by coopbranch_desc";
                //DataTable dt = WebUtil.Query(sql);
                //DdCoopBranchId.DataSource = dt;
                //DdCoopBranchId.DataValueField = "coopbranch_id";
                //DdCoopBranchId.DataTextField = "coopbranch_desc";
                //DdCoopBranchId.DataBind();
                //DdCoopBranchId.SelectedValue = state.SsBranchId;
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
                //UserAccount userAccc = new UserAccount();
                //userAccc.UserName = TbUsername.Text.Trim();
                //userAccc.Password = TbPassword.Text.Trim();
                //userAccc.FullName = TbFullName.Text.Trim();
                //userAccc.Description = TbDescription.Text.Trim();
                //userAccc.CoopBranchId = DdCoopBranchId.SelectedValue.Trim();
                //Walfare wf = WsUtil.Walfare;
                bool result = false;
                result = WsUtil.Walfare.Edit_deptacc_no(state.SsWsPass, state.SsApplication, state.SsBranchId, state.SsCsType);
                  //  wf.Dispose();
                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
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