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
    public partial class w_sheet_wc_fine_person : PageWebSheet, WebSheet
    {
        protected String initPerson;

        public void InitJsPostBack()
        {
            initPerson = WebUtil.JsPostBack(this, "initPerson");
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
                case "initPerson":
                    InitPerson();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                int count = 0; 
                for (int i = 0; i < DwMain.RowCount; i++)
                {
                    String resigncause_code = DwMain.GetItemString(i + 1, "resigncause_code");
                    if (resigncause_code == "03")
                    {
                        count++;
                        Decimal status = DwMain.GetItemDecimal(i + 1, "status");
                        if (status == 1)
                        {

                            String xmlMain = DwMain.Describe("DataWindow.Data.XML");
                            bool result = false;
                            result = WsUtil.Walfare.SavePersonChgStatus(state.SsWsPass, state.SsApplication, "w_sheet_duplicate.pbl", xmlMain);

                            if (result)
                            {
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                            }
                        }
                        else
                            LtServerMessage.Text = WebUtil.WarningMessage("คุณยังไม่ได้ติ๊ก 'คืนสภาพ' !!!");
                    }
                    if(count == 0)
                       LtServerMessage.Text = WebUtil.WarningMessage("สำหรับคืนสภาพสมาชิก 'ยกเลิก' เท่านั้น !!!");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }


        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwCri.SaveDataCache();
        }

        private void InitPerson()
        {            
            ///try { member_no = WebUtil.MemberNoFormat(DwCri.GetItemString(1, "member_no")); }
            ///catch { member_no = ""; }
            ///try { card_person = DwCri.GetItemString(1, "card_person"); }
            ///catch { card_person = ""; }
            ///try { deptaccount_name = "%" + DwCri.GetItemString(1, "deptaccount_name") + "%"; }
            ///catch { deptaccount_name = ""; }
            ///try { deptaccount_sname = "%" + DwCri.GetItemString(1, "deptaccount_sname") + "%"; }
            ///catch { deptaccount_sname = ""; }

            ///if (member_no.Trim() == "" && card_person.Trim() == "" && deptaccount_sname.Trim() == "" && deptaccount_name.Trim() == "")
            ///{
            ///    deptaccount_name = "%";
            ///}
            ///else
            ///{
            ///}
            ///try { SQLcon = SQLcon + " and wcdeptmaster.member_no = '" + WebUtil.MemberNoFormat(DwCri.GetItemString(1, "member_no")) + "'"; }
            ///catch { SQLcon = SQLcon + ""; }
            ///try { SQLcon = SQLcon + " and wcdeptmaster.card_person = '" + DwCri.GetItemString(1, "card_person") + "'"; }
            ///catch { SQLcon = SQLcon + ""; }
            ///try { SQLcon = SQLcon + " and wcdeptmaster.deptaccount_name like '%" + DwCri.GetItemString(1, "deptaccount_name") + "%'"; }
            ///catch { SQLcon = SQLcon + ""; }
            ///try { SQLcon = SQLcon + " and wcdeptmaster.deptaccount_sname like '%" + DwCri.GetItemString(1, "deptaccount_sname") + "%'"; }
            ///catch { SQLcon = SQLcon + ""; }
            ///
            String SQLcon = "", member_no, card_person, deptaccount_name, deptaccount_sname, deptaccount_no;
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            DwMain.SetTransaction(SQLCA);
            String SQLBegin = DwMain.GetSqlSelect();
            try
            {
                deptaccount_no = int.Parse(DwCri.GetItemString(1, "deptaccount_no")).ToString("000000");
            }
            catch { deptaccount_no = ""; }
            try
            {
                member_no = WebUtil.MemberNoFormat(DwCri.GetItemString(1, "member_no"));
            }
            catch { member_no = ""; }
            try
            {
                card_person = DwCri.GetItemString(1, "card_person");
            }
            catch { card_person = ""; }
            try
            {
                deptaccount_name = DwCri.GetItemString(1, "deptaccount_name");
            }
            catch { deptaccount_name = ""; }
            try
            {
                deptaccount_sname = DwCri.GetItemString(1, "deptaccount_sname");
            }
            catch { deptaccount_sname = ""; }


            if (deptaccount_no == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcdeptmaster.deptaccount_no = '" + deptaccount_no + "'";
            }
            if (member_no == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcdeptmaster.member_no = '" + member_no + "'";
            }
            if (card_person == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcdeptmaster.card_person = '" + card_person + "'";
            }

            if (deptaccount_name == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcdeptmaster.deptaccount_name like '%" + deptaccount_name + "%'";
            }

            if (deptaccount_sname == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcdeptmaster.deptaccount_sname like '%" + deptaccount_sname + "%'";
            }

            try
            {
                if (SQLcon == "")
                {
                    //SQLcon = "%";
                    if (state.SsCsType == "1" || state.SsCsType == "8")
                    SQLcon = "and cmucfcoopbranch.cs_type = '" + state.SsCsType + "'";
                    else
                    SQLcon = "%";
                }
                else
                {
                    if (state.SsCsType == "1" || state.SsCsType == "8")
                    SQLcon = SQLcon + "and cmucfcoopbranch.cs_type = '" + state.SsCsType + "'";

                    String SQL = SQLBegin + SQLcon;
                    DwMain.SetSqlSelect(SQL);
                }

                DwMain.Retrieve();
                //DwUtil.RetrieveDataWindow(DwMain, "w_sheet_duplicate.pbl", null, member_no, deptaccount_name, deptaccount_sname, card_person);
                //DwUtil.RetrieveDataWindow(DwMain, "w_sheet_duplicate.pbl", null, null);
                SQLCA.Disconnect();
            }
            catch 
            {
                SQLCA.Disconnect();
                LtServerMessage.Text = WebUtil.ErrorMessage("แก้ข้อผิดพลาด ไม่สามารถค้นหาข้อมูลได้");
            }

        }
    }
}