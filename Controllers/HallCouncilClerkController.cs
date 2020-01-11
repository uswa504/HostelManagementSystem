using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class HallCouncilClerkController : Controller
    {

        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult addhostel()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult hostelAdd()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                int hostel_number = int.Parse(Request["hostel_number"]);
                string hname = Request["hostel_name"];
                string hloc = Request["hlocation"];
                string htype = Request["htype"];
                int hrooms = int.Parse(Request["hostel_rooms"]);
                Hostel hostel = new Hostel
                {
                    hostel_no = hostel_number,
                    hostel_name = hname,
                    hostel_location = hloc,
                    hostel_type = htype,
                    hostel_rooms = hrooms
                };
                dc.Hostels.InsertOnSubmit(hostel);
                dc.SubmitChanges();
                return RedirectToAction("addhostel", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                string name = Request["user_name"];
                string passwd = Request["user_passwd"];
                string role = Request["user_role"];
                int hostel = int.Parse(Request["hostel"]);
                var a = dc.Users.First(x=> x.user_name == name);
                if(a != null)
                {
                    ViewBag.Message = "Username already taken";
                }
                User user = new User
                {
                   user_name = name,
                   user_passwd = passwd,
                   user_role = role,
                   user_session = hostel
                };
                dc.Users.InsertOnSubmit(user);
                dc.SubmitChanges();
                return RedirectToAction("adduser", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult roomAdd()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                int hostelId = int.Parse(Request["hostel_id"]);
                int room_number = int.Parse(Request["rmno"]);
                string room_type = Request["rtype"];
                int capacity = int.Parse(Request["seater"]);
                Room room = new Room
                {
                    room_id = room_number,
                    room_type = room_type,
                    room_capacity = capacity,
                    hostel_id = hostelId
                };
                dc.Rooms.InsertOnSubmit(room);
                dc.SubmitChanges();
                return RedirectToAction("addroom", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult addroom()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult adduser()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult view_hostels()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                var a = dc.Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult viewusers()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                var a = dc.Users.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        /*public ActionResult DeleteHostel(int id)
        {
            var s = dc.Hostels.First(x => x.hostel_id == id);
            dc.Hostels.DeleteOnSubmit(s);
            var b = dc.Allottments.Where(x => x.hostel_id == id).ToList();
            dc.Allottments.DeleteOnSubmit(b);
            var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
            dc.Rooms.DeleteOnSubmit(a);
            dc.SubmitChanges();
            return RedirectToAction("view_hostels");
        }*/
        /* public ActionResult DeleteRoom(int id)
         {
            var s = dc.Hostels.First(x => x.hostel_id == id);
            dc.Hostels.DeleteOnSubmit(s);
            var b = dc.Allottments.Where(x => x.hostel_id == id).ToList();
            dc.Allottments.DeleteOnSubmit(b);
            var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
            dc.Rooms.DeleteOnSubmit(a);
            dc.SubmitChanges();
            return RedirectToAction("view_hostels");
         }*/
        /*public ActionResult DeleteUser(int id)
        {
           var s = dc.Hostels.First(x => x.hostel_id == id);
           dc.Hostels.DeleteOnSubmit(s);
           var b = dc.Allottments.Where(x => x.hostel_id == id).ToList();
           dc.Allottments.DeleteOnSubmit(b);
           var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
           dc.Rooms.DeleteOnSubmit(a);
           dc.SubmitChanges();
           return RedirectToAction("view_hostels");
        }*/
        public ActionResult view_rooms()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                var a = dc.Rooms.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult change_password()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}