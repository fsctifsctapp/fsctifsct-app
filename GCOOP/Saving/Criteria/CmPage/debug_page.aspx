<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="debug_page.aspx.cs" Inherits="Saving.Debug.debug_page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debug Page</title>
    <script type="text/JavaScript">
    function timedRefresh(timeoutPeriod) {
	    setTimeout("location.reload(true);",timeoutPeriod);
    }
    </script>
</head>
<body onload="JavaScript:timedRefresh(3000);">
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%;" border="1">
            <tr>
                <td>
                    Index
                </td>
                <td>
                    SessionName
                </td>
                <td>
                    SessionDesc
                </td>
            </tr>
            <asp:Literal ID="LtSessionList" runat="server"></asp:Literal>
        </table>
    </div>
    </form>
</body>
</html>