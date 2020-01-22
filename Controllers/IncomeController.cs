using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Helper;
using System.Data.Entity;

namespace EMSApp.Controllers
{
    public class IncomeController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: Income
        public ActionResult Index()
        {
            var data = db.TRANSACTION_ITEM.Where(x => x.TYPE == ConstantValue.TransactionTypeIncome).ToList();
            return View(data);
        }

        // GET: Income/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Income/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        public ActionResult Create(TRANSACTION_ITEM collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.TRNS_TITLE))
                {
                    ModelState.AddModelError("", "Leave Start Date is Required!!");
                }
                else
                {
                    collection.TYPE = ConstantValue.TransactionTypeIncome;
                    collection.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.TRANSACTION_ITEM.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            return View(data);
        }

        // POST: Income/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TRANSACTION_ITEM collection)
        {
            try
            {
                // TODO: Add Update logic here
                if (string.IsNullOrEmpty(collection.TRNS_TITLE))
                {
                    ModelState.AddModelError("", "Leave Start Date is Required!!");
                }
                else
                {
                    collection.TYPE = ConstantValue.TransactionTypeIncome;
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
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Income/Delete/5
        public ActionResult Delete(int id)
        {
            var dt = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
            return View(dt);
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
                if (dt != null)
                {
                    db.TRANSACTION_ITEM.Remove(dt);
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

        [HttpGet]
        public ActionResult IncomeSheetIndex()
        {
            var data = db.TRANSACTION_SHEET.Where(x => x.TYPE == ConstantValue.TransactionTypeIncome).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult IncomeSheetCreate()
        {
            GetDataInBag();
            return View();
        }

        private void GetDataInBag(long transId = 0)
        {
            ViewBag.TRNS_ID = ViewBag.LEAVE_TYPE_ID = new SelectList(SetIncomeType(), "Value", "Text", transId);
        }

        private List<SelectListItem> SetIncomeType()
        {
            List<SelectListItem> expTypeList = new SelectList(db.TRANSACTION_ITEM.Where(x=>x.TYPE==ConstantValue.TransactionTypeIncome), "TRNS_ID", "TRNS_TITLE").ToList();
            expTypeList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return expTypeList;
        }

        [HttpPost]
        public ActionResult IncomeSheetCreate(TRANSACTION_SHEET collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Date is Required!!");
                }
                else if (collection.TRNS_ID <= 0)
                {
                    ModelState.AddModelError("", "Expenses Item is Required!!");
                }
                else if (collection.AMOUNT <= 0)
                {
                    ModelState.AddModelError("", "Expenses Amount is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.PAY_TYPE))
                {
                    ModelState.AddModelError("", "Payment Method is Required!!");
                }
                else
                {
                    collection.TYPE = ConstantValue.TransactionTypeIncome;
                    collection.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.TRANSACTION_SHEET.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("IncomeSheetIndex");
                    }
                }
                GetDataInBag();
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
        }
        [HttpGet]
        public ActionResult IncomeSheetEdit(long id)
        {
            var data = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            GetDataInBag(data.TRNS_ID);
            return View(data);
        }
        [HttpPost]
        public ActionResult IncomeSheetEdit(long id, TRANSACTION_SHEET collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.DATE.ToString()))
                {
                    ModelState.AddModelError("", "Date is Required!!");
                }
                else if (collection.TRNS_ID <= 0)
                {
                    ModelState.AddModelError("", "Expenses Item is Required!!");
                }
                else if (collection.AMOUNT <= 0)
                {
                    ModelState.AddModelError("", "Expenses Amount is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.PAY_TYPE))
                {
                    ModelState.AddModelError("", "Payment Method is Required!!");
                }
                else
                {
                    collection.TYPE = ConstantValue.TransactionTypeIncome;
                    collection.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    collection.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("IncomeSheetIndex");
                    }
                }
                GetDataInBag(collection.TRNS_ID);
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag(collection.TRNS_ID);
                return View();
            }
        }
        [HttpGet]
        public ActionResult ExpenseSheetDelete(int id)
        {
            var dt = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
            return View(dt);
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult ExpenseSheetDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeIncome).FirstOrDefault();
                if (dt != null)
                {
                    db.TRANSACTION_SHEET.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("IncomeSheetIndex");
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
