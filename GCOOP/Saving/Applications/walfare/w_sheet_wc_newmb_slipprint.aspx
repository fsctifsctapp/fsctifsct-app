<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_newmb_slipprint.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_newmb_slipprint" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=SelectCode %>
    <%=PeriodChange %>
    
    <%=runProcess%>
    <%=popupReport%>
    <script type="text/javascript">

        function OnDwCtrlBottonClicked(s, r, c) {
            switch (c) {
                case "b_search":
                    PeriodChange();
                    break;
                case "b_print":
                    runProcess();
                    break;
                case "b_sele_all":
                    Gcoop.GetEl("HdSelectCode").value = "all";
                    SelectCode();
                    break;
                case "b_unsele_all":
                    Gcoop.GetEl("HdSelectCode").value = "unsele";
                    SelectCode();
                    break;
            }
        }

        function OnDwCtrlChagned(s, r, c, v) {
            s.SetItem(r, c, v);
            switch (c) {
                case "deptacc_st":
                    s.SetItem(r, "deptacc_end", v);
                    break;
                case "member_nos":
                    s.SetItem(r, "member_noe", v);
                    break;
            }
            s.AcceptTaxt();
        }

        function SheetLoadComplete() {
            //setSelectedRowColor();
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptTaxt();
           // setSelectedRowColor();
        }

        function SheetLoadComplete() {
            //alert(Gcoop.GetEl("jjjj").value); 
            // alert(Gcoop.GetEl("HdOpenIFrame").value);
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
            }
           // ShowTabPage2(Gcoop.ParseInt(Gcoop.GetEl("HdTabIndex").value));
        }


   
//        function setSelectedRowColor() {
//            //alert("1");
//            var color = 'rgb(255,255,255)'; //สีขาว
//            var color1 = 'rgb(231,231,231)'; // สีเทา
//            var color2 = 'rgb(255,200,200)'; // สีชมพู
//            var color_ = 'rgb(255,255,128)'; // สีเหลือง
//            var selected = 0;
//            try {

//                for (var i = 1; i <= objDwMain.RowCount(); i++) {
//                   // alert(color_);
//                    var choose_flag = getObjString(objDwMain, i, "selec_status");
//                    alert(choose_flag);
//                    if (choose_flag == "1") {
//                        //alert(getObjFloat(objDwDetail, i, "itempayamt_net") + "," + getObjString(objDwDetail, i, "itempayamt_net"));                        
//                        $('#objDwMain_detail_' + (i - 1)).css("background-color", color_);
//                        $("input[name^='compute_1_" + (i - 1) + "']").css("background-color", color_);
//                        $("input[name^='wcdeptslipdet_deptslip_no_" + (i - 1) + "']").css("background-color", color_);
//                        $("input[name^='wcdeptslip_deptaccount_no_" + (i - 1) + "']").css("background-color", color_);
//                        $("input[name^='member_no_" + (i - 1) + "']").css("background-color", color_);
//                        $("input[name^='compute_2_" + (i - 1) + "']").css("background-color", color_);

//                       // selected++;
//                        //return;
//                    } else {
//                        $('#objDwMain_detail_' + (i - 1)).css("background-color", color);
//                        $("input[name^='compute_1_" + (i - 1) + "']").css("background-color", color);
//                        $("input[name^='wcdeptslipdet_deptslip_no_" + (i - 1) + "']").css("background-color", color);
//                        $("input[name^='wcdeptslip_deptaccount_no_" + (i - 1) + "']").css("background-color", color);
//                        $("input[name^='member_no_" + (i - 1) + "']").css("background-color", color);
//                        $("input[name^='compute_2_" + (i - 1) + "']").css("background-color", color);
//                    }
//                }
//                // alert(sumpay_amt);
//                //objDwDetail.SetItem(objDwDetail.RowCount(), "compute_2", sumpay_amt);
//                //$('#objDwDetail_summary').find("span[onclick*='compute_2']").text(addCommas(sumpay_amt, 2));
//                //$("span[onclick*='text_sum']").html("<b>จำนวน " + selected + " รายการ รวมเป็น </b>");
//                //alert($("span[onclick*='text_sum']").html());
//            }
//            catch (Error) { }
//        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DWCtrl" runat="server" DataWindowObject="d_wc_mbnew_slipprint_main"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_slip.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwCtrlChagned"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200" ClientEventButtonClicked="OnDwCtrlBottonClicked">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_wc_mbnew_slipprint_detail"
        LibraryList="~/DataWindow/walfare/w_sheet_wc_walfare_slip.pbl" ClientScriptable="True"
        AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" ClientEventItemChanged="OnDwMainItemChanged"
        AutoRestoreContext="False" ClientFormatting="True" TabIndex="200">
    </dw:WebDataWindowControl>
    <br />
    <asp:HiddenField ID="HdSelectCode" Value="" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" value="False"/>
</asp:Content>
