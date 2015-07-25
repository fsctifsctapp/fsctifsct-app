<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Saving.Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <%
        try
        {
            lbl_wsheet.Text = Request["wsheet"].ToString();
        }
        catch { }
    %>
    <asp:Label ID="lbl_wsheet" runat="server" Text=""></asp:Label>
</asp:Content>