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

    public class ShrlonSvEn
    {
        private Security security;
        private n_cst_loansrv_lnfinance svLnFin;
        private n_cst_loansrv_lnorder svLnOrder;
        private n_cst_dbconnectservice svCon;
        private n_cst_loansrv_lnoperate svLoan;
        private n_cst_sharesrv_shroperate svShare;
        private n_cst_mb_memb_service svMemb;
        private n_cst_loansrv_loanright svLoanRigth;
        private n_cst_sh_share_service svSh;
        private n_cst_mbreq_resign svMembRG;
        private n_cst_adjust_memberinfo svmbinfo;
        private n_cst_mbreq_change_group svmbgroup;
        private n_cst_mbreq_adjust_share svMbAdjShr;
        private n_cst_shproc_shrgift svShrgift;
        private n_cst_dividend_service svCalEstdiv;
        private n_cst_divavgoperate_service svDivavg;
        private n_cst_loansrv_carmast svCarmast;
        public ShrlonSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public ShrlonSvEn(String wsPass, bool autoConnect)
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
                svLoan = new n_cst_loansrv_lnoperate();
                svLoan.of_initservice(svCon);
                svShare = new n_cst_sharesrv_shroperate();
                svShare.of_initservice(svCon);
                svMemb = new n_cst_mb_memb_service();
                svMemb.of_initservice(svCon);
                svLoanRigth = new n_cst_loansrv_loanright();
                svLoanRigth.of_initservice(svCon);
                svSh = new n_cst_sh_share_service();
                svSh.of_initservice(svCon);
                svMembRG = new n_cst_mbreq_resign();
                svMembRG.of_initservice(svCon);
                svmbinfo = new n_cst_adjust_memberinfo();
                svmbinfo.of_initservice(svCon);
                svmbgroup = new n_cst_mbreq_change_group();
                svmbgroup.of_initservice(svCon);
                svMbAdjShr = new n_cst_mbreq_adjust_share();
                svMbAdjShr.of_initservice(svCon);
                svLnOrder = new n_cst_loansrv_lnorder();
                svLnOrder.of_initservice(svCon);
                svLnFin = new n_cst_loansrv_lnfinance();
                svLnFin.of_initservice(svCon);

                svCalEstdiv = new n_cst_dividend_service();
                svCalEstdiv.of_initservice(svCon);

                svDivavg = new n_cst_divavgoperate_service();
                svDivavg.of_initservice(svCon);

                svShrgift = new n_cst_shproc_shrgift();
                svShrgift.of_initservice(svCon);
                svCarmast = new n_cst_loansrv_carmast();
                svCarmast.of_initservice(svCon);
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

        ~ShrlonSvEn()
        {
            DisConnect();
        }
        //---------------------------------------------------//

        public String ApvNewMember(String appl, String mem)
        {
            try
            {
                // bask
                svMemb.of_posttomembmaster(appl, mem);
                svMemb.of_posttosharemaster(appl, mem);
                svMemb.of_posttomembexpense(appl, mem);
                DisConnect();
                return "0";
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitApvNewMemberList()
        {
            try
            {
                //  bask
                String result = svMemb.of_initlist_mbreqapv();
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] InitSlipPayIn(String ls_memno, String ls_sliptype, DateTime ldtm_slipdate, DateTime ldtm_opedate, String ls_entry_id, String ls_branch_id)
        {
            try
            {
                str_slippayin strxml = new str_slippayin();
                strxml.entry_id = ls_entry_id;
                strxml.branch_id = ls_branch_id;
                strxml.member_no = ls_memno;
                strxml.sliptype_code = ls_sliptype;
                strxml.slip_date = ldtm_slipdate;
                strxml.operate_date = ldtm_opedate;

                int result = 0;
                String[] arr = new String[4];
                try
                {
                    result = svLoan.of_initslippayin(ref strxml);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = strxml.xml_sliphead;
                    arr[1] = strxml.xml_slipshr;
                    arr[2] = strxml.xml_sliplon;
                    arr[3] = strxml.xml_slipetc;

                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitSlipPayInCalInt(String as_xmlloan, String as_sliptype, DateTime datecal)
        {
            try
            {
                try
                {
                    svLoan.of_initslippayin_calint(ref as_xmlloan, as_sliptype, datecal);
                    this.DisConnect();
                }
                catch (Exception ex)
                {
                    this.DisConnect();
                    throw ex;
                }
                DisConnect();
                return as_xmlloan;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String SaveSlipPayIn(String xml_sliphead, String xml_slipshr, String xml_sliplon, String xml_slipetc, DateTime ldtm_opedate, String ls_entry_id, String ls_branch_id)
        {
            try
            {
                str_slippayin strslip = new str_slippayin();
                strslip.xml_sliphead = xml_sliphead;
                strslip.xml_slipshr = xml_slipshr;
                strslip.xml_sliplon = xml_sliplon;
                strslip.xml_slipetc = xml_slipetc;
                strslip.entry_id = ls_entry_id;
                strslip.branch_id = ls_branch_id;
                int ii = svLoan.of_saveslip_payin(ref strslip);
                if (ii != 1)
                {
                    throw new Exception(" บันทึกไม่สำเร็จ<br>");
                }
                DisConnect();
                return strslip.xml_sliphead;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String Initlist_lnreqapv(String as_loantype_code)
        {
            try
            {
                String result = svLoan.of_initlist_lnreqapv(as_loantype_code);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetLastKeeping(String xml_main)
        {
            try
            {

                String result = svLoanRigth.of_get_lastkeeping(xml_main);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String GetNewContractNo(String as_loantype)
        {
            try
            {
                String re = svLoan.of_getnewcontractno(as_loantype);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String SaveLnReqApv(String as_xmlreqlist, String as_apvid,String as_loantype_code)
        {
            try
            {
                svLoan.of_saveapv_lnreq(ref as_xmlreqlist, as_apvid, as_loantype_code);
                DisConnect();
                return as_xmlreqlist;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int LoanRightItemChangeMain(ref str_itemchange setitem)
        {
            try
            {
                int re = svLoanRigth.of_itemchangemain(ref setitem);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int LoanRigthItemChangeColl(ref str_itemchange setitem)
        {
            try
            {
                int re = svLoanRigth.of_itemchangecoll(ref setitem);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int LoanRightItemChangeClear(ref str_itemchange setitem)
        {
            try
            {
                int re = svLoanRigth.of_itemchangeclr(ref setitem);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String LoanRightSaveReqloan(str_savereqloan strSave)
        {
            try
            {
                String result = svLoanRigth.of_savereqloan(strSave);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public str_lntrncoll InitLnTrnColl(str_lntrncoll str)
        {
            try
            {
                svLoan.of_initreq_lntrncoll(ref str);
                DisConnect();
                return str;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public bool SaveLnTrnColl(str_lntrncoll str)
        {
            try
            {
                svLoan.of_savereq_lntrncoll(ref str);
                DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String[] InitLnTrnCollRecalTrn(String xmlmast, String xmldetail)
        {
            try
            {
                String[] strXML = new String[2];
                svLoan.of_initreq_lntrncollrecaltrn(ref xmlmast, ref xmldetail);

                strXML[0] = xmlmast;
                strXML[1] = xmldetail;
                DisConnect();
                return strXML;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveMBreqApv(String as_xmlreqlist, String as_userid)
        {
            try
            {
                int re = svMemb.of_savembreqapv(as_xmlreqlist, as_userid);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        public str_slippayout InitLnRcv(str_slippayout strSlipPayOut)
        {
            try
            {
                svLoan.of_initlnrcv(ref strSlipPayOut);
                DisConnect();
                return strSlipPayOut;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitListLnRcv(String as_moneytype)
        {
            try
            {
                String result = svLoan.of_initlist_lnrcv(as_moneytype);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public str_slippayout InitLnRcv_RecalInt(str_slippayout strSlipPayOut)
        {
            try
            {
                svLoan.of_initlnrcv_recalint(ref strSlipPayOut);
                DisConnect();
                return strSlipPayOut;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveLnRcv(str_slippayout strSlipPayOut)
        {
            try
            {
                int result;
                result = svLoan.of_saveslip_lnrcv(ref strSlipPayOut);
                DisConnect();
                return result;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        public str_requestopen LoanRequestOpen(str_requestopen strRequestOpen)
        {
            try
            {
                svLoanRigth.of_loanrequestopen(ref strRequestOpen);
                DisConnect();
                return strRequestOpen;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //a
        public int InitcclSlippayinAll(ref str_slipcancel slipcancle, String member_no, String xml_memdet, String xml_sliplist)
        {
            try
            {
                slipcancle.member_no = member_no;
                slipcancle.xml_memdet = xml_memdet;
                slipcancle.xml_sliplist = xml_sliplist;
                int re = svLoan.of_initccl_slippayinall(ref slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //a
        public int InitcclSlippayinDet(ref str_slipcancel slipcancle, String slip_no, String xml_sliphead, String xml_slipdetail)
        {
            try
            {
                slipcancle.slip_no = slip_no;
                slipcancle.xml_sliphead = xml_sliphead;
                slipcancle.xml_slipdetail = xml_slipdetail;
                int re = svLoan.of_initccl_slippayindet(ref slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //a
        public int SavecclPayIn(String xml_sliphead, String xml_slipdetail, String slip_no, String cancel_id, DateTime cancel_date)
        {
            try
            {
                str_slipcancel slipcancle = new str_slipcancel();
                slipcancle.xml_sliphead = xml_sliphead;
                slipcancle.xml_slipdetail = xml_slipdetail;
                slipcancle.slip_no = slip_no;
                slipcancle.cancel_id = cancel_id;
                slipcancle.cancel_date = cancel_date;
                int re = svLoan.of_saveccl_payin(slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //a
        public Decimal GetShareMonthRate(String as_sharetype, Decimal adc_salary)
        {
            try
            {
                decimal re = svSh.of_getsharemonthrate(as_sharetype, adc_salary);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //a
        public Boolean IsvalidIdCard(String as_idcard)
        {
            try
            {
                svMemb.of_isvalididcard(as_idcard);
                Boolean result = svMemb.of_isvalididcard(as_idcard);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //a
        public DateTime CalReTryDate(DateTime adtm_birthdate)
        {
            try
            {
                DateTime retry_date = svMemb.of_calretrydate(adtm_birthdate);
                DisConnect();
                return retry_date;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String ReCalFee(String xml_main)
        {
            try
            {
                String re = svLoanRigth.of_recalfee(xml_main);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String ReqLoopOpen(String xml_main)
        {
            try
            {
                String re = svLoanRigth.of_reqloopopen(xml_main);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String ItemChangeReqLoop(String xml_main, String xml_reqloop)
        {
            try
            {
                String re = svLoanRigth.of_itemchagereqloop(xml_main, xml_reqloop);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        public String InitReqContCancel(String as_contno)
        {
            try
            {
                String contno = svLoan.of_initreq_contcancel(as_contno);
                DisConnect();
                return contno;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int SaveReqContCancel(String as_xmlcontccl, String as_cancelid, DateTime adtm_cancel)
        {
            try
            {
                int re = svLoan.of_savereq_contcancel(as_xmlcontccl, as_cancelid, adtm_cancel);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int InitcclSlipLnrcvAll(ref str_slipcancel slipcancle, String member_no, String xml_memdet, String xml_sliplist)
        {
            try
            {
                slipcancle.member_no = member_no;
                slipcancle.xml_memdet = xml_memdet;
                slipcancle.xml_sliplist = xml_sliplist;
                int re = svLoan.of_initccl_sliplnrcvall(ref slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int InitcclSlipLnrcvDet(ref str_slipcancel slipcancle, String slip_no, String xml_sliphead, String xml_slipdetail)
        {
            try
            {
                slipcancle.slip_no = slip_no;
                slipcancle.xml_sliphead = xml_sliphead;
                slipcancle.xml_slipdetail = xml_slipdetail;
                int re = svLoan.of_initccl_sliplnrcvdet(ref slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int SavecclLnrcv(String xml_sliphead, String xml_slipdetail, String slip_no, String cancel_id, DateTime cancel_date)
        {
            try
            {
                str_slipcancel slipcancle = new str_slipcancel();
                slipcancle.xml_sliphead = xml_sliphead;
                slipcancle.xml_slipdetail = xml_slipdetail;
                slipcancle.slip_no = slip_no;
                slipcancle.cancel_id = cancel_id;
                slipcancle.cancel_date = cancel_date;
                int re = svLoan.of_saveccl_lnrcv(slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String SetSumOtherClr(String xml_main, String xml_otherclr)
        {
            try
            {
                String result = svLoanRigth.of_setsumotherclear(xml_main, xml_otherclr);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int CancelRequest(ref String xml_main, ref String xml_message, String cancle_id, String branch)
        {
            try
            {
                int result = svLoanRigth.of_cancelrequest(ref xml_main, ref xml_message, cancle_id, branch);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public void CreateMthPayTab(String xml_main, String xml_clear, ref String xml_head, ref  String xml_detail)
        {
            try
            {
                svLoanRigth.of_createmthpaytab(xml_main, xml_clear, ref xml_head, ref xml_detail);
                DisConnect();
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String CollInitPrecent(String xml_main, String xml_garantee)
        {

            try
            {
                String result = svLoanRigth.of_collinitpercent(xml_main, xml_garantee);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String CollPercCondition(String xml_main, String xml_garantee, ref String xml_message)
        {
            try
            {
                String result = svLoanRigth.of_collperccondition(xml_main, xml_garantee, ref xml_message);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int CancelReqLoop(ref String xml_main, String cancel_id, String branch)
        {
            //bask
            try
            {
                int result = svLoanRigth.of_cancelreqloop(ref xml_main, cancel_id, branch);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int CheckReqLoop(String loanType)
        {
            try
            {
                int result = svLoanRigth.of_checkreqloop(loanType);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int ViewCollDetail(String strData, DateTime request_date, string xml_guarantee, ref String xml_head, ref String xml_detail)
        {
            try
            {
                int result = svLoanRigth.of_viewcolldetail(strData, request_date, xml_guarantee, ref  xml_head, ref  xml_detail);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int ViewLoanClrDetail(String strData, String xml_clear, ref String xml_detail)
        {
            try
            {
                int result = svLoanRigth.of_viewloanclrdetail(strData, xml_clear, ref xml_detail);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int RegenLoanClear(ref String xml_main, ref String xml_clear, ref String xml_coll, ref String xml_message)
        {
            try
            {
                int result = svLoanRigth.of_regenloanclear(ref xml_main, ref xml_clear, ref xml_coll, ref xml_message);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        public int ResumLoanClear(ref String xml_main, ref String xml_clear, ref String xml_coll, String xml_loandetail, ref String xml_message)
        {
            try
            {
                int result = svLoanRigth.of_resumloanclear(ref xml_main, ref xml_clear, ref xml_coll, xml_loandetail, ref xml_message);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //public int InitReqContAdjust(ref str_lncontaj contadj)
        //{
        //    try
        //    {

        //        int re = svLoan.of_initreq_contadjust(ref contadj);
        //        DisConnect();
        //        return re;
        //    }
        //    catch (Exception ex)
        //    {
        //        DisConnect();
        //        throw ex;
        //    }
        //}

        //public int SaveReqContAdjust(str_lncontaj contadj)
        //{
        //    try
        //    {

        //        int re = svLoan.of_savereq_contadjust(contadj);
        //        DisConnect();
        //        return re;
        //    }
        //    catch (Exception ex)
        //    {
        //        DisConnect();
        //        throw ex;
        //    }
        //}

        public int InitReqContAdjust(ref str_lncontaj contadj, string loancontract_no,
    DateTime contaj_date,
    string xml_contdetail,
    string xml_contpayment,
    string xml_contcoll,
    string xml_contint,
    string xml_contintspc,
    string entry_id,
    string branch_id,string init_fixed)
        {
            try
            {
                //str_lncontaj contadj  = new str_lncontaj();
                contadj.loancontract_no = loancontract_no;
                contadj.contaj_date = contaj_date;
                contadj.xml_contdetail = xml_contdetail;
                contadj.xml_contpayment = xml_contpayment;
                contadj.xml_contcoll = xml_contcoll;
                contadj.xml_contint = xml_contint;
                contadj.xml_contintspc = xml_contintspc;
                contadj.entry_id = entry_id;
                contadj.branch_id = branch_id;
                contadj.init_fixed = init_fixed;
                int re = svLoan.of_initreq_contadjust(ref contadj);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveReqContAdjust(string loancontract_no,
   DateTime contaj_date,
   string xml_contdetail,
   string xml_contpayment,
   string xml_contcoll,
   string xml_contint,
   string xml_contintspc,
   string entry_id,
   string branch_id, string init_fixed)
        {
            try
            {
                str_lncontaj contadj = new str_lncontaj();
                contadj.loancontract_no = loancontract_no;
                contadj.contaj_date = contaj_date;
                contadj.xml_contdetail = xml_contdetail;
                contadj.xml_contpayment = xml_contpayment;
                contadj.xml_contcoll = xml_contcoll;
                contadj.xml_contint = xml_contint;
                contadj.xml_contintspc = xml_contintspc;
                contadj.entry_id = entry_id;
                contadj.branch_id = branch_id;
                contadj.init_fixed = "init_fixed";
                int re = svLoan.of_savereq_contadjust(contadj);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int InitLncollMastAll(ref str_lncollmast strlncoll, String member_no, String xml_memdet, String xml_collmastlist)
        {
            try
            {
                strlncoll.member_no = member_no;
                strlncoll.xml_memdet = xml_memdet;
                strlncoll.xml_collmastlist = xml_collmastlist;
                int re = svLoan.of_initlncollmastall(ref strlncoll);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int InitLncollMastDet(ref str_lncollmast strlncoll, String collmast_no, String xml_collmastdet, String xml_collmemco, String xml_mrtg1, String xml_mrtg2, String xml_mrtg3)
        {
            try
            {
                strlncoll.collmast_no = collmast_no;
                strlncoll.xml_collmastdet = xml_collmastdet;
                strlncoll.xml_collmemco = xml_collmemco;
                strlncoll.xml_mrtg1 = xml_mrtg1;
                strlncoll.xml_mrtg2 = xml_mrtg2;
                strlncoll.xml_mrtg3 = xml_mrtg3;
                int re = svLoan.of_initlncollmastdet(ref strlncoll);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int SaveLncollMast(ref str_lncollmast lstr_lncollmast)
        {
            try
            {

                int re = svLoan.of_savelncollmast(ref lstr_lncollmast);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public String InitCancleResign(String as_member_no)
        {
            try
            {
                String member_no = svMembRG.of_initcancleresign(as_member_no);
                DisConnect();
                return member_no;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int SaveCancelResign(String as_xmlcancel, String as_cancelid, DateTime adtm_cancel)
        {
            try
            {
                int re = svMembRG.of_savecancelresign(as_xmlcancel, as_cancelid, adtm_cancel);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //init แก้ไขข้อมูลสมาชิก
        public int InitRequestChangeMb(ref str_adjust_mbinfo lstr_mbinfo, String member_no, String ls_xmlmaster, String ls_xmlmbdet, String ls_xmlmbstatus, String ls_xmlmoneytr, String ls_xmlremarkstat)
        {
            try
            {
                lstr_mbinfo.member_no = member_no;
                lstr_mbinfo.xmlmaster = ls_xmlmaster;
                lstr_mbinfo.xmldetail = ls_xmlmbdet;
                lstr_mbinfo.xmlstatus = ls_xmlmbstatus;
                lstr_mbinfo.xmlmoneytr = ls_xmlmoneytr;
                lstr_mbinfo.xmlremarkstat = ls_xmlremarkstat;
                int re = svmbinfo.of_initrequest(ref lstr_mbinfo);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //save แก้ไขข้อมูลสมาชิก
        public int SaveRequestChangeMb(ref str_adjust_mbinfo lstr_mbinfo, String appname, DateTime operate_date, String userid, String member_no, String ls_xmlmaster, String ls_xmlmbdet, String ls_xmlmbstatus, String ls_xmlmoneytr, String ls_xmlremarkstat, String ls_xmlbfmaster, String ls_xmlbfmbdet, String ls_xmlbfmbstatus, String ls_xmlbfmoneytr, String ls_xmlbfremarkstat)
        {
            try
            {
                lstr_mbinfo.member_no = member_no;
                lstr_mbinfo.xmlmaster = ls_xmlmaster;
                lstr_mbinfo.xmldetail = ls_xmlmbdet;
                lstr_mbinfo.xmlstatus = ls_xmlmbstatus;
                lstr_mbinfo.xmlmoneytr = ls_xmlmoneytr;
                lstr_mbinfo.xmlremarkstat = ls_xmlremarkstat;
                lstr_mbinfo.xmlbfmaster = ls_xmlbfmaster;
                lstr_mbinfo.xmlbfdetail = ls_xmlbfmbdet;
                lstr_mbinfo.xmlbfstatus = ls_xmlbfmbstatus;
                lstr_mbinfo.xmlbfmoneytr = ls_xmlbfmoneytr;
                lstr_mbinfo.xmlbfremarkstat = ls_xmlbfremarkstat;
                int re = svmbinfo.of_saverequest(ref lstr_mbinfo);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int InitRequestChangeGroup(String as_memno, ref String as_xmlreq, ref String as_xmlhistory)
        {
            try
            {
                int result = svmbgroup.of_initrequest(as_memno, ref as_xmlreq, ref as_xmlhistory);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public int SaveRequestChangeGroup(String as_xmlreq, String as_entryid, DateTime adtm_entrydate)
        {
            try
            {
                int result = svmbgroup.of_saverequest(as_xmlreq, as_entryid, adtm_entrydate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public String InitApvChangeGrouplist()
        {
            return svmbgroup.of_initapvlist();
        }

        public int SaveApvChangeGroup(String xmlReqList, String apvId, DateTime workDate)
        {
            return svmbgroup.of_postapvlist(xmlReqList, apvId, workDate);
        }

        //prazit.
        public String InitLnContPayCriteria(ref str_paytab astr_paytab)
        {
            //return xml_criteria
            try
            {
                astr_paytab.xml_criteria = "";
                int result = svLoan.of_initlncontpaycriteria(ref astr_paytab);
                DisConnect();
                if (result == 1)
                {
                    return astr_paytab.xml_criteria;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //prazit.
        public String InitLnContPaytable(ref str_paytab astr_paytab)
        {
            //return xml_paytab
            try
            {
                int result = svLoan.of_initlncontpaytable(ref astr_paytab);
                DisConnect();
                if (result == 1)
                {
                    return astr_paytab.xml_paytab;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public decimal CheckRightColl(string as_memno,  string as_loantype, DateTime adtm_operate, string as_colltype, string as_refcollno, string as_contclear, short ai_period, Boolean ab_change, ref str_checkrightcoll str_checkrightcoll)
        {
            try
            {
                decimal result = svLoanRigth.of_checkrightcoll(as_memno,  as_loantype, adtm_operate, as_colltype, as_refcollno, as_contclear, ai_period, ab_change, ref str_checkrightcoll );
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Doys.
        public str_slippayout InitSlipMoneyRet(str_slippayout strSlipPayOut)
        {
            try
            {
                svLoan.of_initslipmoneyret(ref strSlipPayOut);
                DisConnect();
                return strSlipPayOut;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Doys.
        public int SaveSlipMoneyRet(str_slippayout strSlipPayOut)
        {
            try
            {
                int ii = svLoan.of_saveslip_moneyret(ref strSlipPayOut);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //Boom
        public short ItemChangeCheckColl(ref str_itemchangecheckcoll strCheckColl)
        {
            try
            {
                short result = svLoanRigth.of_itemchangecheckcoll(ref  strCheckColl);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //lex.ขอผ่อนผัน
        public str_compound InitReqCompound(ref str_compound strCompound)
        {
            try
            {
                svLoan.of_initreq_compound(ref strCompound);
                DisConnect();
                return strCompound;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //lex.บันทึกขอผ่อนผัน
        public int SaveReqCompound(ref str_compound strCompound)
        {
            try
            {
                int ii = svLoan.of_savereq_compound(ref strCompound);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int InitLnPause(ref str_lnpause strlnpause)
        {
            return svLoan.of_initreq_lnpause(ref strlnpause);
        }

        public int SaveLnPause(ref str_lnpause strlnpause)
        {
            return svLoan.of_savereq_lnpause(ref strlnpause);
        }

        public int InitShareWithdraw(ref str_slippayout strslippayout)
        {
            return svShare.of_initsharewithdraw(ref strslippayout);
        }
        public String InitShareWithdrawList()
        {
            return svShare.of_initsharewithdrawlist();
        }
        public int PostShareWithdraw(ref str_slippayout strslippayout)
        {
            return svShare.of_postsharewithdraw(ref strslippayout);
        }
        //a
        public int InitRequestResign(ref str_mbreqresign mbreqresign, String member_no, DateTime entry_date, String entry_id, String xml_dept, String xml_grt, String xml_loan, String xml_request, String xml_share, String xml_sum)
        {
            try
            {
                mbreqresign.entry_date = entry_date;
                mbreqresign.entry_id = entry_id;
                mbreqresign.member_no = member_no;
                mbreqresign.xml_dept = xml_dept;
                mbreqresign.xml_grt = xml_grt;
                mbreqresign.xml_loan = xml_loan;
                mbreqresign.xml_request = xml_request;
                mbreqresign.xml_share = xml_share;
                mbreqresign.xml_sum = xml_sum;
                int result = svMembRG.of_initrequest(ref mbreqresign);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //  a
        public int SaveRequestResign(str_mbreqresign mbreqresign, String member_no, DateTime entry_date, String entry_id, String xml_dept, String xml_grt, String xml_loan, String xml_request, String xml_share, String xml_sum)
        {
            try
            {
                mbreqresign.entry_date = entry_date;
                mbreqresign.entry_id = entry_id;
                mbreqresign.member_no = member_no;
                mbreqresign.xml_dept = xml_dept;
                mbreqresign.xml_grt = xml_grt;
                mbreqresign.xml_loan = xml_loan;
                mbreqresign.xml_request = xml_request;
                mbreqresign.xml_share = xml_share;
                mbreqresign.xml_sum = xml_sum;

                int result = svMembRG.of_saverequest(mbreqresign);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        public int PostApvListResign(String as_xmlreqlist, String as_apvid, DateTime adtm_apvdate)
        {
            try
            {

                int result = svMembRG.of_postapvlist(as_xmlreqlist, as_apvid, adtm_apvdate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        public String InitApvListResign()
        {
            try
            {

                String result = svMembRG.of_initapvlist();
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a
        public String InitRequestShrPayment(String as_memno)
        {
            try
            {
                String result = svMbAdjShr.of_initrequest(as_memno);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        public int SaveRequestShrPayment(String as_xmlreq, String as_entry, DateTime adtm_entrydate)
        {
            try
            {
                int result = svMbAdjShr.of_saverequest(as_xmlreq, as_entry, adtm_entrydate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        public String InitApvlistShrPayment()
        {
            try
            {

                String result = svMbAdjShr.of_initapvlist();
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //  a
        public int PostApvlistShrPayment(String as_xmlreq, String as_entry, DateTime adtm_entrydate)
        {
            try
            {
                int result = svMbAdjShr.of_postapvlist(as_xmlreq, as_entry, adtm_entrydate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        //init master ยกเลิกถอนหุ้น
        public int InitCancelSwd(ref str_slipcancel astr_cancelslip)
        {
            try
            {



                int result = svShare.of_initcancelswd(ref astr_cancelslip);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        //init detail ยกเลิกถอนหุ้น
        public int InitCancelSwdDet(ref str_slipcancel astr_cancelslip)
        {
            try
            {

                int result = svShare.of_initcancelswddet(ref astr_cancelslip);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        //save ยกเลิกถอนหุ้น
        public int SaveCancelSwdDet(str_slipcancel slipcancle)
        {
            try
            {

                int re = svShare.of_cancelsharewithdraw(slipcancle);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //Boom
        public short InitReqReturn(ref String xml_head, ref String xml_detail, ref String xml_message)
        {
            try
            {
                short result = svLoanRigth.of_init_reqreturn(ref xml_head, ref xml_detail, ref xml_message);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //Boom
        public short OpenReqReturn(String request_docno, ref String xml_head, ref String xml_detail, ref String xml_message)
        {
            try
            {
                short result = svLoanRigth.of_openreqreturn(request_docno, ref xml_head, ref xml_detail, ref xml_message);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Boom
        public short CancelReqReturn(ref String xml_head, ref String xml_message, String cancelID, String branchID)
        {
            try
            {
                short result = svLoanRigth.of_cancelreturn(ref xml_head, ref xml_message, cancelID, branchID);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //Boom
        public short SaveReqReturn(String xml_head, String xml_detail, String entryID, String branchID)
        {
            try
            {
                short result = svLoanRigth.of_savereturn(xml_head, xml_detail, entryID, branchID);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //may แก้ไข/ยกเลิก รายการหุ้นของขวัญ
        public str_shrgift InitChageShrgift(str_shrgift strShrgift)
        {
            try
            {
                svShrgift.of_init_shrgiftadjust(ref strShrgift);
                DisConnect();
                return strShrgift;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //may บันทึก แก้ไข/ยกเลิก รายการหุ้นของขวัญ
        public str_shrgift SaveChageShrgift(str_shrgift strShrgift)
        {
            try
            {
                int ii = svShrgift.of_save_shrgiftadjust(ref strShrgift);
                DisConnect();
                return strShrgift;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        public int SaveLncollMast(ref str_lncollmast lstr_lncollmast, String member_no, String xml_collmastdet, String xml_collmemco, String entry_id, String branch_id)
        {
            try
            {
                lstr_lncollmast.member_no = member_no;

                lstr_lncollmast.xml_collmastdet = xml_collmastdet;
                lstr_lncollmast.xml_collmemco = xml_collmemco;
                lstr_lncollmast.entry_id = entry_id;
                lstr_lncollmast.branch_id = branch_id;
                int re = svLoan.of_savelncollmast(ref lstr_lncollmast);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //==============================
        //Mai
        //function ประมวลผลปันผล
        public int CalDivProcess(String xmlprocinfo, String xmlloantype)
        {
            try
            {
                int re = svCalEstdiv.of_caldivprocess(xmlprocinfo, xmlloantype);

                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        //function ประมวลผลปันผล

        public String EstmateProcess(String xml_procinfo, String xml_loantype)
        {
            try
            {
                return svCalEstdiv.of_estimate_process(xml_procinfo, xml_loantype);
                DisConnect();
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Init คีย์ข้อมูลหักปันผล - เฉลี่ยคืน
        public String[] InitDivMethodPM(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initdivmethodpayment(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save คีย์ข้อมูลหักปันผล - เฉลี่ยคืน
        public int SaveDivMethodPM(String xml_head, String xml_detail)
        {

            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                int re = svDivavg.of_savedivmethodpayment(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }


        //Mai
        // Init อายัดปันผล-เฉลี่ยคืน
        public String[] InitDivSequest(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initdivsequest(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save อายัดปันผล-เฉลี่ยคืน
        public int SaveDivSequest(String xml_head, String xml_detail, String entry_id)
        {

            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                astr_divavg.entry_id = entry_id;
                int re = svDivavg.of_savedivsequest(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        // Init คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด (ค่าเริ่มต้น)
        public String InitListPaydivavgMain()
        {
            String xml_listmain = svDivavg.of_initlist_paydivavgmain();
            DisConnect();
            return xml_listmain;
        }

        //Mai
        //Init คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด
        public String InitListPaydivavgDetail(String xml_head)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;

                String xml_divavglist = svDivavg.of_initlist_paydivavgdetail(xml_head);

                DisConnect();
                return xml_divavglist;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        // Save คีย์จ่ายปันผล - เฉลี่ยคืนตามช่วงสังกัด
        public int SaveListPayDivAvg(String xml_list, String entry_id, String branch_id, String tofrom_accid, DateTime operate_date, DateTime slip_date)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_list = xml_list;
                astr_divavg.entry_id = entry_id;
                astr_divavg.branch_id = branch_id;
                astr_divavg.tofrom_accid = tofrom_accid;
                astr_divavg.operate_date = operate_date;
                astr_divavg.slip_date = slip_date;

                int re = svDivavg.of_savelist_paydivavg(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        //Init คีย์จ่ายปันผล - เฉลี่ยคืนตามทะเบียน
        public String[] InitSlipPayOut(String payoutslip_no, String div_year, String member_no, DateTime slip_date, DateTime operate_date)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.payoutslip_no = payoutslip_no;
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;
                astr_divavg.slip_date = slip_date;
                astr_divavg.operate_date = operate_date;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initslippayout(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {
                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        //Save คีย์จ่ายปันผล - เฉลี่ยคืนตามทะเบียน
        public int SaveSlipPayOut(String xml_head, String xml_detail, String entry_id, String branch_id, DateTime operate_date, DateTime slip_date)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                astr_divavg.entry_id = entry_id;
                astr_divavg.branch_id = branch_id;
                astr_divavg.operate_date = operate_date;
                astr_divavg.slip_date = slip_date;

                int result = svDivavg.of_saveslippayout(astr_divavg);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Init งดเฉลี่ยคืน

        public String[] InitDivDropAverage(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initdivdropaverage(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }

                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save งดเฉลี่ยคืน
        public int SaveDivDropAverage(String xml_head, String xml_detail, String entry_id)
        {

            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                astr_divavg.entry_id = entry_id;
                int re = svDivavg.of_savedivdropaverage(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        // Init ยกเลิกจ่ายปันผลเฉลี่ยคืน

        public String[] InitCancelSlipPayOut(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initcancelslippayout(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save ยกเลิกจ่ายปันผลเฉลี่ยคืน
        public int SaveCancelSlipPayOut(String xml_head, String xml_detail, String entry_id, DateTime slip_date)
        {

            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                astr_divavg.entry_id = entry_id;
                astr_divavg.slip_date = slip_date;
                int re = svDivavg.of_savecancelslippayout(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        //Search ค้นหาสมาชิก 
        public String SearchList(String xml_searchdata)
        {
            try
            {
                String xml_searchlist = svDivavg.of_searchlist(xml_searchdata);
                DisConnect();
                return xml_searchlist;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //Mai
        // InitList รายละเอียดปันผล
        public int InitListDivAvg(ref str_divavg astr_divavg, String div_year, String member_no)
        {
            try
            {
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;
                int re = svDivavg.of_initlist_divavg(ref astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // InitDetail รายละเอียดปันผล
        public String InitListDivAvgDetail(String div_year, String member_no, String dataobject)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;
                astr_divavg.dataobject = dataobject;

                String result = "";
                try
                {
                    result = svDivavg.of_initlist_divdetail(astr_divavg);

                }
                catch (Exception ex) { result = ex.Message; }

                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


        //Mai
        //Search ค้นหาใบทำรายการ
        public String SearchListSlipPayOut(String xml_searchdata)
        {
            try
            {
                String xml_searchlist = svDivavg.of_searchlist_slippayout(xml_searchdata);
                DisConnect();
                return xml_searchlist;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        //Search ค้นหารหัสธนาคาร
        public int SearchListBank(ref str_divavg astr_divavg, String xml_head)
        {
            try
            {
                astr_divavg.xml_head = xml_head;
                int re = svDivavg.of_search_bank(ref astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        ////Mai
        ////Search ค้นหาสาขาธนาคาร
        public int SearchListBranch(ref str_divavg astr_divavg, String xml_head, String bank_code)
        {
            try
            {
                astr_divavg.xml_head = xml_head;
                astr_divavg.bank_code = bank_code;
                int re = svDivavg.of_search_bankbranch(ref astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //////Mai
        //////Search ค้นหาสังกัด
        public int SearchListMemGroup(ref str_divavg astr_divavg, String xml_head)
        {
            try
            {
                astr_divavg.xml_head = xml_head;
                int re = svDivavg.of_search_membgroup(ref astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Init ผิดนัดชำระหนี้
        public String[] InitMissPayLoan(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_initmisspayloan(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }

                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save ผิดนัดชำระหนี้
        public int SaveMissPayLoan(String xml_head, String xml_detail)
        {

            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;
                int re = svDivavg.of_savemisspayloan(astr_divavg);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        // Init แสดงรายละเอียดปันผลเฉลี่ยคืน
        public String InitDivavgDetail(String div_year, String member_no)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.div_year = div_year;
                astr_divavg.member_no = member_no;

                int result = 0;
                String arr = "";
                try
                {
                    result = svDivavg.of_initdivavgdetail(ref astr_divavg);
                }
                catch (Exception ex) { arr = ex.Message; }
                if (result == 1)
                {
                    arr = astr_divavg.xml_head;
                }

                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Init แสดงรายละเอียดปันผล เฉลี่ยคืน
        public String[] CalDivavgDetail(String xml_head, String xml_detail)
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                astr_divavg.xml_head = xml_head;
                astr_divavg.xml_detail = xml_detail;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svDivavg.of_caldivavgdetail(ref astr_divavg);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {
                    arr[0] = astr_divavg.xml_head;
                    arr[1] = astr_divavg.xml_detail;
                }

                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        #region ฟังชั่นใหม่สำหรับหน้าจอตารางชำระเท่านั้น @ 27/11/2553

        public void GenPeriodPayTab(ref str_genperiodpaytab astr_paytab)
        {
            try
            {
                svLoanRigth.of_genperiodpaytab(ref astr_paytab);
                DisConnect();
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        #endregion

        //Doys
        public int InitSlipAdjust(ref str_slipadjust slipAdjust)
        {
            try
            {
                int ii = -1;
                ii = svLoan.of_initslipadjust(ref slipAdjust);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Doys
        public int SaveSlipAdjust(ref str_slipadjust slipAdjust)
        {
            try
            {
                int ii = -1;
                ii = svLoan.of_saveslip_adjust(ref slipAdjust);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //เอ  เช็คหุ้นตามฐานเงินเดือน
        public int GetShareBase(Decimal adc_salary, ref Decimal adc_minshare, ref  Decimal adc_maxshare)
        {
            try
            {

                int re = svmbinfo.of_getsharebase(adc_salary, ref adc_minshare, ref adc_maxshare);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //boom
        public void InitEstTrnColl(ref str_esttrncoll strEsttrnColl)
        {

            try
            {

                svLoan.of_initest_trncoll(ref strEsttrnColl);
                DisConnect();

            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        //init  carmast
        public int of_initcarmast(ref str_carmast astr_carmast)
        {


            try
            {

                int result = svCarmast.of_initcarmast(ref astr_carmast);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a 
        //init carmastdet
        public int of_initcarmastdet(ref str_carmast astr_carmast)
        {

            try
            {

                int result = svCarmast.of_initcarmastdet(ref astr_carmast);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        // save_carmast
        public int of_savecarmast(ref str_carmast astr_carmast)
        {


            try
            {

                int result = svCarmast.of_savecarmast(ref astr_carmast);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //Boom
        public String GetDataKeeping(String memberNo, String recvPeriod)
        {

            try
            {

                String result = svLoanRigth.of_get_datakeeping(memberNo, recvPeriod);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


        //ed
        public String of_initlist_lnrcv(String as_moneytype, DateTime adtm_order)
        {
            try
            {
                String result = svLnOrder.of_initlist_lnrcv(as_moneytype, adtm_order);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public Int16 of_initlnrcv(ref str_slippayout astr_slippayout)
        {
            try
            {
                Int16 result = svLnOrder.of_initlnrcv(ref astr_slippayout);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public Int16 of_initlnrcv_recalint(ref str_slippayout astr_slippayout)
        {
            try
            {
                Int16 result = svLnOrder.of_initlnrcv_recalint(ref astr_slippayout);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public Int16 of_saveord_lnrcv(ref str_slippayout astr_slippayout)
        {
            try
            {
                Int16 result = svLnOrder.of_saveord_lnrcv(ref astr_slippayout);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }




        //ed
        public String of_initlist_lnrcv_fin(String as_moneytype, DateTime adtm_paydate)
        {
            try
            {
                String result = svLnFin.of_initlist_lnrcv(as_moneytype, adtm_paydate);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public Int16 of_initlnrcv_fin(ref str_slippayout astr_slippayout)
        {
            try
            {
                Int16 result = svLnFin.of_initlnrcv(ref astr_slippayout);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public Int16 of_saveslip_lnrcv_fin(ref str_slippayout astr_slippayout)
        {
            try
            {
                Int16 result = svLnFin.of_saveslip_lnrcv(ref astr_slippayout);
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
