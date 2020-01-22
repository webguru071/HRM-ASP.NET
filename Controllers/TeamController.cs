using EMSApp.Models;
using EMSApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EMSApp.Controllers
{
    public class TeamController : Controller
    {
        EMSEntities db = new EMSEntities();
        ICombine service = new CombineServices();
        // GET: Team
        public ActionResult Index()
        {
            var data = db.TEAM_INFO.Where(x => x.STATUS == "a").ToList();
            return View(data);
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            GetDataInBag();
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(TEAM_INFO team)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(team.TEAM_TITLE))
                {
                    ModelState.AddModelError("", "Team Title is Required!!");
                }
                else if (team.TEAM_LEADER <= 0)
                {
                    ModelState.AddModelError("", "Team Leader is Required!!");
                }
                else if (string.IsNullOrEmpty(team.STATUS))
                {
                    ModelState.AddModelError("", "Status is Required!!");
                }
                else
                {
                    team.ACTION_BY = Convert.ToInt64(Session["USER_ID"]);
                    team.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.TEAM_INFO.Add(team);
                        db.SaveChanges();
                        return RedirectToAction("Index");
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
        private void GetDataInBag(long id = 0, string status = "")
        {
            ViewBag.TEAM_LEADER = new SelectList(SetEmployee(), "Value", "Text", id);
            ViewBag.STATUS = new SelectList(SetStatusList(), "Value", "Text", status);
        }
        private List<SelectListItem> SetStatusList()
        {
            var activeStatus = new List<SelectListItem>
            {
                new SelectListItem{ Text="Active", Value = "a" },
                new SelectListItem{ Text="Deactive", Value = "d" }
            };
            activeStatus.Insert(0, (new SelectListItem { Text = "Select One", Value = "" }));
            return activeStatus;
        }
        private List<SelectListItem> SetEmployee()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.TEAM_INFO.Where(x => x.ID == id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            Session["AT"] = data.ACTION_BY;
            GetDataInBag(data.TEAM_LEADER, data.STATUS);
            return View(data);
        }

        // POST: Team/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TEAM_INFO team)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(team.TEAM_TITLE))
                {
                    ModelState.AddModelError("", "Team Title is Required!!");
                }
                else if (team.TEAM_LEADER <= 0)
                {
                    ModelState.AddModelError("", "Team Leader is Required!!");
                }
                else if (string.IsNullOrEmpty(team.STATUS))
                {
                    ModelState.AddModelError("", "Status is Required!!");
                }
                else
                {
                    team.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    team.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    team.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(team).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("Index");
                    }
                }
                GetDataInBag(team.TEAM_LEADER, team.STATUS);
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBag(team.TEAM_LEADER, team.STATUS);
                return View();
            }
        }

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Team/Delete/5
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
        public ActionResult TeamMemberIndex()
        {
            var data = db.TEAM_DETAILS.Where(x => x.STATUS == "a").OrderBy(x => x.TEAM_ID).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult TeamMember()
        {
            GetDataInBagForMember();
            return View();
        }
        [HttpPost]
        public ActionResult TeamMember(TEAM_DETAILS details)
        {
            try
            {
                // TODO: Add insert logic here
                if (details.TEAM_ID <= 0)
                {
                    ModelState.AddModelError("", "Team is Required!!");
                }
                else if (details.MEMBER <= 0)
                {
                    ModelState.AddModelError("", "Team Member is Required!!");
                }
                else if (string.IsNullOrEmpty(details.STATUS))
                {
                    ModelState.AddModelError("", "Status is Required!!");
                }
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    details.ACTION_BY = userID;
                    details.ACTION_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.TEAM_DETAILS.Add(details);
                        db.SaveChanges();
                        return RedirectToAction("TeamMemberIndex");
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
            return View();
        }
        [HttpGet]
        public ActionResult TeamMemberEdit(int id)
        {
            var data = db.TEAM_DETAILS.Where(x => x.ID == id).FirstOrDefault();
            Session["AD"] = data.ACTION_DATE;
            Session["AT"] = data.ACTION_BY;
            GetDataInBagForMember(data.TEAM_ID, data.MEMBER, data.STATUS);
            return View(data);
        }
        [HttpPost]
        public ActionResult TeamMemberEdit(int id, TEAM_DETAILS details)
        {
            try
            {
                // TODO: Add insert logic here
                if (details.TEAM_ID <= 0)
                {
                    ModelState.AddModelError("", "Team is Required!!");
                }
                else if (details.MEMBER <= 0)
                {
                    ModelState.AddModelError("", "Team Member is Required!!");
                }
                else if (string.IsNullOrEmpty(details.STATUS))
                {
                    ModelState.AddModelError("", "Status is Required!!");
                }
                else
                {
                    long userID = Convert.ToInt64(Session["USER_ID"]);
                    details.UPDATE_BY = Convert.ToInt64(Session["USER_ID"]);
                    details.ACTION_DATE = Convert.ToDateTime(Session["AD"]);
                    details.UPDATE_DATE = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Entry(details).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["AD"] = null;
                        return RedirectToAction("TeamMemberIndex");
                    }
                }
                GetDataInBagForMember(details.TEAM_ID, details.MEMBER, details.STATUS);
                return View();
            }
            catch (Exception ex)
            {
                GetDataInBagForMember(details.TEAM_ID, details.MEMBER, details.STATUS);
                return View();
            }
        }
        private void GetDataInBagForMember(long teamId = 0, long member = 0, string status = "")
        {
            ViewBag.MEMBER = new SelectList(SetMember(), "Value", "Text", member);
            ViewBag.TEAM_ID = new SelectList(SetTeam(), "Value", "Text", teamId);
            ViewBag.STATUS = new SelectList(SetStatusList(), "Value", "Text", status);
        }

        private List<SelectListItem> SetMember()
        {
            List<SelectListItem> empList = new SelectList(db.EMPLOYEE_INFO, "ID", "EMPLOYEE_NAME").ToList();
            empList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return empList;
        }
        private List<SelectListItem> SetTeam()
        {
            List<SelectListItem> teamList = new SelectList(db.TEAM_INFO, "ID", "TEAM_TITLE").ToList();
            teamList.Insert(0, (new SelectListItem { Text = "Select One", Value = "0" }));
            return teamList;
        }
        public JsonResult GetTeamLeader(int id)
        {
            string leaderName = service.GetTeamLeaderById(id);
            return Json(leaderName, JsonRequestBehavior.AllowGet);
        }
    }
}
