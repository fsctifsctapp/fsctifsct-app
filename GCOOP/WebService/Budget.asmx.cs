using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace WebService
{
    /// <summary>
    /// Summary description for Budget
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Budget : System.Web.Services.WebService
    {

        [WebMethod]
        public String GetMemberName(String wsPass, String membNo)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.GetMemberName(membNo);
        }

        [WebMethod]
        public int SaveBudgetYear(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetYear(xml);
        }

        [WebMethod]
        public int SaveBudgetGroup(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetGroup(xml);
        }

        [WebMethod]
        public int SaveBudgetType(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetType(xml);
        }

        [WebMethod]
        public int SaveBudgetAmount(String wsPass, short year, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetAmount(year, xml);
        }

        [WebMethod]
        public int SaveBudgetDetail(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetDetail(xml);
        }

        [WebMethod]
        public int SaveSlip(String wsPass, String xmlHead, String xmlDetail)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveSlip(xmlHead, xmlDetail);
        }

        [WebMethod]
        public int SaveBudgetGroupYear(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetGroupYear(xml);
        }

        [WebMethod]
        public int SaveBudgetTypeYear(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveBudgetTypeYear(xml);
        }

        [WebMethod]
        public String GetBgMovmentYear(String wsPass, short year, String bgGroup, String bgType)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.GetBgMovmentYear(year, bgGroup, bgType);
        }

        [WebMethod]
        public String GetBgTypeNonAccId(String wsPass)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.GetBgTypeNonAccId();
        }

        [WebMethod]
        public String ProcessCutPay(String wsPass, DateTime Date, String BranchId)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.ProcessCutPay(Date, BranchId);
        }

        [WebMethod]
        public int SaveFromCutPay(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveFromCutPay(xml);
        }

        [WebMethod]
        public String GetSetBudgetAmount(String wsPass, short year)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.GetSetBudgetAmount(year);
        }

        [WebMethod]
        public int SaveCloseMonthDetail(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveCloseMonthDetail(xml);
        }

        [WebMethod]
        public int GetYearBudget(String wsPass, DateTime date)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.GetYearBudget(date);
        }

        [WebMethod]
        public int CloseMonth(String wsPass, short year, short month)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.CloseMonth(year, month);
        }

        [WebMethod]
        public int SaveFromEditPay(String wsPass, String xml)
        {
            BudgetSvEn bg = new BudgetSvEn(wsPass);
            return bg.SaveFromEditPay(xml);
        }
    }
}
