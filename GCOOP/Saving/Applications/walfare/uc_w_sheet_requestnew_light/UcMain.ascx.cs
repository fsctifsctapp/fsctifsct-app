using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.IO;
using CommonLibrary;
using DBAccess;
using System.Drawing;

namespace Saving.Applications.walfare.uc_w_sheet_requestnew_light
{
    public partial class UcMain : System.Web.UI.UserControl
    {
        private string tableName = "WCREQDEPOSIT".ToLower();
        private WebState state;

        public String deptrequest_docno;
        public String Deptrequest_docno
        {
            get { return deptrequest_docno; }
            set { deptrequest_docno = value; }
        }

        public String wftype_code;
        public String Wftype_code
        {
            get { return wftype_code; }
            set { wftype_code = value; }
        }

        public DateTime deptopen_date;
        public DateTime Deptopen_date
        {
            get { return deptopen_date; }
            set { deptopen_date = value; }
        }

        public String member_no;
        public String Member_no
        {
            get { return member_no; }
            set { member_no = value; }
        }

        public String deptaccount_no;
        public String Deptaccount_no
        {
            get { return deptaccount_no; }
            set { deptaccount_no = value; }
        }

        public String sex;
        public String Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public String prename_code;
        public String Prename_code
        {
            get { return prename_code; }
            set { prename_code = value; }
        }

        public String deptaccount_name;
        public String Deptaccount_name
        {
            get { return deptaccount_name; }
            set { deptaccount_name = value; }
        }

        public String deptaccount_sname;
        public String Deptaccount_sname
        {
            get { return deptaccount_sname; }
            set { deptaccount_sname = value; }
        }

        public String card_person;
        public String Card_person
        {
            get { return card_person; }
            set { card_person = value; }
        }

        public DateTime wfbirthday_date;
        public DateTime Wfbirthday_date
        {
            get { return wfbirthday_date; }
            set { wfbirthday_date = value; }
        }

        public String mate_name;
        public String Mate_name
        {
            get { return mate_name; }
            set { mate_name = value; }
        }

        public String manage_corpse_name;
        public String Manage_corpse_name
        {
            get { return manage_corpse_name; }
            set { manage_corpse_name = value; }
        }

        public String contact_address;
        public String Contact_address
        {
            get { return contact_address; }
            set { contact_address = value; }
        }

        public String other_contact_address;
        public String other_Contact_address
        {
            get { return other_contact_address; }
            set { other_contact_address = value; }
        }

        public String membgroup_code;
        public String Membgroup_code
        {
            get { return membgroup_code; }
            set { membgroup_code = value; }
        }

        public String membgroup_desc;
        public String Membgroup_desc
        {
            get { return membgroup_desc; }
            set { membgroup_desc = value; }
        }

        public String depttype_code;
        public String Depttype_code
        {
            get { return depttype_code; }
            set { depttype_code = value; }
        }

        public String member_type;
        public String Member_type
        {
            get { return member_type; }
            set { member_type = value; }
        }
        
        public String ampher_code;
        public String Ampher_code
        {
            get { return ampher_code; }
            set { ampher_code = value; }
        }

        public String other_ampher_code;
        public String Other_Ampher_code
        {
            get { return other_ampher_code; }
            set { other_ampher_code = value; }
        }
        
        public String province_code;
        public String Province_code
        {
            get { return province_code; }
            set { province_code = value; }
        }

        public String other_province_code;
        public String Other_Province_code
        {
            get { return other_province_code; }
            set { other_province_code = value; }
        }

        public DateTime apply_date;
        public DateTime Apply_date
        {
            get { return apply_date; }
            set { apply_date = value; }
        }

        public String postcode;
        public String Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }

        public String other_postcode;
        public String Other_Postcode
        {
            get { return other_postcode; }
            set { other_postcode = value; }
        }

        public String hometel;
        public String Hometel
        {
            get { return hometel; }
            set { hometel = value; }
        }

        public String remark;
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Decimal approve_status;
        public Decimal Approve_status
        {
            get { return approve_status; }
            set { approve_status = value; }
        }

        public String entry_id;
        public String Entry_id
        {
            get { return entry_id; }
            set { entry_id = value; }
        }

        public DateTime entry_date;
        public DateTime Entry_date
        {
            get { return entry_date; }
            set { entry_date = value; }
        }

        public Decimal carreer;
        public Decimal Carreer
        {
            get { return carreer; }
            set { carreer = value; }
        }

        public Decimal pay_status;
        public Decimal Pay_status
        {
            get { return pay_status; }
            set { pay_status = value; }
        }

        public String branch_id;
        public String Branch_id
        {
            get { return branch_id; }
            set { branch_id = value; }
        }

        public decimal foreigner_flag;
        public decimal Foreigner_flag
        {
            get { return foreigner_flag; }
            set { foreigner_flag = value; }
        }
        //public String cs_type;
        //public String Cs_type
        //{
        //    get { return cs_type; }
        //    set { Cs_type = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private List<UcMain> getListUcMain()
        {
            List<UcMain> list = new List<UcMain>();
            UcMain main = new UcMain();
            main.deptrequest_docno = "Auto";
            main.deptaccount_no = "Auto";
            main.apply_date = DateTime.Today;
            main.branch_id = state.SsBranchId;
            main.membgroup_code = state.SsBranchId;
            main.depttype_code = "01";
            main.member_type = "01";
            main.approve_status = 8;
            main.entry_id = state.SsUsername;
            main.entry_date = DateTime.Today;
            main.carreer = 0;
            main.pay_status = 0;
            main.province_code = state.SsProvinceCode;
            main.ampher_code = state.SsDistrictCode;
            main.postcode = state.SsPostCode;
            main.wftype_code = "01";
            main.wfbirthday_date = DateTime.Today;
            //main.cs_type = state.SsCsType;

            //Sdt sdt = WebUtil.QuerySdt("select * from cmucfcoopbranch where coopbranch_id='" + state.SsBranchId + "'");
            //if (sdt.Next())
            //{
            //    main.membgroup_desc = sdt.GetString("coopbranch_desc");
            //}
            try
            {
                String sql2 = "select * from wcmembertype where cs_type = '" + state.SsCsType + "' and wftype_code = '" + main.wftype_code.ToString() + "' and used_status = 1";
                Sdt sdt2 = WebUtil.QuerySdt(sql2);
                if (sdt2.Next())
                {
                    main.deptopen_date = sdt2.GetDate("deptopen_date");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            list.Add(main);
            return list;
        }

        private DataTable GetDataTable(object p, FormView re, String tbName)
        {
            DataTable dt = new DataTable(tbName);
            Type t = p.GetType();
            FieldInfo[] pi = t.GetFields();
            foreach (FieldInfo prop in pi)
            {
                DataColumn col = new DataColumn();
                col.DataType = prop.FieldType;
                col.ColumnName = prop.Name;
                dt.Columns.Add(col);
            }

            for (int r = 0; r < 1; r++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < pi.Length; i++)
                {
                    try
                    {
                        dr[pi[i].Name] = PickDataOra(re, pi[i].Name, pi[i].FieldType.Name);
                    }
                    catch { }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private object PickDataOra(Control item, String field, String type)
        {
            object re = null;
            try
            {
                if (field == "foreigner_flag")
                {
                    re = null;
                }
                type = type.ToLower();
                String fType = item.FindControl(field).GetType().Name.ToLower();
                String v = "";
                if (fType == "dropdownlist")
                {
                    v = ((DropDownList)item.FindControl(field)).SelectedValue.Trim();
                }
                else if (fType == "hiddenfield")
                {
                    v = ((HiddenField)item.FindControl(field)).Value.Trim();
                }
                else if (fType == "checkbox")
                {
                    //del_flag
                    bool bb = ((CheckBox)item.FindControl(field)).Checked;
                    re = bb ? 1 : 0;

                }
                else
                {
                    v = ((TextBox)item.FindControl(field)).Text.Trim();
                }
                if (v == "") return re;
                if (type == "string")
                {
                    re = v;
                }
                else if (type == "decimal" || type == "int" || type == "int32" || type == "int48")
                {
                    re = Convert.ToDecimal(v);//.ToString();
                }                
                else if (type == "datetime")
                {

                    DateTime dt;
                    try
                    {
                        if (v.Length > 10) throw new Exception();
                        dt = DateTime.ParseExact(v, "dd/MM/yyyy", WebUtil.TH);
                    }
                    catch
                    {
                        dt = DateTime.ParseExact(v, "dd/MM/yyyy HH:mm:ss", WebUtil.TH);
                    }
                    re = dt;
                }
            }
            catch { return null; }
            return re;
        }

        private String GetSqlInsertOra(object p, Control fv, String tbName)
        {
            String insert = "INSERT INTO " + tbName + "(";
            DataTable dt = new DataTable(tbName);
            Type t = p.GetType();
            FieldInfo[] pi = t.GetFields();
            for (int i = 0; i < pi.Length; i++)
            {
                if (i == 0)
                {
                    insert += "\"" + pi[i].Name.ToUpper() + "\"";
                }
                else
                {
                    insert += ", \"" + pi[i].Name.ToUpper() + "\"";
                }
            }
            insert += ") values (";
            for (int i = 0; i < pi.Length; i++)
            {
                if (i == 0)
                {
                    insert += PickDataOra2(FormView1, pi[i].Name, pi[i].FieldType.Name);// "\"" + pi[i].Name.ToUpper() + "\"";
                }
                else
                {
                    insert += ", " + PickDataOra2(FormView1, pi[i].Name, pi[i].FieldType.Name);
                }
            }
            insert += ")";
            return insert;
        }

        private String PickDataOra2(Control fv, String field, String type)
        {
            String re = "null";
            try
            {
                type = type.ToLower();
                String fType = fv.FindControl(field).GetType().Name.ToLower();
                String v = "";
                if (fType == "dropdownlist")
                {
                    v = ((DropDownList)fv.FindControl(field)).SelectedValue.Trim();
                }
                else if (fType == "hiddenfield")
                {
                    v = ((HiddenField)fv.FindControl(field)).Value.Trim();
                }
                else
                {
                    v = ((TextBox)fv.FindControl(field)).Text.Trim();
                }

                if (v == "") return re;
                if (type == "string")
                {
                    re = "'" + v + "'";
                }
                else if (type == "decimal" || type == "int" || type == "int32" || type == "int48")
                {
                    re = v.ToString();
                }
                else if (type == "datetime")
                {
                    DateTime dt;
                    try
                    {
                        if (v.Length > 10) throw new Exception();
                        dt = DateTime.ParseExact(v, "dd/MM/yyyy", WebUtil.TH);
                    }
                    catch
                    {
                        dt = DateTime.ParseExact(v, "dd/MM/yyyy HH:mm:ss", WebUtil.TH);
                    }
                    re = "to_date('" + dt.ToString("yyyy-MM-d H:m:s", WebUtil.EN) + "', 'yyyy-mm-dd hh24:mi:ss')";
                }
                else if (fType == "checkbox")
                {
                    //del_flag
                    bool bb = ((CheckBox)fv.FindControl(field)).Checked;
                    re = Convert.ToString(bb ? 1 : 0);

                }
            }
            catch { }
            return re;
        }

        public void InitUcMain(WebState state)
        {
            this.state = state;
            //'province_code'
            FormView1.DataSource = getListUcMain();
            FormView1.DataBind();

            Sdt sdt = WebUtil.QuerySdt("select * from cmucfcoopbranch where coopbranch_id='" + state.SsBranchId + "'");
            if (sdt.Next())
            {
                //main.membgroup_desc = sdt.GetString("coopbranch_desc");
                ((TextBox)FormView1.FindControl("membgroup_desc")).Text = sdt.GetString("coopbranch_desc");
            }

            DataTable dt1 = WebUtil.Query("select prename_code, prename_desc from mbucfprename order by prename_code");
            DropDownList dd = (DropDownList)FormView1.FindControl("prename_code");
            dd.DataSource = dt1;
            dd.DataValueField = "prename_code";
            dd.DataTextField = "prename_desc";
            dd.DataBind();

            DataTable dt2 = WebUtil.Query("select province_code, province_desc from mbucfprovince order by province_desc");
            DropDownList ddProvince = (DropDownList)FormView1.FindControl("province_code");
            ddProvince.DataSource = dt2;
            ddProvince.DataValueField = "province_code";
            ddProvince.DataTextField = "province_desc";
            ddProvince.DataBind();
            try
            {
                ddProvince.SelectedValue = state.SsProvinceCode;
            }
            catch { }

            DataTable dt2_other = WebUtil.Query("select province_code, province_desc from mbucfprovince order by province_desc");
            DropDownList dd_other = (DropDownList)FormView1.FindControl("other_province_code");
            dd_other.DataSource = dt2_other;
            dd_other.DataValueField = "province_code";
            dd_other.DataTextField = "province_desc";
            dd_other.DataBind();
            try
            {
                dd_other.SelectedValue = state.SsProvinceCode;
            }
            catch { }

            DataTable dt3 = WebUtil.Query("select wftype_code, wcmembertype_desc from wcmembertype where cs_type = '"  + state.SsCsType + "' and used_status = 1 order by wftype_code");
            DropDownList ddmembtype = (DropDownList)FormView1.FindControl("wftype_code");
            ddmembtype.DataSource = dt3;
            ddmembtype.DataValueField = "wftype_code";
            ddmembtype.DataTextField = "wcmembertype_desc";
            ddmembtype.DataBind();

           

            //if (state.SsProvinceCode != "")
            //{
            //    FilterAmpher();
            //    try
            //    {
            //        DropDownList dd3 = (DropDownList)FormView1.FindControl("ampher_code");
            //        dd3.SelectedValue = state.SsDistrictCode;
            //    }
            //    catch { }

            //    other_FilterAmpher();
            //    try {
            //        DropDownList dd3_other = (DropDownList)FormView1.FindControl("other_ampher_code");
            //        dd3_other.SelectedValue = state.SsDistrictCode; 
            //    }
            //    catch { }
            //}
        }

        public void Retrieve(WebState state, String deptrequest_docno)
        {
            this.state = state;
            DataTable dt = WebUtil.Query("select * from " + tableName + " where deptrequest_docno = '" + deptrequest_docno + "' and branch_id = '" + state.SsBranchId + "'");
            FormView1.DataSource = dt;
            FormView1.DataBind();

            Sdt sdt = WebUtil.QuerySdt("select * from cmucfcoopbranch where coopbranch_id='" + state.SsBranchId + "'");
            if (sdt.Next())
            {
                //main.membgroup_desc = sdt.GetString("coopbranch_desc");
                ((TextBox)FormView1.FindControl("membgroup_desc")).Text = sdt.GetString("coopbranch_desc");
            }
            int foreigner_flag;

            CheckBox ch = (CheckBox)FormView1.FindControl("foreigner_flag");
            try { foreigner_flag = Convert.ToInt32(dt.Rows[0]["foreigner_flag"]); }
            catch { foreigner_flag = 0; }
            if (foreigner_flag == 1)
            {
                ch.Checked = true;
            }

            DataTable dt1 = WebUtil.Query("select prename_code, prename_desc from mbucfprename order by prename_code");
            DropDownList dd = (DropDownList)FormView1.FindControl("prename_code");
            dd.DataSource = dt1;
            dd.DataValueField = "prename_code";
            dd.DataTextField = "prename_desc";
            dd.DataBind();
            dd.SelectedValue = dt.Rows[0]["prename_code"].ToString();

            DataTable dt2 = WebUtil.Query("select province_code, province_desc from mbucfprovince order by province_desc");
            DropDownList dd2 = (DropDownList)FormView1.FindControl("province_code");
            dd2.DataSource = dt2;
            dd2.DataValueField = "province_code";
            dd2.DataTextField = "province_desc";
            dd2.DataBind();
            String values = dt.Rows[0]["province_code"].ToString();
            dd2.SelectedValue = values.Trim();

            DataTable dt2_other = WebUtil.Query("select province_code, province_desc from mbucfprovince order by province_desc");
            DropDownList dd2_other = (DropDownList)FormView1.FindControl("other_province_code");
            dd2_other.DataSource = dt2_other;
            dd2_other.DataValueField = "province_code";
            dd2_other.DataTextField = "province_desc";
            dd2_other.DataBind();
            String values_other = dt.Rows[0]["other_province_code"].ToString();
            dd2_other.SelectedValue = values_other.Trim();

            DataTable dt3 = WebUtil.Query("select wftype_code, wcmembertype_desc from wcmembertype where cs_type = '" + state.SsCsType + "' and used_status = 1 order by wftype_code");
            DropDownList ddmembtype = (DropDownList)FormView1.FindControl("wftype_code");
            ddmembtype.DataSource = dt3;
            ddmembtype.DataValueField = "wftype_code";
            ddmembtype.DataTextField = "wcmembertype_desc";
            ddmembtype.DataBind();
            String values_membtype = dt.Rows[0]["wftype_code"].ToString();
            ddmembtype.SelectedValue = values_membtype;

            //ampher_code
            FilterAmpher();
            try
            {
                DropDownList dd4 = (DropDownList)FormView1.FindControl("ampher_code");
                dd4.SelectedValue = dt.Rows[0]["ampher_code"].ToString();
                GetPostcode();
            }
            catch { }

            other_FilterAmpher();
            try {
                DropDownList dd4_other = (DropDownList)FormView1.FindControl("other_ampher_code");
                dd4_other.SelectedValue = dt.Rows[0]["other_ampher_code"].ToString();
                other_GetPostcode();
            }
            catch { }
        }

        public void FilterAmpher()
        {
            try
            {
                String province = ((DropDownList)FormView1.FindControl("province_code")).SelectedValue;
                if (province != "" && province != "ฮฮ")
                {
                    String p = "select * from mbucfdistrict where province_code = '" + province + "' order by district_desc";
                    DataTable dt = WebUtil.QuerySdt(p);
                    DropDownList dd = ((DropDownList)FormView1.FindControl("ampher_code"));
                    dd.DataSource = dt;
                    dd.DataValueField = "district_code";
                    dd.DataTextField = "district_desc";
                    dd.DataBind();
                }
            }
            catch { }
        }

        public void other_FilterAmpher()
        {
            try
            {
                String province = ((DropDownList)FormView1.FindControl("other_province_code")).SelectedValue;
                if (province != "" && province != "ฮฮ")
                {
                    String p = "select * from mbucfdistrict where province_code = '" + province + "' order by district_desc";
                    DataTable dt = WebUtil.QuerySdt(p);
                    DropDownList dd = ((DropDownList)FormView1.FindControl("other_ampher_code"));
                    dd.DataSource = dt;
                    dd.DataValueField = "district_code";
                    dd.DataTextField = "district_desc";
                    dd.DataBind();
                }
            }
            catch { }
        }

        public DataTable GetDataTable()
        {
            return GetDataTable(this, FormView1, tableName);
        }

        public String GetMemberNo()
        {
            return ((TextBox)FormView1.FindControl("member_no")).Text.Trim();
        }

        public String GetCardPerson()
        {
            return ((TextBox)FormView1.FindControl("card_person")).Text.Trim();
        }

        protected void province_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAmpher();
        }

        protected void other_province_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            other_FilterAmpher();
        }

        protected void ampher_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPostcode();
        }

        protected void other_ampher_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            other_GetPostcode();
        }

        private void GetPostcode()
        {
            try
            {
                String ampher_code = ((DropDownList)FormView1.FindControl("ampher_code")).SelectedValue;
                String SQLpostcode = "select postcode from mbucfdistrict where district_code = '" + ampher_code + "'";
                Sdt dtpostcode = WebUtil.QuerySdt(SQLpostcode);
                if (dtpostcode.Next())
                {
                    ((TextBox)FormView1.FindControl("postcode")).Text = dtpostcode.GetString("postcode");
                }
            }
            catch
            {
            }
        }

        private void other_GetPostcode()
        {
            try
            {
                String ampher_code = ((DropDownList)FormView1.FindControl("other_ampher_code")).SelectedValue;
                String SQLpostcode = "select postcode from mbucfdistrict where district_code = '" + ampher_code + "'";
                Sdt dtpostcode = WebUtil.QuerySdt(SQLpostcode);
                if (dtpostcode.Next())
                {
                    ((TextBox)FormView1.FindControl("other_postcode")).Text = dtpostcode.GetString("postcode");
                }
            }
            catch
            {
            }
        }

        public void wftype_code_SelectedIndexChanged()
        {
            WebState state = new WebState();
            UcMain main = new UcMain();
            string wftype_code = ((DropDownList)FormView1.FindControl("wftype_code")).Text.Trim();
            string branch_id = ((HiddenField)FormView1.FindControl("branch_id")).Value;
            //string cs_type = ((HiddenField)FormView1.FindControl("cs_type")).Value;
            try
            {
                String sql2 = "select * from wcmembertype where cs_type = '" + state.SsCsType + "' and wftype_code = '" + wftype_code + "' and used_status = 1";
                Sdt sdt2 = WebUtil.QuerySdt(sql2);
                if (sdt2.Next())
                {
                    ((TextBox)FormView1.FindControl("deptopen_date")).Text = sdt2.GetDate("deptopen_date").ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //UcSlip slip = new UcSlip();
            //slip.changeMemType(branch_id, wftype_code, state.SsCsType);

        }

        public void CheckAdminStatus()
        {
            if (state == null)
            {
                state = new WebState();
            }
            if (state.SsUserType == 1 || state.SsUserType == 3)
            {
                TextBox tbdeptopen_date = ((TextBox)FormView1.FindControl("deptopen_date"));
                tbdeptopen_date.ReadOnly = false;
                tbdeptopen_date.BackColor = Color.White;
            }
            //FormView1.f
        }
    }
}