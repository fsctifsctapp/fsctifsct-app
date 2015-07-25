<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_wc_walfare_reqedit.aspx.cs" 
Inherits="Saving.Applications.walfare.w_sheet_wc_walfare_reqedit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postPost%>
    <%=postProvince%>
    <%=other_postPost%>
    <%=other_postProvince%>
    <%=postInitcardPS%>
    <%=postInitmembNo %>
    <%=jsAddRelateRow %>
    <%=jsDeleteRelateRow %>
    <%=jsInitWalfareReq %>
    <%=jsChangeMemtype %>

    <script type="text/javascript">
//        function hideDVtest() {
//            var divstyle = new String();
//            divstyle = document.getElementById("testjaaa").style.visibility;
//            if (divstyle.toLowerCase() == "visible" || divstyle == "") {
////                alert("hide");
//                //            Gcoop.GetEl("testjaaa").style.visibility = "hidden";
//                document.getElementById("testjaaa").style.visibility = "hidden";
//            } else {
//                document.getElementById("testjaaa").style.visibility = "visible";
//            }
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
//            try {
//                var cardPS = objDwMain.GetItem(1, "card_person");
//            } catch (err) {
//                alert("กรุณากรอกเลขบัตรประชาชน");
//                return false;
//            }
//            cardPS = Gcoop.Trim(cardPS);

//            if (cardPS == "" || cardPS == null) {
//                alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

//            chknum = Gcoop.IsNum(cardPS);
//            if (chknum == false) {
//                alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

//            var chkPS = checkID(cardPS);
//            if (!chkPS) {
//                alert("กรุณากรอกเลขบัตรประชาชนให้ถูกต้องครบ 13 หลัก");
//                return false;
//            }

            var chknum = 0;
//            try {
//                var membNo = objDwMain.GetItem(1, "member_no");
//            } catch (err) {
//                alert("กรุณากรอกเลขสมาชิกสหกรณ์");
//                return false;
//            }
//            membNo = Gcoop.Trim(membNo);

//            if (membNo == "" || membNo == null) {
//                alert("กรุณากรอกเลขสมาชิกสหกรณ์ตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

//            chknum = Gcoop.IsNum(membNo);
//            if (chknum == false) {
//                alert("กรุณากรอกเลขสมาชิกสหกรณ์ตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }
            try {
                var apply_tdate = objDwMain.GetItem(1, "apply_tdate");
                var birthday_tdate = objDwMain.GetItem(1, "birthday_tdate");
            } catch (err) {
                alert("กรุณากรอกวันเดือนปีเกิด และวันสมัครให้ครบ");
                return false;
            }
            try {
                var deptaccount_name = objDwMain.GetItem(1, "deptaccount_name");
                var deptaccount_sname = objDwMain.GetItem(1, "deptaccount_sname");
            } catch (err) {
                alert("กรุณากรอกชื่อ - นามสกุล");
                return false;
            }
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

            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function SheetLoadComplete() {
            var chkMembRow = Gcoop.GetEl("HdchkMembRow").value == "true";
            var chkCardPs = Gcoop.GetEl("HdchkcardPs").value == "true";

            if (chkMembRow == true) {
                var membNo = Gcoop.GetEl("HdMember_no").value;
                Gcoop.OpenIFrame("600", "620", "w_dlg_wc_walfare_reqedit.aspx", "?searchMode=member&searchCode=" + membNo);
            } else if (chkCardPs == true) {
                var cardPs = Gcoop.GetEl("HdcardPs").value;
                Gcoop.OpenIFrame("600", "620", "w_dlg_wc_walfare_reqedit.aspx", "?searchMode=card&searchCode=" + cardPs);
            }

            if (Gcoop.GetEl("HdSaveStatus").value == "1") {
                if (confirm("ต้องการพิมพ์ใบเสร็จหรือไม่")) {
                    //                    Gcoop.OpenDlg("1000", "1200", "w_dlg_wc_printslip.aspx", "?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value);
                    if (Gcoop.GetEl("HdFromMaster").value == "1") {
                        window.open("dlg/w_dlg_wc_printslip_frommaster.aspx?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value, "ใบเสร็จ", 'width=800,height=2000,scrollbars=1');
                    } else {
                        window.open("dlg/w_dlg_wc_printslip.aspx?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value, "ใบเสร็จ", 'width=800,height=2000,scrollbars=1');
                    }
                } else {
                    return false;
                }
            }
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
                case "wftype_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsChangeMemtype();
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

        function OnDwMainClick(s, r, c) {
            switch (c) {
                case "bcardperson_search":                
                    var cardPS = objDwMain.GetItem(r, "card_person");
                    cardPS = Gcoop.Trim(cardPS);

                    chknum = Gcoop.IsNum(cardPS);
                    if (chknum == false) {
                        alert("กรุณากรอกตัวเลข และไม่มีช่องว่าง");
                        return false;
                    }

                    if (cardPS.length == 13) {
                        postInitcardPS();
                    } else if (cardPS.length != 0 && cardPS.length != 13) {
                        alert("กรุณากรอกให้ครบ 13 หลัก");
                    }
                    break;
                case "bmember_search":
                    var chknum = 0;

                    var membNo = objDwMain.GetItem(r, "member_no");
                    membNo = Gcoop.Trim(membNo);

                    if (membNo == "") return false;

//                    chknum = Gcoop.IsNum(membNo);
//                    if (chknum == false) {
//                        alert("กรุณากรอกตัวเลข และไม่มีช่องว่าง");
//                        return false;
//                    } else {
//                        postInitmembNo();

//                    }
                    postInitmembNo();
                    break;
            }
        }
        function setReqdocNo(deptrequest_docno) {
            objDwMain.SetItem(1, "deptrequest_docno", deptrequest_docno);
            jsInitWalfareReq();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("600", "620", "w_dlg_wc_walfare_reqedit.aspx");
        }
        function AddRelateRow() {
            var actionstat = Gcoop.GetEl("HdActionStatus").value;
            if (actionstat == "") {
                Gcoop.GetEl("HdActionStatus").value = "add";    
                jsAddRelateRow();
            } else {
                alert("กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
            }
        }
        function OnDeleteRow(s, r, c) {
            if (c == "b_delete" && r > 0) {
                var actionstat = Gcoop.GetEl("HdActionStatus").value;
                var maxRow = s.RowCount();
                if (actionstat == "") {
                    Gcoop.GetEl("HdRow").value = r + "";
                    Gcoop.GetEl("HdActionStatus").value = "del";
                    jsDeleteRelateRow();
                } else if (actionstat == "add" && r == maxRow) {
                    Gcoop.GetEl("HdActionStatus").value = "";
                    postInitWalfareReq();                    
                }else{
                    alert("กรุณากดบันทึกข้อมูลก่อนจึงจะสามารถทำรายการต่อได้");
                }
            }
        }       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_dp_reqdepoist_main_reqedit"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_reqedit.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientEventClicked="OnDwMainClick" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <br />

    <asp:Label ID="Label1" runat="server" Text="การชำระเงิน" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
    <br />
        <dw:WebDataWindowControl ID="DwSlip" runat="server" DataWindowObject="d_wf_reqdetail_reqedit"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_reqedit.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
    <br />

    <asp:Label ID="Lb001" runat="server" Text="ผู้รับเงินสงคราะห์" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>       
   &nbsp; &nbsp; &nbsp;
    <span onclick="AddRelateRow();" style="font: Tahoma; font-size: 14px; color: #0099CC; cursor: pointer;">เพิ่มแถว</span>
    
    <br />
    <dw:WebDataWindowControl ID="DwRelate" runat="server" DataWindowObject="d_dp_reqcodeposit_reqedit"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_reqedit.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
        ClientFormatting="True" ClientEventButtonClicked="OnDeleteRow">
    </dw:WebDataWindowControl>


    <asp:HiddenField ID="HdActionStatus" runat="server" Value=""/>
    <asp:HiddenField ID="HdRow" runat="server" Value="0"/>
    <asp:HiddenField ID="HdSeq_no" runat="server" Value="0"/>
    <asp:HiddenField ID="HdchkMembRow" runat="server" Value=""/>
    <asp:HiddenField ID="HdchkcardPs" runat="server" Value=""/>
    <asp:HiddenField ID="HdMember_no" runat="server" Value=""/>
    <asp:HiddenField ID="HdcardPs" runat="server" Value=""/>

    <asp:HiddenField ID="HdSlipNo" Value="" runat="server" />
    <asp:HiddenField ID="HdSaveStatus" Value="" runat="server" />
    <asp:HiddenField ID="HdFromMaster" Value="" runat="server" />

<%--    <div id="divtest"  >
        <input type="button" value="show/hide" onclick="hideDVtest();" style="cursor: pointer;"/>
        <input type="button" id="testjaaa" value="test" onclick="alert('test');" style="cursor: pointer;"/>
    </div>--%>

</asp:Content>

