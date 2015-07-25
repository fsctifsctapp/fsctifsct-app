using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint.shrlon
{
    class PrintColl : WinPrintInterface
    {
        private n_cst_loansrv_print shrlon;
        String reftype;
        String mastno;
        String formset;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            reftype = args[0];
            mastno = args[1];
            formset = args[2];
        }

        public void SetTransMaual(n_cst_dbconnectservice svCon)
        {
            shrlon = new n_cst_loansrv_print();
            shrlon.of_initservice(svCon);
        }

        public string Run(ref string returnWebService)
        {
            returnWebService = Convert.ToString(shrlon.of_print_coll(reftype, mastno, formset));
            return "ส่งข้อมูลการพิมพ์เรียบร้อย";
        }

        #endregion
    }
}
