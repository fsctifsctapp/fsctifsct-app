using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.ap_deposit
{
    public class PrintBook : WinPrintInterface
    {
        private n_cst_deposit_service dep;
        private String accountNo;
        private String branchId;
        private short seqNo;
        private short pageNo;
        private short lineNo;
        private bool isBf;
        private String printSet;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            accountNo = args[0];
            branchId = args[1];
            seqNo = Convert.ToInt16(args[2]);
            pageNo = Convert.ToInt16(args[3]);
            lineNo = Convert.ToInt16(args[4]);
            isBf = args[5].ToLower() == "true";
            printSet = args[6];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            dep = new n_cst_deposit_service();
            dep.of_settrans(svCon);
            dep.of_init();
        }

        public string Run(ref string returnWebService)
        {
            dep.of_print_book(accountNo, branchId, seqNo, pageNo, lineNo, isBf, printSet, ref returnWebService);
            return "DEP PRINTBOOK " + accountNo;
        }

        #endregion
    }
}
