using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;


namespace WinPrint.app_finance
{
    public class ReprintChq : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        String as_entry;
        DateTime adtm_wdate;
        String as_machine;
        String as_formset;
        String as_cond_xml;
        String as_retrieve_xml;
        String as_chqlist_mal;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            as_entry = args[1];
            adtm_wdate = DateTime.ParseExact(args[2], "yyyy-MM-dd", new CultureInfo("en-US"));
            as_machine = args[3];
            as_formset = args[4];
            as_cond_xml = args[5];
            as_retrieve_xml = args[6];
            as_chqlist_mal = args[7];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postreprintchq(as_branch, as_entry, adtm_wdate, as_machine, as_formset, as_cond_xml, as_retrieve_xml, as_chqlist_mal));
            return "REPRINTCHQ";
        }

        #endregion
    }
}
