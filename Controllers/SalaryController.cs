using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;

namespace EMSApp.Controllers
{
    public class SalaryController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Salary
        public ActionResult Index()
        {
            var data = db.SALARY_INFO.ToList();
            return View(data);
        }
        // GET: Salary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Salary/Create
        public ActionResult Create()
        {
            ViewBag.EMPLOYEE_ID = SetEmployee();
            return View();
        }
        // POST: Salary/Create
        [HttpPost]
        public ActionResult Create(SALARY_INFO salary)
        {
            try
            {
                // TODO: Add insert logic here
                if (salary.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.SALARY_PAID.ToString()))
                {
                    ModelState.AddModelError("", "Salary Paid Date is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.GROSS_SALARY.ToString()))
                {
                    ModelState.AddModelError("", "Salary is Required!!");
                }
                else
                {
                    decimal bonus = salary.BONUS != null ? Convert.ToDecimal(salary.BONUS) : 0;
                    decimal other = (salary.OTHERS != null) ? Convert.ToDecimal(salary.OTHERS) : 0;
                    salary.TOTAL = salary.GROSS_SALARY + bonus + other;
                    if (salary.ACTION_BY.ToString() != null)
                    {
                        salary.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    }
                    salary.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.SALARY_INFO.Add(salary);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
        }
        // GET: Salary/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.SALARY_INFO.Where(x => x.ID == id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            //Session["AT"] = data.ACTION_BY;
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", data.EMPLOYEE_ID);
            return View(data);
        }
        // POST: Salary/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SALARY_INFO salary)
        {
            try
            {
                // TODO: Add update logic here

                if (salary.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.SALARY_PAID.ToString()))
                {
                    ModelState.AddModelError("", "Salary Paid Date is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.GROSS_SALARY.ToString()))
                {
                    ModelState.AddModelError("", "Salary is Required!!");
                }
                else
                {
                    decimal bonus = salary.BONUS != null ? Convert.ToDecimal(salary.BONUS) : 0;
                    decimal other = (salary.OTHERS != null) ? Convert.ToDecimal(salary.OTHERS) : 0;
                    salary.TOTAL = salary.GROSS_SALARY + bonus + other;
                    salary.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    salary.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    salary.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(salary).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
        }

        // GET: Salary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Salary/Delete/5
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
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        public JsonResult GetEmpInfo(long id)
        {
            var data = db.SALARY_SETUP.Where(x => x.EMP_ID == id).FirstOrDefault();
            Dictionary<string, string> listOfData = new Dictionary<string, string>();
            if (data != null)
            {
                listOfData["GROSS_SALARY"] = data.GROSS_SALARY.ToString();
            }
            else
            {
                listOfData["GROSS_SALARY"] = "";
            }
            return Json(listOfData, JsonRequestBehavior.AllowGet);
        }
    }
}
