<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_wc_printslip_frommaster.aspx.cs" Inherits="Saving.Applications.walfare.dlg.w_dlg_wc_printslip_frommaster" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel1" runat="server" Height="800" Width="710">
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>       
        <p style="margin-left:90%;">ต้นฉบับ</p>
        <dw:WebDataWindowControl ID="DwMain_org" runat="server" DataWindowObject="d_wc_slip_payment_reg"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_new.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="12" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChanged" >
        </dw:WebDataWindowControl>

        <p style="margin-left:90%;">สำเนา</p>
        <dw:WebDataWindowControl ID="DwMain_coppy" runat="server" DataWindowObject="d_wc_slip_payment_reg"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_new.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="12" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChanged" >
        </dw:WebDataWindowControl>
    </div>
    </asp:Panel>
    </form>
</body>
</html>
