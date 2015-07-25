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
using System.Globalization;

namespace WebService
{
    /// <summary>
    /// Summary description for Finance
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Finance : System.Web.Services.WebService
    {
        [WebMethod]
        public String[] InitOpenDay(String wsPass, String branch_id, String entry_id, DateTime workdate, String machine)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_openday(branch_id, entry_id, workdate, machine);
        }

        [WebMethod]
        public String GetChildBranch(String wsPass)
        {
            String return_branch;
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return_branch = fin.of_getchild_branch();
            return return_branch;
        }

        [WebMethod]
        public String GetBank(String wsPass)
        {
            String return_ItemType;
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return_ItemType = fin.of_dddwbank();
            return return_ItemType;
        }

        [WebMethod]
        public String GetItemType(String wsPass)
        {
            String return_ItemType;
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return_ItemType = fin.of_dddwfinitemtype();
            return return_ItemType;
        }

        [WebMethod]
        public int OpenDay(String wsPass, String openday_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            int result;
            result = fin.of_open_day(openday_xml);
            return result;
        }

        [WebMethod]
        public int CloseDay(String wsPass, String as_appname, String as_close_day_xml, String as_chqwait_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            int result;
            result = fin.of_close_day(as_appname, as_close_day_xml, as_chqwait_xml);
            return result;
        }

        [WebMethod]
        public String[] FinQuery(String wsPass, String AppName, String UserXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_finquery(AppName, UserXml);
        }

        [WebMethod]
        public String OfInitFinCashControl(String wsPass, String brabchId, DateTime workDate, String EnTryId)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_fincashcontrol(brabchId, workDate, EnTryId);
        }

        [WebMethod]
        public String[] OfInitFinCashControlUser(String wsPass, String fincashctluser_info)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_fincashcontrol_user(fincashctluser_info);
        }

        [WebMethod]
        public int OfInitFinCashControlProcess(String wsPass, String finProcess, String as_machined, String AppName)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_fincashcontrol_process(finProcess, as_machined, AppName);
        }

        [WebMethod]
        public String[] OfInitCloseDay(String wsPass, String as_branch_id, String as_entry_id, DateTime adtm_wdate, String as_appname)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_close_day(as_branch_id, as_entry_id, adtm_wdate, as_appname);
        }

        [WebMethod]
        public String[] OfInitPayRecvSlip(String wsPass, String branchID, String entryID, String machineID, DateTime workdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_payrecv_slip(branchID, entryID, machineID, workdate);
        }

        [WebMethod]
        public String OfGetChildAccid(String wsPass)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getchildaccid();
        }

        [WebMethod]
        public String OfGetChildMoneytype(String wsPass)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getchildmoneytype();
        }

        [WebMethod]
        public String OfGetChildTaxcode(String wsPass)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getchildtaxcode();
        }

        [WebMethod]
        public String OfGetChildBankbranch(String wsPass, String bankcode)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_dddwbankbranch(bankcode);
        }

        [WebMethod]
        public String OfInitChqBookNo(String wsPass, String as_chqbook_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_chq_bookno(as_chqbook_xml);
        }

        [WebMethod]
        public int OfPostChqMas(String wsPass, String as_chqbook_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postchqmas(as_chqbook_xml);
        }

        [WebMethod]
        public String OfPostPayRecv(String wsPass, String mainXml, String itemXml, String taxXml, String appName)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postpayrecv(mainXml, itemXml, taxXml, appName);
        }

        [WebMethod]
        public String DefaultAccId(String wsPass, String moneytype)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_defaultaccid(moneytype);
        }

        [WebMethod]
        public String[] OfCalTax(String wsPass, String mainXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_caltax(mainXml);

        }

        [WebMethod]
        public String OfInitPayRecvMember(String wsPass, String mainXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_payrecv_member(mainXml);
        }

        [WebMethod]
        public String OfRetreiveCancleSlip(String wsPass, String branchID, String mainXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retreive_cancleslip(branchID, mainXml);
        }

        [WebMethod]
        public int OfPostCancleSlip(String wsPass, String branchID, String entryID, String mainXml, String listXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postcancleslip(branchID, entryID, mainXml, listXml);
        }

        [WebMethod]
        public int OfPostToBank(String wsPass, String branchID, String entryID, DateTime workDate, String machineID, Int16 seqNO)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_posttobank(branchID, entryID, workDate, machineID, seqNO);
        }

        //[WebMethod]
        //public int OfInitSetItemBank(String wsPass, String branchID, DateTime workDate)
        //{
        //    FinanceSvEn fin = new FinanceSvEn(wsPass);
        //    return fin.of_init_setitembank(branchID, workDate);
        //}

        [WebMethod]
        public int OfPostCancelChq(String wsPass, String as_branch, DateTime adtm_wdate, String as_cancelid, String as_machine, String as_cancellist_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postcanclechq(as_branch, adtm_wdate, as_cancelid, as_machine, as_cancellist_xml);
        }

        [WebMethod]
        public int OfPostChangeStatusChq(String wsPass, String as_branch, String as_entry_id, DateTime adtm_wdate, String as_machine, String as_chqlist_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postchangedstatuschq(as_branch, as_entry_id, adtm_wdate, as_machine, as_chqlist_xml);
        }

        [WebMethod]
        public int OfPostFinContact(String wsPass, String as_contack_xml, String as_action)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postfincontact(as_contack_xml, as_action);
        }

        [WebMethod]
        public String OfInitBankAccountSlip(String wsPass, String as_main_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_bankaccount_slip(as_main_xml);
        }

        [WebMethod]
        public int OfPostSlipBank(String wsPass, String as_main_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postslipbank(as_main_xml);
        }

        [WebMethod]
        public String[] OfInitChqListFromSlip(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_chqlistfrom_slip(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String[] OfInitChqNoAndBank(String wsPass, String as_bank, String as_bankbranch, String as_chqbookno, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_chqnoandbank(as_bank, as_bankbranch, as_chqbookno, as_branch);
        }

        [WebMethod]
        public String OfPostPayChqFromSlip(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_chqcond_xml, String as_cutbank_xml, String as_chqtype_xml, String as_chqllist_xml, String as_formset)
        {
            String[] arg = new String[9];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_chqcond_xml;
            arg[5] = as_cutbank_xml;
            arg[6] = as_chqtype_xml;
            arg[7] = as_chqllist_xml;
            arg[8] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintChqFromSlip", arg);
            //FinanceSvEn fin = new FinanceSvEn(wsPass);
            //return fin.of_postpaychq_fromslip(as_branch, as_entry, adtm_wdate, as_machine, as_chqcond_xml, as_cutbank_xml, as_chqtype_xml, as_chqllist_xml, as_formset);

        }

        [WebMethod]
        public String[] OfInitPayChq(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_paychq(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String OfPostPayChq(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_main_xml, String as_chqlist_xml, String as_formset)
        {
            String[] arg = new String[7];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_main_xml;
            arg[5] = as_chqlist_xml;
            arg[6] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintChq", arg);
            //FinanceSvEn fin = new FinanceSvEn(wsPass);
            //return fin.of_postpaychq(as_branch, as_entry, adtm_wdate, as_machine, as_main_xml, as_chqlist_xml, as_formset);
        }

        [WebMethod]
        public String[] OfInitPayChqSplit(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_paychq_split(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String OfPostPayChqSplit(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_cond_xml, String as_bankcut_xml, String as_chqtype_xml, String as_chqlist_xml, String as_chqspilt_xml, String as_formset)
        {
            String[] arg = new String[10];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_cond_xml;
            arg[5] = as_bankcut_xml;
            arg[6] = as_chqtype_xml;
            arg[7] = as_chqlist_xml;
            arg[8] = as_chqspilt_xml;
            arg[9] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintChqFromSplit", arg);
            //FinanceSvEn fin = new FinanceSvEn(wsPass);
            //return fin.of_postpaychq_split(as_branch, as_entry, adtm_wdate, as_machine, as_cond_xml, as_bankcut_xml, as_chqtype_xml, as_chqlist_xml, as_chqspilt_xml, as_formset);
        }

        [WebMethod]
        public String OfRetrieveCancelChq(String wsPass, String as_branch, String as_cond_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievecancelchq(as_branch, as_cond_xml);
        }

        [WebMethod]
        public String OfRetrieveChangeChqStatus(String wsPass, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievechangechqstatus(as_branch);
        }

        [WebMethod]
        public String OfRetrieveChangeChqDetail(String wsPass, String as_chqno, String as_bookno, String as_bank, String as_bankbranch, Int16 ai_seqno)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievechangchqdetail(as_chqno, as_bookno, as_bank, as_bankbranch, ai_seqno);
        }

        [WebMethod]
        public String OfInitPostToBank(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_posttobank(as_branch, adtm_wdate);
        }

        [WebMethod]
        public int OfFinPostToBank(String wsPass, String as_branch, String as_entryid, DateTime adtm_wdate, String as_machine, String as_item_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_fin_posttobank(as_branch, as_entryid, adtm_wdate, as_machine, as_item_xml);
        }

        [WebMethod]
        public String OfInitFinConTact(String wsPass, String as_contact_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_fincontact(as_contact_xml);
        }

        [WebMethod]
        public String OfInitSendChq(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_sendchq(as_branch, as_entry, adtm_wdate);
        }

        [WebMethod]
        public int OfPostSaveSendChq(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_sendchq_xml, String as_waitchq_xml, String as_sendchqacc_xml, Int16 ai_accknow)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postsavesendchq(as_branch, as_entry, adtm_wdate, as_machine, as_sendchq_xml, as_waitchq_xml, as_sendchqacc_xml, ai_accknow);
        }

        [WebMethod]
        public String OfRetrieveCancelSendChq(String wsPass, String as_branch, DateTime adtm_wdate, String as_bank_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievecancel_sendchq(as_branch, adtm_wdate, as_bank_xml);
        }

        [WebMethod]
        public int OfPostCancelSendChq(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_bank_xml, String as_cancellist_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postcancel_sendchq(as_branch, as_entry, adtm_wdate, as_machine, as_bank_xml, as_cancellist_xml);
        }

        [WebMethod]
        public int OfPostCancelPostToBank(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_banklist_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postcancelposttobank(as_branch, as_entry, adtm_wdate, as_machine, as_banklist_xml);
        }

        [WebMethod]
        public int OfPostCancelSengChqDel(String wsPass, String as_branch, String as_chqno, String as_bank, String as_bankbranch, Int16 si_seqno)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postcancelsendchq(as_branch, as_chqno, as_bank, as_bankbranch, si_seqno);
        }

        [WebMethod]
        public String[] OfInitPostOtherToFin(String wsPass, String as_memb_xml, String appname)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_postotherto_fin(as_memb_xml, appname);
        }

        [WebMethod]
        public String OfRetrieveFinSlipDet(String wsPass, String as_slipno, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retreivefinslipdet(as_slipno, as_branch);
        }

        [WebMethod]
        public String OfPostOtheToFin(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_appname, String as_main_xml, String as_itemdet_xml, String as_cancelXml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postotherto_fin(as_branch, as_entry, adtm_wdate, as_appname, as_main_xml, as_itemdet_xml, as_cancelXml);
        }

        [WebMethod]
        public String OfRetrievePayChqList(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievepaychqlist(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String OfRetrieveChqFromSlip(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievechqfromslip(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String OfRetrieveRePrintChq(String wsPass, String as_branch, String as_retrieve_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievereprintchq(as_branch, as_retrieve_xml);
        }

        [WebMethod]
        public String OfRetrieveRePrintPaySlip(String wsPass, String as_branch, String as_cond_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievereprintpayslip(as_branch, as_cond_xml);
        }

        [WebMethod]
        public String OfRetrieveRePrintReceipt(String wsPass, String as_branch, String as_cond_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievereprintreceipt(as_branch, as_cond_xml);
        }

        [WebMethod]
        public String OfPostRePrintChq(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_formset, String as_cond_xml, String as_retrieve_xml, String as_chqlist_mal)
        {
            String[] arg = new String[8];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_formset;
            arg[5] = as_cond_xml;
            arg[6] = as_retrieve_xml;
            arg[7] = as_chqlist_mal;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "ReprintChq", arg);
        }

        [WebMethod]
        public String OfPostRePrintPaySlip(String wsPass, String as_branch, DateTime adtm_wdate, String as_list_xml, String as_formset)
        {
            String[] arg = new String[4];
            arg[0] = as_branch;
            arg[1] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[2] = as_list_xml;
            arg[3] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "ReprintPayslip", arg);
        }

        [WebMethod]
        public String OfPostRePrintReceipt(String wsPass, String as_branch, DateTime adtm_wdate, String as_list_xml, String as_formset)
        {
            String[] arg = new String[4];
            arg[0] = as_branch;
            arg[1] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[2] = as_list_xml;
            arg[3] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "ReprintReceipt", arg);
        }

        [WebMethod]
        public String OfPostPrintSlip(String wsPass, String as_slipNo, String as_branchID, String as_formSet)
        {
            String[] arg = new String[3];
            arg[0] = as_slipNo;
            arg[1] = as_branchID;
            arg[2] = as_formSet;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintSlip", arg);
        }

        [WebMethod]
        public String OfPostPrintCashCrtl(String wsPass, String as_branchID, String as_username, String as_app, DateTime adtm_workdate, Int16 ai_seqNo, String as_formSet)
        {
            String[] arg = new String[6];
            arg[0] = as_branchID;
            arg[1] = as_username;
            arg[2] = as_app;
            arg[3] = adtm_workdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[4] = ai_seqNo.ToString();
            arg[5] = as_formSet;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintCashCtrl", arg);

        }

        [WebMethod]
        public Decimal[] OfItemCaltax(String wsPass, Int16 ai_recvpay, Int16 ai_calvat, Int16 ai_taxcode, Decimal adc_itemamt)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_itemcaltax(ai_recvpay, ai_calvat, ai_taxcode, adc_itemamt);
        }

        [WebMethod]
        public String[] OfGetAddress(String wsPass, String as_memberno, Int16 ai_memberflag, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getaddress(as_memberno, ai_memberflag, as_branch);
        }

        [WebMethod]
        public String OfRetrieveTaxPay(String wsPass, String as_branch, String as_main_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievetaxpay(as_branch, as_main_xml);
        }

        [WebMethod]
        public String OfPostPrintTaxPay(String wsPass, String as_branch, String as_slipno, String as_formset)
        {
            String[] arg = new String[3];
            arg[0] = as_branch;
            arg[1] = as_slipno;
            arg[2] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintTaxPay", arg);
        }

        [WebMethod]
        public String OfPostRePrintTaxPay(String wsPass, String as_branch, DateTime adtm_wdate, String as_slipno, Int16 ai_topay, Int16 ai_keep, Int16 ai_formcoop, String as_formset)
        {
            String[] arg = new String[7];
            arg[0] = as_branch;
            arg[1] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[2] = as_slipno;
            arg[3] = Convert.ToString(ai_topay);
            arg[4] = Convert.ToString(ai_keep);
            arg[5] = Convert.ToString(ai_formcoop);
            arg[6] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "RePrintTaxPay", arg);
        }

        [WebMethod]
        public String[] OfRetrieveBankAccount(String wsPass, String as_branch, String as_bank_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievebankaccount(as_branch, as_bank_xml);
        }

        [WebMethod]
        public int OfPostUpdateBankAccount(String wsPass, String as_bank_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postupdatebankaccount(as_bank_xml);
        }

        [WebMethod]
        public String OfRetrieveListFormatChq(String wsPass, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievelistformatchq(as_branch);
        }

        [WebMethod]
        public String OfRetrieveFormatChq(String wsPass, String as_branch, String as_bankcode, Int16 ai_chqtype, Int16 ai_printtype)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrieveformatchq(as_branch, as_bankcode, ai_chqtype, ai_printtype);
        }

        [WebMethod]
        public int OfPostPrintPreviewChq(String wsPass, String as_branch, String as_bank, Int16 ai_chqsize, String as_printtype, String as_formset)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postprintpreviewchq(as_branch, as_bank, ai_chqsize, as_printtype, as_formset);
        }

        [WebMethod]
        public String OfRetrieveReceiveChq(String wsPass, String as_branch)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_retrievereceivechq(as_branch);
        }

        [WebMethod]
        public Int16 OfPostReceiveChq(String wsPass, String as_entryid, String as_machineid, DateTime as_wdate, String as_chqlist_xml)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postreceivechq(as_entryid, as_machineid, as_wdate, as_chqlist_xml);
        }

        [WebMethod]
        public String[] OfInitChqFromApvLoan(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_chqfrom_apvloan(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String[] OfInitPayChqApvLoanCbt(String wsPass, String as_branch, DateTime adtm_wdate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_paychq_apvloancbt(as_branch, adtm_wdate);
        }

        [WebMethod]
        public String OfPostPayChqFromApvLoan(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_cond_xml, String as_cutbank_xml, String as_chqtype_xml, String as_chqlist_xml, String as_formset)
        {
            String[] arg = new String[9];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_cond_xml;
            arg[5] = as_cutbank_xml;
            arg[6] = as_chqtype_xml;
            arg[7] = as_chqlist_xml;
            arg[8] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintChqFromApvLoan", arg);
            //FinanceSvEn fin = new FinanceSvEn(wsPass);
            //return fin.of_postpaychq_fromapvloan(as_branch, as_entry, adtm_wdate, as_machine, as_cond_xml, as_cutbank_xml, as_chqtype_xml, as_chqlist_xml, as_formset);
        }

        [WebMethod]
        public String OfPostPayChqApvLoanCbt(String wsPass, String as_branch, String as_entry, DateTime adtm_wdate, String as_machine, String as_main_xml, String as_chqlist_xml, String as_formset)
        {
            String[] arg = new String[7];
            arg[0] = as_branch;
            arg[1] = as_entry;
            arg[2] = adtm_wdate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            arg[3] = as_machine;
            arg[4] = as_main_xml;
            arg[5] = as_chqlist_xml;
            arg[6] = as_formset;
            return new WinPrintCalling(wsPass).CallWinPrint("app_finance", "PrintChqFromApvLoanCbt", arg);
            //FinanceSvEn fin = new FinanceSvEn(wsPass);
            //return fin.of_postpaychq_apvloancbt(as_branch, as_entry, adtm_wdate, as_machine, as_main_xml, as_chqlist_xml, as_formset);
        }

        [WebMethod]
        public int OfProcessOtherToFin(String wsPass, String as_branchid, String as_entry_id, DateTime adtm_wdate, String as_machineid)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_postprocessotherto_fin(as_branchid, as_entry_id, adtm_wdate, as_machineid);
        }
        //bask-------------------
        [WebMethod]
        public String InitMoneyOrder(String wsPass, String xmlMain, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_init_moneyorder(xmlMain, entryId, workDate);
        }

        [WebMethod]
        public int InitSaveMoneyOrder(String wsPass, String xmlMain, String xmlDetail, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_save_moneyorder(xmlMain, xmlDetail, entryId, workDate);
        }

        [WebMethod]
        public int OperateMoneyOrder(String wsPass, String ptbDocNo, String ptbTypeCode, int seqNo, String referDocNo, short statusFlag, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_operate_moneyorder(ptbDocNo, ptbTypeCode, seqNo, referDocNo, statusFlag, entryId, workDate);
        }

        [WebMethod]
        public String GetListMoneyOrder(String wsPass, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getlist_moneyorder(workDate);
        }

        [WebMethod]
        public String[] GetDataMoneyOrder(String wsPass, String docNo)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getdata_moneyorder(docNo);
        }

        [WebMethod]
        public String GetListApprMoneyOrder(String wsPass, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getlistappr_moneyorder(workDate);
        }

        [WebMethod]
        public String GetListCancelMoneyOrder(String wsPass, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getlistcancel_moneyorder(workDate);
        }

        [WebMethod]
        public String GetListReapprMoneyOrder(String wsPass, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_getlistreappr_moneyorder(workDate);
        }

        [WebMethod]
        public int SaveApproveMoneyOrder(String wsPass, String xmlList, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_approve_moneyorder(xmlList, entryId, workDate);
        }

        [WebMethod]
        public int SaveCancelMoneyOrder(String wsPass, String xmlList, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_cancel_moneyorder(xmlList, entryId, workDate);
        }

        [WebMethod]
        public int SaveCancelApprMoneyOrder(String wsPass, String xmlList, String entryId, DateTime workDate)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_cancelappr_moneyorder(xmlList, entryId, workDate);
        }
        //----------bask

        // a 
        [WebMethod]
        public decimal of_recal_installment(String wsPass, String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref Decimal adc_periodpay)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_recal_installment(as_contno, adtm_paydate, adc_prnbal, ref adc_periodpay);

        }
        // a 
        [WebMethod]
        public decimal of_recal_periodpay(String wsPass, String as_contno, DateTime adtm_paydate, decimal adc_prnbal, ref short ai_installment)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_recal_periodpay(as_contno, adtm_paydate, adc_prnbal, ref ai_installment);

        }

        // a 
        [WebMethod]
        public decimal of_recal_installmentmanual(String wsPass, String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref Decimal adc_paymentfixed)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_recal_installmentmanual(as_contno, adtm_paydate, adc_prnbal, ref adc_paymentfixed);

        }

        // a 
        [WebMethod]
        public decimal of_recal_periodpaymanual(String wsPass, String as_contno, DateTime adtm_paydate, Decimal adc_prnbal, ref  short ai_installfixed)
        {
            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_recal_periodpaymanual(as_contno, adtm_paydate, adc_prnbal, ref ai_installfixed);

        }
        // A   สร้างแผ่านส่งธนาคาร
        [WebMethod]
        public String of_initlist_lnrcvdisk(String wsPass, String as_lntype_start, String as_lntype_end, DateTime adtm_reqdatre_start, DateTime adtm_reqdate_end)
        {

            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_initlist_lnrcvdisk(as_lntype_start, as_lntype_end, adtm_reqdatre_start, adtm_reqdate_end);

        }

        // A   Save สร้างแผ่านส่งธนาคาร
        [WebMethod]
        public int of_save_lnrcvdisk(String wsPass, str_lnrcvdisk as_str_lncvdisk)
        {

            FinanceSvEn fin = new FinanceSvEn(wsPass);
            return fin.of_save_lnrcvdisk(as_str_lncvdisk);

        }
    }
}
