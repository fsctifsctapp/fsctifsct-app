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
    public class KpPauseLoan : MainProgress,Running
    {
        n_cst_pauseloan_keep svPauseLoan;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        str_keep  astr_keep = new str_keep();

        public KpPauseLoan(String connectionString, str_keep astr_keep)
        {
            this.connectString = connectionString;
            this.astr_keep = astr_keep;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svPauseLoan = new n_cst_pauseloan_keep();
            svPauseLoan.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svPauseLoan.of_setprogress(ref svProgress);
       
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

        ~KpPauseLoan()
        {
            DisConnect();
        }


        #region Running Members

        public str_progress GetProgress()
        {
            if (isError)
            {
                return progress;
            }
            else
            {
                svPauseLoan.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
             if (thread != null)
             {
                 try
                 {
                     svPauseLoan.of_savepauseloankeep(astr_keep);
                     DisConnect();
                 }
                 catch (Exception ex)
                 {
                     DisConnect();
                     isError = true;
                     svPauseLoan.of_setprogress(ref svProgress);
                     progress = svProgress.of_get_progress();
                     progress.status = -1;
                     progress.progress_text = ex.Message;
                 }
             }
        }

        #endregion
    }
}
