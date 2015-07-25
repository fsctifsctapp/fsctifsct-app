<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wc_create_branch.aspx.cs" 
Inherits="Saving.Applications.walfare.w_sheet_wc_create_branch" %>

 <%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsRetrieveBranch%>
    <%=jsBranchChk %>
    <%=postPost %>
    <%=postProvince %>
     <script type="text/javascript">
         function OnDwMainBclick(s, r, c) {
             if (c == "bsearch") {
                 s.AcceptText();
                 Gcoop.GetEl("HdStatusSave").value = "edit";
                 jsRetrieveBranch();
             }
         }

         function OnDwMainItemChanged(s, r, c, v) {
             switch (c) {
                 case "ampher_code":
                     s.SetItem(r, c, v);
                     s.AcceptText();
                     postPost();
                     break;
                 case "province_code":
                     s.SetItem(r, c, v);
                     s.AcceptText();
                     postProvince();
                     break;
               case "coopbranch_id":
                     s.SetItem(r, c, v);
                     s.AcceptText();
                     jsBranchChk();
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

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_create_branch"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_permission_all.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventClicked="OnDwMainClick" AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainBclick">
    </dw:WebDataWindowControl>

    <asp:HiddenField ID="HdStatusSave" runat="server" Value=""/>
</asp:Content>
