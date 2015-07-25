<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Saving.Index" %>

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
            cursor: pointer;
            color:White;
        }
        .styleDetail
        {
            text-decoration: underline;
            font-weight: bold;
            cursor: pointer;
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
        <asp:HiddenField ID="HfAppClicked" runat="server" />
        <asp:HiddenField ID="HfAppEng" runat="server" />
        <table style="width: 980px; height: 850px; background: url(img/home/HomeBG.jpg) no-repeat;"
            border="0">
            <tr>
                <td style="height: 80px;">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
                    <div style="text-align: right;">
                        <a style=" float:left;" href="./flash/index.aspx"><< go to Home Screen 1 >></a>
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
                        <%=genJavaScriptInnerHTML %>
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
                                            <!--Table 2 Column-->
                                            <table style="width: 100%; height:640px; border: solid 3px white;" border="0">
                                                <tr>
                                                    <td width="60%" valign="top">
                                                        <table style="width: 100%;  height:630px;" border="0">
                                                            <tr>
                                                                <td width="50%" valign="top">
                                                                    <!--Table 2/1-->
                                                                    <table style="width: 100%;" border="0">
                                                                        <asp:Literal ID="LtLeft" runat="server"></asp:Literal>
                                                                    </table>
                                                                </td>
                                                                <td width="50%" valign="top" style="border-left: solid 3px white;">
                                                                    <!--Table 2/2-->
                                                                    <table style="width: 100%;" border="0">
                                                                        <asp:Literal ID="LtRight" runat="server"></asp:Literal>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="40%" valign="top" style="border-left: solid 1px white;">
                                                        <div id="divShowDetail">
                                                        </div>
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
