using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EmployeeDemogReport()
        {
            return View();
        }
    }    
}