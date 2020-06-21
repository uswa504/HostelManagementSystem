using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{

    public class ViceChancellorController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult View_hostels()
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                var a = dc.View_Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_rooms(int id)
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Users()
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                var a = dc.View_Users.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult See_Info(int id)
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                dynamic model = new ExpandoObject();
                model.a = dc.Allottments.First(x => x.allottee_id == id);
                var a = dc.Allottments.First(x => x.allottee_id == id);
                model.b = dc.Students.Where(y => y.std_cnic == a.std_cnic);
                var t = dc.Allottments.First(r => r.std_cnic == a.std_cnic);
                model.c = dc.Hostels.Where(z => z.hostel_id == t.hostel_id);
                model.d = dc.Rooms.Where(c => c.hostel_id == t.hostel_id && c.room_id == t.room_id);
                model.e = dc.Educations.Where(x => x.std_cnic == a.std_cnic).ToList();
                model.f = dc.Sessions.Where(x => x.std_cnic == a.std_cnic && x.session_activeStatus == "active");
                model.g = dc.Departments.ToList();
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Allottments()
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                var a = dc.View_Allottments.Where(s => s.allotte_activeStatus == "active").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"] != null && Session["user_role"].ToString() == "vc")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}