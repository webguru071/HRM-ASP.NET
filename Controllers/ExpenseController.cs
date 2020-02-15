using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Helper;
using System.Data.Entity;
using System.Collections;

namespace EMSApp.Controllers
{
    public class ExpenseController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        // GET: Expense
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.TRANSACTION_ITEM.Where(x => x.TYPE == ConstantValue.TransactionTypeExpense).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Expense/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }
        // POST: Expense/Create
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
                    collection.TYPE = ConstantValue.TransactionTypeExpense;
                    collection.ACTION_BY =converterHelper.GetLoggedUserID();
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
        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: Expense/Edit/5
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
                    collection.TYPE = ConstantValue.TransactionTypeExpense;
                    collection.UPDATE_BY =             converterHelper.GetLoggedUserID();
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
        // GET: Expense/Delete/5
        public ActionResult Delete(int id)
        {
            var dt = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
            return View(dt);           
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.TRANSACTION_ITEM.Where(x => x.TRNS_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
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
        public ActionResult ExpenseSheetIndex()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.TRANSACTION_SHEET.Where(x => x.TYPE == ConstantValue.TransactionTypeExpense).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        [HttpGet]
        public ActionResult ExpenseSheetCreate()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        private void GetDataInBag(long transId=0)
        {
            ViewBag.TRNS_ID  = new SelectList(SetExpType(), "Value", "Text", transId);
        }

        private List<SelectListItem> SetExpType()
        {
            List<SelectListItem> expTypeList = new SelectList(db.TRANSACTION_ITEM.Where(x => x.TYPE == ConstantValue.TransactionTypeExpense), "TRNS_ID", "TRNS_TITLE").ToList();
            expTypeList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return expTypeList;
        }
        [HttpPost]
        public ActionResult ExpenseSheetCreate(TRANSACTION_SHEET collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(collection.DATE))
                {
                    ModelState.AddModelError("", "Date is Required!!");
                }
                else if (collection.TRNS_ID<=0)
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
                    collection.TYPE = ConstantValue.TransactionTypeExpense;
                    collection.ACTION_BY =             converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.TRANSACTION_SHEET.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("ExpenseSheetIndex");
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
        public ActionResult ExpenseSheetEdit(long id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                long trnsId = Convert.ToInt64(data.TRNS_ID);
                GetDataInBag(trnsId);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }            
        }
        [HttpPost]
        public ActionResult ExpenseSheetEdit(long id,TRANSACTION_SHEET collection)
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
                    collection.TYPE = ConstantValue.TransactionTypeExpense;
                    collection.UPDATE_BY =converterHelper.GetLoggedUserID();
                    collection.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    collection.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(collection).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("ExpenseSheetIndex");
                    }
                }               
            }
            catch (Exception ex)
            {
                
                
            }
            long trnsId = Convert.ToInt64(collection.TRNS_ID);
            GetDataInBag(trnsId);
            return View();
        }
        [HttpGet]
        public ActionResult ExpenseSheetDelete(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
           
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult ExpenseSheetDelete(int id, FormCollection collection)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                // TODO: Add delete logic here
                var dt = db.TRANSACTION_SHEET.Where(x => x.TRNS_S_ID == id && x.TYPE == ConstantValue.TransactionTypeExpense).FirstOrDefault();
                if (dt != null)
                {
                    db.TRANSACTION_SHEET.Remove(dt);
                    db.SaveChanges();
                    return RedirectToAction("ExpenseSheetIndex");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
    }
}
