using System;
using System.Web.Security;
using System.Linq;
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
            var obj = dc.Users.Where(a => a.user_name.Equals(user_name) && a.user_passwd.Equals(passwordArray) && a.user_activeStatus == "active").FirstOrDefault();
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
                return RedirectToAction("Manage_MessDues", "Warden");
            }
            //Login check for superitendant
            if (obj != null && obj.user_role.Equals("superitendant"))
            {
                Session["user_id"] = obj.user_id;
                Session["user_role"] = "superitendant";
                Session["hostel"] = obj.hostel_id;
                return RedirectToAction("adduser", "Superitendant");
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
        public ActionResult Change()
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
                if(a.user_role == "hostel_clerk") {
                    return View("Change_password","HostelClerk");
                }
                else if (a.user_role == "warden")
                {
                    return View("Change_password", "Warden");
                }
                else if (a.user_role == "superitendant")
                {
                    return View("Change_password", "Superitendant");
                }
                else if (a.user_role == "student")
                {
                    return View("Change_password", "Student");
                }
                else if (a.user_role == "vc")
                {
                    return View("Change_password", "ViceChancellor");
                }
                else if (a.user_role == "chc")
                {
                    return View("Change_password", "ChairmanHallCouncil");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult UserAdd()
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "warden")
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
                return RedirectToAction("Adduser", "Superitendant");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}