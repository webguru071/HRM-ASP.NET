using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class EquipmentManageController : Controller
    {
        // GET: EquipmentManage
        public ActionResult Index()
        {
            return View();
        }

        // GET: EquipmentManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EquipmentManage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentManage/Create
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

        // GET: EquipmentManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EquipmentManage/Edit/5
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

        // GET: EquipmentManage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipmentManage/Delete/5
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
