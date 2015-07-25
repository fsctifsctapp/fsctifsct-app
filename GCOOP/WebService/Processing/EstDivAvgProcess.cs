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
    public class EstDivAvgProcess : MainProgress ,Running 
    {
        n_cst_dividend_service svCalEstdiv;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        private bool isComplete = false;
        String xml_procinfo;
        String xml_loantype;

         public EstDivAvgProcess(String connectionString, String xml_procinfo, String xml_loantype)
        {
            this.connectString = connectionString;
            this.xml_procinfo = xml_procinfo;
            this.xml_loantype = xml_loantype;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svCalEstdiv = new n_cst_dividend_service();
            svCalEstdiv.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svCalEstdiv.of_setprogress(ref svProgress);

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

        ~EstDivAvgProcess()
        {
            DisConnect();
        }

        #region Running Members

        public str_progress GetProgress()
        {
            if (isError || isComplete)
            {
                return progress;
            }
            else
            {
                svCalEstdiv.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
            {
                try
                {
                    String EstText = svCalEstdiv.of_estimate_process(xml_procinfo, xml_loantype);
                    svCalEstdiv.of_setprogress(ref svProgress);
                    Progress = svProgress.of_get_progress();
                    progress.status = 1;
                    progress.progress_text = EstText;
                    isComplete = true;
                    DisConnect();
                }
                catch (Exception ex)
                {
                    DisConnect();
                    isError = true;
                    svCalEstdiv.of_setprogress(ref svProgress);
                    progress = svProgress.of_get_progress();
                    progress.status = -1;
                    progress.progress_text = ex.Message;
                }
            }
        }

        #endregion
    }
}
