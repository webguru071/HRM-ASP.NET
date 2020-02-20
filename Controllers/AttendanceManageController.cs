using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class AttendanceManageController : Controller
    {
        ConverterHelper converterHelper = new ConverterHelper();
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        // GET: AttendanceManage
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin())
            {
                long empId = converterHelper.GetLoggedEmployeeID();
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                var data = service.GetAttendanceData(empId: empId,toDate:date);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }           
        }

        // GET: AttendanceManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceManage/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }

        // POST: AttendanceManage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here                
                if (string.IsNullOrEmpty(collection["checkIn"]) && string.IsNullOrEmpty(collection["checkOut"]))
                {
                    ModelState.AddModelError("", "Attendance Status is Required!!");
                }
                if (!string.IsNullOrEmpty(collection["checkIn"]) && !string.IsNullOrEmpty(collection["checkOut"]))
                {
                    ModelState.AddModelError("", "Select Only One Status!!");
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
                        return View(collection);
                    }
                    else
                    {
                        ATTENDANCE_DETAILS attn = new ATTENDANCE_DETAILS();
                        attn.EMPLOYEE_ID = Convert.ToInt64(data.EMPLOYEE_ID);
                        attn.ATT_DATE = DateTime.Today.Date;
                        attn.STATUS = (string.IsNullOrEmpty(collection["checkIn"])) ? Helper.ConstantValue.AttendanceCheckOut : Helper.ConstantValue.AttendanceCheckIn;
                        int count = db.ATTENDANCE_DETAILS.Where(x => x.EMPLOYEE_ID == attn.EMPLOYEE_ID && x.ATT_DATE == attn.ATT_DATE).Count();
                        attn.SL_NO = count + 1;
                        if (!string.IsNullOrEmpty(collection["checkIn"]))
                        {
                            attn.CHECK_IN_TIME = DateTime.Now.TimeOfDay;
                        }
                        else if (!string.IsNullOrEmpty(collection["checkOut"]))
                        {
                            attn.CHECK_OUT_TIME = DateTime.Now.TimeOfDay;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Give Proper Information");
                            return View(collection);
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
