using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using EMSApp.Services;
using System.Web.Mvc;
using EMSApp.Models;
using EMSApp.Models.UserModel;
using Newtonsoft.Json;

namespace EMSApp.Controllers
{
    public class SalarySetupController : Controller
    {
        EMSEntities db = new EMSEntities();
        // GET: SalarySetup
        public ActionResult Index()
        {
            var data = db.SALARY_SETUP.Where(x => x.CANGE_TYPE == Helper.ConstantValue.TypeActive).ToList();
            return View(data);
        }
        // GET: SalarySetup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: SalarySetup/Create
        public ActionResult Create()
        {
            var data = db.SALARY_GRADE.ToList();
            GetDataInBag();
            return View(data);
        }
        // POST: SalarySetup/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                // TODO: Add insert logic here
                if (Convert.ToInt64(collection["EMP_ID"]) <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["PAY_TYPE"]))
                {
                    ModelState.AddModelError("", "Salary Patment Type is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["BASIC_SALARY"]))
                {
                    ModelState.AddModelError("", "Basic Salary is Required!!");
                }
                else
                {
                    List<SalarySeupClass> list = new List<SalarySeupClass>();
                    decimal basicSalary = Convert.ToDecimal(collection["BASIC_SALARY"]);
                    decimal grossSalary = 0;
                    string gradeString = "";
                    foreach (var key in collection.AllKeys)
                    {
                        bool isNum = int.TryParse(key, out int n);
                        if (isNum)
                        {
                            long id = Convert.ToInt64(key);
                            decimal rateValue = 0;
                            decimal value = 0;
                            if (int.TryParse(collection[key], out int i))
                            {
                                value = Convert.ToDecimal(collection[key]);
                            }

                            var data = db.SALARY_GRADE.Where(x => x.GRADE_ID == id).FirstOrDefault();
                            if (value != 0)
                            {
                                gradeString = gradeString + data.GRADE_TITLE + ": " + value + "%" + "; ";
                                rateValue = (basicSalary * value) / 100;
                            }

                            if (data.GRADE_TYPE == Helper.ConstantValue.SalaryGradeAdd)
                            {

                                grossSalary = (grossSalary <= 0) ? basicSalary + rateValue : grossSalary + rateValue;
                            }
                            else
                            {
                                grossSalary = (grossSalary <= 0) ? basicSalary - rateValue : grossSalary - rateValue;
                            }
                        }
                    }

                    SALARY_SETUP setUp = new SALARY_SETUP();
                    setUp.EMP_ID = Convert.ToInt64(collection["EMP_ID"]);
                    setUp.PAY_TYPE = collection["PAY_TYPE"];
                    setUp.GROSS_SALARY = grossSalary;
                    setUp.SALARY_GRADE_SETUP = gradeString;
                    setUp.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    setUp.ACTION_DATE = DateTime.Now;
                    setUp.CANGE_TYPE = Helper.ConstantValue.TypeActive;
                    if (ModelState.IsValid)
                    {
                        db.SALARY_SETUP.Add(setUp);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                var dt = db.SALARY_GRADE.ToList();
                GetDataInBag();
                return View(dt);
            }
            catch (Exception ex)
            {
                var dt = db.SALARY_GRADE.ToList();
                GetDataInBag();
                return View(dt);
            }
        }
        // GET: SalarySetup/Edit/5
        public ActionResult Edit(int id)
        {
            var dataGrade = db.SALARY_GRADE.ToList();
            var data = db.SALARY_SETUP.Where(x => x.SALARY_SET_ID == id).FirstOrDefault();
            GetDataInBag(data.EMP_ID, data.PAY_TYPE);
            List<SalarySeupClass> empObj = JsonConvert.DeserializeObject<List<SalarySeupClass>>(data.SALARY_GRADE_SETUP);
            foreach (var obj in empObj)
            {
                ViewData.Add(obj.GRADE_ID.ToString(), obj.GRADE_TITLE_VALUE);
            }
            return View(dataGrade);
        }
        // POST: SalarySetup/Edit/5
        [HttpPost]
        public ActionResult Edit(int newid, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (Convert.ToInt64(collection["EMP_ID"]) <= 0)
                {
                    ModelState.AddModelError("", "Employee Name is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["PAY_TYPE"]))
                {
                    ModelState.AddModelError("", "Salary Patment Type is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["BASIC_SALARY"]))
                {
                    ModelState.AddModelError("", "Basic Salary is Required!!");
                }
                else
                {
                    List<SalarySeupClass> list = new List<SalarySeupClass>();
                    decimal basicSalary = Convert.ToDecimal(collection["BASIC_SALARY"]);
                    decimal grossSalary = 0;
                    foreach (var key in collection.AllKeys)
                    {
                        bool isNum = int.TryParse(key, out int n);
                        if (isNum)
                        {
                            SalarySeupClass obj = new SalarySeupClass();
                            long id = obj.GRADE_ID = Convert.ToInt64(key);
                            decimal value = obj.GRADE_TITLE_VALUE = Convert.ToDecimal(int.TryParse(key, out int i) ? collection[key] : "0");
                            var data = db.SALARY_GRADE.Where(x => x.GRADE_ID == id).FirstOrDefault();
                            decimal rateValue = (basicSalary * value) / 100;
                            if (data.GRADE_TYPE == Helper.ConstantValue.SalaryGradeAdd)
                            {

                                grossSalary = (grossSalary <= 0) ? basicSalary + rateValue : grossSalary + rateValue;
                            }
                            else
                            {
                                grossSalary = (grossSalary <= 0) ? basicSalary - rateValue : grossSalary - rateValue;
                            }
                            list.Add(obj);
                        }
                    }
                    string jSonString = GetJsonString(list);
                    SALARY_SETUP setUp = new SALARY_SETUP();
                    setUp.EMP_ID = Convert.ToInt64(collection["EMP_ID"]);
                    setUp.PAY_TYPE = collection["PAY_TYPE"];
                    setUp.GROSS_SALARY = grossSalary;
                    setUp.SALARY_GRADE_SETUP = jSonString;
                    setUp.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    setUp.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    setUp.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(setUp).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                GetDataInBag();
                return View();

            }
            catch (Exception ex)
            {
                return View();
            }
        }
        // GET: SalarySetup/Delete/5
        public ActionResult Delete(int id)
        {
            var dt = db.SALARY_SETUP.Where(x => x.SALARY_SET_ID == id && x.CANGE_TYPE == Helper.ConstantValue.TypeActive).FirstOrDefault();
            Session["AD"] = dt.ACTION_DATE;
            return View(dt);
        }
        // POST: SalarySetup/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, SALARY_SETUP collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.SALARY_SETUP.Where(x => x.SALARY_SET_ID == id && x.CANGE_TYPE == Helper.ConstantValue.TypeActive).FirstOrDefault();
                if (dt != null)
                {
                    if (ModelState.IsValid)
                    {
                        ICombine service = new CombineServices();
                        bool result = service.SalarySetupStatusChange(id: id, statusV: Helper.ConstantValue.TypeActive);
                        if (result)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Sorry!! Item Is Not Deleted!!!");
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        private void GetDataInBag(long empId = 0, string payType = "")
        {
            ViewBag.EMP_ID = new SelectList(SetEmployee(), "Value", "Text", empId);
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO.Where(x => x.IS_DELETED == Helper.ConstantValue.TypeActive), "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        public string GetJsonString(List<SalarySeupClass> list)
        {
            string JsonStr = null;


            MemoryStream str = new MemoryStream();

            //this line very important which make ready program to make JSON  
            //GetType giving idea about you are going to create json for "System.Collections.Generic.List`1[tblMyFriend]"  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(list.GetType());

            //Writing the JSON  
            ser.WriteObject(str, list);

            str.Position = 0;
            StreamReader sr = new StreamReader(str);
            JsonStr = sr.ReadToEnd();
            //object empObj = JsonConvert.DeserializeObject<object>(JsonStr);
            return JsonStr;
        }
        public JsonResult GetEmpInfo(long id)
        {
            var data = db.POSITIONAL_INFO.Where(x => x.EMPLOYEE_ID == id && x.CHANGE_TYPE == Helper.ConstantValue.TypeActive).FirstOrDefault();
            Dictionary<string, string> listOfData = new Dictionary<string, string>();
            if (data != null)
            {
                listOfData["BASIC_SALARY"] = data.BASIC_SALARY.ToString();
            }
            else
            {
                listOfData["BASIC_SALARY"] = "";
            }
            return Json(listOfData, JsonRequestBehavior.AllowGet);
        }
    }
}
