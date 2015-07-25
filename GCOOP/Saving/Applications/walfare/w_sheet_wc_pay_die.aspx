<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_pay_die.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_pay_die" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%=jsselect_option %>
    <%=jsselect_deptacc%>
    <script type="text/javascript">
        /*   function MenubarOpen() {
        var branch_id;
        try {
        branch_id = objDwOption.GetItem(1, "branch_id");
        } catch (err) {
        branch_id = "";
        }
        Gcoop.OpenIFrame("600", "620", "w_dlg_wc_trn_member.aspx", "?branch_id=" + branch_id);
        }

        function AddRow() {
        var branch_id;
        try {
        branch_id = objDwOption.GetItem(1, "branch_id");
        } catch (err) {
        branch_id = "";
        }
        Gcoop.OpenIFrame("600", "620", "w_dlg_wc_trn_member.aspx", "?branch_id=" + branch_id);
        }
        */
        function OnDwOptionItemChanged(s, r, c, v) {
            switch (c) {
                case "year_mm":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsselect_option();
                    break;
            }
        }

        function OnDwOptionBottonClicked(s, r, c, v) {
            switch (c) {
                case "b_1":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsselect_deptacc();
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

    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_chg_die_search_mm"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwOptionBottonClicked">
    </dw:WebDataWindowControl>
      <br />
   
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_chg_die_pay_detail"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" Width="720px" Height="450px">
    </dw:WebDataWindowControl>
     <br />
    
   

    <asp:HiddenField ID="HdRow" runat="server" Value="" />

</asp:Content>
