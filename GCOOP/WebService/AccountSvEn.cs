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
    public class AccountSvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_account_service svAcc;
        private Security security;

        public AccountSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public AccountSvEn(String wsPass, bool autoConnect)
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
                svAcc = new n_cst_account_service();
                svAcc.of_init(svCon, "000");//กำ สาขาต้องฮาสโคดไปก่อนน่ะ -*- พี่ระเล่นไม่บอก่อน
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

        ~AccountSvEn()
        {
            DisConnect();
        }

        //-------------------------------- เริ่มต้นใช้ PB SErvice
        // function เรียก Voucher ตามวันที่
        public String of_get_vclist_day(DateTime dateList, String branchId)
        {
            try
            {
                svAcc.of_set_default_accountid(branchId);
                String xmlVcList = "";
                svAcc.of_get_vclist_day(dateList, branchId, ref xmlVcList);
                DisConnect();
                return xmlVcList;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        // function เรียกรายละเอียด ตาม ใบ Voucher แต่ละใบ
        public String[] of_get_list_vcmas_det(String vcNo)
        {
            try
            {
                String[] result = new string[2];
                String xmlVcDet = "";
                String xmlVcMas = "";
                svAcc.of_get_list_vcmas_det(vcNo, ref xmlVcMas, ref xmlVcDet);
                result[0] = xmlVcMas;
                result[1] = xmlVcDet;
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        //function เพิ่มหรือแก้ไขข้อมูลใบ Voucher ใหม่
        public int of_add_new_update_voucher( String xmlVcMas, String xmlVcDet, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_add_new_updatevoucher(xmlVcMas, xmlVcDet, branchID);
                DisConnect();
                return result;
                
            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        //function โชว์ยอดเงินสดยกมา และเงินสดยกไป
        public Decimal[] of_get_cash_bg_fw(DateTime dateList, String branchId)
        {
            try
            {
                svAcc.of_set_default_accountid(branchId);
                Decimal [] result = new Decimal[2];
                Decimal Begin = 0;
                Decimal Forward = 0;
                svAcc.of_get_cash_bg_fw(dateList, branchId, ref Begin, ref Forward);
                result[0] = Begin;
                result[1] = Forward;
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        
        //function ในการดึงข้อมูลใบรายการ Voucher หน้าจอผ่านรายการ
        public String of_get_vclist_nopost(DateTime DateBegin, DateTime DateEnd, Int16 PostStatus,String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                String xmlVcList = "";
                svAcc.of_get_vclist_nopost(DateBegin, DateEnd, PostStatus, branchID, ref xmlVcList);
                DisConnect();
                return xmlVcList;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //function บันทึกหน้าจอ ผ่านรายการ
        public int of_post_to_gl( String postlist, String entry_id, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_post_to_gl (postlist, entry_id, branchID);
                DisConnect();
                return result;
                
            }
            catch (Exception ex) { throw ex; }
        }

        //function บันทึกหน้าจอ ยกเลิกผ่านรายการ
        public int of_cancel_post_to_gl(String postlist, String entry_id, String branchID, DateTime Vcdate)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_cancel_post_to_gl(postlist, entry_id, branchID, Vcdate);
                DisConnect();
                return result;

            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        //function Close Year
        public int of_close_year(Int16  accyear, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result  = svAcc.of_close_year(accyear, branchID);
                DisConnect();
                return result;

            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }


        //function Close Month
        public int Of_close_month(Int16 accyear, Int16 accperiod, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_close_month(accyear, accperiod, branchID);
                DisConnect();
                return result;

            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        //function Canclose Close Month
        public int Of_cancel_close_month(Int16 accyear, Int16 accperiod, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_cancel_close_month (accyear, accperiod, branchID);
                DisConnect();
                return result;

            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        //function ตรวจสอบว่าลบปีบัญชีได้หรือไม่
        public int Of_candel_year(Int16 accyear)
        {
            try
            {
                int result = svAcc.of_candel_year (accyear);
                DisConnect();
                return result;

            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }

        // function Cancel CloseYear

        public int Of_cancel_closeyear(Int16 accyear, String branchID)
        {
            try
            {
                svAcc.of_set_default_accountid(branchID);
                int result = svAcc.of_cancel_closeyear (accyear, branchID);
                DisConnect();
                return result;

            }
            catch (Exception ex)
            { 
                DisConnect(); 
                throw ex; 
            }
        }

        //function ตรวจสอบว่าลบงวดบัญชีได้หรือไม่
        public int Of_candel_period(Int16 accyear,Int16 accperiod)
        {
            try
            {
                int result = svAcc.of_candel_period (accyear,accperiod);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }


        //function ประมวลผลออกงบการเงิน
        public string of_gen_balance_sheet(string xmlHead, string moneysheet_code, string branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String xmlPLSheet = " ";
                if (svAcc.of_gen_balance_sheet(xmlHead, moneysheet_code, branch_id, ref xmlPLSheet) == 1)
                {
                    DisConnect();
                    return xmlPLSheet;
                }
                else
                {
                    DisConnect();
                    return xmlPLSheet;
                }
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ประมวลผลออกงบทดลอง
        public string of_gen_trial_bs(String xmlChooseReport, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String XmlTrial = " ";
                if (svAcc.of_gen_trial_bs(xmlChooseReport, branch_id, ref XmlTrial) == 1)
                {
                    DisConnect();
                    return XmlTrial;
                }
                else
                {
                    DisConnect();
                    return XmlTrial;
                }
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function เพิ่มรหัสบัญชีใหม่
        public int of_add_newaccount_id(String xmlAccountDetail, Int16 update_add)
        {
            try
            {
                int result = svAcc.of_add_newaccount_id(xmlAccountDetail, update_add);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ลบรหัสบัญชี
        public int of_delete_accountid(String accountid)
        {
            try
            {
                int result = svAcc.of_delete_accountid(accountid);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function บันทึกสูตรค่าคงที่ Const
        public int of_update_bs_constant_exp(String xmlMain, String xmlDetail, String money_code, Int16 data_group)
        {
            try
            {
                int result = svAcc.of_update_bs_constant_exp(xmlMain, xmlDetail, money_code, data_group);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function บันทึกสูตรงบการเงิน ผลรวม อ้างงบอื่น SM
        public int of_save_exp_bs_sheet_sm(String xmlMain, String money_code, String data_group)
        {
            try
            {
                int result = svAcc.of_save_exp_bs_sheet_sm(xmlMain, money_code, data_group);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function บันทึกสูตรงบการเงิน การคำนวณ FC
        public int of_save_exp_bs_sheet_fc(String xmlMain, String money_code, String data_group)
        {
            try
            {
                int result = svAcc.of_save_exp_bs_sheet_fc(xmlMain, money_code, data_group);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        // function ดึงข้อมูลรายการยกเลิกรายวัน
        public String of_get_vc_listcancel(DateTime dateList, String branchId)
        {
            try
            {
                svAcc.of_set_default_accountid(branchId);
                String xmlVcList = "";
                svAcc.of_get_vc_listcancel (dateList, branchId, ref xmlVcList);
                DisConnect();
                return xmlVcList;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
            
        }

        //function บันทึกข้อมูลยกเลิก Voucher
        public int of_cancel_voucher(DateTime voucher_date, String voucher_no, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                int result = svAcc.of_cancel_voucher(voucher_date, voucher_no, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function Generate บันทึกแก้ไขยอดยกมาต้นปี
        public int of_add_first_sumleger_period(Int16  account_year, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                int result = svAcc.of_add_first_sumleger_period(account_year, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function บันทึกข้อมูลหน้าจัดรูปแบบงบการเงิน
        public int Of_save_money_sheet(String xmlVcDet, String money_code)
        {
            try
            {
                int result = svAcc.of_save_money_sheet(xmlVcDet,money_code);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function บันทึกแก้ไขหน้าจอบันทึกยอดยกมา
        public int of_update_accbegin(String xmlBegindata, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                int result = svAcc.of_update_accbegin(xmlBegindata, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ดึงข้อมูลขึ้นมา Show Dialog ของ บันทึกสูตรงบการเงิน ผลรวม อ้างงบอื่น FC
        public String[] of_get_set_formula_fc(String money_sheetcode, Int16 data_group)
        {
            try
            {
                String[] result = new string[2];
                String xmlFc = "";
                String xmlFcChoose = "";
                svAcc.of_get_set_formular_fc(money_sheetcode, data_group, ref xmlFc, ref xmlFcChoose);
                result[0] = xmlFc;
                result[1] = xmlFcChoose;
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ดึงข้อมูลขึ้นมา Show Dialog ของ บันทึกสูตรงบการเงิน ผลรวม อ้างงบอื่น SM
        public String[] of_get_set_formula_sm(String money_sheetcode, Int16  data_group)
        {
            try
            {
                String[] result = new string[2];
                String xmlSm = "";
                String xmlSmChoose = "";
                svAcc.of_get_set_formular_sm(money_sheetcode, data_group, ref xmlSm, ref xmlSmChoose);
                result[0] = xmlSm;
                result[1] = xmlSmChoose;
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ดึงข้อมูลขึ้นมา Show Dialog ของ Constant
        public String[] of_gen_display_consstant_bs(String data_desc)
        {
            try
            {
                String[] result = new string[2];
                String xmlBuzzDisplay = "";
                String xmlUsrType = "";
                svAcc.of_gen_display_consstant_bs(data_desc, ref xmlBuzzDisplay, ref xmlUsrType);
                result[0] = xmlBuzzDisplay;
                result[1] = xmlUsrType;
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ประมวลผลรายงาน สรุปประจำวัน
        public String of_gen_day_journalreport(DateTime st_date, DateTime ed_date, Int16 type_group, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String result = svAcc.of_gen_day_journalreport( st_date,  ed_date,  type_group,  branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ประมวลผลรายงาน แยกประเภทบัญชี
        public String of_gen_ledger_report(DateTime st_date, DateTime ed_date, String acc_id_1, String acc_id_2, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String result = svAcc.of_gen_ledger_report(st_date, ed_date, acc_id_1, acc_id_2, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ประมวลผลรายงาน กระดาษทำงานงบกระแสเงินสด เดเค
        public String of_gen_cashpaper_drcr(DateTime st_date, DateTime ed_date, Int16 sum_Period, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String result = svAcc.of_gen_cashpaper_drcr(st_date, ed_date, sum_Period, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }

        //function ประมวลผลรายงาน งบกระแสเงินสด
        public String of_gen_cashflow_sheet(string acc_bs_head_xml, string moneysheet_code, string branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String XmlTrial = " ";
                if (svAcc.of_gen_cashflow_sheet(acc_bs_head_xml, moneysheet_code, branch_id, ref XmlTrial) == 1)
                {
                    DisConnect();
                    return XmlTrial;
                }
                else
                {
                    DisConnect();
                    return XmlTrial;
                }
            }
            catch (Exception ex) 
            {
                DisConnect(); 
                throw ex;
            }
        }
        //function ประมวลผลรายงาน กระดาษทำงานงบกระแสเงินสด กิจกรรม
        public String of_gen_cashpaper_atv(DateTime st_date, DateTime ed_date, Int16 sum_Period, String branch_id)
        {
            try
            {
                svAcc.of_set_default_accountid(branch_id);
                String result = svAcc.of_gen_cashpaper_activities(st_date, ed_date, sum_Period, branch_id);
                DisConnect();
                return result;
            }
            catch (Exception ex) 
            {
                DisConnect();
                throw ex; 
            }
        }
        //ดึงข้อมูลการจัดสรรกำไร by bask
        public String of_get_contuseprofit(short year, short period)
        {
            try
            {
                String result = svAcc.of_get_contuseprofit(year, period);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //บันทึกมูลการจัดสรรกำไร by bask
        public int of_save_contuseprofit(String xmlConuse)
        {
            try
            {
                int result = svAcc.of_save_contuseprofit(xmlConuse);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_get_account_splitactivity(short year, short period, String branchId)
        {
            try
            {
                String result = svAcc.of_get_account_splitactivity(year, period, branchId);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_save_account_splitactivity(String xmlDetail)
        {
            try
            {
                int result = svAcc.of_save_account_splitactivity(xmlDetail);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ดึงข้อมูล String_XML of_get_cashpaper_detail(sheet_type, activity)
        public String of_get_cashpaper_detail(short sheetType, String accActivity)
        {
            try
            {
                String result = svAcc.of_get_cashpaper_detail(sheetType, accActivity);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //บันทึก integer of_update_cashpaper_detail( xml, sheet_type, activity )
        public int of_update_cashpaper_detail(String xmlDetail, short sheetType, String accActivity)
        {
            try
            {
                int result = svAcc.of_update_cashpaper_detail(xmlDetail, sheetType, accActivity);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String of_get_vcdetail_forset_noncash(DateTime startDate, DateTime endDate, String accountId)
        {
            try
            {
                String result = svAcc.of_get_vcdetail_forset_noncash(startDate, endDate, accountId);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int of_update_setnoncash_paper(String detailXml, DateTime startDate, DateTime endDate, String accountId)
        {
            try
            {
                int result = svAcc.of_update_setnoncash_paper(detailXml, startDate, endDate, accountId);
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
