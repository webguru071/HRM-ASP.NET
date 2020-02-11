using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class LoginController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Login/Create
        public ActionResult LogIn()
        {
            return View();
        }
        // POST: Login/Create
        [HttpPost]
        public ActionResult LogIn(FormCollection user)
        {
            string userId = user["username"];
            string pass = user["pass"];
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pass))
            {
                ModelState.AddModelError("", "Please Give Proper User Id and Password!!");
                return View();
            }
            else
            {
                var data = db.USER_INFO.Where(x => x.USER_ID == userId && x.PASSWORD == pass).FirstOrDefault();
                if (data != null)
                {
                    var empData = db.EMPLOYEE_INFO.Where(x => x.ID == data.EMPLOYEE_ID && x.IS_DELETED == Helper.ConstantValue.TypeActive).FirstOrDefault();
                    Session["USER_ID"] = data.ID;
                    Session["USER_NAME"] = data.USER_NAME;
                    Session["USER_LEVEL"]=data.USER_LEVEL;
                    Session["EMP_ID"]=data.EMPLOYEE_ID;
                    Session["IMAGE"]=empData.IMAGE;
                    if (data.USER_LEVEL == Helper.ConstantValue.UserLevelAdmin)
                    {                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {                        
                        return RedirectToAction("Dashboard", "Home");
                    }                    
                }
                else
                {
                    ModelState.AddModelError("", "User Id and Password is Invalid!!!");
                    return View();
                }
            }            
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session["USER_ID"] = null;
            Response.Redirect("~/Login/Login");
            return View();
        }
        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Login/Delete/5
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
