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
    /// Summary description for Account
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Account : System.Web.Services.WebService
    {
        // service โชว์ข้อมูล Voucher ตามวันที่ voucher
        [WebMethod]
        public String GetVcListDay(String wsPass, DateTime dateList, String branchId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_vclist_day(dateList, branchId);
        }

        // คลิกที่รายการใบ Voucher แสดงข้อมูลแต่ละใบ
        [WebMethod]
        public String[] GetListVcMasDetail(String wsPass, String vcNo)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_list_vcmas_det(vcNo);
        }

        // service หน้าจอบันทึกแก้ไขรายการ Voucher
        [WebMethod]
        public int  AddNewUpdateVoucher(String wsPass, String xmlVcMas, String xmlVcDet, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_add_new_update_voucher(xmlVcMas, xmlVcDet,branchID);
        }

        // service หายอดเงินสดยกมา ยกไป
        [WebMethod]
        public Decimal[] GetCashBeginForward(String wsPass, DateTime dateList, String branchId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_cash_bg_fw (dateList, branchId);
        }

        // service Get Vclist No Post to Gl
        [WebMethod]
        public String GetVclistNopost(String wsPass, DateTime DateBegin,DateTime DateEnd,Int16 PostStatus, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_vclist_nopost(DateBegin, DateEnd, PostStatus , branchID);   
        }

        // service การ Save  Post To GL
        [WebMethod]
        public int PostToGl(String wsPass, String postlist, String entry_id, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_post_to_gl(postlist, entry_id, branchID);
        }

        // service การ Save  Cancel Post To GL
        [WebMethod]
        public int CancelPostToGl(String wsPass, String postlist, String entry_id, String branchID, DateTime Vcdate)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_cancel_post_to_gl (postlist, entry_id, branchID, Vcdate);
        }

        // service การปิดสิ้นปีบัญชี
        [WebMethod]
        public int CloseYear(String wsPass, Int16 accyear, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_close_year (accyear,branchID);
        }

        // service การปิดสิ้นเดือนบัญชี
        [WebMethod]
        public int CloseMonth(String wsPass, Int16 accyear, Int16 accperiod, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_close_month(accyear, accperiod, branchID);
        }

        // service ยกเลิกการปิดสิ้นเดือนบัญชี
        [WebMethod]
        public int CancelCloseMonth(String wsPass, Int16 accyear, Int16 accperiod, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_cancel_close_month (accyear, accperiod, branchID);
        }

        // service ลบแถวปีบัญชี
        [WebMethod]
        public int DeleteRowAccyear(String wsPass, Int16 accyear)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_candel_year (accyear);
        }

        // service ยกเลิกการปิดสิ้นปีบัญชี
        [WebMethod]
        public int CancelCloseYear(String wsPass, Int16 accyear, String branchID)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_cancel_closeyear (accyear,  branchID);
        }

        // service ลบแถวงวดบัญชี
        [WebMethod]
        public int DeleteRowAccperiod(String wsPass, Int16 accyear, Int16 accperiod)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_candel_period (accyear,accperiod);
        }
        // service ประมวลผลออกงบการเงิน
        [WebMethod]
        public string GenBalanceSheet(String wsPass, string xmlHead, string moneysheet_code, string branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_balance_sheet(xmlHead, moneysheet_code, branch_id);
        }

        // service ประมวลผลออกงบทดลอง
        [WebMethod]
        public string GenTrial(String wsPass, String xmlChooseReport, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_trial_bs(xmlChooseReport, branch_id);
        }

        // service เพิ่มรหัสบัญชี
        [WebMethod]
        public int SaveAccountId(String wsPass, String AccountDetail, Int16 update_add)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_add_newaccount_id(AccountDetail, update_add);
        }

         //service ลบรหัสบัญชี
        [WebMethod]
        public int DeleteAccountId(String wsPass, String accountid)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_delete_accountid(accountid);
        }

        // service บันทึกสูตรค่าคงที่
        [WebMethod]
        public int SaveFormulaConstant(String wsPass, String xmlMain, String xmlDetail, String money_code, Int16 data_group)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_update_bs_constant_exp(xmlMain, xmlDetail, money_code, data_group);
        }

        // service บันทึกสูตรงบการเงิน ผลรวม อ้างงบอื่น
        [WebMethod]
        public int SaveFormulaSM(String wsPass, String xmlMain, String money_code, String data_group)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_save_exp_bs_sheet_sm(xmlMain, money_code, data_group);
        }

        // service บันทึกสูตรงบการเงิน การคำนวณ
        [WebMethod]
        public int SaveFormulaFC(String wsPass, String xmlMain, String money_code, String data_group)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_save_exp_bs_sheet_fc(xmlMain, money_code, data_group);
        }
        
        // service ดึงข้อมูลรายการยกเลิกรายวัน
        [WebMethod]
        public String GetVcListDayCancel(String wsPass, DateTime dateList, String branchId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_vc_listcancel(dateList, branchId);
        }

        //service บันทึกข้อมูลยกเลิก Voucher
        [WebMethod]
        public int SaveCancelVoucher(String wsPass, DateTime voucher_date, String voucher_no, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_cancel_voucher(voucher_date, voucher_no, branch_id);
        }

        //service Generate บันทึกแก้ไขยอดยกมา
        [WebMethod]
        public int  GenerateFirstSumleger(String wsPass, Int16  account_year, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_add_first_sumleger_period(account_year, branch_id);
        }
        // service หน้าจอบันทึกข้อมูลจัดรูปแบบงบการเงิน
        [WebMethod]
        public int SaveMoneySheet(String wsPass, String xmlVcDet, String money_code)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.Of_save_money_sheet(xmlVcDet, money_code);
        }
        // service Save หน้าจอบันทึกข้อมูลยอดยกมา
        [WebMethod]
        public int SaveBeginAcc(String wsPass, String xmlBegindata, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_update_accbegin(xmlBegindata,branch_id);
        }
        // Service Show ข้อมูล Dialog หน้าจอบันทึกสูตร FC
        [WebMethod]
        public String[] GetSetFormulaFC(String wsPass, String money_sheetcode,Int16  data_group)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_set_formula_fc(money_sheetcode, data_group);
        }

        // Service Show ข้อมูล Dialog หน้าจอบันทึกสูตร SM
        [WebMethod]
        public String[] GetSetFormulaSM(String wsPass, String money_sheetcode, Int16 data_group)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_set_formula_sm (money_sheetcode, data_group);
        }
        // Service Show ข้อมูล Dialog หน้าจอบันทึกสูตร SM
        [WebMethod]
        public String[] GetSetFormulaConstant(String wsPass, String data_desc)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_display_consstant_bs(data_desc);
        }
        // Service สหรับออกรายงานสรุป vc ประจำวัน
        [WebMethod]
        public String GenJournalReport(String wsPass, DateTime bg_date, DateTime ed_date, Int16 type_group, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_day_journalreport(bg_date, ed_date, type_group, branch_id);
        }
        //ประมวลผลรายงาน แยกประเภทบัญชี
        [WebMethod]
        public String GenLedgerReport(String wsPass, DateTime st_date, DateTime ed_date, String acc_id_1, String acc_id_2, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_ledger_report(st_date, ed_date, acc_id_1, acc_id_2, branch_id);
        }
        //ประมวลผลรายงาน กระดาษทำงานงบกระแสเงินสด เดเค
        [WebMethod]
        public String GenCashFlowPaperDRCR(String wsPass, DateTime st_date, DateTime ed_date, Int16 sum_Period, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_cashpaper_drcr(st_date, ed_date, sum_Period, branch_id);
        }
        //ประมวลผลรายงาน งบกระแสเงินสด
        [WebMethod]
        public String GenCashFlowSheet(String wsPass, string acc_bs_head_xml, string moneysheet_code, string branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_cashflow_sheet(acc_bs_head_xml, moneysheet_code, branch_id);
        }
        //ประมวลผลรายงาน กระดาษทำงานงบกระแสเงินสด กิจกรรม
        [WebMethod]
        public String GenCashFlowPaperATV(String wsPass, DateTime st_date, DateTime ed_date, Int16 sum_Period, String branch_id)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_gen_cashpaper_atv(st_date, ed_date, sum_Period, branch_id);
        }
        //ดึงข้อมูลการจัดสรรกำไร by bask
        [WebMethod]
        public String GetContuseProfit(String wsPass, short year, short period)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);    
            return acc.of_get_contuseprofit(year, period);
        }
        //บันทึกข้อมูลการจัดสรรกำไร by bask
        [WebMethod]
        public int SaveContuseProfit(String wsPass, String xmlConuse)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_save_contuseprofit(xmlConuse);
        }

        [WebMethod]
        public String GetAccountSplitActivity(String wsPass, short year, short period, String branchId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_account_splitactivity(year, period, branchId);
        }

        [WebMethod]
        public int SaveAccountSplitActivity(String wsPass, String xmlDetail)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_save_account_splitactivity(xmlDetail);
        }

        [WebMethod]
        public String GetCashPaperDetail(String wsPass, short sheetType, String accActivity)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_cashpaper_detail(sheetType, accActivity);
        }

        [WebMethod]
        public int UpdateCashPaperDetail(String wsPass, String xmlDetail, short sheetType, String accActivity)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_update_cashpaper_detail(xmlDetail, sheetType, accActivity);
        }

        [WebMethod]
        public String GetVcdetailForsetNoncash(String wsPass, DateTime startDate, DateTime endDate, String accountId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_get_vcdetail_forset_noncash(startDate, endDate, accountId);
        }

        [WebMethod]
        public int UpdateSetnoncashPaper(String wsPass, String detailXml, DateTime startDate, DateTime endDate, String accountId)
        {
            AccountSvEn acc = new AccountSvEn(wsPass);
            return acc.of_update_setnoncash_paper(detailXml, startDate, endDate, accountId);
        }
    }
}
