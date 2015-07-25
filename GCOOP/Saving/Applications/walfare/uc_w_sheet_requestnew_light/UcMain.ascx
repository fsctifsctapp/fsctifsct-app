<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcMain.ascx.cs" Inherits="Saving.Applications.walfare.uc_w_sheet_requestnew_light.UcMain" %>
<style type="text/css">
    .LabelNormal
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 0px;
    }
    .LabelBlue
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        background-color: rgb(211, 231, 255);
        text-align: right;
    }
    .ButtonShort
    {
        width: 29px;
    }
    .TextBoxSolid
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        height: 21px;
        width: 128px;
        background-color: rgb(255, 251, 240);
    }
    .TextBoxSolidShort
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        height: 21px;
        width: 95px;
        background-color: rgb(255, 251, 240);
    }
    .TextBoxSolidDis
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        height: 21px;
        width: 128px;
        background-color: rgb(231, 231, 231);
    }
    .TextBoxSolidLong
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        height: 21px;
        width: 391px;
        background-color: rgb(255, 251, 240);
    }
    .TextBoxSolidhalf
    {
        font-family: Tahoma;
        font-size: 13px;
        border: 1px solid #000000;
        height: 21px;
        width:100%;
        background-color: rgb(255, 251, 240);
    }
</style>
<script type="text/javascript">


    function addSlashes(input) {
        var v = input.value;
        if (v.match(/^\d{2}$/) !== null) {
            input.value = v + '/';
        } else if (v.match(/^\d{2}\/\d{2}$/) !== null) {
            input.value = v + '/';
        }
    }
      

       </script>
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table cellpadding="2" cellspacing="2" class="LabelNormal" width="740">
            <tr>
                <td class="LabelBlue" width="15%">
                    เลขที่คำขอ:
                </td>
                <td width="17%">
                    <asp:TextBox ID="deptrequest_docno" Text='<%# Bind("deptrequest_docno") %>' runat="server"
                        CssClass="TextBoxSolidDis" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="LabelBlue" width="17%">
                    ประเภท:
                </td>
                <td width="18%">
                    <asp:DropDownList ID="wftype_code" runat="server" CssClass="TextBoxSolid"  AutoPostBack="True" onChanged = "postwftypecode">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue" width="15%">
                    วันที่คุ้มครอง:
                </td>
                <td width="18%">
                    <asp:TextBox ID="deptopen_date" Text='<%# Bind("deptopen_date", "{0:dd/MM/yyyy}") %>'
                        runat="server" CssClass="TextBoxSolidDis" onkeyup="addSlashes(this);" MaxLength ="10" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    เลขสมาชิก สอ.:
                </td>
                <td width="23%">
                    <asp:TextBox ID="member_no" Text='<%# Bind("member_no") %>' runat="server" CssClass="TextBoxSolidShort" MaxLength="7"></asp:TextBox>
                    <input type="button" value="ค้น" class="ButtonShort" onclick="postMemberNo()" />
                </td>
                <td class="LabelBlue">
                    เลขฌาปนกิจ:
                </td>
                <td>
                    <asp:TextBox ID="deptaccount_no" Text='<%# Bind("deptaccount_no") %>' runat="server"
                        CssClass="TextBoxSolidDis" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="LabelBlue">
                    เพศ:
                </td>
                <td>
                    <asp:DropDownList ID="sex" runat="server" CssClass="TextBoxSolid" SelectedValue='<%# Bind("Sex") %>'>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="ชาย" Value="M"></asp:ListItem>
                        <asp:ListItem Text="หญิง" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    คำนำหน้า:
                </td>
                <td width="23%">
                    <asp:DropDownList ID="prename_code" runat="server" CssClass="TextBoxSolid">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue">
                    ชื่อ:
                </td>
                <td>
                    <asp:TextBox ID="deptaccount_name" Text='<%# Bind("deptaccount_name") %>' runat="server"
                        CssClass="TextBoxSolid"></asp:TextBox>
                </td>
                <td class="LabelBlue">
                    นามสกุล:
                </td>
                <td>
                    <asp:TextBox ID="deptaccount_sname" Text='<%# Bind("deptaccount_sname") %>' runat="server"
                        CssClass="TextBoxSolid"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    บัตรประชาชน:
                </td>
                <td width="23%">
                    <asp:TextBox ID="card_person" Text='<%# Bind("card_person") %>' runat="server" CssClass="TextBoxSolidShort"></asp:TextBox>
                    <input type="button" value="ค้น" class="ButtonShort" onclick="postCardPerson()" />
                    <asp:CheckBox ID="foreigner_flag" runat="server" />
                </td>
                <td class="LabelBlue">
                    วันเกิด:
                </td>
                <td>
                    <asp:TextBox ID="wfbirthday_date" Text='<%# Bind("wfbirthday_date", "{0:dd/MM/yyyy}") %>'
                        runat="server" onkeyup="addSlashes(this);" MaxLength ="10"  CssClass="TextBoxSolid"></asp:TextBox>
                </td>
                <td class="LabelBlue">
                    วันที่สมัคร:
                </td>
                <td>
                    <asp:TextBox ID="apply_date" Text='<%# Bind("apply_date", "{0:dd/MM/yyyy}") %>' onkeyup="addSlashes(this);" MaxLength ="10" runat="server"
                        CssClass="TextBoxSolid"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    ชื่อสามีหรือภรรยา :
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="mate_name" Text='<%# Bind("mate_name") %>' runat="server"
                        CssClass="TextBoxSolidhalf"></asp:TextBox>
                </td>
                <td class="LabelBlue">
                    ชื่อผู้จัดการศพที่ระบุไว้ :
                </td>
                <td colspan="2" ">
                    <asp:TextBox ID="manage_corpse_name" Text='<%# Bind("manage_corpse_name") %>' runat="server"
                        CssClass="TextBoxSolidhalf"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="LabelTransparent" colspan="2">
                    ที่อยู่ ที่สามารถติดต่อได้:
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    ที่อยู่:
                </td>
                <td colspan="5" width="23%">
                    <asp:TextBox ID="other_contact_address" Text='<%# Bind("other_contact_address") %>' runat="server"
                        CssClass="TextBoxSolidhalf"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td class="LabelBlue">
                    เขต/อำเภอ:
                </td>
                <td width="23%">
                    <asp:DropDownList ID="other_ampher_code" runat="server" CssClass="TextBoxSolid" AutoPostBack="True"
                        OnSelectedIndexChanged="other_ampher_code_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue">
                    จังหวัด:
                </td>
                <td>
                    <asp:DropDownList ID="other_province_code" runat="server" CssClass="TextBoxSolid" AutoPostBack="True"
                        OnSelectedIndexChanged="other_province_code_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue">
                    รหัสไปรษณีย์:
                </td>
                <td width="23%">
                    <asp:TextBox ID="other_postcode" Text='<%# Bind("other_postcode") %>' runat="server" CssClass="TextBoxSolid"></asp:TextBox>
                </td>                
            </tr>
            <tr>
                <td class="LabelTransparent" colspan="2">
                    ที่อยู่ตามทะเบียนบ้าน:
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    ที่อยู่:
                </td>
                <td colspan="5" width="23%">
                    <asp:TextBox ID="contact_address" Text='<%# Bind("contact_address") %>' runat="server"
                        CssClass="TextBoxSolidhalf"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td class="LabelBlue">
                    เขต/อำเภอ:
                </td>
                <td width="23%">
                    <asp:DropDownList ID="ampher_code" runat="server" CssClass="TextBoxSolid" AutoPostBack="True"
                        OnSelectedIndexChanged="ampher_code_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue">
                    จังหวัด:
                </td>
                <td>
                    <asp:DropDownList ID="province_code" runat="server" CssClass="TextBoxSolid" AutoPostBack="True"
                        OnSelectedIndexChanged="province_code_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="LabelBlue">
                    รหัสไปรษณีย์:
                </td>
                <td width="23%">
                    <asp:TextBox ID="postcode" Text='<%# Bind("postcode") %>' runat="server" CssClass="TextBoxSolid"></asp:TextBox>
                </td>                
            </tr>
            <tr>
            <td colspan="5">
            .
                </td>
            </tr>
            <tr>
                <td class="LabelBlue">
                    สถานะ:
                </td>
                <td>
                    <asp:TextBox ID="status" runat="server"  CssClass="TextBoxSolidDis" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="LabelBlue">
                    ศูนย์ฯ:
                </td>
                <td>
                    <asp:TextBox ID="membgroup_desc" Text="" runat="server" CssClass="TextBoxSolidDis"
                        ReadOnly="true"></asp:TextBox>
                </td>
                
                <td class="LabelBlue">
                    โทรศัพท์:
                </td>
                <td>
                    <asp:TextBox ID="hometel" Text='<%# Bind("hometel") %>' runat="server" CssClass="TextBoxSolid"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LabelBlue" >
                    หมายเหตุ:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="remark" Text='<%# Bind("remark") %>' runat="server" CssClass="TextBoxSolidhalf"></asp:TextBox>
                </td>
            </tr>
        </table>
            
        <asp:HiddenField ID="branch_id" Value='<%# Bind("branch_id") %>' runat="server" />
        <asp:HiddenField ID="membgroup_code" Value='<%# Bind("membgroup_code") %>' runat="server" />
        <asp:HiddenField ID="depttype_code" Value='<%# Bind("depttype_code") %>' runat="server" />
        <asp:HiddenField ID="member_type" Value='<%# Bind("member_type") %>' runat="server" />
        <asp:HiddenField ID="approve_status" Value='<%# Bind("approve_status") %>' runat="server" />
        <asp:HiddenField ID="entry_id" Value='<%# Bind("entry_id") %>' runat="server" />
        <asp:HiddenField ID="entry_date" Value='<%# Bind("entry_date") %>' runat="server" />
        <asp:HiddenField ID="carreer" Value='<%# Bind("carreer") %>' runat="server" />
        <asp:HiddenField ID="pay_status" Value='<%# Bind("pay_status") %>' runat="server" />
    </EditItemTemplate>
</asp:FormView>
