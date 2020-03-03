using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMSApp.Services.Position
{
    public class PositionService : IPosition
    {
        DBHelper dbHelper = new DBHelper();
        public List<POSITIONAL_INFO> GetPositionList()
        {
            List<POSITIONAL_INFO> dataList = new List<POSITIONAL_INFO>();
            DataTable dtData = GetDataTable();
            if (dtData != null)
            {
                foreach (DataRow dRow in dtData.Rows)
                {
                    POSITIONAL_INFO pInfo = new POSITIONAL_INFO();
                    pInfo.POSITION_ID = Convert.ToInt64(dRow["POSITION_ID"]);
                    pInfo.POSITION_TITLE = Convert.ToString(dRow["POSITION_TITLE"]);
                    pInfo.DUTY_TYPE = Convert.ToString(dRow["DUTY_TYPE_TEXT"]);
                    pInfo.RATE_TYPE = Convert.ToString(dRow["RATE_TYPE_TEXT"]);
                    pInfo.PAY_FREQ = Convert.ToString(dRow["PAY_FREQ_TEXT"]);
                    pInfo.DIV_ID = Convert.ToInt64(dRow["DIV_ID"]);
                    dataList.Add(pInfo);
                }
            }
            return dataList;
        }

        public bool InsertIncrementInfo(INCREMENT_INFO incObj, POSITIONAL_INFO posObj)
        {
            List<KeyValuePair<SqlCommand, string>> list = new List<KeyValuePair<SqlCommand, string>>();
            list = GetInccrementInsertQuery(incObj, list);
            list = GetPositionalInfoUpdateQuery(posObj, list);
            try
            {
                bool result = dbHelper.ExecuteCommandWithParameterList(list);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private List<KeyValuePair<SqlCommand, string>> GetPositionalInfoUpdateQuery(POSITIONAL_INFO posObj, List<KeyValuePair<SqlCommand, string>> list)
        {
            string insertQuery = @"UPDATE POSITIONAL_INFO SET BASIC_SALARY=@BASIC_SALARY,UPDATE_BY=@UPDATE_BY,UPDATE_DATE=@UPDATE_DATE WHERE EMPLOYEE_ID=@EMPLOYEE_ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("BASIC_SALARY", SqlDbType.Decimal).Value = posObj.BASIC_SALARY;
            command.Parameters.Add("UPDATE_BY", SqlDbType.BigInt).Value = posObj.UPDATE_BY;
            command.Parameters.Add("EMPLOYEE_ID", SqlDbType.BigInt).Value = posObj.EMPLOYEE_ID;
            command.Parameters.Add("UPDATE_DATE", SqlDbType.DateTime).Value = posObj.UPDATE_DATE;
            list.Add(new KeyValuePair<SqlCommand, string>(command, insertQuery));
            return list;
        }

        private List<KeyValuePair<SqlCommand, string>> GetInccrementInsertQuery(INCREMENT_INFO incObj, List<KeyValuePair<SqlCommand, string>> list)
        {
            string insertQuery = @"INSERT INTO INCREMENT_INFO (EMP_ID,INCREMENT_RATE,INCREMENT_TYPE,INCREMENT_FROM,ACTION_BY,ACTION_DATE,REMARKS,INCREMENT_CALCULATE_AS,PREV_BASIC) 
                                VALUES(@EMP_ID,@INCREMENT_RATE,@INCREMENT_TYPE,@INCREMENT_FROM,@ACTION_BY,@ACTION_DATE,@REMARKS,@INCREMENT_CALCULATE_AS,@PREV_BASIC)";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("EMP_ID", SqlDbType.BigInt).Value = incObj.EMP_ID;
            command.Parameters.Add("INCREMENT_RATE", SqlDbType.Int).Value = incObj.INCREMENT_RATE;
            command.Parameters.Add("INCREMENT_TYPE", SqlDbType.BigInt).Value = incObj.INCREMENT_TYPE;
            command.Parameters.Add("INCREMENT_FROM", SqlDbType.Date).Value = incObj.INCREMENT_FROM;
            command.Parameters.Add("ACTION_BY", SqlDbType.BigInt).Value = incObj.ACTION_BY;
            command.Parameters.Add("ACTION_DATE", SqlDbType.DateTime).Value = incObj.ACTION_DATE;
            command.Parameters.Add("REMARKS", SqlDbType.NVarChar,500).Value = (string.IsNullOrEmpty(incObj.REMARKS)) ? "": incObj.REMARKS;
            command.Parameters.Add("INCREMENT_CALCULATE_AS", SqlDbType.Char,1).Value = incObj.INCREMENT_CALCULATE_AS;
            command.Parameters.Add("PREV_BASIC", SqlDbType.Decimal).Value = incObj.PREV_BASIC;

            list.Add(new KeyValuePair<SqlCommand, string>(command, insertQuery));
            return list;
        }

        private DataTable GetDataTable()
        {
            string query = @"SELECT *,
                            CASE
                            WHEN PIN.DUTY_TYPE='f' THEN 'Full Time'
                            WHEN PIN.DUTY_TYPE='p' THEN 'Part Time'
                            WHEN PIN.DUTY_TYPE='c' THEN 'Contractual'
                            WHEN PIN.DUTY_TYPE='o' THEN 'Other'
                            END DUTY_TYPE_TEXT,
                            CASE
                            WHEN PIN.RATE_TYPE='h' THEN 'Hourly'
                            WHEN PIN.RATE_TYPE='s' THEN 'Salary'
                            END RATE_TYPE_TEXT,
                            CASE
                            WHEN PIN.PAY_FREQ='d' THEN 'Daily'
                            WHEN PIN.PAY_FREQ='w' THEN 'Weekly'
                            WHEN PIN.PAY_FREQ='b' THEN 'Biweekly'
                            WHEN PIN.PAY_FREQ='m' THEN 'Monthly'
                            END PAY_FREQ_TEXT
                            FROM POSITIONAL_INFO PIN";
            DataTable dt = dbHelper.GetDataTable(query);
            return dt;
        }

        public bool UpdateIncrementInfo(INCREMENT_INFO incObj, POSITIONAL_INFO posObj)
        {
            List<KeyValuePair<SqlCommand, string>> list = new List<KeyValuePair<SqlCommand, string>>();
            list = GetInccrementUpdateQuery(incObj, list);
            list = GetPositionalInfoUpdateQuery(posObj, list);
            try
            {
                bool result = dbHelper.ExecuteCommandWithParameterList(list);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private List<KeyValuePair<SqlCommand, string>> GetInccrementUpdateQuery(INCREMENT_INFO incObj, List<KeyValuePair<SqlCommand, string>> list)
        {
            string updateQuery = @"UPDATE INCREMENT_INFO SET EMP_ID=@EMP_ID,INCREMENT_RATE=@INCREMENT_RATE,INCREMENT_TYPE=@INCREMENT_TYPE,INCREMENT_FROM=@INCREMENT_FROM,UPDATE_BY=@UPDATE_BY,UPDATE_DATE=@UPDATE_DATE,REMARKS=@REMARKS,INCREMENT_CALCULATE_AS=@INCREMENT_CALCULATE_AS
                                    WHERE INCREMENT_ID=@INCREMENT_ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("INCREMENT_ID", SqlDbType.BigInt).Value = incObj.INCREMENT_ID;
            command.Parameters.Add("EMP_ID", SqlDbType.BigInt).Value = incObj.EMP_ID;
            command.Parameters.Add("INCREMENT_RATE", SqlDbType.Int).Value = incObj.INCREMENT_RATE;
            command.Parameters.Add("INCREMENT_TYPE", SqlDbType.BigInt).Value = incObj.INCREMENT_TYPE;
            command.Parameters.Add("INCREMENT_FROM", SqlDbType.Date).Value = incObj.INCREMENT_FROM;
            command.Parameters.Add("UPDATE_BY", SqlDbType.BigInt).Value = incObj.UPDATE_BY;
            command.Parameters.Add("UPDATE_DATE", SqlDbType.DateTime).Value = incObj.UPDATE_DATE;
            command.Parameters.Add("REMARKS", SqlDbType.NVarChar, 500).Value = (string.IsNullOrEmpty(incObj.REMARKS)) ? "" : incObj.REMARKS;
            command.Parameters.Add("INCREMENT_CALCULATE_AS", SqlDbType.Char, 1).Value = incObj.INCREMENT_CALCULATE_AS;
            list.Add(new KeyValuePair<SqlCommand, string>(command, updateQuery));
            return list;
        }
    }
}