using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;

namespace EMSApp.Controllers
{
    public class SalarySetupController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: SalarySetup
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalarySetup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalarySetup/Create
        public ActionResult Create()
        {
            var data = db.SALARY_GRADE.ToList();
            GetDataInBag();
            return View(data);
        }

        // POST: SalarySetup/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalarySetup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalarySetup/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalarySetup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalarySetup/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        private void GetDataInBag(long empId = 0, long leaveType = 0)
        {
            ViewBag.EMP_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
    }
}
