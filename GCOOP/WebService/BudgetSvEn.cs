using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using pbservice;

namespace WebService
{
    public class BudgetSvEn
    {
        private Security security;
        private n_cst_dbconnectservice svCon;
        private n_cst_mb_memb_service svMemb;
        private n_cst_budget_service svBud;

        public BudgetSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public BudgetSvEn(String wsPass, bool autoConnect)
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
                svMemb = new n_cst_mb_memb_service();
                svMemb.of_initservice(svCon);
                svBud = new n_cst_budget_service();
                svBud.of_init(svCon);
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

        ~BudgetSvEn()
        {
            DisConnect();
        }
        //---------------------------------------------------//

        public String GetMemberName(String membNo)
        {
            try
            {
                String memberName = svMemb.of_getmembername(membNo);
                DisConnect();
                return memberName;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetYear(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_year(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetGroup(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_group(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetType(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_type(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetAmount(short year, String xml)
        {
            try
            {
                int result = svBud.of_save_budget_amount(year, xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetDetail(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_detail(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveSlip(String xmlHead, String xmlDetail)
        {
            try
            {
                int result = svBud.of_save_slip(xmlHead,xmlDetail);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetGroupYear(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_groupyear(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveBudgetTypeYear(String xml)
        {
            try
            {
                int result = svBud.of_save_budget_typeyear(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetBgMovmentYear(short year, String bgGroup, String bgType)
        {
            try
            {
                String result = svBud.of_get_bg_movment_year(year, bgGroup, bgType);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetBgTypeNonAccId()
        {
            try
            {
                String result = svBud.of_get_bgtype_nonaccid();
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String ProcessCutPay(DateTime Date, String BranchId)
        {
            try
            {
                String result = svBud.of_process_cutpay(Date, BranchId);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveFromCutPay(String xml)
        {
            try
            {
                int result = svBud.of_save_fromcut_pay(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetSetBudgetAmount(short year)
        {
            try
            {
                String result = svBud.of_get_setbudget_amount(year);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveCloseMonthDetail(String xml)
        {
            try
            {
                int result = svBud.of_save_closemonth_detail(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int GetYearBudget(DateTime date)
        {
            try
            {
                int result = svBud.of_get_year_budget(date);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int CloseMonth(short year, short month)
        {
            try
            {
                int result = svBud.of_close_month(year, month);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        public int SaveFromEditPay(String xml)
        {
            try
            {
                int result = svBud.of_save_fromedit_pay(xml);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
    }
}
