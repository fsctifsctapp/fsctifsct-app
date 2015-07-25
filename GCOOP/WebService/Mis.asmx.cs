using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using pbservice;

namespace WebService
{
    /// <summary>
    /// Summary description for Mis
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Servicwe to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Mis : System.Web.Services.WebService
    {
        [WebMethod]
        public String[] InitGphBalanceLoanMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceLoanMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceLoanQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceLoanQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceLoanHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceLoanHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceLoanYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceLoanYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphBalanceShareMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceShareMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceShareQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceShareQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceShareHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceShareHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphBalanceShareYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphBalanceShareYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphNewMemberMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphNewMemberMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphNewMemberQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphNewMemberQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphNewMemberHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphNewMemberHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphNewMemberYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphNewMemberYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayInDeptMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInDeptMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInDeptQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInDeptQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInDeptHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInDeptHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInDeptYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInDeptYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayInLoanMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInLoanMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInLoanQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInLoanQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInLoanHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInLoanHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInLoanYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInLoanYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayInShareMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInShareMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInShareQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInShareQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInShareHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInShareHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayInShareYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayInShareYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayOutDeptMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutDeptMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutDeptQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutDeptQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutDeptHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutDeptHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutDeptYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutDeptYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayOutLoanMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutLoanMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutLoanQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutLoanQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutLoanHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutLoanHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutLoanYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutLoanYear(startYear, endYear);
        }



        [WebMethod]
        public String[] InitGphPayOutShareMonth(String wsPass, Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutShareMonth(startMonth, startYear, endMonth, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutShareQuar(String wsPass, Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutShareQuar(startQuar, startYear, endQuar, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutShareHalf(String wsPass, Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutShareHalf(startHalf, startYear, endHalf, endYear);
        }

        [WebMethod]
        public String[] InitGphPayOutShareYear(String wsPass, Int16 startYear, Int16 endYear)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.InitGphPayOutShareYear(startYear, endYear);
        }




        [WebMethod]
        public String RatioSaveVariables(String wsPass, String as_savexml)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioSaveVariables(as_savexml);     
        }

        [WebMethod]
        public String RatioSaveRatios(String wsPass, String as_savexml)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioSaveRatios(as_savexml);          
        }

        [WebMethod]
        public String RatioSaveOperands(String wsPass, String as_savexml)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioSaveOperands(as_savexml);          
        }

        [WebMethod]
        public String RatioSaveGroups(String wsPass, String as_savexml)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioSaveGroups(as_savexml);           
        }




        [WebMethod]
        public String RatioGetVariables(String wsPass)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioGetVariables();           
        }

        [WebMethod]
        public String RatioGetRatios(String wsPass)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioGetRatios(); 
        }

        [WebMethod]
        public String RatioGetRatiosGroups(String wsPass, int al_groupid)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioGetRatiosGroups(al_groupid); 
        }

        [WebMethod]
        public String RatioGetOperands(String wsPass, int al_ratioid)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioGetOperands(al_ratioid);
        }

        [WebMethod]
        public String RatioGetGroups(String wsPass)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioGetGroups();
        }

        [WebMethod]
        public String RatioChangeGroups(String wsPass, int al_ratioid, int al_newgroupid)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.RatioChangeGroups(al_ratioid, al_newgroupid);
        }

        [WebMethod]
        public String GetMoneyHead(String wsPass)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.GetMoneyHead(); 
        }

        [WebMethod]
        public String GetMoneyDet(String wsPass, String as_sheetcode)
        {
            MisSvEn mis = new MisSvEn(wsPass);
            return mis.GetMoneyDet(as_sheetcode);
        }
    }
}
