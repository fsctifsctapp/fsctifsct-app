using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Sybase.DataWindow;
using DBAccess;
using System.Data;
using System.Globalization;
using GcoopServiceCs;
using System.Windows.Forms;
using DTG.Spreadsheet;

namespace Saving.Applications.walfare
{
    public partial class w_sheet_wc_expexcel_membcard : PageWebSheet, WebSheet
    {
        protected String SearchData;
        protected String SelectBranch;
        DataTable table = new DataTable();

        public void InitJsPostBack()
        {
            SearchData = WebUtil.JsPostBack(this, "SearchData");
            SelectBranch = WebUtil.JsPostBack(this, "SelectBranch");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                DwMain.SetItemString(1, "start_tdate", "01/01/" + Syear);
                DwMain.SetItemString(1, "end_tdate", "31/12/" + Syear);
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
                case "SelectBranch":
                    SelectBranchToSet();
                    break;

                case "SearchData":
                    initSearchData();
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "sta_branch", "w_sheet_wc_export_excel.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "end_branch", "w_sheet_wc_export_excel.pbl", null);
                if (!IsPostBack)
                {
                    String sql = "select min(coopbranch_id) as minbranch, max(coopbranch_id) maxbranch from cmucfcoopbranch where cs_type = '8'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        string minbranch = dt.GetString("minbranch");
                        string maxbranch = dt.GetString("maxbranch");

                        DwMain.SetItemString(1, "sta_branch", minbranch);
                        DwMain.SetItemString(1, "end_branch", maxbranch);
                    }
                }
            }
            catch { }
            DwMain.SaveDataCache();
        }
        private void SelectBranchToSet()
        {
            try
            {
                string sbranch = DwMain.GetItemString(1, "s_branch");
                DwMain.SetItemString(1, "sta_branch", sbranch);
            }
            catch { }
            try
            {
                DwMain.SetItemString(1, "end_branch", DwMain.GetItemString(1, "e_branch"));
            }
            catch { }
        }

        public void initSearchData()
        {
            try
            {
                table.Columns.Add("ลำดับ");
                table.Columns.Add("ชื่อ - นามสกุล");
                table.Columns.Add("บัตรประชาชน");
                table.Columns.Add("เลขฌาปนกิจ");
                table.Columns.Add("ชื่อสหกรณ์");
                table.Columns.Add("วันคุ้มครอง/วันออกบัตร");

                string eDateTH = DwMain.GetItemString(1, "end_tdate");
                string sDateTH = DwMain.GetItemString(1, "start_tdate");
                string sDateEN = sDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(sDateTH.Substring(6, 4)) - 543);
                string eDateEN = eDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(eDateTH.Substring(6, 4)) - 543);
                string sbranch = DwMain.GetItemString(1, "sta_branch");
                string ebranch = DwMain.GetItemString(1, "end_branch");
                String sql;
                try
                {
                    sql = @"select wr.wfaccount_name as wfaccount_name, wr.card_person as card_person, wr.deptaccount_no as deptaccount_no, cb.coopbranch_desc as
                    coopbranch_desc, wr.deptopen_date as deptopen_date from wcdeptmaster wr left join cmucfcoopbranch cb on (wr.branch_id = cb.coopbranch_id)
                    where wr.branch_id between '" + sbranch + "' and '" + ebranch + @"' and
                    wr.deptopen_date between to_date('" + sDateEN + @"', 'dd/mm/yyyy') and to_date('" + eDateEN + @"', 'dd/mm/yyyy')
                    and wr.deptclose_status = 0 and cb.cs_type = '8' order by wr.branch_id, wr.deptopen_date, wr.deptaccount_no";
                    /*
                    sql = @"select wr.deptaccount_no as docno, wr.member_no as member_no, mp.prename_desc as prename_desc, wr.deptaccount_name as deptaccount_name
                    , wr.deptaccount_sname as deptaccount_sname, wr.card_person as card_person, wr.wfbirthday_date as wfbirthday_date
                    , wr.deptopen_date as deptopen_date, wr.apply_date as apply_date, wr.contact_address as addr, md.district_desc as district
                    , mpn.province_desc as province, md.postcode as mdpostcode, wr.postcode as wrpostcode, wr.phone as hometel
                    from wcdeptmaster wr left join mbucfprename mp on (wr.prename_code = mp.prename_code) 
                    left join mbucfdistrict md on (wr.ampher_code = md.district_code) left join mbucfprovince mpn
                    on (wr.province_code = mpn.province_code) where wr.branch_id = '" + state.SsBranchId + @"' and
                    wr.deptopen_date between to_date('" + sDateEN + @"', 'dd/mm/yyyy') and to_date('" + eDateEN + @"', 'dd/mm/yyyy')
                    and wr.deptclose_status = 0 order by wr.deptopen_date, wr.deptrequest_docno";
                     * */


                    Sdt dt = WebUtil.QuerySdt(sql);
                    int i = 1;

                    table.Rows.Add(new object[] { "ลำดับ", "ชื่อ - นามสกุล", "บัตรประชาชน", "เลขฌาปนกิจ", "ชื่อสหกรณ์", "วันคุ้มครอง/วันออกบัตร" });

                    string deptaccount_no, wfaccount_name, card_person, coopbranch_desc, openday;
                    DateTime opendate;
                    while (dt.Next())
                    {
                        deptaccount_no = dt.GetString("deptaccount_no");
                        wfaccount_name = dt.GetString("wfaccount_name");
                        //card_person = String.Format(dt.GetString("card_person"), "# ### ##### ## #");                        
                        card_person = dt.GetDecimal("card_person").ToString("# #### ##### ## #").Trim();
                        coopbranch_desc = "สหกรณ์ออมทรัพย์ " + dt.GetString("coopbranch_desc") + " จำกัด";
                        opendate = dt.GetDate("deptopen_date");
                        int day, imonth, year;
                        day = opendate.Day;
                        imonth = opendate.Month;
                        string smonth = "";
                        switch (imonth)
                        {
                            case 1:
                                smonth = "มกราคม";
                                break;
                            case 2:
                                smonth = "กุมภาพันธ์";
                                break;
                            case 3:
                                smonth = "มีนาคม";
                                break;
                            case 4:
                                smonth = "เมษายน";
                                break;
                            case 5:
                                smonth = "พฤษภาคม";
                                break;
                            case 6:
                                smonth = "มิถุนายน";
                                break;
                            case 7:
                                smonth = "กรกฏาคม";
                                break;
                            case 8:
                                smonth = "สิงหาคม";
                                break;
                            case 9:
                                smonth = "กันยายน";
                                break;
                            case 10:
                                smonth = "ตุลาคม";
                                break;
                            case 11:
                                smonth = "พฤศจิกายน";
                                break;
                            case 12:
                                smonth = "ธันวาคม";
                                break;
                        }
                        year = opendate.Year + 543;
                        openday = day + " " + smonth + " " + year;


                        table.Rows.Add(new object[] { i, wfaccount_name, card_person, deptaccount_no, coopbranch_desc, openday });
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
                //table.Rows.Add(new object[] { "1", "Rodney", "Johnson", "M", "06.07.1964", "Adult literacy teacher" });
                //table.Rows.Add(new object[] { "2", "Connie", "King", "F", "07.19.1989", "Occupational health and safety technician" });
                //table.Rows.Add(new object[] { "3", "Steven", "Gray", "M", "12.24.1977", "Telephone repairer" });
                //table.Rows.Add(new object[] { "4", "Cynthia", "Manigault", "F", "03.19.1969", "Employee assistance plan manager" });
                //table.Rows.Add(new object[] { "5", "Gracie", "Salas", "F", "07.26.1967", "Heavy vehicle service technician" });
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            GridViewExcel.DataSource = table;
            GridViewExcel.DataBind();
            GridViewExcel.HeaderRow.Visible = false;
            GridViewExcel.Rows[0].BackColor = System.Drawing.Color.Gray;

            GridViewExcel.Width = 3000;
            //
            Session["data"] = table;
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            try
            {
                // 1 method: Export DataTable to Excel
                ExcelWorkbook Wbook = new ExcelWorkbook();
                ExcelWorksheet Wsheet = Wbook.Worksheets.Add("Employees");
                DataTable tb = new DataTable();
                tb = (DataTable)Session["data"];
                Session["data"] = null;
                Wsheet.ReadFromDataTable(tb);

                // 2 method: Export GridView to Excel directly
                // (uncomment to try this method)

                //int rows = GridView1.Rows.Count;
                //int cols = GridView1.Columns.Count;
                //for (int r = 0; r < rows; r++)
                //{
                //    for (int c = 0; c < cols; c++)
                //    {
                //        string val = GridView1.Rows[r].Cells[c].Text;
                //        Wsheet.Cells[r, c].Value = val;
                //    }
                //}

                // Select the Excel file type for export
                string formatFile = "XLS";
                switch (formatFile)
                {
                    case "XLS":
                        {
                            Response.ContentType = "application/vnd.ms-excel";
                            Response.AddHeader("content-disposition", "attachment;filename=" + state.SsCsType + "_" + state.SsBranchId + ".xls");
                            Response.BinaryWrite(Wbook.WriteXLS().ToArray());
                            Response.Flush();
                            Response.End();
                            break;
                        }
                    case "XLSX":
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(Wbook.WriteXLSX().ToArray());
                            Response.Flush(); Response.End();
                            break;
                        }
                    case "CSV":
                        {
                            Response.ContentType = "text/comma-separated-values";
                            Response.BinaryWrite(Wbook.WriteCSV().ToArray());
                            Response.Flush(); Response.End();
                            break;
                        }
                    case "HTML":
                        {
                            Response.ContentType = "text/html";
                            Response.BinaryWrite(Request.BinaryRead(Request.TotalBytes));
                            Response.Flush(); Response.End();
                            break;
                        }
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลไม่สามารถทำรายการได้");
            }
        }
    }
}