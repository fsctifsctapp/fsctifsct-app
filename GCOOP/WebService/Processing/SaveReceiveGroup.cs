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
    public class SaveReceiveGroup : MainProgress, Running 
    {
        n_cst_agentoperate_service svAgent;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        String xml_head;
        String xml_detail;

         public SaveReceiveGroup(String connectionString, String xml_head, String xml_detail)
        {
            this.connectString = connectionString;
            this.xml_head = xml_head;
            this.xml_detail = xml_detail;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svAgent  = new n_cst_agentoperate_service();
            svAgent.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svAgent.of_setprogress(ref svProgress);

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

        ~SaveReceiveGroup()
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
                svAgent.of_setprogress(ref svProgress);
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
                     astr_agent.xml_head  = xml_head;
                     astr_agent.xml_detail = xml_detail;
                     svAgent.of_savereceivegroup(astr_agent);
                     DisConnect();
                 }
                 catch (Exception ex)
                 {
                     DisConnect();
                     isError = true;
                     svAgent.of_setprogress(ref svProgress);
                     progress = svProgress.of_get_progress();
                     progress.status = -1;
                     progress.progress_text = ex.Message;
                 }
             }
        }

        #endregion
    }
}
