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
    public class SlClosemonthProcess : MainProgress, Running
    {
        n_cst_shrlon_balance svshclosemonth;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        short ai_year;
        short ai_month;
        String as_appname;
        String as_userid;

        public SlClosemonthProcess(String connectionString, short ai_year, short ai_month, String as_appname, String as_userid)
        {
            this.connectString = connectionString;
            this.ai_year = ai_year;
            this.ai_month = ai_month;
            this.as_appname = as_appname;
            this.as_userid = as_userid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svshclosemonth = new n_cst_shrlon_balance();
            svshclosemonth.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svshclosemonth.of_setprogress(ref svProgress);
       
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

        ~SlClosemonthProcess()
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
                svshclosemonth.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svshclosemonth.of_closemonth(ai_year, ai_month, as_appname, as_userid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svshclosemonth.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }

        #endregion
    }
}
