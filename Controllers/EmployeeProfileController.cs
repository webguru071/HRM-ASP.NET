﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class EmployeeProfileController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        ConverterHelper converter = new ConverterHelper();

        // GET: EmployeeProfile
        public ActionResult Index()
        {
            if (converter.CheckLogin())
            {
                long empId = converter.GetLoggedEmployeeID();
                if (empId > 0)
                {
                    var data = db.EMPLOYEE_INFO.Where(x => x.ID == empId && x.IS_DELETED == Helper.ConstantValue.UserStatusActive).FirstOrDefault();
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
        // GET: EmployeeProfile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: EmployeeProfile/Create
        public ActionResult Create()
        {
            if (converter.CheckLogin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
          
        }
        // POST: EmployeeProfile/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: EmployeeProfile/Edit/5
        public ActionResult Edit(int id)
        {
            if (converter.CheckLogin())
            {
                long empId = converter.GetLoggedEmployeeID();
                if (empId == id)
                {
                    var data = db.EMPLOYEE_INFO.Where(x => x.IS_DELETED == Helper.ConstantValue.UserStatusActive && x.ID == empId).FirstOrDefault();
                    Session["AD"] = data.ACTION_DATE;
                    Session["NAME"] = data.EMPLOYEE_NAME;
                    Session["data"] = data;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: EmployeeProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EMPLOYEE_INFO emp)
        {
            try
            {
                // TODO: Add update logic here
                emp.EMPLOYEE_NAME = converter.GetLoggedEmployeeName();
                emp.IS_DELETED = Convert.ToString(Session["ISD"]);
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
            catch(Exception ex)
            {
                return View();
            }
            return View();
        }

        // GET: EmployeeProfile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: EmployeeProfile/Delete/5
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
