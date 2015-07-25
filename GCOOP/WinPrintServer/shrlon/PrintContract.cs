using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.shrlon
{
    public class PrintContract: WinPrintInterface
    {
        private String reqNo;
        private String refType;
        private String printSet;
        private n_cst_loansrv_print svPrint;

        public void SetArgument(string[] args)
        {
            reqNo = args[0];
            refType = args[1];
            printSet = args[2];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            svPrint = new n_cst_loansrv_print();
            svPrint.of_initservice(svCon);
        }

        public string Run(ref string returnWebService)
        {
            svPrint.of_print_contract(reqNo, refType, printSet);
            returnWebService = "1";
            return "PrintContract " + refType + ": " + reqNo;
        }
    }
}
