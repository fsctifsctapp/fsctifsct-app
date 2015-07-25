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
    public class CWKeeping
    {
        private Int32 _seqNo;
        /// <summary>
        /// 
        /// </summary>
        public Int32 Seqno
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        private String _keepTypeDesc;
        /// <summary>
        /// 
        /// </summary>
        public String KeepTypeDesc
        {
            get { return _keepTypeDesc; }
            set { _keepTypeDesc = value; }
        }
        private String _recvPeriod;
        /// <summary>
        /// 
        /// </summary>
        public String RecvPeriod
        {
            get { return _recvPeriod; }
            set { _recvPeriod = value; }
        }
        private Double _itemPayment;
        /// <summary>
        /// 
        /// </summary>
        public Double ItemPayment
        {
            get { return _itemPayment; }
            set { _itemPayment = value; }
        }

        public List<CWKeeping> GetKeeping(String memberNo)
        {
            List<CWKeeping> cList = new List<CWKeeping>();
            String memNo = memberNo.Trim();
            Sta ta = new DBAccess.Sta(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            String sql = @"
                  SELECT     
                             KPTEMPRECEIVEDET.SEQ_NO, KPUCFKEEPITEMTYPE.KEEPITEMTYPE_DESC, 
                             KPTEMPRECEIVEDET.ITEM_PAYMENT,KPTEMPRECEIVEDET.RECV_PERIOD
                  FROM       
                             KPTEMPRECEIVEDET, KPUCFKEEPITEMTYPE
                  WHERE     
                             KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE = KPUCFKEEPITEMTYPE.KEEPITEMTYPE_CODE AND 
                             (KPTEMPRECEIVEDET.MEMBER_NO = '" + memNo + "') AND (KPTEMPRECEIVEDET.RECV_PERIOD =(SELECT     MAX(RECV_PERIOD) AS EXPR1 FROM  KPTEMPRECEIVEDET KPTEMPRECEIVEDET_1 WHERE     (MEMBER_NO = '" + memNo + "'))) ORDER BY KPTEMPRECEIVEDET.SEQ_NO ";
            Sdt dt = ta.Query(sql);
            ta.Close();
            while (dt.Next())
            {
                CWKeeping m = new CWKeeping();
                m.Seqno = dt.GetInt32("SEQ_NO");
                m.KeepTypeDesc = dt.GetString("KEEPITEMTYPE_DESC");
                m.ItemPayment = dt.GetDouble("ITEM_PAYMENT");
                m.RecvPeriod = dt.GetString("RECV_PERIOD");
                cList.Add(m);
            }
            return cList;
        }
    }
}
