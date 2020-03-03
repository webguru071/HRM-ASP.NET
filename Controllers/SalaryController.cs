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
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class SalaryController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        DBHelper dbHelper = new DBHelper();
        ICombine service = new CombineServices();
        // GET: Salary
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_INFO.OrderByDescending(x => x.ACTION_DATE).ToList();
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
                    salary.STATUS = ConstantValue.SalaryPaymentIndividual;
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
        public ActionResult AllEmpSalaryCreate(FormCollection collection, string submit)
        {
            if (string.IsNullOrEmpty(collection["SALARY_MONTH"]))
            {
                ModelState.AddModelError("", "Payment Month is Required!!");
            }
            else if (string.IsNullOrEmpty(collection["SALARY_YEAR"]))
            {
                ModelState.AddModelError("", "Payment Year is Required!!");
            }
            else
            {
                string paidDAate = collection["SALARY_MONTH"] + ", " + collection["SALARY_YEAR"];
                Session["paidDAate"] = paidDAate;
                var checkData = db.SALARY_INFO.Where(x => x.SALARY_PAID == paidDAate).ToList();
                if (checkData.Count > 0)
                {
                    ModelState.AddModelError("", "Salary is Already Generated for " + paidDAate + "!!!");
                }
                else
                {
                    switch (submit)
                    {
                        case "Generate":
                            {
                                GetSalaryInfo(collection["SALARY_MONTH"], collection["SALARY_YEAR"]);
                                break;
                            }
                        case "Pay":
                            {
                                break;
                            }
                        case "Calculate":
                            {
                                break;
                            }
                        default:
                            {
                                ModelState.AddModelError("", "Error Accured!!!");
                                break;
                            }
                    }
                }                               
            }
            ViewBag.SALARY_MONTH = SetMonth(collection["SALARY_MONTH"]);
            ViewBag.SALARY_YEAR = SetYear();
            return View(collection);
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
                string[] arr = dateStr.Split(',');
                string month = arr[0].Trim();
                string year = arr[1].Trim();
                string fromDate = year + "-" + month + "-01";
                DateTime firstDate = Convert.ToDateTime(fromDate);
                DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
                string toDate = lastDate.ToString("yyyy-MM-dd");
                var data = db.SALARY_INFO.Where(x => x.SALARY_PAID == dateStr).ToList();
                List<SalaryInfo> infoList = new List<SalaryInfo>();
                var dataLeaveList = service.GetEmployeeLeaveList(fromDate: fromDate, toDate: toDate);
                var positionData = db.POSITIONAL_INFO.Where(x => x.STATUS == ConstantValue.TypeActive).ToList();
                foreach (var item in data)
                {
                    SalaryInfo obj = new SalaryInfo();
                    obj.EMP_ID = item.EMPLOYEE_ID;
                    obj.EMPLOYEE_NAME = dataLeaveList.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).Select(x => x.EMPLOYEE_NAME).FirstOrDefault();
                    obj.BASIC_SALARY = positionData.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).Select(x => x.BASIC_SALARY).FirstOrDefault();
                    obj.GROSS_SALARY = item.GROSS_SALARY;
                    obj.BONUS = Convert.ToDecimal(item.BONUS);
                    obj.OTHERS = Convert.ToDecimal(item.OTHERS);
                    obj.ADDITION = Convert.ToDecimal(item.ADDITION);
                    obj.DEDUCTION = Convert.ToDecimal(item.DEDUCTION);
                    obj.ADVANCE = Convert.ToDecimal(item.ADVANCE);
                    obj.COMMISSION = Convert.ToDecimal(item.COMMISSION);
                    obj.TOTAL = Convert.ToDecimal(item.TOTAL);
                    obj.NOTE = item.REMARKS;
                    obj.TOTAL_LEAVE = dataLeaveList.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).Select(x => x.TOTAL_LEAVE_TAKEN).FirstOrDefault();
                    infoList.Add(obj);
                }
                ViewBag.ListValue = infoList;
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

        private dynamic SetMonth(string month = "")
        {
            List<SelectListItem> list = new SelectList(ListValue.MonthDate, "Value", "Key", month).ToList();
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
        public void GetSalaryInfo(string month, string year)
        {
            string fromDate = year + "-" + month + "-01";
            DateTime firstDate = Convert.ToDateTime(fromDate);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            string toDate = lastDate.ToString("yyyy-MM-dd");
            List<SalaryInfo> infoList = new List<SalaryInfo>();
            var empData = db.EMPLOYEE_INFO.ToList();
            var dataLeaveList = service.GetEmployeeLeaveList(fromDate: fromDate, toDate: toDate);
            var positionData = db.POSITIONAL_INFO.Where(x => x.STATUS == ConstantValue.TypeActive).ToList();
            var data = db.SALARY_SETUP.Where(x => x.CANGE_TYPE == ConstantValue.TypeActive).ToList();
            foreach (var item in data)
            {
                SalaryInfo obj = new SalaryInfo();
                obj.EMP_ID = item.EMP_ID;
                obj.EMPLOYEE_NAME = empData.Where(x => x.ID == item.EMP_ID).Select(x => x.EMPLOYEE_NAME).FirstOrDefault();
                obj.BASIC_SALARY = positionData.Where(x => x.EMPLOYEE_ID == item.EMP_ID).Select(x => x.BASIC_SALARY).FirstOrDefault();
                obj.SALARY_GRADE_STRING = item.SALARY_GRADE_SETUP_STRING;
                obj.GROSS_SALARY = item.GROSS_SALARY;
                obj.BONUS = 0;
                obj.OTHERS = 0;
                obj.ADDITION = 0;
                obj.DEDUCTION = 0;
                obj.ADVANCE = 0;
                obj.COMMISSION = 0;
                obj.TOTAL = obj.GROSS_SALARY;
                obj.TOTAL_LEAVE = dataLeaveList.Where(x => x.EMPLOYEE_ID == item.EMP_ID).Select(x => x.TOTAL_LEAVE_TAKEN).FirstOrDefault();
                infoList.Add(obj);
            }
            ViewBag.ListValue = infoList;
        }
        [HttpPost]
        public ActionResult InsertData(string[] arrayList)
        {
            string paidDAate = Convert.ToString(Session["paidDAate"]);
            string sumValue = arrayList[0].Trim();
            string[] sumArr = sumValue.Split(':');
            SALARY_INFO_SUM objSum = new SALARY_INFO_SUM();
            objSum.SALARY_PAID_MONTH = paidDAate;
            objSum.TOTAL_PAID = Convert.ToDecimal(sumArr[0]);
            List<SALARY_INFO> sInfoList = new List<SALARY_INFO>();
            for (int i=1;i< arrayList.Length;i++)
            {
                string valueStr = arrayList[i].Trim();
                string[] valueArr = valueStr.Split(':');
                SALARY_INFO obj = new SALARY_INFO();
                obj.EMPLOYEE_ID = Convert.ToInt64(valueArr[0]);
                obj.BONUS = Convert.ToDecimal(valueArr[1]);
                obj.OTHERS = 0;
                obj.ADDITION = Convert.ToDecimal(valueArr[2]);
                obj.DEDUCTION = Convert.ToDecimal(valueArr[3]);
                obj.ADVANCE = Convert.ToDecimal(valueArr[4]);
                obj.COMMISSION = Convert.ToDecimal(valueArr[5]);
                obj.GROSS_SALARY = Convert.ToDecimal(valueArr[6]);
                obj.TOTAL = Convert.ToDecimal(valueArr[7]);
                obj.REMARKS = Convert.ToString(valueArr[8]);
                obj.SALARY_PAID = paidDAate;
                obj.ACTION_BY = converterHelper.GetLoggedUserID(); 
                obj.ACTION_DATE = DateTime.Now;
                sInfoList.Add(obj);
            }
            bool result=service.InsertSalary(objSum: objSum, objInfo: sInfoList);
            if (result)
            {
                return View("AllEmpIndex");
            }
            return View();
        }
    }
}
