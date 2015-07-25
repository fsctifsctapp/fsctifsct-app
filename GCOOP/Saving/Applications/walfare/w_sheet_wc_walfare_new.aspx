<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_walfare_new.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_walfare_new" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postPost%>
    <%=postProvince%>
    <%=postAddRowDwRelate%>
    <%=postDelRowDwRelate%>
    <script type="text/javascript">
        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "ampher_code") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postPost();
            } else if (c == "province_code") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postProvince();
            }
            return 0;
        }

        function AddRowDwRelate() {
            postAddRowDwRelate();
            return 0;
        }

        function OnDwMainClick(s, r, c) {
            return 0;
        }

        function OnDwRelateButtonClicked(s, r, c) {
            if (c == "b_delete" && r > 0) {
                Gcoop.GetEl("HdDeleteRow").value = r + "";
                postDelRowDwRelate();
            } else if (c == "b_addrow") {
                AddRowDwRelate();
            }
            return 0;
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_dp_reqdepoist_main"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_new.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientEventClicked="OnDwMainClick" ClientFormatting="True"
        TabIndex="200">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label1" runat="server" Text="การชำระเงิน" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
    <br />
    <dw:WebDataWindowControl ID="DwSlip" runat="server" DataWindowObject="d_wf_reqdetail"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_new.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
        ClientFormatting="True" TabIndex="400">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Lb001" runat="server" Text="ผู้รับผลประโยชน์" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
    <br />
    <dw:WebDataWindowControl ID="DwRelate" runat="server" DataWindowObject="d_dp_reqcodeposit"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_new.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
        ClientFormatting="True" ClientEventButtonClicked="OnDwRelateButtonClicked" TabIndex="600">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdDeleteRow" runat="server" />
</asp:Content>
