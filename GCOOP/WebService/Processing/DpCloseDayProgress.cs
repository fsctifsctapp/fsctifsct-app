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
    public class DpCloseDayProgress : MainProgress, Running
    {
        private n_cst_deposit_service svDep;
        private n_cst_dbconnectservice svCon;
        private n_cst_progresscontrol svProgress;
        private bool isError = false;

        //Argument
        private DateTime closeDate;
        private DateTime workDate;
        private String appName;
        private String branchId;
        private String entryId;
        private String machine;

        //Constructor
        public DpCloseDayProgress(String wsPass, DateTime closeDate, DateTime workDate, String appName, String branchId, String entryId, String machine)
        {
            Security sec = new Security(wsPass);
            this.connectString = sec.ConnectionString;
            this.closeDate = closeDate;
            this.workDate = workDate;
            this.appName = appName;
            this.branchId = branchId;
            this.entryId = entryId;
            this.machine = machine;

            //เริ่มประกาศตัวแปรเพื่อประมวลผล
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

        //Destructor
        ~DpCloseDayProgress()
        {
            DisConnect();
        }

        public void RunningThread()
        {
            svDep.of_close_day(closeDate, workDate, application, branchId, entryId, machine);
            int maxLoop = Convert.ToInt32(svDep.of_get_loopcloseday(closeDate));
            DateTime dtCurrrent = closeDate;
            for (int i = 1; i <= maxLoop; i++)
            {
                //1.
                svDep.of_operate_endday(dtCurrrent, branchId, entryId, machine);
                //depService.OperateEndDay(state.SsWsPass, state.SsWorkDate, state.SsBranchId, state.SsUsername, state.SsClientComputerName);

                //2.
                svDep.of_process_upint(branchId, entryId, machine, workDate, dtCurrrent);
                //depService.ProcessUpInt(state.SsWsPass, state.SsBranchId, state.SsUsername, state.SsClientComputerName, state.SsWorkDate, closeDate);

                //3.
                svDep.of_updatereport_balday(dtCurrrent, branchId, entryId);
                //depService.UpdateReportBalDay(state.SsWsPass, state.SsWorkDate, state.SsBranchId, state.SsUsername);

                //4.
                if (svDep.of_is_endmonth_date(dtCurrrent))
                {
                    //5.
                    svDep.of_calint_remain(branchId, dtCurrrent);
                    svDep.of_close_month(workDate, application, Convert.ToInt16(workDate.Month), Convert.ToInt16(workDate.Year), branchId, entryId);
                }
                //6.
                if (svDep.of_is_endyear_date(dtCurrrent))
                {
                    //7.
                    svDep.of_close_year(Convert.ToInt16(workDate.Year), workDate, entryId, machine, application, branchId);
                }

                dtCurrrent = dtCurrrent.AddDays(1);

                //8.
                svDep.of_genreport_balday(dtCurrrent, branchId, entryId);

                //9.
                String errMessage1 = "";
                int ii = svDep.of_postint_nextday(dtCurrrent, workDate, entryId, branchId, machine, ref errMessage1);
            }
            svDep.of_update_closedaystatus(closeDate, application, branchId);
        }

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
                    RunningThread();
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
