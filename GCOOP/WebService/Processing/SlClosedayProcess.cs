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
    public class SlClosedayProcess : MainProgress, Running
    {
        n_cst_shrlon_closeday svshcloseday;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        DateTime adtm_closeday;
        String as_appname;
        String as_userid;

        public SlClosedayProcess(String connectionString, DateTime adtm_closeday, String as_appname, String as_userid)
        {
            this.connectString = connectionString;
            this.adtm_closeday = adtm_closeday;
            this.as_appname = as_appname;
            this.as_userid = as_userid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svshcloseday = new n_cst_shrlon_closeday();
            svshcloseday.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svshcloseday.of_setprogress(ref svProgress);
       
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

        ~SlClosedayProcess()
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
                svshcloseday.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svshcloseday.of_closeday(adtm_closeday, as_appname, as_userid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svshcloseday.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }

        #endregion
    }
}
