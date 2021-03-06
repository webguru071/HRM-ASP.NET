﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Helper;
namespace EMSApp.Controllers
{
    public class LeaveController : Controller
    {        
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        // GET: Leave
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.LEAVE_TYPE.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // GET: Leave/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Leave/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
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
                    long userID = converterHelper.GetLoggedUserID();
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.LEAVE_TYPE.Where(x => x.LEAVE_ID == id).FirstOrDefault();
                Session["AD"] = dt.ACTIVE_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
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
                    collection.UPDATE_BY = converterHelper.GetLoggedUserID();
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.LEAVE_TYPE.Where(x => x.LEAVE_ID == id).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: Leave/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.LEAVE_TYPE.Where(x => x.LEAVE_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.LEAVE_TYPE.Remove(dt);
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
        [HttpGet]
        public ActionResult LeaveAppIndex()
        {
            if (converterHelper.CheckLogin())
            {
                if (Session["USER_ID"] != null)
                {
                    if (converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
                    {
                        var data = db.LEAVE_APPLICATION.ToList();
                        return View(data);
                    }
                    else
                    {
                        long empId = converterHelper.GetLoggedEmployeeID(); ;
                        var data = db.LEAVE_APPLICATION.Where(x => x.EMPLOYEE_ID == empId).ToList();
                        return View(data);
                    }
                }
                else
                {
                    return RedirectToAction("LogIn","Login");
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpGet]
        public ActionResult LeaveAppAdd()
        {
            if (converterHelper.CheckLogin())
            {
                if (converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
                {
                    GetDataInBag();
                }
                else
                {
                    long empId = converterHelper.GetLoggedEmployeeID();;
                    GetDataInBag(empId: empId);
                }
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpPost]
        public ActionResult LeaveAppAdd(LEAVE_APPLICATION collection)
        {
            try
            {
                // TODO: Add insert logic here

                if (collection.LEAVE_TYPE_ID <= 0)
                {
                    ModelState.AddModelError("", "Leave Type is Required!!");
                }

                else if (string.IsNullOrEmpty(collection.START_DATE))
                {
                    ModelState.AddModelError("", "Leave Start Date is Required!!");
                }
                
                else
                {
                    bool flag = true;
                    if (Convert.ToString(Session["USER_LEVEL"]) == ConstantValue.UserLevelAdmin)
                    {
                        if (collection.EMPLOYEE_ID <= 0)
                        {
                            flag = false;
                            ModelState.AddModelError("", "Employee Name is Required!!");
                        }
                        else if (string.IsNullOrEmpty(collection.APPROVED_START_DATE))
                        {
                            flag = false;
                            ModelState.AddModelError("", "Approved Leave Start Date is Required!!");
                        }                        
                        else if (string.IsNullOrEmpty(collection.STATUS))
                        {
                            flag = false;
                            ModelState.AddModelError("", "Status is Required!!");
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        if (converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelEmployee)
                        {
                            collection.EMPLOYEE_ID = converterHelper.GetLoggedEmployeeID();
                            collection.STATUS = ConstantValue.LeaveStatusPending;
                            collection.APPROVED_START_DATE = null;
                            collection.APPROVED_END_DATE = null;
                            if (collection.LEAVE_TYPE_ID == 2)
                            {
                                collection.LEAVE_DAY = 1;
                            }
                            else if (collection.LEAVE_TYPE_ID == 3)
                            {
                                collection.LEAVE_DAY = .5;
                            }
                            else
                            {
                                TimeSpan difference = Convert.ToDateTime(collection.END_DATE).Subtract(Convert.ToDateTime(collection.START_DATE));
                                collection.LEAVE_DAY = difference.Days + 1;
                            }                           
                        }
                        if (collection.APPROVED_END_DATE != null)
                        {
                            if (collection.LEAVE_TYPE_ID == 2)
                            {
                                collection.LEAVE_DAY = 1;
                            }
                            else if (collection.LEAVE_TYPE_ID == 3)
                            {
                                collection.LEAVE_DAY = .5;
                            }
                            else
                            {
                                TimeSpan difference = Convert.ToDateTime(collection.END_DATE).Subtract(Convert.ToDateTime(collection.START_DATE));
                                collection.LEAVE_DAY = difference.Days + 1;
                            }
                        }                        
                        collection.ACTION_BY = converterHelper.GetLoggedUserID();
                        collection.ACTION_DATE = DateTime.Now;
                        if (ModelState.IsValid)
                        {
                            db.LEAVE_APPLICATION.Add(collection);
                            db.SaveChanges();
                            return RedirectToAction("LeaveAppIndex");
                        }
                    }
                }
                GetDataInBag(empId: collection.EMPLOYEE_ID);
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag(empId: collection.EMPLOYEE_ID);
                return View();
            }
        }
        [HttpGet]
        public ActionResult LeaveAppEdit(long id)
        {
            if (converterHelper.CheckLogin())
            {
                var data = db.LEAVE_APPLICATION.Where(x => x.LEAVE_APP_ID == id && x.STATUS != ConstantValue.LeaveStatusApproved).FirstOrDefault();
                //long empId = converterHelper.GetLoggedEmployeeID();
                Session["AD"] = data.ACTION_DATE;
                GetDataInBag(data.EMPLOYEE_ID, data.LEAVE_TYPE_ID);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }
        [HttpPost]
        public ActionResult LeaveAppEdit(long id, LEAVE_APPLICATION collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.LEAVE_TYPE_ID <= 0)
                {
                    ModelState.AddModelError("", "Leave Type is Required!!");
                }

                else if (string.IsNullOrEmpty(collection.START_DATE))
                {
                    ModelState.AddModelError("", "Leave Start Date is Required!!");
                }
                
                else
                {
                    bool flag = true;
                    if (Convert.ToString(Session["USER_LEVEL"]) == ConstantValue.UserLevelAdmin)
                    {
                        if (collection.EMPLOYEE_ID <= 0)
                        {
                            flag = false;
                            ModelState.AddModelError("", "Employee Name is Required!!");
                        }
                        else if (string.IsNullOrEmpty(collection.STATUS))
                        {
                            flag = false;
                            ModelState.AddModelError("", "Status is Required!!");
                        }
                        else if (string.IsNullOrEmpty(collection.APPROVED_START_DATE) && collection.STATUS != ConstantValue.LeaveStatusCanceled)
                        {
                            flag = false;
                            ModelState.AddModelError("", "Approved Leave Start Date is Required!!");
                        } 
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        if (Convert.ToString(Session["USER_LEVEL"]) == ConstantValue.UserLevelEmployee)
                        {
                            collection.EMPLOYEE_ID = converterHelper.GetLoggedEmployeeID(); 
                            collection.STATUS = ConstantValue.LeaveStatusPending;
                            collection.APPROVED_START_DATE = null;
                            collection.APPROVED_END_DATE = null; 
                            TimeSpan difference = Convert.ToDateTime(collection.END_DATE).Subtract(Convert.ToDateTime(collection.START_DATE));
                            collection.LEAVE_DAY = difference.Days + 1;
                        }
                        if (collection.APPROVED_END_DATE != null)
                        {
                            TimeSpan difference = Convert.ToDateTime(collection.APPROVED_END_DATE).Subtract(Convert.ToDateTime(collection.APPROVED_START_DATE));
                            collection.LEAVE_DAY = difference.Days + 1;
                        }
                        collection.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                        collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                        collection.UPDATE_DATE = DateTime.Now;
                        if (ModelState.IsValid)
                        {
                            db.Entry(collection).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["AD"] = null;
                            return RedirectToAction("LeaveAppIndex");
                        }
                    }
                }
                GetDataInBag(empId: collection.EMPLOYEE_ID);
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag(empId: collection.EMPLOYEE_ID);
                return View();
            }
        }
        private void GetDataInBag(long empId = 0, long leaveType = 0)
        {
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
            ViewBag.LEAVE_TYPE_ID = new SelectList(SetLeaveType(), "Value", "Text", leaveType);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        private List<SelectListItem> SetLeaveType()
        {
            List<SelectListItem> leaveTypeList = new SelectList(db.LEAVE_TYPE, "LEAVE_ID", "LEAVE_TITLE").ToList();
            leaveTypeList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return leaveTypeList;
        }
        [HttpGet]
        public ActionResult LeaveAppDelete(int id)
        {
            try
            {
                var dt = db.LEAVE_APPLICATION.Where(x => x.LEAVE_APP_ID == id).FirstOrDefault();
                return View(dt);
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }

        // POST: Leave/Delete/5
        [HttpPost]
        public ActionResult LeaveAppDelete(int id, FormCollection collection)
        {
            if (converterHelper.CheckLogin())
            {
                // TODO: Add delete logic here
                var dt = db.LEAVE_APPLICATION.Where(x => x.LEAVE_APP_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.LEAVE_APPLICATION.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("LeaveAppIndex");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
    }
}
