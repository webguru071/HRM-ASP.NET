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
        public ActionResult LogIn(USER_INFO user)
        {
            string userId = user.USER_ID;
            string pass = user.PASSWORD;
            if (string.IsNullOrEmpty(user.USER_ID) || string.IsNullOrEmpty(user.PASSWORD))
            {
                ModelState.AddModelError("", "Please Give Proper User Id and Password!!");
            }
            else
            {
                var data = db.USER_INFO.Where(x => x.USER_ID == userId && x.PASSWORD == pass).FirstOrDefault();
                if (data != null)
                {
                    Session["USER_ID"] = data.ID;
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    Response.Redirect("~/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "User Id and Password is Invalid!!!");
                    return View();
                }
            }
            return View();

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
