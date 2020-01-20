using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
namespace EMSApp.Controllers
{
    public class LeaveController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Leave
        public ActionResult Index()
        {
            var data = db.LEAVE_TYPE.ToList();
            return View(data);
        }

        // GET: Leave/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Leave/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leave/Create
        [HttpPost]
        public ActionResult Create(LEAVE_TYPE collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.LEAVE_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Leave Type");
                }
                
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTIVE_BY = userID;
                    collection.ACTIVE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.LEAVE_TYPE.Add(collection);
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

        // GET: Leave/Edit/5
        public ActionResult Edit(int id)
        {
            var dt = db.LEAVE_TYPE.Where(x => x.LEAVE_ID == id).FirstOrDefault();            
            Session["AD"] = dt.ACTIVE_DATE;
            return View(dt);
        }

        // POST: Leave/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LEAVE_TYPE collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.LEAVE_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Leave Type");
                }

                else
                {
                    collection.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTIVE_DATE = Convert.ToDateTime(Session["AD"]);
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

        // GET: Leave/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Leave/Delete/5
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
