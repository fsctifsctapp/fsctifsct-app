using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using pbservice;

namespace WebService.Processing
{
    public class DpIntAdvanceProgress : MainProgress, Running
    {
        private n_cst_deposit_service svDep;
        private n_cst_dbconnectservice svCon;
        private n_cst_progresscontrol svProgress;
        private bool isError = false;

        //Argument
        private DateTime workDate;
        private DateTime dateFrom;
        private DateTime dateTo;
        private String deptTypeFrom;
        private String deptTypeTo;
        private String branchId;     
 
        public DpIntAdvanceProgress(String wsPass, DateTime workDate, DateTime dateFrom, DateTime dateTo, String deptTypeFrom, String deptTypeTo, String branchId)
        {
            Security sec = new Security(wsPass);
            this.connectString = sec.ConnectionString;
            this.workDate = workDate;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.deptTypeFrom = deptTypeFrom;
            this.deptTypeTo = deptTypeTo;
            this.branchId = branchId;

            progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svDep = new n_cst_deposit_service();
            svDep.of_settrans(svCon);
            svDep.of_init();

            svProgress = new n_cst_progresscontrol();
            svDep.of_set_progresscontrol(ref svProgress);
            SetRunning(this);
        }

        ~DpIntAdvanceProgress()
        {
            DisConnect();
        }
        
        //----------------------------------------

        #region Running Members

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        public str_progress GetProgress()
        {
            if (isError)
            {
                return progress;
            }
            else
            {
                svDep.of_set_progresscontrol(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svDep.of_gen_int_advance(workDate, dateFrom, dateTo, deptTypeFrom, deptTypeTo, branchId);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svDep.of_set_progresscontrol(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }
        }

        #endregion
    }
}
