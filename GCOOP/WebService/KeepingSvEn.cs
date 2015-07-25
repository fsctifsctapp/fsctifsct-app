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
    public class KeepingSvEn
    {
        private Security security;
        private n_cst_dbconnectservice KpCon;
        private n_cst_return_keep Kpkeep;
        private n_cst_keeping_disk Kpdisk;

        //EGAT
        private n_cst_pauseloan_keep Kppause;
        private n_cst_remainpay_keep Kpremain;


          public KeepingSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public KeepingSvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                KpCon = new n_cst_dbconnectservice();
                KpCon.of_connectdb(security.ConnectionString);
                Kpkeep = new n_cst_return_keep();
                Kpkeep.of_initservice(KpCon);
                Kpdisk = new n_cst_keeping_disk();
                Kpdisk.of_initservice(KpCon);

                //EGAT
                Kppause = new n_cst_pauseloan_keep();
                Kppause.of_initservice(KpCon);
                Kpremain = new n_cst_remainpay_keep();
                Kpremain.of_initservice(KpCon);

            }
        }

        public void DisConnect()
        {
            try
            {
                KpCon.of_disconnectdb();
            }
            catch { }
        }

        ~KeepingSvEn()
        {
            DisConnect();
        }
       // a
        public int InitReceiptReturn( String as_memno,String as_recvperiod,ref String as_xmlhead,ref String as_xmlreceipt )
        {
            try
            {

                int re = Kpkeep.of_initreceiptreturn(as_memno, as_recvperiod, ref as_xmlhead, ref as_xmlreceipt);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }
        // a
        public int SaveReceiptReturn(String as_xmlhead, String as_xmlreceipt, DateTime adtm_returndate, String as_userid, String as_branchid)
        {
            try
            {
                int re = Kpkeep.of_savereceiptreturn(as_xmlhead, as_xmlreceipt, adtm_returndate, as_userid, as_branchid);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }
        //a
        // decodeจากแผ่าน 
        public String decode(String as_recvperiod, DateTime adtm_receipt, String as_xmldata)
        {
            try
            {
                String re = Kpdisk.of_decodefile(as_recvperiod, adtm_receipt, as_xmldata);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }
        //a
        // cutkeepจากแผ่าน
        public int cutkeep(String as_recvperiod, String as_xmldata)
        {
            try
            {
                int re = Kpdisk.of_cutkeep(as_recvperiod, as_xmldata);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }


        //================
        // EGAT
        //Mai InitPauseLoanKeep หน้าจองดกู้แบบกำหนดระยะเวลา
        public int InitPauseLoanKeep(ref str_keep astr_keep)
        {
            try
            {
                int result = Kppause.of_initpauseloankeep(ref astr_keep);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


        //Mai SavePauseLoanKeep  หน้าจองดกู้แบบกำหนดระยะเวลา
        public int SavePauseLoanKeep(str_keep astr_keep)
        {
            try
            {
                int result = Kppause.of_savepauseloankeep(astr_keep);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


        //Mai หน้าจอ IMP&EXP disk
        public int DiskProcess(ref str_keep astr_keep)
        {
            try
            {
                int result = Kpdisk.of_diskprocess(ref astr_keep);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai InitRemainPay หน้าจอชำระยอดค้างซองเงินเดือน
        public int InitRemainPay(ref str_keep astr_keep)
        {
            try
            {
                int result = Kpremain.of_initremainpay(ref astr_keep);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


        //Mai SaveRemainPay  หน้าจอชำระยอดค้างซองเงินเดือน
        public int SaveRemainPay(str_keep astr_keep)
        {
            try
            {
                int result = Kpremain.of_saveremainpay(astr_keep);
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
