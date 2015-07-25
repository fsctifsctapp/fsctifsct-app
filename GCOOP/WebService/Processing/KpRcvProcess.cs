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
    public class KpRcvProcess : MainProgress, Running
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_keeping_process svKeeping;
        private n_cst_progresscontrol svProgress;
        private bool isError = false;
        private String xml;

        public KpRcvProcess(String connectionString, String xml)
        {
            this.connectString = connectionString;
            this.xml = xml;
            ConstructorEnding();
        }

        private void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svKeeping = new n_cst_keeping_process();
            svKeeping.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svKeeping.of_setprogress(ref svProgress);
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

        ~KpRcvProcess()
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
                svKeeping.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svKeeping.of_rcvprocess(xml);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svKeeping.of_setprogress(ref svProgress);
                    Progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }
        }

        #endregion
    }
}
