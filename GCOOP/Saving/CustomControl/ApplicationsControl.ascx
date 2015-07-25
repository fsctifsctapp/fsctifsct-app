<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationsControl.ascx.cs"
    Inherits="Saving.CustomControl.ApplicationsControl" %>
<style type="text/css">
    .tableApplication
    {
        /*border: solid 1px #4499BC;*/
        margin-top: 20px;
    }
    .tableApplication td
    {
        /*border: solid 1px #4499BC;*/
        height: 150px;
        width: 150px;
        vertical-align: top;
        text-align: center;
    }
    .imApplication
    {
        margin-top: 1px;
        cursor: pointer;
    }
    .imApplicationNone
    {
        margin-top: 1px;
        cursor: auto;
    }
    .lbApplication
    {
        font-family: Tahoma, MS Sans Serif, Serif;
        font-size: 16px;
        color: #990000;
        cursor: pointer;
    }
    .lbWorkDateOn
    {
        font-family: Tahoma, MS Sans Serif, Serif;
        font-size: 10px;
        color: blue;
    }
</style>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <table class="tableApplication" cellpadding="0" cellspacing="0" align="center">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="LtTrEnd" runat="server" Text='<%#Eval("TrStart")%>'></asp:Literal>
        <td>
            <div onclick="window.location='?app=<%#Eval("Application")%>&cstype=<%#Eval("CsType")%>&csdesc=<%#Eval("CsDesc")%>'">
                <asp:Image ID="ImApplicationIcon" runat="server" ImageUrl='<%#Eval("Picture")%>'
                    CssClass='<%#Eval("PictureCss")%>' Height="84px" Width="84px" />
                <br />
                <asp:Label ID="LbWorkDateStatus" runat="server" Text='<%#Eval("WorkDate")%>' CssClass="lbWorkDateOn"></asp:Label>
                <br />
                <asp:Label ID="LbApplicationText" runat="server" Text='<%#Eval("Name")%>' CssClass="lbApplication"></asp:Label>
            </div>
        </td>
        <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("TrEnd")%>'></asp:Literal>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
