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
    public partial class w_sheet_wc_memo_cancal : PageWebSheet, WebSheet
    {
        protected String Search_id;

        public void InitJsPostBack()
        {
            Search_id = WebUtil.JsPostBack(this, "Search_id");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwCri);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "Search_id":
                    Search_id_detail();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            Sta ta = new Sta(state.SsConnectionString);
            ta.Transection();
            try
            {
                string memo_id;
                Decimal memo_status;
                memo_id = DwMain.GetItemString(1, "memo_id");
                memo_status = DwMain.GetItemDecimal(1, "memo_status");
                String sql_ckstatus = "select * from cmmemodetail  where memo_id ='" + memo_id + "' ";
                Sdt dtchk_status = ta.Query(sql_ckstatus);
                if (dtchk_status.Next())
                {
                    Decimal old_status = dtchk_status.GetDecimal("memo_status");
                    if (memo_status == old_status)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสถานะ");
                        return;
                    }
                }
                
                String updateData = "update cmmemodetail set memo_status = '" + memo_status + "' where memo_id ='" + memo_id + "' ";
                ta.Exe(updateData);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                //int[] rows = new int[DwMain.RowCount];
                //for (int i = 1; i <= DwMain.RowCount; i++)
                //{
                //    rows[i - 1] = i;
                //}

                //    DwUtil.InsertDataWindow(DwMain, "w_sheet_wc_trn_memb.pbl", "cmmemodetail", rows);
                //    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                ta.Commit();
                ta.Close();
               // DwMain.Reset();

            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                //  LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก สถานะอนุมัติและเลขกรอก สอ.ใหม่ ก่อนทำการกดบันทึก");
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwCri.SaveDataCache();
        }

        private void Search_id_detail()
        {
            try
            {
                string get_memo_id;
                get_memo_id = DwCri.GetItemString(1, "memo_id");

                if (get_memo_id.Trim().Length != 10)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกเลขที่รายการให้ครบ 10 หลัก");
                    return;
                }
                DwUtil.RetrieveDataWindow(DwMain, "w_sheet_wc_trn_memb.pbl", null, get_memo_id);

            }


            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
    }
}