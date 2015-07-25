using System;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data;

namespace DBAccess
{
    public class Sta
    {
        private String url;
        // ORACLE
        private OracleConnection con;
        private OracleCommand cmd;
        private OracleTransaction tran;
        // OLEDB
        private OleDbConnection oleCon;
        private OleDbCommand oldCmd;
        private OleDbTransaction oleTran;

        private DbType dbType;
        private bool isTran;

        public Sta(String connectionString)
        {
            this.url = connectionString;
            //Provider=MSDAORA;Data Source=doys/gcoop;Persist Security Info=True;Password=scobkcat;User ID=scobkcat
            //Data Source=doys/gcoop;Persist Security Info=True;User ID=scobkcat;Password=scobkcat;
            if (this.url.ToLower().IndexOf("Provider=MSDAORA;".ToLower()) >= 0)
            {
                this.dbType = DbType.OleDb;
            }
            else
            {
                this.dbType = DbType.Oracle;
            }
            this.SecoundConstructor();
        }

        //---------------------------------------------------------------

        private void SecoundConstructor()
        {
            if (dbType == DbType.Oracle)
            {
                SecoundConstructorOracle();
            }
            else if (dbType == DbType.OleDb)
            {
                SecoundConstructorOleDb();
            }
        }

        private void SecoundConstructorOracle()
        {
            con = new OracleConnection(url);
            cmd = con.CreateCommand();
        }

        private void SecoundConstructorOleDb()
        {
            oleCon = new OleDbConnection(url);
            oldCmd = oleCon.CreateCommand();
        }

        //---------------------------------------------------------------

        public Sdt Query(String sql)
        {
            if (dbType == DbType.Oracle)
            {
                return QueryOracle(sql);
            }
            else if (dbType == DbType.OleDb)
            {
                return QueryOleDb(sql);
            }
            return null;
        }

        private Sdt QueryOracle(String sql)
        {
            Sdt dt = new Sdt();
            OracleDataAdapter da;
            cmd.CommandText = sql;
            da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            dt.SecoundConstructor();
            return dt;
        }

        private Sdt QueryOleDb(String sql)
        {
            Sdt dt = new Sdt();
            OleDbDataAdapter da;
            oldCmd.CommandText = sql;
            da = new OleDbDataAdapter(oldCmd);
            da.Fill(dt);
            dt.SecoundConstructor();
            return dt;
        }

        //---------------------------------------------------------------

        public DataTable QueryDataTable(String sql)
        {
            if (dbType == DbType.Oracle)
            {
                return QueryDataTableOracle(sql);
            }
            else if (dbType == DbType.OleDb)
            {
                return QueryDataTableOleDb(sql);
            }
            return null;
        }

        private DataTable QueryDataTableOracle(String sql)
        {
            DataTable dt = new DataTable();
            OracleDataAdapter da;
            cmd.CommandText = sql;
            da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryDataTableOleDb(String sql)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da;
            oldCmd.CommandText = sql;
            da = new OleDbDataAdapter(oldCmd);
            da.Fill(dt);
            return dt;
        }

        //---------------------------------------------------------------

        public int Exe(String sql)
        {
            if (dbType == DbType.Oracle)
            {
                return ExeOracle(sql);
            }
            else if (dbType == DbType.OleDb)
            {
                return ExeOleDb(sql);
            }
            return -9;
        }

        private int ExeOracle(String sql)
        {
            try
            {
                con.Open();
            }
            catch { }
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }

        public int ExeOleDb(String sql)
        {
            try
            {
                oleCon.Open();
            }
            catch { }
            oldCmd.CommandText = sql;
            return oldCmd.ExecuteNonQuery();
        }

        //---------------------------------------------------------------

        public bool Transection()
        {
            if (dbType == DbType.Oracle)
            {
                return TransectionOracle();
            }
            else if (dbType == DbType.OleDb)
            {
                return TransectionOleDb();
            }
            return false;
        }

        private bool TransectionOracle()
        {
            try
            {
                con.Open();
            }
            catch { }
            try
            {
                tran = con.BeginTransaction();
                isTran = true;
            }
            catch
            {
                isTran = false;
            }
            return isTran;
        }

        private bool TransectionOleDb()
        {
            try
            {
                oleCon.Open();
            }
            catch { }
            try
            {
                oleTran = oleCon.BeginTransaction();
                isTran = true;
            }
            catch
            {
                isTran = false;
            }
            return isTran;
        }

        //---------------------------------------------------------------

        public void RollBack()
        {
            if (dbType == DbType.Oracle)
            {
                RollBackOracle();
            }
            else if (dbType == DbType.OleDb)
            {
                RollBackOleDb();
            }
        }

        private void RollBackOracle()
        {
            try
            {
                con.Open();
            }
            catch { }
            tran.Rollback();
        }

        private void RollBackOleDb()
        {
            try
            {
                oleCon.Open();
            }
            catch { }
            oleTran.Rollback();
        }

        //---------------------------------------------------------------

        public void RollBack(bool close)
        {
            RollBack();
            if (close) Close();
        }

        //---------------------------------------------------------------

        public void Commit()
        {
            if (dbType == DbType.Oracle)
            {
                CommitOracle();
            }
            else if (dbType == DbType.OleDb)
            {
                CommitOleDb();
            }
        }

        private void CommitOracle()
        {
            try
            {
                con.Open();
            }
            catch { }
            tran.Commit();
        }

        private void CommitOleDb()
        {
            try
            {
                oleCon.Open();
            }
            catch { }
            oleTran.Commit();
        }

        //---------------------------------------------------------------

        public void Commit(bool close)
        {
            Commit();
            if (close) Close();
        }

        //---------------------------------------------------------------

        public void Close()
        {
            if (dbType == DbType.Oracle)
            {
                CloseOracle();
            }
            else if (dbType == DbType.OleDb)
            {
                CloseOleDb();
            }
        }

        private void CloseOracle()
        {
            try
            {
                con.Close();
            }
            catch { }
        }

        private void CloseOleDb()
        {
            try
            {
                oleCon.Close();
            }
            catch { }
        }
    }
}
