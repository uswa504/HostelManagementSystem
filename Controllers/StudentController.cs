using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
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

                dynamic model = new ExpandoObject();
                model.a = dc.Allottments.Where(x => x.std_cnic == cnic && x.allotte_activeStatus == "active");
                model.b = dc.Students.Where(y => y.std_cnic == cnic);
                var t = dc.Allottments.First(y => y.std_cnic == cnic);
                model.c = dc.Hostels.Where(z => z.hostel_id == t.hostel_id);
                //model.d = dc.Rooms.First(c => c.hostel_id == t.hostel_id);
                return View(model);
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
    }
}