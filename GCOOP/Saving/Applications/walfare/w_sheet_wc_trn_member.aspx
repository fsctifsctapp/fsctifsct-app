<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_trn_member.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_trn_member" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsinitAccNo %>
    <%=jsbranch_id %>
    <%=jsbranch_desc %>
    <%=jsselect_option %>
    <script type="text/javascript">
        function MenubarOpen() {
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

        function setdeptNo(deptaccount_no) {
            Gcoop.GetEl("HdDeptaccount_no").value = deptaccount_no;
            jsinitAccNo();
        }

        function OnDwOptionItemChanged(s, r, c, v) {
            switch (c) {
                case "select_option":
                    s.SetItem(r, c, v);
                    s.AcceptText();
//                    jsselect_option();
                    break;
            }
        }

        function OnDwMainItemChanged(s, r, c, v) {
            Gcoop.GetEl("HdRow").value = r + "";
            switch (c) {
                case "branch_id":                    
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_id();
                    break;
                case "coopbranch_id":                    
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_desc();
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

    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_trn_option"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>

    <span onclick="AddRow();" style="font: Tahoma; font-size: 14px; color: #0099CC; cursor: pointer;">เพิ่มแถว</span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_trn_memb"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <br />

    <asp:HiddenField ID="HdDeptaccount_no" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />

</asp:Content>
