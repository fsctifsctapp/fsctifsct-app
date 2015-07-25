<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_wc_ma_programer.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_ma_programer" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsGenSlip %>
    <%=jsupdate_Reg %>
    <%=jsGenStatement %>
    <%=jsGenCodept %>
    <%=jsLogin %>
    <%=jsGenNewCsType%>>
    <script type="text/javascript">
        function GenSlip() {
            if (confirm("Do you Confirm???")) {
                jsGenSlip();
            }
        }
        function GenNewCsType() {
            if (confirm("Do you Confirm???")) {
                jsGenNewCsType();
            }
        }
        function update_Reg() {
            if (confirm("Do you Confirm???")) {
                jsupdate_Reg();
            }
        }
        function GenStatement() {
            if (confirm("Do you Confirm???")) {
                jsGenStatement();
            }
        }
        function GenCodept() {
            if (confirm("Do you Confirm???")) {
                jsGenCodept();
            }
        }
        function ClickLogin() {
            if (confirm("Do you Confirm???")) {
                jsLogin();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <div id="Login" runat="server">
            <input type="password" id="passlogin" runat="server" style="width:200px;"/>
            <input type="button" id="Button1" onclick="ClickLogin();" value="Login" style="width:100px;" />
        </div>
        <div id="Tmain" runat="server" visible="false">
            <div>
                <input type="button" id="genSlip" onclick="GenSlip();" value="Genslip" style="width:100px;" /> สร้าง Slip
                <br /><br /><hr /><br />
                <input type="button" value="UpdateReg" onclick="update_Reg();" style="width:100px;" /> Update Deptaccount_no
                <br /><br />
            </div>
            <hr />
            <table>
                <tr>
                    <td>
                        DeptOpen Date: 
                    </td>
                    <td>
                        <input type="text" value="" id="opendate" runat="server" /> ค.ศ.
                    </td>
                </tr>
                <tr>
                    <td>
                        Prncbal:
                    </td>
                    <td>
                        <input type="text" value="" id="prncbal" runat="server" /> **เฉพาะ GenStatement
                    </td>
                </tr>
                <tr>
                    <td>
                        Group Branch:
                    </td>
                    <td>
                        <textarea id="groupBranch" cols="80" rows="2" runat="server"></textarea>
                    </td>
                </tr>
            </table>
            <br />
            <input type="button" id="genStatement" onclick="GenStatement();" value="GenStatement" /> สร้าง Statement 
            <input type="button" id="genCodept" onclick="GenCodept();" value="GenCodept" /> ดึงผู้รับผลประโยชน์จากคำขอ
             <input type="button" id="gennewcstype" onclick="GenNewCsType();" value="GenNewCsType" /> สร้างเลขฌาปนกิจใหม่(tgc state)
            <br /><br /><hr /><br />
        </div>
</asp:Content>
