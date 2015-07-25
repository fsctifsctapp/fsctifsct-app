<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_cancel_approve_paid.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_cancel_approve_paid" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDeptaccountNo%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            var branch_id = objDwMain.GetItem(1, "branch_id");
            Gcoop.OpenIFrame("600", "620", "w_dlg_wc_deptedit.aspx", "?branch_id=" + branch_id);
        }

        function setdeptNo(deptaccount_no) {
            objDwMain.SetItem(1, "deptaccount_no", deptaccount_no);
            jsPostDeptaccountNo();
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "deptaccount_no":
                    jsPostDeptaccountNo();
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_cancel_approve_paid"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_wc_cancel_approve_slip"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
</asp:Content>
