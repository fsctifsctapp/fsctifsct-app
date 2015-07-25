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
    /// Summary description for Insurance
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Insurance : System.Web.Services.WebService
    {
        [WebMethod]
        public String[] InitInsuRequestNew(String wsPass, String pbl, String application, String xmlInsuRequest)
        {
            return new InsuranceSvEn(wsPass).InitInsuRequestNew(pbl, application, xmlInsuRequest);
        }

        [WebMethod]
        public int SaveInsuRequestNew(String wsPass, String pbl, String application, DateTime workDate, String xmlInsuRequest)
        {
            return new InsuranceSvEn(wsPass).SaveInsuRequestNew(pbl, application, workDate, xmlInsuRequest);
        }

         [WebMethod]
        public String[] InitInsuReqResign(String wsPass, String pbl, String application, String xmlInsuReqResign)
        {
            return new InsuranceSvEn(wsPass).InitInsuReqResign(pbl, application, xmlInsuReqResign);
        }
         [WebMethod]
         public int SaveInsuReqResign(String wsPass, String pbl, String application, DateTime workDate, String xmlInsuReqResign, String xmlInsuReqResigndel)
         {
             return new InsuranceSvEn(wsPass).SaveInsuReqResign(pbl, application, workDate, xmlInsuReqResign, xmlInsuReqResigndel);
         }
         [WebMethod]
         public int SaveAppinsnew(String wsPass, String member_no)
         {
             return new InsuranceSvEn(wsPass).SaveAppinsnew(member_no);
         }

         //[WebMethod]
         //public String[] InitInsuSliReq(String wsPass, String pbl, String application, String xmlInsuReqRequest)
         //{
         //    return new InsuranceSvEn(wsPass).InitInsuSliReq(pbl, application, xmlInsuReqRequest);
         //}
        //========== By AOI 31.03.2011
         [WebMethod]
         public String[] InitInsuSliReq(String wsPass, String pbl, String application, String xmlInsuReqRequest)
         {
             return new InsuranceSvEn(wsPass).InitInsuSliReq(pbl, application, xmlInsuReqRequest);
         }
         [WebMethod]
         public int SaveInsuSliReq(String wsPass, String pbl, String application, DateTime workDate, String xmlInsuReqResign, String xmlInsuSlipDet)
         {
             return new InsuranceSvEn(wsPass).SaveInsuSliReq(pbl, application, workDate, xmlInsuReqResign, xmlInsuSlipDet);
         }
         //[WebMethod]
         //public String[] InitInsuSliDet(String wsPass, String pbl, String application, String xmlInsuReqRequest, String memberno)
         //{
         //    return new InsuranceSvEn(wsPass).InitInsuSliDet(pbl, application, xmlInsuReqRequest, memberno);
         //}

         [WebMethod]
         public int SaveSlipDetail(string wsPass, String pbl, String application, DateTime workDate, String xmlInsuSlipRequest, String slipno)
         {
             return new InsuranceSvEn(wsPass).SaveSlipDet(pbl, application, workDate, xmlInsuSlipRequest, slipno);
         }
        //===== End AOI 31.03.2011
        //by kae ยกเลิกปิดประกัน
         [WebMethod]
         public int SaveCancelresign(String wsPass, String pbl, String application, DateTime workDate, String xmlInsuCancelResign)
         {

             return new InsuranceSvEn(wsPass).SaveCancelresign(pbl, application, workDate, xmlInsuCancelResign);
         }
           
    }
}
