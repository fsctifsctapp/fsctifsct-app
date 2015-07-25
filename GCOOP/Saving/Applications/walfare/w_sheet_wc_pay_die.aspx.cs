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
    public partial class w_sheet_wc_pay_die : PageWebSheet, WebSheet
    {

        protected String jsselect_option;
        protected String jsselect_deptacc;
        private DwThDate tDwMain;
        private DwThDate tDWOption;
        private String pbl = "w_sheet_wc_membermaster.pbl";


        public void InitJsPostBack()
        {

            jsselect_option = WebUtil.JsPostBack(this, "jsselect_option");
            jsselect_deptacc = WebUtil.JsPostBack(this, "jsselect_deptacc");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("die_date", "die_tdate");
            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("deptclose_date", "deptclose_tdate");
            tDWOption = new DwThDate(DwOption, this);
          
        }

        public void WebSheetLoadBegin()
        {

         //  DwUtil.RetrieveDDDW(DwOption, "yyyymm", pbl, null);
            if (!IsPostBack)
            {
                DwOption.InsertRow(0);
               
                
          
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

                case "jsselect_option":
                    jjsselect_option();
                    break;
                case "jsselect_deptacc":
                    jjsselect_deptacc();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {

                String XmlMain = DwMain.Describe("DataWindow.data.XML");
                //string branch_id = state.SsBranchId;

                bool result = false;

                result = WsUtil.Walfare.Pay_die_member_save(state.SsWsPass, state.SsApplication, XmlMain, pbl, state.SsUsername, state.SsCsType);

                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();

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
            try
            {
                DwUtil.RetrieveDDDW(DwOption, "year_mm", pbl, null);
            }
            catch { }
            tDWOption.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwOption.SaveDataCache();
            DwMain.SaveDataCache();
            
        }


        public void jjsselect_option()
        {

            String cs_t = state.SsCsType;
            String year_mm = DwOption.GetItemString(1, "year_mm");
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, year_mm, cs_t, "8", "%");
            DwOption.SetItemString(1,"deptacc",null);

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDateTime(i, "pay_date", DateTime.Today);
            }

        }
        public void jjsselect_deptacc()
        {

            String cs_t = state.SsCsType;
            String year_mm = DwOption.GetItemString(1, "year_mm");
            String deptacc = DwOption.GetItemString(1, "deptacc");
            String acc = "%" + deptacc + "%";
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, year_mm, cs_t, "%", acc);

            //for (int i = 1; i <= DwMain.RowCount; i++)
            //{
            //    DwMain.SetItemDateTime(i, "pay_date", DateTime.Today);
            //}

        }

    }
}