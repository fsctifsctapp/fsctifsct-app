using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_permission_all : PageWebSheet, WebSheet
    {
        private String pbl = "w_sheet_wc_permission_all.pbl";

        protected String jsselectRegister;
        protected String jsselectAdmin;
        protected String jsselectEditRegis;
        protected String jsselectMember;
        protected String jsselectResign;
        protected String jsselectPaid;

        public void InitJsPostBack()
        {
            jsselectAdmin = WebUtil.JsPostBack(this, "jsselectAdmin");
            jsselectRegister = WebUtil.JsPostBack(this, "jsselectRegister");
            jsselectEditRegis = WebUtil.JsPostBack(this, "jsselectEditRegis");
            jsselectMember = WebUtil.JsPostBack(this, "jsselectMember");
            jsselectResign = WebUtil.JsPostBack(this, "jsselectResign");
            jsselectPaid = WebUtil.JsPostBack(this, "jsselectPaid");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null,state.SsCsType, state.SsApplication);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsselectAdmin":
                    JSselectAdmin();
                    break;
                case "jsselectRegister":
                    JSselectRegister();
                    break;
                case "jsselectEditRegis":
                    JSselectEditRegis();
                    break;
                case "jsselectMember":
                    JSselectMember();
                    break;
                case "jsselectResign":
                    JSselectResign();
                    break;
                case "jsselectPaid":
                    JSselectPaid();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");
                int ii = WsUtil.Walfare.PermissUsers(state.SsWsPass, state.SsApplication, pbl, xmlDwMain);
                LtServerMessage.Text = WebUtil.CompleteMessage("กำหนดสิทธิ์การใช้งานระบบสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {           
            DwMain.SaveDataCache();
        }

        public void JSselectAdmin()
        {
            decimal check_admin = 2;
            switch (uadmin.Value)
            {
                case "1":
                    check_admin = 1;
                    break;
                case "2":
                    check_admin = 2;
                    break;
                case "3":
                    check_admin = 3;
                    break;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "user_type", check_admin);
            }
        }

        public void JSselectRegister()
        {
            decimal check_regis = 0;
            if (upermiss.Checked == true)
            {
                check_regis = 1;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "check_flag", check_regis);
            }
        }

        public void JSselectPaid()
        {
            decimal check_regis = 0;
            if (paid.Checked == true)
            {
                check_regis = 1;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "paid_flag", check_regis);
            }
        }

        public void JSselectEditRegis()
        {
            decimal check_regis = 0;
            if (editregis.Checked == true)
            {
                check_regis = 1;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "editreqregis_flag", check_regis);
            }
        }
        public void JSselectMember()
        {
            decimal check_regis = 0;
            if (member.Checked == true)
            {
                check_regis = 1;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "memb_flag", check_regis);
            }
        }
        public void JSselectResign()
        {
            decimal check_regis = 0;
            if (resign.Checked == true)
            {
                check_regis = 1;
            }
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemDecimal(i, "resign_flag", check_regis);
            }
        }

    }
}