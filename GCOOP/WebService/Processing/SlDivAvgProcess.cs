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
using System.Threading;

namespace WebService.Processing
{
    public class SlDivAvgProcess: MainProgress, Running
    {
        n_cst_dividendprocmet_service svsDivAvg;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        str_divavg strDivAvg = new str_divavg();

        public SlDivAvgProcess(String connectionString, str_divavg strDivAvg)
        {
            this.connectString = connectionString;
            this.strDivAvg = strDivAvg;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svsDivAvg = new n_cst_dividendprocmet_service();
            svsDivAvg.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svsDivAvg.of_setprogress(ref svProgress);
       
            SetRunning(this);
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~SlDivAvgProcess()
        {
            DisConnect();
        }

        //--------------------------------------------------------------------------------------


        #region Running Members

        public str_progress GetProgress()
        {
            if (isError)
            {
                return progress;
            }
            else
            {
                svsDivAvg.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svsDivAvg.of_metprocess(strDivAvg);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svsDivAvg.of_setprogress(ref svProgress);
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }


        #endregion
    }
}
