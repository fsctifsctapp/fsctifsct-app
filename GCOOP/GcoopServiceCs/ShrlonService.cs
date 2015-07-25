using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sybase.DataWindow;
using DBAccess;

namespace GcoopServiceCs
{
    public class ShrlonService
    {
        private String connectionString;

        public ShrlonService(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public StructLoanRequest InitMemberNo(String pbl, String memberNo, String xmlDwMain)
        {
            Sta ta = new Sta(connectionString);
            StructLoanRequest resu = new StructLoanRequest();
            try
            {
                //d_sl_loanrequest_master
                DataStore dwMain = new DataStore(pbl, "d_sl_loanrequest_master");
                dwMain.ImportString(xmlDwMain, FileSaveAsType.Xml);
                String sql = "select * from mbmembmaster where member_no='" + memberNo + "'";
                Sdt dt = ta.Query(sql);
                if (!dt.Next()) throw new Exception("ไม่มีข้อมูลสมาชิก " + memberNo);
                dwMain.SetItemString(1, "member_name", dt.GetString("memb_name") + " " + dt.GetString("memb_surname"));
                dwMain.SetItemDateTime(1, "birth_date", dt.GetDate("birth_date"));
                resu.xmlMain = dwMain.Describe("DataWindow.Data.XML");
            }
            catch (Exception ex)
            {
                try
                {
                    ta.Close();
                }
                catch { }
                throw ex;
            }
            return resu;
        }
    }
}
