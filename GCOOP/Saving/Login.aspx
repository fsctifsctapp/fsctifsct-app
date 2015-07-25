<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Saving.Login" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function buttonclear() {
            document.getElementById("ctl00_ContentPlace_txt_username").value = "";
            document.getElementById("ctl00_ContentPlace_txt_password").value = "";
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <div class="loginform">
        <table style="width: 100%;" border="0">
            <tr>
                <td align="right" width="130px">
                    <br />
                    <br />
                    สาขา&nbsp;:
                </td>
                <td>
                    <br />
                    <br />
                    <asp:DropDownList ID="DlBranchId" runat="server" Width="230px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <br />
                    ชื่อผู้ใช้&nbsp;:
                </td>
                <td>
                    <br />
                    <asp:TextBox ID="txt_username" runat="server" Width="225px" Text="admin"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <br />
                    รหัสผ่าน&nbsp;:
                </td>
                <td>
                    <br />
                    <asp:TextBox ID="txt_password" runat="server" TextMode="Password" Width="225px" Text="1234"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <br />
                    เครื่องพิมพ์&nbsp;:
                    <br />
                </td>
                <td>
                    <br />
                    <asp:DropDownList ID="DlPrinter" runat="server" Width="230px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <br />
                    <asp:Button ID="b_login" runat="server" Text="Login" OnClick="b_login_Click" Width="55px"
                        Height="30px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="b_clear" style="width: 55px; height: 30px;" type="button" value="Clear"
                        onclick="buttonclear();" />                   
                    <br />
                    <asp:Literal ID="LtConnectMode" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <br />
                    <asp:Label ID="LbServerMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
