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
using EMSApp.Helper;

namespace EMSApp.Controllers
{
    public class SalarySetupController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converterHelper = new ConverterHelper();
        ICombine service = new CombineServices();

        // GET: SalarySetup
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_SETUP.Where(x => x.CANGE_TYPE == Helper.ConstantValue.TypeActive).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        // GET: SalarySetup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: SalarySetup/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_GRADE.ToList();
                GetDataInBag();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
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
                else if (string.IsNullOrEmpty(collection["CALCULATE_AS"]))
                {
                    ModelState.AddModelError("", "Benefit Calculation Type is Required!!");
                }
                else
                {
                    List<SalarySeupClass> list = new List<SalarySeupClass>();
                    decimal basicSalary = Convert.ToDecimal(collection["BASIC_SALARY"]);
                    decimal grossSalary = 0;
                    string gradeString = "";
                    string gradeAdd = collection["CALCULATE_AS"];
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
                                if (collection["CALCULATE_AS"] == ConstantValue.SalarySetupInPercentage)
                                {
                                    rateValue = (basicSalary * value) / 100;
                                    gradeString = gradeString + data.GRADE_TITLE + ": " + value + "% ; ";
                                }
                                else
                                {
                                    rateValue = value;
                                    gradeString = gradeString + data.GRADE_TITLE + ": " + value + " ; ";
                                }
                               
                                gradeAdd = gradeAdd + " " + key + ":" + value;
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
                    setUp.POSITION_ID = Convert.ToInt64(Session["POSITION_ID"]);
                    setUp.PAY_TYPE = collection["PAY_TYPE"];
                    setUp.GROSS_SALARY = grossSalary;
                    setUp.SALARY_GRADE_SETUP = gradeAdd;
                    setUp.SALARY_GRADE_SETUP_STRING = gradeString;
                    setUp.ACTION_BY = converterHelper.GetLoggedUserID();
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
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var data = db.SALARY_SETUP.Where(x => x.SALARY_SET_ID == id).FirstOrDefault();
                var positionalData = db.POSITIONAL_INFO.Where(x => x.EMPLOYEE_ID == data.EMP_ID).FirstOrDefault();
                Session["POSITION_ID"] = data.POSITION_ID;
                Session["AD"] = data.ACTION_DATE;
                GetDataInBag(data.EMP_ID, data.PAY_TYPE);
                var dataGrade = db.SALARY_GRADE.ToList();
                string grdStr = data.SALARY_GRADE_SETUP.Trim();
                string[] gradArray = grdStr.Split(new char[0]);
                int arrCount = gradArray.Length;
                ViewBag.CALCULATE_AS = gradArray[0];
                data.GROSS_SALARY = positionalData.BASIC_SALARY;
                int index = 1;
                List<SalarySeupClass> gradeListValue = new List<SalarySeupClass>();
                foreach (var dt in dataGrade)
                {
                    SalarySeupClass obj = new SalarySeupClass();
                    obj.GRADE_ID = dt.GRADE_ID;
                    obj.GRADE_TITLE = dt.GRADE_TITLE;
                    if (arrCount > index && !string.IsNullOrWhiteSpace(gradArray[index]))
                    {
                        string[] setup = gradArray[index].Split(':');
                        if (Convert.ToInt64(setup[0]) == dt.GRADE_ID)
                        {
                            obj.GRADE_TITLE_VALUE = Convert.ToDecimal(setup[1]);
                            index++;
                        }
                    }
                    else
                    {
                        obj.GRADE_TITLE_VALUE = 0;
                    }
                    gradeListValue.Add(obj);

                }
                ViewBag.ListValue = gradeListValue;
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }
        // POST: SalarySetup/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
                else if (string.IsNullOrEmpty(collection["GROSS_SALARY"]))
                {
                    ModelState.AddModelError("", "Basic Salary is Required!!");
                }
                else if (string.IsNullOrEmpty(collection["CALCULATE_AS"]))
                {
                    ModelState.AddModelError("", "Benefit Calculation Type is Required!!");
                }
                else
                {
                    List<SalarySeupClass> list = new List<SalarySeupClass>();
                    decimal basicSalary = Convert.ToDecimal(collection["GROSS_SALARY"]);
                    decimal grossSalary = 0;
                    string gradeString = "";
                    string gradeAdd = collection["CALCULATE_AS"];
                    foreach (var key in collection.AllKeys)
                    {
                        bool isNum = int.TryParse(key, out int n);
                        if (isNum)
                        {
                            long KeyId = Convert.ToInt64(key);
                            decimal rateValue = 0;
                            decimal value = 0;
                            if (int.TryParse(collection[key], out int i))
                            {
                                value = Convert.ToDecimal(collection[key]);
                            }

                            var data = db.SALARY_GRADE.Where(x => x.GRADE_ID == KeyId).FirstOrDefault();
                            if (value != 0)
                            {
                                if (collection["CALCULATE_AS"] == ConstantValue.SalarySetupInPercentage)
                                {
                                    rateValue = (basicSalary * value) / 100;
                                    gradeString = gradeString + data.GRADE_TITLE + ": " + value + "% ; ";
                                }
                                else
                                {
                                    rateValue = value;
                                    gradeString = gradeString + data.GRADE_TITLE + ": " + value + " ; ";
                                }
                                
                                gradeAdd = gradeAdd + " " + key + ":" + value;
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
                    setUp.SALARY_SET_ID = Convert.ToInt64(collection["EMP_ID"]);
                    setUp.EMP_ID = Convert.ToInt64(collection["EMP_ID"]);
                    setUp.POSITION_ID = Convert.ToInt64(Session["POSITION_ID"]);
                    setUp.PAY_TYPE = collection["PAY_TYPE"];
                    setUp.GROSS_SALARY = grossSalary;
                    setUp.SALARY_GRADE_SETUP = gradeAdd;
                    setUp.SALARY_GRADE_SETUP_STRING = gradeString;
                    setUp.CANGE_TYPE = ConstantValue.TypeActive;
                    setUp.UPDATE_BY = converterHelper.GetLoggedUserID();
                    setUp.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    setUp.UPDATE_DATE = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        bool result = service.SalarySetupUpdate(id: id, obj: setUp);
                        if (result)
                        {
                            Session["POSITION_ID"] = null;
                            return RedirectToAction("Index");
                        }                           
                    }
                }
            }
            catch (Exception ex)
            {

            }
            GetDataInBag();
            return View(collection);
        }
        // GET: SalarySetup/Delete/5
        public ActionResult Delete(int id)
        {
            if (converterHelper.CheckLogin() && converterHelper.GetLoggedUserLevel() == ConstantValue.UserLevelAdmin)
            {
                var dt = db.SALARY_SETUP.Where(x => x.SALARY_SET_ID == id && x.CANGE_TYPE == Helper.ConstantValue.TypeActive).FirstOrDefault();
                Session["AD"] = dt.ACTION_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
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
                        bool result = service.SalarySetupStatusChange(id: id, statusV: Helper.ConstantValue.UserStatusDeactive);
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
            var data = db.POSITIONAL_INFO.Where(x => x.EMPLOYEE_ID == id && x.STATUS == Helper.ConstantValue.TypeActive).FirstOrDefault();
            Dictionary<string, string> listOfData = new Dictionary<string, string>();
            if (data != null)
            {
                listOfData["BASIC_SALARY"] = data.BASIC_SALARY.ToString();
                Session["POSITION_ID"] = data.POSITION_ID;
            }
            else
            {
                listOfData["BASIC_SALARY"] = "";
            }
            return Json(listOfData, JsonRequestBehavior.AllowGet);
        }       
    }
}
