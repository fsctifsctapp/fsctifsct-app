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

namespace WebService.Processing
{
    public class SlCfbalProcess : MainProgress, Running
    {
        n_cst_lnproc_confirmbal svlnproc;
        n_cst_dbconnectservice svCon;
        n_cst_progresscontrol svProgress;
        String ls_xmlbalcriteria;
        String ls_entryid;

        public SlCfbalProcess(String connectionString, String ls_xmlbalcriteria, String ls_entryid)
        {
            this.connectString = connectionString;
            this.ls_xmlbalcriteria = ls_xmlbalcriteria;
            this.ls_entryid = ls_entryid;
            ConstructorEnding();
        }

        public void ConstructorEnding()
        {
            Progress = new str_progress();

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svlnproc = new n_cst_lnproc_confirmbal();
            svlnproc.of_initservice(svCon);

            svProgress = new n_cst_progresscontrol();
            svlnproc.of_setprogress(ref svProgress);
       
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

        ~SlCfbalProcess()
        {
            DisConnect();
        }

        //--------------------------------------------------------------------------------------


        #region Running Members

        public str_progress GetProgress()
        {
            svlnproc.of_setprogress(ref svProgress);
            return svProgress.of_get_progress();
        }

        public void Run()
        {
            if (thread != null)
            {
                svlnproc.of_procconfirmbalance(ls_xmlbalcriteria, ls_entryid);
                Progressing.Remove(this.Application, this.ID); // remove list 
            }
        }

        #endregion
    }
}
