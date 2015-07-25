<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_fine_person.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_fine_person" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initPerson %>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwCriButtonClicked(s, r, c) {
            if (c == "b_search") {
                initPerson();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_wc_chkother_person_cs_search"
        LibraryList="~/DataWindow/walfare/w_sheet_duplicate.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwCriButtonClicked">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_chkother_cs_search_detail"
            LibraryList="~/DataWindow/walfare/w_sheet_duplicate.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="25" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
</asp:Content>
