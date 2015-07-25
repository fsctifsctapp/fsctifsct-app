<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_wc_branch_info.aspx.cs" Inherits="Saving.Applications.walfare.dlg.w_dlg_wc_branch_info" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%=postselectbranch_id %>
    <%=postselectbranch_desc %>
    <%=postSaveInformation %>
    <script type="text/javascript">
        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "coopbranch_id_1") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postselectbranch_id();
            }
            else if (c == "coopbranch_id") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postselectbranch_desc();
            }
        }
        function selectRow(sender, rowNumber, objectName) {
            deptrequest_docno = objDwList.GetItem(rowNumber, "deptrequest_docno");
            parent.setReqdocNo(deptrequest_docno);
            parent.RemoveIFrame();
        }
        function closepage() {
            parent.RemoveIFrame();
        }
        function SaveInformation() {
            postSaveInformation();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_branch_info"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_permission_all.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="12" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChanged" >
        </dw:WebDataWindowControl>
         <dw:WebDataWindowControl ID="DwPermiss" runat="server" DataWindowObject="d_wc_wins_permiss"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_permission_all.pbl" 
            ClientScriptable="True" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <br />
    <div id="buttonExit" align="center">
        <input type="button" id="saveinfo" value="บันทึก" onclick="SaveInformation();"/>
        <input type="button" id="closeifram" value="ปิดหน้าจอ" onclick="closepage();"/>
    </div>
    </form>
</body>
</html>
