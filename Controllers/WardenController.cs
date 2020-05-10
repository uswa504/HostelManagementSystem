﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class WardenController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Adduser()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                ViewBag.hostel = (int)Session["hostel"];
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int hostel = (int)Session["hostel"];
                string name = Request["user_name"];
                string passwd = Request["user_passwd"];
                System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
                byte[] passwordArray = encryptpwd.GetBytes(passwd);
                string role = "hostel_clerk";
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
                return RedirectToAction("Adduser", "Warden");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_rooms()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_MessDues()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_AnnualDues()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                  var t = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_session_month == DateTime.Now.ToString("year"));
                  return View(t);
                }
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Update(int id)
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
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
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_StudentsMesses.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_AnnualRecords()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.Dues.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change()
        {
            if (Session["user_role"].ToString() == "warden" || Session["user_role"].ToString() == "admin")
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
                return RedirectToAction("Manage_Dues", "Warden");
            }
            else return RedirectToAction("Index", "Home");
        }

    }
}