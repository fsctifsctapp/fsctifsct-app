<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_collateral.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_collateral" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
   <%--<h2 class="block">
        ข้อมูลการค้ำประกัน</h2>--%>
        <div class="column1-unit">
        <table style="width:100%;">
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;ข้อมูลสมาชิก&nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwCollMem" runat="server" 
                            DataWindowObject="d_member" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;คุณค้ำประกันใครบ้าง&nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwCollWho" runat="server" 
                            DataWindowObject="d_collwho" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;ใครค้ำประกันคุณบ้าง&nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwWhoColl" runat="server" 
                            DataWindowObject="d_whocoll" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
    </h2>
    </div>
    <hr class="clear-contentunit" />
    <div class="column2-unit-left">
  
        <!-- เนื้อหา 2 Column ด้านซ้าย-->
    </div>
    <div class="column2-unit-right">
    
        <!-- เนื้อหา 2 Column ด้านขวา-->
    </div>
</asp:Content>
