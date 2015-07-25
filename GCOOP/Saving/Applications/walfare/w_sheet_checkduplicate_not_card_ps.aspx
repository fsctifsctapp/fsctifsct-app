<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_checkduplicate_not_card_ps.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_checkduplicate_not_card_ps" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_checkduplicate"
        LibraryList="~/DataWindow/walfare/w_sheet_duplicate.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>

</asp:Content>
