<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_ucf_round_for_pay.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_ucf_round_for_pay" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostForyear %>
     <%=jsAdd_cstype%>
     <%=jsupdateFee%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function AddRow() {
            jsAdd_cstype();
        }

        function OnDwOptionItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "for_year":
                    jsPostForyear();
                    break;
            }
        }
        function OnDwMainButtonClicked(s, r, c) {
            switch (c) {
                case "b_del":
                    objDwMain.DeleteRow(r);
                    break;
            }
        }

        function OnDwOptionButtonClicked(s, r, c) {
            switch (c) {
                case "update":
                    jsupdateFee();
                    break;
            }
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
             switch (c) {
                 case "deptopen_tdate":
                    break;
               
                }
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_cri_ucf_paid"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        ClientEventButtonClicked="OnDwOptionButtonClicked" AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <span onclick="AddRow();" style="font: Tahoma; font-size: 14px; color: #0099CC; cursor: pointer;">
        เพิ่มแถว</span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_paid_for_report_02"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_paid.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventButtonClicked="OnDwMainButtonClicked" AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>

</asp:Content>
