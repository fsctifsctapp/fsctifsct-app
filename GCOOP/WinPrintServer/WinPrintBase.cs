using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPrint
{
    public class WinPrintBase
    {
        public pbservice.n_cst_dbconnectservice svCon;

        public void Connect(String connectionString)
        {
            svCon = new pbservice.n_cst_dbconnectservice();
            svCon.of_connectdb(connectionString);
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }
    }
}
