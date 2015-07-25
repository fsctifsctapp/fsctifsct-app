using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.app_finance
{
    class PrintSlip : WinPrintInterface
    {
        private n_cst_finance_service fin;
        String slip_no;
        String branchid;
        String formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            slip_no = args[0];
            branchid = args[1];
            formset = args[2];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            fin = new n_cst_finance_service();
            fin.of_settrans(svCon);
            fin.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(fin.of_postprintslip(slip_no, branchid, formset));
            return "ส่งข้อมูลการพิมพ์เรียบร้อย";
        }

        #endregion
    }
}
