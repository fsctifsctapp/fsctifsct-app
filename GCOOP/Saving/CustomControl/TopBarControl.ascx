<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBarControl.ascx.cs"
    Inherits="Saving.CustomControl.TopBarControl" %>
<link id="CssTopBar" href="../Css/Topbar.css" rel="stylesheet" type="text/css" runat="server" />
<table cellpadding="0" cellspacing="0" class="topbar">
    <tr>
        <td style="font-family: Tahoma; font-size: 10px; font-weight: bold;">
            &nbsp;
            <asp:CheckBox ID="CbDbProfile" runat="server" Text=" DBProfile" Font-Bold="True"
                Font-Names="Tahoma" Font-Size="10px" />
            <asp:DropDownList ID="DdDbProfile" runat="server" Enabled="False" AutoPostBack="True"
                OnSelectedIndexChanged="DdDbProfile_SelectedIndexChanged" Width="150px">
            </asp:DropDownList>
            <asp:Label ID="LbDbProfile" runat="server" Text=""></asp:Label>
            <asp:Label ID="LbApplication" runat="server" Text="| &nbsp; Application" Font-Bold="True"
                Font-Names="Tahoma" Font-Size="10px"></asp:Label>
            <asp:Label ID="LbWorkDate" runat="server" Text="| &nbsp; WorkDate" Font-Bold="True"
                Font-Names="Tahoma" Font-Size="10px"></asp:Label>
            <asp:Label ID="LbUsername" runat="server" Text="| &nbsp; Username" Font-Bold="True"
                Font-Names="Tahoma" Font-Size="10px"></asp:Label>
            <asp:Label ID="LbBranch" runat="server" Text="| &nbsp; Branch" Font-Bold="True" Font-Names="Tahoma"
                Font-Size="10px"></asp:Label>
            <asp:Label ID="LbIpAddress" runat="server" Text="| &nbsp; IP" Font-Bold="True" Font-Names="Tahoma"
                Font-Size="10px"></asp:Label>
            <asp:Label ID="LbLoadTime" runat="server" Text="| &nbsp; Load" Font-Bold="True" Font-Names="Tahoma"
                Font-Size="10px"></asp:Label>
        </td>
    </tr>
</table>
<script type="text/javascript">
    var objCbProfile = document.getElementById('<%=CbDbProfile.ClientID%>');
    var objDdProfile = document.getElementById('<%=DdDbProfile.ClientID%>');

    objCbProfile.onclick = function () {
        if (objCbProfile.checked) {
            objDdProfile.disabled = false;
        } else {
            objDdProfile.disabled = true;
        }
    }
</script>
