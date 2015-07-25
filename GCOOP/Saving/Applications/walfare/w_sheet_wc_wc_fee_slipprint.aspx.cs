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
    public partial class w_sheet_wc_wc_fee_slipprint : PageWebSheet, WebSheet
    {
        protected String SelectCode;
        protected String PeriodChange;
        protected String runProcess;
        protected String popupReport;
        protected String app = "";
        protected String gid = "";
        protected String rid = "";
        protected String pdf;
        public void InitJsPostBack()
        {
            SelectCode = WebUtil.JsPostBack(this, "SelectCode");
            PeriodChange = WebUtil.JsPostBack(this, "PeriodChange");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DWCtrl.InsertRow(0);
                string Syear = Convert.ToString(DateTime.Today.Year + 543);
                DWCtrl.SetItemString(1, "pay_year", Syear);
                DWCtrl.SetItemString(1, "st_tdate", "01/01/" + Syear);
                DWCtrl.SetItemString(1, "end_tdate", "31/12/" + Syear);
            }
            else
            {
                this.RestoreContextDw(DWCtrl);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "SelectCode":
                    JSSelectCode();
                    break;
                case "PeriodChange":
                    JSPeriodChange();
                    break;
                case "runProcess":
                    RunProcess();
                    break;
                case "popupReport":
                    PopupReport();
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DWCtrl.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void JSSelectCode()
        {
            switch (HdSelectCode.Value)
            {
                case "all":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "selec_status", 1);
                    }
                    break;
                case "unsele":
                    for (int i = 0; i < DwMain.RowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "selec_status", 0);
                    }
                    break;
                                 
            }
            HdSelectCode.Value = "";
        }

        private void JSPeriodChange()
        {
            DwTrans SQLCA = new DwTrans();
            SQLCA.Connect();
            try
            {
               
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();

                String SQLcon = "", deptaccount_name, deptaccount_sname, card_person, deptopen_tdate;
                String st_tdate, end_tdate, deptacc_st, deptacc_end, member_nos, member_noe;
                String st_DateEN, end_DateEN, deptopen_ENdate;
                //DateTime deptopen_date;


                try
                {
                    deptaccount_name = DWCtrl.GetItemString(1, "deptaccount_name");
                }
                catch { deptaccount_name = ""; }

                try
                {
                    deptaccount_sname = DWCtrl.GetItemString(1, "deptaccount_sname");
                }
                catch { deptaccount_sname = ""; }

                try
                {
                    card_person = DWCtrl.GetItemString(1, "card_person");
                }
                catch { card_person = ""; }

                try
                {
                    deptopen_tdate = DWCtrl.GetItemString(1, "deptopen_tdate");
                }
                catch { deptopen_tdate = ""; }

                try
                {
                    st_tdate = DWCtrl.GetItemString(1, "st_tdate");
                   // st_DateEN = st_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(st_tdate.Substring(6, 4)) - 543);
                    end_tdate = DWCtrl.GetItemString(1, "end_tdate");
                   // end_DateEN = end_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(end_tdate.Substring(6, 4)) - 543);
                }
                catch 
                { 
                    st_tdate ="";
                    end_tdate = "";
                }

                try
                {
                    deptacc_st = DWCtrl.GetItemString(1, "deptacc_st");
                    deptacc_end = DWCtrl.GetItemString(1, "deptacc_end");
                }
                catch 
                {
                    deptacc_st = "";
                    deptacc_end = "";
                }
                try
                {
                    member_nos = DWCtrl.GetItemString(1, "member_nos");
                    member_noe = DWCtrl.GetItemString(1, "member_noe");
                }
                catch
                {
                    member_nos = "";
                    member_noe = "";
                }
  
 
                if (deptaccount_name != "")
                {
                    SQLcon = SQLcon + " and wcdeptmaster.deptaccount_name like '%" + deptaccount_name.Trim() + "%'";
                }

                if (deptaccount_sname != "")
                {
                    SQLcon = SQLcon + " and wcdeptmaster.deptaccount_sname like '%" + deptaccount_sname.Trim() + "%'";
                }

                if (card_person != "")
                {
                    SQLcon = SQLcon + " and wcdeptmaster.card_person like '%" + card_person.Trim() + "%'";
                }

                if (deptopen_tdate != "")
                {
                    deptopen_ENdate = deptopen_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(deptopen_tdate.Substring(6, 4)) - 543);
                    SQLcon = SQLcon + " and wcdeptmaster.deptopen_date = to_date('" + deptopen_ENdate + "','dd/mm/yyyy') ";
                }

                if (st_tdate != "" && end_tdate != "")
                {
                    st_DateEN = st_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(st_tdate.Substring(6, 4)) - 543);
                    end_DateEN = end_tdate.Substring(0, 6) + Convert.ToString(Convert.ToInt32(end_tdate.Substring(6, 4)) - 543);
                    SQLcon = SQLcon + " and WCDEPTSLIP.DEPTSLIP_DATE between to_date('" + st_DateEN + "','dd/mm/yyyy') and to_date('" + end_DateEN + "','dd/mm/yyyy')";
                }

                if (deptacc_st != "" && deptacc_end != "")
                {
                    SQLcon = SQLcon + " and trim(WCDEPTSLIP.DEPTACCOUNT_NO) between '" + deptacc_st.Trim() + "' and '" + deptacc_end.Trim() + "'";
                }

                if (member_nos != "" && member_noe != "")
                {
                    SQLcon = SQLcon + " and trim(wcdeptmaster.member_no) between '" + member_nos.Trim() + "' and '" + member_noe.Trim() + "'";
                }
                String sqlSost = "";
                String sort_sele = DWCtrl.GetItemString(1, "sort_sele");
                if (sort_sele == "1")
                {
                    sqlSost = "order by WCDEPTSLIP.DEPTACCOUNT_NO";
                }
                else if (sort_sele == "2")
                {
                    sqlSost = "order by wcdeptmaster.member_no";
                }
                else
                {
                    sqlSost = "order by WCDEPTSLIPDET.DEPTSLIP_NO";
                }

                String SQL;
                if (SQLcon == "")
                {
                    SQL = SQLBegin + " and WCDEPTSLIP.branch_id = '" + state.SsBranchId + "' and CMUCFCOOPBRANCH.CS_TYPE = '" + state.SsCsType + "'" + sqlSost;
                }
                else
                {
                    SQL = SQLBegin + SQLcon + " and WCDEPTSLIP.branch_id = '" + state.SsBranchId + "'and CMUCFCOOPBRANCH.CS_TYPE = '" + state.SsCsType + "'" + sqlSost;

                }
                try
                {
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
            catch (Exception ex)
            {
                SQLCA.Disconnect();
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        #region Report Process
        private void RunProcess()
        {
            bool status = false;
            status = BeforePrintProc();
            if (status == false)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกรายการที่จะพิมพ์");
            }
            else
            {
                // state.SsWsPass = new Encryption().EncryptStrBase64("1234+" + Session["ss_connectionstring"].ToString());
                app = "walfare";
                gid = "walfare_daily";
                rid = "walfare_daily99_99";
                //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
                //String start_docno = dw_criteria.GetItemString(1, "start_docno");
                // String slip_no = "5800971929";
                String branch_id = state.SsBranchId;
                String ascstype = state.SsCsType;
                String as_year = DWCtrl.GetItemString(1, "pay_year");
                String slip_type = "WPF";
                // jjjj.Value = "54889589589";
                // String branch_id = dw_criteria.GetItemString(1, "as_branchid");
                // String membno = dw_criteria.GetItemString(1, "as_deptaccno");
                //  membno = int.Parse(membno).ToString("000000");


                //String egroup_code = dw_criteria.GetItemString(1, "egroup_code");

                String coop_name = state.SsCoopName;
                ReportHelper lnv_helper = new ReportHelper();

                //for (int i = 1; i <= DwMain.RowCount; i++)
                //{
                //    slip_no = DwMain.GetItemString(i, "wcdeptslipdet_deptslip_no");

                //    if (First != 1)
                //    {
                //        ALLslip_no += ", ";
                //    }
                //    ALLslip_no += slip_no;
                //    First++;
                //}
                lnv_helper.AddArgument(state.SsUsername, ArgumentType.String);
                lnv_helper.AddArgument(slip_type, ArgumentType.String);
                lnv_helper.AddArgument(branch_id, ArgumentType.String);
                lnv_helper.AddArgument(ascstype, ArgumentType.String);
                lnv_helper.AddArgument(as_year, ArgumentType.String);




                //-------------------------------------------------------

                String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
                pdfFileName += "_" + gid + "_" + rid + ".pdf";
                pdfFileName = pdfFileName.Trim();
                try
                {
                    CommonLibrary.WsReport.Report lws_report = WsUtil.Report;
                    String criteriaXML = lnv_helper.PopArgumentsXML();
                    this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                    String li_return = lws_report.Run(state.SsWsPass, app, Session.SessionID, gid, rid, criteriaXML, pdfFileName);
                    if (li_return == "true")
                    {
                        HdOpenIFrame.Value = "True";
                    }
                    PDFUtil pdfUtil = new PDFUtil(Session);
                    pdfUtil.SourceFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.sourcefile") + pdfFileName;
                    try
                    {
                        String sql = "select * from cmucfcoopbranch where coopbranch_id = '" + branch_id + "'";
                        Sdt dt = WebUtil.QuerySdt(sql);
                        if (dt.Next())
                        {
                            pdfUtil.IsSendPDF = true;
                            pdfUtil.DesFile = WsUtil.Common.GetConstantValue(state.SsWsPass, "reportpdf.desfile") + ascstype + '-' + branch_id + "-" + dt.GetString("coopbranch_desc") + ".pdf";
                        }
                    }
                    catch { pdfUtil.IsSendPDF = false; }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    return;
                }
            }
        }
        #endregion

        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }

        public Boolean BeforePrintProc()
        {
            String slip_no = String.Empty;
            Decimal selec_status = 0;
            int count = 0;
            String entry_id = state.SsUsername;
            String branch_id = state.SsBranchId;
            String Cs_type = state.SsCsType;

            String clr_temp = "delete from wcsliptemp where entry_id  = '" + entry_id + "' and branch_id = '" + branch_id + "' and cs_type = '" + Cs_type + "' ";
            WebUtil.QuerySdt(clr_temp);

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                selec_status = DwMain.GetItemDecimal(i, "selec_status");

                if (selec_status == 1)
                {
                    count++;
                    slip_no = DwMain.GetItemString(i, "wcdeptslipdet_deptslip_no");

                    String sqlinsertdet = @"insert into wcsliptemp(deptslip_no, entry_id, branch_id, cs_type)
                    values('" + slip_no + "','" + entry_id + "', '" + branch_id + "', '" + Cs_type + "')";

                    WebUtil.QuerySdt(sqlinsertdet);
                } 
            }
            if (count == 0)
            return false;

            else
            return true;
        }
        //#endregion
    }
}