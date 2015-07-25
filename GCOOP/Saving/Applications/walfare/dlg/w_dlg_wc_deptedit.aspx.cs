using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;

namespace Saving.Applications.walfare.dlg
{
    public partial class w_dlg_wc_deptedit : PageWebDialog, WebDialog
    {
        protected String postJsShowlist;
        private DwThDate tDwList;

        public void InitJsPostBack()
        {
            postJsShowlist = WebUtil.JsPostBack(this, "postJsShowlist");
            tDwList = new DwThDate(DwList, this);
            tDwList.Add("wcdeptmaster_deptopen_date", "deptopen_tdate");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                try
                {
                    HdBranch.Value = Request["branch_id"];
                   
                }
                catch
                {
                    HdBranch.Value = "";
                }
                ///EXTRA SEARCH MODE
                //try
                //{
                //    string mode = Request["searchMode"];
                //    if (mode == "member")
                //    {
                //        DwMain.SetItemString(1, "member_no", Request["searchCode"]);
                //        HdExtraSearchMode.Value = "member";
                //    }
                //    else if (mode == "card")
                //    {
                //        DwMain.SetItemString(1, "card_person", Request["searchCode"]);
                //        HdExtraSearchMode.Value = "card";
                //    }
                //}
                //catch { }
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
            tDwList.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
          
        }

        private void JsShowlist()
        {
            HdExtraSearchMode.Value = "";

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

            //if (deptaccount_no.Trim() == "" && member_no.Trim() == "" && card_person.Trim() == "" && deptaccount_sname.Trim() == "" & deptaccount_name.Trim() == "") deptaccount_name = "%";

            //try
            //{
            //    DwUtil.RetrieveDataWindow(DwList, "w_sheet_wc_membermaster.pbl", null, member_no, deptaccount_name, deptaccount_sname, card_person, state.SsBranchId, deptaccount_no);
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}

            String SQLcon = "", deptaccount_no, member_no, card_person, deptaccount_name, deptaccount_sname, round_st, round_end;
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            DwList.SetTransaction(SQLCA);
            String SQLBegin = DwList.GetSqlSelect();
            string sDateEN, eDateEN;

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
            try
            {
                round_st = DwMain.GetItemString(1, "round_st");
            }
            catch { round_st = ""; }
            try
            {
                round_end = DwMain.GetItemString(1, "round_end");
            }
            catch { round_end = ""; }



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
            if (round_st == "" && round_end == "")
            {
                SQLcon = SQLcon + "";
            }
            else
            {
                sDateEN = round_st.Substring(0, 6) + Convert.ToString(Convert.ToInt32(round_st.Substring(6, 4)) - 543);
                eDateEN = round_end.Substring(0, 6) + Convert.ToString(Convert.ToInt32(round_end.Substring(6, 4)) - 543);
                SQLcon = SQLcon + " and wcdeptmaster.deptopen_date between to_date('" + sDateEN + "', 'dd/mm/yyyy') and to_date('" + eDateEN + "', 'dd/mm/yyyy')";
            }


            try
            {
                string branch_id = state.SsBranchId;
                if (HdBranch.Value.Trim() != "")
                {
                    branch_id = HdBranch.Value;
                }
                String SQL;
                if (SQLcon == "")
                {
                    SQL = SQLBegin + " and wcdeptmaster.branch_id = '" + branch_id + "'";
                }
                else
                {
                    SQL = SQLBegin + SQLcon + " and wcdeptmaster.branch_id = '" + branch_id + "'";
                    
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