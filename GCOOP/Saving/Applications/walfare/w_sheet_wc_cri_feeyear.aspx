<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_wc_cri_feeyear.aspx.cs" Inherits="Saving.Applications.walfare.w_sheet_wc_cri_feeyear" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=runProcess%>
    <%=popupReport%>
    <%=PostPost%>
     <%=jsbranch_id %>
    <%=jsbranch_desc %>
  
    <script type="text/javascript">
        function OndwcriteriaItemchange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();

            switch (c) {

                case "as_cstype":
                    PostPost();
                    break;

                case "branch_id":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_id();
                    break;
                case "branch_idd":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsbranch_desc();
                    break;
            }
        }
        function OnClickLinkNext() {
            objdw_criteria.AcceptText();
            runProcess();
        }
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="text-align: center">
        <tr>
            <td align="center">
                <asp:Label ID="ReportName" runat="server" Text="ชื่อรายงาน" Enabled="False" EnableTheming="False"
                    Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Large"
                    Font-Underline="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 500px">
                <dw:WebDataWindowControl ID="dw_criteria" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" ClientFormatting="True" ClientScriptable="True" DataWindowObject="u_cri_wc_fee_year"
                    LibraryList="~/DataWindow/criteria/criteria.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    ClientEventItemChanged="OndwcriteriaItemchange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 100px;">
                            
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left" style="width: 100px;">
                            <span style="cursor: pointer" onclick="OnClickLinkNext();">ออกรายงาน &gt;</span>
                            <%--<asp:LinkButton ID="LinkNext" runat="server" onclick="LinkNext_Click">ออกรายงาน &gt;</asp:LinkButton>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
</asp:Content>
