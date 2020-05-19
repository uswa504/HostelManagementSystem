using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    time_of_addition = DateTime.Now,
                    hostel_activeStatus = "active"
                };
                dc.Hostels.InsertOnSubmit(hostel);
                dc.SubmitChanges();
                return RedirectToAction("Addhostel", "ChairmanHallCouncil");
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