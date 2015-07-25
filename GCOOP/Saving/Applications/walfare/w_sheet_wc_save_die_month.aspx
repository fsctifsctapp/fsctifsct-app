<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_save_die_month.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_save_die_month" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%=jsselect_option %>
    <%=SelectCode %>
      <%=jsbranch_id %>
    <%=jsbranch_desc %>
    <script type="text/javascript">
    
        function OnDwOptionBottonClicked(s, r, c) {
            switch (c) {
                case "b_1":
                    jsselect_option();
                    break;
               
            }
        }

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

        function OnDwSelemBottonClicked(s, r, c) {
            switch (c) {
               
                case "unchk_all":
                    Gcoop.GetEl("HdSelectCode").value = "unchk_all";
                    SelectCode();
                    break;
                case "chk_all":
                    Gcoop.GetEl("HdSelectCode").value = "chk_all";
                    SelectCode();
                
            }
        }

       


        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_chg_die_search"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwOptionBottonClicked">
    </dw:WebDataWindowControl>
      <br />
     <dw:WebDataWindowControl ID="DwSelem" runat="server" DataWindowObject="d_wc_chg_die_sele_month"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwSelemonthItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwSelemBottonClicked">
    </dw:WebDataWindowControl>
     <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_chg_die_detail"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
     <br />
    
   

    <asp:HiddenField ID="HdRow" runat="server" Value="" />
     <asp:HiddenField ID="HdSelectCode" Value="" runat="server" />

</asp:Content>
