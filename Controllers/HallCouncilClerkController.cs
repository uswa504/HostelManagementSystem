﻿using System;
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
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult hostelAdd()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
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
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                var a = dc.Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
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
            return RedirectToAction("adduser", "HallCouncilClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult view_hostels()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                var a = dc.View_Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult viewusers()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                var a = dc.View_Users.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateUser(int id)
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                var a = dc.Users.First(x => x.user_id == id);
                if (a != null) {
                    string status = Request["status"];
                    a.user_activeStatus = status;
                    dc.SubmitChanges();
                }
                return View("viewusers");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult view_rooms(int id)
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                var a = dc.Rooms.Where(x=> x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult change_password()
        {
            if (Session["user_role"] != null && (Session["user_role"].ToString() == "hc_clerk" || Session["user_role"].ToString() == "admin"))
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}