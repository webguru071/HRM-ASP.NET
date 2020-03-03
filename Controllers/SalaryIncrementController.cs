using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Services.Position;

namespace EMSApp.Controllers
{
    public class SalaryIncrementController : Controller
    {
        EMSEntities db = new EMSEntities();
        IPosition pService = new PositionService();
        ConverterHelper converterHelper = new ConverterHelper();
        // GET: SalaryIncrement
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin())
            {
                long empId = converterHelper.GetLoggedEmployeeID();
                if (empId > 0)
                {
                    var data = db.INCREMENT_INFO.OrderByDescending(x => x.INCREMENT_FROM).ToList();
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
                else if (string.IsNullOrEmpty(collection.INCREMENT_TYPE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_CALCULATE_AS.ToString()))
                {
                    ModelState.AddModelError("", "Please Select Calculation Type!!");
                }
                else
                {
                    collection.INCREMENT_FROM = DateTime.Today.Date;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    POSITIONAL_INFO pInfo = new POSITIONAL_INFO();
                    pInfo.EMPLOYEE_ID = collection.EMP_ID;
                    var pData = db.POSITIONAL_INFO.Where(x => x.EMPLOYEE_ID == pInfo.EMPLOYEE_ID).FirstOrDefault();
                    decimal basic = pData.BASIC_SALARY;
                    collection.PREV_BASIC = basic;
                    if (collection.INCREMENT_CALCULATE_AS == ConstantValue.SalarySetupInAmount)
                    {
                        basic = basic + collection.INCREMENT_RATE;
                    }
                    else
                    {
                        decimal increment = (basic * collection.INCREMENT_RATE / 100);
                        basic = basic + increment;
                    }
                    pInfo.BASIC_SALARY = basic;
                    pInfo.UPDATE_BY= converterHelper.GetLoggedUserID();
                    pInfo.UPDATE_DATE= DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        bool result = pService.InsertIncrementInfo(incObj:collection,posObj:pInfo);
                        if(result)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to Save Increment!!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            GetDataInBg(collection.EMP_ID);
            return View(collection);
        }

        // GET: SalaryIncrement/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INCREMENT_INFO.Where(x => x.EMP_ID == id).FirstOrDefault();
                GetDataInBg(empId: data.EMP_ID, incId: data.INCREMENT_TYPE);
                Session["AD"] = data.ACTION_DATE;
                Session["PREV_BASIC"] = data.PREV_BASIC;
                ViewBag.INCREMENT_CALCULATE_AS = data.INCREMENT_CALCULATE_AS.Trim();
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
                else if (string.IsNullOrEmpty(collection.INCREMENT_TYPE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_RATE.ToString()))
                {
                    ModelState.AddModelError("", "Increment Rate is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.INCREMENT_CALCULATE_AS.ToString()))
                {
                    ModelState.AddModelError("", "Please Select Calculation Type!!");
                }
                else
                {
                    collection.INCREMENT_ID = id;
                    collection.INCREMENT_FROM = DateTime.Today.Date;
                    collection.UPDATE_BY = converterHelper.GetLoggedUserID();
                    collection.UPDATE_DATE = DateTime.Now;
                    POSITIONAL_INFO pInfo = new POSITIONAL_INFO();
                    pInfo.EMPLOYEE_ID = collection.EMP_ID;                    
                    decimal basic = Convert.ToDecimal(Session["PREV_BASIC"]);
                    if (collection.INCREMENT_CALCULATE_AS == ConstantValue.SalarySetupInAmount)
                    {
                        basic = basic + collection.INCREMENT_RATE;
                    }
                    else
                    {
                        decimal increment = (basic * collection.INCREMENT_RATE / 100);
                        basic = basic + increment;
                    }
                    pInfo.BASIC_SALARY = basic;
                    pInfo.UPDATE_BY = converterHelper.GetLoggedUserID();
                    pInfo.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        bool result = pService.UpdateIncrementInfo(incObj: collection, posObj: pInfo);
                        if (result)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to Update Increment!!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            GetDataInBg(collection.EMP_ID);
            return View(collection);
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
