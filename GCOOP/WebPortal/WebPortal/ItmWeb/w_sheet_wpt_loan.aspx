<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wpt_loan.aspx.cs"
    Inherits="WebPortal.ItmWeb.w_sheet_wpt_loan" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function loan_click(sender, rowNumber, objectName) {
        var loancontract_no = objDwLoan.GetItem(rowNumber, "loancontract_no");
        //alert(loancontract_no);
        window.location = "?loancontract_no=" + loancontract_no;
    }
</script>
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
                    <legend>ข้อมูลสมาชิก</legend>
                    <dw:WebDataWindowControl ID="DwLoanMem" runat="server" DataWindowObject="d_member"
                        LibraryList="~/DwAccess/datawindow.pbl">
                    </dw:WebDataWindowControl>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="DwLoan" runat="server" DataWindowObject="d_loanmaster"
                        LibraryList="~/DwAccess/datawindow.pbl" ClientScriptable="True" 
                        ClientEventClicked="loan_click">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <dw:WebDataWindowControl ID="DwLoanStm" runat="server" 
                        DataWindowObject="d_loanstm" LibraryList="~/DwAccess/datawindow.pbl" 
                        RowsPerPage="15">
                        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                    
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
