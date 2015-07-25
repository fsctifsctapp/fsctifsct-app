using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Sybase.DataWindow.Web;
using Sybase.DataWindow;
using CommonLibrary.WsCommon;

namespace CommonLibrary
{
    public class DwUtil
    {
        //Utility ที่มีการเชื่อมต่อกับ WsCommon
        public static void RetrieveDDDW(WebDataWindowControl dwObj, String columnName, String libraryList, params object[] args)
        {
            Common comSrv = WsUtil.Common;
            WebState state = new WebState();
            String dwobjectName = dwObj.Describe(columnName + ".dddw.name");
            String strDS;
            try
            {
                if (libraryList.ToLower() == "criteria.pbl")
                {
                    strDS = comSrv.GetXmlDataStore(state.SsWsPass, libraryList.Split('.').GetValue(0).ToString(), libraryList, dwobjectName, args);
                }
                else
                {
                    strDS = comSrv.GetXmlDataStore(state.SsWsPass, state.SsApplication, libraryList, dwobjectName, args);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("ERROR:NOROWXML") >= 0)
                {
                    DataWindowChild dwChild1 = dwObj.GetChild(columnName);
                    dwChild1.Reset();
                    comSrv.Dispose();
                    return;
                }
                strDS = "";
            }
            DataWindowChild dwChild = dwObj.GetChild(columnName);
            DwUtil.ImportData(strDS, dwChild, null, FileSaveAsType.Xml);
            comSrv.Dispose();
        }

        public static void RetrieveDataWindow(WebDataWindowControl dwObj, String libraryList, DwThDate dwThDate, params object[] args)
        {
            Common comSrv = WsUtil.Common;
            WebState state = new WebState();
            String dwobjectName = dwObj.DataWindowObject;
            String strDS = "";
            try
            {
                strDS = comSrv.GetXmlDataStore(state.SsWsPass, state.SsApplication, libraryList, dwobjectName, args);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("ERROR:NOROWXML") >= 0)
                {
                    dwObj.Reset();
                    return;
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(strDS))
                {
                    dwObj.Reset();
                    dwObj.ImportString(strDS, FileSaveAsType.Xml);
                    if (dwObj.RowCount > 0 && dwThDate != null)
                    {
                        dwThDate.Eng2ThaiAllRow();
                    }
                }
            }
            catch
            {
                DwUtil.ImportData(strDS, dwObj, dwThDate, FileSaveAsType.Xml);
            }
            comSrv.Dispose();
        }

        public static int InsertDataWindow(IDataStore dwStore, String libraryList, String tableName)
        {
            int[] rows = new int[dwStore.RowCount];
            for (int i = 1; i <= dwStore.RowCount; i++)
            {
                rows[i - 1] = i;
            }
            return DwUtil.InsertDataWindow(dwStore, libraryList, tableName, rows);
        }

        public static int InsertDataWindow(IDataStore dwStore, String libraryList, String tableName, int[] rows)
        {
            WebState state = new WebState();
            Common cm = WsUtil.Common;
            String xml = dwStore.Describe("DataWindow.Data.XML");
            return cm.InsertDataWindow(state.SsWsPass, xml, libraryList, tableName, dwStore.DataWindowObject, state.SsApplication, rows);
        }

        public static int UpdateDateWindow(IDataStore dwStore, String libraryList, String tableName)
        {
            int[] rows = new int[dwStore.RowCount];
            for (int i = 1; i <= dwStore.RowCount; i++)
            {
                rows[i - 1] = i;
            }
            return DwUtil.UpdateDateWindow(dwStore, libraryList, tableName, rows);
        }

        public static int UpdateDateWindow(IDataStore dwStore, String libraryList, String tableName, int[] rows)
        {
            WebState state = new WebState();
            Common cm = WsUtil.Common;
            String xml = dwStore.Describe("DataWindow.Data.XML");
            return cm.UpdateDataWindow(state.SsWsPass, xml, libraryList, tableName, dwStore.DataWindowObject, state.SsApplication, rows);
        }

        public static void ImportData(String sqlSyntax, IDataWindowBase dataWindow, DwThDate dwThDate)
        {
            DataTable dt = WebUtil.Query(sqlSyntax);
            ImportData(dt, dataWindow, dwThDate);
        }

        public static void ImportData(DataTable dataTable, IDataWindowBase dataWindow, DwThDate dwThDate)
        {
            if (dataTable == null) return;
            dataWindow.Reset();
            System.IO.StringWriter strWriter = new System.IO.StringWriter();
            dataTable.WriteXml(strWriter);
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
            try
            {
                if (dwThDate != null)
                {
                    dwThDate.Eng2ThaiAllRow();
                }
                else
                {
                    dataWindow.ResetUpdateStatus();
                }
            }
            catch { }
        }

        public static void ImportData(String xmlString, IDataWindowBase dataWindow, DwThDate dwThDate, FileSaveAsType dataImportType)
        {
            if (dataImportType != FileSaveAsType.Xml)
                return;
            if (string.IsNullOrEmpty(xmlString))
            {
                dataWindow.Reset();
                return;
            }
            if (!WebUtil.IsXML(xmlString))
            {
                throw new Exception("ข้อมูลรูปแบบ XML ไม่ถูกต้อง");
            }
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            System.IO.StringReader sReader = new System.IO.StringReader(xmlString);
            ds.ReadXml(sReader);
            dt = ds.Tables[0];

            if (dt.Rows.Count < 1)
                throw new Exception("ไม่มีข้อมูลใน XML");
            String[] cType = new String[dataWindow.ColumnCount];
            String[] cName = new String[dataWindow.ColumnCount];
            dataWindow.Reset();
            //วนเพื่อหาชื่อ column และ type
            for (int i = 0; i < dataWindow.ColumnCount; i++)
            {
                cName[i] = dataWindow.Describe("#" + (i + 1) + ".Name");
                cType[i] = dataWindow.Describe(cName[i] + ".ColType").ToLower();
                if (cType[i].IndexOf("(") > 0)
                {
                    cType[i] = cType[i].Substring(0, cType[i].IndexOf("("));
                }
            }
            //วนเพื่อยิงค่าเข้าไปทีละค่า
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                dataWindow.InsertRow(0);
                for (int c = 0; c < cType.Length; c++)
                {
                    try
                    {
                        if (cType[c] == "datetime")
                        {
                            dataWindow.SetItemDateTime(r + 1, cName[c], DateTime.ParseExact(dt.Rows[r][c].ToString(), "yyyy-MM-dd HH:mm:ss", WebUtil.EN));
                        }
                        else if (cType[c] == "date")
                        {
                            dataWindow.SetItemDate(r + 1, cName[c], DateTime.ParseExact(dt.Rows[r][c].ToString(), "yyyy-MM-dd", WebUtil.EN));
                        }
                        else if (cType[c] == "int" || cType[c] == "long" || cType[c] == "number" || cType[c] == "decimal")
                        {
                            dataWindow.SetItemDecimal(r + 1, cName[c], Convert.ToDecimal(dt.Rows[r][cName[c]]));
                        }
                        else if (cType[c] == "double")
                        {
                            dataWindow.SetItemDouble(r + 1, cName[c], Convert.ToDouble(dt.Rows[r][cName[c]]));
                        }
                        else
                        {
                            dataWindow.SetItemString(r + 1, cName[c], Convert.ToString(dt.Rows[r][cName[c]]));
                        }
                    }
                    catch { }
                }
            }
            try
            {
                if (dwThDate == null)
                {
                    dataWindow.ResetUpdateStatus();
                }
                else
                {
                    dwThDate.Eng2ThaiAllRow();
                }
            }
            catch { }
        }

        public static void DeleteLastRow(IDataStore dwObj)
        {
            if (dwObj.RowCount > 1)
            {
                dwObj.DeleteRow(dwObj.RowCount);
            }
        }

        public static String GetString(IDataStore dw, int row, String column)
        {
            try
            {
                return dw.GetItemString(row, column).Trim();
            }
            catch
            {
                return null;
            }
        }

        public static int GetInt(IDataStore dw, int row, String column)
        {
            try
            {
                return Convert.ToInt32(dw.GetItemDecimal(row, column));
            }
            catch
            {
                return 0;
            }
        }

        public static Decimal GetDec(IDataStore dw, int row, String column)
        {
            try
            {
                return dw.GetItemDecimal(row, column);
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime GetDateTime(IDataStore dw, int row, String column)
        {
            try
            {
                return dw.GetItemDateTime(row, column);
            }
            catch
            {
                return new DateTime(1370, 1, 1);
            }
        }

        public static String GetString(IDataWindowBase dw, int row, String column, String ifExceptionValue)
        {
            try
            {
                return dw.GetItemString(row, column).Trim();
            }
            catch
            {
                return ifExceptionValue;
            }
        }

        public static int GetInt(IDataStore dw, int row, String column, int ifExceptionValue)
        {
            try
            {
                return Convert.ToInt32(dw.GetItemDecimal(row, column));
            }
            catch
            {
                return ifExceptionValue;
            }
        }

        public static Decimal GetDec(IDataStore dw, int row, String column, decimal ifExceptionValue)
        {
            try
            {
                return dw.GetItemDecimal(row, column);
            }
            catch
            {
                return ifExceptionValue;
            }
        }
    }
}