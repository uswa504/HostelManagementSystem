using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class ChairmanHallCouncilController : Controller
    {

        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Addhostel()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult hostelAdd()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                int hostel_number = int.Parse(Request["hostel_number"]);
                string hname = Request["hostel_name"];
                string hloc = Request["hlocation"];
                string htype = Request["htype"];
                Hostel hostel = new Hostel
                {
                    hostel_no = hostel_number,
                    hostel_name = hname,
                    hostel_location = hloc,
                    hostel_type = htype,
                    hostel_roomCount = 0,
                    hostel_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now,
                    hostel_activeStatus = "active"
                };
                dc.Hostels.InsertOnSubmit(hostel);
                dc.SubmitChanges();
                return RedirectToAction("Addhostel", "ChairmanHallCouncil");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult adduser()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                int hostel = 0;
                string name = Request["user_name"];
                string passwd = Request["user_passwd"];
                System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
                byte[] passwordArray = encryptpwd.GetBytes(passwd);
                string role = Request["user_role"];
                if (int.Parse(Request["hostel"]) != 0)
                {
                    hostel = int.Parse(Request["hostel"]);
                }
                User user = new User
                {
                    user_name = name,
                    user_passwd = passwordArray,
                    user_role = role,
                    user_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now,
                    user_activeStatus = "active",
                    hostel_id = hostel
                };
                dc.Users.InsertOnSubmit(user);
                dc.SubmitChanges();
                return RedirectToAction("adduser", "ChairmanHallCouncil");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_hostels()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_rooms(int id)
        {
            if (Session["user_role"].ToString() == "chc")
            {
                var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Users()
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Users.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult See_Info(int id)
        {
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
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
            if (Session["user_role"].ToString() == "chc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Allottments.Where(s => s.allotte_activeStatus == "active").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "chc")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}