<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_group_wait_paid.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_group_wait_paid" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=SelectCode %>
    <%=PeriodChange %>
    <script type="text/javascript">

        function OnDwCtrlBottonClicked(s, r, c) {
            switch (c) {
                case "b_search":
                    PeriodChange();
                    break;
                case "b_approve":
                    Gcoop.GetEl("HdSelectCode").value = "approve";
                    SelectCode();
                    break;
                case "b_wait":
                    Gcoop.GetEl("HdSelectCode").value = "wait";
                    SelectCode();
                    break;
                case "b_cancle":
                    Gcoop.GetEl("HdSelectCode").value = "cancle";
                    SelectCode();
                    break;
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwCtrlChagned(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DWCtrl" runat="server" DataWindowObject="d_wc_cri_paid_group"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwCtrlChagned"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwCtrlBottonClicked">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_wait_paid_group"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <br />
    <asp:HiddenField ID="HdSelectCode" Value="" runat="server" />
</asp:Content>
