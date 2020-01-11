using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class VCCHCController : Controller
    {

        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Hostel_Details(int id)
        {
            if (Session["user_role"].ToString() == "vc/chc")
            {
                var a = dc.Hostels.First(s=> s.hostel_id == id);
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Hostels()
        {
            if (Session["user_role"].ToString() == "vc/chc")
            {
                var a = dc.Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}