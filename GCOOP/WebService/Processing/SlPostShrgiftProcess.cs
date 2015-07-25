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
    public class SlPostShrgiftProcess : MainProgress, Running
    {
        n_cst_shproc_shrgift svshpost;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        String as_xmlgiftcriteria;
        String as_postid;
        String as_branchid;

        public SlPostShrgiftProcess(String connectionString, String as_xmlgiftcriteria, String as_postid, String as_branchid)
        {
            this.connectString = connectionString;
            this.as_xmlgiftcriteria = as_xmlgiftcriteria;
            this.as_postid = as_postid;
            this.as_branchid = as_branchid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svshpost = new n_cst_shproc_shrgift();
            svshpost.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svshpost.of_setprogress(ref svProgress);
       
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

        ~SlPostShrgiftProcess()
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
                svshpost.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svshpost.of_postshrgift(as_xmlgiftcriteria, as_postid, as_branchid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svshpost.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }

        #endregion
    }
}
