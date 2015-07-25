using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Applications.walfare.dlg
{
    public partial class w_dlg_wc_inform_new_admin : PageWebDialog, WebDialog
    {
        protected String postJsShowlist;
        public void InitJsPostBack()
        {
            postJsShowlist = WebUtil.JsPostBack(this, "postJsShowlist");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postJsShowlist")
            {
                JsShowlist();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }
        private void JsShowlist()
        {
            //String deptaccount_no, member_no, card_person, deptaccount_name, deptaccount_sname;

            //try { deptaccount_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no")); }
            //catch { deptaccount_no = ""; }
            //try { member_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no")); }
            //catch { member_no = ""; }
            //try { card_person = DwMain.GetItemString(1, "card_person"); }
            //catch { card_person = ""; }
            //try { deptaccount_name = "%" + DwMain.GetItemString(1, "deptaccount_name") + "%"; }
            //catch { deptaccount_name = ""; }
            //try { deptaccount_sname = "%" + DwMain.GetItemString(1, "deptaccount_sname") + "%"; }
            //catch { deptaccount_sname = ""; }

            //if (deptaccount_no == "" && member_no.Trim() == "" && card_person.Trim() == "" && deptaccount_sname.Trim() == "" & deptaccount_name.Trim() == "") deptaccount_name = "%";

            //try
            //{
            //    DwUtil.RetrieveDataWindow(DwList, "w_sheet_wc_inform.pbl", null, member_no, deptaccount_name, deptaccount_sname, card_person, state.SsBranchId, deptaccount_no);
            //}
            //catch
            //{
            //}
            String SQLcon = "", deptaccount_no, member_no, card_person, deptaccount_name, deptaccount_sname;
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            DwList.SetTransaction(SQLCA);
            String SQLBegin = DwList.GetSqlSelect();

            try
            {
                deptaccount_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "deptaccount_no"));
            }
            catch { deptaccount_no = ""; }
            try
            {
                member_no = WebUtil.MemberNoFormat(DwMain.GetItemString(1, "member_no"));
            }
            catch { member_no = ""; }
            try
            {
                card_person = DwMain.GetItemString(1, "card_person");
            }
            catch { card_person = ""; }
            try
            {
                deptaccount_name = DwMain.GetItemString(1, "deptaccount_name");
            }
            catch { deptaccount_name = ""; }
            try
            {
                deptaccount_sname = DwMain.GetItemString(1, "deptaccount_sname");
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
                String SQL;
                if (SQLcon == "")
                {
                    SQL = SQLBegin ;
                }
                else
                {
                    SQL = SQLBegin + SQLcon +  "ORDER BY wcdeptmaster.deptaccount_no ASC";
                }
                DwList.SetSqlSelect(SQL);
                DwList.Retrieve();
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