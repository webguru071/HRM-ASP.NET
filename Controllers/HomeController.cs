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
            //var data=db.EMPLOYEE_APPLICATION.Where(x=>x.)
            if(Convert.ToString(Session["USER_LEVEL"]) == Helper.ConstantValue.UserLevelAdmin)
            {
                var leaveApp = db.LEAVE_APPLICATION.Where(x => x.STATUS == Helper.ConstantValue.LeaveStatusPending).ToList();
                return View(leaveApp);
            }
            else
            {
                Response.Redirect("~/Home/Dashboard");
                return View();
            }           
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            var notific = db.NOTICE_BOARD.Where(x => x.STATUS == Helper.ConstantValue.TypeActive).ToList();
            return View(notific);
        }
    }
}