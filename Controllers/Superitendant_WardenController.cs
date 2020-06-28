using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class Superitendant_WardenController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Adduser()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                ViewBag.hostel = (int)Session["hostel"];
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Allottment()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                int hostel = (int)Session["hostel"];
                var a = dc.View_Allottments.Where(s => s.hostel_id == hostel && s.allotte_activeStatus == "active").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult View_Info(int id)
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.First(x => x.allottee_id == id);
                decimal cnic = (decimal)a.std_cnic;
                model.b = dc.Educations.Where(x => x.std_cnic == cnic).ToList();
                model.c = dc.Sessions.Where(x => x.std_cnic == cnic && x.session_activeStatus == "active");
                var t = dc.Sessions.First(x => x.std_cnic == cnic && x.session_activeStatus == "active");
                var z = dc.Departments.First(b => b.dep_id == t.dep_id);
                ViewBag.dep = z.dep_name;
                model.d = dc.Students.Where(x => x.std_cnic == cnic);
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_rooms()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                int id = (int)Session["hostel"];
                var a = dc.View_Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_MessDues()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden")
            {
                List<List<Due>> data = new List<List<Due>>();
                List<string> names = new List<string>();
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    var b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && (d.dues_type == "mess" || d.dues_type == "partial mess")).ToList();
                    data.Add(b);
                    names.Add(s.std_name);
                }
                ViewBag.names = names;
                ViewBag.data = data;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_AnnualDues()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                List<List<Due>> data = new List<List<Due>>();
                List<string> names = new List<string>();
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    var b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && (d.dues_type == "annual" || d.dues_type == "partial annual")).ToList();
                    data.Add(b);
                    names.Add(s.std_name);
                }
                ViewBag.names = names;
                ViewBag.data = data;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult View_StudentsMess()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                List<List<Due>> data = new List<List<Due>>();
                List<string> names = new List<string>();
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id);
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    var b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && (d.dues_type == "mess" || d.dues_type == "partial mess")).ToList();
                    data.Add(b);
                    names.Add(s.std_name);
                }
                ViewBag.names = names;
                ViewBag.data = data;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_AnnualRecords()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden"))
            {
                List<List<Due>> data = new List<List<Due>>();
                List<string> names = new List<string>();
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id);
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    var b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && (d.dues_type == "annual" || d.dues_type == "partial annual")).ToList();
                    data.Add(b);
                    names.Add(s.std_name);
                }
                ViewBag.names = names;
                ViewBag.data = data;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateMess(int id, int user_id, string month, string amount, string lastDate, string paidStatus, string paidDate, string recipt)
        {
            var a = dc.Dues.First(s => s.allottee_id == id && s.dues_session_month.Equals(month));
            if (a != null && Session["user_role"].ToString() == "superitendant")
            {
                a.dues_amount = amount == "" ? 0 : int.Parse(amount);
                if (lastDate != "")
                    a.dues_lastDate = DateTime.Parse(lastDate);
                else a.dues_lastDate = null;
                if (paidStatus != "")
                    a.dues_paidStatus = paidStatus;
                else a.dues_paidStatus = null;
                if (paidDate != "")
                    a.dues_paidDate = DateTime.Parse(paidDate);
                else a.dues_paidDate = null;
                if (recipt != "")
                    a.dues_recipt_no = recipt;
                else a.dues_recipt_no = null;
                a.dues_addedBy = user_id;
                a.time_of_addition = DateTime.Now;
                dc.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateAnnual(int id, int user_id, string year, string amount, string lastDate, string paidStatus, string paidDate, string recipt)
        {
            var a = dc.Dues.First(s => s.allottee_id == id && s.dues_session_month.Equals(year));
            if (a != null)
            {
                a.dues_amount = amount == "" ? 0 : int.Parse(amount);
                if (lastDate != "")
                    a.dues_lastDate = DateTime.Parse(lastDate);
                else a.dues_lastDate = null;
                if (paidStatus != "")
                    a.dues_paidStatus = paidStatus;
                else a.dues_paidStatus = null;
                if (paidDate != "")
                    a.dues_paidDate = DateTime.Parse(paidDate);
                else a.dues_paidDate = null;
                if (recipt != "")
                    a.dues_recipt_no = recipt;
                else a.dues_recipt_no = null;
                a.dues_addedBy = user_id;
                a.time_of_addition = DateTime.Now;
                dc.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult change_password()
        {

            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult AddPartialAnnual(int id, string year)
        {
            Due due = new Due
            {
                allottee_id = id,
                dues_type = "partial annual",
                dues_amount = null,
                dues_session_month = year,
                dues_lastDate = null,
                dues_paidStatus = null,
                dues_paidDate = null,
                dues_addedBy = null,
                dues_recipt_no = null,
                time_of_addition = DateTime.Now
            };
            dc.Dues.InsertOnSubmit(due);
            dc.SubmitChanges();
            return RedirectToAction("Manage_AnnualDues");
        }
        public ActionResult AddPartialMess(int id, string month)
        {
            Due due = new Due
            {
                allottee_id = id,
                dues_type = "partial mess",
                dues_amount = null,
                dues_session_month = month,
                dues_lastDate = null,
                dues_paidStatus = null,
                dues_paidDate = null,
                dues_addedBy = null,
                dues_recipt_no = null,
                time_of_addition = DateTime.Now
            };
            dc.Dues.InsertOnSubmit(due);
            dc.SubmitChanges();
            return RedirectToAction("Manage_MessDues");
        }
    }
}