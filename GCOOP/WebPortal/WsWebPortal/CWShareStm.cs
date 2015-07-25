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
using DBAccess;
using System.Collections.Generic;

namespace WsWebPortal
{
    public class CWShareStm
    {
        private String _shareTypeCode;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public String ShareTypeCode
        {
            get { return _shareTypeCode; }
            set { _shareTypeCode = value; }
        }
         private String _shaItemTypeCode;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public String ShaItemTypeCode
        {
            get { return _shaItemTypeCode; }
            set { _shaItemTypeCode = value; }
        }       
        private String _shaItemTypeDesc;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public String ShaItemTypeDesc
        {
            get { return _shaItemTypeDesc; }
            set { _shaItemTypeDesc = value; }
        }          
        private Double _shareAmount;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public Double ShareAmount
        {
            get { return _shareAmount; }
            set { _shareAmount = value; }
        }    
        private Double _shareValue;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public Double ShareValue
        {
            get { return _shareValue; }
            set { _shareValue = value; }
        }
        private Int32 _seqNo;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public Int32 SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        private DateTime _oprDate;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public DateTime OprDate
        {
            get { return _oprDate; }
            set { _oprDate = value; }
        }

        public List<CWShareStm> GetShareStm(String memberNo)
        {
            List<CWShareStm> cList = new List<CWShareStm>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                 SELECT     
                    SHSHARESTATEMENT.SHARETYPE_CODE, 
                    SHSHARESTATEMENT.SEQ_NO,SHSHARESTATEMENT.SHRITEMTYPE_CODE, 
                    SHSHARESTATEMENT.SHARE_AMOUNT, SHSHARESTATEMENT.OPERATE_DATE, 
                    SHUCFSHRITEMTYPE.SHRITEMTYPE_DESC,SHSHARETYPE.SHARE_VALUE
                 FROM
                    SHSHARESTATEMENT, SHUCFSHRITEMTYPE,SHSHARETYPE
                 WHERE
                    SHSHARESTATEMENT.SHRITEMTYPE_CODE = SHUCFSHRITEMTYPE.SHRITEMTYPE_CODE AND 
				            SHSHARESTATEMENT.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE AND
                    (SHSHARESTATEMENT.MEMBER_NO ='" + memNo + "')ORDER BY SHSHARESTATEMENT.SEQ_NO";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWShareStm m = new CWShareStm();
                m.SeqNo= dt.GetInt32("SEQ_NO");
                m.ShareTypeCode = dt.GetString("SHARETYPE_CODE");
                m.ShaItemTypeCode = dt.GetString("SHRITEMTYPE_CODE");
                m.ShareAmount = dt.GetDouble("SHARE_AMOUNT");
                m.OprDate = dt.GetDate("OPERATE_DATE");
                m.ShaItemTypeDesc = dt.GetString("SHRITEMTYPE_DESC");
                m.ShareValue = dt.GetDouble("SHARE_VALUE");
                cList.Add(m);
            }
            return cList;
        }
    }
}
