using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;
using System.Globalization;

namespace WinPrint.ap_deposit
{
    class PrintBookFirstPage : WinPrintInterface
    {
        private n_cst_deposit_service dep;
        private String deptAccountNo, branch_id, entryId, bookNo, reson, apvId, printSet;
        private short normFlag, reprint;
        private DateTime workDate;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            try
            {
                deptAccountNo = args[0];
                branch_id = args[1];
                entryId = args[2];
                bookNo = args[3];
                reson = args[4];
                apvId = args[5];
                printSet = args[6];
                normFlag = Convert.ToInt16(args[7]);
                workDate = DateTime.ParseExact(args[8], "yyyy-MM-dd", new CultureInfo("en-US"));// Convert.ToDateTime(args[8]);
                reprint = Convert.ToInt16(args[9]);
            }
            catch { }
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
            dep = new n_cst_deposit_service();
            dep.of_settrans(svCon);
            dep.of_init();
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = dep.of_print_book_firstpage(deptAccountNo, branch_id, workDate, entryId, bookNo, reson, apvId, normFlag, printSet, reprint) + "";
            return "DEP Print first page account " + deptAccountNo;
        }

        #endregion
    }
}
