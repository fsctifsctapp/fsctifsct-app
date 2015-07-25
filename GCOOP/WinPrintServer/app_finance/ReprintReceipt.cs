using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;

namespace WinPrint.app_finance
{
    class ReprintReceipt : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        DateTime adtm_wdate;
        String as_list_xml;
        String as_formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            adtm_wdate = DateTime.ParseExact(args[1], "yyyy-MM-dd", new CultureInfo("en-US"));
            as_list_xml = args[2];
            as_formset = args[3];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            String as_message = "";
            returnWebService = Convert.ToString(fin.of_postreprintreceipt(as_branch, adtm_wdate, as_list_xml, as_formset, ref as_message));
            return "REPRINTRECEIPT";
        }

        #endregion
    }
}
