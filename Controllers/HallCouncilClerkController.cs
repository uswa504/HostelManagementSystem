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
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult hostelAdd()
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                int? hrooms;
                int hostel_number = int.Parse(Request["hostel_number"]);
                string hname = Request["hostel_name"];
                string hloc = Request["hlocation"];
                string htype = Request["htype"];
                hrooms = null;
                Hostel hostel = new Hostel
                {
                    hostel_no = hostel_number,
                    hostel_name = hname,
                    hostel_location = hloc,
                    hostel_type = htype,
                    hostel_roomCount = hrooms,
                    hostel_addedBy = (int)Session["user_id"],
                    time_of_addition= DateTime.Now,
                    hostel_activeStatus="active"
                };
                dc.Hostels.InsertOnSubmit(hostel);
                dc.SubmitChanges();
                return RedirectToAction("addhostel", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult adduser()
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
            int? hostel;
            string name = Request["user_name"];
            string passwd = Request["user_passwd"];
            System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
            byte[] passwordArray = encryptpwd.GetBytes(passwd);
            string role = Request["user_role"];
            if (int.Parse(Request["hostel"]) != 0)
            {
                hostel = int.Parse(Request["hostel"]);
            }
            else hostel = null;
            /*var a = dc.Users.First(x=> x.user_name == name);
            if(a != null)
            {
                ViewBag.Message = "Username already taken";
            }*/
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
            return RedirectToAction("adduser", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult view_hostels()
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult viewusers()
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Users.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult update_hostel(int id)
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Hostels.First(x => x.hostel_id == id);
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateHostel(int id)
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin") {
                int hostel_number = int.Parse(Request["hostel_number"]);
                string hname = Request["hostel_name"];
                string hloc = Request["hlocation"];
                string htype = Request["htype"];
                string hstatus = Request["hstatus"];
                var s = dc.Hostels.First(x => x.hostel_id == id);
                if (s != null)
                {
                    s.hostel_no = hostel_number;
                    s.hostel_name = hname;
                    s.hostel_location = hloc;
                    s.hostel_type = htype;
                    s.hostel_activeStatus = hstatus;
                    dc.SubmitChanges();
                    return RedirectToAction("view_hostels");
                }
                else return RedirectToAction("view_hostels");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteUser(int id)
        {
            if (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin")
            {
                var s = dc.Users.First(x => x.user_id == id);
                if (s != null)
                {
                    s.user_activeStatus = "unactive";
                    dc.SubmitChanges();
                    return RedirectToAction("viewusers");
                }
                else return RedirectToAction("viewusers");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult view_rooms(int id)
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                var a = dc.Rooms.Where(x=> x.hostel_id == id).ToList();
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
        public ActionResult Change()
        {
            if (Session["user_role"].ToString() == "hc_clerk")
            {
                string old = Request["oldpassword"];
                string newpassword = Request["newpassword"];
                System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
                byte[] passwordArray = encryptpwd.GetBytes(old);
                int userid = (int)Session["user_id"];
                var a = dc.Users.First(x => x.user_id == userid);
                if (a != null && a.user_passwd == passwordArray)
                {
                    byte[] newPasswordArray = encryptpwd.GetBytes(newpassword);
                    a.user_passwd = newPasswordArray;
                    dc.SubmitChanges();
                }
                return RedirectToAction("addhostel", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}