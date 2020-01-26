using EMSApp.Models;
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
    }
}