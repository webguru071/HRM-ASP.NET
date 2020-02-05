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
            //Session["USER_LEVEL"] = Helper.ConstantValue.UserLevelAdmin;
            //string name = Session["USER_NAME"].ToString();
            var leaveApp = db.LEAVE_APPLICATION.Where(x => x.STATUS == Helper.ConstantValue.LeaveStatusPending).ToList();
            return View(leaveApp);            
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
           // Session["USER_LEVEL"] = Helper.ConstantValue.UserLevelEmployee;
            var notific = db.NOTICE_BOARD.Where(x => x.STATUS == Helper.ConstantValue.TypeActive).OrderByDescending(x=>x.ACTION_DATE).ToList();
            return View(notific);
        }
    }
}