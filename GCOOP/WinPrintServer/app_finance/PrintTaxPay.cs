using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;


namespace WinPrint.app_finance
{
    class PrintTaxPay : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        String as_slipno;
        String as_formset;


        #region WinPrintInterface Members

   
        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            as_slipno = args[1];
            as_formset = args[2];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postprinttaxpay(as_branch, as_slipno, as_formset));
            return "PRINTTAXPAY";
        }

        #endregion
    }
}
