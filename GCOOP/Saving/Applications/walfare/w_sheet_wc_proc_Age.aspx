<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_proc_Age.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_proc_Age" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
     <%=jsupdateFee%>
    <script type="text/javascript">
//        function Validate() {
//            return confirm("ยืนยันการบันทึกข้อมูล");
//        }

//        function AddRow() {
//            jsAdd_cstype();
//        }

        function OnDwOptionButtonClicked(s, r, c) {
            switch (c) {
                case "b_1":
                    jsupdateFee();
                    break;
            }
        }

      

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwOption" runat="server" DataWindowObject="d_wc_ageproc"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_membermaster.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwOptionItemChanged"
        ClientEventButtonClicked="OnDwOptionButtonClicked" AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
   

</asp:Content>
