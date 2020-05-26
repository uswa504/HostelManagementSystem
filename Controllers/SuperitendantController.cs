using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class SuperitendantController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Adduser()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                ViewBag.hostel = (int)Session["hostel"];
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        
        public ActionResult View_rooms()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_MessDues()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "mess");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_AnnualDues()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "annual");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateAnnual(int id)
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Dues.First(s => s.allottee_id == id);
                if (a != null)
                {
                    dc.SubmitChanges();
                    return RedirectToAction("Manage_AnnualDues");
                }
                else return RedirectToAction("Manage_AnnualDues");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_StudentsMess()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_StudentsMesses.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_AnnualRecords()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "annual");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult change_password()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}