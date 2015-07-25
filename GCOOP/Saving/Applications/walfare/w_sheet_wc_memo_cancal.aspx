<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_memo_cancal.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_memo_cancal" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=Search_id%>

       
    <script type="text/javascript">
        function DwCriItemChanged(s, r, c, v) {
            switch (c) {
                case "memo_id":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    Search_id();
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
    <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_wc_memo_admin_can_search"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwCriButtonClicked"
        ClientEventItemChanged="DwCriItemChanged">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_memo_admin_cancal"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="25" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
</asp:Content>
