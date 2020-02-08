using System;
using System.Collections.Generic;
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
        List<AttendanceClass> GetAttendanceData(string fromDate="",string toDate="",long empId=0);
        List<EmployeeReport> GetDeptWiseData(string status="",long deptId=0, long empId = 0);
        List<SalaryInfo> GetSalaryWithBenifitsData(string status="");
    }
}