using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Saving.CmConfig;
namespace Saving.Applications.ap_deposit
{
    public partial class w_sheet_dp_prncfix_process : PageWebSheet,WebSheet
    {
        private DwThDate tDwMain;
        protected String PrncFixProcess;
        
        #region WebSheet Members

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain,this);
            tDwMain.Add("start_date", "start_tdate");
            tDwMain.Add("end_date", "end_tdate");
            PrncFixProcess = WebUtil.JsPostBack(this, "PrncFixProcess");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "start_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "end_date", state.SsWorkDate);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                tDwMain.Eng2ThaiAllRow();
                DwUtil.RetrieveDDDW(DwMain, "branch_id", "dp_prncfix_process.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "start_dp_type", "dp_prncfix_process.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "end_dp_type", "dp_prncfix_process.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PrncFixProcess")
            {
                String branchId = DwMain.GetItemString(1, "branch_id");
                DateTime startDate = DwMain.GetItemDateTime(1, "start_date");
                DateTime endDate = DwMain.GetItemDateTime(1, "end_date");
                String startType = DwMain.GetItemString(1, "start_dp_type");
                String endType = DwMain.GetItemString(1, "end_dp_type");
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion
    }
}
