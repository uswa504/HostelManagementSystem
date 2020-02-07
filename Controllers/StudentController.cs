using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class StudentController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Dashboard()
        {
            if (Session["user_role"].ToString() == "student")
            {
                decimal cnic = (decimal)Session["user_cnic"];
                var a = dc.Allottments.Where(x => x.std_cnic == cnic);
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change()
        {
            if (Session["user_role"].ToString() == "student")
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
                return RedirectToAction("Dashboard");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "student")
            { 
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult send()
        {
            if (Session["user_role"].ToString() == "student")
            {
                string query = Request["query"];
                Request request = new Request()
                {
                    req_msg = query,
                    allotte_id = 9
                };
                dc.Requests.InsertOnSubmit(request);
                dc.SubmitChanges();
                return RedirectToAction("dashboard", "Student");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult send_request()
        {
            if (Session["user_role"].ToString() == "student")
            {

                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}