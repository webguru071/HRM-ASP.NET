using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMSApp.Helper;
using EMSApp.Models;

namespace EMSApp.Controllers
{
    public class EquipmentManageController : Controller
    {
        EMSEntities db = new EMSEntities();
        ConverterHelper converter = new ConverterHelper();

        // GET: EquipmentManage
        public ActionResult Index()
        {
           if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var data = db.ASSET_MANAGEMENT.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // GET: EquipmentManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EquipmentManage/Create
        public ActionResult Create()
        {
           if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                GetDataInBag();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }
        // POST: EquipmentManage/Create
        [HttpPost]
        public ActionResult Create(ASSET_MANAGEMENT collection)
        {
            try
            {
                if (collection.EMP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Emoployee");
                }
                else if (collection.EQUP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Equipment");
                }
                else if (string.IsNullOrEmpty(collection.DATE_ASSN))
                {
                    ModelState.AddModelError("", "Please Give Assigned Date");
                }
                else
                {
                    collection.ACTION_BY = converter.GetLoggedUserID();
                    collection.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.ASSET_MANAGEMENT.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
            GetDataInBag();
            return View();
        }

        // GET: EquipmentManage/Edit/5
        public ActionResult Edit(int id)
        {
           if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.ASSET_MANAGEMENT.Where(x => x.ASSET_MNG_ID == id).FirstOrDefault();
                GetDataInBag(dt.EMP_ID, dt.EQUP_ID);
                Session["AD"] = dt.ACTION_DATE;
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
            
        }

        // POST: EquipmentManage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ASSET_MANAGEMENT collection)
        {
            try
            {
                if (collection.EMP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Emoployee");
                }
                else if (collection.EQUP_ID <= 0)
                {
                    ModelState.AddModelError("", "Please Select Equipment");
                }
                else if (string.IsNullOrEmpty(collection.DATE_ASSN))
                {
                    ModelState.AddModelError("", "Please Give Assigned Date");
                }
                else
                {
                    collection.UPDATE_BY = converter.GetLoggedUserID();
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
            }
            catch (Exception ex)
            {
                GetDataInBag();
                return View();
            }
            GetDataInBag();
            return View();
        }

        // GET: EquipmentManage/Delete/5
        public ActionResult Delete(int id)
        {
           if (converter.CheckLogin() && converter.GetLoggedUserLevel()==ConstantValue.UserLevelAdmin)
            {
                var dt = db.ASSET_MANAGEMENT.Where(x => x.ASSET_MNG_ID == id).FirstOrDefault();
                return View(dt);
            }
            else
            {
                return RedirectToAction("LogIn", "Login");
            }
        }

        // POST: EquipmentManage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var dt = db.ASSET_MANAGEMENT.Where(x => x.ASSET_MNG_ID == id).FirstOrDefault();
                if (dt != null)
                {
                    db.ASSET_MANAGEMENT.Remove(dt);
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
        private void GetDataInBag(long empId = 0, long eqpId = 0)
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            ViewBag.EMP_ID = new SelectList(empList, "Value", "Text", empId);            
            List<SelectListItem> listEqp = new SelectList(db.EQUEPMENTS_INFO, "EQUEPMENT_ID", "EQP_TITLE").ToList();
            listEqp.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            ViewBag.EQUP_ID = new SelectList(listEqp, "Value", "Text", eqpId);
        }
    }
}
