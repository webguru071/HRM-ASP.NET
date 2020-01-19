using EMSApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class PositionController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Position
        public ActionResult Index()
        {
            var data = db.POSITIONAL_INFO.ToList();
            return View(data);
        }

        // GET: Position/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            GetDataInBag();
            return View();
        }

        // POST: Position/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Position/Edit/5
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

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Position/Delete/5
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
        private void GetDataInBag(long empId = 0, long divId = 0)
        {
            ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
            ViewBag.DIV_ID = new SelectList(SetDiv(), "Value", "Text", divId);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
       
        private List<SelectListItem> SetDiv()
        {
            List<SelectListItem> divtList = new SelectList(db.DIVISION_INFO, "DIV_ID", "DIV_TITLE").ToList();
            divtList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return divtList;
        }
    }
}
