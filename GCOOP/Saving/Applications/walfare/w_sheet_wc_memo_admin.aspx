﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_memo_admin.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_memo_admin" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%=jsbranch_id %>
    <%=jsbranch_desc %>
     <%=jstype_chang%>
      <%=jsmoney_chang%>
   
    <script type="text/javascript">

        function OnDwOptionItemChanged(s, r, c, v) {
            switch (c) {
                case "branch_id":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_id();
                    break;
                case "branch_idd":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_desc();
                    break;
            }
        }

        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "memo_type":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jstype_chang();
                    break;
                case "money_type":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsmoney_chang();
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

    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_edit_bank_option"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>


    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_memo_fin"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
     <br />
 
    <asp:HiddenField ID="HdDeptaccount_no" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />

</asp:Content>
