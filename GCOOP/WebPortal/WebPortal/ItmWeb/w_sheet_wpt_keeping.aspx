<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_keeping.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_keeping" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
   <%--    <h2 class="block">
        เรียกเก็บ</h2>--%>
    <div class="column1-unit">
        <table style="width:100%;">
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;ข้อมูลสมาชิก&nbsp;</legend>
                
                        <dw:WebDataWindowControl ID="DwKeepMem" runat="server" 
                            DataWindowObject="d_member" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                
                </td>
            </tr>
            <tr>
                <td>
                <div class= "contactform">
                     <fieldset><legend>&nbsp;เรียกเก็บ&nbsp;</legend>
                         <dw:WebDataWindowControl ID="DwKeeping" runat="server" 
                             DataWindowObject="d_keeping" LibraryList="~/DwAccess/datawindow.pbl">
                         </dw:WebDataWindowControl>
                </div>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    </td>
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
