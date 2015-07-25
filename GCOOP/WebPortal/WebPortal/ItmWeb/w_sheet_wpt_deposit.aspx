<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_deposit.aspx.cs" Inherits="WebPortal.ItmWeb.w_sheet_wpt_deposit" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function dept_click(sender, rowNumber, objectName) {
        //objDwDept.Retrieve();
        var deptaccount_no = objDwDept.GetItem(rowNumber, "deptaccount_no");
        //alert(deptaccount_no);
        //objDwDeptStm.Retrieve(deptaccount_no);
        window.location = "?deptaccount_no="+deptaccount_no;
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
   <%--<h2 class="block">
    ข้อมูลเงินฝาก
        <!-- หัวข้อ -->
    </h2>--%>
    <div class="column1-unit">
        <table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <div class="contactform">
                    <fieldset><legend>&nbsp;ข้อมูลสมาชิก&nbsp;</legend>
                        <dw:WebDataWindowControl ID="DwDeptMem" runat="server" 
                            DataWindowObject="d_member" LibraryList="~/DwAccess/datawindow.pbl">
                        </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <dw:WebDataWindowControl ID="DwDept" runat="server" 
                        DataWindowObject="d_deptmaster" LibraryList="~/DwAccess/datawindow.pbl" 
                        ClientScriptable="True" ClientEventClicked="dept_click">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <dw:WebDataWindowControl ID="DwDeptStm" runat="server" 
                        DataWindowObject="d_deptstm" LibraryList="~/DwAccess/datawindow.pbl" 
                        ClientScriptable="True" RowsPerPage="15">
                        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
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
