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
    public class SaveReceiveGroupMem : MainProgress ,Running 
    {
        n_cst_agentoperate_service svAgent;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        private bool isError = false;
        String xml_head;
        String xml_detail;
        String as_entry_id;
        String as_machine_id;
        DateTime adtm_adj_time;
        DateTime adtm_system_date;


        public SaveReceiveGroupMem(String connectionString, String xml_head, String xml_detail, String as_entry_id, String as_machine_id, DateTime adtm_adj_time, DateTime adtm_system_date)
        {
            this.connectString = connectionString;
            this.xml_head = xml_head;
            this.xml_detail = xml_detail;
            this.as_entry_id = as_entry_id;
            this.as_machine_id = as_machine_id;
            this.adtm_adj_time = adtm_adj_time;
            this.adtm_system_date = adtm_system_date;
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

        ~SaveReceiveGroupMem()
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
                     svAgent.of_savereceivegroupmem(astr_agent, as_entry_id, as_machine_id, adtm_adj_time, adtm_system_date);
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
