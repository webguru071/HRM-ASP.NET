using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;

namespace EMSApp.Controllers
{
    public class DepartmentController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Department
        public ActionResult Index()
        {
            var data = db.DEPARTMENT_INFO.ToList();
            return View(data);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(DEPARTMENT_INFO obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.DEPT_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Department Name");
                }

                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    obj.ACTION_BY = userID;
                    obj.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.DEPARTMENT_INFO.Add(obj);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            var dt = db.DEPARTMENT_INFO.Where(x => x.DEPT_ID == id).FirstOrDefault();
            Session["AD"] = dt.ACTION_DATE;
            return View(dt);
        }

        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DEPARTMENT_INFO obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.DEPT_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Department Name");
                }

                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    obj.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    obj.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    obj.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            var dt = db.DEPARTMENT_INFO.Where(x => x.DEPT_ID == id).FirstOrDefault();
            return View(dt);
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.DEPARTMENT_INFO.Where(x => x.DEPT_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.DEPARTMENT_INFO.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }
    }
}
