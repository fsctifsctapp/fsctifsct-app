<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_calloan.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_calloan" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
   <h2 class="block">
     <%--   ระบบข้อมูลสมาชิก--%>
    กรุณาตรวจสอบความถูกต้องอีกครั้งกับทางสหกรณ์    
   </h2>
    <div class="column1-unit" 
        style="font-family: tahoma; font-size: 12px; font-weight: bold;">
        <table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
                <td  
                    
                    style="border: thin solid #D7E8FF; font-family: tahoma; font-size: 12px; font-weight: bold;">
                    จำนวนเงินกู้</td>
                <td>
                    <asp:TextBox ID="prn" runat="server" ></asp:TextBox>
                </td>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td 
                    
                    style="border-color: #EBE9ED; border-style: solid; border-width: thin; font-family: tahoma; font-size: 12px; font-weight: bold">
                    อัตราดอกเบี้ย</td>
                <td >
                    <asp:TextBox ID="intrate" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td 
                    
                    style="border: thin solid #D7E8FF; font-family: tahoma; font-size: 12px; font-weight: bold">
                    จำนวนงวด</td>
                <td >
                    <asp:TextBox ID="period" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td 
                    
                    style="border-color: #EBE9ED; border-style: solid; border-width: thin; font-family: tahoma; font-size: 12px; font-weight: bold" >
                    วันที่เริ่มชำระ</td>
                <td style="font-family: tahoma; font-size: 12px; font-weight: bold" >
                    เดือน<asp:TextBox ID="month" runat="server" Width="35px" MaxLength="2"></asp:TextBox>
                    /ปี<asp:TextBox ID="year" runat="server" Width="56px" MaxLength="4"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td 
                    
                    style="border: thin solid #D7E8FF; font-family: tahoma; font-size: 12px; font-weight: bold">
                    แบบการชำระ</td>
                <td >
                    <asp:DropDownList ID="prntype" runat="server" AutoPostBack="False">
                        <asp:ListItem Value="1">คงต้น</asp:ListItem>
                        <asp:ListItem Value="2">คงยอด</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" Text="คำนวณ" onclick="Button1_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td >
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        <table  cellpadding="0" cellspacing="0" border="1" bordercolor="#EBE9ED" 
            style="font-family: tahoma; font-size: 12px; font-weight: bold; color: #656565">
            <tr height="25">
                <td align="center" width="30" bgcolor="#D7E8FF">งวดที่</td>
                <td align="center" width="100" bgcolor="#D7E8FF">วันที่ชำระ</td>
                <td align="center" width="50" bgcolor="#D7E8FF">จำนวนวัน</td>
                <td width="95" align="right" bgcolor="#D7E8FF">ชำระต้น</td>
                <td width="108" align="right" bgcolor="#D7E8FF">ชำระดอกเบี้ย</td>
                <td width="124" align="right" bgcolor="#D7E8FF">รวมยอดชำระ</td>
                <td width="112" align="right" bgcolor="#D7E8FF">เงินต้นคงเหลือ</td>
            </tr>
            <asp:Literal ID="Literal1" runat="server">
            </asp:Literal>
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
