<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_divavg.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_divavg" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%--    <h2 class="block">
    สิทธิกู้
        <!-- หัวข้อ -->
    </h2>--%>
    <div class="column1-unit">
        <table style="width:100%;">
            <tr>
                <td>
                <div class="contactform">
                    <fieldset><legend>&nbsp;ข้อมูลสมาชิก&nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwDivAvgMem" runat="server" 
                            DataWindowObject="d_member" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
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
                <div class="contactform">
                    <fieldset><legend>&nbsp;เงินปันผลและเงินเฉลี่ยคืน &nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwDivAvg" runat="server" 
                            DataWindowObject="d_divavg" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
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
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    <hr class="clear-contentunit" />
    <div class="column2-unit-left">
    
        <!-- เนื้อหา 2 Column ด้านซ้าย-->
    </div>
    <div class="column2-unit-right">
    
        <!-- เนื้อหา 2 Column ด้านขวา-->
    </div> 
</asp:Content>
