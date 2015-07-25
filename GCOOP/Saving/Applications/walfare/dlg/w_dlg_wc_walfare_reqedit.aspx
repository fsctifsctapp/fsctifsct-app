<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_wc_walfare_reqedit.aspx.cs" 
Inherits="Saving.Applications.walfare.dlg.w_dlg_wc_walfare_reqedit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%=postJsShowlist %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        //    function OnDwMainItemChanged(s, r, c, v) {
        //        switch(c) {
        //            case "member_no":
        //                Gcoop.GetEl("Hdchkchange").value = "member_no"; 
        //                break;
        //            case "card_person":
        //                Gcoop.GetEl("Hdchkchange").value = "card_person"; 
        //                break;
        //            case "deptaccount_name":
        //                Gcoop.GetEl("Hdchkchange").value = "deptaccount_name"; 
        //                break;
        //            case "deptaccount_sname":
        //                Gcoop.GetEl("Hdchkchange").value = "deptaccount_sname"; 
        //                break;
        //        }
        //        postJsShowlist();
        //    }
        function OnDwMainButtonClicked(s, r, c, v) {
            if (c == "b_search") {
                postJsShowlist();
            }
        }
        function selectRow(sender, rowNumber, objectName) {
            deptrequest_docno = objDwList.GetItem(rowNumber, "deptrequest_docno");
            //            alert(deptrequest_docno);
            parent.setReqdocNo(deptrequest_docno);
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
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_reqedit_search"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_reqedit.pbl" ClientScriptable="True" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwMainItemChanged" 
            ClientEventButtonClicked="OnDwMainButtonClicked" ClientFormatting="True">
        </dw:WebDataWindowControl>
        <br /> <br />
        <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_wc_reqedit_search_detail"
            LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_reqedit.pbl" ClientEventClicked="selectRow"
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
    </div>
    </form>
</body>
</html>
