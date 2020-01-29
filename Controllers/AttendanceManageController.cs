using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;

namespace EMSApp.Controllers
{
    public class AttendanceManageController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: AttendanceManage
        public ActionResult Index()
        {
            long empId = Convert.ToInt64(Session["EMP_ID"]);
            var data = db.ATTENDANCE_DETAILS.Where(x=>x.EMPLOYEE_ID== empId).OrderBy(x => x.ATT_DATE).ToList();
            return View(data);
        }

        // GET: AttendanceManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceManage/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: AttendanceManage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection["STATUS"]))
                {
                    ModelState.AddModelError("", "Attendance Status is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["pass"]))
                {
                    ModelState.AddModelError("", "Give Password for Confirmation!!");
                }               
                else
                {
                    string pass = collection["pass"];
                    long userId = Convert.ToInt64(Session["USER_ID"]);
                    var data = db.USER_INFO.Where(x => x.ID == userId).FirstOrDefault();
                    if (data.PASSWORD != pass)
                    {
                        ModelState.AddModelError("", "Password is Not Matched!!");
                        return View();
                    }
                    else
                    {
                        ATTENDANCE_DETAILS attn = new ATTENDANCE_DETAILS();
                        attn.EMPLOYEE_ID = Convert.ToInt64(data.EMPLOYEE_ID);
                        attn.ATT_DATE = DateTime.Today.Date;
                        attn.STATUS = Convert.ToString(collection["STATUS"]);
                        int count = db.ATTENDANCE_DETAILS.Where(x => x.EMPLOYEE_ID == attn.EMPLOYEE_ID && x.STATUS == attn.STATUS && x.ATT_DATE==attn.ATT_DATE).Count();
                        attn.SL_NO = count +1;
                        if (Convert.ToString(collection["STATUS"]) == Helper.ConstantValue.AttendanceCheckIn)
                        {
                            attn.CHECK_IN_TIME = DateTime.Now.TimeOfDay;                            
                        }
                        else if (Convert.ToString(collection["STATUS"]) == Helper.ConstantValue.AttendanceCheckOut)
                        {
                            attn.CHECK_OUT_TIME = DateTime.Now.TimeOfDay;
                        }
                        else
                        {

                        }
                        if (ModelState.IsValid)
                        {
                            db.ATTENDANCE_DETAILS.Add(attn);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
               
                return View();
            }
            catch (Exception ex)
            {
                
                return View();
            }
        }
          
        // GET: AttendanceManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceManage/Edit/5
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

        // GET: AttendanceManage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceManage/Delete/5
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
