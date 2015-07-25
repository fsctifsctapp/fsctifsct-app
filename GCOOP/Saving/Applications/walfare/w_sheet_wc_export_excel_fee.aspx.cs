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
    public partial class w_sheet_wc_export_excel_fee : PageWebSheet, WebSheet
    {
        protected String SearchData;
        DataTable table = new DataTable();

        public void InitJsPostBack()
        {
            SearchData = WebUtil.JsPostBack(this, "SearchData");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                //DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
               // DwMain.SetItemString(1, "start_tdate", "01/01/" + Syear);
               // DwMain.SetItemString(1, "end_tdate", "31/12/" + Syear);
                DwMain.SetItemString(1, "as_cstype", state.SsCsType);
                DwMain.SetItemString(1, "branch_idd", state.SsBranchId);
                DwMain.SetItemString(1, "branch_id", state.SsBranchId);
                DwMain.SetItemString(1, "year", Syear);
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
            string not_cstype, cstype;

            not_cstype = "0";
            cstype = state.SsCsType;

            try
            {

                DwUtil.RetrieveDDDW(DwMain, "as_cstype", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "branch_idd", "w_sheet_wc_trn_memb.pbl", cstype, not_cstype);


            }
            catch { }

            DwMain.SaveDataCache();
        }

        public void initSearchData()
        {
            try
            {
                table.Columns.Add("ลำดับ");
                table.Columns.Add("เลขณาปนกิจ");
                table.Columns.Add("เลขสมาชิก สอ.");
                table.Columns.Add("ชื่อ - สกุล");
                table.Columns.Add("บัตรประชาชน");
                table.Columns.Add("วันที่เริ่มเป็นสมาชิก");
                table.Columns.Add("จำนวนเรียกเก็บ");
                table.Columns.Add("สถานะ");

                String ascstype = state.SsCsType;
                String branch_id = DwMain.GetItemString(1, "branch_id");
                String status = DwMain.GetItemString(1, "status");
                String year = DwMain.GetItemString(1, "year");
                Decimal yearr = Convert.ToInt32(year);
                String ym = yearr - 1 + "12";
                Decimal all = DwMain.GetItemDecimal(1, "all");

                if (all == 1)
                {
                    status = "%";
                }
                String sql;
               // int RowCount;
                //bool RowFirst = false;
                try
                {
                    
                       sql = @" select
                                re.wfmember_no as wfmember_no,
                                re.member_no as member_no,
                                dem.wfaccount_name as wfaccount_name,
                                dem.deptopen_date as deptopen_date,
                                dem.card_person as card_person,
                                re.fee_year as fee_year,
                                re.status_post as status_post,
                                cmcb.CS_NAME  as CS_NAME,   
                                cmb.COOPBRANCH_DESC as COOPBRANCH_DESC,
                                re.branch_id as branch_id 
                                from wcrecievemonth re,
                                wcdeptmaster dem,
                                CMUCFCOOPBRANCH cmb,
                                CMUCFCREMATION cmcb
                                where (re.wfmember_no = dem.deptaccount_no)
                                and (re.branch_id = cmb.coopbranch_id)
                                and (cmb.cs_type = cmcb.cs_type) 
                                and( re.wcitemtype_code = 'FEE'
                                and re.recv_period = '" + ym + @"'
                                and re.branch_id = '" + branch_id + @"'
                                and re.status_post like '" + status + @"'
                                and cmb.cs_type = '" + ascstype + @"')";
                                     
                    Sdt dt = WebUtil.QuerySdt(sql);
                    int i = 1, j;

                    table.Rows.Add(new object[] { "ลำดับ", "เลขณาปนกิจ", "เลขสมาชิก สอ.", "ชื่อ - สกุล", "บัตรประชาชน", "วันที่เริ่มเป็นสมาชิก", "จำนวนเรียกเก็บ", "สถานะ" });


                    string wfmember_no, member_no, wfaccount_name, deptopen_date, fee_year, status_post, status_desc, card_person;
                    while (dt.Next())
                    {
                      
                        wfmember_no = dt.GetString("wfmember_no");
                        member_no = dt.GetString("member_no");
                        wfaccount_name = dt.GetString("wfaccount_name");
                        card_person = dt.GetString("card_person");
                        deptopen_date = dt.GetDateTh("deptopen_date").ToString();
                        fee_year = dt.GetDecimal("fee_year").ToString();
                        status_post = dt.GetDecimal("status_post").ToString();
                        status_desc = "";

                        status_desc = status_post;
                        switch (status_desc)
                        {
                            case "1":
                                status_desc = "ชำระแล้ว";
                                break;
                            case "2":
                                status_desc = "รอสมาคมตรวจสอบ";
                                break;
                            case "8":
                                status_desc = "ยังไม่ชำระ";
                                break;
                            case "9":
                                status_desc = "ไม่ชำระ";
                                break;
                            case "-9":
                                status_desc = "สิ้นสุด";
                                break;
                            default:
                                status_desc = "ไม่ระบุ";
                                break;
                        }



                        table.Rows.Add(new object[] { i, wfmember_no, member_no, wfaccount_name, card_person, deptopen_date, fee_year, status_desc });
                               
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
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            GridViewExcel.DataSource = table;
            GridViewExcel.DataBind();
            GridViewExcel.HeaderRow.Visible = false;
            GridViewExcel.Rows[0].BackColor = System.Drawing.Color.Gray;

            GridViewExcel.Width = 3300;

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
                            Response.AddHeader("content-disposition", "attachment;filename=" + state.SsCsType + "FEE_" + state.SsBranchId + ".xls");
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