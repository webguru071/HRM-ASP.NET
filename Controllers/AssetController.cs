using EMSApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class AssetController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Asset
        public ActionResult Index()
        {
            var data = db.ASSET_INFO.ToList();
            return View(data);
        }
        // GET: Asset/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Asset/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asset/Create
        [HttpPost]
        public ActionResult Create(ASSET_INFO collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection.ASSET_TITLE))
                {
                    ModelState.AddModelError("", "Asset Type Name is Required!!");
                }
                else
                {
                    collection.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.ASSET_INFO.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        // GET: Asset/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.ASSET_INFO.Where(x=>x.ASSET_ID==id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            return View(data);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ASSET_INFO collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection.ASSET_TITLE))
                {
                    ModelState.AddModelError("", "Asset Type Name is Required!!");
                }
                else
                {
                    collection.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
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
                return View();
            }
            return View();
        }

        // GET: Asset/Delete/5
        public ActionResult Delete(int id)
        {
            var data = db.ASSET_INFO.Where(x => x.ASSET_ID == id).FirstOrDefault();
            return View(data);
        }

        // POST: Asset/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ASSET_INFO collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.ASSET_INFO.Where(x => x.ASSET_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.ASSET_INFO.Remove(dt);
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
    }
}
