<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_sheet_wpt_forgetpassword.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_forgetpassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width: 100%; font-family: tahoma; font-size: 12px; ">
            <tr>
                <td colspan="2">
                    <font color="#000000"><strong><font face="MS Sans Serif" size="2" 
                        style="font-family: tahoma; font-size: 12px; color: #000000; font-weight: normal;">
                    ** ลืมรหัสผ่านกรุณาใส่ e-Mail ที่ใช้ในการสมัครขั้นต้น &nbsp; **</font></strong></font></td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right">
                    หมายเลขสมาชิก :</td>
                <td>
                    <asp:TextBox ID="memberno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Email :</td>
                <td>
                    <asp:TextBox ID="email" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="send" runat="server" onclick="send_Click" Text="ส่ง" 
                        Width="50px" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
