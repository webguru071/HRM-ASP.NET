using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class SalaryIncrementTypeController : Controller
    {
        EMSEntities db = new EMSEntities();
        //ICombine service = new CombineServices();
        ConverterHelper converterHelper = new ConverterHelper();
        // GET: SalaryIncrementType
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INCREMENT_TYPE_INFO.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }

        // GET: SalaryIncrementType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalaryIncrementType/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: SalaryIncrementType/Create
        [HttpPost]
        public ActionResult Create(INCREMENT_TYPE_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.INCREMENT_TYPE_TITLE))
                {
                    ModelState.AddModelError("", "Increment Type is Required!!");
                }
                else
                {
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.INCREMENT_TYPE_INFO.Add(collection);
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

        // GET: SalaryIncrementType/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INCREMENT_TYPE_INFO.Where(x => x.INCREMENT_TYPE_ID == id).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: SalaryIncrementType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, INCREMENT_TYPE_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.INCREMENT_TYPE_TITLE))
                {
                    ModelState.AddModelError("", "Increment Type is Required!!");
                }
                else
                {
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

        // GET: SalaryIncrementType/Delete/5
        public ActionResult Delete(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INCREMENT_TYPE_INFO.Where(x => x.INCREMENT_TYPE_ID == id).FirstOrDefault();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: SalaryIncrementType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.INCREMENT_TYPE_INFO.Where(x => x.INCREMENT_TYPE_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.INCREMENT_TYPE_INFO.Remove(dt);
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
