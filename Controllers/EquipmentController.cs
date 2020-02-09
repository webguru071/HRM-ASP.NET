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
    public class EquipmentController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converter = new ConverterHelper();

        // GET: Equipment
        public ActionResult Index()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.EQUEPMENTS_INFO.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // GET: Equipment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Equipment/Create
        public ActionResult Create()
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // POST: Equipment/Create
        [HttpPost]
        public ActionResult Create(EQUEPMENTS_INFO collection)
        {
            try
            {
                if (collection.ASSET_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Asset Type");
                }
                if (string.IsNullOrEmpty(collection.EQP_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Equipment Title");
                }
                if (string.IsNullOrEmpty(collection.MODEL_NO))
                {
                    ModelState.AddModelError("", "Please Add Equipment Model");
                }
                else
                {
                    collection.ACTION_BY = converter.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.EQUEPMENTS_INFO.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
            GetDataInBag();
            return View();
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.EQUEPMENTS_INFO.Where(x => x.EQUEPMENT_ID == id).FirstOrDefault();
                GetDataInBag(dt.ASSET_ID);
                Session["AD"] = dt.ACTION_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EQUEPMENTS_INFO collection)
        {
            try
            {
                if (collection.ASSET_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Asset Type");
                }
                if (string.IsNullOrEmpty(collection.EQP_TITLE))
                {
                    ModelState.AddModelError("", "Please Add Equipment Title");
                }
                if (string.IsNullOrEmpty(collection.MODEL_NO))
                {
                    ModelState.AddModelError("", "Please Add Equipment Model");
                }
                else
                {
                    collection.UPDATE_BY = converter.GetLoggedUserID();
                    collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    collection.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag(collection.ASSET_ID);
                return View();
            }
            GetDataInBag(collection.ASSET_ID);
            return View();
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int id)
        {
            if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.EQUEPMENTS_INFO.Where(x => x.EQUEPMENT_ID == id).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // POST: Equipment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, EQUEPMENTS_INFO collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.EQUEPMENTS_INFO.Where(x => x.EQUEPMENT_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.EQUEPMENTS_INFO.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }
        private void GetDataInBag(long assetId = 0)
        {
            List<SelectListItem> list = new SelectList(db.ASSET_INFO, "ASSET_ID", "ASSET_TITLE").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            ViewBag.ASSET_ID = new SelectList(list, "Value", "Text", assetId);
        }
    }
}
