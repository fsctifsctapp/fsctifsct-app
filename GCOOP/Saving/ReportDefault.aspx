<%@ Page Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="ReportDefault.aspx.cs"
    Inherits="Saving.ReportDefault" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .subreport tr td
        {
            cursor: pointer;
        }
        .subreport tr td:hover
        {
            text-decoration: underline;
        }
        .tdpoint img
        {
            width: 23px;
            height: 21px;
        }
    </style>

    <script type="text/javascript">
    function showPointer(row){
        document.getElementById("p_row"+row).style.visibility="visible";
        document.getElementById("t_row"+row).style.backgroundColor="rgb(225,225,225)";
       
    }
    function hindPointer(row){
        document.getElementById("p_row"+row).style.visibility="hidden";
        document.getElementById("t_row"+row).style.backgroundColor="Transparent";
    }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table class="subreport" style="width: 100%; font-size: medium;">
        <asp:Literal ID="ltr_submenu" runat="server"></asp:Literal>
        <asp:Panel ID="pnl_cnst" runat="server" Width="100%">
        </asp:Panel>
    </table>
</asp:Content>
