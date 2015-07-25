using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using CommonLibrary;

namespace Saving.Applications.walfare.uc_w_sheet_requestnew_light
{
    public partial class UcOther : System.Web.UI.UserControl
    {
        private string tableName = "WCREQCODEPOSIT".ToLower();

        public string deptrequest_docno;
        public string Deptrequest_docno
        {
            get { return deptrequest_docno; }
            set { deptrequest_docno = value; }
        }

        public decimal seq_no;
        public decimal Seq_no
        {
            get { return seq_no; }
            set { seq_no = value; }
        }

        public string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string codept_id;
        public string Codept_id
        {
            get { return codept_id; }
            set { codept_id = value; }
        }

        public string codept_addr;
        public string Codept_addr
        {
            get { return codept_addr; }
            set { codept_addr = value; }
        }

        public string branch_id;
        public string Branch_id
        {
            get { return branch_id; }
            set { branch_id = value; }
        }

        public decimal del_flag;
        public decimal Del_flag
        {
            get { return del_flag; }
            set { del_flag = value; }
        }

        public decimal foreigner_flag;
        public decimal Foreigner_flag
        {
            get { return foreigner_flag; }
            set { foreigner_flag = value; }
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
                else if (fType == "checkbox")
                {
                    //del_flag
                    
                    bool bb = ((CheckBox)item.FindControl(field)).Checked;
                    if (field == "del_flag")
                    {
                        return bb ? 1 : 0;
                    }
                    else
                    {
                        re = bb ? 1 : 0;
                    }
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

        private List<UcOther> getListUcOther(String branchId)
        {
            List<UcOther> list = new List<UcOther>();
            UcOther ot = null;
            for (int i = 1; i <= 15; i++)
            {
                ot = new UcOther();
                ot.seq_no = i;
                ot.branch_id = branchId;
                list.Add(ot);
            }
            return list;
        }

        public void InitUcOther(String branchId)
        {
            Repeater1.DataSource = getListUcOther(branchId);
            Repeater1.DataBind();
        }

        public DataTable GetDataTable()
        {
            return GetDataTable(this, Repeater1, tableName);
        }

        public void Retrieve(WebState state, String deptrequest_docno)
        {
            try
            {
                DataTable dt = WebUtil.Query("select * from " + tableName + " where deptrequest_docno = '" + deptrequest_docno + "' and branch_id='" + state.SsBranchId + "' order by seq_no");
                if (dt.Rows.Count > 0)
                {
                    int lastSeq = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["seq_no"]);
                    for (int i = lastSeq; i < 15; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["seq_no"] = i + 1;
                        dt.Rows.Add(dr);
                    }
                    int count = 1;
                    
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                    int foreigner_flag;
                    foreach (RepeaterItem rpItem in Repeater1.Items)
                    {
                        if (count <= lastSeq)
                        {
                            CheckBox ch = rpItem.FindControl("foreigner_flag") as CheckBox;
                            try { foreigner_flag = Convert.ToInt32(dt.Rows[count - 1]["foreigner_flag"]); }
                            catch { foreigner_flag = 0; }
                            if (foreigner_flag == 1)
                            {
                                ch.Checked = true;
                            }
                        }
                        count++;
                    }

                }
                else throw new Exception();
            }
            catch
            {
                InitUcOther(state.SsBranchId);
            }
        }
    }
}