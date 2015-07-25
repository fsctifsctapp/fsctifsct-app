<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcSlip.ascx.cs" Inherits="Saving.Applications.walfare.uc_w_sheet_requestnew_light.UcSlip" %>
<style type="text/css">
    .Table2
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 0px;
    }
    .Table2 td
    {
        height: 23px;
        border: 1px solid #000000;
    }
    .NoneBorderBox
    {
        border: 0px;
        width: 100%;
        text-align: left;
    }
    .NoneBorderBoxCen
    {
        border: 0px;
        width: 100%;
        text-align: center;
    }
    .NoneBorderBoxRight
    {
        border: 0px;
        width: 100%;
        text-align: right;
    }
</style>
<asp:Label ID="Label1" runat="server" Text="การชำระเงิน" Font-Bold="True" Font-Names="Tahoma"
    Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <table width="740" class="Table2" cellpadding="1" cellspacing="1">
            <tr>
                <td width="7%" align="center" style="background-color: rgb(211, 231, 255)">
                    ลำดับ
                </td>
                <td width="73%" align="center" style="background-color: rgb(211, 231, 255)">
                    รายการ
                </td>
                <td width="20%" align="center" style="background-color: rgb(211, 231, 255)">
                    จำนวนเงิน
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td align="center">
                <asp:TextBox ID="seq_no" runat="server" Text='<%#Bind("seq_no")%>' CssClass="NoneBorderBoxCen"
                    ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="deptitemtype_desc" Text='<%#Bind("deptitemtype_desc")%>' runat="server"
                    CssClass="NoneBorderBox" ReadOnly="true"></asp:TextBox>
            </td>
            <td align="right">
                <asp:TextBox ID="amt" runat="server" Text='<%#Bind("amt",  "{0:#,##0.00}")%>' CssClass="NoneBorderBoxRight" ReadOnly="true"></asp:TextBox>
                <asp:HiddenField ID="deptitemtype_code" Value='<%#Bind("deptitemtype_code")%>' runat="server" />
                <asp:HiddenField ID="branch_id" Value='<%#Bind("branch_id")%>' runat="server" />
                <asp:HiddenField ID="status_pay" Value='<%#Bind("status_pay")%>' runat="server" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>