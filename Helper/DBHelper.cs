using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMSApp.Helper
{
    public class DBHelper
    {
        SqlConnection sqlConn;
        SqlDataAdapter sqlDa;
        SqlCommand sqlCmd;
        DataSet sqlDataSet;

        string strconnString = GetConnString();
        public static string GetConnString()
        {
            string connStr = "";
            try
            {
                if (ConfigurationManager.ConnectionStrings["entityConn"] == null)
                    connStr = @"Data Source=.;Initial Catalog=EMS;User ID=sa;password=12345";
                else
                    connStr = ConfigurationManager.ConnectionStrings["entityConn"].ConnectionString;
            }
            catch (Exception ex)
            {

                // ReallySimpleLog.WriteLog(ex);
            }
            return connStr;
        }
        private void OpenDBConnection()
        {
            sqlConn = new SqlConnection(strconnString);
            if (sqlConn.State == ConnectionState.Closed)
            {
                try
                {
                    sqlConn.Open();
                }
                catch (Exception ex)
                {
                    //ReallySimpleLog.WriteLog(ex);
                }
            }
        }

        private void CloseDBConnection()
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlDa = null;
            sqlCmd = null;
            sqlConn = null;
            sqlDataSet = null;
        }
        public object GetSingleValue(string query)
        {
            object scalarValue = new object();
            try
            {
                OpenDBConnection();
                sqlCmd = new SqlCommand(query, sqlConn);
                scalarValue = sqlCmd.ExecuteScalar();
                return scalarValue;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public DataSet GetDataSet(string strSql, string strDataTblName)
        {
            try
            {
                OpenDBConnection();
                sqlDa = new SqlDataAdapter(strSql, sqlConn);
                sqlDataSet = new DataSet();
                sqlCmd = new SqlCommand();
                sqlDa.Fill(sqlDataSet, strDataTblName);
                return sqlDataSet;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public DataTable GetDataTable(string strSQL)
        {
            try
            {
                OpenDBConnection();
                sqlDa = new SqlDataAdapter(strSQL, sqlConn);
                sqlDataSet = new DataSet();
                sqlCmd = new SqlCommand();
                sqlDa.Fill(sqlDataSet, "datatable");
                return sqlDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                return null;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public DataTable GetDataTable(SqlCommand command, string strSQL)
        {
            try
            {
                OpenDBConnection();
                command.CommandText = strSQL;
                command.Connection = sqlConn;
                sqlDa = new SqlDataAdapter(command);
                sqlDataSet = new DataSet();
                sqlDa.Fill(sqlDataSet, "datatable");
                return sqlDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                return null;
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public DataSet GetDataSet(string strSQL)
        {
            try
            {
                OpenDBConnection();
                sqlDa = new SqlDataAdapter(strSQL, sqlConn);
                sqlDataSet = new DataSet();
                sqlCmd = new SqlCommand();
                sqlDa.Fill(sqlDataSet, "datatable");
                return sqlDataSet;
            }
            catch (Exception ex)
            {
                // ReallySimpleLog.WriteLog(ex);
                return null;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public bool ExecuteDML(String strSql)
        {
            try
            {
                OpenDBConnection();
                sqlCmd = new SqlCommand(strSql, sqlConn);
                sqlCmd.ExecuteNonQuery();
                sqlCmd = null;
                return true;
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                return false;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public string ExecuteDMLGetId(String strSql)
        {
            string returnId = "1";
            try
            {
                OpenDBConnection();
                sqlCmd = new SqlCommand(strSql, sqlConn);
                returnId = sqlCmd.ExecuteScalar().ToString();
                sqlCmd = null;
                return returnId;
            }
            catch (Exception ex)
            {
                // ReallySimpleLog.WriteLog(ex);
                return returnId;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public bool ExecuteBatchDML(List<string> arrSQL)
        {
            OpenDBConnection();
            SqlTransaction sqlTrans = null;
            string sql = "";
            try
            {
                int intLoop = 0;
                sqlTrans = sqlConn.BeginTransaction();
                for (intLoop = 0; intLoop < arrSQL.Count; intLoop++)
                {
                    string query = string.Format(arrSQL[intLoop].ToString().Trim());
                    sql = query;
                    sqlCmd = new SqlCommand(query, sqlConn);
                    sqlCmd.CommandTimeout = 120 * 60;
                    sqlCmd.ExecuteNonQuery();
                }
                sqlTrans.Commit();
                sqlCmd = null;
                return true;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                //ReallySimpleLog.WriteLog(sql);
                //ReallySimpleLog.WriteLog(ex);
                return false;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        internal bool ExecuteCommandWithParameter(SqlCommand command, string query)
        {
            try
            {
                OpenDBConnection();
                command.Connection = sqlConn;
                command.CommandText = query;
                int result = command.ExecuteNonQuery();
                return (result == 1);
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                return false;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        //when store procedure 
        internal bool ExecuteCommandWithParameterSp(SqlCommand command, string query)
        {
            try
            {
                OpenDBConnection();
                command.Connection = sqlConn;
                command.CommandText = query;
                int result = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                return false;
            }
            finally
            {
                CloseDBConnection();
            }
        }
        internal bool ExecuteCommandWithParameterList(List<KeyValuePair<SqlCommand, string>> queryList)
        {
            OpenDBConnection();
            SqlTransaction oraTrans = null;
            string erroSql = "";
            try
            {
                oraTrans = sqlConn.BeginTransaction();
                foreach (var element in queryList)
                {
                    SqlCommand command = (SqlCommand)element.Key;
                    string query = element.Value;
                    erroSql = query;
                    command.Connection = sqlConn;
                    command.CommandText = query;
                    command.Transaction = oraTrans;
                    command.ExecuteNonQuery();
                }
                oraTrans.Commit();
                sqlCmd = null;
                return true;
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
                //ReallySimpleLog.WriteLog(erroSql);
                return false;
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public DataSet GetDataSetByProcedure(Hashtable ht, string SProc)
        {
            SqlConnection con = new SqlConnection(strconnString);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con = new SqlConnection(strconnString);
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandText = SProc;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (object obj in ht.Keys)
                {
                    string ColumnName = Convert.ToString(obj);
                    SqlParameter param = new SqlParameter(ColumnName, ht[obj]);
                    cmd.Parameters.Add(param);
                }
                cmd.CommandTimeout = 1000000;
                adp.SelectCommand = cmd;
                adp.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                //ReallySimpleLog.WriteLog(ex);
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public DataTable GetDataByProcedure(string SProc)
        {


            SqlConnection con = new SqlConnection(strconnString);
            SqlCommand cmd = new SqlCommand();


            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con = new SqlConnection(strconnString);
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandText = SProc;
                cmd.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand = cmd;
                adp.Fill(ds, "Table1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }

    }
}