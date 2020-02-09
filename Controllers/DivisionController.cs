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
    public class DivisionController : Controller
    {
        EMSEntities db = new EMSEntities(); 
        ConverterHelper converter = new ConverterHelper();
        // GET: Division
        public ActionResult Index()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.DIVISION_INFO.ToList();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // GET: Division/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Division/Create
        public ActionResult Create()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }    
        // POST: Division/Create
        [HttpPost]
        public ActionResult Create(DIVISION_INFO obj)
        {
            try
            {
                if (obj.DEPT_ID<=0)
                {
                    ModelState.AddModelError("", "Please Select Department Name");
                }
                if (string.IsNullOrEmpty(obj.DIV_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Division Name");
                }
                else
                { 
                    obj.ACTION_BY = converter.GetLoggedUserID();
                    obj.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.DIVISION_INFO.Add(obj);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
            GetDataInBag();
            return View();
        }
        // GET: Division/Edit/5
        public ActionResult Edit(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.DIVISION_INFO.Where(x => x.DIV_ID == id).FirstOrDefault();
                GetDataInBag(dt.DEPT_ID);
                Session["AD"] = dt.ACTION_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }

        // POST: Division/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DIVISION_INFO obj)
        {
            try
            {
                if (obj.DEPT_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Department Name");
                }
                if (string.IsNullOrEmpty(obj.DIV_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Division Name");
                }
                else
                {                    
                    obj.UPDATE_BY = converter.GetLoggedUserID();
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
                GetDataInBag(obj.DEPT_ID);
                return View();
            }
            GetDataInBag(obj.DEPT_ID);
            return View();
        }

        // GET: Division/Delete/5
        public ActionResult Delete(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.DIVISION_INFO.Where(x => x.DIV_ID == id).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }

        // POST: Division/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var dt = db.DIVISION_INFO.Where(x => x.DIV_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.DIVISION_INFO.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                return View();
            }
            return View();
        }
        private void GetDataInBag(long dept=0)
        {
            List<SelectListItem> deptList = new SelectList(db.DEPARTMENT_INFO, "DEPT_ID", "DEPT_TITLE").ToList();
            deptList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            ViewBag.DEPT_ID= new SelectList(deptList, "Value", "Text", dept); 
        }
    }
}
