<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_collateral_check.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_collateral_check" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">

    <h2 class="block">
<%--        ระบบข้อมูลสมาชิก--%>
        
   </h2>
    <div class="column1-unit">
        <table style="width:100%;">
            <tr>
              <td colspan="2" style="font-family: tahoma; font-size: 12px; font-weight: bold">                    ประเภทหลักประกัน 
                    : 
                    <asp:DropDownList ID="DropDownList1" runat="server" 
                      DataSourceID="ObjectDataSource1" DataTextField="COLLMASTTYPE_DESC" 
                      DataValueField="COLLMASTTYPE_CODE">                    </asp:DropDownList>
                  <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                      SelectMethod="GetCollType" TypeName="WebPortal.Itm.ItmService">
                  </asp:ObjectDataSource>
&nbsp;<input id="Button1" type="button" value="ตกลง" 
                        onclick="opencrp()" />                </td>
            </tr>
            <tr>
              <td class="style2"><table style="width:100%;">
                <tr>
                  <td align="center" bgcolor="#EFF3FB" 
                        style="font-family: tahoma; font-size: 12px; font-weight: bold;">สิทธิการค้ำ</td>
                  <td align="center" bgcolor="#EFF3FB" 
                        style="font-family: tahoma; font-size: 12px; font-weight: bold">สิทธิค้ำคงเหลือ</td>
                </tr>
                
                <tr>
                  <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
                  <td>&nbsp;</td>
                </tr>
              </table></td>
                <td>                   
				 <div class="blockform">
                    <fieldset><legend>&nbsp;รายการค้ำประกัน&nbsp;</legend>
					<table width="400px" border="0" cellspacing="1" cellpadding="1" >
  <tr>
    <td align="center" 
          style="font-family: tahoma; font-size: 12px; color: #FFFFFF; font-weight: bold;">
        &nbsp;</td>
    <td bgcolor="#507CD1" align="center" 
          style="font-family: tahoma; font-size: 12px; color: #FFFFFF; font-weight: bold;">เลขที่สัญญา</td>
    <td bgcolor="#507CD1" align="center" 
          style="font-family: tahoma; font-size: 12px; font-weight: bold; color: #FFFFFF">ชื่อ - สกุล</td>
    <td bgcolor="#507CD1" align="center" 
          style="font-family: tahoma; font-size: 12px; font-weight: bold; color: #FFFFFF">เงินที่ใช้ค้ำ</td>
  </tr>
  <tr>
    <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
    <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
    <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
    <td style="font-family: tahoma; font-size: 12px">&nbsp;</td>
    <td style="font-family: tahoma; font-size: 12px" align="right">รวม</td>
    <td>&nbsp;</td>
  </tr>
</table>
					</fieldset>
				</div>				</td>
            </tr>
            <tr>
              <td class="style2">&nbsp;</td>
              <td>&nbsp;</td>
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
