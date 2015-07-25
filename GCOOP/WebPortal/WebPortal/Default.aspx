<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="WebPortal.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 400px;
            font-family: Tahoma;
            font-weight: bold;
            font-size: 14px;
        }
        .style3
        {
            width: 150px;
            font-family: Tahoma;
            font-size: 12px;
        }
        .style4
        {
            width: 17px;
        }
        .style5
        {
            width: 17px;
            font-family: Tahoma;
            font-size: 12px;
        }
        .style6
        {
            width: 600px;
            font-family: Tahoma;
            font-size: 13px;
        }
        .style7
        {
            font-family: Tahoma;
            font-size: 12px;
            font-weight: bold;
            color: Red;
        }
        .style8
        {
            height: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%--    <h2 class="block">
        ระบบข้อมูลสมาชิก
    </h2>--%>
    <div class="column1-unit">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="contactform">
                        <fieldset>
                            <legend>ระบบข้อมูลสมาชิก</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        <img alt="" src="img/bg_bullet_half_2.gif" />ข้อแนะนำและข้อตกลงในการใช้งานระบบข้อมูลสมาชิก
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style6">
                                        <li>การเข้าใช้งานระบบข้อมูลสมาชิกจะต้องทำการสมัครเข้าใช้งานระะบบและต้องเป็นสมาชิกของ
                                            สหกรณ์ออมทรัพย์การสื่อสารแห่งประเทศไทย จำกัด เท่านั้น</li>
                                        <li>เพื่อความเรียบร้อยในการสมัครใช้งาน ระบบฯ และเพื่อยืนยันผู้สมัคร กรุณาทำตามขั้นตอนที่ระบบแนะนำ</li>
                                        <li>หากปรากฏว่า ชื่อหรือหมายเลขสมาชิก ของท่านได้มีการสมัครใช้งานแล้ว โดยท่านไม่ทราบหรือทำการสมัครด้วยตัวท่านเองกรุณาแจ้งเจ้าหน้าที่เพื่อทำการ
                                            ตรวจสอบความถูกต้อง ต่อไป</li>
                                        <li>กรุณาเก็บรักษา username / password ของท่าน เพื่อสิทธิและความปลอดภัยในข้อมูลของท่านเอง
                                        </li>
                                        <li>หากปรากฏว่ามีบุคคลแอบอ้าง สมัครใช้งานระบบและเจ้าหน้าที่ตรวจสอบแล้ว จะทำการลบรายชื่อนั้น
                                            ๆ ออกจากระบบ โดยไม่ต้องแจ้งให้ทราบ</li>
                                        <li>ข้อมูลของสมาชิก ในระบบจะทำการปรับปรุงข้อมูล ทุก ๆ เดือน หากสมาชิกท่านใดพบข้อมูลไม่ตรงหรือมีข้อสงสัย
                                            กรุณาติดต่อเจ้าหน้าที่</li>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                        <asp:Literal ID="RegMem" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        &nbsp;
                                    </td>
                                    <td >
                                        &nbsp;
                                        <%--<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>--%>
                                    </td>
                                </tr>
                                </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
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
