using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GcoopServiceCs;

namespace WebService
{
    public class ShrlonDotNetSvEnCs
    {
        private ShrlonService shrlon;
        private Security security;

        public ShrlonDotNetSvEnCs(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public ShrlonDotNetSvEnCs(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                shrlon = new ShrlonService(security.ConnectionString);
            }
        }

        public void DisConnect()
        {
            try
            {
            }
            catch { }
        }

        ~ShrlonDotNetSvEnCs()
        {
            DisConnect();
        }

        private String PblFullPath(String pbl, String application)
        {
            return security.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + pbl;
        }

        public StructLoanRequest InitMemberNo(String pbl, String application, String memberNo, String xmlDwMain)
        {
            return shrlon.InitMemberNo(this.PblFullPath(pbl, application), memberNo, xmlDwMain);
        }
    }
}