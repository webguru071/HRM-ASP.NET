using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        ConverterHelper converterHelper = new ConverterHelper();
        public List<EmployeeReport> GetDeptWiseData(string status = "", long deptId = 0, long empId = 0)
        {           
            string iDFilter = (deptId <= 0) ? "" : " AND DII.DEPT_ID=" + deptId;
            string empIdFilter = (empId <= 0) ? "" : " AND EI.ID=" + empId;
            string statusFilter = (string.IsNullOrEmpty(status)) ? "" : " AND EI.IS_DELETED='" + status + "' ";
            string query = @"SELECT EI.*,DII.*,DI.*,PI.*
                            ,CASE
                            WHEN EI.IS_DELETED='a' THEN 'Active'
                            WHEN EI.IS_DELETED='d' THEN 'Deactive'
                            ELSE ''
                            END STATUS
                            ,CASE
                            WHEN EI.GENDER='m' THEN 'Male'
                            WHEN EI.GENDER='f' THEN 'Female'
                            WHEN EI.GENDER='o' THEN 'Other'
                            ELSE ''
                            END GENDER_TEXT
                            ,CASE
                            WHEN EI.MARITALA_STATUS='s' THEN 'Single'
                            WHEN EI.MARITALA_STATUS='m' THEN 'Married'
                            WHEN EI.MARITALA_STATUS='x' THEN 'Separeted'
                            ELSE ''
                            END MARITALA_STATUS_TEXT
                            ,CASE
                            WHEN PI.DUTY_TYPE='f' THEN 'Full Time'
                            WHEN PI.DUTY_TYPE='p' THEN 'Part Time'
                            WHEN PI.DUTY_TYPE='c' THEN 'Contructual'
                            WHEN PI.DUTY_TYPE='o' THEN 'Other'
                            ELSE ''
                            END DUTY_TYPE_TEXT
                            ,CASE
                            WHEN PI.RATE_TYPE='h' THEN 'Hourly'
                            WHEN PI.RATE_TYPE='s' THEN 'Salary'
                            ELSE ''
                            END RATE_TYPE_TEXT
                            ,CASE
                            WHEN PI.PAY_FREQ='d' THEN 'Daily'
                            WHEN PI.PAY_FREQ='w' THEN 'Weekly'
                            WHEN PI.PAY_FREQ='b' THEN 'Biweekly'
                            WHEN PI.PAY_FREQ='m' THEN 'Monthly'
                            ELSE ''
                            END PAY_FREQ_TEXT
                            FROM  POSITIONAL_INFO PI
                            INNER JOIN EMPLOYEE_INFO EI ON EI.ID=PI.EMPLOYEE_ID
                            INNER JOIN DIVISION_INFO DI ON PI.DIV_ID=DI.DIV_ID
                            INNER JOIN DEPARTMENT_INFO DII ON DI.DEPT_ID=DII.DEPT_ID
                            WHERE PI.CHANGE_TYPE='a' " + iDFilter + statusFilter +empIdFilter;
            DataTable data = dbHelper.GetDataTable(query);
            return GetDeptWiseEmpDataInList(data);
        }
        private List<EmployeeReport> GetDeptWiseEmpDataInList(DataTable dataTable)
        {
            List<EmployeeReport> dataList = new List<EmployeeReport>();
            if (dataTable != null)
            {
                foreach (DataRow dRow in dataTable.Rows)
                {
                    EmployeeReport pInfo = new EmployeeReport();
                    pInfo.ID = Convert.ToInt64(dRow["ID"]);
                    pInfo.EMPLOYEE_NAME = Convert.ToString(dRow["EMPLOYEE_NAME"]);
                    pInfo.ADDRESS = Convert.ToString(dRow["ADDRESS"]);
                    pInfo.NID = Convert.ToString(dRow["NID"]);                    
                    pInfo.CITY = Convert.ToString(dRow["CITY"]);
                    if (!string.IsNullOrEmpty(dRow["POSTAL_CODE"].ToString()))
                    {
                        pInfo.POSTAL_CODE = Convert.ToString(dRow["POSTAL_CODE"]);
                    }
                    pInfo.DOB = Convert.ToString(dRow["DOB"]);
                    pInfo.GENDER_TEXT = Convert.ToString(dRow["GENDER_TEXT"]);
                    pInfo.MARITALA_STATUS_TEXT = Convert.ToString(dRow["MARITALA_STATUS_TEXT"]);
                    pInfo.NATIONALITY = Convert.ToString(dRow["NATIONALITY"]);
                    pInfo.BLOOD_GROUP = Convert.ToString(dRow["BLOOD_GROUP"]);
                    pInfo.DUTY_TYPE_TEXT = Convert.ToString(dRow["DUTY_TYPE_TEXT"]);
                    pInfo.RATE_TYPE_TEXT = Convert.ToString(dRow["RATE_TYPE_TEXT"]);
                    pInfo.PAY_FREQ_TEXT = Convert.ToString(dRow["PAY_FREQ_TEXT"]);
                    pInfo.DEPT_TITLE = Convert.ToString(dRow["DEPT_TITLE"]);
                    pInfo.DIV_TITLE = Convert.ToString(dRow["DIV_TITLE"]);
                    pInfo.POSITION_TITLE = Convert.ToString(dRow["POSITION_TITLE"]);
                    pInfo.CONTACT  = Convert.ToString(dRow["CONTACT"]);
                    pInfo.EMAIL = Convert.ToString(dRow["EMAIL"]);
                    pInfo.JOINING_DATE = Convert.ToString(dRow["JOINING_DATE"]);
                    pInfo.RESIGNING_DATE = Convert.ToString(dRow["RESIGNING_DATE"]);
                    pInfo.STATUS = Convert.ToString(dRow["STATUS"]);
                    dataList.Add(pInfo);
                }
            }
            return dataList;
        }
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
                    pInfo.ATT_DATE = dateAtt.ToString("dd-MM-yyyy");                    
                    pInfo.CHECK_IN_TIME =converterHelper.GetFormatted12HTime( Convert.ToString(dRow["CHECK_IN_TIME"]));
                    pInfo.CHECK_OUT_TIME = converterHelper.GetFormatted12HTime(Convert.ToString(dRow["CHECK_OUT_TIME"]));
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

        public List<SalaryInfo> GetSalaryWithBenifitsData(string status = "")
        {
            string statusFilter = (string.IsNullOrEmpty(status)) ? "" : " AND EI.IS_DELETED='" + status + "' ";
            string query = @"SELECT SS.*,PI.*,EI.* FROM SALARY_SETUP SS
                            INNER JOIN EMPLOYEE_INFO EI ON SS.EMP_ID=EI.ID
                            INNER JOIN POSITIONAL_INFO PI ON PI.EMPLOYEE_ID=SS.EMP_ID";
            DataTable data = dbHelper.GetDataTable(query);
            return GetSalaryBenifitDataInList(data);
        }

        private List<SalaryInfo> GetSalaryBenifitDataInList(DataTable data)
        {
            List<SalaryInfo> dataList = new List<SalaryInfo>();
            if (data != null)
            {
                foreach (DataRow dRow in data.Rows)
                {
                    SalaryInfo pInfo = new SalaryInfo();
                    pInfo.EMPLOYEE_NAME = Convert.ToString(dRow["EMPLOYEE_NAME"]);
                    pInfo.BASIC_SALARY = Convert.ToDecimal(dRow["BASIC_SALARY"]);
                    pInfo.GROSS_SALARY = Convert.ToDecimal(dRow["GROSS_SALARY"]);
                    pInfo.SALARY_GRADE_SETUP = Convert.ToString(dRow["SALARY_GRADE_SETUP"]);
                    dataList.Add(pInfo);
                }
            }
            return dataList;
        }
    }
}