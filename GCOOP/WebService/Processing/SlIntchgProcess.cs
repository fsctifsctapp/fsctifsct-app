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
    public class SlIntchgProcess : MainProgress, Running
    {
        n_cst_lnproc_intchanged svlnintchg;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        String as_xmlintsetcriteria;
        String as_entryid;


        public SlIntchgProcess(String connectionString, String as_xmlintsetcriteria, String as_entryid)
        {
            this.connectString = connectionString;
            this.as_xmlintsetcriteria = as_xmlintsetcriteria;
            this.as_entryid = as_entryid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svlnintchg = new n_cst_lnproc_intchanged();
            svlnintchg.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svlnintchg.of_setprogress(ref svProgress);
       
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

        ~SlIntchgProcess()
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
                svlnintchg.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    svlnintchg.of_procintchg(as_xmlintsetcriteria, as_entryid);
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svlnintchg.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }

        }
        #endregion
    }
}
