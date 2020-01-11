using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class HomeController : Controller
    {
        HMSDataContext dc = new HMSDataContext();
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult Login()
        {
            string user_name = Request["username"];
            string user_passwd = Request["password"];
            var obj = dc.Users.Where(a => a.user_name.Equals(user_name) && a.user_passwd.Equals(user_passwd)).FirstOrDefault();
            //Login check for Hostel Clerk
            if (obj != null && obj.user_role.Equals("hostel_clerk"))
            {
                Session["user_role"] = "hostel_clerk";
                Session["hostel_assign"] = obj.user_session;
                return RedirectToAction("add_allotment","HostelClerk");
            }
            //Login check for warden
            if (obj != null && obj.user_role.Equals("warden"))
            {
                Session["user_role"] = "warden";
                Session["hostel_assign"] = obj.user_session;
                return RedirectToAction("dashboard", "Student");
            }
            //Login check for Student
            else if (obj != null && obj.user_role.Equals("student"))
            {
                Session["user_role"] = "student";
                Session["user_id"] = obj.user_id;
                var a = dc.Students.First(x => x.user_id == obj.user_id);
                if (a == null)
                {
                    //genearte alert
                }
                Session["user_cnic"] = a.std_cnic;
                return RedirectToAction("dashboard", "Student");
            }
            //Login check for Hall Council Clerk
            else if (obj != null && obj.user_role.Equals("hc_clerk"))
            {
                Session["user_role"] = "hc_clerk";
                return RedirectToAction("addhostel", "HallCouncilClerk");
            }
            //Login check for VC and CHC
            else if (obj != null && (obj.user_role.Equals("vc") || obj.user_role.Equals("chc")))
            {
                Session["user_role"] = "vc/chc";
                return RedirectToAction("View_Hostels", "VCCHC");
            }
            else
            {
                //ViewBag.Message = "Incorrect username or password";
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}