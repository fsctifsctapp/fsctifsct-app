<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_permission_all.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_permission_all" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsselectAdmin %>
    <%=jsselectRegister %>
    <%=jsselectEditRegis %>
    <%=jsselectMember %>
    <%=jsselectResign %>
    <%=jsselectPaid %>
    <script type="text/javascript">
        function OnDwMainItemChanged(s, r, c, v) {
            return 0;
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function selectRow(sender, rowNumber, objectName) {
            if (rowNumber > 0 && objectName != "datawindow") {
                if (objectName != "user_permiss" && objectName != "user_type" && objectName != "check_flag" && objectName != "editreqregis_flag" && objectName != "memb_flag" && objectName != "resign_flag" && objectName != "paid_flag") {
                    var username = objDwMain.GetItem(rowNumber, "user_name");
                    Gcoop.OpenIFrame("560", "570", "w_dlg_wc_branch_info.aspx", "?username=" + username);
                }
            }
            return 0;
        }
        function selectAdmin() {
            jsselectAdmin();
        }
        function selectRegister() {
            jsselectRegister();
        }
        function selectEditRegis() {
            jsselectEditRegis();
        }
        function selectMember() {
            jsselectMember();
        }
        function selectResign() {
            jsselectResign();
        }
        function selectPaid() {
            jsselectPaid();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <p style="color:Red; font-family:CordiaUPC; font-size:25px;">
    Check All&nbsp::&nbsp&nbsp
    <select id="uadmin" runat="server" onchange="selectAdmin();">
        <option value="1">Admin</option>
        <option value="3">แก้ไขได้</option>
        <option value="2">ปกติ</option>    
    </select>

    <%--<input type="checkbox" id="uadmin2" onchange="selectAdmin();" runat="server" />--%>admin
    &nbsp&nbsp
    <input type="checkbox" id="upermiss" onchange="selectRegister();" runat="server" />รับสมัคร
    &nbsp&nbsp
    <input type="checkbox" id="editregis" onchange="selectEditRegis();" runat="server" />แก้ไขสมัคร
    &nbsp&nbsp
    <input type="checkbox" id="member" onchange="selectMember();" runat="server" />ทะเบียน
    &nbsp&nbsp
    <input type="checkbox" id="resign" onchange="selectResign();" runat="server" />แจ้งออก/เสียชีวิต
    &nbsp&nbsp
    <input type="checkbox" id="paid" onchange="selectPaid();" runat="server" />รับชำระ
    </p>    
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_permission_user" 
        LibraryList="~/DataWindow/walfare/w_sheet_wc_permission_all.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventClicked="selectRow"
        AutoRestoreContext="False" ClientFormatting="True" Width="720px" Height="700px">
    </dw:WebDataWindowControl>
    <%--<input type="button" value="UpdateReg" onclick="update_Reg();" />--%>
</asp:Content>
