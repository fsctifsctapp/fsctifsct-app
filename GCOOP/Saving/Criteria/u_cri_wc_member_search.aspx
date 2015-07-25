<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="u_cri_wc_member_search.aspx.cs" Inherits="Saving.Criteria.u_cri_wc_member_search" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsinitAccNo %>
    <%=postPost %>
    <%=postProvince%>
    <%=other_postPost%>
    <%=other_postProvince%>
    <%=jsAddRelateRow %>
    <%=jsDeleteRelateRow %>
    <script type="text/javascript">
        //        function MenubarOpen() {
        //            Gcoop.OpenIFrame("560", "620", "w_dlg_wc_walfare_reqedit.aspx");
        //        }
        function checkID(id) {
            if (id.length != 13) {
                return false;
            }
            for (i = 0, sum = 0; i < 12; i++) {
                sum += parseFloat(id.charAt(i)) * (13 - i);
            }
            if ((11 - sum % 11) % 10 != parseFloat(id.charAt(12))) {
                return false;
            }
            return true;
        }
        function Validate() {
            var cardPS = objDwMain.GetItem(1, "card_person");
            cardPS = Gcoop.Trim(cardPS);
            try {
                //                if (cardPS == "" || cardPS == null) {
                //                    alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
                //                    return false;
                //                }

                //                chknum = Gcoop.IsNum(cardPS);
                //                if (chknum == false) {
                //                    alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
                //                    return false;
                //                }

                //                var chkPS = checkID(cardPS);
                //                if (!chkPS) {
                //                    alert("กรุณากรอกเลขบัตรประชาชนให้ถูกต้องครบ 13 หลัก");
                //                    return false;
                //                }

                var chknum = 0;

                //                var membNo = objDwMain.GetItem(1, "member_no");
                //                membNo = Gcoop.Trim(membNo);

                //                if (membNo == "" || membNo == null) {
                //                    alert("กรุณากรอกเลขสมาชิกสหกรณ์ตัวเลข และไม่มีช่องว่าง");
                //                    return false;
                //                }

                //            chknum = Gcoop.IsNum(membNo);
                //            if (chknum == false) {
                //                alert("กรุณากรอกเลขสมาชิกสหกรณ์ตัวเลข และไม่มีช่องว่าง");
                //                return false;
                //            }
                var apply_tdate = objDwMain.GetItem(1, "apply_tdate");
                var birthday_tdate = objDwMain.GetItem(1, "wfbirthday_tdate");
                var deptaccount_name = objDwMain.GetItem(1, "deptaccount_name");
                var deptaccount_sname = objDwMain.GetItem(1, "deptaccount_sname");
                if (Gcoop.Trim(apply_tdate) == null || Gcoop.Trim(apply_tdate) == "") {
                    alert("กรุณากรอกวันที่สมัคร");
                    return false;
                }
                if (Gcoop.Trim(birthday_tdate) == null || Gcoop.Trim(birthday_tdate) == "") {
                    alert("กรุณากรอกวันเกิดผู้สมัคร");
                    return false;
                }
                if (Gcoop.Trim(deptaccount_name) == null || Gcoop.Trim(deptaccount_name) == "") {
                    alert("กรุณากรอกชื่อผู้สมัคร");
                    return false;
                }
                if (Gcoop.Trim(deptaccount_sname) == null || Gcoop.Trim(deptaccount_sname) == "") {
                    alert("กรุณากรอกนามสกุลผู้สมัคร");
                    return false;
                }
            } catch (err) {
            }

            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "ampher_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    postPost();
                    break;
                case "province_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    postProvince();
                    break;
                case "other_ampher_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    other_postPost();
                    break;
                case "other_province_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    other_postProvince();
                    break;

            }
        }
        function OnDwMainButtonClicked(s, r, c) {
            switch (c) {
                case "acoount_no_search":
                    jsinitAccNo();
                    break;
            }
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("600", "620", "w_dlg_wc_deptedit.aspx");
        }
        function setdeptNo(deptaccount_no) {
            objDwMain.SetItem(1, "deptaccount_no", deptaccount_no);
            jsinitAccNo();
        }

        function ShowTabPage2(tab) {
            var i = 1;
            var tabamount = 5;
            for (i = 1; i <= tabamount; i++) {
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).className = "tabTypeTdSelected";
                } else {
                    document.getElementById("tab_" + i).style.visibility = "hidden";
                    document.getElementById("stab_" + i).className = "tabTypeTdDefault";
                }
            }
            Gcoop.GetEl("HdTabIndex").value = tab + "";
        }

        function SheetLoadComplete() {
            ShowTabPage2(Gcoop.ParseInt(Gcoop.GetEl("HdTabIndex").value));
        }

        function AddRelateRow() {
            //            var actionstat = Gcoop.GetEl("HdActionStatus").value;
            //            if (actionstat == "") {
            //                Gcoop.GetEl("HdActionStatus").value = "add";
            jsAddRelateRow();
            //            } else {
            //                alert("กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
            //            }
        }
        function OnDeleteRow(s, r, c) {
            if (c == "b_delete" && r > 0) {
                //                var actionstat = Gcoop.GetEl("HdActionStatus").value;
                //                var maxRow = s.RowCount();
                //                if (actionstat == "") {
                Gcoop.GetEl("HdRow").value = r + "";
                //                    Gcoop.GetEl("HdActionStatus").value = "del";
                jsDeleteRelateRow();
                //                } else if (actionstat == "add" && r == maxRow) {
                //                    Gcoop.GetEl("HdActionStatus").value = "";
                //                    postInitWalfareReq();
                //                } else {
                //                    alert("กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
                //                }
            }
        }
    </script>
    <style type="text/css">
        .tabTypeDefault
        {
            width: 700px;
            border-spacing: 2px;
            margin-left: 6px;
        }
        .tabTypeTdDefault
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #777777;
            border: solid 1px #55A9CD;
            background-color: rgb(200,235,255);
            cursor: pointer;
        }
        .tabTypeTdSelected
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #660066;
            border: solid 1px #77CBEF;
            background-color: #76EFFF;
            cursor: pointer;
            text-decoration: underline;
        }
        .tabTypeTdDefault:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTypeTdSelected:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTableDetail
        {
            width: 743px;
            margin-left: 6px;
        }
        .tabTableDetail td
        {
        }
        .tableMessage
        {
            border: solid 1px #77771;
            width: 743px;
            margin-left: 6px;
        }
        .tableMessage td
        {
            font-family: Tahoma, Sans-Serif, Serif;
            font-size: 14px;
            font-weight: bold;
            border: solid 1px #EE0022;
            text-align: center;
            vertical-align: middle;
            margin-top: 15px;
            margin-bottom: 15px;
            background-color: #FDDDAA;
            height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_dp_dept_edit_master"
        LibraryList="~/DataWindow/criteria/criteria.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <table class="tabTypeDefault">
        <tr>
            <td class="tabTypeTdSelected" id="stab_1" onclick="ShowTabPage2(1);">
                ผู้รับผลประโยชน์
            </td>
            <td class="tabTypeTdDefault" id="stab_2" onclick="ShowTabPage2(2);">
                ข้อมูลการเรียกเก็บเงิน
            </td>
            <td class="tabTypeTdDefault" id="stab_3" onclick="ShowTabPage2(3);">
                ข้อมูลการโอนย้าย
            </td>
        </tr>
    </table>
    <table class="tabTableDetail">
        <tr>
            <td style="height: 470px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label1" runat="server" Text="ผู้รับผลประโยชน์" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    &nbsp; &nbsp; &nbsp; <span onclick="AddRelateRow();" style="font: Tahoma; font-size: 14px;
                        color: #0099CC; cursor: pointer;">เพิ่มแถว</span>
                    <br />
                    <dw:WebDataWindowControl ID="DwCodept" runat="server" DataWindowObject="d_wc_master_codept"
                        LibraryList="~/DataWindow/criteria/criteria.pbl" ClientScriptable="True"
                        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
                        AutoRestoreContext="False" ClientFormatting="True" ClientEventButtonClicked="OnDeleteRow">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label2" runat="server" Text="ข้อมูลการเรียกเก็บเงิน" Font-Bold="True"
                        Font-Names="Tahoma" Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    <br />
                    <dw:WebDataWindowControl ID="DwStment" runat="server" DataWindowObject="d_dp_dept_edit_statement"
                        LibraryList="~/DataWindow/criteria/criteria.pbl" ClientScriptable="True"
                        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
                        AutoRestoreContext="False" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_3" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label3" runat="server" Text="ข้อมูลการโอนย้าย" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    <br />
                    <dw:WebDataWindowControl ID="Dwtrn" runat="server" DataWindowObject="d_dp_dept_trn_member"
                        LibraryList="~/DataWindow/criteria/criteria.pbl" ClientScriptable="True"
                        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
                        ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdTabIndex" runat="server" Value="1" />
    <asp:HiddenField ID="HdActionStatus" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="0" />
    <asp:HiddenField ID="HdSeq_no" runat="server" Value="0" />
    <asp:HiddenField ID="Hdrowcount" runat="server" Value="0" />
</asp:Content>