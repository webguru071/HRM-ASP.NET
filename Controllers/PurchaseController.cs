using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Models.UserModel;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Services.Inventory;

namespace EMSApp.Controllers
{
    public class PurchaseController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        IInventory invService = new InventoryService();
        // GET: Purchase
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INV_INFO.Where(x => x.STOCK_TYPE == ConstantValue.TransactionSheetTransTypePurchase).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // GET: Purchase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Purchase/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Purchase/Create
        [HttpPost]
        public ActionResult Create(INV_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.EQP_ID <= 0)
                {
                    ModelState.AddModelError("", "Product Name is Required!!");
                }
                else if (collection.VENDOR_ID <= 0)
                {
                    ModelState.AddModelError("", "Supplier is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Purchase Date is Required!!");
                }
                else if (collection.UNIT <= 0)
                {
                    ModelState.AddModelError("", "Purchase Unit is Required!!");
                }
                else
                {
                    STOCK_INFO stock = new STOCK_INFO();
                    stock.INV_ID = collection.INV_ID= GetInvId();
                    stock.EQP_ID = collection.EQP_ID;
                    stock.UNIT = collection.UNIT;
                    stock.STOCK_FOR = ConstantValue.StockForAdd;
                    stock.STOCK_TYPE = collection.STOCK_TYPE = ConstantValue.TransactionSheetTransTypePurchase;
                    stock.ACTION_BY = collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    stock.ACTION_DATE = collection.ACTION_DATE  = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        using (var dbContextTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.INV_INFO.Add(collection);
                                db.STOCK_INFO.Add(stock);
                                db.SaveChanges();
                                dbContextTransaction.Commit();
                                return RedirectToAction("Index");
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            long supplierId = Convert.ToInt64(collection.VENDOR_ID);
            GetDataInBag(supplierId: supplierId);
            return View();
        }

        private long GetInvId()
        {
            DBHelper context = new DBHelper();
            string query = @"SELECT NEXT VALUE FOR seq_INV_INFO_INV_ID";
            return Convert.ToInt64(context.ExecuteDMLGetId(query));
        }

        // GET: Purchase/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INV_INFO.Where(x => x.INV_ID == id && x.STOCK_TYPE == ConstantValue.TransactionSheetTransTypePurchase).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                long supplierId = Convert.ToInt64(data.VENDOR_ID);
                GetDataInBag(supplierId: supplierId,eqpId:data.EQP_ID);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Purchase/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, INV_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.EQP_ID <= 0)
                {
                    ModelState.AddModelError("", "Product Name is Required!!");
                }
                else if (collection.VENDOR_ID <= 0)
                {
                    ModelState.AddModelError("", "Supplier is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Purchase Date is Required!!");
                }
                else if (collection.UNIT <= 0)
                {
                    ModelState.AddModelError("", "Purchase Unit is Required!!");
                }
                else
                {
                    STOCK_INFO stock = new STOCK_INFO();
                    stock.INV_ID = id;
                    stock.EQP_ID = collection.EQP_ID;
                    stock.UNIT = collection.UNIT;
                    stock.STOCK_FOR = ConstantValue.StockForAdd;
                    stock.STOCK_TYPE = collection.STOCK_TYPE = ConstantValue.TransactionSheetTransTypePurchase;
                    stock.UPDATE_BY= collection.UPDATE_BY = converterHelper.GetLoggedUserID();
                    stock.UPDATE_DATE = collection.UPDATE_DATE = DateTime.Now ;
                    if (ModelState.IsValid)
                    {
                        bool updateResult = invService.UpdateInventoryData(collection:collection,stock:stock,id:id);
                        if (updateResult)
                        {
                            return RedirectToAction("Index");
                        }                       
                    }
                }
            }
            catch(Exception ex)
            {
            }
            long supplierId = Convert.ToInt64(collection.VENDOR_ID);
            GetDataInBag(supplierId: supplierId,eqpId:collection.EQP_ID);
            return View();
        }

        // GET: Purchase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Purchase/Delete/5
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
        [HttpGet]
        public ActionResult IndexWastage()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INV_INFO.Where(x => x.STOCK_TYPE == ConstantValue.TransactionSheetTransTypeWastage).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        [HttpGet]
        public ActionResult WastageCreate()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Purchase/Delete/5
        [HttpPost]
        public ActionResult WastageCreate( INV_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.EQP_ID <= 0)
                {
                    ModelState.AddModelError("", "Product Name is Required!!");
                }
                else if (collection.VENDOR_ID <= 0)
                {
                    ModelState.AddModelError("", "Supplier is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Purchase Date is Required!!");
                }
                else if (collection.UNIT <= 0)
                {
                    ModelState.AddModelError("", "Purchase Unit is Required!!");
                }
                else
                {
                    STOCK_INFO stock = new STOCK_INFO();
                    stock.INV_ID = collection.INV_ID = GetInvId();
                    stock.EQP_ID = collection.EQP_ID;
                    stock.UNIT = collection.UNIT;
                    stock.STOCK_FOR = ConstantValue.StockForDeduct;
                    stock.STOCK_TYPE = collection.STOCK_TYPE = ConstantValue.TransactionSheetTransTypeWastage;
                    stock.ACTION_BY = collection.ACTION_BY = converterHelper.GetLoggedUserID();
                    stock.ACTION_DATE = collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        using (var dbContextTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.INV_INFO.Add(collection);
                                db.STOCK_INFO.Add(stock);
                                db.SaveChanges();
                                dbContextTransaction.Commit();
                                return RedirectToAction("IndexWastage");
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            long supplierId = Convert.ToInt64(collection.VENDOR_ID);
            GetDataInBag(supplierId: supplierId);
            return View();
        }
        [HttpGet]
        public ActionResult WastageEdit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.INV_INFO.Where(x => x.INV_ID == id && x.STOCK_TYPE == ConstantValue.TransactionSheetTransTypeWastage).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                long supplierId = Convert.ToInt64(data.VENDOR_ID);
                GetDataInBag(supplierId: supplierId, eqpId: data.EQP_ID);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: Purchase/Edit/5
        [HttpPost]
        public ActionResult WastageEdit(int id, INV_INFO collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.EQP_ID <= 0)
                {
                    ModelState.AddModelError("", "Product Name is Required!!");
                }
                else if (collection.VENDOR_ID <= 0)
                {
                    ModelState.AddModelError("", "Supplier is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Purchase Date is Required!!");
                }
                else if (collection.UNIT <= 0)
                {
                    ModelState.AddModelError("", "Purchase Unit is Required!!");
                }
                else
                {
                    STOCK_INFO stock = new STOCK_INFO();
                    stock.INV_ID = id;
                    stock.EQP_ID = collection.EQP_ID;
                    stock.UNIT = collection.UNIT;
                    stock.STOCK_FOR = ConstantValue.StockForDeduct;
                    stock.STOCK_TYPE = collection.STOCK_TYPE = ConstantValue.TransactionSheetTransTypeWastage;
                    stock.UPDATE_BY = collection.UPDATE_BY = converterHelper.GetLoggedUserID();
                    stock.UPDATE_DATE = collection.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        bool updateResult = invService.UpdateInventoryData(collection: collection, stock: stock, id: id);
                        if (updateResult)
                        {
                            return RedirectToAction("IndexWastage");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            long supplierId = Convert.ToInt64(collection.VENDOR_ID);
            GetDataInBag(supplierId: supplierId, eqpId: collection.EQP_ID);
            return View();
        }
        private void GetDataInBag(long assetId = 0, long supplierId = 0, long eqpId=0)
        {
            ViewBag.ASSET_ID = new SelectList(SetAsset(), "Value", "Text", assetId);
            ViewBag.VENDOR_ID = new SelectList(SetSupplier(), "Value", "Text", supplierId);
            if(eqpId == 0)
            {
                var iniText = new List<SelectListItem>
            {
                new SelectListItem{ Text="Select One", Value = ""}                
            };
                ViewBag.EQP_ID = iniText;
            }
            else
            {
                var data = db.EQUEPMENTS_INFO.Where(x => x.EQUEPMENT_ID == eqpId).FirstOrDefault();
                ViewBag.ASSET_ID = new SelectList(SetAsset(), "Value", "Text", data.ASSET_ID);
                List<SelectListItem> list = new SelectList(db.EQUEPMENTS_INFO.Where(x => x.ASSET_ID == data.ASSET_ID), "EQUEPMENT_ID", "EQP_TITLE").ToList();
                list.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));                
                ViewBag.EQP_ID = new SelectList(list, "Value", "Text", eqpId);
            }
        }
        private List<SelectListItem> SetAsset()
        {
            List<SelectListItem> list = new SelectList(db.ASSET_INFO, "ASSET_ID", "ASSET_TITLE").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return list;
        }
        public JsonResult SetEquipment(long assetId)
        {
            List<SelectListItem> list = new SelectList(db.EQUEPMENTS_INFO.Where(x => x.ASSET_ID == assetId), "EQUEPMENT_ID", "EQP_TITLE").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
        private List<SelectListItem> SetSupplier()
        {
            List<SelectListItem> list = new SelectList(db.VENDOR_INFO.Where(x => x.VENDOR_TYPE == ConstantValue.VendorTypeSupplier), "VENDOR_ID", "VENDOR_NAME").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return list;
        }
    }
}
