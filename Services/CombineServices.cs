using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        EMSEntities db = new EMSEntities();
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
                            WHERE EI.ID>0 " + iDFilter + statusFilter + empIdFilter;
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
                    pInfo.CONTACT = Convert.ToString(dRow["CONTACT"]);
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
                    pInfo.CHECK_IN_TIME = converterHelper.GetFormatted12HTime(Convert.ToString(dRow["CHECK_IN_TIME"]));
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

        public List<AttendanceClass> GetAttendanceDataMonthly(string fromDate = "", string toDate = "", long empId = 0)
        {
            string DateFilter = "";
            string idFilter = empId > 0 ? " and ei.id=" + empId + " " : "";
            if (!string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(fromDate))
            {
                DateFilter = " and ad.ATT_DATE BETWEEN '" + fromDate + "' AND '" + toDate + "' ";
            }

            string query = @"select ei.ID,ei.EMPLOYEE_NAME,ad.ATT_DATE from ATTENDANCE_DETAILS ad
                            inner join EMPLOYEE_INFO ei on ei.ID=ad.EMPLOYEE_ID 
                            where ei.id!=0" + idFilter + DateFilter + @"  group by ad.ATT_DATE,ei.EMPLOYEE_NAME,ei.ID";
            DataTable data = dbHelper.GetDataTable(query);
            List<AttendanceClass> list = GetAttendanceDataMonthyList(data);
            return list;
        }
        public List<AttendanceClass> GetAttendanceDataMonthyList(DataTable data)
        {
            List<AttendanceClass> list = new List<AttendanceClass>();
            var dataEmp = db.EMPLOYEE_INFO.ToList();
            try
            {
                foreach (DataRow dRow in data.Rows)
                {
                    AttendanceClass listObj = new AttendanceClass();
                    long id = 0;
                    DataColumnCollection columns = data.Columns;
                    if (columns.Contains("ID"))
                    {
                        id = Convert.ToInt64(dRow["ID"]);
                    }
                    else
                    {
                        id = Convert.ToInt64(dRow["EMPLOYEE_ID"]);
                    }
                    string name = "";
                    if (columns.Contains("EMPLOYEE_NAME"))
                    {
                        name = Convert.ToString(dRow["EMPLOYEE_NAME"]);
                    }
                    else
                    {
                        var temp = dataEmp.Where(x => x.ID == id).FirstOrDefault();
                        name = temp.EMPLOYEE_NAME;
                    }
                    var allEmpPOsitionalData = db.POSITIONAL_INFO.Where(x => x.STATUS == ConstantValue.TypeActive).ToList();
                    DateTime date = Convert.ToDateTime(dRow["ATT_DATE"]);
                    listObj.EMPLOYEE_ID = id;
                    listObj.EMPLOYEE_NAME = name;
                    listObj.ATT_DATE = date.ToString("dd/MM/yyyy");
                    var dataList = db.ATTENDANCE_DETAILS.Where(x => x.EMPLOYEE_ID == id && x.ATT_DATE == date).ToList();
                    int maxSl = Convert.ToInt32(dataList.Max(x => x.SL_NO));
                    var dayData = dataList.Where(x => x.STATUS == ConstantValue.AttendanceCheckIn).FirstOrDefault();
                    int slNo = Convert.ToInt32(dayData.SL_NO);
                    DateTime checkIn = DateTime.ParseExact(dayData.CHECK_IN_TIME.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                    listObj.CHECK_IN_TIME = checkIn.ToString("hh:mm tt");
                    bool flag = true;
                    DateTime checkOut = checkIn;
                    TimeSpan wHour = TimeSpan.Zero;
                    for (int i = slNo; i <= maxSl; i++)
                    {
                        if (slNo <= maxSl)
                        {
                            if (flag)
                            {
                                dayData = dataList.Where(x => x.STATUS == ConstantValue.AttendanceCheckOut && x.SL_NO > slNo).FirstOrDefault();
                                if (dayData != null)
                                {
                                    flag = false;
                                    checkOut = DateTime.ParseExact(dayData.CHECK_OUT_TIME.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                                    slNo = Convert.ToInt32(dayData.SL_NO);
                                    if (wHour == TimeSpan.Zero)
                                    {
                                        wHour = checkOut.Subtract(checkIn);
                                    }
                                    else
                                    {
                                        wHour += checkOut.Subtract(checkIn);
                                    }

                                }

                            }
                            else
                            {
                                flag = true;
                                dayData = dataList.Where(x => x.STATUS == ConstantValue.AttendanceCheckIn && x.SL_NO > slNo).FirstOrDefault();
                                if (dayData != null)
                                {
                                    slNo = Convert.ToInt32(dayData.SL_NO);
                                    checkIn = DateTime.ParseExact(dayData.CHECK_IN_TIME.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                    listObj.CHECK_OUT_TIME = checkOut.ToString("hh:mm tt");
                    listObj.PERDAY_WORKING_HOUR = wHour.ToString();
                    TimeSpan totalDayWT = Convert.ToDateTime(listObj.CHECK_OUT_TIME).Subtract(Convert.ToDateTime(listObj.CHECK_IN_TIME));
                    listObj.TOTAL_WORKING_HOUR = totalDayWT.ToString();
                    listObj.TOTAL_BREAK = totalDayWT.Subtract(wHour).ToString();
                    var empPOsitionalData = allEmpPOsitionalData.Where(x => x.EMPLOYEE_ID == listObj.EMPLOYEE_ID).FirstOrDefault();
                    TimeSpan actualWH = TimeSpan.FromHours(Convert.ToInt64(empPOsitionalData.WORKING_HOURS)).Subtract(ConstantValue.BreakTime);
                    TimeSpan lessWork = actualWH.Subtract(wHour);
                    TimeSpan overTime = wHour.Subtract(actualWH);
                    if (lessWork > TimeSpan.Zero)
                    {
                        listObj.LESS_WORK = lessWork.ToString();
                    }
                    else
                    {
                        listObj.LESS_WORK = "0";
                    }
                    if (overTime > TimeSpan.Zero)
                    {
                        listObj.OVER_TIME = overTime.ToString();
                    }
                    else
                    {
                        listObj.OVER_TIME = "0";
                    }
                    TimeSpan workShift = empPOsitionalData.WORKING_SHIFT;
                    DateTime arvTime = Convert.ToDateTime(listObj.CHECK_IN_TIME);
                    TimeSpan lateArrival = (arvTime.TimeOfDay).Subtract(workShift);
                    if (lateArrival > TimeSpan.Zero)
                    {
                        listObj.LATE_ARRIVED = lateArrival.ToString();
                    }
                    else
                    {
                        listObj.LATE_ARRIVED = "0";
                    }
                    list.Add(listObj);
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public List<EmployeeLeaveClass> GetEmployeeLeaveList(long empId = 0, string fromDate = "", string toDate = "")
        {
            List<EmployeeLeaveClass> leaveList = new List<EmployeeLeaveClass>();
            try
            {
                string DateFilter = "";
                string idFilter = empId > 0 ? " and ei.id=" + empId + "  " : " ";
                if (!string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(fromDate))
                {
                    DateFilter = " AND LA.APPROVED_START_DATE BETWEEN '" + fromDate + "' AND '" + toDate + "' OR LA.APPROVED_END_DATE BETWEEN '" + fromDate + "' AND '" + toDate + "' ";
                }
                string query = @"SELECT EI.EMPLOYEE_NAME,LA.EMPLOYEE_ID,SUM(LA.LEAVE_DAY) AS SUM_LEAVE FROM LEAVE_APPLICATION LA
                            INNER JOIN EMPLOYEE_INFO EI ON EI.ID=LA.EMPLOYEE_ID
                            WHERE LA.STATUS='A' " + DateFilter + idFilter + @" GROUP BY  LA.EMPLOYEE_ID,EI.EMPLOYEE_NAME";
                DataTable dt = dbHelper.GetDataTable(query);
                foreach (DataRow drow in dt.Rows)
                {
                    EmployeeLeaveClass obj = new EmployeeLeaveClass();
                    if (dt.Rows.Count > 0)
                    {
                        long id = Convert.ToInt64(drow["EMPLOYEE_ID"]);
                        string empName = Convert.ToString(drow["EMPLOYEE_NAME"]);
                        double totalLeave = Convert.ToDouble(drow["SUM_LEAVE"]);
                        double casualleave = 0, medicalLeave = 0, fullday = 0, halfDay = 0, dayCount = 0;
                        var dataList = db.LEAVE_APPLICATION.Where(x => x.STATUS == ConstantValue.LeaveStatusApproved && x.EMPLOYEE_ID == id).ToList();
                        foreach (var countList in dataList)
                        {
                            if ((Convert.ToDateTime(countList.APPROVED_START_DATE) >= Convert.ToDateTime(fromDate) && Convert.ToDateTime(countList.APPROVED_START_DATE) <= Convert.ToDateTime(toDate)) || Convert.ToDateTime(countList.APPROVED_END_DATE) >= Convert.ToDateTime(fromDate))
                            {
                                if (countList.LEAVE_TYPE_ID == ConstantValue.LeaveDayFullDayID)
                                {
                                    //count for full day leave
                                    fullday++;
                                    dayCount = dayCount + 1;
                                }
                                else if (countList.LEAVE_TYPE_ID == ConstantValue.LeaveDayHalfDayID)
                                {
                                    //count for half day leave
                                    halfDay++;
                                    dayCount = dayCount + .5;
                                }
                                else
                                {
                                    //count for casual and medical leave
                                    DateTime startdate = Convert.ToDateTime(countList.APPROVED_START_DATE);
                                    DateTime enddate = Convert.ToDateTime(countList.APPROVED_END_DATE);
                                    if (enddate > Convert.ToDateTime(toDate))
                                    {
                                        TimeSpan difference = Convert.ToDateTime(countList.APPROVED_END_DATE).Subtract(Convert.ToDateTime(toDate));
                                        double dayOther = difference.Days;
                                        double leaveDayRemain = Convert.ToInt64(countList.LEAVE_DAY) - dayOther;
                                        if (countList.LEAVE_TYPE_ID == ConstantValue.LeaveDayCasualID)
                                        {
                                            casualleave = casualleave + leaveDayRemain;
                                        }
                                        else
                                        {
                                            medicalLeave = medicalLeave + leaveDayRemain; 
                                        }
                                        dayCount = dayCount + leaveDayRemain;
                                    }
                                    else if (enddate >= Convert.ToDateTime(fromDate) && startdate <= Convert.ToDateTime(fromDate))
                                    {
                                        TimeSpan difference = Convert.ToDateTime(countList.APPROVED_END_DATE).Subtract(Convert.ToDateTime(fromDate));
                                        double dayRem = difference.Days + 1;
                                        if (countList.LEAVE_TYPE_ID == ConstantValue.LeaveDayCasualID)
                                        {
                                            casualleave = casualleave + dayRem;
                                        }
                                        else
                                        {
                                            medicalLeave = medicalLeave + dayRem;                                           
                                        }
                                        dayCount = dayCount + dayRem;
                                    }
                                    else
                                    {
                                        TimeSpan difference = Convert.ToDateTime(countList.APPROVED_END_DATE).Subtract(Convert.ToDateTime(countList.APPROVED_START_DATE));
                                        double allLeave = difference.Days + 1;
                                        if (countList.LEAVE_TYPE_ID == ConstantValue.LeaveDayCasualID)
                                        {
                                            casualleave = casualleave + allLeave;
                                        }
                                        else
                                        {
                                            medicalLeave = medicalLeave + allLeave;
                                        }
                                        dayCount = dayCount + allLeave;
                                    }
                                }
                            }
                        }
                        obj.EMPLOYEE_ID = id;
                        obj.EMPLOYEE_NAME = empName;
                        obj.TOTAL_LEAVE = ConstantValue.LeaveDayCountTotal;
                        obj.CASUAL_LEAVE = casualleave;
                        obj.MEDI_LEAVE = medicalLeave;
                        obj.HALF_DAY_LEAVE = halfDay;
                        obj.FULL_DAY_LEAVE = fullday;
                        obj.TOTAL_LEAVE_TAKEN = dayCount;
                        double remainLeaves = obj.TOTAL_LEAVE - totalLeave;
                        if (remainLeaves >= 0)
                        {
                            obj.REMAIN_LEAVE = remainLeaves;
                            obj.EXCEED_LEAVE = 0;
                        }
                        else
                        {
                            obj.REMAIN_LEAVE = 0;
                            obj.EXCEED_LEAVE = obj.TOTAL_LEAVE_TAKEN - totalLeave;
                        }
                        leaveList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return leaveList;
        }

        public bool SalarySetupUpdate(long id, SALARY_SETUP obj)
        {
            string query = @"UPDATE SALARY_SETUP SET EMP_ID="+obj.EMP_ID+", POSITION_ID="+obj.POSITION_ID+ ",PAY_TYPE='" + obj.PAY_TYPE + "',GROSS_SALARY=" + obj.GROSS_SALARY + ",SALARY_GRADE_SETUP='" + obj.SALARY_GRADE_SETUP 
                + "', SALARY_GRADE_SETUP_STRING='" + obj.SALARY_GRADE_SETUP_STRING + "',CANGE_TYPE='" + obj.CANGE_TYPE + "',UPDATE_BY=" + obj.UPDATE_BY + ",UPDATE_DATE='" + obj.UPDATE_DATE + "' WHERE SALARY_SET_ID="+id;
            return dbHelper.ExecuteDML(query);             
        }

        public bool InsertSalary(SALARY_INFO_SUM objSum, List<SALARY_INFO> objInfo)
        {
            List<KeyValuePair<SqlCommand, string>> list = new List<KeyValuePair<SqlCommand, string>>();
            list = GetSalarySumInsertQuery(objSum, list);
            list = GetSalaryInfoInsertQuery(objInfo, list);
            try
            {
                bool result = dbHelper.ExecuteCommandWithParameterList(list);
                return result;
            }
            catch(Exception ex)
            {
                return false;
            }           
        }

        private List<KeyValuePair<SqlCommand, string>> GetSalaryInfoInsertQuery(List<SALARY_INFO> objInfo, List<KeyValuePair<SqlCommand, string>> list)
        {
            string insertQuery = @"INSERT INTO SALARY_INFO (EMPLOYEE_ID,SALARY_PAID,GROSS_SALARY,BONUS,DEDUCTION,TOTAL,OTHERS,ADDITION,ADVANCE,COMMISSION,REMARKS,ACTION_BY,ACTION_DATE)
                                   VALUES(@EMPLOYEE_ID,@SALARY_PAID,@GROSS_SALARY,@BONUS,@DEDUCTION,@TOTAL,@OTHERS,@ADDITION,@ADVANCE,@COMMISSION,@REMARKS,@ACTION_BY,@ACTION_DATE)";
            foreach(var obj in objInfo)
            {
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("EMPLOYEE_ID", SqlDbType.BigInt).Value = obj.EMPLOYEE_ID;
                command.Parameters.Add("SALARY_PAID", SqlDbType.NVarChar,50).Value = obj.SALARY_PAID;
                command.Parameters.Add("GROSS_SALARY", SqlDbType.Decimal).Value = obj.GROSS_SALARY;
                command.Parameters.Add("BONUS", SqlDbType.Decimal).Value = obj.BONUS;
                command.Parameters.Add("DEDUCTION", SqlDbType.Decimal).Value = obj.DEDUCTION;
                command.Parameters.Add("TOTAL", SqlDbType.Decimal).Value = obj.TOTAL;
                command.Parameters.Add("OTHERS", SqlDbType.Decimal).Value = obj.OTHERS;
                command.Parameters.Add("ADDITION", SqlDbType.Decimal).Value = obj.ADDITION;
                command.Parameters.Add("ADVANCE", SqlDbType.Decimal).Value = obj.ADVANCE;
                command.Parameters.Add("COMMISSION", SqlDbType.Decimal).Value = obj.COMMISSION;
                command.Parameters.Add("REMARKS", SqlDbType.NVarChar,500).Value = obj.REMARKS;
                command.Parameters.Add("ACTION_BY", SqlDbType.BigInt).Value = obj.ACTION_BY;
                command.Parameters.Add("ACTION_DATE", SqlDbType.DateTime).Value = obj.ACTION_DATE;
                list.Add(new KeyValuePair<SqlCommand, string>(command, insertQuery));
            }           
            return list;
        }

        private List<KeyValuePair<SqlCommand, string>> GetSalarySumInsertQuery(SALARY_INFO_SUM objSum, List<KeyValuePair<SqlCommand, string>> list)
        {
            string insertQuery = @"INSERT INTO SALARY_INFO_SUM (SALARY_PAID_MONTH,TOTAL_PAID) VALUES(@SALARY_PAID_MONTH,@TOTAL_PAID)";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("SALARY_PAID_MONTH", SqlDbType.NVarChar,50).Value = objSum.SALARY_PAID_MONTH;
            command.Parameters.Add("TOTAL_PAID", SqlDbType.Decimal).Value = objSum.TOTAL_PAID;
            list.Add(new KeyValuePair<SqlCommand, string>(command, insertQuery));
            return list;
        }

        public bool EmployeeDelete(long id, string statusV)
        {
            List<KeyValuePair<SqlCommand, string>> list = new List<KeyValuePair<SqlCommand, string>>();
            list = ChangeEmployeeInfo(id, statusV, list);
            list = ChangePositionalInfo(id, statusV, list);
            list = ChangeSalarySetup(id, statusV, list);
            list = ChangeUserId(id, statusV, list);
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

        private List<KeyValuePair<SqlCommand, string>> ChangeUserId(long id, string statusV, List<KeyValuePair<SqlCommand, string>> list)
        {
            string query = @"UPDATE USER_INFO SET IS_DELETED=@IS_DELETED WHERE EMPLOYEE_ID=@EMPLOYEE_ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("EMPLOYEE_ID", SqlDbType.BigInt).Value = id;
            command.Parameters.Add("IS_DELETED", SqlDbType.Char,1).Value = statusV;
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }

        private List<KeyValuePair<SqlCommand, string>> ChangeSalarySetup(long id, string statusV, List<KeyValuePair<SqlCommand, string>> list)
        {
            string query = @"UPDATE SALARY_SETUP SET CANGE_TYPE=@CANGE_TYPE WHERE EMP_ID=@EMP_ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("EMP_ID", SqlDbType.BigInt).Value = id;
            command.Parameters.Add("CANGE_TYPE", SqlDbType.Char,1).Value = statusV;
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }

        private List<KeyValuePair<SqlCommand, string>> ChangePositionalInfo(long id, string statusV, List<KeyValuePair<SqlCommand, string>> list)
        {
            string query = @"UPDATE POSITIONAL_INFO SET STATUS=@STATUS WHERE EMPLOYEE_ID=@EMPLOYEE_ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("EMPLOYEE_ID", SqlDbType.BigInt).Value = id;
            command.Parameters.Add("STATUS", SqlDbType.Char,1).Value = statusV;
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }

        private List<KeyValuePair<SqlCommand, string>> ChangeEmployeeInfo(long id, string statusV, List<KeyValuePair<SqlCommand, string>> list)
        {
            string query = @"UPDATE EMPLOYEE_INFO SET IS_DELETED=@IS_DELETED WHERE ID=@ID";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("ID", SqlDbType.BigInt).Value = id;
            command.Parameters.Add("IS_DELETED", SqlDbType.Char,1).Value = statusV;
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }
    }
}