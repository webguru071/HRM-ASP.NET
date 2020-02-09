using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class ReportsController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        ConverterHelper converter = new ConverterHelper();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EmployeeDemogReport()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                ViewBag.EMPLOYEE_ID = SetEmployee();
                var data = service.GetDeptWiseData();
                return View(data);
            }
           else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }
        [HttpPost]
        public ActionResult EmployeeDemogReport(FormCollection collection)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string status = collection["IS_DELETED"];
                long empId = converter.GetLoggedEmployeeID();
                var data = service.GetDeptWiseData(status: status, empId: empId);
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
        [HttpGet]
        public ActionResult AttendanceReport()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = service.GetAttendanceData();
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
        [HttpPost]
        public ActionResult AttendanceReport(FormCollection collection)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string fromDate = collection["fromDate"];
                string toDate = collection["toDate"];
                long empId = converter.GetLoggedEmployeeID();
                var data = service.GetAttendanceData(fromDate: fromDate, toDate: toDate, empId: empId);
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
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
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                ViewBag.DEPT_ID = SetDepartment();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpPost]
        public ActionResult DepartmentWiseReport(FormCollection collection)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string status = collection["IS_DELETED"];
                long deptId = Convert.ToInt64(collection["DEPT_ID"]);
                var data = service.GetDeptWiseData(status: status, deptId: deptId);
                ViewBag.DEPT_ID = SetDepartment();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
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
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = service.GetSalaryWithBenifitsData();
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpPost]
        public ActionResult AllEmployeeSalaryReport(FormCollection collection)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string status = collection["CANGE_TYPE"];
                long deptId = Convert.ToInt64(collection["DEPT_ID"]);
                var data = service.GetSalaryWithBenifitsData();
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpGet]
        public ActionResult EmployeeSalaryReport()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
        [HttpPost]
        public ActionResult EmployeeSalaryReport(FormCollection collection)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                long Id=converter.GetLoggedEmployeeID();
                var data = db.SALARY_INFO.Where(x => x.EMPLOYEE_ID == Id).OrderByDescending(x => x.SALARY_PAID).ToList();
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
    }
}