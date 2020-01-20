﻿using EMSApp.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class HomeController : Controller
    {
        EMSEntities db = new EMSEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(USER_INFO user)
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
            Response.Redirect("~/Home/Login");
            return View();
        }
    }
}