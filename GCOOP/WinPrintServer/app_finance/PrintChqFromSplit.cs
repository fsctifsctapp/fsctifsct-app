using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;

namespace WinPrint.app_finance
{
    class PrintChqFromSplit : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        String as_entry;
        DateTime adtm_wdate;
        String as_machine;
        String as_cond_xml;
        String as_bankcut_xml;
        String as_chqtype_xml;
        String as_chqlist_xml;
        String as_chqspilt_xml;
        String as_formset;


        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            as_entry = args[1];
            adtm_wdate = DateTime.ParseExact(args[2], "yyyy-MM-dd", new CultureInfo("en-US"));
            as_machine = args[3];
            as_cond_xml = args[4];
            as_bankcut_xml = args[5];
            as_chqtype_xml = args[6];
            as_chqlist_xml = args[7];
            as_chqspilt_xml = args[8];
            as_formset = args[9];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postpaychq_split(as_branch, as_entry, adtm_wdate, as_machine, as_cond_xml, as_bankcut_xml, as_chqtype_xml, as_chqlist_xml, as_chqspilt_xml, as_formset));
            return "FIN PRINTCHQFROMSPLIT";
        }

        #endregion
    }
}
