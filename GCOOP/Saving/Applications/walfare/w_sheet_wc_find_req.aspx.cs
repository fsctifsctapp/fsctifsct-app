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
    public partial class w_sheet_wc_find_req : PageWebSheet, WebSheet
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
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwCri.SaveDataCache();
        }

        private void InitPerson()
        {
            String SQLcon = "", member_no, card_person, deptaccount_name, deptaccount_sname;
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            DwMain.SetTransaction(SQLCA);
            String SQLBegin = DwMain.GetSqlSelect();

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


            if (member_no == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcreqdeposit.member_no = '" + member_no + "'";
            }
            if (card_person == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcreqdeposit.card_person = '" + card_person + "'";
            }

            if (deptaccount_name == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcreqdeposit.deptaccount_name like '%" + deptaccount_name + "%'";
            }

            if (deptaccount_sname == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                SQLcon = SQLcon + " and wcreqdeposit.deptaccount_sname like '%" + deptaccount_sname + "%'";
            }

            try
            {

                String SQL = SQLBegin + SQLcon + "and cmucfcoopbranch.cs_type = '" + state.SsCsType + "' ";
                DwMain.SetSqlSelect(SQL);

                DwMain.Retrieve();
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