<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Saving.Flash.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GCOOP - Isocare</title>
    <style type="text/css">
        #selectextra
        {
            width: 150px;
            float: right;
            text-align: right;
        }
        .div_select
        {
            width: 700px;
            margin-left: 400px;
            font-weight: bold;
            color: White;
            text-align: right;
        }
    </style>
</head>
<body bgcolor="#000000">
    <form id="form1" runat="server">
    <div>
        <p align="center">
            <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"
                width="980" height="550" id="Ghome" align="middle">
                <param name="allowScriptAccess" value="sameDomain" />
                <param name="movie" value="../flash/Ghome.swf" />
                <param name="quality" value="high" />
                <param name="bgcolor" value="#000000" />
                <embed src="../flash/Ghome.swf" quality="high" bgcolor="#000000" width="980" height="550"
                    name="Ghome" align="middle" allowscriptaccess="sameDomain" type="application/x-shockwave-flash"
                    pluginspage="http://www.macromedia.com/go/getflashplayer" />
            </object>
        </p>
        <div class="div_select">
            <table style="width: 100%;">
                <tr>
                    <td>
                        ระบบงานเสริม :&nbsp;
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Login.aspx?app=app_assist">ระบบ สวัสดิการ</asp:HyperLink>
                    </td>
                    <td>
                        <a href="#">ระบบ HR</a>
                    </td>
                    <td>
                        <a href="#">ระบบ CMD</a>
                    </td>
                    <td>
                        <a href="#" onclick="window.open('http://localhost:8889/webportal/Default.aspx')">ระบบ ITM</a>
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx?app=mis">ระบบ MIS</asp:HyperLink>
                    </td>
                    
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
