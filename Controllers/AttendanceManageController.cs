﻿using System;
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
            DataTable result = new DataTable();
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
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";                            
                        }                      
                        DataTable dt = ConvertXSLXtoDataTable(path, connString);
                        if (dt != null)
                        {
                            result = InsertIntoDB(dt);
                            if (result!=null)
                            {
                                ViewBag.ResultSuccess = "Data Import Successfully!!!";
                                ViewBag.ResultFailed = "";

                            }
                            else
                            {
                                ViewBag.ResultFailed = "Failed To Import!!!";
                                ViewBag.ResultSuccess = "";
                            }
                        }
                        else
                        {
                            ViewBag.ResultFailed = "Data is Not Matched or Empty!!!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(result);
        }
        private DataTable InsertIntoDB(DataTable dt)
        {
            bool result = new bool();
            var cardData = db.CARD_ASSIGN_INFO.ToList();            
            DataTable dtNew = new DataTable();
            dtNew.Columns.AddRange(new DataColumn[6] { new DataColumn("EMPLOYEE_ID", typeof(int)),
                        new DataColumn("ATT_DATE", typeof(DateTime)),
                        new DataColumn("CHECK_IN_TIME", typeof(TimeSpan)),
                        new DataColumn("CHECK_OUT_TIME", typeof(TimeSpan)),
                        new DataColumn("SL_NO", typeof(int)),
                        new DataColumn("STATUS",typeof(string)) });
            int slNoIN = 0;
            int slNoOUT = 0;
            string dateFlag = "";
            try
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dRow[0].ToString()))
                    {
                        string cardNo = Convert.ToString(dRow[0]);
                        if (Convert.ToInt32(cardNo) == 24)
                        {
                            string my = cardNo;
                        }
                        string check_in = Convert.ToString(dRow[2]);
                        string check_out = Convert.ToString(dRow[3]);
                        DateTime date = Convert.ToDateTime(dRow[5]);
                        string dateStr = date.ToString();
                        if (dateFlag != dateStr)
                        {
                            slNoIN = 1;
                            slNoOUT = slNoIN + 1;
                            dateFlag = dateStr;
                        }
                        else
                        {
                            slNoIN= slNoOUT+1;
                            slNoOUT=slNoIN+1;
                        }
                        var empDataCard = cardData.Where(x => x.CARD_NO.Trim() == cardNo.Trim()).FirstOrDefault();
                        if (empDataCard != null)
                        {
                            dtNew.Rows.Add(empDataCard.EMP_ID, date, TimeSpan.Parse(check_in), null, slNoIN, ConstantValue.AttendanceCheckIn);
                            dtNew.Rows.Add(empDataCard.EMP_ID, date, null, TimeSpan.Parse(check_out), slNoOUT, ConstantValue.AttendanceCheckOut);
                        }                        
                    }
                }
            }
            catch(Exception ex)
            {
                result = false;
            }
            int count = dtNew.Rows.Count;
            DBHelper dbHelper = new DBHelper();
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(DBHelper.GetConnString()))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name
                            sqlBulkCopy.DestinationTableName = "dbo.ATTENDANCE_DETAILS";

                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            sqlBulkCopy.ColumnMappings.Add("EMPLOYEE_ID", "EMPLOYEE_ID");
                            sqlBulkCopy.ColumnMappings.Add("ATT_DATE", "ATT_DATE");
                            sqlBulkCopy.ColumnMappings.Add("CHECK_IN_TIME", "CHECK_IN_TIME");
                            sqlBulkCopy.ColumnMappings.Add("CHECK_OUT_TIME", "CHECK_OUT_TIME");
                            sqlBulkCopy.ColumnMappings.Add("SL_NO", "SL_NO");
                            sqlBulkCopy.ColumnMappings.Add("STATUS", "STATUS");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dtNew);
                            con.Close();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                 
                }
            }            
            return dtNew;
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

