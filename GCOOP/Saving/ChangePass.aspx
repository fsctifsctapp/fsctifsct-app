<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="Saving.ChangePass" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 125px;
        }
        .style2
        {
            width: 159px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width:57%;" align="center">
        <tr>
            <td class="style1">
                รหัสผ่านเดิม :</td>
            <td class="style2">
                <asp:TextBox ID="TextBox1" runat="server" Width="218px" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                รหัสผ่านใหม่ :</td>
            <td class="style2">
                <asp:TextBox ID="TextBox2" runat="server" Width="218px" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                ยืนยันรหัสผ่าน :</td>
            <td class="style2">
                <asp:TextBox ID="TextBox3" runat="server" Width="218px" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="ตกลง" 
                    Width="98px" />
            </td>
        </tr>
    </table>
</asp:Content>
