<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_expexcel_membcard.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_expexcel_membcard" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=SearchData%>
<%=SelectBranch%>
    <script type="text/javascript">
        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "bsearch":
                    SearchData();
                    break;
            }
        }
        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "s_branch":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    SelectBranch();
                    break;
                case "e_branch":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    SelectBranch();
                    break;
            }  
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div id="expexcel">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_cri_branchid_rdate_fsct"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_export_excel.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClick">
        </dw:WebDataWindowControl>
        <br />
        <asp:Button ID="BexpExcel" runat="server" OnClick="ExportToExcel" Text="Export Excel" />
        <br />
        <asp:Panel ID="Panel1" runat="server" Height="800" Width="750" ScrollBars="Both">                    
            <asp:GridView ID="GridViewExcel" runat="server">
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>
