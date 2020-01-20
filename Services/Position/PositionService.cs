using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}