using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;


namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_memo_admin : PageWebSheet, WebSheet
    {
        protected String jsbranch_id;
        protected String jsbranch_desc;
        protected String jstype_chang;
        protected String jsmoney_chang;
       // protected String jsselect_option;
       // private DwThDate tDwMain;
        // private DwThDate tDWOption;


        public void InitJsPostBack()
        {
            jsbranch_id = WebUtil.JsPostBack(this, "jsbranch_id");
            jsbranch_desc = WebUtil.JsPostBack(this, "jsbranch_desc");
            jstype_chang = WebUtil.JsPostBack(this, "jstype_chang");
            jsmoney_chang = WebUtil.JsPostBack(this, "jsmoney_chang");
           // jsselect_option = WebUtil.JsPostBack(this, "jsselect_option");

          ///////  tDwMain = new DwThDate(DwMain, this);
          //  tDwMain.Add("memo_date", "memo_tdate");

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
                DwMain.InsertRow(0);
               
                DwOption.SetItemString(1, "branch_idd", state.SsBranchId);
                DwOption.SetItemString(1, "branch_id", state.SsBranchId);
                //DwMain.SetItemDateTime(1, "memo_date", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                string Sday = Convert.ToString(DateTime.Today.Day);
                string Smonth = Convert.ToString(DateTime.Today.Month);
              

             
                DateTime ENdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
               // DateTime time_now = state.SsWorkDate;
                String striDate = ENdate.ToString("dd/MM/yyyy");
                String time_n = DateTime.Now.ToString("HH:mm")+" น.";
                string eDateTH = striDate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(striDate.Substring(6, 4)) + 543);
                DwMain.SetItemString(1, "memo_date", eDateTH);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemString(1, "memo_time", time_n);
                ChangeMemoType();
                //DwIn.Visible = false;
                //DwMain.Visible = false;
               // jjsselect_option();
            }
            else
            {
                this.RestoreContextDw(DwOption);
                this.RestoreContextDw(DwMain);

            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsbranch_id":
                    ChangeBranchId();
                    break;
                case "jsbranch_desc":
                    ChangeBranchDesc();
                    break;
                case "jstype_chang":
                    ChangeMemoType();
                    break;
                case "jsmoney_chang":
                    ChangeMoney();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string select_option;
                try { select_option = DwOption.GetItemString(1, "branch_id"); }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกศูนย์ประสานงาน");
                    return;
                }
                String XmlMain = DwMain.Describe("DataWindow.data.XML");
                //string branch_id = state.SsBranchId;

                bool result = false;

                result = WsUtil.Walfare.Memo_Admin_Save(state.SsWsPass, state.SsApplication, XmlMain, "w_sheet_wc_trn_memb.pbl", state.SsUsername, state.SsCsType, select_option);
           
                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    WebSheetLoadBegin();
                    //DwMain.SetItemString(1, "detail","");
                    //DwMain.SetItemString(1, "remark", "");
                    //DwMain.SetItemDecimal(1, "money_rev", 0);
                    //DwMain.SetItemDecimal(1, "money_pay", 0);
                    //DwMain.SetItemDecimal(1, "memb_in", 0);
                    //DwMain.SetItemDecimal(1, "memb_out", 0);
                    //DwMain.Reset();

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                //  LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก สถานะอนุมัติและเลขกรอก สอ.ใหม่ ก่อนทำการกดบันทึก");
            }
        }

        public void WebSheetLoadEnd()
        {
            string not_cstype, cstype;
           
                not_cstype = "0";
                cstype = state.SsCsType;
                
            
            try
            {
                DwUtil.RetrieveDDDW(DwOption, "branch_idd", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);
               

            }
            catch { }
            // tDWOption.Eng2ThaiAllRow();
           // tDwMain.Eng2ThaiAllRow();
          
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
        }



        public void ChangeBranchId()
        {
            //int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwOption.SetItemString(1, "branch_idd", DwOption.GetItemString(1, "branch_id"));
              // jjsselect_option();
            }
            catch { }
        }

        public void ChangeBranchDesc()
        {
            // int row = Convert.ToInt32(HdRow.Value);
            try
            {
                DwOption.SetItemString(1, "branch_id", DwOption.GetItemString(1, "branch_idd"));
               // jjsselect_option();
            }
            catch { }
        }

         public void ChangeMemoType()
          {
             
             // string cs_t = state.SsCsType;
              string memo_type;
              DwMain.SetItemString(1, "memo_detail", "");
              DwMain.SetItemString(1, "money_type", "");
              DwMain.SetItemString(1, "bank_code", "");
              memo_type = DwMain.GetItemString(1, "memo_type");
              DwUtil.RetrieveDDDW(DwMain, "memo_detail", "w_sheet_wc_trn_memb.pbl", memo_type);

              if (memo_type == "01")
              {
                  DwUtil.RetrieveDDDW(DwMain, "money_type", "w_sheet_wc_trn_memb.pbl", null);
                  // DwUtil.RetrieveDDDW(DwMain, "bank_code", "w_sheet_wc_trn_memb.pbl", null);
              }
              //else
              //{
              //    DwMain.SetItemString(1, "money_type", " ");
              //    DwMain.SetItemString(1, "bank_code", " ");
              //}
             
             
             
          }
         public void ChangeMoney()
         {
             string money_type;
             money_type = DwMain.GetItemString(1, "money_type");
             if (money_type == "02")
             {
                 DwUtil.RetrieveDDDW(DwMain, "bank_code", "w_sheet_wc_trn_memb.pbl", null);
             }
         }

    }
}