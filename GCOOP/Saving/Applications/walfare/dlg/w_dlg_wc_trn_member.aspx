﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_wc_trn_member.aspx.cs" Inherits="Saving.Applications.walfare.dlg.w_dlg_wc_trn_member" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
         <%=postJsShowlist %>
        <script type="text/javascript">
            function OnDwMainButtonClicked(s, r, c) {

                if (c == "b_search") {
                    postJsShowlist();
                }
            }
            function selectRow(sender, rowNumber, objectName) {
                var detaccount_no = objDwList.GetItem(rowNumber, "deptaccount_no");
                parent.setdeptNo(detaccount_no);
                parent.RemoveIFrame();
            }
            function DialogLoadComplete() {
                if (Gcoop.GetEl("HdExtraSearchMode").value != "") {
                    postJsShowlist();
                }
            }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <h4 align="left" style="color: #0099CC;">ค้นหา</h4>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_dlg_trn_search"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientScriptable="True" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwMainItemChanged" 
            ClientEventButtonClicked="OnDwMainButtonClicked" ClientFormatting="True">
        </dw:WebDataWindowControl>
        <br /> <br />
        <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_wc_dlg_trn_search_detail"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_trn_memb.pbl" ClientEventClicked="selectRow"
            ClientScriptable="True" RowsPerPage="12" HorizontalScrollBar="Auto" VerticalScrollBar="Auto"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <br />
        <div align="center">
            <input type="button" value="ปิดหน้าจอ" onclick="parent.RemoveIFrame();" />
        </div>
        <asp:HiddenField ID="Hdchkchange" runat="server" Value=""/>
        <asp:HiddenField ID="HdExtraSearchMode" Value="" runat="server" />
        <asp:HiddenField ID="HdExtraSearchCode" Value="" runat="server" />
        <asp:HiddenField ID="HdBranch" Value="" runat="server" />
    </div>
    </form>
</body>
</html>
