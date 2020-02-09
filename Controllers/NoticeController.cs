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
    public class NoticeController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converter = new ConverterHelper();

        // GET: Notice
        public ActionResult Index()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.NOTICE_BOARD.Where(X => X.STATUS == "a").ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // GET: Notice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notice/Create
        public ActionResult Create()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: Notice/Create
        [HttpPost]
        public ActionResult Create(NOTICE_BOARD objNotice)
        {
            try
            {
                if (string.IsNullOrEmpty(objNotice.NOTICE))
                {
                    ModelState.AddModelError("", "Please Add Some Text");
                }
                else if (string.IsNullOrEmpty(objNotice.STATUS))
                {
                    ModelState.AddModelError("", "Please Select Status");
                }
                else
                {
                    long userID = converter.GetLoggedUserID();
                    objNotice.ACTION_BY = userID;
                    objNotice.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.NOTICE_BOARD.Add(objNotice);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            GetDataInBag(objNotice.STATUS, Convert.ToInt64(objNotice.DEPT_ID), Convert.ToInt64(objNotice.DIV_ID));
            return View(objNotice);
        }
        // GET: Notice/Edit/5
        public ActionResult Edit(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.NOTICE_BOARD.Where(x => x.ID == id).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                GetDataInBag(data.STATUS, dept: Convert.ToInt64(data.DEPT_ID), div: Convert.ToInt64(data.DIV_ID));
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }           
        }
        // POST: Notice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, NOTICE_BOARD objNotice)
        {
            try
            {
                if (string.IsNullOrEmpty(objNotice.NOTICE))
                {
                    ModelState.AddModelError("", "Please Add Some Text");
                }
                else if (string.IsNullOrEmpty(objNotice.STATUS))
                {
                    ModelState.AddModelError("", "Please Select Status");
                }
                else
                {
                    objNotice.UPDATE_BY = converter.GetLoggedUserID();
                    objNotice.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    objNotice.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(objNotice).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            GetDataInBag(objNotice.STATUS, Convert.ToInt64(objNotice.DEPT_ID), Convert.ToInt64(objNotice.DIV_ID));
            return View();
        }
        // GET: Notice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Notice/Delete/5
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
        private void GetDataInBag(string status = "", long dept = 0, long div = 0)
        {
            List<SelectListItem> deptList = new SelectList(db.DEPARTMENT_INFO, "DEPT_ID", "DEPT_TITLE").ToList();
            deptList.Insert(0, (new SelectListItem { Text = "All Department", Value = "0" }));
            ViewBag.DEPT_ID = new SelectList(deptList, "Value", "Text", dept);
            var iniText = new List<SelectListItem>
            {
                new SelectListItem{ Text="Select One", Value = ""}
            };
            if (dept > 0)
            {
                List<SelectListItem> division = new SelectList(db.DIVISION_INFO.Where(x => x.DEPT_ID == dept), "DIV_ID", "DIV_TITLE").ToList();
                division.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
                ViewBag.DIV_ID = new SelectList(division, "Value", "Text", div);
                //ViewBag.TEAM_ID = iniText;
            }
            else
            {
                ViewBag.DIV_ID = iniText;
                // ViewBag.TEAM_ID = iniText;
            }
            ViewBag.STATUS = new SelectList(SetStatusList(), "Value", "Text", status);
        }
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
        public JsonResult GetDivision(int deptId)
        {
            List<SelectListItem> division = new SelectList(db.DIVISION_INFO.Where(x => x.DEPT_ID == deptId), "DIV_ID", "DIV_TITLE").ToList();
            return Json(new SelectList(division, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTeam(int divId)
        {
            List<SelectListItem> division = new SelectList(db.TEAM_INFO, "DIV_ID", "DIV_TITLE").ToList();
            return Json(new SelectList(division, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
    }
}
