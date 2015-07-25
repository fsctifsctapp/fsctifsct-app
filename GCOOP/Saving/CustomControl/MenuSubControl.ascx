<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuSubControl.ascx.cs"
    Inherits="Saving.CustomControl.MenuSubControl" %>
<link id="CssMenuSub" href="../css/MenuSub.css" rel="stylesheet" type="text/css"
    runat="server" />
<div id="divMenuSub" style="text-align: left;" runat="server">
    <asp:Repeater ID="RepeaterMenuSub" runat="server">
        <HeaderTemplate>
            <table class="menuSub" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <th>
                        <%=Group%>
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="menuSubTd" style="cursor: pointer;">
                    <a href="<%#Eval("PageLink")%>">
                        <div>
                            <asp:Label ID="LbMenuSub" runat="server" Text='<%#Eval("Name")%>' />
                        </div>
                    </a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr>
                <td class="menuSubFotter">
                    &nbsp;
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
