using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class ReportsController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EmployeeDemogReport()
        {
            ViewBag.EMPLOYEE_ID = SetEmployee();
            var data = service.GetDeptWiseData();
            return View(data);
        }
        [HttpPost]
        public ActionResult EmployeeDemogReport(FormCollection collection)
        {
            string status = collection["IS_DELETED"];
            long empId = Convert.ToInt64(collection["EMPLOYEE_ID"]);
            var data = service.GetDeptWiseData(status: status, empId: empId);
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
        [HttpGet]
        public ActionResult AttendanceReport()
        {
            var data = service.GetAttendanceData();
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
        [HttpPost]
        public ActionResult AttendanceReport(FormCollection collection)
        {
            string fromDate = collection["fromDate"];
            string toDate = collection["toDate"];
            long empId = Convert.ToInt64(collection["EMPLOYEE_ID"]);
            var data = service.GetAttendanceData(fromDate: fromDate, toDate: toDate, empId: empId);
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select Employee", Value = "0" }));
            return empList;
        }
        [HttpGet]
        public ActionResult DepartmentWiseReport()
        {
            ViewBag.DEPT_ID = SetDepartment();
            return View();
        }
        [HttpPost]
        public ActionResult DepartmentWiseReport(FormCollection collection)
        {
            string status = collection["IS_DELETED"];
            long deptId = Convert.ToInt64(collection["DEPT_ID"]);
            var data = service.GetDeptWiseData(status: status, deptId: deptId);
            ViewBag.DEPT_ID = SetDepartment();
            return View(data);
        }
        private List<SelectListItem> SetDepartment()
        {
            List<SelectListItem> empList = new SelectList(db.DEPARTMENT_INFO, "DEPT_ID", "DEPT_TITLE").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select Department", Value = "0" }));
            return empList;
        }
        [HttpGet]
        public ActionResult AllEmployeeSalaryReport()
        {
            var data = service.GetSalaryWithBenifitsData();
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
        [HttpPost]
        public ActionResult AllEmployeeSalaryReport(FormCollection collection)
        {
            string status = collection["CANGE_TYPE"];
            long deptId = Convert.ToInt64(collection["DEPT_ID"]);
            var data = service.GetSalaryWithBenifitsData();
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
        [HttpGet]
        public ActionResult EmployeeSalaryReport()
        {           
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View();
        }
        [HttpPost]
        public ActionResult EmployeeSalaryReport(FormCollection collection)
        {
            long Id = Convert.ToInt64(collection["EMPLOYEE_ID"]);
            var data = db.SALARY_INFO.Where(x=>x.EMPLOYEE_ID==Id).OrderByDescending(x=>x.SALARY_PAID).ToList();
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View(data);
        }
    }
}