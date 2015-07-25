using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sybase.DataWindow;
using System.Globalization;
using DBAccess;
using System.Data;

namespace GcoopServiceCs
{
    public class DwHandle
    {
        private DataStore dwMain;
        private String synTax;
        private String[] columnName;
        private String[] columnType;
        private String[] columnDBName;
        private bool[] columnKey;

        public DwHandle(DataStore dwMain)
        {
            this.ConStructorEnding(dwMain);
        }

        public DwHandle(String xmlData, String pbl, String dataObjectName)
        {
            DataStore dwMain = new DataStore(pbl, dataObjectName);
            dwMain.ImportString(xmlData, FileSaveAsType.Xml);
            ConStructorEnding(dwMain);
        }

        public void ConStructorEnding(DataStore dwMain)
        {
            this.dwMain = dwMain;
            this.synTax = dwMain.Syntax;
            columnName = new string[dwMain.ColumnCount];
            columnDBName = new string[dwMain.ColumnCount];
            columnType = new string[dwMain.ColumnCount];
            columnKey = new bool[dwMain.ColumnCount];
            for (int i = 0; i < dwMain.ColumnCount; i++)
            {
                columnName[i] = dwMain.Describe("#" + (i + 1) + ".Name");
                columnDBName[i] = dwMain.Describe(columnName[i] + ".DBName").ToLower();
                columnType[i] = dwMain.Describe(columnName[i] + ".ColType").ToLower();
                try
                {
                    columnKey[i] = dwMain.Describe(columnName[i] + ".Key").ToLower() == "yes";
                }
                catch
                {
                    columnKey[i] = false;
                }
                if (columnType[i].IndexOf("(") > 0)
                {
                    columnType[i] = columnType[i].Substring(0, columnType[i].IndexOf("("));
                }
            }
        }

        public String SqlInsertSyntax(String tableName, int row)
        {
            tableName = tableName.ToLower();
            String resu = "insert into " + tableName + "(";
            StringBuilder sb = new StringBuilder(resu);
            String[] dbName = new String[2];
            bool isFirst = true;
            for (int i = 0; i < columnName.Length; i++)
            {
                dbName = columnDBName[i].Split('.');
                if (dbName[0].ToLower() == tableName)
                {
                    if (!isFirst)
                    {
                        sb.Append(", ");
                    }
                    isFirst = false;
                    sb.Append("\"" + columnName[i].ToUpper() + "\"");
                }
            }
            sb.Append(")values(");
            isFirst = true;
            for (int i = 0; i < columnName.Length; i++)
            {
                dbName = columnDBName[i].Split('.');
                if (dbName[0].ToLower() == tableName)
                {
                    if (!isFirst)
                    {
                        sb.Append(", ");
                    }
                    isFirst = false;
                    sb.Append(pickData(row, i));
                }
            }
            sb.Append(")");
            resu = sb.ToString();
            return resu;
        }

        public String SqlUpdateSyntax(String tableName, int row)
        {
            String resu = "";
            String where = "\nwhere ";
            tableName = tableName.ToLower();
            bool isFirst = true;
            for (int i = 0; i < columnName.Length; i++)
            {
                if (columnKey[i])
                {
                    if (!isFirst) where += "and ";
                    isFirst = false;
                    where += "\"" + columnName[i].ToUpper() +"\"=" + pickData(row, i) + " ";
                }
            }
            where = where == "\nwhere " ? "" : where;
            StringBuilder sb = new StringBuilder("update " + tableName + " set ");
            isFirst = true;
            string[] dbName = new string[2];
            for (int i = 0; i < columnName.Length; i++)
            {
                dbName = columnDBName[i].Split('.');
                if (dbName[0].ToLower() == tableName && !columnKey[i])
                {
                    if (!isFirst)
                    {
                        sb.Append(", ");
                    }
                    isFirst = false;
                    sb.Append("\n\t\"" + columnName[i].ToUpper() + "\"=" + pickData(row, i));
                }
            }
            sb.Append(where);
            resu = sb.ToString();
            return resu;
        }

        private String pickData(int row, int colIndex)
        {
            string colType = columnType[colIndex];
            String resu = "null";
            if (colType == "char")
            {
                try
                {
                    resu = "'" + dwMain.GetItemString(row, columnName[colIndex]) + "'";
                }
                catch { }
            }
            else if (colType == "decimal" || colType == "long" || colType == "number" || colType == "float" || colType == "double")
            {
                try
                {
                    resu = dwMain.GetItemDecimal(row, columnName[colIndex]).ToString();
                }
                catch { }
            }
            else if (colType == "datetime")
            {
                //dataWindow.SetItemDateTime(r + 1, cName[c], DateTime.ParseExact(dt.Rows[r][c].ToString(), "yyyy-MM-dd HH:mm:ss", WebUtil.EN));
                try
                {
                    CultureInfo en = new CultureInfo("en-US");
                    resu = "to_date('" + dwMain.GetItemDateTime(row, columnName[colIndex]).ToString("yyyy-MM-d H:m:s", en) + "', 'yyyy-mm-dd hh24:mi:ss')";
                }
                catch { }
            }
            return resu;
        }

        public int UpdateData(String connectionString, String table, int[] rows)
        {
            int resu = -1;
            Sta ta = new Sta(connectionString);
            ta.Transection();
            try
            {
                int ii = 0;
                for (int i = 0; i < rows.Length; i++)
                {
                    ta.Exe(this.SqlUpdateSyntax(table, rows[i]));
                    ii++;
                }
                ta.Commit();
                ta.Close();
                resu = ii;
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }

        public int InsertData(String connectionString, String table, int[] rows)
        {
            int resu = -1;
            Sta ta = new Sta(connectionString);
            ta.Transection();
            try
            {
                int ii = 0;
                for (int i = 0; i < rows.Length; i++)
                {
                    ta.Exe(this.SqlInsertSyntax(table, rows[i]));
                    ii++;
                }
                ta.Commit();
                ta.Close();
                resu = ii;
            }
            catch (Exception ex)
            {
                try
                {
                    ta.RollBack();
                }
                catch { }
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }

        public static void ImportData(DataTable dataTable, IDataStore dataWindow)
        {
            if (dataTable == null) return;
            dataWindow.Reset();
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                dataWindow.InsertRow(0);
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    try
                    {
                        String cType = dataTable.Columns[c].DataType.Name.ToLower();
                        if (cType == "datetime")
                        {
                            dataWindow.SetItemDateTime(r + 1, dataTable.Columns[c].ColumnName, Convert.ToDateTime(dataTable.Rows[r][c]));
                        }
                        else if (cType == "date")
                        {
                            dataWindow.SetItemDateTime(r + 1, dataTable.Columns[c].ColumnName, Convert.ToDateTime(dataTable.Rows[r][c]));
                        }
                        else if (cType == "int" || cType == "int16" || cType == "int32" || cType == "int64" || cType == "short" || cType == "dec" || cType == "decimal" || cType == "long" || cType == "float" || cType == "double")
                        {
                            dataWindow.SetItemDecimal(r + 1, dataTable.Columns[c].ColumnName, Convert.ToDecimal(dataTable.Rows[r][c]));
                        }
                        else
                        {
                            dataWindow.SetItemString(r + 1, dataTable.Columns[c].ColumnName, Convert.ToString(dataTable.Rows[r][c]));
                        }
                    }
                    catch { }
                }
            }
        }
    
    }
}