using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class NoticeController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Notice
        public ActionResult Index()
        {
            var data = db.NOTICE_BOARD.Where(X=>X.STATUS=="a").ToList();
            return View(data);
        }

        // GET: Notice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        private void GetDataInBag(string status = "")
        {          
            ViewBag.STATUS = new SelectList(SetStatusList(), "Value", "Text", status);
        }
        private List<SelectListItem> SetStatusList()
        {
            var activeStatus = new List<SelectListItem>
            {
                new SelectListItem{ Text="Active", Value = "a" },
                new SelectListItem{ Text="Deactive", Value = "d" }
            };
            activeStatus.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return activeStatus;
        }
        // GET: Notice/Create
        public ActionResult Create()
        {
            GetDataInBag();
            return View();
        }

        // POST: Notice/Create
        [HttpPost]
        public ActionResult Create(NOTICE_BOARD objNotice)
        {
            try
            {
                if (string.IsNullOrEmpty(objNotice.NOTICE))
                {
                    ModelState.AddModelError("", "Please Add Some Text");
                }
                else if (string.IsNullOrEmpty(objNotice.STATUS))
                {
                    ModelState.AddModelError("", "Please Select Status");
                }
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    objNotice.ACTION_BY = userID;
                    objNotice.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.NOTICE_BOARD.Add(objNotice);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag();
            }
            return View();
        }

        // GET: Notice/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.NOTICE_BOARD.Where(x => x.ID == id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            GetDataInBag(data.STATUS);
            return View(data);
        }

        // POST: Notice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, NOTICE_BOARD objNotice)
        {
            try
            {
                if (string.IsNullOrEmpty(objNotice.NOTICE))
                {
                    ModelState.AddModelError("", "Please Add Some Text");
                }
                else if (string.IsNullOrEmpty(objNotice.STATUS))
                {
                    ModelState.AddModelError("", "Please Select Status");
                }
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    objNotice.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    objNotice.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    objNotice.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(objNotice).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag(objNotice.STATUS);
            }
            return View();
        }

        // GET: Notice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notice/Delete/5
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
