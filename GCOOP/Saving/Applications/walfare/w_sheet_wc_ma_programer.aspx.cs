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
    public partial class w_sheet_wc_ma_programer : PageWebSheet, WebSheet
    {
        protected String jsGenSlip;
        protected String jsupdate_Reg;
        protected String jsGenStatement;
        protected String jsGenCodept;
        protected String jsLogin;
        protected String jsGenNewCsType;

        public void InitJsPostBack()
        {
            jsGenSlip = WebUtil.JsPostBack(this, "jsGenSlip");
            jsupdate_Reg = WebUtil.JsPostBack(this, "jsupdate_Reg");
            jsGenStatement = WebUtil.JsPostBack(this, "jsGenStatement");
            jsGenCodept = WebUtil.JsPostBack(this, "jsGenCodept");
            jsLogin = WebUtil.JsPostBack(this, "jsLogin");
            jsGenNewCsType = WebUtil.JsPostBack(this, "jsGenNewCsType");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
            else
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsGenSlip":
                    JSGenSlip();
                    break;
                case "jsupdate_Reg":
                    JSupdate_Reg();
                    break;
                case "jsGenStatement":
                    GenStatement();
                    break;
                case "jsGenCodept":
                    GenCodept();
                    break;
                case "jsLogin":
                    ClickLogin();
                    break;
                case "jsGenNewCsType":
                    GenNewCsType();
                    break;
                default:

                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
        }

        private void JSGenSlip()
        {
            try
            {
                bool result = WsUtil.Walfare.GenSlip(state.SsWsPass, state.SsApplication, state.SsWorkDate, state.SsUsername);
                if (result == true)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("good");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        private void JSupdate_Reg()
        {
            try
            {
                bool resu = WsUtil.Walfare.UpdateReqDocno(state.SsWsPass, state.SsApplication);
                if (resu)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            }
        }

        private void GenStatement()
        {
            try
            {
                DateTime deptopen_date = Convert.ToDateTime(opendate.Value);
                decimal prncibalance = Convert.ToDecimal(prncbal.Value);
                String group_branch = groupBranch.Value;
                bool resu = WsUtil.Walfare.GenStatement(state.SsWsPass, state.SsApplication, deptopen_date, prncibalance, group_branch);
                if (resu)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void GenCodept()
        {
            try
            {

                DateTime deptopen_date = DateTime.ParseExact(opendate.Value, "dd/MM/yyyy", WebUtil.EN); 
                String group_branch = groupBranch.Value;
                bool resu = WsUtil.Walfare.GenCodept(state.SsWsPass, state.SsApplication, deptopen_date, group_branch);
                if (resu)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void  GenNewCsType()
        {
            try
            {
                bool resu = WsUtil.Walfare.GenNewCsType(state.SsWsPass, state.SsApplication,state.SsCsType);
                if (resu)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ClickLogin()
        {
            try
            {
                if (passlogin.Value == "herosonic")
                {
                    Tmain.Visible = true;
                    Login.Visible = false;
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("รหัสผ่านไม่ถูกต้อง");
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}