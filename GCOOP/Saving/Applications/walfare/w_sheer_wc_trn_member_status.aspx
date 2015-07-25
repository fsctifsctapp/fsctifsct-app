<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheer_wc_trn_member_status.aspx.cs" Inherits="Saving.Applications.walfare.w_sheer_wc_trn_member_status" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
  
    <script type="text/javascript">

      
//        function Validate() {
//            return confirm("ยืนยันการบันทึกข้อมูล");
//        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

  

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_trn_status"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
     <br />
 
    <asp:HiddenField ID="HdDeptaccount_no" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />

</asp:Content>
