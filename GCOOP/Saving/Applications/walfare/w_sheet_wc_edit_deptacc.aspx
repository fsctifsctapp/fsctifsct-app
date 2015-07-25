<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_edit_deptacc.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_edit_deptacc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsselectBranch %>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function SheetLoadComplete() {
            Gcoop.GetEl("TbBranchId").value = Gcoop.GetEl("HdBranchId").value;
            Gcoop.GetEl("HdBranchId").value = "";
        }

        function selectBranch() {
            Gcoop.GetEl("HdBranchId").value = Gcoop.GetEl("TbBranchId").value;
            jsselectBranch();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <tr>
            <td>
                Username :
            </td>
            <td>
                <asp:TextBox ID="TbUsername" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Password :
            </td>
            <td>
                <asp:TextBox ID="TbPassword" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                ConfirmPassword :
            </td>
            <td>
                <asp:TextBox ID="TbConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                ชื่อ - นามสกุล :
            </td>
            <td>
                <asp:TextBox ID="TbFullName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                รายละเอียด :
            </td>
            <td>
                <asp:TextBox ID="TbDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                สาขา :
            </td>
            <td>
                <input type="text" id="TbBranchId" onchange="selectBranch();" style="width: 50px;" /> - 
                <asp:DropDownList ID="DdCoopBranchId" runat="server">                    
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdBranchId" runat="server" />

</asp:Content>
