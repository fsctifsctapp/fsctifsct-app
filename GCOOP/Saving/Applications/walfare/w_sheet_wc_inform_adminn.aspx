<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
 CodeBehind="w_sheet_wc_inform_adminn.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_inform_adminn" %>

 <%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsInitinform %>
    <%=jsResignChange %>
    <%=jsInitinformOld %>
    <%=jssetDataResignChange %>
    <script type="text/javascript">
        function Validate() {
            if (Gcoop.GetEl("HdStatus").value == "1") {
                return confirm("ยืนยันการบันทึกข้อมูล");
            } else {
                alert("ไม่สามารถบันทึกข้อมูลได้");
            }
        }
        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "deptaccount_no":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsInitinform();
                    break;
                case "resigncause_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsResignChange();
                    break;
                case "inform_tdate":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsResignChange();
                    break;
                case "quantitymem_amt":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jssetDataResignChange();
                    break;
            }
        }
        function OnDwMainClick(s, r, c) {
            switch (c) {
                case "b_search":
                    Gcoop.OpenIFrame("560", "620", "w_dlg_wc_inform_new_admin.aspx");
                    break;
            }
        }
        function setAccnoNew(deptaccount_no) {
            objDwMain.SetItem(1, "deptaccount_no", deptaccount_no);
            Gcoop.GetEl("HdStatus").value = "1";
            jsInitinform();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("560", "620", "w_dlg_wc_inform_old.aspx");
        }
        function setAccnoOld(dpreqchg_doc) {
            objDwMain.SetItem(1, "dpreqchg_doc", dpreqchg_doc);
            Gcoop.GetEl("HdStatus").value = "0";
            jsInitinformOld();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wf_informchg"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_inform.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventClicked="OnDwMainClick" AutoRestoreContext="False" ClientFormatting="True">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="Dwdiem" runat="server" DataWindowObject="d_wc_inform_die_sele_month"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_inform.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwdiemItemChanged"
        ClientEventClicked="OnDwdiemClick" AutoRestoreContext="False" ClientFormatting="True">
    </dw:WebDataWindowControl>

    <asp:HiddenField ID="HdStatus" runat="server" Value=""/>
    <asp:HiddenField ID="HdModeSave" runat="server" Value=""/>
</asp:Content>

