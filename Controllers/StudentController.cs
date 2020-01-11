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
        HMSDataContext dc = new HMSDataContext();
        public ActionResult dashboard()
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
                int userid = (int)Session["user_id"];
                var a = dc.Users.First(x => x.user_id == userid);
                if (a != null && a.user_passwd == old)
                {
                    a.user_passwd = newpassword;
                    dc.SubmitChanges();
                }
                //else return Content("<script language='javascript' type='text/javascript'>alert('Save Successfully');</script>");
                return RedirectToAction("dashboard", "Student");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult change_password()
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
                    std_cnic = (decimal)Session["user_cnic"]
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