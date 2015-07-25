<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs"
    Inherits="Saving.CustomControl.LoginControl" %>
<div id="DivLogIn" align="center" runat="server">
    <style type="text/css">
        .tableLogIn
        {
            width: 500px;
            border: solid 1px #AAAACB;
            margin-top: 70px;
        }
        .tableLogIn td
        {
            vertical-align: middle;
            text-align: left;
            height: 35px;
        }
        .tableLogIn span
        {
            margin-left: 20px;
        }
        .tableLogInInputWidth
        {
            width: 170px;
        }
    </style>
    <table id="TableLogIn" class="tableLogIn" runat="server">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="ศูนย์ประสานงาน"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="DdBranchId" runat="server" CssClass="tableLogInInputWidth">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="ชื่อผู้ใช้งาน"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="TbUsername" runat="server" CssClass="tableLogInInputWidth"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="รหัสผ่าน"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="TbPassword" runat="server" TextMode="Password" CssClass="tableLogInInputWidth"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="BtLogin" runat="server" Text="เข้าสู่ระบบ" OnClick="BtLogin_Click"
                    Width="90px" />
                &nbsp;
                <asp:Button ID="BtHome" runat="server" Text="กลับหน้าหลัก" Width="90px" OnClick="BtHome_Click" />
                <br />
                <asp:Literal ID="LtLoginMessage" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
</div>
<%=focusControl%>
