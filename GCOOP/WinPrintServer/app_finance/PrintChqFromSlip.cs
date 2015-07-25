using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;

namespace WinPrint.app_finance
{
    public class PrintChqFromSlip : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        String as_entry;
        DateTime adtm_wdate;
        String as_machine;
        String as_chqcond_xml;
        String as_cutbank_xml;
        String as_chqtype_xml;
        String as_chqllist_xml;
        String as_formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            as_entry = args[1];
            adtm_wdate = DateTime.ParseExact(args[2], "yyyy-MM-dd", new CultureInfo("en-US"));
            as_machine = args[3];
            as_chqcond_xml = args[4];
            as_cutbank_xml = args[5];
            as_chqtype_xml = args[6];
            as_chqllist_xml = args[7];
            as_formset = args[8];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postpaychq_fromslip(as_branch, as_entry, adtm_wdate, as_machine, as_chqcond_xml, as_cutbank_xml, as_chqtype_xml, as_chqllist_xml, as_formset));
            return "FIN PRINTCHQFROMSLIP ";
        }

        #endregion
    }
}
