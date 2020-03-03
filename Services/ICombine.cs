using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EMSApp.Models;
using EMSApp.Models.UserModel;

namespace EMSApp.Services
{
    public interface ICombine
    {
        string GetTeamLeaderById(long id);
        bool SalarySetupStatusChange(long id, string statusV);
        bool EmployeeDelete(long id, string statusV);
        bool SalarySetupUpdate(long id, SALARY_SETUP obj);
        bool InsertSalary(SALARY_INFO_SUM objSum,  List<SALARY_INFO> objInfo);
        List<AttendanceClass> GetAttendanceData(string fromDate="",string toDate="",long empId=0);
        List<AttendanceClass> GetAttendanceDataMonthly(string fromDate="",string toDate="",long empId=0);
        List<EmployeeReport> GetDeptWiseData(string status="",long deptId=0, long empId = 0);
        List<SalaryInfo> GetSalaryWithBenifitsData(string status="");
        List<AttendanceClass> GetAttendanceDataMonthyList(DataTable data);
        List<EmployeeLeaveClass> GetEmployeeLeaveList(long empId = 0, string fromDate = "", string toDate = "");
    }
}