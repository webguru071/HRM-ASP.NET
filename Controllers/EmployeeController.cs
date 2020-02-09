using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMSApp.Models;
using System.Web.Mvc;
using System.Data.Entity;
using EMSApp.Helper;

namespace EMSApp.Controllers
{
    public class EmployeeController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converter = new ConverterHelper();

        // GET: Employee
        public ActionResult Index()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.EMPLOYEE_INFO.Where(x => x.IS_DELETED == ConstantValue.TypeActive).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Employee/Create
        public ActionResult Create()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                ViewBag.IS_DELETED = SetStatusList();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EMPLOYEE_INFO emp)
        {
            try
            {
                if (string.IsNullOrEmpty(emp.EMPLOYEE_NAME))
                {
                    ModelState.AddModelError("", "Employee Name Required!!");
                }
                else if (string.IsNullOrEmpty(emp.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.GENDER))
                {
                    ModelState.AddModelError("", "Gender is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.DOB))
                {
                    ModelState.AddModelError("", "Date of Birth is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.MARITALA_STATUS))
                {
                    ModelState.AddModelError("", "Marital Status is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.IS_DELETED))
                {
                    ModelState.AddModelError("", "Employee Status is Required!!");
                }
                else
                {
                    // TODO: Add insert logic here

                    emp.ACTION_BY = converter.GetLoggedUserID();
                    emp.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.EMPLOYEE_INFO.Add(emp);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.IS_DELETED = SetStatusList();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.IS_DELETED = SetStatusList();
                return View();
            }
        }
        //Set active status
        private List<SelectListItem> SetStatusList()
        {
            var activeStatus = new List<SelectListItem>
            {
                new SelectListItem{ Text="Active", Value = Helper.ConstantValue.TypeActive },
                new SelectListItem{ Text="Deactive", Value = Helper.ConstantValue.TypeDeactive }
            };
            activeStatus.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return activeStatus;
        }
        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.EMPLOYEE_INFO.Where(x => x.ID == id).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                //Session["AT"] = data.ACTION_BY;
                ViewBag.IS_DELETED = new SelectList(SetStatusList(), "Value", "Text", data.IS_DELETED);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EMPLOYEE_INFO emp)
        {
            try
            {
                if (string.IsNullOrEmpty(emp.EMPLOYEE_NAME))
                {
                    ModelState.AddModelError("", "Employee Name Required!!");
                }
                else if (string.IsNullOrEmpty(emp.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.GENDER))
                {
                    ModelState.AddModelError("", "Gender is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.DOB))
                {
                    ModelState.AddModelError("", "Date of Birth is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.MARITALA_STATUS))
                {
                    ModelState.AddModelError("", "Marital Status is Required!!");
                }
                else if (string.IsNullOrEmpty(emp.IS_DELETED))
                {
                    ModelState.AddModelError("", "Employee Status is Required!!");
                }
                else
                {
                    // TODO: Add insert logic here

                    emp.UPDATE_BY = converter.GetLoggedUserID();
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
                ViewBag.IS_DELETED = SetStatusList();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.IS_DELETED = SetStatusList();
                return View();
            }
        }
        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Employee/Delete/5
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
