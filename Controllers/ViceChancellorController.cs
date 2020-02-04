using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{

    public class ViceChancellorController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult View_hostels()
        {
            if (Session["user_role"].ToString() == "vc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Hostels.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_rooms(int id)
        {
            if (Session["user_role"].ToString() == "vc" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "vc" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change()
        {
            if (Session["user_role"].ToString() == "vc" || Session["user_role"].ToString() == "admin")
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
                return View("View_hostels");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}