using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Helper;
using System.Data.Entity;

namespace EMSApp.Controllers
{
    public class SalaryGradeController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: SalaryGrade
        public ActionResult Index()
        {
            var data = db.SALARY_GRADE.ToList();
            return View(data);
        }
        // GET: SalaryGrade/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: SalaryGrade/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: SalaryGrade/Create
        [HttpPost]
        public ActionResult Create(SALARY_GRADE collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.GRADE_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Grade Title");

                }
                else if (string.IsNullOrEmpty(collection.GRADE_TYPE))
                {
                    ModelState.AddModelError("", "Please Add Grade Type");
                }
                else
                {
                    collection.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.SALARY_GRADE.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            catch
            {
                return View();
            }
            return View();
        }
        // GET: SalaryGrade/Edit/5
        public ActionResult Edit(int id)
        {
            var dt = db.SALARY_GRADE.Where(x => x.GRADE_ID == id).FirstOrDefault();
            Session["AD"] = dt.ACTION_DATE;
            return View(dt);
        }
        // POST: SalaryGrade/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SALARY_GRADE collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.GRADE_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Grade Title");

                }
                else if (string.IsNullOrEmpty(collection.GRADE_TYPE))
                {
                    ModelState.AddModelError("", "Please Add Grade Type");
                }
                else
                {
                    collection.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    collection.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
            }

            catch
            {
                return View();
            }
            return View();
        }
        // GET: SalaryGrade/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: SalaryGrade/Delete/5
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
    }
}
