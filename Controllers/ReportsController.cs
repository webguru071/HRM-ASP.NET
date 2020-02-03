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
            return View();
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
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
    }
}