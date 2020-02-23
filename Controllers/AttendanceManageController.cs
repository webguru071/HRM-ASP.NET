using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;
using EMSApp.Services;

namespace EMSApp.Controllers
{
    public class AttendanceManageController : Controller
    {
        ConverterHelper converterHelper = new ConverterHelper();
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        // GET: AttendanceManage
        public ActionResult Index()
        {
            if (converterHelper.CheckLogin())
            {
                long empId = converterHelper.GetLoggedEmployeeID();
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                var data = service.GetAttendanceData(empId: empId, toDate: date);
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // GET: AttendanceManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceManage/Create
        public ActionResult Create()
        {
            if (converterHelper.CheckLogin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: AttendanceManage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here                
                if (string.IsNullOrEmpty(collection["checkIn"]) && string.IsNullOrEmpty(collection["checkOut"]))
                {
                    ModelState.AddModelError("", "Attendance Status is Required!!");
                }
                if (!string.IsNullOrEmpty(collection["checkIn"]) && !string.IsNullOrEmpty(collection["checkOut"]))
                {
                    ModelState.AddModelError("", "Select Only One Status!!");
                }
                else if (string.IsNullOrEmpty(collection["pass"]))
                {
                    ModelState.AddModelError("", "Give Password for Confirmation!!");
                }
                else
                {
                    string pass = collection["pass"];
                    long userId = Convert.ToInt64(Session["USER_ID"]);
                    var data = db.USER_INFO.Where(x => x.ID == userId).FirstOrDefault();
                    if (data.PASSWORD != pass)
                    {
                        ModelState.AddModelError("", "Password is Not Matched!!");
                        return View(collection);
                    }
                    else
                    {
                        ATTENDANCE_DETAILS attn = new ATTENDANCE_DETAILS();
                        attn.EMPLOYEE_ID = Convert.ToInt64(data.EMPLOYEE_ID);
                        attn.ATT_DATE = DateTime.Today.Date;
                        attn.STATUS = (string.IsNullOrEmpty(collection["checkIn"])) ? Helper.ConstantValue.AttendanceCheckOut : Helper.ConstantValue.AttendanceCheckIn;
                        int count = db.ATTENDANCE_DETAILS.Where(x => x.EMPLOYEE_ID == attn.EMPLOYEE_ID && x.ATT_DATE == attn.ATT_DATE).Count();
                        attn.SL_NO = count + 1;
                        if (!string.IsNullOrEmpty(collection["checkIn"]))
                        {
                            attn.CHECK_IN_TIME = DateTime.Now.TimeOfDay;
                        }
                        else if (!string.IsNullOrEmpty(collection["checkOut"]))
                        {
                            attn.CHECK_OUT_TIME = DateTime.Now.TimeOfDay;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Give Proper Information");
                            return View(collection);
                        }
                        if (ModelState.IsValid)
                        {
                            db.ATTENDANCE_DETAILS.Add(attn);
                            db.SaveChanges();
                            return RedirectToAction("Index");
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

        // GET: AttendanceManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceManage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        // GET: AttendanceManage/Import
        public ActionResult Import()
        {
            return View();
        }
        // POST: AttendanceManage/Delete/5
        [HttpPost]
        public ActionResult Import(FormCollection collection, HttpPostedFileBase uploadFile)
        {
            try
            {
                // TODO: Add delete logic here
                if (uploadFile == null || uploadFile.ContentLength <= 0)
                {
                    ModelState.AddModelError("", "File is Required!!!");
                }
                else if (!uploadFile.FileName.EndsWith("xls") && !uploadFile.FileName.EndsWith("xlsx"))
                {
                    ModelState.AddModelError("", "File is Not in Correct Format!!!");
                }
                else if (uploadFile.ContentLength > 500000)
                {
                    ModelState.AddModelError("", "File is Too Long. Max Size is 500MB!!!");
                }
                else
                {
                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(DateTime.Now.ToString() + "_" + uploadFile.FileName);
                        var path = Path.Combine(Server.MapPath("/ExcelFiles"), fileName);
                        //if (System.IO.File.Exists(path))
                        //{
                        //    System.IO.File.Delete(path);
                        //}
                        uploadFile.SaveAs(path);
                        string connString = "";
                        string extension = Path.GetExtension(uploadFile.FileName);
                        if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            DataTable dt = ConvertXSLXtoDataTable(path, connString);
                            bool result = InsertIntoDB(dt);
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            DataTable dt = ConvertXSLXtoDataTable(path, connString);
                            bool result = InsertIntoDB(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        private bool InsertIntoDB(DataTable dt)
        {
            bool result = true;
            var cardData = db.CARD_ASSIGN_INFO.ToList();
            List<ATTENDANCE_DETAILS> attList = new List<ATTENDANCE_DETAILS>();
            int slNo = 0;
            string dateFlag = "";
            foreach (DataRow dRow in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dRow[0].ToString()))
                {
                    string cardNo = Convert.ToString(dRow[0]);
                    string check_in = Convert.ToString(dRow[2]);
                    string check_out = Convert.ToString(dRow[3]);
                    DateTime date = Convert.ToDateTime(dRow[5]);
                    string dateStr = date.ToString();
                    if (dateFlag != dateStr)
                    {
                        slNo = 0;
                        dateFlag = dateStr;
                    }
                    ATTENDANCE_DETAILS objIn = new ATTENDANCE_DETAILS();
                    ATTENDANCE_DETAILS objOut = new ATTENDANCE_DETAILS();
                    var empDataCard = cardData.Where(x => x.CARD_NO.Trim() == cardNo.Trim()).FirstOrDefault();
                    objIn.EMPLOYEE_ID = objOut.EMPLOYEE_ID = empDataCard.EMP_ID;
                    objIn.ATT_DATE = objOut.ATT_DATE = date;
                    objIn.CHECK_IN_TIME = TimeSpan.Parse(check_in);
                    slNo++;
                    objIn.SL_NO = slNo;
                    objIn.STATUS = ConstantValue.AttendanceCheckIn;
                    objOut.CHECK_OUT_TIME = TimeSpan.Parse(check_out);
                    slNo++;
                    objOut.SL_NO = slNo;
                    objOut.STATUS = ConstantValue.AttendanceCheckOut;
                    attList.Add(objIn);
                    attList.Add(objOut);
                }               
            }
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in attList)
                    {
                        db.ATTENDANCE_DETAILS.Add(obj);
                        db.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    result = false;
                }
            }
            if (result)
            {
                ViewBag.ResultSuccess = "Data Import Successfully!!!";
                ViewBag.ResultFailed = "";

            }
            else
            {
                ViewBag.ResultFailed = "Failed To Import!!!";
                ViewBag.ResultSuccess = "";
            }
            return result;
        }
        private DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }
    }
}

