<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="WebPortal.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 400px;
            font-family: Tahoma;
            font-weight: bold;
            font-size: 14px;
        }
        .style3
        {
            width: 150px;
            font-family: Tahoma;
            font-size: 12px;
        }
        .style4
        {
            width: 17px;
        }
        .style5
        {
            width: 17px;
            font-family: Tahoma;
            font-size: 12px;
        }
        .style6
        {
            width: 600px;
            font-family: Tahoma;
            font-size: 13px;
        }
        .style7
        {
            font-family: Tahoma;
            font-size: 12px;
            font-weight: bold;
            color: Red;
        }
        .style8
        {
            height: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CenterContent" runat="server">
    <%--    <h2 class="block">
        �к���������Ҫԡ
    </h2>--%>
    <div class="column1-unit">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="contactform">
                        <fieldset>
                            <legend>�к���������Ҫԡ</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        <img alt="" src="img/bg_bullet_half_2.gif" />����й���Т�͵�ŧ㹡����ҹ�к���������Ҫԡ
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        &nbsp;
                                    </td>
                                    <td class="style6">
                                        <li>��������ҹ�к���������Ҫԡ�е�ͧ�ӡ����Ѥ������ҹ��к���е�ͧ����Ҫԡ�ͧ
                                            �ˡó������Ѿ�������������觻������ �ӡѴ ��ҹ��</li>
                                        <li>���ͤ������º����㹡����Ѥ���ҹ �к�� ��������׹�ѹ�����Ѥ� ��سҷӵ����鹵͹����к��й�</li>
                                        <li>�ҡ��ҡ���� �������������Ţ��Ҫԡ �ͧ��ҹ���ա����Ѥ���ҹ���� �·�ҹ����Һ���ͷӡ����Ѥô��µ�Ƿ�ҹ�ͧ��س������˹�ҷ�����ͷӡ��
                                            ��Ǩ�ͺ�����١��ͧ ����</li>
                                        <li>��س����ѡ�� username / password �ͧ��ҹ �����Է����Ф�����ʹ���㹢����Ţͧ��ҹ�ͧ
                                        </li>
                                        <li>�ҡ��ҡ�����պؤ���ͺ��ҧ ��Ѥ���ҹ�к�������˹�ҷ���Ǩ�ͺ���� �зӡ��ź��ª��͹��
                                            � �͡�ҡ�к� ������ͧ������Һ</li>
                                        <li>�����Ţͧ��Ҫԡ ��к��зӡ�û�Ѻ��ا������ �ء � ��͹ �ҡ��Ҫԡ��ҹ㴾����������ç�����բ��ʧ���
                                            ��سҵԴ������˹�ҷ��</li>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                        <asp:Literal ID="RegMem" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        &nbsp;
                                    </td>
                                    <td >
                                        &nbsp;
                                        <%--<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>--%>
                                    </td>
                                </tr>
                                </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
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
