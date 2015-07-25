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
    public partial class w_sheet_wc_export_excel : PageWebSheet, WebSheet
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
            DwMain.SaveDataCache();
        }

        public void initSearchData()
        {
            try
            {
                table.Columns.Add("ลำดับ");
                table.Columns.Add("#");
                table.Columns.Add("เลขสมาชิก สอ.");
                table.Columns.Add("ประเภทสมาชิก");
                table.Columns.Add("คำนำหน้า");
                table.Columns.Add("ชื่อ");
                table.Columns.Add("สกุล");
                table.Columns.Add("บัตรประชาชน");
                table.Columns.Add("วันเกิด");
                table.Columns.Add("วันสมัคร");
                table.Columns.Add("วันที่เริ่มเป็นสมาชิก");
                table.Columns.Add("รอบ");
                table.Columns.Add("ว/ด/ป ที่ส่งเงินคงสภาพ");
                table.Columns.Add("ชื่อสามีหรือภรรยา");
                table.Columns.Add("ชื่อผู้จัดการศพที่ระบุไว้");
                table.Columns.Add("ที่อยู่(ที่สามารถติดต่อได้)");
                table.Columns.Add("อำเภอ");
                table.Columns.Add("จังหวัด");
                table.Columns.Add("ไปรษณีย์");
                table.Columns.Add("โทรศัพท์");
                table.Columns.Add("คนที่");
                table.Columns.Add("ผู้รับเงินสงเคราะห์");
                table.Columns.Add("บัตรประชาชนผู้รับเงินสงเคราะห์");
                table.Columns.Add("ที่อยุ่ผู้รับเงินสงเคราะห์");
                table.Columns.Add("สาเหตุการพ้นสภาพ");
                table.Columns.Add("วันที่พ้นสภาพ");
                table.Columns.Add("ศูนย์ประสานงาน(สอ.)");
                table.Columns.Add("หมายเหตุ");

                string eDateTH = DwMain.GetItemString(1, "end_tdate");
                string sDateTH = DwMain.GetItemString(1, "start_tdate");
                string sDateEN = sDateTH.Substring(0,6) + Convert.ToString(Convert.ToInt32(sDateTH.Substring(6,4))-543);
                string eDateEN = eDateTH.Substring(0, 6) + Convert.ToString(Convert.ToInt32(eDateTH.Substring(6, 4))-543);
                String sql, CoSql;
                int RowCount;
                bool RowFirst = false;
                try
                {
                    string report_type;
                    try
                    {
                        report_type = DwMain.GetItemString(1, "report_type");
                    }
                    catch
                    { 
                        report_type = "99";
                    }
                    if (report_type == "01")
                    {
                        sql = @"select wr.deptrequest_docno as docno, wr.remark as remark, wr.member_no as member_no, mp.prename_desc as prename_desc, wr.deptaccount_name as deptaccount_name
                        , wr.deptaccount_sname as deptaccount_sname, wr.card_person as card_person, wr.wfbirthday_date as wfbirthday_date, wr.deptopen_date as deptopen_date
                        , '' as effective_date, wr.apply_date as apply_date, wr.other_contact_address as addr, md.district_desc as district, wr.mate_name as mate_name
                        , mpn.province_desc as province, md.postcode as mdpostcode, wr.other_postcode as wrpostcode, wr.hometel as hometel, wr.manage_corpse_name as manage_corpse_name
                        , wt.wcmembertype_desc as wftype_desc,wr.wftype_code as wftype_code
                        ,(select  round_regis from wcucfroundregisfixed where cs_type = wt.cs_type and deptopen_date = wr.deptopen_date) as round_regis
                        from wcreqdeposit wr left join mbucfprename mp on (wr.prename_code = mp.prename_code)
                        left join mbucfdistrict md on (wr.other_ampher_code = md.district_code) left join mbucfprovince mpn
                        on (wr.other_province_code = mpn.province_code) left join wcmembertype wt on(wr.wftype_code = wt.wftype_code)
                        where wr.branch_id = '" + state.SsBranchId + @"' and wt.cs_type = '" + state.SsCsType + @"' and
                        wr.deptopen_date between to_date('" + sDateEN + @"', 'dd/mm/yyyy') and to_date('" + eDateEN + @"', 'dd/mm/yyyy')
                        and approve_status <> -9 order by wr.deptopen_date, wr.deptrequest_docno";
                    }
                    else if (report_type == "02")
                    {
                        sql = @"select wr.deptaccount_no as docno, wr.remark as remark, wr.member_no as member_no, mp.prename_desc as prename_desc, wr.deptaccount_name as deptaccount_name
                        , wr.deptaccount_sname as deptaccount_sname, wr.card_person as card_person, wr.wfbirthday_date as wfbirthday_date, wr.deptopen_date as deptopen_date
                        , wr.effective_date as effective_date, wr.apply_date as apply_date, wr.other_contact_address as addr, md.district_desc as district, wr.mate_name as mate_name
                        , mpn.province_desc as province, md.postcode as mdpostcode, wr.other_postcode as wrpostcode, wr.phone as hometel, wr.manage_corpse_name as manage_corpse_name
                        , wr.deptclose_date as deptclose_date, wr.resigncause_code as resigncause_code, wt.wcmembertype_desc as wftype_desc, wr.wftype_code as wftype_code
                        , (select coopbranch_desc from cmucfcoopbranch where coopbranch_id = wr.branch_id and cs_type = wt.cs_type) as coopbranch_desc
                        ,(select  round_regis from wcucfroundregisfixed where cs_type = wt.cs_type and deptopen_date = wr.deptopen_date) as round_regis
                        from wcdeptmaster wr left join mbucfprename mp on (wr.prename_code = mp.prename_code)
                        left join mbucfdistrict md on (wr.other_ampher_code = md.district_code) left join mbucfprovince mpn
                        on (wr.other_province_code = mpn.province_code) left join wcmembertype wt on(wr.wftype_code = wt.wftype_code)
                        where wr.branch_id = '" + state.SsBranchId + @"' and wt.cs_type = '" + state.SsCsType + @"' and 
                        wr.deptopen_date between to_date('" + sDateEN + @"', 'dd/mm/yyyy') and to_date('" + eDateEN + @"', 'dd/mm/yyyy')
                        and (wr.resigncause_code <>'03' or wr.resigncause_code is null) order by wr.deptopen_date, wr.deptaccount_no";
                    }
                    else if (report_type == "03")
                    {
                        sql = @"select wr.deptaccount_no as docno, wr.remark as remark, wr.member_no as member_no, mp.prename_desc as prename_desc, wr.deptaccount_name as deptaccount_name
                        , wr.deptaccount_sname as deptaccount_sname, wr.card_person as card_person, wr.wfbirthday_date as wfbirthday_date, wr.deptopen_date as deptopen_date
                        , wr.effective_date as effective_date, wr.apply_date as apply_date, wr.other_contact_address as addr, md.district_desc as district, wr.mate_name as mate_name
                        , mpn.province_desc as province, md.postcode as mdpostcode, wr.other_postcode as wrpostcode, wr.phone as hometel, wr.manage_corpse_name as manage_corpse_name
                        , wr.deptclose_date as deptclose_date, wr.resigncause_code as resigncause_code, wt.wcmembertype_desc as wftype_desc, wr.wftype_code as wftype_code
                        , (select coopbranch_desc from cmucfcoopbranch where coopbranch_id = wr.branch_id and cs_type = wt.cs_type) as coopbranch_desc
                        ,(select  round_regis from wcucfroundregisfixed where cs_type = wt.cs_type and deptopen_date = wr.deptopen_date) as round_regis
                        from wcdeptmaster wr left join mbucfprename mp on (wr.prename_code = mp.prename_code) 
                        left join mbucfdistrict md on (wr.other_ampher_code = md.district_code) left join mbucfprovince mpn
                        on (wr.other_province_code = mpn.province_code) left join wcmembertype wt on(wr.wftype_code = wt.wftype_code)
                        where wr.branch_id = '" + state.SsBranchId + @"' and wt.cs_type = '" + state.SsCsType + @"' and 
                        wr.deptopen_date between to_date('" + sDateEN + @"', 'dd/mm/yyyy') and to_date('" + eDateEN + @"', 'dd/mm/yyyy')
                        and wr.deptclose_status = 0 order by wr.deptopen_date, wr.deptaccount_no";
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรูณาเลือกประเภทรายงาน");
                        return;
                    }

                    Sdt dt = WebUtil.QuerySdt(sql);
                    int i = 1, j;
                    if (report_type == "01")
                    {
                        table.Rows.Add(new object[] { "ลำดับ", "เลขที่คำขอ", "ประเภทสมาชิก", "เลขสมาชิก สอ.", "คำนำหน้า", "ชื่อ", "สกุล", "บัตรประชาชน", "วันเกิด", "วันสมัคร", "วันที่เริ่มเป็นสมาชิก", "รอบ", "ว/ด/ป ที่ส่งเงินคงสภาพ", "ชื่อสามีหรือภรรยา", "ชื่อผู้จัดการศพที่ระบุไว้", "ที่อยู่(ที่สามารถติดต่อได้)", "อำเภอ", "จังหวัด", "ไปรษณีย์", "โทรศัพท์", "คนที่", "ผู้รับผลประโยชน์", "บัตรประชาชนผู้รับผลประโยชน์", "ที่อยู่ผู้รับผลประโยชน์", "", "", "หมายเหตุ" });
                    }
                    else
                    {
                        table.Rows.Add(new object[] { "ลำดับ", "เลขฌาปนกิจ", "ประเภทสมาชิก", "เลขสมาชิก สอ.", "คำนำหน้า", "ชื่อ", "สกุล", "บัตรประชาชน", "วันเกิด", "วันสมัคร", "วันที่เริ่มเป็นสมาชิก", "รอบ", "ว/ด/ป ที่ส่งเงินคงสภาพ", "ชื่อสามีหรือภรรยา", "ชื่อผู้จัดการศพที่ระบุไว้", "ที่อยู่(ที่สามารถติดต่อได้)", "อำเภอ", "จังหวัด", "ไปรษณีย์", "โทรศัพท์", "คนที่", "ผู้รับผลประโยชน์", "บัตรประชาชนผู้รับผลประโยชน์", "ที่อยู่ผู้รับผลประโยชน์", "สาเหตุการพ้นสภาพ", "วันที่พ้นสภาพ", "ศูนย์ประสานงาน(สอ.)", "หมายเหตุ" });
                    }
                    String round_regis, remark, docno, member_no, prename, wname, wsname, card_person, birthday, openday, effectiveday, applyday, matename, managecorpsename, addr, district, province, postcode, hometel, codept_name, codept_id, codept_addr, resigncause, closeday, wftype_desc, wftype_code, coopbranch_desc;
                    while (dt.Next())
                    {
                        //if (state.SsCsType == "8" || state.SsCsType == "1")
                        //{
                        //    round_regis = dt.GetString("round_regis");
                        //}
                        //else
                        //{
                        //    round_regis = "";
                        //}
                       
                        round_regis = dt.GetString("round_regis");
                        wftype_code = dt.GetString("wftype_code");
                        if (state.SsCsType == "1" && wftype_code == "05" && round_regis == "")
                        {
                            round_regis = "เพื่อกู้";
                        }



                        remark = dt.GetString("remark");
                        docno = dt.GetString("docno");
                        member_no = dt.GetString("member_no");
                        prename = dt.GetString("prename_desc");
                        wname = dt.GetString("deptaccount_name");
                        wsname = dt.GetString("deptaccount_sname");
                        wftype_desc = dt.GetString("wftype_desc");
                        //card_person = dt.GetString("card_person");
                        card_person = dt.GetDecimal("card_person").ToString("# #### ##### ## #");
                        birthday = dt.GetDateTh("wfbirthday_date").ToString();
                        openday = dt.GetDateTh("deptopen_date").ToString();
                        if (report_type == "01")
                        {
                            resigncause = "";
                            closeday = "";
                            effectiveday = "";
                        }
                        else
                        {
                            resigncause = dt.GetString("resigncause_code") ;
                            switch (resigncause)
                            {
                                case "01":
                                    resigncause = "ลาออก";
                                    break;
                                case "02":
                                    resigncause = "เสียชีวิต";
                                    break;
                                case "04":
                                    resigncause = "สื้นสุด";
                                    break;
                                //default:
                                //    resigncause = "ไม่ระบุ";
                                //    break;
                            }
                            closeday = dt.GetDateTh("deptclose_date").ToString();
                            if (dt.GetDate("deptclose_date") == DateTime.MinValue)
                            {
                                closeday = "";
                            }
                            effectiveday = dt.GetDateTh("effective_date").ToString();
                            
                        }
                        //if (report_type != "01")
                        coopbranch_desc = dt.GetString("coopbranch_desc");
                        applyday = dt.GetDateTh("apply_date").ToString();
                        matename = dt.GetString("mate_name");
                        managecorpsename = dt.GetString("manage_corpse_name");
                        addr = dt.GetString("addr");
                        district = dt.GetString("district");
                        province = dt.GetString("province");
                        hometel = dt.GetString("hometel");
                        if (dt.GetString("wrpostcode") == null)
                        {
                            postcode = dt.GetString("mdpostcode");
                        }
                        else
                        {
                            postcode = dt.GetString("wrpostcode");
                        }
                        if (report_type == "01")
                        {
                            CoSql = "select \"NAME\" as codept_name, codept_id, codept_addr from wcreqcodeposit where deptrequest_docno = '" + docno + "' and branch_id = '" + state.SsBranchId + "'";
                        }
                        else
                        {
                            CoSql = "select \"NAME\" as codept_name, codept_id, codept_addre as codept_addr from wccodeposit where deptaccount_no = '" + docno + "' and branch_id = '" + state.SsBranchId + "'";
                        }
                        Sdt dtCodept = WebUtil.QuerySdt(CoSql);
                        RowCount = dtCodept.GetRowCount();
                        if (RowCount > 0)
                        {
                            RowFirst = true;
                            j = 1;
                            while (dtCodept.Next())
                            {
                                codept_name = dtCodept.GetString("codept_name");
                                codept_id = dtCodept.GetDecimal("codept_id").ToString("# #### ##### ## #");
                                codept_addr = dtCodept.GetString("codept_addr");
                                if (RowFirst)
                                {
                                    table.Rows.Add(new object[] { i, docno, wftype_desc, member_no, prename, wname, wsname, card_person, birthday, applyday, openday, round_regis, effectiveday, matename, managecorpsename, addr, district, province, postcode, hometel, j, codept_name, codept_id, codept_addr, resigncause, closeday, coopbranch_desc, remark });
                                }
                                else
                                {
                                    table.Rows.Add(new object[] { "","", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", j, codept_name, codept_id, codept_addr, "", "", "", "" });
                                }
                                RowFirst = false;
                                j++;
                            }
                        }
                        else
                        {
                            table.Rows.Add(new object[] { i, docno, wftype_desc, member_no, prename, wname, wsname, card_person, birthday, applyday, openday, round_regis, effectiveday, matename, managecorpsename, addr, district, province, postcode, hometel, "", "", "", "", resigncause, closeday, coopbranch_desc, remark });
                        }
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