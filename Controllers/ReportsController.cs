using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Models.UserModel;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class ReportsController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        ConverterHelper converterHelper = new ConverterHelper();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EmployeeDemogReport()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string status = collection["IS_DELETED"];
                long empId = converterHelper.GetLoggedEmployeeID();
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                string fromDate = collection["fromDate"];
                string toDate = collection["toDate"];
                long empId = Convert.ToInt64(collection["EMPLOYEE_ID"]);
                var data = service.GetAttendanceData(fromDate: fromDate, toDate: toDate, empId: empId);
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        [HttpGet]
        public ActionResult AttendanceReportMonthly()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {                
                ViewBag.EMPLOYEE_ID = SetEmployee();
                ViewBag.MONTH = SetMonth();
                ViewBag.YEAR = SetYear();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        [HttpPost]
        public ActionResult AttendanceReportMonthly(FormCollection collection)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                if (string.IsNullOrEmpty(collection["MONTH"]))
                {
                    ModelState.AddModelError("", "Please Select Month!!!");
                }
                else if (string.IsNullOrEmpty(collection["YEAR"]))
                {
                    ModelState.AddModelError("", "Please Select Year!!!");
                }
                else if (string.IsNullOrEmpty(collection["EMPLOYEE_ID"]))
                {
                    ModelState.AddModelError("", "Please Select Employee!!!");
                }
                else
                {
                    string fromDate = "";
                    string toDate = "";
                    if (!string.IsNullOrEmpty(collection["MONTH"]) && !string.IsNullOrEmpty(collection["YEAR"]))
                    {
                        fromDate = collection["YEAR"] + "-" + collection["MONTH"] + "-01";
                        DateTime firstDate = Convert.ToDateTime(fromDate);
                        DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
                        toDate = lastDate.ToString("yyyy-MM-dd");
                    }
                    long empId = Convert.ToInt64(collection["EMPLOYEE_ID"]);
                    var data = service.GetAttendanceDataMonthly(fromDate: fromDate, toDate: toDate, empId: empId);
                    ViewBag.STR = " of " + collection["MONTH"] + ", " + collection["YEAR"];
                    ViewBag.EMPLOYEE_ID = SetEmployee();
                    ViewBag.MONTH = SetMonth();
                    ViewBag.YEAR = SetYear();
                    return View(data);
                }
                ViewBag.STR = " of " + collection["MONTH"] + ", " + collection["YEAR"];
                ViewBag.EMPLOYEE_ID = SetEmployee();
                ViewBag.MONTH = SetMonth();
                ViewBag.YEAR = SetYear();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select Employee", Value = "" }));
            return empList;
        }
        [HttpGet]
        public ActionResult DepartmentWiseReport()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                long Id = converterHelper.GetLoggedEmployeeID();
                var data = db.SALARY_INFO.Where(x => x.EMPLOYEE_ID == Id).OrderByDescending(x => x.SALARY_PAID).ToList();
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        [HttpGet]
        public ActionResult InvStockReport()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var allData = db.STOCK_INFO.ToList();
                List<long> idList = new List<long>();
                List<InvProductClass> dataList = new List<InvProductClass>();
                foreach (var data in allData)
                {
                    long id = data.EQP_ID;
                    bool flag = idList.Contains(id);
                    if (!flag)
                    {
                        idList.Add(id);
                        int unitPurc = allData.Where(x => x.STOCK_FOR == ConstantValue.StockForAdd && x.EQP_ID == id).Sum(x => x.UNIT);
                        int unitSold = allData.Where(x => x.STOCK_FOR == ConstantValue.StockForDeduct && x.EQP_ID == id).Sum(x => x.UNIT);
                        int unitAvail = unitPurc - unitSold;
                        InvProductClass dt = new InvProductClass();
                        dt.EQP_ID = id;
                        dt.UNIT = unitAvail;
                        dt.EQUIPMENT_TITLE = data.EQUEPMENTS_INFO.EQP_TITLE;
                        dataList.Add(dt);
                    }
                }
                return View(dataList);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        private dynamic SetYear()
        {
            string year = DateTime.Now.Year.ToString();
            List<SelectListItem> list = new SelectList(ListValue.Year, "Value", "Key", year).ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return list;
        }

        private dynamic SetMonth()
        {
            //string month = (DateTime.Now.Month).ToString();
            List<SelectListItem> list = new SelectList(ListValue.MonthDate, "Value", "Key").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return list;
        }
    }
}