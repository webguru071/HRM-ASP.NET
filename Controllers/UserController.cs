using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class UserController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: User
        public ActionResult Index()
        {
            var data = db.USER_INFO.Where(x => x.IS_DELETED == "a").ToList();
            return View(data);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        private List<SelectListItem> SetStatusList()
        {
            var activeStatus = new List<SelectListItem>
            {
                new SelectListItem{ Text="Active", Value = "a" },
                new SelectListItem{ Text="Deactive", Value = "d" }
            };
            activeStatus.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return activeStatus;
        }
        private List<SelectListItem> SetUserLevel()
        {
            var userLevel = new List<SelectListItem>
            {
                new SelectListItem{ Text="Admin", Value = "a" },
                new SelectListItem{ Text="Employee", Value = "e" }
            };
            userLevel.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return userLevel;
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        // GET: User/Create
        public ActionResult Create()
        {
            GetDataInBag();
            return View();
        }
        private void GetDataInBag()
        {
            ViewBag.EMPLOYEE_ID = SetEmployee();
            ViewBag.USER_LEVEL = SetUserLevel();
            ViewBag.IS_DELETED = SetStatusList();
        }
        // POST: User/Create
        [HttpPost]
        public ActionResult Create(USER_INFO user)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(user.USER_NAME))
                {
                    ModelState.AddModelError("", "User Name is Required!!");
                }
                else if (string.IsNullOrEmpty(user.USER_ID))
                {
                    ModelState.AddModelError("", "User Id is Required!!");
                }
                else if (string.IsNullOrEmpty(user.PASSWORD))
                {
                    ModelState.AddModelError("", "Password is Required!!");
                }
                else
                {
                    int count = 0;
                    count = db.USER_INFO.Where(x => x.USER_ID == user.USER_ID).Count();
                    if (count == 0)
                    {
                        if (user.ACTION_BY.ToString() != null)
                        {
                            user.ACTION_BY = Convert.ToInt64(Session["ID"]);
                        }
                        user.ACTION_DATE = DateTime.Now;
                        if (user.EMPLOYEE_ID == 0)
                        {
                            user.EMPLOYEE_ID = null;
                        }
                        if (ModelState.IsValid)
                        {
                            db.USER_INFO.Add(user);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Id is Already Created!!");
                    }
                }
                GetDataInBag();
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.USER_INFO.Where(x => x.ID == id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            Session["AT"] = data.ACTION_BY;
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", data.EMPLOYEE_ID);
            ViewBag.USER_LEVEL = new SelectList(SetUserLevel(), "Value", "Text", data.USER_LEVEL);
            ViewBag.IS_DELETED = new SelectList(SetStatusList(), "Value", "Text", data.IS_DELETED);
            return View(data);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, USER_INFO user)
        {
            try
            {
                // TODO: Add update logic here
                if (string.IsNullOrEmpty(user.USER_NAME))
                {
                    ModelState.AddModelError("", "User Name is Required!!");
                }
                else if (string.IsNullOrEmpty(user.USER_ID))
                {
                    ModelState.AddModelError("", "User Id is Required!!");
                }
                else if (string.IsNullOrEmpty(user.PASSWORD))
                {
                    ModelState.AddModelError("", "Password is Required!!");
                }
                else
                {
                    int count = 0;
                    count = db.USER_INFO.Where(x => x.USER_ID == user.USER_ID).Count();
                    if (count == 0)
                    {
                        user.UPDATE_BY = Convert.ToInt64(Session["ID"]);
                        user.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                        user.UPDATE_DATE = DateTime.Now;
                        if (user.EMPLOYEE_ID == 0)
                        {
                            user.EMPLOYEE_ID = null;
                        }
                        if (ModelState.IsValid)
                        {
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["AD"] = null;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Id is Already Created!!");
                    }

                }
                GetDataInBag();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
        public JsonResult GetEmpInfo(long id)
        {
            var data = db.EMPLOYEE_INFO.Where(x => x.ID == id).FirstOrDefault();
            Dictionary<string, string> listOfData = new Dictionary<string, string>();
            if (data != null)
            {
                listOfData["EMPLOYEE_NAME"] = data.EMPLOYEE_NAME;
                listOfData["CONTACT"] = data.CONTACT;
            }
            else
            {
                listOfData["EMPLOYEE_NAME"] = "";
                listOfData["CONTACT"] = "";
            }
            return Json(listOfData, JsonRequestBehavior.AllowGet);
        }
    }
}
