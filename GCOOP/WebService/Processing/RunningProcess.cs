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
using System.Threading;
using pbservice;

namespace WebService.Processing
{
    public class RunningProcess
    {
        
        private String w_sheet_id;
        private String application;
        private String xml;

        private str_progress progress;
        private n_cst_progresscontrol svProgress;
        private n_cst_keeping_process svKeeping;
        private n_cst_dbconnectservice svCon;
        private Thread thread;
        private Security security;

        public String W_Sheet_ID
        {
            get { return w_sheet_id; }
            set { w_sheet_id = value; }
        }

        public String Application
        {
            get { return application; }
            set { application = value; }
        }

        public RunningProcess()
        {
            security = new Security("1234", false);
            svCon = new n_cst_dbconnectservice();
            svCon.of_connectdb(security.ConnectionString);

            progress = new str_progress();
            svKeeping = new n_cst_keeping_process();
            svKeeping.of_initservice(svCon);
            svProgress = new n_cst_progresscontrol();
            svKeeping.of_setprogress(ref svProgress);

        }

        public str_progress GetStatus()
        {
            svKeeping.of_setprogress(ref svProgress);
            return svProgress.of_get_progress(); //this.progress;
        }

        public void Start(String id, String application, String xml)
        {
            this.w_sheet_id = id;
            this.application = application;
            this.xml = xml;
            this.xml = "<?xml version=\"1.0\" encoding=\"UTF-16LE\" standalone=\"no\"?>" + @"

<d_kp_keep_option><d_kp_keep_option_row><receipt_tdate></receipt_tdate><calint_tdate></calint_tdate><receive_year>2553</receive_year><receive_month>9</receive_month><seq_no></seq_no><proc_type></proc_type><operate_date></operate_date><receipt_date>2010-09-30 00:00:00</receipt_date><calint_date>2010-09-30 00:00:00</calint_date><item_date></item_date><postmaster_status></postmaster_status><share_status>1</share_status><loan_status>0</loan_status><deposit_status>0</deposit_status><ffee_status>0</ffee_status><notify_status>0</notify_status><moneyret_status>0</moneyret_status><other_status>0</other_status><recpno_status>0</recpno_status><recpno_docno>0000</recpno_docno><proc_status>1</proc_status><group_text></group_text><mem_text></mem_text><sks_status></sks_status><prakan_status></prakan_status><report_status></report_status><insurefire_status></insurefire_status><tofrom_accid></tofrom_accid><moneytype_code>CHQ</moneytype_code><emp_type></emp_type><entry_id>entry_id</entry_id><branch_id>000</branch_id></d_kp_keep_option_row></d_kp_keep_option>";
            thread = new Thread(new ThreadStart(Running));
            thread.Start();

        }

        public void Stop()
        {
            thread = null;
        }

        private void Running()
        {
            if (thread != null)
            {
                svKeeping.of_rcvprocess(xml);
            }
        }


    }
}
