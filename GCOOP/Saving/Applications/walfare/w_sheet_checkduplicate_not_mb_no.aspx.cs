using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;

namespace Saving.Applications
{
    public partial class w_sheet_checkduplicate_not_mb_no : PageWebSheet, WebSheet
    {

        public void InitJsPostBack()
        {

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
            }
            else
            {
                this.RestoreContextDw(DwMain);
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            String sql1 = @"
                        select * from (
                        select
                        r.card_person as card_person
                        from wcreqdeposit r
                        where r.branch_id in (
                                select coopbranch_id from cmucfcoopbranch
                                where cs_type = '" + state.SsCsType + @"'
                                )
                        and r.deptaccount_no = (
                                select m.deptaccount_no from wcdeptmaster m 
                                where m.deptaccount_no = r.deptaccount_no 
			                    and m.deptclose_status = 0 
                                and r.branch_id = m.branch_id
                                )
                        and r.approve_status = 1
                        group by
                        r.card_person
                        having
                        count(r.card_person)>1
                        
                        union

                        select
                        r.card_person as card_person
                        from wcreqdeposit r
                        where r.branch_id in (
                                select coopbranch_id from cmucfcoopbranch
                                where cs_type = '" + state.SsCsType + @"'
                                )
                        and r.deptaccount_no = (
                                select m.deptaccount_no from wcdeptmaster m 
                                where m.deptaccount_no = r.deptaccount_no 
			                    and m.deptclose_status = 0 
                                and r.branch_id = m.branch_id
                                )
                        and r.approve_status = 1
                        group by
                        r.card_person
                        having
                        count(r.card_person)>1
                        )";
            Sdt dt1 = WebUtil.QuerySdt(sql1);

            String sql2, sql_prename, sql_branch;
            string prename = "", memb_name, branch_desc = "";
            Sdt dt2, dt3, dt4;
            int i = 1;
            while (dt1.Next())
            {
                sql2 = "select * from wcreqdeposit where card_person = '" + dt1.GetString("card_person") + "'";

                dt2 = WebUtil.QuerySdt(sql2);

                while (dt2.Next())
                {
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(i, "deptrequest_docno", dt2.GetString("deptrequest_docno"));
                    DwMain.SetItemString(i, "member_no", dt2.GetString("member_no"));
                    DwMain.SetItemString(i, "card_person", dt2.GetString("card_person"));

                    sql_prename = "select prename_desc from mbucfprename where prename_code = '" + dt2.GetString("prename_code") + "'";
                    dt3 = WebUtil.QuerySdt(sql_prename);
                    if (dt3.Next())
                    {
                        prename = dt3.GetString("prename_desc");
                    }
                    memb_name = prename + dt2.GetString("deptaccount_name") + "   " + dt2.GetString("deptaccount_sname");
                    DwMain.SetItemString(i, "memb_name", memb_name);

                    sql_branch = "select coopbranch_desc from cmucfcoopbranch where coopbranch_id = '" + dt2.GetString("branch_id") + "'";
                    dt4 = WebUtil.QuerySdt(sql_branch);
                    if (dt4.Next())
                    {
                        branch_desc = dt4.GetString("coopbranch_desc");
                    }
                    DwMain.SetItemString(i, "branch_id", dt2.GetString("branch_id"));

                    DwMain.SetItemString(i, "branch_desc", branch_desc);
                    DwMain.SetItemString(i, "deptopen_date", dt2.GetDateTh("deptopen_date"));
                    i++;
                }
            }


            DwMain.SaveDataCache();
        }
    }
}