using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using CommonLibrary;
using System.Collections;
using DBAccess;

namespace Saving.Applications.walfare.uc_w_sheet_requestnew_light
{
    public partial class UcSlip : System.Web.UI.UserControl
    {
        private WebState state;
        private string tableName = "WCREQDETAIL".ToLower();

        public string deptitemtype_code;
        public string Deptitemtype_code
        {
            get { return deptitemtype_code; }
            set { deptitemtype_code = value; }
        }

        public decimal seq_no;
        public decimal Seq_no
        {
            get { return seq_no; }
            set { seq_no = value; }
        }

        public string deptitemtype_desc;
        public string Deptitemtype_desc
        {
            get { return deptitemtype_desc; }
            set { deptitemtype_desc = value; }
        }

        public decimal amt;
        public decimal Amt
        {
            get { return amt; }
            set { amt = value; }
        }

        public decimal status_pay;
        public decimal Status_pay
        {
            get { return status_pay; }
            set { status_pay = value; }
        }

        public string branch_id;
        public string Branch_id
        {
            get { return branch_id; }
            set { branch_id = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private DataTable GetDataTable(object p, Repeater re, String tbName)
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

            for (int r = 0; r < re.Items.Count; r++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < pi.Length; i++)
                {
                    try
                    {
                        dr[pi[i].Name] = PickDataOra(re.Items[r], pi[i].Name, pi[i].FieldType.Name);
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

        private List<UcSlip> getListUcSlip(String branchId, String wftypeCode, int payStatus)
        {
            List<UcSlip> list = new List<UcSlip>();
            UcSlip slip = null;

            if (wftypeCode != "")
            {
                WebState state = new WebState();
                String sql = "select * from wcmembertype where wftype_code='" + wftypeCode + "' and cs_type='" + state.SsCsType + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    slip = new UcSlip();
                    slip.seq_no = 1;
                    slip.deptitemtype_code = "FEE";
                    slip.deptitemtype_desc = "FEE - ค่าธรรมเนียมสมัครใหม่";
                    slip.status_pay = payStatus;
                    slip.amt = dt.GetDecimal("feeappl_amt");
                    slip.branch_id = branchId;
                    list.Add(slip);

                    slip = new UcSlip();
                    slip.seq_no = 2;
                    slip.deptitemtype_code = "WFY";
                    slip.deptitemtype_desc = "WFY - ค่าธรรมเนียมรายปี";
                    slip.status_pay = payStatus;
                    slip.amt = dt.GetDecimal("feeperyear_amt");
                    slip.branch_id = branchId;
                    list.Add(slip);

                    slip = new UcSlip();
                    slip.seq_no = 3;
                    slip.deptitemtype_code = "WPF";
                    slip.deptitemtype_desc = "WPF - เงินสงเคราะห์ศพล่วงหน้า";
                    slip.status_pay = payStatus;
                    slip.amt = dt.GetDecimal("paybffuture_amt");
                    slip.branch_id = branchId;
                    list.Add(slip);
                    
                }
            }
            return list;
        }

        public void InitUcSlip(String branchId, String wftypeCode, int payStatus)
        {
            Repeater1.DataSource = this.getListUcSlip(branchId, wftypeCode, payStatus);
            Repeater1.DataBind();
        }

        public DataTable GetDataTable()
        {
            return GetDataTable(this, Repeater1, tableName);
        }

        public void Retrieve(WebState state, String deptrequest_docno)
        {
            this.state = state;
            DataTable dt = WebUtil.Query("select wcreqdetail.* , wcreqdetail.deptitemtype_code || ' - ' || wcucfdeptitemtype.deptitemtype_desc as deptitemtype_desc from wcreqdetail left join wcucfdeptitemtype on wcreqdetail.deptitemtype_code = wcucfdeptitemtype.deptitemtype_code where deptrequest_docno = '" + deptrequest_docno + "' and branch_id='" + state.SsBranchId + "'");
            
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }

        public void CheckAdminStatus()
        {
            if (state == null)
            {
                state = new WebState();
            }
            if (state.SsUserType == 1 || state.SsUserType == 3)
            {
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    TextBox tbamt = ((TextBox)Repeater1.Items[i].FindControl("amt"));
                    tbamt.ReadOnly = false;
                }
            }
        }
    }
}