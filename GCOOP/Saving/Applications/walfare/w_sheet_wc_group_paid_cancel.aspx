<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_group_paid_cancel.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_group_paid_cancel" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=SelectCode %>
<%=PostSearch %>
<%=PeriodChange %>
    <script type="text/javascript">
        function OnDwBottonClicked(s, r, c) {
            switch (c) {
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
                case "b_search":
                    PostSearch();
                    break;
            }
            
        }

        function OnDwCriItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
//                case "branch_id":                    
//                    Postchangebranch();
//                    break;
                case "period":
                    PeriodChange();
                    break;
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DWCri" runat="server" DataWindowObject="d_wc_cri_paid_group_cancel"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwCriItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwBottonClicked">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_paid_group_cancel"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <br />


    <asp:HiddenField ID="HdSelectCode" Value="" runat="server" />
</asp:Content>
