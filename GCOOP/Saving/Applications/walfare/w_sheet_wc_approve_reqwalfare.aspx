<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_approve_reqwalfare.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_approve_reqwalfare" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <script type="text/javascript">

        function Validate() {
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Label ID="Label1" runat="server" Text="ทดสอบ"></asp:Label>
</asp:Content>

