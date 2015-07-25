using System;
using System.Configuration;

namespace WebService
{
    public class DwTrans : Sybase.DataWindow.Transaction
    {
        private String connectionString;
        public String ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public DwTrans(String connectionString) : base(new System.ComponentModel.Container())
        {
            try
            {
                this.connectionString = connectionString;
                //this.connectionString = "Data Source=imm/gco;Persist Security Info=True;User ID=scocbtch;Password=scocbtch;";
                // Profile
                this.Dbms = Sybase.DataWindow.DbmsType.Oracle10g;
                this.Password = GetElement("Password");
                this.ServerName = GetElement("Data Source");
                this.UserId = GetElement("User ID");
                this.AutoCommit = false;
                this.DbParameter = "PBCatalogOwner='" + this.UserId + "',TableCriteria='," + this.UserId + "'";
            }
            catch (Exception ex) {  }
        }

        private String GetElement(String elementName)
        {
            String result = null;
            try
            {
                String[] conArray = connectionString.Split(';');
                for (int i = 0; i < conArray.Length; i++)
                {
                    if (conArray[i].IndexOf(elementName) == 0)
                    {
                        String[] ar2 = conArray[i].Split('=');
                        result = ar2[1].Trim();
                        break;
                    }
                }
            }
            catch { result = null; }
            return result;
        }
    }
}

