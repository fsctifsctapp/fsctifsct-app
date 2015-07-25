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
using WebService.Processing;
using GcoopServiceCs;

namespace WebService
{
    /// <summary>
    /// Summary description for Shrlon
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Shrlon : System.Web.Services.WebService
    {
        [WebMethod]
        public String ApvNewMember(String wsPass, String appl, String mem)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ApvNewMember(appl, mem);
        }

        [WebMethod]
        public String InitApvNewMemberList(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitApvNewMemberList();
        }

        [WebMethod]
        public String[] InitSlipPayIn(String wsPass, String ls_memno, String ls_sliptype, DateTime ldtm_slipdate, DateTime ldtm_opedate, String ls_entry_id, String ls_branch_id)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitSlipPayIn(ls_memno, ls_sliptype, ldtm_slipdate, ldtm_opedate, ls_entry_id, ls_branch_id);
        }

        [WebMethod]
        public String InitSlipPayInCalInt(String wsPass, String as_xmlloan, String as_sliptype, DateTime datecal)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitSlipPayInCalInt(as_xmlloan, as_sliptype, datecal);
        }

        [WebMethod]
        public String SaveSlipPayIn(String wsPass, String xml_sliphead, String xml_slipshr, String xml_sliplon, String xml_slipetc, DateTime ldtm_opedate, String ls_entry_id, String ls_branch_id)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveSlipPayIn(xml_sliphead, xml_slipshr, xml_sliplon, xml_slipetc, ldtm_opedate, ls_entry_id, ls_branch_id);
        }

        [WebMethod]
        public String Initlist_lnreqapv(String wsPass, String as_loantype_code)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.Initlist_lnreqapv(as_loantype_code);
        }

        [WebMethod]
        public String GetLastKeeping(String wsPass, String xml_main)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.GetLastKeeping(xml_main);
        }

        [WebMethod]
        public String GenReqDocNo(String wsPass, String as_loantype)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.GetNewContractNo(as_loantype);
        }

        [WebMethod]
        public String SaveLnReqRpv(String wsPass, String as_xmlreqlist, String as_apvid, String as_loantype_code)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveLnReqApv(as_xmlreqlist, as_apvid, as_loantype_code);
        }

        [WebMethod]
        public int LoanRightItemChangeMain(String wsPass, ref str_itemchange setitem)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.LoanRightItemChangeMain(ref setitem);
        }

        [WebMethod]
        public int LoanRightItemChangeColl(String wsPass, ref str_itemchange setitem)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.LoanRigthItemChangeColl(ref setitem);
        }

        [WebMethod]
        public int LoanRightItemChangeClear(String wsPass, ref str_itemchange setitem)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.LoanRightItemChangeClear(ref setitem);
        }

        [WebMethod]
        public String LoanRightSaveReqloan(String wsPass, str_savereqloan strSave)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.LoanRightSaveReqloan(strSave);
        }

        [WebMethod]
        public str_lntrncoll InitLnTrnColl(String wsPass, str_lntrncoll str)
        {
            return new ShrlonSvEn(wsPass).InitLnTrnColl(str);
        }

        [WebMethod]
        public bool SaveLnTrnColl(String wsPass, str_lntrncoll str)
        {
            return new ShrlonSvEn(wsPass).SaveLnTrnColl(str);
        }

        [WebMethod]
        public String[] InitLnTrnCollRecalTrn(String wsPass, String xmlmast, String xmldetail)
        {
            return new ShrlonSvEn(wsPass).InitLnTrnCollRecalTrn(xmlmast, xmldetail);
        }

        [WebMethod]
        public str_slippayout InitLnRcv(String wsPass, str_slippayout strSlipPayOut)
        {
            return new ShrlonSvEn(wsPass).InitLnRcv(strSlipPayOut);
        }

        [WebMethod]
        public int SaveMBreqApv(String wsPass, String as_xmlreqlist, String as_userid)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveMBreqApv(as_xmlreqlist, as_userid);
        }

        [WebMethod]
        public String InitListLnRcv(String wsPass, String as_moneytype)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitListLnRcv(as_moneytype);
        }

        [WebMethod]
        public str_slippayout InitLnRcv_RecalInt(String wsPass, str_slippayout strSlipPayOut)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitLnRcv_RecalInt(strSlipPayOut);
        }

        [WebMethod]
        public int SaveLnRcv(String wsPass, str_slippayout strSlipPayOut)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveLnRcv(strSlipPayOut);
        }

        [WebMethod]
        public str_requestopen LoanRequestOpen(String wsPass, str_requestopen strRequestOpen)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.LoanRequestOpen(strRequestOpen);

        }
        [WebMethod]
        public int InitcclSlippayinAll(ref str_slipcancel slipcancle, String wsPass, String member_no, String xml_memdet, String xml_sliplist)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitcclSlippayinAll(ref slipcancle, member_no, xml_memdet, xml_sliplist);
        }
        [WebMethod]
        public int InitcclSlippayinDet(ref str_slipcancel slipcancle, String wsPass, String slip_no, String xml_sliphead, String xml_slipdetail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitcclSlippayinDet(ref slipcancle, slip_no, xml_sliphead, xml_slipdetail);
        }
        [WebMethod]
        public int SavecclPayIn(String wsPass, String xml_sliphead, String xml_slipdetail, String slip_no, String cancel_id, DateTime cancel_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);

            return ln.SavecclPayIn(xml_sliphead, xml_slipdetail, slip_no, cancel_id, cancel_date);

        }
        [WebMethod]
        public Decimal GetShareMonthRate(String wsPass, String as_sharetype, Decimal adc_salary)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.GetShareMonthRate(as_sharetype, adc_salary);

        }
        [WebMethod]
        public Boolean IsvalidIdCard(String wsPass, String as_idcard)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.IsvalidIdCard(as_idcard);

        }
        [WebMethod]
        public DateTime CalReTryDate(String wsPass, DateTime adtm_birthdate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CalReTryDate(adtm_birthdate);
        }
        [WebMethod]
        public String ReqLoopOpen(String wsPass, String xml_main)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ReqLoopOpen(xml_main);
        }
        [WebMethod]
        public String ReCalFee(String wsPass, String xml_main)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ReCalFee(xml_main);
        }
        [WebMethod]
        public String ItemChangeReqLoop(String wsPass, String xml_main, String xml_reqloop)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ItemChangeReqLoop(xml_main, xml_reqloop);
        }
        [WebMethod]
        public String InitReqContCancel(String wsPass, String as_contno)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitReqContCancel(as_contno);

        }
        [WebMethod]
        public int SaveReqContCancel(String wsPass, String as_xmlcontccl, String as_cancelid, DateTime adtm_cancel)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveReqContCancel(as_xmlcontccl, as_cancelid, adtm_cancel);
        }
        [WebMethod]
        public int InitcclSlipLnrcvAll(ref str_slipcancel slipcancle, String wsPass, String member_no, String xml_memdet, String xml_sliplist)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitcclSlipLnrcvAll(ref slipcancle, member_no, xml_memdet, xml_sliplist);
        }
        [WebMethod]
        public int InitcclSlipLnrcvDet(ref str_slipcancel slipcancle, String wsPass, String slip_no, String xml_sliphead, String xml_slipdetail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitcclSlipLnrcvDet(ref slipcancle, slip_no, xml_sliphead, xml_slipdetail);
        }
        [WebMethod]
        public int SavecclLnrcv(String wsPass, String xml_sliphead, String xml_slipdetail, String slip_no, String cancel_id, DateTime cancel_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);

            return ln.SavecclLnrcv(xml_sliphead, xml_slipdetail, slip_no, cancel_id, cancel_date);

        }
        [WebMethod]
        public String SetSumOtherClr(String wsPass, String xml_main, String xml_otherclr)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SetSumOtherClr(xml_main, xml_otherclr);
        }
        [WebMethod]
        public int CancelRequest(String wsPass, ref String xml_main, ref String xml_message, String cancel_id, String branch)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CancelRequest(ref xml_main, ref xml_message, cancel_id, branch);
        }

        [WebMethod]
        public void CreateMthPayTab(String wsPass, String xml_main, String xml_clear, ref String xml_head, ref String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            ln.CreateMthPayTab(xml_main, xml_clear, ref xml_head, ref xml_detail);
        }
        [WebMethod]
        public String CollInitPrecent(String wsPass, String xml_main, String xml_garantee)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CollInitPrecent(xml_main, xml_garantee);
        }
        [WebMethod]
        public String CollPercCondition(String wsPass, String xml_main, String xml_garantee, ref String xml_message)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CollPercCondition(xml_main, xml_garantee, ref xml_message);
        }
        [WebMethod]
        public int CancelReqLoop(String wsPass, ref String xml_main, String cancel_id, String branch)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CancelReqLoop(ref xml_main, cancel_id, branch);
        }

        [WebMethod]
        public int CheckReqLoop(String wsPass, String loanType)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CheckReqLoop(loanType);
        }
        [WebMethod]
        public int ViewCollDetail(String wsPass, String strData, DateTime request_date, string xml_guarantee, ref String xml_head, ref String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ViewCollDetail(strData, request_date, xml_guarantee, ref xml_head, ref xml_detail);
        }
        [WebMethod]
        public int ViewLoanClearDetail(String wsPass, String strData, String xml_clear, ref String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ViewLoanClrDetail(strData, xml_clear, ref xml_detail);
        }
        [WebMethod]
        public int RegenLoanClear(String wsPass, ref String xml_main, ref String xml_clear, ref String xml_coll, ref String xml_message)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.RegenLoanClear(ref xml_main, ref xml_clear, ref xml_coll, ref xml_message);
        }
        [WebMethod]
        public int ResumLoanClear(String wsPass, ref String xml_main, ref String xml_clear, ref String xml_coll, String xml_loandetail, ref String xml_message)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ResumLoanClear(ref xml_main, ref xml_clear, ref xml_coll, xml_loandetail, ref xml_message);
        }
        //[WebMethod]
        //public int InitReqContAdjust(ref str_lncontaj contadj, String wsPass)
        //{
        //    ShrlonSvEn ln = new ShrlonSvEn(wsPass);
        //    return ln.InitReqContAdjust(ref contadj);
        //}

        //[WebMethod]
        //public int SaveReqContAdjust(String wsPass, str_lncontaj contadj)
        //{
        //    ShrlonSvEn ln = new ShrlonSvEn(wsPass);
        //    return ln.SaveReqContAdjust(contadj);
        //}
        [WebMethod]
        public int InitReqContAdjust(ref str_lncontaj contadj, String wsPass, string loancontract_no,
            DateTime contaj_date,
            string xml_contdetail,
            string xml_contpayment,
            string xml_contcoll,
            string xml_contint,
            string xml_contintspc,
            string entry_id,
            string branch_id, string init_fixed)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitReqContAdjust(ref contadj, loancontract_no, contaj_date, xml_contdetail, xml_contpayment, xml_contcoll, xml_contint, xml_contintspc, entry_id, branch_id, init_fixed);
        }

        [WebMethod]
        public int SaveReqContAdjust(String wsPass, string loancontract_no,
            DateTime contaj_date,
            string xml_contdetail,
            string xml_contpayment,
            string xml_contcoll,
            string xml_contint,
            string xml_contintspc,
            string entry_id,
            string branch_id, string init_fixed)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveReqContAdjust(loancontract_no, contaj_date, xml_contdetail, xml_contpayment, xml_contcoll, xml_contint, xml_contintspc, entry_id, branch_id, init_fixed);
        }

        [WebMethod]
        public int InitLncollMastAll(String wsPass, ref str_lncollmast strlncoll, String member_no, String xml_memdet, String xml_collmastlist)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);

            return ln.InitLncollMastAll(ref strlncoll, member_no, xml_memdet, xml_collmastlist);
        }
        [WebMethod]
        public int InitLncollMastDet(String wsPass, ref str_lncollmast strlncoll, String collmast_no, String xml_collmastdet, String xml_collmemco, String xml_mrtg1, String xml_mrtg2, String xml_mrtg3)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitLncollMastDet(ref strlncoll, collmast_no, xml_collmastdet, xml_collmemco, xml_mrtg1, xml_mrtg2, xml_mrtg3);
        }


        [WebMethod]
        public String InitCancleResign(String wsPass, String as_member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitCancleResign(as_member_no);
        }

        [WebMethod]
        public int SaveCancelResign(String wsPass, String as_xmlcancel, String as_cancelid, DateTime adtm_cancel)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveCancelResign(as_xmlcancel, as_cancelid, adtm_cancel);
        }

        [WebMethod]
        public int InitRequestChangeMb(String wsPass, ref str_adjust_mbinfo lstr_mbinfo, String member_no, String ls_xmlmaster, String ls_xmlmbdet, String ls_xmlmbstatus, String ls_xmlmoneytr, String ls_xmlremarkstat)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitRequestChangeMb(ref lstr_mbinfo, member_no, ls_xmlmaster, ls_xmlmbdet, ls_xmlmbstatus, ls_xmlmoneytr, ls_xmlremarkstat);
        }
        [WebMethod]
        public int SaveRequestChangeMb(String wsPass, ref str_adjust_mbinfo lstr_mbinfo, String appname, DateTime operate_date, String userid, String member_no, String ls_xmlmaster, String ls_xmlmbdet, String ls_xmlmbstatus, String ls_xmlmoneytr, String ls_xmlremarkstat, String ls_xmlbfmaster, String ls_xmlbfmbdet, String ls_xmlbfmbstatus, String ls_xmlbfmoneytr, String ls_xmlbfremarkstat)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveRequestChangeMb(ref lstr_mbinfo, appname, operate_date, userid, member_no, ls_xmlmaster, ls_xmlmbdet, ls_xmlmbstatus, ls_xmlmoneytr, ls_xmlremarkstat, ls_xmlbfmaster, ls_xmlbfmbdet, ls_xmlbfmbstatus, ls_xmlbfmoneytr, ls_xmlbfremarkstat);
        }

        [WebMethod]
        public int InitRequestChangeGroup(String wsPass, String as_memno, ref String as_xmlreq, ref String as_xmlhistory)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitRequestChangeGroup(as_memno, ref as_xmlreq, ref as_xmlhistory);
        }

        [WebMethod]
        public int SaveRequestChangeGroup(String wsPass, String as_xmlreq, String as_entryid, DateTime adtm_entrydate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveRequestChangeGroup(as_xmlreq, as_entryid, adtm_entrydate);
        }

        [WebMethod]
        public String InitApvChangeGrouplist(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitApvChangeGrouplist();
        }

        [WebMethod]
        public int SaveApvChangeGroup(String wsPass, String xmlReqList, String apvId, DateTime workDate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveApvChangeGroup(xmlReqList, apvId, workDate);
        }

        //prazit
        [WebMethod]
        public String InitLnContPayCriteria(String wsPass, ref str_paytab astr_paytab)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitLnContPayCriteria(ref astr_paytab);
        }

        //prazit
        [WebMethod]
        public String InitLnContPaytable(String wsPass, ref str_paytab astr_paytab)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitLnContPaytable(ref astr_paytab);
        }

        [WebMethod]
        public decimal CheckRightColl(string wsPass, string as_memno, string as_loantype, DateTime adtm_operate, string as_colltype, string as_refcollno, string as_contclear, short ai_period, Boolean ab_change, ref str_checkrightcoll astr_checkrightcoll)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CheckRightColl(as_memno, as_loantype, adtm_operate, as_colltype, as_refcollno, as_contclear, ai_period, ab_change, ref astr_checkrightcoll);
        }

        //Doys.
        [WebMethod]
        public str_slippayout InitSlipMoneyRet(String wsPass, str_slippayout strSlipPayOut)
        {
            return new ShrlonSvEn(wsPass).InitSlipMoneyRet(strSlipPayOut);
        }

        //Doys.
        [WebMethod]
        public int SaveSlipMoneyRet(String wsPass, str_slippayout strSlipPayOut)
        {
            return new ShrlonSvEn(wsPass).SaveSlipMoneyRet(strSlipPayOut);
        }
        //Boom
        [WebMethod]
        public short ItemChangeCheckColl(String wsPass, ref str_itemchangecheckcoll strCheckColl)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.ItemChangeCheckColl(ref strCheckColl);
        }
        //may ประมวลผลยืนยันยอด
        [WebMethod]
        public int RunCfbalProcess(String wsPass, String ls_xmlbalcriteria, String ls_entryid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlCfbalProcess sl = new SlCfbalProcess(sec.ConnectionString, ls_xmlbalcriteria, ls_entryid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //may ประมวลผลหุ้นของขวัญ
        [WebMethod]
        public int RunShgiftProcess(String wsPass, String ls_xmlbalcriteria, String ls_entryid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlShrgiftProcess sl = new SlShrgiftProcess(sec.ConnectionString, ls_xmlbalcriteria, ls_entryid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //may ประมวลผลเปลี่ยนอัตราดอกเบี้ย
        [WebMethod]
        public int RunIntchgProcess(String wsPass, String as_xmlintsetcriteria, String as_entryid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlIntchgProcess sl = new SlIntchgProcess(sec.ConnectionString, as_xmlintsetcriteria, as_entryid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //may ตัดยอดหุ้นของขวัญ
        [WebMethod]
        public int RunPostShrgiftProcess(String wsPass, String as_xmlgiftcriteria, String as_postid, String as_branchid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlPostShrgiftProcess sl = new SlPostShrgiftProcess(sec.ConnectionString, as_xmlgiftcriteria, as_postid, as_branchid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }



        //may แก้ไข/ยกเลิกหุ้นของขวัญ
        [WebMethod]
        public str_shrgift InitChageShrgift(String wsPass, str_shrgift strShrgift)
        {
            return new ShrlonSvEn(wsPass).InitChageShrgift(strShrgift);
        }

        //may บันทึกแก้ไข/ยกเลิกหุ้นของขวัญ
        [WebMethod]
        public str_shrgift SaveChageShrgift(String wsPass, str_shrgift strShrgift)
        {
            return new ShrlonSvEn(wsPass).SaveChageShrgift(strShrgift);
        }

        //may จัดทำหนี้สั้นยาว
        [WebMethod]
        public int RunLnShotLongProcess(String wsPass, String as_xmlintsetcriteria, String as_userid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlLnshortlongProcess sl = new SlLnshortlongProcess(sec.ConnectionString, as_xmlintsetcriteria, as_userid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //may ปิดสิ้นวัน
        [WebMethod]
        public int RunClosedayProcess(String wsPass, DateTime adtm_closeday, String as_appname, String as_userid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlClosedayProcess sl = new SlClosedayProcess(sec.ConnectionString, adtm_closeday, as_appname, as_userid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }


        //may ปิดสิ้นเดือน
        [WebMethod]
        public int RunClosemonthProcess(String wsPass, short ai_year, short ai_month, String as_appname, String as_userid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlClosemonthProcess sl = new SlClosemonthProcess(sec.ConnectionString, ai_year, ai_month, as_appname, as_userid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //may ปิดสิ้นปี
        [WebMethod]
        public int RunCloseyearProcess(String wsPass, short ai_year, String as_branch, String as_entryid, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlCloseyearProcess sl = new SlCloseyearProcess(sec.ConnectionString, ai_year, as_branch, as_entryid);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //lex.ขอผ่อนผัน
        [WebMethod]
        public str_compound InitReqCompound(String wsPass, ref str_compound strCompound)
        {
            return new ShrlonSvEn(wsPass).InitReqCompound(ref strCompound);
        }

        //lex.บันทึกขอผ่อนผัน
        [WebMethod]
        public int SaveReqCompound(String wsPass, ref str_compound strCompound)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveReqCompound(ref strCompound);
        }


        [WebMethod]
        public int InitLnPause(String wsPass, ref str_lnpause strlnpause)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitLnPause(ref strlnpause);
        }

        [WebMethod]
        public int SaveLnPause(String wsPass, ref str_lnpause strlnpause)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveLnPause(ref strlnpause);
        }

        [WebMethod]
        public int InitShareWithdraw(String wsPass, ref str_slippayout strslippayout)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitShareWithdraw(ref strslippayout);
        }
        [WebMethod]
        public String InitShareWithdrawList(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitShareWithdrawList();
        }
        [WebMethod]
        public int PostShareWithdraw(String wsPass, ref str_slippayout strslippayout)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.PostShareWithdraw(ref strslippayout);
        }
        [WebMethod]
        public int InitRequestResign(String wsPass, ref str_mbreqresign mbreqresign, String member_no, DateTime entry_date, String entry_id, String xml_dept, String xml_grt, String xml_loan, String xml_request, String xml_share, String xml_sum)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitRequestResign(ref mbreqresign, member_no, entry_date, entry_id, xml_dept, xml_grt, xml_loan, xml_request, xml_share, xml_sum);


        }
        [WebMethod]
        public int SaveRequestResign(String wsPass, str_mbreqresign mbreqresign, String member_no, DateTime entry_date, String entry_id, String xml_dept, String xml_grt, String xml_loan, String xml_request, String xml_share, String xml_sum)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveRequestResign(mbreqresign, member_no, entry_date, entry_id, xml_dept, xml_grt, xml_loan, xml_request, xml_share, xml_sum);


        }
        [WebMethod]
        public int PostApvListResign(String wsPass, String as_xmlreqlist, String as_apvid, DateTime adtm_apvdate)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.PostApvListResign(as_xmlreqlist, as_apvid, adtm_apvdate);


        }
        [WebMethod]
        public String InitApvListResign(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitApvListResign();

        }

        [WebMethod]
        public String InitRequestShrPayment(String wsPass, String as_memno)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitRequestShrPayment(as_memno);

        }

        [WebMethod]
        public int SaveRequestShrPayment(String wsPass, String as_xmlreq, String as_entry, DateTime adtm_entrydate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveRequestShrPayment(as_xmlreq, as_entry, adtm_entrydate);

        }
        [WebMethod]
        public String InitApvlistShrPayment(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitApvlistShrPayment();

        }

        [WebMethod]
        public int PostApvlistShrPayment(String wsPass, String as_xmlreq, String as_entry, DateTime adtm_entrydate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.PostApvlistShrPayment(as_xmlreq, as_entry, adtm_entrydate);

        }
        //a
        //init master ยกเลิกถอนหุ้น
        [WebMethod]
        public int InitCancelSwd(String wsPass, ref str_slipcancel astr_cancelslip)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitCancelSwd(ref astr_cancelslip);

        }
        //a
        //init detail ยกเลิกถอนหุ้น
        [WebMethod]
        public int InitCancelSwdDet(String wsPass, ref str_slipcancel astr_cancelslip)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitCancelSwdDet(ref astr_cancelslip);
        }
        //a
        //save ยกเลิกถอนหุ้น
        [WebMethod]
        public int SaveCancelSwdDet(String wsPass, str_slipcancel astr_cancelslip)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);

            return ln.SaveCancelSwdDet(astr_cancelslip);

        }
        //Boom
        [WebMethod]
        public short InitReqReturn(String wsPass, ref String xml_head, ref String xml_detail, ref String xml_message)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitReqReturn(ref xml_head, ref xml_detail, ref xml_message);
        }

        //Boom
        [WebMethod]
        public short OpenReqReturn(String wsPass, String request_docno, ref String xml_head, ref String xml_detail, ref String xml_message)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.OpenReqReturn(request_docno, ref xml_head, ref xml_detail, ref xml_message);
        }
        //Boom
        [WebMethod]
        public short CancelReqRetrun(String wsPass, ref String xml_head, ref String xml_message, String cancelID, String branchID)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CancelReqReturn(ref xml_head, ref xml_message, cancelID, branchID);
        }
        //Boom
        [WebMethod]
        public short SaveReqReturn(String wsPass, String xml_head, String xml_detail, String entryID, String branchID)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveReqReturn(xml_head, xml_detail, entryID, branchID);
        }

        [WebMethod]
        public int SaveLncollMast(String wsPass, ref str_lncollmast lstr_lncollmast)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveLncollMast(ref lstr_lncollmast);
        }
        //Mai
        // function ประมวลผลปันผล
        [WebMethod]
        public int Caldiv(String wsPass, String xmlprocinfo, String xmlloantype)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CalDivProcess(xmlprocinfo, xmlloantype);
        }

        //Mai
        // function ประมาลผลปันผล
        [WebMethod]
        public String Estimate(String wsPass, String xmlprocinfo, String xmlloantype)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.EstmateProcess(xmlprocinfo, xmlloantype);
        }

        //Mai
        //function Init คีย์ข้อมูลหักปันผล - เฉลี่ยคืน
        [WebMethod]
        public String[] InitDivMethodPM(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitDivMethodPM(div_year, member_no);
        }

        //Mai
        //function Save คีย์ข้อมูลหักปันผล - เฉลี่ยคืน
        [WebMethod]
        public int SaveDivMethodPM(String wsPass, String xml_head, String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveDivMethodPM(xml_head, xml_detail);
        }

        //Mai
        //function Init อายัดปันผล - เฉลี่ยคืน
        [WebMethod]
        public String[] InitDivSequest(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitDivSequest(div_year, member_no);
        }

        //Mai
        //function Save อายัดปันผล - เฉลี่ยคืน
        [WebMethod]
        public int SaveDivSequest(String wsPass, String xml_head, String xml_detail, String entry_id)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveDivSequest(xml_head, xml_detail, entry_id);
        }

        //Mai
        //function Init คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด (ค่าเริ่มต้น)
        [WebMethod]
        public String InitListPaydivavgMain(String wsPass)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitListPaydivavgMain();
        }

        //Mai
        //function Init คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด
        [WebMethod]
        public String InitListPaydivavgDetail(String wsPass, String xml_head)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitListPaydivavgDetail(xml_head);
        }

        //Mai
        //function Save คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด
        [WebMethod]
        public int SaveListPayDivAvg(String wsPass, String xml_list, String entry_id, String branch_id, String tofrom_accid, DateTime operate_date, DateTime slip_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveListPayDivAvg(xml_list, entry_id, branch_id, tofrom_accid, operate_date, slip_date);
        }

        //Mai
        //function Init คีย์จ่ายปันผล - เฉลี่ยคืนตามทะเบียน
        [WebMethod]
        public String[] InitSlipPayOut(String wsPass, String payoutslip_no, String div_year, String member_no, DateTime slip_date, DateTime operate_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitSlipPayOut(payoutslip_no, div_year, member_no, slip_date, operate_date);
        }

        //Mai
        //function Save คีย์จ่ายปันผล - เฉลี่ยคืนตามทะเบียน
        [WebMethod]
        public int SaveSlipPayOut(String wsPass, String xml_head, String xml_detail, String entry_id, String branch_id, DateTime operate_date, DateTime slip_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveSlipPayOut(xml_head, xml_detail, entry_id, branch_id, operate_date, slip_date);
        }

        //Mai
        //function Init งดเฉลี่ยคืน
        [WebMethod]
        public String[] InitDivDropAverage(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitDivDropAverage(div_year, member_no);
        }

        //Mai
        //function Save งดเฉลี่ยคืน
        [WebMethod]
        public int SaveDivDropAverage(String wsPass, String xml_head, String xml_detail, String entry_id)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveDivDropAverage(xml_head, xml_detail, entry_id);
        }

        //Mai
        //function Init ยกเลิกจ่ายปันผลเฉลี่ยคืน
        [WebMethod]
        public String[] InitCancelSlipPayOut(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitCancelSlipPayOut(div_year, member_no);
        }

        //Mai
        //function Save ยกเลิกจ่ายปันผลเฉลี่ยคืน
        [WebMethod]
        public int SaveCancelSlipPayOut(String wsPass, String xml_head, String xml_detail, String entry_id, DateTime slip_date)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveCancelSlipPayOut(xml_head, xml_detail, entry_id, slip_date);
        }

        //Mai
        //function Search ค้นหาสมาชิก
        [WebMethod]
        public String SearchList(String wsPass, String xml_searchdata)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SearchList(xml_searchdata);
        }

        //Mai
        ////function InitDetail รายละเอียดปันผล
        [WebMethod]
        public String InitListDivAvgDetail(String wsPass, String div_year, String member_no, String dataobject)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitListDivAvgDetail(div_year, member_no, dataobject);
        }

        //Mai
        //function InitList รายละเอียดปันผล
        [WebMethod]
        public int InitListDivAvg(String wsPass, ref str_divavg astr_divavg, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitListDivAvg(ref astr_divavg, div_year, member_no);
        }


        //Mai
        //function Search ค้นหาใบทำรายการ
        [WebMethod]
        public String SearchListSlipPayOut(String wsPass, String xml_searchdata)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SearchListSlipPayOut(xml_searchdata);
        }
        //MAI
        //progressbar ประมวลปันผล
        [WebMethod]
        public int RunCalDivAvgProcess(String wsPass, String xml_procinfo, String xml_loantype, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            DivAvgProcess sl = new DivAvgProcess(sec.ConnectionString, xml_procinfo, xml_loantype);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }
        //MAI
        //progressbar ประมาณปันผล
        [WebMethod]
        public int RunEstDivAvgProcess(String wsPass, String xml_procinfo, String xml_loantype, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            EstDivAvgProcess sl = new EstDivAvgProcess(sec.ConnectionString, xml_procinfo, xml_loantype);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        //Mai
        //function Search ค้นหาธนาคาร
        [WebMethod]
        public int SearchListBank(String wsPass, ref str_divavg astr_divavg, String xml_head)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SearchListBank(ref astr_divavg, xml_head);
        }

        ////Mai
        ////function Search ค้นหาสาขาธนาคาร
        [WebMethod]
        public int SearchListBranch(String wsPass, ref str_divavg astr_divavg, String xml_head, String bank_code)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SearchListBranch(ref astr_divavg, xml_head, bank_code);
        }

        ////Mai
        ////function Search ค้นหาสังกัด
        [WebMethod]
        public int SearchListMemGroup(String wsPass, ref str_divavg astr_divavg, String xml_head)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SearchListMemGroup(ref astr_divavg, xml_head);
        }

        //Mai
        //function Init ผิดนัดชำระหนี้
        [WebMethod]
        public String[] InitMissPayLoan(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitMissPayLoan(div_year, member_no);
        }

        //Mai
        //function Save ผิดนัดชำระหนี้
        [WebMethod]
        public int SaveMissPayLoan(String wsPass, String xml_head, String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.SaveMissPayLoan(xml_head, xml_detail);
        }

        //Mai
        //function Init แสดงรายละเอียดปันผล เฉลี่ยคืน
        [WebMethod]
        public String InitDivavgDetail(String wsPass, String div_year, String member_no)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.InitDivavgDetail(div_year, member_no);
        }

        //Mai
        //function CalDivavgDetail คำนวณปันผล
        [WebMethod]
        public String[] CalDivavgDetail(String wsPass, String xml_head, String xml_detail)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.CalDivavgDetail(xml_head, xml_detail);
        }

        //may ประมวลผลตั้งค่าคีย์ปันผลเฉลี่ยคืน
        [WebMethod]
        public int RunDividendProcess(String wsPass, str_divavg strDivAvg, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SlDivAvgProcess sl = new SlDivAvgProcess(sec.ConnectionString, strDivAvg);
            return Processing.Progressing.Add(sl, application, w_sheet_id, true);
        }

        #region ฟังชั่นใหม่สำหรับหน้าจอตารางชำระเท่านั้น @ 27/11/2553

        [WebMethod]
        public void GenPeriodPayTab(String wsPass, ref str_genperiodpaytab astr_paytab)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            ln.GenPeriodPayTab(ref astr_paytab);
        }

        #endregion

        //Doys
        [WebMethod]
        public int InitSlipAdjust(String wsPass, ref str_slipadjust slipAdjust)
        {
            return new ShrlonSvEn(wsPass).InitSlipAdjust(ref slipAdjust);
        }

        //Doys
        [WebMethod]
        public int SaveSlipAdjust(String wsPass, ref str_slipadjust slipAdjust)
        {
            return new ShrlonSvEn(wsPass).SaveSlipAdjust(ref slipAdjust);
        }
        //เอ  เช็คหุ้นตามฐานเงินเดือน
        [WebMethod]
        public int GetShareBase(String wsPass, Decimal adc_salary, ref Decimal adc_minshare, ref  Decimal adc_maxshare)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.GetShareBase(adc_salary, ref adc_minshare, ref adc_maxshare);


        }

        //Boom 
        [WebMethod]
        public void InitEstTrnColl(String wsPass, ref str_esttrncoll strEsttrnColl)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            ln.InitEstTrnColl(ref strEsttrnColl);
        }
        //a 
        // init carmast
        [WebMethod]
        public int of_initcarmast(String wsPass, ref str_carmast astr_carmast)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initcarmast(ref astr_carmast);


        }
        //a
        // init carmastdet
        [WebMethod]
        public int of_initcarmastdet(String wsPass, ref str_carmast astr_carmast)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initcarmastdet(ref astr_carmast);

        }
        //a
        //save carmast
        [WebMethod]
        public int of_savecarmast(String wsPass, ref str_carmast astr_carmast)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_savecarmast(ref astr_carmast);

        }
        //Boom 
        [WebMethod]
        public String GetDataKeeping(String wsPass, String memberNo, String recvPeriod)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.GetDataKeeping(memberNo, recvPeriod);
        }

        //Doys
        [WebMethod]
        public StructLoanRequest InitMemberNoLoanRequestDotNet(String wsPass, String application, String pbl, String memberNo, String xmlDwMain)
        {
            return new ShrlonDotNetSvEnCs(wsPass).InitMemberNo(pbl, application, memberNo, xmlDwMain);
        }

        //May  ปริ๊นท์ ทด.
        [WebMethod]
        public String OfPrintColl(String wsPass, String as_reftype, String as_mastno, String as_formSet)
        {
            String[] arg = new String[3];
            arg[0] = as_reftype;
            arg[1] = as_mastno;
            arg[2] = as_formSet;
            return new WinPrintCalling(wsPass).CallWinPrint("shrlon", "PrintColl", arg);
        }

        //nok
        [WebMethod]
        public string PrintSlipPayin(String wsPass, string application, String slip_no, string formset)
        {
            String[] ss = new String[2];
            ss[0] = slip_no;
            ss[1] = formset;
            return new WinPrintCalling(wsPass).CallWinPrint(application, "PrintSlipPayin", ss);
        }

        //Doys
        [WebMethod]
        public String PrintContract(String wsPass, String application, String reqNo, String refType, String printSet)
        {
            String[] arg = new String[3];
            arg[0] = reqNo;
            arg[1] = refType;
            arg[2] = printSet;
            return new WinPrintCalling(wsPass).CallWinPrint(application, "PrintContract", arg);
        }

        //ed
        [WebMethod]
        public String OfInitListInRcv(String wsPass, String as_moneytype, DateTime adtm_order)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initlist_lnrcv(as_moneytype, adtm_order);
        }

        //ed
        [WebMethod]
        public Int16 OfInitLnRcv(String wsPass, ref str_slippayout astr_slippayout)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initlnrcv(ref astr_slippayout);
        }

        //ed
        [WebMethod]
        public Int16 OfInitLnRcvReCalInt(String wsPass, ref str_slippayout astr_slippayout)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initlnrcv_recalint(ref astr_slippayout);

        }

        //ed
        [WebMethod]
        public Int16 OfSaveOrdInRcv(String wsPass, ref str_slippayout astr_slippayout)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_saveord_lnrcv(ref astr_slippayout);

        }



        //ed
        [WebMethod]
        public String OfInitListInRcvFin(String wsPass, String as_moneytype, DateTime adtm_paydate)
        {
            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initlist_lnrcv_fin(as_moneytype, adtm_paydate);
        }

        //ed
        [WebMethod]
        public Int16 OfInitLnRcvFin(String wsPass, ref str_slippayout astr_slippayout)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_initlnrcv_fin(ref astr_slippayout);
        }

        //ed
        [WebMethod]
        public Int16 OfSaveOrdInRcvFin(String wsPass, ref str_slippayout astr_slippayout)
        {

            ShrlonSvEn ln = new ShrlonSvEn(wsPass);
            return ln.of_saveslip_lnrcv_fin(ref astr_slippayout);

        }
    }
}
