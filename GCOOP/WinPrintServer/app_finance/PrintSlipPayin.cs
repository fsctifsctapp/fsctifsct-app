using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.app_finance
{
    public class PrintSlipPayin : WinPrintInterface
    {
        private string slip_no;
        private string printset;
        private n_cst_loansrv_print svLoanPrint;

        public void SetArgument(string[] args)
        {
            slip_no = args[0];
            printset = args[1];
            //throw new NotImplementedException();
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            svLoanPrint = new n_cst_loansrv_print();
            svLoanPrint.of_initservice(svCon);
            //throw new NotImplementedException();
        }

        public string Run(ref string returnWebService)
        {
            svLoanPrint.of_print_slippayin(slip_no, printset);
            returnWebService = "1";
            return "SlipPayin " + slip_no;
            //throw new NotImplementedException();
        }
    }
}
