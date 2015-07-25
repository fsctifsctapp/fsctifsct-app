<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Saving.Flash.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .seticon img:hover
        {
            filter: none;
        }
        .seticon img
        {
            filter: Gray alpha(opacity=60);
        }
        .style1
        {
            text-decoration: underline;
            font-weight: bold;
        }
        body
        {
            background-color: rgb(200,235,255);
            font-family: "trebuchet ms" ,Tahoma;
        }
    </style>
    <%=genJavaScript %>
    <title>Isocare Systems Co.,Ltd.</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table style="width: 980px; height: 850px; background: url(HomeBG.jpg) no-repeat;"
            border="0">
            <tr>
                <td style="height: 80px;">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
                    <div style="text-align: right;">
                        <a style=" float:left;" href="../index.aspx"><< go to Home Screen 2 >></a>
                        <asp:Panel ID="PnlSelectDB" runat="server">
                            <asp:DropDownList ID="DlSelectDB" runat="server" Width="230px" OnSelectedIndexChanged="DlSelectDB_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                    <center>
                        <%=genExcJavaScript %>
                        <asp:Panel ID="PnlSelectDBError" runat="server" Visible="false">
                            <asp:DropDownList ID="DlSelectDBError" runat="server" Width="230px" OnSelectedIndexChanged="DlSelectDB_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </asp:Panel>
                    </center>
                    <table style="width: 100%;" border="0">
                        <tr>
                            <td>
                                <!--Table 1 Column กำหนดขนาดของ เมนูที่อยู่บน Background-->
                                <table style="width: 100%;" border="0">
                                    <tr>
                                        <td align="center">
                                            <!--Table 2 Column เพื่อแบ่ง ระบบงานหลัก กับ ระบบงานเสริม-->
                                            <table style="width: 100%;" border="0">
                                                <tr>
                                                    <td width="50%" valign="top">
                                                        <!--Table 2 Column ระบบงานหลัก-->
                                                        <table style="width: 100%;" border="0">
                                                            <asp:Literal ID="LtMainMenu" runat="server"></asp:Literal>
                                                        </table>
                                                    </td>
                                                    <td width="50%" valign="top" style="border-left: solid 1px white;">
                                                        <!--Table 2 Column ระบบงานเสริม-->
                                                        <table style="width: 100%;" border="0">
                                                            <asp:Literal ID="LtExpandMenu" runat="server"></asp:Literal>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
