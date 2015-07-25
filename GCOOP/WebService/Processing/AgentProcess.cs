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
    public class AgentProcess : MainProgress ,Running 
    {
        n_cst_agentproc_service svAgPrc;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        private bool isComplete = false;
        String xml_agentoption;

         public AgentProcess(String connectionString, String xml_agentoption)
        {
            this.connectString = connectionString;
            this.xml_agentoption = xml_agentoption;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svAgPrc  = new n_cst_agentproc_service();
            svAgPrc.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svAgPrc.of_setprogress(ref svProgress);

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

        ~AgentProcess()
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
                svAgPrc.of_setprogress(ref svProgress);
                return svProgress.of_get_progress();
            }
        }

        public void Run()
        {
            if (thread != null)
             {
                 try
                 {
                     str_agent astr_agent = new str_agent();
                     astr_agent.xml_agentoption = xml_agentoption;
                     svAgPrc.of_agentprocess(astr_agent);
                     //progress.status = 1;
                     ////progress.progress_text = c;
                     //isComplete = true;
                     DisConnect();
                 }
                 catch (Exception ex)
                 {
                     DisConnect();
                     isError = true;
                     svAgPrc.of_setprogress(ref svProgress);
                     progress = svProgress.of_get_progress();
                     progress.status = -1;
                     progress.progress_text = ex.Message;
                 }
             }
        }

        #endregion
    }
}
