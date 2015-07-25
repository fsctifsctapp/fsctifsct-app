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
    public class SlCloseyearProcess: MainProgress, Running
    {
        n_cst_shrlon_close_year svshcloseyear;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        short ai_year;
        String as_branch;
        String as_entryid;

        public SlCloseyearProcess(String connectionString,short ai_year, String as_branch, String as_entryid)
        {
            this.connectString = connectionString;
            this.ai_year = ai_year;
            this.as_branch = as_branch;
            this.as_entryid = as_entryid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svshcloseyear = new n_cst_shrlon_close_year();
            svshcloseyear.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svshcloseyear.of_setprogress(ref svProgress);
       
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

        ~SlCloseyearProcess()
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
                svshcloseyear.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svshcloseyear.of_closeyear(ai_year, as_branch, as_entryid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svshcloseyear.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }

        #endregion
    }
}
