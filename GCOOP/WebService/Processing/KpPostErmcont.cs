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
    public class KpPostErmcont : MainProgress, Running
    {
        n_cst_dbconnectservice svCon;
        n_cst_keeping_process svKp;
        n_cst_progresscontrol svProgress;
        private short year;
        private short month;
        private DateTime apvdate;
        private String userid;
        private bool isError = false;

        public KpPostErmcont(String connectionString, short year, short month, DateTime apvdate, String userid)
        {
            this.connectString = connectionString;
            this.year = year;
            this.month = month;
            this.apvdate = apvdate;
            this.userid = userid;
            ConstructorEnding();
        }

        private void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svKp = new n_cst_keeping_process();
            svKp.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svKp.of_setprogress(ref svProgress);
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

        ~KpPostErmcont()
        {
            DisConnect();
        }

        //--------------------------------------------------------------------------------------


        #region Running Members

        public str_progress GetProgress()
        {
            if (isError)
            {
                return Progress;
            }
            else
            {
                svKp.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svKp.of_postemrcont(year, month, apvdate, userid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svKp.of_setprogress(ref svProgress);
                    Progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }
        }

        #endregion

       
    }
}
