<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setting_page.aspx.cs" Inherits="Saving.CmPage.setting_page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Setting Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="LbServerMessage" runat="server"></asp:Literal>
        <table style="width: 100%;">
            <tr>
                <td>
                    ตั้งค่า เครื่องพิมพ์
                </td>
                <td>
                    เลือกเครื่องพิมพ์ใหม่
                    <asp:DropDownList ID="DlPrinter" runat="server" Width="230px">
                    </asp:DropDownList>
                    <asp:Button ID="b_save" runat="server" Text="Save" onclick="b_save_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
