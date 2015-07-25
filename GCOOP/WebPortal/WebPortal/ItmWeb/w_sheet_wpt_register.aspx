<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_register.aspx.cs"
    Inherits="WebPortal.ItmWeb.w_sheet_wpt_register" Title="Untitled Page" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
//        function OnInsertRow() {
//            objDwRegister.InsertRow(objDwRegister.RowCount());
//        }
        function UpdateDw(sender, rowNumber, objectName) {
            var emailFilter = /^.+@.+\..{2,3}$/;
            var str = objDwRegister.GetItem(1,"email_address");
            var pwd = objDwRegister.GetItem(1,"password");
            var cpwd = objDwRegister.GetItem(1,"conf_pass");
//            alert(str);
//            alert(pwd);
//            alert(cpwd);
            if (str != "") {
                if (!(emailFilter.test(str))) {
                    alert("ท่านใส่อีเมล์ไม่ถูกต้อง");
                    return false;
                } else {
                    if (pwd != cpwd) {
                        alert("ยืนยัน password ไม่ตรง");
                        return false;
                    }
                }
                objDwRegister.Update();
                alert("บันทึกข้อมูลเรียบร้อยแล้ว");
                window.location = "/Default.aspx";
            } else {
                alert("กรุณาป้อน Email ด้วย");
            }

        }
        


    </script>
    <style type="text/css">
        .style1
        {
            width: 197px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%--   <h2 class="block">
        ระบบข้อมูลสมาชิก
    </h2>--%>

    <div class="column1-unit">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset>
                    <legend>สมัครสมาชิก</legend>
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    <table style="width:100%;">
                                        <tr>
                                            <td colspan="2">
                                                <dw:WebDataWindowControl ID="DwRegister" runat="server" ClientScriptable="True" 
                                                    DataWindowObject="d_register" LibraryList="~/DwAccess/datawindow.pbl">
                                                </dw:WebDataWindowControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                &nbsp;</td>
                                            <td>
                                    <input id="Button1" type="button" value="บันทึก" onclick="UpdateDw()" /><asp:Button 
                                        ID="Button3" runat="server" PostBackUrl="~/Default.aspx" Text="ยกเลิก" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </fieldset></div>
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

    <%--<hr class="clear-contentunit" />--%>
    <div class="column2-unit-left">
        <!-- เนื้อหา 2 Column ด้านซ้าย-->
    </div>
    <div class="column2-unit-right">
        <!-- เนื้อหา 2 Column ด้านขวา-->
    </div>
</asp:Content>
