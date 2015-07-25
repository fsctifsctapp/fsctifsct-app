<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_paid_add.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_paid_add" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsChangeDeptNo%>
    <%=jsAddRow %>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwCriItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "deptaccount_no":
                    jsChangeDeptNo();
                    break;
            }
        }
        function AddRow() {
            jsAddRow();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_wc_paid_group_add_cri"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwCriItemChanged"
        ClientEventClicked="OnDwCriClick" AutoRestoreContext="False" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <span onclick="AddRow();" style="font: Tahoma; font-size: 18px; color: #0099CC; cursor: pointer;">
        เพิ่มแถว</span>
    <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_wc_paid_group_add"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwListItemChanged"
        ClientEventClicked="OnDwListClick" AutoRestoreContext="False" ClientFormatting="True">
    </dw:WebDataWindowControl>
</asp:Content>
