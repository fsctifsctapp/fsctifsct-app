<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_collateral.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_collateral" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
   <%--<h2 class="block">
        �����š�ä�ӻ�Сѹ</h2>--%>
        <div class="column1-unit">
        <table style="width:100%;">
            <tr>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;��������Ҫԡ&nbsp;</legend>
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
                    <fieldset><legend>&nbsp;�س��ӻ�Сѹ�ú�ҧ&nbsp;</legend>
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
                    <fieldset><legend>&nbsp;�ä�ӻ�Сѹ�س��ҧ&nbsp;</legend>
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
  
        <!-- ������ 2 Column ��ҹ����-->
    </div>
    <div class="column2-unit-right">
    
        <!-- ������ 2 Column ��ҹ���-->
    </div>
</asp:Content>
