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
using GcoopServiceCs;

namespace WebService
{
    public class InsuranceSvEn
    {
        private InsuranceService ins;
        private Security security;

        public InsuranceSvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public InsuranceSvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                ins = new InsuranceService(security.ConnectionString);
            }
        }

        public void DisConnect()
        {
            try
            {
            }
            catch { }
        }

        ~InsuranceSvEn()
        {
            DisConnect();
        }

        private String PblFullPath(String pbl, String application)
        {
            return security.PhysicalPath + "Saving\\DataWindow\\" + application + "\\" + pbl;
        }

        public String[] InitInsuRequestNew(String pbl, String application, String xmlInsuRequest)
        {
            return ins.InitInsuRequestNew(this.PblFullPath(pbl, application), xmlInsuRequest);
        }
        public String[] InitInsuReqResign(String pbl, String application, String xmlInsuReqResign)
        {
            return ins.InitInsuReqResign(this.PblFullPath(pbl, application), xmlInsuReqResign);
        }
        public int SaveInsuRequestNew(String pbl, String application, DateTime workDate, String xmlInsuRequest)
        {
            return ins.SaveInsuRequestNew(this.PblFullPath(pbl, application), workDate, xmlInsuRequest);
        }
        public int SaveInsuReqResign(String pbl, String application, DateTime workDate, String xmlInsuReqResign, String xmlInsuReqResigndel)
        {
            return ins.SaveInsuReqResign(this.PblFullPath(pbl, application), workDate, xmlInsuReqResign, xmlInsuReqResigndel);
        }
        public int SaveAppinsnew(String member_no)
        {
            return ins.SaveAppinsnew(member_no);
        }

        //==== By AOI 01.04.2011
        public String[] InitInsuSliReq(String pbl, String application, String xmlInsuRequest)
        {
            return ins.InitInsuSliReq(this.PblFullPath(pbl, application), xmlInsuRequest);
        }
        public int SaveInsuSliReq(String pbl, String application, DateTime workDate, String xmlInsuReqResign, String xmlInsuSlipDet)
        {
            return ins.SaveInsuSliReq(this.PblFullPath(pbl, application), workDate, xmlInsuReqResign, xmlInsuSlipDet);
        }
        //public String[] InitInsuSliDet(String pbl, String application, String xmlInsuRequest, String memberno)
        //{
        //    return ins.InitInsuSliDet(this.PblFullPath(pbl, application), xmlInsuRequest, memberno);
        //}

        public int SaveSlipDet(String pbl, String application, DateTime workDate, string xmlInsuSlipRequest, String slipno)
        {
            return ins.SaveSlipDet(this.PblFullPath(pbl, application), workDate, xmlInsuSlipRequest, slipno);
        }
        //======= END AOI 31.03.2011
        // by kae ยกเลิกปิดประกัน
        public int SaveCancelresign(String pbl, String application, DateTime workDate, String xmlInsuCancelResign)
        {

            return ins.SaveCancelresign(this.PblFullPath(pbl, application), workDate, xmlInsuCancelResign);
        }
    }
}
