using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;


namespace WinPrint.app_finance
{
    class RePrintTaxPay : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String as_branch;
        DateTime adtm_wdate;
        String as_slipno;
        Int16 ai_topay;
        Int16 ai_keep;
        Int16 ai_formcoop;
        String as_formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            as_branch = args[0];
            adtm_wdate = DateTime.ParseExact(args[1], "yyyy-MM-dd", new CultureInfo("en-US"));
            as_slipno = args[2];
            ai_topay = Convert.ToInt16(args[3]);
            ai_keep = Convert.ToInt16(args[4]);
            ai_formcoop = Convert.ToInt16(args[5]);
            as_formset = args[6];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postreprinttaxpay(as_branch, adtm_wdate, as_slipno, ai_topay, ai_keep, ai_formcoop, as_formset));
            return "REPRINTTAXPAY";
        }

        #endregion
    }
}
