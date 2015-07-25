using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pbservice;

namespace WebService
{
    public class WalfareSvEn
    {
        private Security security;
        private n_cst_dbconnectservice svCon;

        public WalfareSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public WalfareSvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                svCon = new n_cst_dbconnectservice();
                svCon.of_connectdb(security.ConnectionString);
            }
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~WalfareSvEn()
        {
            DisConnect();
        }
        //---------------------------------------------------//

    }
}