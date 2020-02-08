﻿using EMSApp.Helper;
using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class HomeController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converter = new ConverterHelper();
        public ActionResult Index()
        {
            //Session["USER_LEVEL"] = Helper.ConstantValue.UserLevelAdmin;
            //string name = Session["USER_NAME"].ToString();
            var leaveApp = db.LEAVE_APPLICATION.Where(x => x.STATUS == ConstantValue.LeaveStatusPending).ToList();
            return View(leaveApp);            
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            // Session["USER_LEVEL"] = Helper.ConstantValue.UserLevelEmployee;
            long empId = converter.GetLoggedEmployeeID();
            var empPosition = db.POSITIONAL_INFO.Where(x => x.EMPLOYEE_ID == empId).FirstOrDefault();
            var empdivInfo = db.DIVISION_INFO.Where(x => x.DIV_ID == empPosition.DIV_ID).FirstOrDefault();
            var data = db.NOTICE_BOARD.Where(X => X.STATUS == ConstantValue.TypeActive).ToList();
            List<NOTICE_BOARD> permittedList = new List<NOTICE_BOARD>();
            foreach (var dt in data)
            {
                if (dt.DEPT_ID > 0)
                {
                    if (dt.DIV_ID > 0)
                    {
                        if (dt.DIV_ID == empPosition.DIV_ID)
                        {
                            permittedList.Add(dt);
                        }
                    }
                    else
                    {
                        if (dt.DEPT_ID == empdivInfo.DEPT_ID)
                        {
                            permittedList.Add(dt);
                        }
                    }
                }
                else
                {
                    permittedList.Add(dt);
                }
            }
            return View(permittedList);             
        }
    }
}