using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pbservice;

namespace WinPrint
{
    public interface WinPrintInterface
    {
        void SetArgument(string[] args);
        void SetTransMaual(n_cst_dbconnectservice svCon);
        String Run(ref String returnWebService);
    }
}