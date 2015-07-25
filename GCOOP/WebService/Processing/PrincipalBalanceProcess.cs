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
    public class PrincipalBalanceProcess : MainProgress, Running
    {
        n_cst_principalbalance_new svPrincbal;
        n_cst_progresscontrol svProgress;
        n_cst_dbconnectservice svCon;
        DateTime operateDate;

        public PrincipalBalanceProcess(String connectionString, DateTime operateDate)
        {
            this.connectString = connectionString;
            this.operateDate = operateDate;

            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(this.connectString);

            svProgress = new n_cst_progresscontrol();
            Progress = svProgress.of_get_progress();  //ไม่รู้ตรงไหนใช้.ดอยบอกใช้ที่ MainProgress ดัก Exception ในนั้นเอง.

            svPrincbal = new n_cst_principalbalance_new();
            svPrincbal.of_settrans(svCon);
            svPrincbal.of_setprogress(ref svProgress);

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

        ~PrincipalBalanceProcess()
        {
            DisConnect();
        }

        #region Running Members

        public pbservice.str_progress GetProgress()
        {
            svPrincbal.of_getprogress(ref svProgress);
            return svProgress.of_get_progress();
        }

        public void Run()
        {
            if (thread != null)
            {
                svPrincbal.of_setoperatedate(operateDate);
                svPrincbal.of_start();
                //Progressing.Remove(this.Application, this.ID);
            }
        }

        #endregion
    }
}
