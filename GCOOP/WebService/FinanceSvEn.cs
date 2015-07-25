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
    public class FinanceSvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_finance_service svFin;
        private n_cst_finance_utility svFinUtl;
        private n_cst_moneyorder_service svMoney;
        private n_cst_loansrv_installment svInstallment;
        private n_cst_loansrv_lnoperate svlnoperate;
        private Security security;

        public FinanceSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public FinanceSvEn(String wsPass, bool autoConnect)
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
                svFin = new n_cst_finance_service();
                svFin.of_settrans(svCon);
                svFin.of_init();
                svFinUtl = new n_cst_finance_utility();
                svFinUtl.of_settrans(svCon);
                svMoney = new n_cst_moneyorder_service();
                //svMoney.
                svMoney.of_init(svCon);
                svInstallment = new n_cst_loansrv_installment();
                svInstallment.of_initservice(svCon);
                svlnoperate = new n_cst_loansrv_lnoperate();
                svlnoperate.of_initservice(svCon);
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

        ~FinanceSvEn()
        {
            DisConnect();
        }

        //-------------------------------------------------------------

        public String[] of_init_openday(String brabchId, String username, DateTime workDate, String clientName)
        {
            try
            {
                String openDayXml = "", errmessage = "";
                String[] returnValue = new String[2];
                int re = svFin.of_init_openday(brabchId, username, workDate, clientName, ref openDayXml, ref errmessage);
                DisConnect();
                returnValue[0] = openDayXml;
                returnValue[1] = errmessage;
                return returnValue;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_open_day(String xml)
        {
            try
            {
                int re = svFin.of_open_day(xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_close_day(String as_branch_id, String as_entry_id, DateTime adtm_wdate, String as_appname)
        {
            try
            {
                String as_closeday_xml = "", as_chqwait_xml = "";
                String[] ClsDay = new String[2];
                int re = svFin.of_init_close_day(as_branch_id, as_entry_id, adtm_wdate, as_appname, ref as_closeday_xml, ref as_chqwait_xml);
                ClsDay[0] = as_closeday_xml;
                ClsDay[1] = as_chqwait_xml;
                DisConnect();
                return ClsDay;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        public int of_close_day(String as_appname, String as_close_day_xml, String as_chqwait_xml)
        {
            try
            {
                int re = svFin.of_close_day(as_appname, as_close_day_xml, as_chqwait_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getchild_branch()
        {
            try
            {
                String oxml = "";
                svFinUtl.of_getchildbranch(ref oxml);
                DisConnect();
                return oxml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_dddwbank()
        {
            try
            {
                String xml = svFinUtl.of_dddwbank();
                DisConnect();
                return xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_dddwbankbranch(String bankcode)
        {
            try
            {
                String bXml = "";
                int re = svFinUtl.of_dddwbankbranch(bankcode, ref bXml);
                DisConnect();
                return bXml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_dddwfinitemtype()
        {
            try
            {
                String xml = svFinUtl.of_dddwfinitemtype();
                DisConnect();
                return xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_finquery(String AppName, String QueryXml)
        {
            try
            {
                String UserDetailXml = "", RecvXml = "", PayXml = "";
                String[] Xml = new String[3];
                svFin.of_finquery(AppName, QueryXml, ref UserDetailXml, ref RecvXml, ref PayXml);
                Xml[0] = UserDetailXml;
                Xml[1] = RecvXml;
                Xml[2] = PayXml;
                DisConnect();
                return Xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_fincashcontrol(String brabchId, DateTime workDate, String EnTryId)
        {
            try
            {
                String fincashctl_info = "";
                Int16 re = svFin.of_init_fincashcontrol(brabchId, workDate, EnTryId, ref fincashctl_info);
                DisConnect();
                return fincashctl_info;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_fincashcontrol_user(String fincashctluser_info)
        {
            try
            {
                String as_fullname = "";
                String[] Xml = new String[2];
                int re = svFin.of_init_fincashcontrol_user(ref fincashctluser_info, ref as_fullname);
                Xml[0] = fincashctluser_info;
                Xml[1] = as_fullname;
                DisConnect();
                return Xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_fincashcontrol_process(String finProcess, String as_machined, String AppName)
        {
            try
            {
                int re = svFin.of_fincashcontrol_process(finProcess, as_machined, AppName);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_payrecv_slip(String branchID, String entryID, String machineID, DateTime workdate)
        {
            try
            {
                String[] result = new String[2];
                String mainXml = "", itemXml = "";
                int re = svFin.of_init_payrecv_slip(branchID, entryID, machineID, workdate, ref mainXml);
                result[0] = mainXml;
                result[1] = itemXml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getchildaccid()
        {
            try
            {
                string re = svFinUtl.of_getchildfinaccid();
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getchildmoneytype()
        {
            try
            {
                string re = svFinUtl.of_getchildmoneytype();
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getchildtaxcode()
        {
            try
            {
                string re = svFinUtl.of_getchildtaxcode();
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_chq_bookno(String as_chqbook_xml)
        {
            try
            {
                int re = svFin.of_init_chq_bookno(ref as_chqbook_xml);
                DisConnect();
                return as_chqbook_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postchqmas(String as_chqbook_xml)
        {
            try
            {
                int re = svFin.of_postchqmas(as_chqbook_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_postpayrecv(String mainXml, String itemXml, String taxXml, String appName)
        {
            try
            {
                String re = svFin.of_payslip(mainXml, itemXml, taxXml, appName);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_defaultaccid(String moneytype)
        {
            try
            {
                string re = svFin.of_defaultaccid(moneytype);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_caltax(String mainXml)
        {
            try
            {
                String[] result = new String[2];
                String as_taxdet_xml = "";
                int re = svFin.of_caltax(ref mainXml, ref as_taxdet_xml);
                result[0] = mainXml;
                result[1] = as_taxdet_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_payrecv_member(String mainXml)
        {
            try
            {
                int re = svFin.of_init_payrecv_member(ref mainXml);
                DisConnect();
                return mainXml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retreive_cancleslip(String branchID, String mainXml)
        {
            try
            {
                String listXml = "";
                int re = svFin.of_retrieve_cancleslip(branchID, mainXml, ref listXml);
                DisConnect();
                return listXml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postcancleslip(String branchID, String entryID, String mainXml, String listXml)
        {
            try
            {
                int re = svFin.of_postcancelslip(branchID, entryID, mainXml, listXml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_posttobank(String branchID, String entryID, DateTime workDate, String machineID, Int16 seqNO)
        {
            try
            {
                int re = svFin.of_posttobank(branchID, entryID, workDate, machineID, seqNO);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //public int of_init_setitembank(String branchID, DateTime workDate)
        //{
        //    try
        //    {
        //        int re = svFin.of_init_setitembank(branchID, workDate );
        //        DisConnect();
        //        return re;
        //    }
        //    catch (Exception ex)
        //    {
        //        DisConnect();
        //        throw ex;
        //    }
        //}

        public int of_postcanclechq(String as_branch, DateTime adtm_wdate, String as_cancelid, String as_machine, String as_cancellist_xml)
        {
            try
            {
                int re = svFin.of_postcancelchq(as_branch, adtm_wdate, as_cancelid, as_machine, as_cancellist_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postchangedstatuschq(String as_branch, String as_entry_id, DateTime adtm_wdate, String as_machine, String as_chqlist_xml)
        {
            try
            {
                int re = svFin.of_postchangedstatuschq(as_branch, as_entry_id, adtm_wdate, as_machine, as_chqlist_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postfincontact(String as_contack_xml, String as_action)
        {
            try
            {
                int re = svFin.of_postfincontact(as_contack_xml, as_action);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_bankaccount_slip(String as_main_xml)
        {
            try
            {
                int re = svFin.of_init_bankaccount_slip(ref as_main_xml);
                DisConnect();
                return as_main_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postslipbank(String as_main_xml)
        {
            try
            {
                int re = svFin.of_postslipbank(as_main_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_chqlistfrom_slip(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String[] result = new String[4];
                String as_chqcond_xml = "", as_cutbank_xml = "", as_chqtype_xml = "", as_chqlist_xml = "";
                int re = svFin.of_init_chqlistfrom_slip(as_branch, adtm_wdate, ref as_chqcond_xml, ref as_cutbank_xml, ref as_chqtype_xml, ref as_chqlist_xml);
                result[0] = as_chqcond_xml;
                result[1] = as_cutbank_xml;
                result[2] = as_chqtype_xml;
                result[3] = as_chqlist_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_chqnoandbank(String as_bank, String as_bankbranch, String as_chqbookno, String as_branch)
        {
            try
            {
                String[] result = new String[2];
                String as_accno = "", as_startchqno = "";
                int re = svFin.of_init_chqnoandbank(as_bank, as_bankbranch, as_chqbookno, as_branch, ref as_accno, ref as_startchqno);
                result[0] = as_accno;
                result[1] = as_startchqno;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postpaychq_fromslip(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_chqcond_xml, String as_cutbank_xml, String as_chqtype_xml, String as_chqllist_xml, String as_formset)
        {
            try
            {
                int re = svFin.of_postpaychq_fromslip(as_branch, as_entry, adtm_wdate, as_machine, as_chqcond_xml, as_cutbank_xml, as_chqtype_xml, as_chqllist_xml, as_formset);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_paychq(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String as_main_xml = "", as_chqlist_xml = "";
                String[] result = new String[2];
                int re = svFin.of_init_paychq(as_branch, adtm_wdate, ref as_main_xml, ref as_chqlist_xml);
                result[0] = as_main_xml;
                result[1] = as_chqlist_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postpaychq(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_main_xml, String as_chqlist_xml, String as_formset)
        {
            try
            {
                int re = svFin.of_postpaychq(as_branch, as_entry, adtm_wdate, as_machine, as_main_xml, as_chqlist_xml, as_formset);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_paychq_split(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String[] result = new String[4];
                String as_chqcond_xml = "", as_cutbank_xml = "", as_chqtype_xml = "", as_chqlist_xml = "";
                int re = svFin.of_init_paychq_split(as_branch, adtm_wdate, ref as_chqcond_xml, ref as_cutbank_xml, ref as_chqtype_xml, ref as_chqlist_xml);
                result[0] = as_chqcond_xml;
                result[1] = as_cutbank_xml;
                result[2] = as_chqtype_xml;
                result[3] = as_chqlist_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postpaychq_split(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_cond_xml, String as_bankcut_xml, String as_chqtype_xml, String as_chqlist_xml, String as_chqspilt_xml, String as_formset)
        {
            try
            {
                int re = svFin.of_postpaychq_split(as_branch, as_entry, adtm_wdate, as_machine, as_cond_xml, as_bankcut_xml, as_chqtype_xml, as_chqlist_xml, as_chqspilt_xml, as_formset);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievecancelchq(String as_branch, String as_cond_xml)
        {
            try
            {
                String as_chqcancel_xml = "";
                int re = svFin.of_retrievecancelchq(as_branch, as_cond_xml, ref as_chqcancel_xml);
                DisConnect();
                return as_chqcancel_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievechangechqstatus(String as_branch)
        {
            try
            {
                String as_chqlist_xml = "";
                int re = svFin.of_retrievechangechqstatus(as_branch, ref as_chqlist_xml);
                DisConnect();
                return as_chqlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievechangchqdetail(String as_chqno, String as_bookno, String as_bank, String as_bankbranch, Int16 ai_seqno)
        {
            try
            {
                String as_chqdetail_xml = "";
                int re = svFin.of_retrievechangchqdetail(as_chqno, as_bookno, as_bank, as_bankbranch, ai_seqno, ref as_chqdetail_xml);
                DisConnect();
                return as_chqdetail_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_posttobank(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String as_xmlinfo = "";
                int re = svFin.of_init_posttobank(as_branch, adtm_wdate, ref as_xmlinfo);
                DisConnect();
                return as_xmlinfo;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_fin_posttobank(String as_branch, String as_entryid, DateTime adtm_wdate, String as_machine, String as_item_xml)
        {
            try
            {
                int re = svFin.of_fin_posttobank(as_branch, as_entryid, adtm_wdate, as_machine, as_item_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_fincontact(String as_contact_xml)
        {
            try
            {
                int re = svFin.of_init_fincontact(ref as_contact_xml);
                DisConnect();
                return as_contact_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_init_sendchq(String as_branch, String as_entry, DateTime adtm_wdate)
        {
            try
            {
                String as_sendchq_xml = "";
                int re = svFin.of_init_sendchq(as_branch, as_entry, adtm_wdate, ref as_sendchq_xml);
                DisConnect();
                return as_sendchq_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postsavesendchq(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_sendchq_xml, String as_waitchq_xml, String as_sendchqacc_xml, Int16 ai_accknow)
        {
            try
            {
                int re = svFin.of_postsavesendchq(as_branch, as_entry, adtm_wdate, as_machine, as_sendchq_xml, as_waitchq_xml, as_sendchqacc_xml, ai_accknow);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievecancel_sendchq(String as_branch, DateTime adtm_wdate, String as_bank_xml)
        {
            try
            {
                String as_cancellist = "";
                int re = svFin.of_retrievecancel_sendchq(as_branch, adtm_wdate, as_bank_xml, ref as_cancellist);
                DisConnect();
                return as_cancellist;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postcancel_sendchq(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_bank_xml, String as_cancellist_xml)
        {
            try
            {
                int re = svFin.of_postcancel_sendchq(as_branch, as_entry, adtm_wdate, as_machine, as_bank_xml, as_cancellist_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postcancelposttobank(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_banklist_xml)
        {
            try
            {
                int re = svFin.of_postcancelposttobank(as_branch, as_entry, adtm_wdate, as_machine, as_banklist_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postcancelsendchq(String as_branch, String as_chqno, String as_bank, String as_bankbranch, Int16 si_seqno)
        {
            try
            {
                int re = svFin.of_postcancelsendchq(as_branch, as_chqno, as_bank, as_bankbranch, si_seqno);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_postotherto_fin(String as_memb_xml, String appname)
        {
            try
            {
                String[] result = new String[3];
                String as_slipmain_xml = "", as_cancel_xml = "";
                int re = svFin.of_init_postotherto_fin(ref as_memb_xml, ref as_slipmain_xml, ref as_cancel_xml, appname);
                result[0] = as_memb_xml;
                result[1] = as_slipmain_xml;
                result[2] = as_cancel_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retreivefinslipdet(String as_slipno, String as_branch)
        {
            try
            {
                String as_slipdet_xml = "";
                int re = svFin.of_retrievefinslipdet(as_slipno, as_branch, ref as_slipdet_xml);
                DisConnect();
                return as_slipdet_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_postotherto_fin(String as_branch, String as_entry, DateTime adtm_wdate, String as_appname, String as_main_xml, String as_itemdet_xml, String as_cancelXml)
        {
            try
            {
                String re = svFin.of_postotherto_fin(as_branch, as_entry, adtm_wdate, as_appname, as_main_xml, as_itemdet_xml, as_cancelXml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievepaychqlist(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String as_chqlist_xml = "";
                int re = svFin.of_retrievepaychqlist(as_branch, adtm_wdate, ref as_chqlist_xml);
                DisConnect();
                return as_chqlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievechqfromslip(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String as_chqlist_xml = "";
                int re = svFin.of_retrievechqfromslip(as_branch, adtm_wdate, ref as_chqlist_xml);
                DisConnect();
                return as_chqlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievereprintchq(String as_branch, String as_retrieve_xml)
        {
            try
            {
                String as_chqlist_xml = "";
                int re = svFin.of_retrievereprintchq(as_branch, as_retrieve_xml, ref as_chqlist_xml);
                DisConnect();
                return as_chqlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievereprintpayslip(String as_branch, String as_cond_xml)
        {
            try
            {
                String as_slip_xml = "";
                int re = svFin.of_retrievereprintpayslip(as_branch, as_cond_xml, ref as_slip_xml);
                DisConnect();
                return as_slip_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievereprintreceipt(String as_branch, String as_cond_xml)
        {
            try
            {
                String as_list_xml = "";
                int re = svFin.of_retrievereprintreceipt(as_branch, as_cond_xml, ref as_list_xml);
                DisConnect();
                return as_list_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Decimal[] of_itemcaltax(Int16 ai_recvpay, Int16 ai_catvat, Int16 ai_taxcode, Decimal adc_itemamt)
        {
            try
            {
                Decimal[] result = new Decimal[3];
                Decimal adc_taxamt = 0, adc_itemamt_net = 0, adc_vatamt = 0;
                int re = svFin.of_itemcaltax(ai_recvpay, ai_catvat, ai_taxcode, adc_itemamt, ref adc_taxamt, ref adc_itemamt_net, ref adc_vatamt);
                result[0] = adc_taxamt;
                result[1] = adc_itemamt_net;
                result[2] = adc_vatamt;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_getaddress(String as_memberno, Int16 ai_memberflag, String as_branch)
        {
            try
            {
                String[] result = new String[2];
                String as_taxaddr = "", as_taxid = "";
                int re = svFin.of_getaddress(ref as_taxaddr, ref as_taxid, as_memberno, ai_memberflag, as_branch);
                result[0] = as_taxaddr;
                result[1] = as_taxid;
                return result;

            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievetaxpay(String as_branch, String as_main_xml)
        {
            try
            {
                String as_list_xml = "";
                int re = svFin.of_retrievetaxpay(as_branch, as_main_xml, ref as_list_xml);
                DisConnect();
                return as_list_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_retrievebankaccount(String as_branch, String as_bank_xml)
        {
            try
            {
                String[] result = new String[2];
                String as_bankstm_xml = "";
                int re = svFin.of_retrievebankaccount(as_branch, ref as_bank_xml, ref as_bankstm_xml);
                result[0] = as_bank_xml;
                result[1] = as_bankstm_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postupdatebankaccount(String as_bank_xml)
        {
            try
            {
                int re = svFin.of_postupdatebankaccount(as_bank_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievelistformatchq(String as_branch)
        {
            try
            {
                String as_formlist_xml = "";
                //int re = svFin.of_retrievelistformatchq(as_branch, ref as_formlist_xml);
                DisConnect();
                return as_formlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrieveformatchq(String as_branch, String as_bankcode, Int16 ai_chqtype, Int16 ai_printtype)
        {
            try
            {
                String as_format_xml = "";
                //int re = svFin.of_retrieveformatchq(as_branch, as_bankcode, ai_chqtype, ai_printtype, ref as_format_xml);
                DisConnect();
                return as_format_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postprintpreviewchq(String as_branch, String as_bank, Int16 ai_chqsize, String as_printtype, String as_formset)
        {
            try
            {
                //int re = svFin.of_postprintpreviewchq(as_branch, as_bank, ai_chqsize, as_printtype, as_formset);
                DisConnect();
                return 1;//re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_retrievereceivechq(String as_branch)
        {
            try
            {
                String as_chqlist_xml = "";
                int re = svFin.of_retrievereceivechq(as_branch, ref as_chqlist_xml);
                DisConnect();
                return as_chqlist_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Int16 of_postreceivechq(String as_entryid, String as_machineid, DateTime as_wdate, String as_chqlist_xml)
        {
            try
            {
                Int16 re = svFin.of_postreceivechq(as_entryid, as_machineid, as_wdate, as_chqlist_xml);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_chqfrom_apvloan(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String[] result = new String[4];
                String as_chqcond_xml = "", as_cutbank_xml = "", as_chqtype_xml = "", as_chqlist_xml = "";
                Int16 re = svFin.of_init_chqfrom_apvloan(as_branch, adtm_wdate, ref as_chqcond_xml, ref as_cutbank_xml, ref as_chqtype_xml, ref as_chqlist_xml);
                result[0] = as_chqcond_xml;
                result[1] = as_cutbank_xml;
                result[2] = as_chqtype_xml;
                result[3] = as_chqlist_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_init_paychq_apvloancbt(String as_branch, DateTime adtm_wdate)
        {
            try
            {
                String[] result = new String[2];
                String as_main_xml = "", as_chqlist_xml = "";
                Int16 re = svFin.of_init_paychq_apvloancbt(as_branch, adtm_wdate, ref as_main_xml, ref as_chqlist_xml);
                result[0] = as_main_xml;
                result[1] = as_chqlist_xml;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Int16 of_postpaychq_fromapvloan(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_cond_xml, String as_cutbank_xml, String as_chqtype_xml, String as_chqlist_xml, String as_formset)
        {
            try
            {
                Int16 re = svFin.of_postpaychq_fromapvloan(as_branch, as_entry, adtm_wdate, as_machine, as_cond_xml, as_cutbank_xml, as_chqtype_xml, as_chqlist_xml, as_formset);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public Int16 of_postpaychq_apvloancbt(String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_main_xml, String as_chqlist_xml, String as_formset)
        {
            try
            {
                Int16 re = svFin.of_postpaychq_apvloancbt(as_branch, as_entry, adtm_wdate, as_machine, as_main_xml, as_chqlist_xml, as_formset);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_postprocessotherto_fin(String as_branchid, String as_entry_id, DateTime adtm_wdate, String as_machineid)
        {
            try
            {
                int re = svFin.of_postprocessotherto_fin(as_branchid, as_entry_id, adtm_wdate, as_machineid);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //bask--------------
        public String of_init_moneyorder(String xmlMain, String entryId, DateTime workDate)
        {
            try
            {
                String result = svMoney.of_init_moneyorder(xmlMain, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_save_moneyorder(String xmlMain, String xmlDetail, String entryId, DateTime workDate)
        {
            try
            {
                int result = svMoney.of_save_moneyorder(xmlMain, xmlDetail, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_operate_moneyorder(String ptbDocNo, String ptbTypeCode, int seqNo, String referDocNo, short statusFlag, String entryId, DateTime workDate)
        {
            try
            {
                int result = svMoney.of_operate_moneyorder(ptbDocNo, ptbTypeCode, seqNo, referDocNo, statusFlag, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getlist_moneyorder(DateTime workDate)
        {
            try
            {
                String result = svMoney.of_getlist_moneyorder(workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] of_getdata_moneyorder(String docNo)
        {
            String xmlMaster = "";
            String xmlDetail = "";
            try
            {
                String[] result = new String[2];
                int re = svMoney.of_getdata_moneyorder(docNo, ref xmlMaster, ref xmlDetail);
                result[0] = xmlMaster;
                result[1] = xmlDetail;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getlistappr_moneyorder(DateTime workDate)
        {
            try
            {
                String result = svMoney.of_getlistappr_moneyorder(workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getlistcancel_moneyorder(DateTime workDate)
        {
            try
            {
                String result = svMoney.of_getlistcancel_moneyorder(workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_getlistreappr_moneyorder(DateTime workDate)
        {
            try
            {
                String result = svMoney.of_getlistreappr_moneyorder(workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_approve_moneyorder(String xmlList, String entryId, DateTime workDate)
        {
            try
            {
                int result = svMoney.of_approve_moneyorder(xmlList, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_cancel_moneyorder(String xmlList, String entryId, DateTime workDate)
        {
            try
            {
                int result = svMoney.of_cancel_moneyorder(xmlList, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_cancelappr_moneyorder(String xmlList, String entryId, DateTime workDate)
        {
            try
            {
                int result = svMoney.of_cancelappr_moneyorder(xmlList, entryId, workDate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //-------------bask

        // a 
        public decimal of_recal_installment(String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref Decimal adc_periodpay)
        {

            try
            {
                decimal result = svInstallment.of_recal_installment(as_contno, adtm_paydate, adc_prnbal, ref adc_periodpay);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        // a 
        public decimal of_recal_periodpay(String as_contno, DateTime adtm_paydate, decimal adc_prnbal, ref short ai_installment)
        {

            try
            {
                decimal result = svInstallment.of_recal_periodpay(as_contno, adtm_paydate, adc_prnbal, ref ai_installment);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        // a 
        public decimal of_recal_installmentmanual(String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref Decimal adc_paymentfixed)
        {

            try
            {
                decimal result = svInstallment.of_recal_installmentmanual(as_contno, adtm_paydate, adc_prnbal, ref adc_paymentfixed);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        // a 
        public decimal of_recal_periodpaymanual(String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref  short ai_installfixed)
        {

            try
            {
                decimal result = svInstallment.of_recal_periodpaymanual(as_contno, adtm_paydate, adc_prnbal, ref ai_installfixed);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        // A   สร้างแผ่านส่งธนาคาร
        public String of_initlist_lnrcvdisk(String as_lntype_start, String as_lntype_end, DateTime adtm_reqdatre_start, DateTime adtm_reqdate_end)
        {
            try
            {
                String result_xml = svlnoperate.of_initlist_lnrcvdisk(as_lntype_start, as_lntype_end, adtm_reqdatre_start, adtm_reqdate_end);
                DisConnect();
                return result_xml;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }


        }

        // A   Save สร้างแผ่านส่งธนาคาร
        public int of_save_lnrcvdisk(str_lnrcvdisk as_str_lncvdisk)
        {
            try
            {

                int result = svlnoperate.of_save_lnrcvdisk(as_str_lncvdisk);
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