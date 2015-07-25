using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;

namespace WinPrint.app_finance
{
    class PrintCashCtrl : WinPrintInterface
    {

        private n_cst_finance_service fin;
        protected String as_branch;
        protected String as_username;
        protected String as_app;
        protected DateTime adtm_workdate;
        protected Int16 ai_seqno;
        protected String as_formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
              as_branch = args[0];
              as_username = args[1];
              as_app = args[2];
              adtm_workdate = DateTime.ParseExact(args[3], "yyyy-MM-dd", new CultureInfo("en-US"));
              ai_seqno = Convert.ToInt16( args[4] );
              as_formset = args[5];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            fin.of_postprintslipcashctrl(as_branch, as_username, as_app, adtm_workdate, ai_seqno, as_formset);
            return "FIN CashControl";
        }

        #endregion
    }
}
