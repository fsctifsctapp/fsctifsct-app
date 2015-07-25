using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.ap_deposit
{
    public class PrintSlip : WinPrintInterface
    {
        private n_cst_deposit_service dep;
        private String slipNo;
        private String branchId;
        private String printSet;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            slipNo = args[0];
            branchId = args[1];
            printSet = args[2];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            dep = new n_cst_deposit_service();
            dep.of_settrans(svCon);
        }

        public String Run(ref String monitorResult)
        {
            monitorResult = "DEP Slip " + slipNo;
            dep.of_print_slip(slipNo, branchId, printSet);
            return "true";
        }

        #endregion
    }
}
