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
    public class LoanAssistSvEn
    {
        private Security security;
        private n_cst_dbconnectservice svCon;
        private n_cst_loanassist_keeping svKeepCancel;
        private n_cst_loanassist_service svloanCancel;
        private n_cst_loanassist_operate svOperateCancel;
        private n_cst_loanassist_loanright svLoanRight;
        private n_cst_loanassist_operate svTranLoan;
        public LoanAssistSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public LoanAssistSvEn(String wsPass, bool autoConnect)
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
                svKeepCancel = new n_cst_loanassist_keeping();
                svKeepCancel.of_initservice(svCon);
                svloanCancel = new n_cst_loanassist_service();
                svloanCancel.of_initservice(svCon);
                svOperateCancel = new n_cst_loanassist_operate();
                svOperateCancel.of_initservice(svCon);
                svLoanRight = new n_cst_loanassist_loanright();
                svLoanRight.of_initservice(svCon);
                svTranLoan = new n_cst_loanassist_operate();
                svTranLoan.of_initservice(svCon);
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

        ~LoanAssistSvEn()
        {
            DisConnect();
        }
        //---------------------------------------------------//

        //may ยกเลิกใบเสร็จ
        public int InitCancelRecieve(String member_no, String recvperiod, String xmlhead, String xmlrecept)
        {
            try
            {
                int result = svKeepCancel.of_initreceiptreturn(member_no, recvperiod, ref xmlhead, ref xmlrecept);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //may บันทึก ยกเลิกใบเสร็จ
        public int SaveCancelRecieve(String xmlhead, String xmlreceipt, DateTime adtm_returndate, String as_userid, String as_branchid)
        {
            try
            {
                int ii = svKeepCancel.of_savereceiptreturn(xmlhead, xmlreceipt, adtm_returndate, as_userid, as_branchid);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a ยกเลิกสัญญาเงินกู้
        public String InitReqContCancel(String as_contno)
        {
            try
            {
                String contno = svloanCancel.of_initreq_contcancel(as_contno);
                DisConnect();
                return contno;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //a บันทึก ยกเลิกสัญญาเงินกู้
        public int SaveReqContCancel(String as_xmlcontccl, String as_cancelid, DateTime adtm_cancel)
        {
            try
            {
                int re = svloanCancel.of_savereq_contcancel(as_xmlcontccl, as_cancelid, adtm_cancel);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a รับชำระ
        public String[] InitSlipPayIn(String ls_memno, String ls_sliptype, DateTime ldtm_slipdate, DateTime ldtm_opedate)
        {
            try
            {
                str_sliplspayin strxml = new str_sliplspayin();

                strxml.member_no = ls_memno;
                strxml.sliptype_code = ls_sliptype;
                strxml.slip_date = ldtm_slipdate;
                strxml.operate_date = ldtm_opedate;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svOperateCancel.of_initslippayin(ref strxml);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = strxml.xml_sliphead;
                    arr[1] = strxml.xml_sliplon;


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
                    svOperateCancel.of_initslippayin_calint(ref as_xmlloan, ref as_sliptype, ref datecal);
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
        //a บันทึก รับชำระ
        public int SaveSlipPayIn(String xml_sliphead, String xml_sliplon, String ls_entry_id, String ls_branch_id)
        {
            try
            {
                str_sliplspayin strslip = new str_sliplspayin();
                strslip.xml_sliphead = xml_sliphead;
                strslip.xml_sliplon = xml_sliplon;
                strslip.entry_id = ls_entry_id;
                strslip.branch_id = ls_branch_id;
                int ii = svOperateCancel.of_saveslip_payin(ref strslip);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a initlistจ่าย
        public String InitListLnRcv()
        {
            try
            {
                String result = svOperateCancel.of_initlist_lnreqapv();
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a จ่ายเงินกู้
        public str_sliplspayout InitLnRcv(str_sliplspayout strSlipPayOut)
        {
            try
            {
                svOperateCancel.of_initlnrcv(ref strSlipPayOut);
                DisConnect();
                return strSlipPayOut;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a save จ่ายเงินกู้
        public int SaveLnRcv(str_sliplspayout strSlipPayOut)
        {
            try
            {
                int result;
                result = svOperateCancel.of_saveslip_lnrcv(ref strSlipPayOut);
                DisConnect();
                return result;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }
        // Boom กำหนดค่าเริ่มต้นใบคำขอกู้
        public short InitLoanReq(ref str_lsloanright strLoanright)
        {

            try
            {
                short result = svLoanRight.of_initloanreq(ref   strLoanright);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        // Boom บันทึกใบคำขอกู้
        public short SaveReqLoan(str_lsloanright strLoanright)
        {
            try
            {
                short result = svLoanRight.of_savereqloan(strLoanright);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        // Boom สร้างเลขที่สัญญา
        public String GenNewContNo(String strLoantype)
        {
            try
            {
                String result = svLoanRight.of_getnewcontractno(strLoantype);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //may โอนหนี้ให้ผู้ค้ำ
        public str_lntrncoll InitLnTrnColl(str_lntrncoll str)
        {
            try
            {
                svTranLoan.of_initreq_lntrncoll(ref str);
                DisConnect();
                return str;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //may โอนหนี้ให้ผู้ค้ำ (เฉลี่ยให้ผู้ค้ำ)
        public String[] InitLnTrnCollRecalTrn(String xmlmast, String xmldetail)
        {
            try
            {
                String[] strXML = new String[2];
                svTranLoan.of_initreq_lntrncollrecaltrn(ref xmlmast, ref xmldetail);

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

        //may บันทึก โอนหนี้ให้ผู้ค้ำ
        public int SaveLnTrnColl(str_lntrncoll astr_trncoll)
        {
            try
            {
                int ii = svTranLoan.of_savereq_lntrncoll(ref astr_trncoll);
                DisConnect();
                return ii;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
    }
}
