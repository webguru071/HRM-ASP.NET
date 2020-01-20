using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EMSApp.Services.Position;

namespace EMSApp.Controllers
{
    public class PositionController : Controller
    {
        EMSEntities db = new EMSEntities();
        IPosition ins = new PositionService();
        // GET: Position
        public ActionResult Index()
        {
            var data = db.POSITIONAL_INFO.ToList();
            return View(data);
        }

        // GET: Position/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            GetDataInBag();
            return View();
        }

        // POST: Position/Create
        [HttpPost]
        public ActionResult Create(POSITIONAL_INFO obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Employee Name");
                }
                else if (obj.DIV_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Division Name");
                }
                else if (string.IsNullOrEmpty(obj.POSITION_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Position Name");
                }
                else if (string.IsNullOrEmpty(obj.DUTY_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Duty Type");
                }
                else if (string.IsNullOrEmpty(obj.RATE_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Rate Type");
                }
                else if (string.IsNullOrEmpty(obj.PAY_FREQ))
                {
                    ModelState.AddModelError("", "Please Select Payment Frequency Type");
                }
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    obj.ACTION_BY = userID;
                    obj.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.POSITIONAL_INFO.Add(obj);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                GetDataInBag();
                return View();
            }
            GetDataInBag();
            return View();
        }

        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            var dt = db.POSITIONAL_INFO.Where(x => x.POSITION_ID == id).FirstOrDefault();
            GetDataInBag(dt.EMPLOYEE_ID,dt.DIV_ID);
            Session["AD"] = dt.ACTION_DATE;
            return View(dt);
        }

        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, POSITIONAL_INFO obj )
        {
            try
            {
                // TODO: Add update logic here
                if (obj.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Employee Name");
                }
                else if (obj.DIV_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Division Name");
                }
                else if (string.IsNullOrEmpty(obj.POSITION_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Position Name");
                }
                else if (string.IsNullOrEmpty(obj.DUTY_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Duty Type");
                }
                else if (string.IsNullOrEmpty(obj.RATE_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Rate Type");
                }
                else if (string.IsNullOrEmpty(obj.PAY_FREQ))
                {
                    ModelState.AddModelError("", "Please Select Payment Frequency Type");
                }
                else
                {                    
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
            catch(Exception ex)
            {
              
            }
            GetDataInBag(obj.EMPLOYEE_ID, obj.DIV_ID);
            return View();
        }

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Position/Delete/5
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
        private void GetDataInBag(long empId = 0, long divId = 0)
        {
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
            ViewBag.DIV_ID = new SelectList(SetDiv(), "Value", "Text", divId);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
       
        private List<SelectListItem> SetDiv()
        {
            List<SelectListItem> divtList = new SelectList(db.DIVISION_INFO, "DIV_ID", "DIV_TITLE").ToList();
            divtList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return divtList;
        }
    }
}
