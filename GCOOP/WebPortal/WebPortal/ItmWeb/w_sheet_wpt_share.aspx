<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_share.aspx.cs"
    Inherits="WebPortal.ItmWeb.w_sheet_wpt_share" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%--    <h2 class="block">
        �к���������Ҫԡ
        
   </h2>--%>
    <div class="column1-unit">
        <table style="width: 100%;">
            <tr>
                <td align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div class="contactform">
                        <fieldset>
                            <legend>��������Ҫԡ</legend>
                            <dw:WebDataWindowControl ID="DwMember" runat="server" 
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
                    <legend>���������</legend>
                        <dw:WebDataWindowControl ID="DwShare" runat="server" 
                            DataWindowObject="d_sharemaster" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <dw:WebDataWindowControl ID="DwShareStm" runat="server" 
                        DataWindowObject="d_sharestm" LibraryList="~/DwAccess/datawindow.pbl">
                    </dw:WebDataWindowControl>
                    
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
        <!-- ������ 2 Column ��ҹ����-->
    </div>
    <div class="column2-unit-right">
        <!-- ������ 2 Column ��ҹ���-->
    </div>
</asp:Content>
