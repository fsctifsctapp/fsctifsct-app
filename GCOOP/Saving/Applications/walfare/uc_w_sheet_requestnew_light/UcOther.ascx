<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcOther.ascx.cs" Inherits="Saving.Applications.walfare.uc_w_sheet_requestnew_light.UcOther" %>
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
    .NoneBorderBoxRight
    {
        border: 0px;
        width: 100%;
        text-align: right;
    }
</style>
<asp:Label ID="Lb001" runat="server" Text="ผู้รับเงินสงเคราะห์" Font-Bold="True" Font-Names="Tahoma"
    Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <table width="740" class="Table2" cellpadding="1" cellspacing="1">
            <tr>
                <td width="7%" align="center" style="background-color: rgb(211, 231, 255)">
                    ลำดับ
                </td>
                <td width="25%" align="center" style="background-color: rgb(211, 231, 255)">
                    ชื่อ - นามสกุล
                </td>
                <td width="15%" align="center" style="background-color: rgb(211, 231, 255)">
                    บัตรประชาชน
                </td>
                <td width="43%" align="center" style="background-color: rgb(211, 231, 255)">
                    ที่อยู่
                </td>
                <td width="5%" align="center" style="background-color: rgb(211, 231, 255)">
                    ต่างชาติ
                </td>
                <td width="5%" align="center" style="background-color: rgb(211, 231, 255)">
                    ลบแถว
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
                <asp:TextBox ID="name" Text='<%#Bind("name")%>' runat="server" CssClass="NoneBorderBox"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="codept_id" Text='<%#Bind("codept_id")%>' runat="server" CssClass="NoneBorderBox" MaxLength="15"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="codept_addr" Text='<%#Bind("codept_addr")%>' runat="server" CssClass="NoneBorderBox"></asp:TextBox>
            </td>
            <td align="center">
                <asp:CheckBox ID="foreigner_flag" runat="server" />
            </td>
            <td align="center">
                <asp:CheckBox ID="del_flag" Checked="false" runat="server" />
                <asp:HiddenField ID="branch_id" Value='<%#Bind("branch_id")%>' runat="server" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
