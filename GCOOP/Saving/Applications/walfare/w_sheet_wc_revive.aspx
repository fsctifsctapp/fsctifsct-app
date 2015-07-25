<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_revive.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_revive" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsinitAccNo%>
    <%=jsResignCauseCode%>
    <script type="text/javascript">
        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "deptaccount_no":
                    jsinitAccNo();
                    break;
                case "resigncause_code":
                    jsResignCauseCode();
                    break;
                case "reqchg_status":
                    jsResignCauseCode();
                    break;
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwMainButtonClicked(s, r, c) {
            switch (c) {
                case "acoount_no_search":
                    jsinitAccNo();
                    break;
            }
        }


        function MenubarOpen() {
            Gcoop.OpenIFrame("620", "620", "w_dlg_wc_receive.aspx");

            var branch_id = objDwMain.GetItem(1, "branch_id");
            if (branch_id != "" && branch_id != null) {
                Gcoop.OpenIFrame("620", "620", "w_dlg_wc_receive.aspx", "?branch_id=" + branch_id);
            }
            else {
                alert("กรุณาระบุเลขที่สมาชิกก่อน");
            }
        }
        function setdeptNo(deptaccount_no) {
            objDwMain.SetItem(1, "deptaccount_no", deptaccount_no);
            jsinitAccNo();
        }

     
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">    
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_revive"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_inform.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="Hdbranch_id" runat="server" Value="0"/>
</asp:Content>
