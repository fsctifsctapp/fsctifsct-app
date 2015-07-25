<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_changepassword.aspx.cs"
    Inherits="WebPortal.ItmWeb.w_sheet_wpt_changepassword" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-family: Tahoma;
            font-size: 12px;
            font-weight: bold;
            color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%-- <h2 class="block">
    เปลี่ยนรหัสผ่าน</h2>--%>
    <!-- หัวข้อ -->
    <div class="column1-unit">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="contactform">
                        <fieldset>
                            <legend>&nbsp;ข้อมูลสมาชิก&nbsp;</legend>
                            <dw:WebDataWindowControl ID="DwUser" runat="server" 
                                DataWindowObject="d_member" LibraryList="~/DwAccess/datawindow.pbl">
                            </dw:WebDataWindowControl>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="contactform">
                        <fieldset>
                            <legend>&nbsp;เปลี่ยนรหัสผ่าน&nbsp;</legend>
                            <table style="font-family: tahoma; font-size: 12px" width="550px">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        <asp:Label ID="altmsg" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Username :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="username" runat="server" Width="160px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Password เดิม :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="current" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Password ใหม่ :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="newpass" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        ยืนยัน Password ใหม่ :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="confirm" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" OnClick="save_Click" Text="บันทึก" Width="66px" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <%--  <hr class="clear-contentunit" />--%>
    <div class="column2-unit-left">
        <!-- เนื้อหา 2 Column ด้านซ้าย-->
    </div>
    <div class="column2-unit-right">
        <!-- เนื้อหา 2 Column ด้านขวา-->
    </div>
</asp:Content>
