using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class PositionController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();

        // GET: Position
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.POSITIONAL_INFO.Where(x => x.STATUS == ConstantValue.TypeActive).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        // GET: Position/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Position/Create
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
        // POST: Position/Create
        [HttpPost]
        public ActionResult Create(POSITIONAL_INFO obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Employee Name!!!");
                }
                else if (obj.DIV_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Division Name!!!");
                }
                else if (string.IsNullOrEmpty(obj.POSITION_TITLE))
                {
                    ModelState.AddModelError("", "Please Enter Job Post!!!");
                }
                else if (obj.BASIC_SALARY <= 0)
                {
                    ModelState.AddModelError("", "Please Enter Basic Salary!!!");
                }
                else if (string.IsNullOrEmpty(obj.DUTY_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Job Type!!!");
                }
                else if (string.IsNullOrEmpty(obj.RATE_TYPE))
                {
                    ModelState.AddModelError("", "Please Select Salary Count!!!");
                }
                else if (string.IsNullOrEmpty(obj.WORKING_SHIFT.ToString()))
                {
                    ModelState.AddModelError("", "Work Shift is Required!!!");
                }
                else if (Convert.ToInt32(obj.WORKING_HOURS)<=0)
                {
                    ModelState.AddModelError("", "Total Working Hour is Required!!!");
                }
                else if (string.IsNullOrEmpty(obj.PAY_FREQ))
                {
                    ModelState.AddModelError("", "Please Select Payment Frequency Type");
                }
                else
                {
                    long userID = converterHelper.GetLoggedUserID();
                    obj.ACTION_BY = userID;
                    obj.ACTION_DATE = DateTime.Now;
                    obj.STATUS = ConstantValue.TypeActive;
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var dt = db.POSITIONAL_INFO.Where(x => x.POSITION_ID == id).FirstOrDefault();
                Session["DATA"] = dt;
                GetDataInBag(dt.EMPLOYEE_ID, dt.DIV_ID);
                Session["AD"] = dt.ACTION_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, POSITIONAL_INFO obj)
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
                else if (obj.BASIC_SALARY <= 0)
                {
                    ModelState.AddModelError("", "Please Enter Basic Salary");
                }
                else
                {
                    //obj.STATUS = ConstantValue.TypeActive;
                    obj.UPDATE_BY = converterHelper.GetLoggedUserID();
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
            GetDataInBag(obj.EMPLOYEE_ID, obj.DIV_ID);
            return View();
        }

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var dt = db.POSITIONAL_INFO.Where(x => x.POSITION_ID == id).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }

        }
        // POST: Position/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, POSITIONAL_INFO collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.POSITIONAL_INFO.Where(x => x.POSITION_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.POSITIONAL_INFO.Remove(dt);
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
        private void GetDataInBag(long empId = 0, long divId = 0, long deptId = 0)
        {
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
            ViewBag.DIV_ID = new SelectList(SetDiv(), "Value", "Text", divId);
            if (divId != 0)
            {
                var data = db.DIVISION_INFO.Where(x => x.DIV_ID == divId).FirstOrDefault();
                deptId = data.DEPT_ID;
            }
            ViewBag.DEPT_ID = new SelectList(SetDept(), "Value", "Text", deptId);
            TimeSpan ts = new TimeSpan(12, 0, 0);
            ViewBag.WORKING_SHIFT = ts;
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
        private List<SelectListItem> SetDept()
        {
            List<SelectListItem> divtList = new SelectList(db.DEPARTMENT_INFO, "DEPT_ID", "DEPT_TITLE").ToList();
            divtList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return divtList;
        }
    }
}
