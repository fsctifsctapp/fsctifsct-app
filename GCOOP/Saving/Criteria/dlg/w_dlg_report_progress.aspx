<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_report_progress.aspx.cs"
    Inherits="Saving.Applications.keeping.dlg.w_dlg_report_progress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>การดึงข้อมูลรายงาน</title>

    <script src="../../js/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript">
    var reload = 1;
    
    function ChangeTextColor(el,color){
        var tmp = Gcoop.GetEl(el).innerHTML;
        Gcoop.GetEl(el).innerHTML = '<font color="'+color+'">' + tmp + '</font>';
    }
    
    function sRefresh(text)
    {
        try{
            text = Gcoop.Trim(text);
            var arr = text.split(",");
            if( arr[6] != undefined ){
                Gcoop.GetEl("stext").innerHTML = arr[7];
                ChangeTextColor("stext","blue");
            }
            if(arr[0] == 1){
                Gcoop.GetEl("stext").innerHTML = arr[7];
                window.open("<%=pdf%>","_blank");
                parent.RemoveIFrame();
            } else if(arr[0] == -1){
                reload = 0;
                lastStatus = -1;
                Gcoop.GetEl("simg").src = "../../img/error.gif";
                Gcoop.GetEl("stext").innerHTML = "ออกรายงานไม่สำเร็จ, กรุณาคลิกปุ่ม ปิด แล้วลองใหม่<br><br>debugInfo("+text+")<br>";
                Gcoop.GetEl("btnClose").style.visibility = "visible";
                ChangeTextColor("stext","red");
            }
        }catch(i6969){
            Gcoop.GetEl("stext").innerHTML = "JS"+i6969;
            ChangeTextColor("stext","red");
        }
    }
    
    function sReload()
    {
        if( reload == 0 ) return;
        try{
            var currDate = new Date();
            var urls = parent.Gcoop.GetUrl() + "Criteria/AjaxReportProcess.aspx?rand=" + currDate.getMinutes() + currDate.getSeconds();
            urls = urls+"&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>";
            Ajax.toPost(urls, "", sRefresh);
            ++secCount;
            document.getElementById("scount").innerHTML = secCount.toString()+" times";
            setTimeout("sReload()", 1000);
        }catch(err){
        }
    }
    
    function DialogLoadComplete(){
        sReload();
    }
    
    var secCount = 0;
    </script>

</head>
<body>
    <p>
        <br />
    </p>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div align="center">
        <asp:Label ID="stext" runat="server" Text="กำลังดึงข้อมูลรายงาน,รอสักครู่" ForeColor="#267BCA"
            Font-Size="12px" Font-Bold="False" Font-Names="Tahoma"></asp:Label>
    </div>
    <div align="center">
        <asp:Image ID="simg" runat="server" ImageUrl="~/img/processing.gif" />
    </div>
    <div align="center">
        <asp:Label ID="scount" runat="server" Font-Names="Tahoma" Font-Size="12px"></asp:Label>
    </div>
    <div align="center">
        <br />
        <input id="btnClose" style="visibility: hidden" type="button" value="ปิด" onclick="parent.RemoveIFrame();" />
    </div>
    </form>
</body>
</html>
