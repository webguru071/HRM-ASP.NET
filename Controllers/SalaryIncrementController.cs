using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class SalaryIncrementController : Controller
    {
        EMSEntities db = new EMSEntities();
        //ICombine service = new CombineServices();
        ConverterHelper converterHelper = new ConverterHelper();
        // GET: SalaryIncrement
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin())
            {
                long empId = converterHelper.GetLoggedEmployeeID();
                if (empId > 0)
                {
                    var data = db.INCREMENT_INFO.OrderByDescending(x=>x.INCREMENT_FROM).ToList();
                    return View(data);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // GET: SalaryIncrement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalaryIncrement/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                GetDataInBg();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }        
        // POST: SalaryIncrement/Create
        [HttpPost]
        public ActionResult Create(INCREMENT_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.EMP_ID.ToString()))
                {
                    ModelState.AddModelError("", "Please Select Employee!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else
                {
                    collection.INCREMENT_FROM= DateTime.Today.Date;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.INCREMENT_INFO.Add(collection);
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

        // GET: SalaryIncrement/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INCREMENT_INFO.Where(x => x.EMP_ID == id).FirstOrDefault();
                GetDataInBg(empId: data.EMP_ID, incId: data.INCREMENT_TYPE);
                Session["AD"] = data.ACTION_DATE;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: SalaryIncrement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, INCREMENT_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.EMP_ID.ToString()))
                {
                    ModelState.AddModelError("", "Please Select Employee!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else
                {
                    //Increment Calculation

                    collection.INCREMENT_FROM = DateTime.Today.Date;
                    collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    collection.UPDATE_BY = converterHelper.GetLoggedUserID();
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
            catch (Exception ex)
            {
            }
            return View();
        }

        // GET: SalaryIncrement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalaryIncrement/Delete/5
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
        private void GetDataInBg(long empId = 0, long incId = 0)
        {
            ViewBag.EMP_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
            ViewBag.INCREMENT_TYPE = new SelectList(SetIncrementType(), "Value", "Text", incId);
        }

        private IEnumerable SetIncrementType()
        {
            List<SelectListItem> list = new SelectList(db.INCREMENT_TYPE_INFO, "INCREMENT_TYPE_ID", "INCREMENT_TYPE_TITLE").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return list;
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return empList;
        }
    }
}
