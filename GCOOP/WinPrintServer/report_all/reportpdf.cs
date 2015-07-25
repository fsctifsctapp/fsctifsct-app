using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbreport;

namespace WinPrint.report_all
{
    public class reportpdf : WinPrintInterface
    {
        private string pkApp;
        private string pkGid;
        private string pkRid;
        private string xmlCriteria;
        private string pdfFilename;
        private string connectionString;
        private string userId;

        #region WinPrintInterface Members

        public void SetArgument(string[] args)
        {
            pkApp = args[0];
            pkGid = args[1];
            pkRid = args[2];
            xmlCriteria = args[3];
            pdfFilename = args[4];
            connectionString = args[5];
            userId = args[6];
        }

        public void SetTransMaual(pbservice.n_cst_dbconnectservice svCon)
        {
        }

        public string Run(ref string returnWebService)
        {
            n_cst_dbconnectservice lnv_con = new n_cst_dbconnectservice();
            lnv_con.of_connectdb(connectionString);
            try
            {
                n_cst_reportservice lnv_print = new n_cst_reportservice();
                lnv_print.of_settrans(ref lnv_con);

                int li_return = lnv_print.of_report_print_pdf(pkApp, pkGid, pkRid, userId, xmlCriteria, pdfFilename);
                //int li_return = lnv_print.of_report_print_pdf(pkApp, pkGid, pkRid, xmlCriteria, pdfFilename);

                returnWebService = Convert.ToString(li_return);
                lnv_con.of_disconnectdb();
                //return "Create PDF Report(" + pkApp + "," + pkGid + "," + pkRid + "," + pdfFilename + ",XML:[" + xmlCriteria + "]) return " + returnWebService;
                return "PDF " + pkApp + ", " + pkGid + ", " + pkRid + " return " + li_return;
            }
            catch (Exception ex)
            {
                lnv_con.of_disconnectdb();
                throw ex;
            }
        }

        #endregion
    }
}
