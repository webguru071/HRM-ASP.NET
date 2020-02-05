using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class EmployeeProfileController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();

        // GET: EmployeeProfile
        public ActionResult Index()
        {
            long empId = Convert.ToInt64(Session["EMP_ID"]);
            if (empId > 0)            {
                var data = db.EMPLOYEE_INFO.Where(x => x.ID == empId && x.IS_DELETED == Helper.ConstantValue.UserStatusActive).FirstOrDefault();
                return View(data);
            }
            else
            {
                return View();
            }
        }
        // GET: EmployeeProfile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: EmployeeProfile/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: EmployeeProfile/Create
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
        // GET: EmployeeProfile/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.EMPLOYEE_INFO.Where(x => x.ID == id && x.IS_DELETED == Helper.ConstantValue.UserStatusActive).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            Session["NAME"] = data.EMPLOYEE_NAME;
            Session["ISD"] = data.IS_DELETED;
            return View(data);
        }
        // POST: EmployeeProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EMPLOYEE_INFO emp)
        {
            try
            {
                // TODO: Add update logic here
                emp.EMPLOYEE_NAME = Convert.ToString(Session["NAME"]);
                emp.IS_DELETED = Convert.ToString(Session["ISD"]);
                emp.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                emp.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                emp.UPDATE_DATE = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["AD"] = null;
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                return View();
            }
            return View();
        }

        // GET: EmployeeProfile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: EmployeeProfile/Delete/5
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
