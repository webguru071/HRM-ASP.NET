using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Models.UserModel;

namespace EMSApp.Controllers
{
    public class SalaryController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        DBHelper dbHelper = new DBHelper();
        // GET: Salary
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_INFO.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        // GET: Salary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Salary/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                ViewBag.EMP_ID = SetEmployee();
                ViewBag.SALARY_MONTH = SetMonth();
                ViewBag.SALARY_YEAR = SetYear();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        // POST: Salary/Create
        [HttpPost]
        public ActionResult Create(SalaryInfo collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (Convert.ToInt64(collection.EMP_ID) <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.SALARY_MONTH))
                {
                    ModelState.AddModelError("", "Payment Month is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.SALARY_YEAR))
                {
                    ModelState.AddModelError("", "Payment Year is Required!!");
                }
                else if (string.IsNullOrEmpty(collection.GROSS_SALARY.ToString()))
                {
                    ModelState.AddModelError("", "Salary is Required!!");
                }
                else
                {
                    SALARY_INFO salary = new SALARY_INFO();
                    salary.EMPLOYEE_ID = Convert.ToInt64(collection.EMP_ID);
                    salary.GROSS_SALARY = collection.GROSS_SALARY;
                    salary.SALARY_PAID = collection.SALARY_MONTH + ", " + collection.SALARY_YEAR;
                    decimal bonus = !string.IsNullOrEmpty(collection.BONUS.ToString()) ? Convert.ToDecimal(collection.BONUS) : 0;
                    decimal other = !string.IsNullOrEmpty(collection.OTHERS.ToString()) ? Convert.ToDecimal(collection.OTHERS) : 0;
                    salary.BONUS = bonus;
                    salary.OTHERS = other;
                    salary.TOTAL = Convert.ToInt64(collection.GROSS_SALARY) + bonus + other;
                    if (salary.ACTION_BY.ToString() != null)
                    {
                        salary.ACTION_BY = converterHelper.GetLoggedUserID();
                    }
                    salary.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.SALARY_INFO.Add(salary);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.EMP_ID = SetEmployee();
            ViewBag.SALARY_MONTH = SetMonth();
            ViewBag.SALARY_YEAR = SetYear();
            return View();
        }
        // GET: Salary/Edit/5
        public ActionResult Edit(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_INFO.Where(x => x.ID == id).FirstOrDefault();
                Session["AD"] = data.ACTION_DATE;
                //Session["AT"] = data.ACTION_BY;
                ViewBag.EMPLOYEE_ID = new SelectList(SetEmployee(), "Value", "Text", data.EMPLOYEE_ID);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        [HttpGet]
        public ActionResult AllEmpIndex()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = dbHelper.GetDataTable(@"SELECT SALARY_PAID FROM SALARY_INFO GROUP BY SALARY_PAID");
                List<SALARY_INFO> list = new List<SALARY_INFO>();
                foreach (DataRow dt in data.Rows)
                {
                    SALARY_INFO obj = new SALARY_INFO();
                    obj.SALARY_PAID = Convert.ToString(dt["SALARY_PAID"]);
                    list.Add(obj);
                }
                return View(list);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        [HttpGet]
        public ActionResult AllEmpSalaryCreate()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                ViewBag.SALARY_MONTH = SetMonth();
                ViewBag.SALARY_YEAR = SetYear();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        [HttpPost]
        public ActionResult AllEmpSalaryCreate(SalaryInfo collection)
        {
            if (string.IsNullOrEmpty(collection.SALARY_MONTH))
            {
                ModelState.AddModelError("", "Payment Month is Required!!");
            }
            else if (string.IsNullOrEmpty(collection.SALARY_YEAR))
            {
                ModelState.AddModelError("", "Payment Year is Required!!");
            }
            else
            {
                string paidDAate = collection.SALARY_MONTH + ", " + collection.SALARY_YEAR;
                var checkData = db.SALARY_INFO.Where(x => x.SALARY_PAID == paidDAate).ToList();
                if (checkData.Count > 0)
                {
                    ModelState.AddModelError("", "Salary is Already Generated for " + paidDAate + "!!!");
                }
                if (ModelState.IsValid)
                {
                    var data = db.SALARY_SETUP.Where(x => x.CANGE_TYPE == ConstantValue.TypeActive).ToList();
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var dt in data)
                            {
                                SALARY_INFO sInfo = new SALARY_INFO();
                                sInfo.EMPLOYEE_ID = dt.EMP_ID;
                                sInfo.GROSS_SALARY = dt.GROSS_SALARY;
                                sInfo.BONUS = 0;
                                sInfo.OTHERS = 0;
                                sInfo.SALARY_PAID = paidDAate;
                                sInfo.TOTAL = sInfo.GROSS_SALARY + sInfo.BONUS + sInfo.OTHERS;
                                sInfo.ACTION_BY = converterHelper.GetLoggedUserID();
                                sInfo.ACTION_DATE = DateTime.Now;
                                db.SALARY_INFO.Add(sInfo);
                                db.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                    ShowDataInList(paidDAate);
                }
            }
            ViewBag.SALARY_MONTH = SetMonth();
            ViewBag.SALARY_YEAR = SetYear();
            return View();
        }
        [HttpGet]
        public ActionResult ViewAllEmployeeSalaryMonthWise(string monthStr)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                ShowDataInList(monthStr);
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        private void ShowDataInList(string dateStr)
        {
            try
            {
                var data = db.SALARY_INFO.Where(x => x.SALARY_PAID == dateStr).ToList();
                var empData = db.EMPLOYEE_INFO.ToList();
                List<SalaryInfo> list = new List<SalaryInfo>();
                foreach (var dt in data)
                {
                    SalaryInfo info = new SalaryInfo();
                    var empDataInd = empData.Find(x => x.ID == dt.EMPLOYEE_ID);
                    info.EMPLOYEE_NAME = empDataInd.EMPLOYEE_NAME;
                    info.GROSS_SALARY = dt.GROSS_SALARY;
                    info.SALARY_PAID = dateStr;
                    list.Add(info);
                }
                ViewBag.SALARY_INFO = list;
            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult Edit(int id, SALARY_INFO salary)
        {
            try
            {
                // TODO: Add update logic here

                if (salary.EMPLOYEE_ID <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.SALARY_PAID.ToString()))
                {
                    ModelState.AddModelError("", "Salary Paid Date is Required!!");
                }
                else if (string.IsNullOrEmpty(salary.GROSS_SALARY.ToString()))
                {
                    ModelState.AddModelError("", "Salary is Required!!");
                }
                else
                {
                    decimal bonus = salary.BONUS != null ? Convert.ToDecimal(salary.BONUS) : 0;
                    decimal other = (salary.OTHERS != null) ? Convert.ToDecimal(salary.OTHERS) : 0;
                    salary.TOTAL = salary.GROSS_SALARY + bonus + other;
                    salary.UPDATE_BY = converterHelper.GetLoggedUserID();
                    salary.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    salary.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(salary).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.EMPLOYEE_ID = SetEmployee();
                return View();
            }
        }

        // GET: Salary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Salary/Delete/5
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
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        private dynamic SetYear()
        {
            string year = DateTime.Now.Year.ToString();
            List<SelectListItem> list = new SelectList(ListValue.Year, "Value", "Key", year).ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return list;
        }

        private dynamic SetMonth()
        {
            List<SelectListItem> list = new SelectList(ListValue.Month, "Value", "Key").ToList();
            list.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return list;
        }
        public JsonResult GetEmpInfo(long id)
        {
            var data = db.SALARY_SETUP.Where(x => x.EMP_ID == id).FirstOrDefault();
            Dictionary<string, string> listOfData = new Dictionary<string, string>();
            if (data != null)
            {
                listOfData["GROSS_SALARY"] = data.GROSS_SALARY.ToString();
            }
            else
            {
                listOfData["GROSS_SALARY"] = "";
            }
            return Json(listOfData, JsonRequestBehavior.AllowGet);
        }
    }
}
