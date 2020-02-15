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
    public class VendorController : Controller
    {
        // GET: Vendor
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        [HttpGet]
        public ActionResult SupplierIndex()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.VENDOR_INFO.Where(x => x.VENDOR_TYPE == ConstantValue.VendorTypeSupplier).OrderBy(x => x.STATUS).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        // GET: Vendor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Vendor/Create
        [HttpGet]
        public ActionResult CreateSupplier()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Vendor/Create
        [HttpPost]
        public ActionResult CreateSupplier(VENDOR_INFO collection)
        {  // TODO: Add insert logic here
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.VENDOR_NAME))
                {
                    ModelState.AddModelError("", "Supplier Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else
                {
                    collection.VENDOR_TYPE = ConstantValue.VendorTypeSupplier;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.VENDOR_INFO.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("SupplierIndex");
                    }
                }

            }
            catch
            {
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditSupplier(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.VENDOR_INFO.Where(x => x.VENDOR_ID == id && x.VENDOR_TYPE == ConstantValue.VendorTypeSupplier).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        public ActionResult EditSupplier(int id, VENDOR_INFO collection)
        {
            try
            {
                // TODO: Add update logic here
                if (string.IsNullOrEmpty(collection.VENDOR_NAME))
                {
                    ModelState.AddModelError("", "Supplier Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else
                {
                    collection.VENDOR_TYPE = ConstantValue.VendorTypeSupplier;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("SupplierIndex");
                    }
                }

            }
            catch
            {
            }
            return View();
        }
        [HttpGet]
        public ActionResult DeleteSupplier(int id)
        {
            return View();
        }

        // POST: Vendor/Delete/5
        [HttpPost]
        public ActionResult DeleteSupplier(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("SupplierIndex");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult BuyerIndex()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.VENDOR_INFO.Where(x => x.VENDOR_TYPE == ConstantValue.VendorTypeBuyer).OrderBy(x=>x.STATUS).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        [HttpGet]
        public ActionResult CreateBuyer()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Vendor/Create
        [HttpPost]
        public ActionResult CreateBuyer(VENDOR_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.VENDOR_NAME))
                {
                    ModelState.AddModelError("", "Customer Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else
                {
                    collection.VENDOR_TYPE = ConstantValue.VendorTypeBuyer;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.VENDOR_INFO.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("BuyerIndex");
                    }
                }

            }
            catch
            {
            }
            return View();
        }
        // GET: Vendor/Edit/5       
        [HttpGet]
        public ActionResult EditBuyer(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.VENDOR_INFO.Where(x => x.VENDOR_ID == id && x.VENDOR_TYPE == ConstantValue.VendorTypeBuyer).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        public ActionResult EditBuyer(int id, VENDOR_INFO collection)
        {
            try
            {
                // TODO: Add update logic here
                if (string.IsNullOrEmpty(collection.VENDOR_NAME))
                {
                    ModelState.AddModelError("", "Supplier Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.ADDRESS))
                {
                    ModelState.AddModelError("", "Address is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.CONTACT))
                {
                    ModelState.AddModelError("", "Contact is Required!!");
                }
                else
                {
                    collection.VENDOR_TYPE = ConstantValue.VendorTypeBuyer;
                    collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("BuyerIndex");
                    }
                }
            }
            catch
            {
            }
            return View();

        }
        // GET: Vendor/Delete/5

        [HttpGet]
        public ActionResult DeleteBuyer(int id)
        {
            return View();
        }

        // POST: Vendor/Delete/5
        [HttpPost]
        public ActionResult DeleteBuyer(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("BuyerIndex");
            }
            catch
            {
                return View();
            }
        }
    }
}
