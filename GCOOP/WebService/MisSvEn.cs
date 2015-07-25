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
using System.IO;
using System.Globalization;
using DBAccess;
using pbservice;

namespace WebService
{
    public class MisSvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_mis_service svMis;
        private Security security;

        public MisSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public MisSvEn(String wsPass, bool autoConnect)
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
                svMis = new n_cst_mis_service();
                svMis.of_settrans(ref svCon);
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

        ~MisSvEn()
        {
            DisConnect();
        }



        public String[] InitGphBalanceLoanMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_loan_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceLoanQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_loan_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceLoanHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_loan_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceLoanYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_loan_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphBalanceShareMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_share_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceShareQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_share_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceShareHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_share_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphBalanceShareYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_balance_share_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphNewMemberMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_newmember_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphNewMemberQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_newmember_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphNewMemberHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_newmember_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphNewMemberYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_newmember_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayInDeptMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_dept_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInDeptQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_dept_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInDeptHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_dept_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInDeptYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_dept_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayInLoanMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_loan_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInLoanQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_loan_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInLoanHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_loan_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInLoanYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_loan_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayInShareMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_share_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInShareQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_share_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInShareHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_share_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayInShareYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payin_share_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayOutDeptMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_dept_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutDeptQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_dept_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutDeptHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_dept_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutDeptYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_dept_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayOutLoanMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_loan_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutLoanQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_loan_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutLoanHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_loan_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutLoanYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_loan_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String[] InitGphPayOutShareMonth(Int16 startMonth, Int16 startYear, Int16 endMonth, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_share_month(startMonth, startYear, endMonth, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutShareQuar(Int16 startQuar, Int16 startYear, Int16 endQuar, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_share_quarter(startQuar, startYear, endQuar, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutShareHalf(Int16 startHalf, Int16 startYear, Int16 endHalf, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_share_half(startHalf, startYear, endHalf, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }

        public String[] InitGphPayOutShareYear(Int16 startYear, Int16 endYear)
        {
            String[] result = new String[2];
            String gphXml = "", tableXml = "";
            int re = svMis.of_gen_gph_payout_share_year(startYear, endYear, ref gphXml, ref tableXml);
            result[0] = gphXml;
            result[1] = tableXml;
            DisConnect();
            return result;
        }



        public String RatioSaveVariables(String as_savexml)
        {
            Int16 re = svMis.of_ratio_save_variables(as_savexml);
            return "true";
        }

        public String RatioSaveRatios(String as_savexml)
        {
            Int16 re = svMis.of_ratio_save_ratios(as_savexml);
            return "true";
        }

        public String RatioSaveOperands(String as_savexml)
        {
            Int16 re = svMis.of_ratio_save_operands(as_savexml);
            return "true";
        }

        public String RatioSaveGroups(String as_savexml)
        {
            Int16 re = svMis.of_ratio_save_groups(as_savexml);
            return "true";
        }



        public String RatioGetVariables()
        {
            String GetVar;
            String as_returnxml = "";
            Int16 re = svMis.of_ratio_get_variables(ref as_returnxml);
            GetVar = as_returnxml;
            return GetVar;
        }

        public String RatioGetRatios()
        {
            String GetRatio;
            String as_returnxml = "";
            Int16 re = svMis.of_ratio_get_ratios(ref as_returnxml);
            GetRatio = as_returnxml;
            return GetRatio;
        }

        public String RatioGetRatiosGroups(int al_groupid)
        {
            String GetRatioGroup;
            String as_returnxml = "";
            Int16 re = svMis.of_ratio_get_ratios(al_groupid, ref as_returnxml);
            GetRatioGroup = as_returnxml;
            return GetRatioGroup;
        }

        public String RatioGetOperands(int al_ratioid)
        {
            String GetOperand;
            String as_returnxml = "";
            Int16 re = svMis.of_ratio_get_operands(al_ratioid, ref as_returnxml);
            GetOperand = as_returnxml;
            return GetOperand;
        }

        public String RatioGetGroups()
        {
            String GetGroups;
            String as_returnxml = "";
            Int16 re = svMis.of_ratio_get_groups(ref as_returnxml);
            GetGroups = as_returnxml;
            return GetGroups;
        }

        public String RatioChangeGroups(int al_ratioid, int al_newgroupid)
        {
            Int16 re = svMis.of_ratio_change_group(al_ratioid, al_newgroupid);
            return "return";
        }

        public String GetMoneyHead()
        {
            String GetHead;
            String as_sheetheadxml = "";
            Int16 re = svMis.of_ratio_get_accmoneysheethead(ref as_sheetheadxml);
            GetHead = as_sheetheadxml;
            return GetHead;
        }

        public String GetMoneyDet(String as_sheetcode)
        {
            String GetDet;
            String as_sheetheadxml = "";
            Int16 re = svMis.of_ratio_get_accmoneysheetdet(as_sheetcode, ref as_sheetheadxml);
            GetDet = as_sheetheadxml;
            return GetDet;
        }

    }
}
