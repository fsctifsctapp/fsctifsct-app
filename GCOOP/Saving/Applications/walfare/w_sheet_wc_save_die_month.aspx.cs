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
    public partial class w_sheet_wc_save_die_month : PageWebSheet, WebSheet
    {
       
        protected String jsselect_option;
        private DwThDate tDwMain;
        protected String SelectCode;
        protected String jsbranch_id;
        protected String jsbranch_desc;
      // private DwThDate tDWOption;


        public void InitJsPostBack()
        {
            SelectCode = WebUtil.JsPostBack(this, "SelectCode");
            jsselect_option = WebUtil.JsPostBack(this, "jsselect_option");
            jsbranch_id = WebUtil.JsPostBack(this, "jsbranch_id");
            jsbranch_desc = WebUtil.JsPostBack(this, "jsbranch_desc");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("die_date", "die_tdate");
            //tDWOption = new DwThDate(DwOption, this);
            tDwMain.Add("inform_date", "inform_tdate");
            //tDWOption.Add("as_enddate", "as_tenddate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
               // DwSelem.InsertRow(0);
              //  DwMain.InsertRow(0);
               // DwOption.SetItemDateTime(1, "as_startdate", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
                //DwOption.SetItemDateTime(1, "as_enddate", new DateTime(DateTime.Today.Year, 12, 31));
                //DwIn.Visible = false;
              //  DwMain.Visible = false;
                DwOption.SetItemString(1, "branch_idd", state.SsBranchId);
                DwOption.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                DwOption.SetItemString(1, "as_tstartdate", "01/01/" + Syear);
                DwOption.SetItemString(1, "as_tenddate", "31/12/" + Syear);
              //  DwSelem.Visible = false;
               // jjsselect_option();
            }
            else
            {
                this.RestoreContextDw(DwOption);
                this.RestoreContextDw(DwMain);
           //     this.RestoreContextDw(DwSelem);
              
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
               
                case "jsselect_option":
                    jjsselect_option();
                    break;
                case "SelectCode":
                    JSSelectCode();
                    break;
                case "jsbranch_id":
                    ChangeBranchId();
                    break;
                case "jsbranch_desc":
                    ChangeBranchDesc();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                bool result = false;
                //String sele_month = DwSelem.GetItemString(1,"sele_mon");
                //if (sele_month == "00")
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกเดือนที่จะบันทึก");
                //    return;
                //}
                //else
                //{
                    String XmlMain = DwMain.Describe("DataWindow.data.XML");
                   // String for_year = DwSelem.GetItemString(1, "for_year");
                    //String yearmm = for_year + sele_month;
                   result = WsUtil.Walfare.Pay_die_member(state.SsWsPass, state.SsApplication, XmlMain, "w_sheet_wc_membermaster.pbl", state.SsUsername, state.SsCsType);

                    if (result)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        DwMain.Reset();
                      //  DwSelem.Reset();
                    }
              //  }
           
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
                DwUtil.RetrieveDDDW(DwOption, "branch_idd", "w_sheet_wc_membermaster.pbl", cstype, not_cstype);


            }
            catch { }
           // tDWOption.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
            DwSelem.SaveDataCache();
        }

       
        public void jjsselect_option()
        {
           
            string cs_t = state.SsCsType;
        //  DwMain.Visible = true;
          // DwSelem.Visible = true;

           //string eDateTH = DwOption.GetItemString(1, "as_tstartdate");
           //string sDateTH = DwOption.GetItemString(1, "as_tenddate");
           //string sDateEN = sDateTH.Substring(1, 8);
           //LtServerMessage.Text = WebUtil.CompleteMessage(sDateEN);
           //string eDateEN = eDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(eDateTH.Substring(6, 4)) - 543);
          
           // DateTime s_date = DwOption.GetItemDate(1, "as_startdate");
            //DateTime e_date = DwOption.GetItemDate(1, "as_enddate");
           string branch_id_se = DwOption.GetItemString(1, "branch_id");
           string eDateTH = DwOption.GetItemString(1, "as_tenddate");
           string sDateTH = DwOption.GetItemString(1, "as_tstartdate");
            string sDateEN = sDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(sDateTH.Substring(6, 4)) - 543);
            string eDateEN = eDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(eDateTH.Substring(6, 4)) - 543);
            DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_membermaster.pbl", null, sDateEN, eDateEN, cs_t, branch_id_se);
           
        }
        private void JSSelectCode()
        {
            switch (HdSelectCode.Value)
            {
                case "unchk_all":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemString(i + 1, "sele", "0");
                    }
                    break;
                case "chk_all":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemString(i + 1, "sele", "1");
                    }
                    break;
              
            }
            HdSelectCode.Value = "";
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

    }
}