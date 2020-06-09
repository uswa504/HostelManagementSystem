using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class StudentController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Dashboard()
        {
            if (Session["user_role"].ToString() == "student")
            {
                decimal cnic = (decimal)Session["user_cnic"];

                dynamic model = new ExpandoObject();
                model.a = dc.Allottments.Where(x => x.std_cnic == cnic && x.allotte_activeStatus == "active");
                model.b = dc.Students.Where(y => y.std_cnic == cnic);
                var t = dc.Allottments.First(y => y.std_cnic == cnic);
                model.c = dc.Hostels.Where(z => z.hostel_id == t.hostel_id);
                model.d = dc.Rooms.Where(c => c.hostel_id == t.hostel_id && c.room_id == t.room_id);
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_AnnualDues()
        {
            if (Session["user_role"].ToString() == "student")
            {
                decimal cnic = (decimal)Session["user_cnic"];
                var x = dc.Allottments.First(q => q.std_cnic == cnic);
                var a = dc.Dues.Where(s => s.allottee_id == x.allottee_id && s.dues_type == "annual").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_MessDues()
        {
            if (Session["user_role"].ToString() == "student")
            {
                decimal cnic = (decimal)Session["user_cnic"];
                var x = dc.Allottments.First(q => q.std_cnic == cnic);
                var a = dc.Dues.Where(s => s.allottee_id == x.allottee_id && s.dues_type == "mess").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "student")
            { 
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}