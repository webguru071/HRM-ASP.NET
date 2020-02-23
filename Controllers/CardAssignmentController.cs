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
    public class CardAssignmentController : Controller
    {
        ConverterHelper converterHelper = new ConverterHelper();
        EMSEntities db = new EMSEntities();
        // GET: CardAssignment
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.CARD_ASSIGN_INFO.OrderByDescending(x=>x.ACTION_DATE).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // GET: CardAssignment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CardAssignment/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: CardAssignment/Create
        [HttpPost]
        public ActionResult Create(CARD_ASSIGN_INFO collection)
        {
            try
            {
                if (collection.EMP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Employee!!!");
                }

                else if (string.IsNullOrEmpty(collection.CARD_NO))
                {
                    ModelState.AddModelError("", "Card No is Required!!!");
                }
                else
                {
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.CARD_ASSIGN_INFO.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }                   
                }
            }
            catch(Exception ex)
            {
                
            }
            GetDataInBag();
            return View(collection);
        }

        // GET: CardAssignment/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin())
            {
                var data = db.CARD_ASSIGN_INFO.Where(x => x.CARD_ASSGN_ID == id ).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                GetDataInBag(data.EMP_ID);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: CardAssignment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CARD_ASSIGN_INFO collection)
        {
            try
            {
                if (collection.EMP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Employee!!!");
                }

                else if (string.IsNullOrEmpty(collection.CARD_NO))
                {
                    ModelState.AddModelError("", "Card No is Required!!!");
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
            catch (Exception ex)
            {

            }
            GetDataInBag(collection.EMP_ID);
            return View(collection);
        }

        // GET: CardAssignment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CardAssignment/Delete/5
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
        private void GetDataInBag(long empId = 0)
        {
            ViewBag.EMP_ID = new SelectList(SetEmployee(), "Value", "Text", empId);            
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return empList;
        }
    }
}
