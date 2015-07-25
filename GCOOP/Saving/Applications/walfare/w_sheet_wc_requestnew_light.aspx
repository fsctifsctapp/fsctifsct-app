<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_requestnew_light.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_requestnew_light"
    Culture="th-TH" %>

<%@ Register Src="uc_w_sheet_requestnew_light/UcMain.ascx" TagName="UcMain" TagPrefix="uc1" %>
<%@ Register Src="uc_w_sheet_requestnew_light/UcSlip.ascx" TagName="UcSlip" TagPrefix="uc2" %>
<%@ Register Src="uc_w_sheet_requestnew_light/UcOther.ascx" TagName="UcOther" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postTest%>
    <%=postRequestDocNo%>
    <%=postMemberNo%>
    <%=postCardPerson%>
    <%=postSaveSheet %>
    <%=postwfmembertype %>
    <script type="text/javascript" language="javascript">
        function IsNumeric(number) {
            var num;
            try {
                number = number + "";
                if (number.substring(0, 1) == "-") {
                    num = number.substr(1);
                } else {
                    num = number;
                }
                var ch = num;
                var len, digit;
                if (ch == " ") {
                    return false;
                    len = 0;
                } else {
                    len = ch.length;
                }
                for (var i = 0; i < len; i++) {
                    digit = ch.charAt(i)
                    if (digit >= "0" && digit <= "9") {
                        //
                    } else {
                        return false;
                    }
                }
                return true;
            } catch (Err) { return false; }
        }

        function IsThDate(dt) {
            try {
                dt = Gcoop.Trim(dt + "");
                if (dt.indexOf("/") <= 0) {
                    return false;
                }
                var sp = new Array();
                sp = dt.split('/');
                if (!IsNumeric(sp[0]) || !IsNumeric(sp[1]) || !IsNumeric(sp[2])) {
                    return false;
                }
                dd = Gcoop.ParseInt(sp[0]);
                mm = Gcoop.ParseInt(sp[1]);
                yyyy = Gcoop.ParseInt(sp[2]) - 543;
                if (dd < 1 || dd > 31) {
                    return false;
                }
                if (mm == 2) {
                    if (dd > 29) return false;
                    if (yyyy % 4 != 0) {
                        if (dd > 28) return false;
                    }
                } else if (mm == 4 || mm == 6 || mm == 11) {
                    if (dd > 30) return false;
                }
            } catch (Err) {
                return false;
            }
            return true;
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("530", "620", "w_dlg_wc_walfare_reqedit.aspx");
        }

        function MenubarNew() {
            window.location = "";
        }

        function setReqdocNo(deptrequest_docno) {
            Gcoop.GetEl("HdRequestDocNo").value = deptrequest_docno;
            postRequestDocNo();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdExtraSearchMode").value != "") {
                var args = "?searchMode=" + Gcoop.GetEl("HdExtraSearchMode").value + "&searchCode=" + Gcoop.GetEl("HdExtraSearchCode").value;
                Gcoop.OpenIFrame("530", "620", "w_dlg_wc_walfare_reqedit.aspx", args);
            }
            if (Gcoop.GetEl("HdSaveStatus").value == "1") {
                if(confirm("ต้องการพิมพ์ใบเสร็จหรือไม่")){
                    //                    Gcoop.OpenDlg("1000", "1200", "w_dlg_wc_printslip.aspx", "?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value);
                    window.open("dlg/w_dlg_wc_printslip.aspx?deptslip_no=" + Gcoop.GetEl("HdSlipNo").value, "ใบเสร็จ", 'width=800,height=2000,scrollbars=1');
                }else{
                    return false;
                }
            }
            if (Gcoop.GetEl("HdForceSave").value == "1") {
                if (confirm(Gcoop.GetEl("HdMassage").value)) {
                    Gcoop.GetEl("HdForceSave").value = "2";
                    postSaveSheet();
                } else {
                    return;
                }
            }

        }

        function Validate() {
            //ctl00_ContentPlace_UcMain1_FormView1_wfbirthday_date
            //ctl00_ContentPlace_UcMain1_FormView1_apply_date
            var birthDay = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_wfbirthday_date");
            var regDate = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_apply_date");
            if (!IsThDate(birthDay.value)) {
                alert("กรุณากรอกค่าวันเกิดให้ถูกต้อง :รูปแบบวันที่ dd/mm/yyyy เช่น 01/01/2550");
                return false;
            }
            if (!IsThDate(regDate.value)) {
                alert("กรุณากรอกค่าวันที่ติดตั้งให้ถูกต้อง :รูปแบบวันที่ dd/mm/yyyy เช่น 01/01/2550");
                return false;
            }
            var deptaccount_name = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_deptaccount_name");
            var deptaccount_sname = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_deptaccount_sname");
            var card_person = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_card_person");
            var member_no = document.getElementById("ctl00_ContentPlace_UcMain1_FormView1_member_no");

            if (Gcoop.Trim(deptaccount_name.value) == null || Gcoop.Trim(deptaccount_name.value) == "") {
                alert("กรุณากรอกชื่อผู้สมัคร");
                return false;
            }
            if (Gcoop.Trim(deptaccount_sname.value) == null || Gcoop.Trim(deptaccount_sname.value) == "") {
                alert("กรุณากรอกนามสกุลผู้สมัคร");
                return false;
            }

//            var chknum = Gcoop.IsNum(Gcoop.Trim(card_person.value));
//            if (chknum == false) {
//                alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

//            if (Gcoop.Trim(card_person.value) == null || Gcoop.Trim(card_person.value) == "") {
//                alert("กรุณากรอกเลขบัตรประชาชนเป็นตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

//            var chkPS = checkID(Gcoop.Trim(card_person.value));
//            if (!chkPS) {
//                alert("กรุณากรอกเลขบัตรประชาชนให้ถูกต้องครบ 13 หลัก");
//                return false;
//            }

//            chknum = Gcoop.IsNum(member_no.value);
//            if (chknum == false) {
//                alert("กรุณากรอกเลขสมาชิกสหกรณ์ตัวเลข และไม่มีช่องว่าง");
//                return false;
//            }

            if (Gcoop.Trim(deptaccount_name.value) == null || Gcoop.Trim(deptaccount_name.value) == "") {
                alert("กรุณากรอกชื่อผู้สมัคร");
                return false;
            }
//            var codept_id = document.getElementById("ctl00_ContentPlace_UcOther1_Repeater1_ctl02_codept_id");
//            alert(codept_id.value);

            return confirm("ยืนยันการบันทึกข้อมูล");
        }

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


    $(function () {
        AutoSlash('input[name="ctl00$ContentPlace$UcMain1$FormView1$wfbirthday_date"]');
    });
      

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div align="left">
        <uc1:UcMain ID="UcMain1" runat="server" />   
        
             
        <p style="color:Red;">
             *** หมายเหตุเพิ่มเติม: หากผู้สมัครเป็นชาวต่างชาติให้ Check ถูก ที่ช่องข้างหลัง บัตรประชาชน
        </p>
        <br />
        <uc2:UcSlip ID="UcSlip1" runat="server" />
        <br />
        <uc3:UcOther ID="UcOther1" runat="server" />
        <br />
    </div>
    <asp:HiddenField ID="HdSaveType" Value="Insert" runat="server" />
    <asp:HiddenField ID="HdRequestDocNo" runat="server" />
    <asp:HiddenField ID="HdExtraSearchMode" Value="" runat="server" />
    <asp:HiddenField ID="HdExtraSearchCode" Value="" runat="server" />

    <asp:HiddenField ID="HdSlipNo" Value="" runat="server" />
    <asp:HiddenField ID="HdSaveStatus" Value="" runat="server" />

    <asp:HiddenField ID="HdForceSave" Value="" runat="server" />
    <asp:HiddenField ID="HdMassage" Value="" runat="server" />
</asp:Content>
