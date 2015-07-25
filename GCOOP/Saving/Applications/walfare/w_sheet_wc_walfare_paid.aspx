<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_walfare_paid.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_walfare_paid" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postDeptAccountNo%>
    <%=postAddRowSlip%>
    <%=postDeleteSlip%>
    <%=postSlipCode%>
    <%=postRecppaytype_code %>
    <%=postBank %>
    <script type="text/javascript">
        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "deptaccount_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postDeptAccountNo();
            } else if (c == "sliptype_code") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postSlipCode();
            }
            else if (c == "recppaytype_code") {
                s.AcceptText();
                s.SetItem(r, c, v);
                postRecppaytype_code();
            }
            else if (c == "expense_bank") {
                s.AcceptText();
                s.SetItem(r, c, v);
                postBank();
            }

            return 0;
        }

        function OnDwSlipButtonClicked(s, r, c) {
            if (c == "b_addrow") {
                postAddRowSlip();
            } else if (c == "b_delete" && r > 0) {
                if (Gcoop.GetEl("HdReqDocno").value != "" && r <= 3) {
                    alert("ไม่สามารถลบข้อมูลรายการแถวที่ 1 - 3 ได้");
                    return 0;
                }
                Gcoop.GetEl("HdRowSlip").value = r + "";
                postDeleteSlip();
            }
        }

        function OnDwSlipItemChanged(s, r, c, v) {
            if (c == "deptitemtype_code") {
                var fild = c + "_" + (r - 1);
                var fild2 = "prncslip_amt_" + (r - 1);
                var resu = "";
                try {
                    var lbValue = Gcoop.GetElForm(fild)[Gcoop.GetElForm(fild).selectedIndex].innerHTML;
                    var expValue = new Array();
                    expValue = Gcoop.Explode(" ", lbValue);
                    resu = expValue[1];
                } catch (err) { }
                s.SetItem(r, "slip_desc", resu);
                s.AcceptText();
                Gcoop.Focus(fild2);
                Gcoop.GetElForm(fild2).select();
            } else if (c == "prncslip_amt") {
                var total = 0;
                for (i = 1; i <= s.RowCount(); i++) {
                    if (i == r) {
                        total += Gcoop.ParseFloat(v);
                    } else {
                        total += Gcoop.ParseFloat(s.GetItem(i, "prncslip_amt"));
                    }
                }
                objDwMain.SetItem(1, "deptslip_amt", total);
                objDwMain.AcceptText();
            }            
            return 0;
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("600", "620", "w_dlg_wc_deptedit.aspx");
        }

        function setdeptNo(deptaccount_no) {
            objDwMain.SetItem(1, "deptaccount_no", deptaccount_no);
            postDeptAccountNo();
        }

        function SheetLoadComplete() {
            var total = 0;
            for (i = 1; i <= objDwSlip.RowCount(); i++) {
                total += Gcoop.ParseFloat(objDwSlip.GetItem(i, "prncslip_amt"));
            }
            if (total > 0) {
                objDwMain.SetItem(1, "deptslip_amt", total);
                objDwMain.AcceptText();
            }

            if (Gcoop.GetEl("HdSaveStatus").value == "1") {
                if (confirm("ต้องการพิมพ์ใบเสร็จหรือไม่")) {
                    //                    Gcoop.OpenDlg("1000", "1200", "w_dlg_wc_printslip.aspx", "?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value);                    
                    window.open("dlg/w_dlg_wc_printslip_frommaster.aspx?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value, "ใบเสร็จ", 'width=800,height=2000,scrollbars=1');                   
                } else {
                    return false;
                }
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wf_paid_slip_master"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label1" runat="server" Text="การชำระเงิน" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
    <br />
    <dw:WebDataWindowControl ID="DwSlip" runat="server" DataWindowObject="d_wf_paid_slip_item"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
        ClientFormatting="True" ClientEventButtonClicked="OnDwSlipButtonClicked" ClientEventItemChanged="OnDwSlipItemChanged"
        TabIndex="400">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRowSlip" Value="0" runat="server" />
    <asp:HiddenField ID="HdReqDocno" Value="" runat="server" />

    
    <asp:HiddenField ID="HdSlipNo" Value="" runat="server" />
    <asp:HiddenField ID="HdSaveStatus" Value="" runat="server" />
</asp:Content>
