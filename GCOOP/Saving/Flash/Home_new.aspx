<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home_new.aspx.cs" Inherits="Saving.Flash.Home1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .seticonleftside
        {
            cursor: pointer;
            position: absolute;
            margin: 0 0 0 30px;
            z-index: 100;
        }
        .seticonleftside img:hover
        {
            filter: Gray;
        }
        .seticonrightside
        {
            cursor: pointer;
            position: absolute;
            margin: 0 0 0 695px;
            z-index: 100;
        }
        .seticonrightside img:hover
        {
            filter: Gray;
        }
        .leftdescription
        {
            position: absolute;
            cursor: pointer;
            text-align: right;
            margin: 140px 0 0 300px;
            z-index: 90;
        }
        .rithtdescription
        {
            position: absolute;
            cursor: pointer;
            text-align: left;
            margin: 140px 0 0 510px;
            z-index: 90;
        }
        .style1
        {
            font-weight: bold;
            text-decoration: underline;
        }
        body
        {
            /*text-align: center; for IE */
            /*margin: 0 auto;  for the rest */
            background-color: rgb(200,235,255);
        }
    </style>

    <script type="text/javascript">

//        this.moveTo(0, 0);
//        resizeTo(screen.availWidth, screen.availHeight);

        var strmain = "", strltext_1 = "", strltext_2 = "", strltext_3 = "", strltext_4 = "";
        var stradd = "", strrtext_1 = "", strrtext_2 = "", strrtext_3 = "", strrtext_4 = "";

        function overlefticon(licon) {
            setleftValue(licon);
            var main = document.getElementById(strmain);
            var ltext_1 = document.getElementById(strltext_1);
            main.style.margin = "0 0 0 0";
            ltext_1.style.visibility = "visible";

        }

        function overrighticon(ricon) {
            setrightValue(ricon)
            var add = document.getElementById(stradd);
            var rtext_1 = document.getElementById(strrtext_1);
            add.style.margin = "0 0 0 0";
            rtext_1.style.visibility = "visible";


        }

        function setleftValue(licon) {
            strmain = "main" + licon;
            strltext_1 = "ltext" + licon + "_1";
        }
        function setrightValue(ricon) {
            stradd = "add" + ricon;
            strrtext_1 = "rtext" + ricon + "_1";
        }
        function pageopen(sys) {
            if (sys == "main1") { window.location = "../Login.aspx?app=shrlon"; }
            if (sys == "main2") { window.location = "../Login.aspx?app=ap_deposit"; }
            if (sys == "main3") { window.location = "../Login.aspx?app=app_finance"; }
            if (sys == "main4") { window.location = "../Login.aspx?app=account"; }
            if (sys == "main5") { window.location = "../Login.aspx?app=keeping"; }
            if (sys == "expand1") { window.location = "../Login.aspx?app=app_assist"; }
            if (sys == "expand2") { window.location = "../Login.aspx?app=hr"; }
            if (sys == "expand3") { window.location = "../Login.aspx?app=cmd"; }
            if (sys == "expand4") { window.location = "../Login.aspx?app=mis"; }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="seticonleftside" style="margin-top: 140px">
        <img id="main1" alt="" src="Main1.png" onclick="pageopen('main1')" />
    </div>
    <div class="seticonleftside" style="margin-top: 265px">
        <img id="main2" alt="" src="Main2.png" onclick="pageopen('main2')" />
    </div>
    <div class="seticonleftside" style="margin-top: 390px">
        <img id="main3" alt="" src="Main3.png" onclick="pageopen('main3')" />
    </div>
    <div class="seticonleftside" style="margin-top: 510px">
        <img id="main4" alt="" src="Main4.png" onclick="pageopen('main4')" />
    </div>
    <div class="seticonleftside" style="margin-top: 635px">
        <img id="main5" alt="" src="Main5.png" onclick="pageopen('main5')" />
    </div>
    <div class="seticonrightside" style="margin-top: 140px">
        <img id="add1" alt="" src="Expand1.png" onclick="pageopen('expand1')" />
    </div>
    <div class="seticonrightside" style="margin-top: 265px">
        <img id="add2" alt="" src="Expand2.png" onclick="pageopen('expand2')" />
    </div>
    <div class="seticonrightside" style="margin-top: 390px">
        <img id="add3" alt="" src="Expand3.png" onclick="pageopen('expand3')" />
    </div>
    <div class="seticonrightside" style="margin-top: 510px">
        <img id="add4" alt="" src="Expand4.png" onclick="pageopen('expand4')" />
    </div>
    <div class="seticonrightside" style="margin-top: 635px">
    </div>
    <div>
        <%--    <asp:Literal ID="ltr_leftside" runat="server"></asp:Literal>
    <asp:Literal ID="ltr_rightside" runat="server"></asp:Literal>--%>
        <div class="leftdescription" style="margin-top: 135px">
            <span id="ltext1_1" style="visibility: visible;" onclick="pageopen('main1')">
            <span class="style1">ระบบหุ้นหนี้</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="leftdescription" style="margin-top: 255px">
            <span id="ltext2_1" style="visibility: visible;" onclick="pageopen('main2')"><span
                class="style1">ระบบเงินฝาก</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="leftdescription" style="margin-top: 380px">
            <span id="ltext3_1" style="visibility: visible;" onclick="pageopen('main3')"><span
                class="style1">ระบบการเงิน</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="leftdescription" style="margin-top: 505px">
            <span id="ltext4_1" style="visibility: visible;" onclick="pageopen('main4')"><span
                class="style1">ระบบบัญชี</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="leftdescription" style="margin-top: 625px">
            <span id="ltext5_1" style="visibility: visible" onclick="pageopen('main5')"><span
                class="style1">ระบบจัดเก็บ</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        
        
        
        <div class="rithtdescription" style="margin-top: 135px">
            <span id="rtext1_1" style="visibility: visible;" onclick="pageopen('expand1')"><span
                class="style1">ระบบสวัสดิการ</span><br />
                <br />
                9 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="rithtdescription" style="margin-top: 255px">
            <span id="rtext2_1" style="visibility: visible;" onclick="pageopen('expand2')"><span
                class="style1">ระบบ HR</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="rithtdescription" style="margin-top: 380px">
            <span id="rtext3_1" style="visibility: visible;" onclick="pageopen('expand3')"><span
                class="style1">ระบบพัสดุครุภัณฑ์</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
        <div class="rithtdescription" style="margin-top: 505px">
            <span id="rtext4_1" style="visibility: visible;" onclick="pageopen('expand4')"><span
                class="style1">ระบบ MIS</span><br />
                <br />
                09 / 05 / 2553<br />
                สถานะปิดงานสิ้นวัน :<br />
                ปิดงานสิ้นวันเรียบร้อยแล้ว</span>
        </div>
    </div>
    <table width="940" border="0" style="margin: 120px 0 0 20px; position: absolute;">
        <tr>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('main1')">
            </td>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('expand1')">
            </td>
        </tr>
        <tr>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('main2')">
            </td>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('expand2')">
            </td>
        </tr>
        <tr>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('main3')">
            </td>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('expand3')">
            </td>
        </tr>
        <tr>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('main4')">
            </td>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('expand4')">
            </td>
        </tr>
        <tr>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;"
                onclick="pageopen('main5')">
            </td>
            <td style="height: 120px; background-color: white; filter: alpha(opacity=20); cursor: pointer;">
            </td>
        </tr>
    </table>
    <img alt="" src="HomeBG.jpg" />
    </form>
</body>
</html>
