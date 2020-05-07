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
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult Login()
        {
            string user_name = Request["username"];
            string user_passwd = Request["password"];
            System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
            byte[] passwordArray = encryptpwd.GetBytes(user_passwd);
            var obj = dc.Users.Where(a => a.user_name.Equals(user_name) && a.user_passwd.Equals(passwordArray)).FirstOrDefault();
            //Login check for Hostel Clerk
            if (obj != null && obj.user_role.Equals("hostel_clerk"))
            {
                Session["user_id"] = obj.user_id;
                Session["user_role"] = "hostel_clerk";
                Session["hostel"] = obj.hostel_id;
                return RedirectToAction("Add_allotment","HostelClerk");
            }
            //Login check for warden
            if (obj != null && obj.user_role.Equals("warden"))
            {

                Session["user_id"] = obj.user_id;
                Session["user_role"] = "warden";
                Session["hostel"] = obj.hostel_id;
                return RedirectToAction("Manage_Dues", "Warden");
            }
            //Login check for superitendant
            if (obj != null && obj.user_role.Equals("superitendant"))
            {
                Session["user_id"] = obj.user_id;
                Session["user_role"] = "superitendant";
                Session["hostel"] = obj.hostel_id;
                return RedirectToAction("View_Request", "Superitendant");
            }
            //Login check for Student
            else if (obj != null && obj.user_role.Equals("student"))
            {
                Session["user_role"] = "student";
                Session["user_id"] = obj.user_id;
                var a = dc.Students.First(x => x.user_id == obj.user_id);
                Session["user_cnic"] = a.std_cnic;
                var s = dc.Allottments.First(y => y.std_cnic == a.std_cnic && y.allotte_activeStatus == "active");
                if (a == null)
                {
                    //genearte alert
                }
                Session["allotte_id"] = s.allottee_id;
                return RedirectToAction("Dashboard", "Student");
            }
            //Login check for Hall Council Clerk
            else if (obj != null && (obj.user_role.Equals("hc_clerk") || obj.user_role.Equals("admin")))
            {
                Session["user_id"] = obj.user_id;
                Session["user_role"] = "hc_clerk";
                return RedirectToAction("addhostel", "HallCouncilClerk");
            }
            //Login check for VC
            else if (obj != null && obj.user_role.Equals("vc"))
            {
                Session["user_role"] = "vc";
                Session["user_id"] = obj.user_id;
                return RedirectToAction("View_Hostels", "ViceChancellor");
            }
            else if (obj != null && obj.user_role.Equals("chc"))
            {
                Session["user_role"] = "chc";
                Session["user_id"] = obj.user_id;
                return RedirectToAction("View_hostels", "ChairmanHallCouncil");
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