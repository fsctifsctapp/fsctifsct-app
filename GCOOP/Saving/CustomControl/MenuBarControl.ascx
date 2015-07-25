<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuBarControl.ascx.cs"
    Inherits="Saving.CustomControl.MenuBarControl" %>
<link id="CssMenuBar" href="../Css/MenuBar.css" rel="stylesheet" type="text/css"
    runat="server" />
<asp:Repeater ID="RepeaterMenuBar" runat="server">
    <HeaderTemplate>
        <table class="menuBar">
            <tr>
    </HeaderTemplate>
    <ItemTemplate>
        <td width="100" valign="top">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("PageLink")%>'>
                <asp:Image ID="Image9" runat="server" ImageUrl="~/img/ico/MyDocuments.png" />
                <h3>
                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("Name")%>' />
                </h3>
            </asp:HyperLink>
        </td>
    </ItemTemplate>
    <FooterTemplate>
        </tr> </table>
    </FooterTemplate>
</asp:Repeater>
