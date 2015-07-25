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
    public partial class w_dlg_wc_printslip : PageWebDialog, WebDialog
    {

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                string deptslip_no = Request["deptslip_no"];
                DwUtil.RetrieveDataWindow(DwMain_org, "w_sheet_wc_walfare_new.pbl", null, deptslip_no, state.SsBranchId, state.SsCsType);
                DwUtil.RetrieveDataWindow(DwMain_coppy, "w_sheet_wc_walfare_new.pbl", null, deptslip_no, state.SsBranchId, state.SsCsType);
            }
            catch(Exception ex){
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }
    }
}