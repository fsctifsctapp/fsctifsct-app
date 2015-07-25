<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_dp_prncfix_process.aspx.cs"
    Inherits="Saving.Applications.ap_deposit.w_sheet_dp_prncfix_process" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=PrncFixProcess%>
<script type ="text/javascript">
    function OnDwMainButtonClicked(sender, rowNumber, buttonName){
        if(buttonName == "cb_ok"){
            PrncFixProcess();
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_dp_prncfix_main"
        LibraryList="~/DataWindow/ap_deposit/dp_prncfix_process.pbl"
        ClientFormatting="True" ClientEventButtonClicking="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
</asp:Content>
