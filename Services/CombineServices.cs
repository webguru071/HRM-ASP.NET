using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Models.UserModel;

namespace EMSApp.Services
{
    public class CombineServices : ICombine
    {
        DBHelper dbHelper = new DBHelper();

        public List<AttendanceClass> GetAttendanceData(string fromDate = "", string toDate = "", long empId = 0)
        {
            string fromDateFilter = (string.IsNullOrEmpty(fromDate)) ? "" : " AND AT.ATT_DATE='" + fromDate + "' ";
            string toDateFilter = (string.IsNullOrEmpty(toDate)) ? "" : " AND AT.ATT_DATE='" + toDate + "' ";
            string iDFilter = (empId <= 0) ? "" : " AND AT.EMPLOYEE_ID=" + empId;
            string DateFilter = "";
            if (!string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(fromDate))
            {
                fromDateFilter = "";
                toDateFilter = "";
                DateFilter = " AND AT.ATT_DATE BETWEEN '" + fromDate + "' AND '" + toDate + "' ";
            }

            string query = @"SELECT AT.*,EI.EMPLOYEE_NAME FROM ATTENDANCE_DETAILS AT
                            INNER JOIN EMPLOYEE_INFO EI ON EI.ID=AT.EMPLOYEE_ID 
                            WHERE AT.EMPLOYEE_ID!=0 " + fromDateFilter + toDateFilter + DateFilter + iDFilter + @"  ORDER BY EMPLOYEE_ID";
            DataTable data = dbHelper.GetDataTable(query);
            return GetAttendanceDataInList(data);
        }
        private List<AttendanceClass> GetAttendanceDataInList(DataTable dataTable)
        {
            List<AttendanceClass> dataList = new List<AttendanceClass>();
            if (dataTable != null)
            {
                foreach (DataRow dRow in dataTable.Rows)
                {
                    AttendanceClass pInfo = new AttendanceClass();
                    pInfo.EMPLOYEE_NAME = Convert.ToString(dRow["EMPLOYEE_NAME"]);
                    DateTime dateAtt = Convert.ToDateTime(dRow["ATT_DATE"]);
                    pInfo.ATT_DATE = dateAtt.ToString("yyyy-MM-dd");
                    pInfo.CHECK_IN_TIME = Convert.ToString(dRow["CHECK_IN_TIME"]);
                    pInfo.CHECK_IN_TIME = Convert.ToString(dRow["CHECK_IN_TIME"]);
                    pInfo.CHECK_OUT_TIME = Convert.ToString(dRow["CHECK_OUT_TIME"]);
                    dataList.Add(pInfo);
                }
            }
            return dataList;
        }
        public string GetTeamLeaderById(long id)
        {
            string query = @"SELECT * FROM TEAM_INFO TI 
                            INNER JOIN EMPLOYEE_INFO EI ON EI.ID=TI.TEAM_LEADER
                            WHERE EI.IS_DELETED='a' AND TI.ID=" + id;
            DataTable data = dbHelper.GetDataTable(query);
            string leaderName = data.Rows[0]["EMPLOYEE_NAME"].ToString();
            return leaderName;
        }

        public bool SalarySetupStatusChange(long id, string statusV)
        {
            string query = @"UPDATE SALARY_SETUP  SET CANGE_TYPE='" + statusV + "' WHERE SALARY_SET_ID=" + id;
            bool result = dbHelper.ExecuteDML(query);
            return result;
        }

    }
}