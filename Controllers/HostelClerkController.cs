using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Hostel_Management_System.Models;

namespace Online_Hostel_Management_System.Controllers
{
    public class HostelClerkController : Controller
    {
        readonly HMSDataContext dc = new HMSDataContext();
        public ActionResult Addroom() { 
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Add_allotment()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int hostel = (int)Session["hostel"];
                var a = dc.Rooms.Where(x=> x.hostel_id == hostel).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult View_rooms()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_Rooms.Where(x => x.hostel_id == id).ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Add_room()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int roomno = int.Parse(Request["rmno"]);
                int seater = int.Parse(Request["seater"]);
                string rtype = Request["rtype"];
                Room room = new Room
                {
                    room_id = roomno,
                    room_type = rtype,
                    room_capacity = seater,
                    room_addedBy = (int)Session["user_id"],
                    room_status = "active",
                    time_of_addition = DateTime.Now,
                    hostel_id = (int)Session["hostel"]
                };
                dc.Rooms.InsertOnSubmit(room);
                dc.SubmitChanges();
                return RedirectToAction("Addroom");
            }
            else return RedirectToAction("Index", "Home");
        }

        /*public ActionResult student_info()
        {
            string username = Request["stud_username"];
            string passwd = Request["stud_passwd"];
            User user = new User
            {
                user_name = username,
                user_passwd = passwd,
                user_role = "student",
                user_session = null
            };
            dc.Users.InsertOnSubmit(user);
            dc.SubmitChanges();
            string std_name = Request["name"];
            decimal std_cnic = decimal.Parse(Request["cnic"]);
            string std_fatherName = Request["fname"];
            string std_fatherOccupation = Request["occupation"];
            string std_fatherIncome = Request["income"];
            string std_presentAddress = Request["paddress"];
            string std_permanentAddress = Request["permanentaddress"];
            string district = Request["district"];
            decimal std_phone = decimal.Parse(Request["stu_num"]);
            decimal std_parentPhone = decimal.Parse(Request["grd_num"]);
            string std_bloodGroup = Request["bloodGroup"];
            string std_HBSAg_report = Request["hbsag"];
            string std_antiCV_report = Request["hcv"];
            string std_nationality = Request["nationality"];

            Student std = new Student
            {
                std_cnic = std_cnic,
                std_name = std_name,
                std_fatherName = std_fatherName,
                std_fatherOccupation = std_fatherOccupation,
                std_presentAddress = std_presentAddress,
                std_permanentAddress = std_permanentAddress,
                std_phone = std_phone,
                std_parentPhone = std_parentPhone,
                std_district = district,
                std_bloodGroup = std_bloodGroup,
                std_HBSAg_report = std_HBSAg_report,
                std_AntiCV_report = std_antiCV_report,
                std_nationality = std_nationality,
                user_id = user.user_id
            };
            dc.Students.InsertOnSubmit(std);
            dc.SubmitChanges();
            int room_number = int.Parse(Request["room_number"]);
            string room_type = Request["room_type"];
            string stay_duration = Request["stay_duration"];
            DateTime session_end = DateTime.Parse(Request["session_end"]);
            DateTime sessionStart = DateTime.Parse(Request["session_start"]);
            string total_dues = Request["total_dues"];
            Allottment allottment = new Allottment
            {
                room_id = room_number,
                hostel_id = (int)Session["hostel_assign"],
                stay_duration = stay_duration,
                session_start = sessionStart,
                session_end = session_end,
                std_cnic = std.std_cnic
            };
            dc.Allottments.InsertOnSubmit(allottment);
            dc.SubmitChanges();
            string dpt_name = Request["dep_name"];
            string dpt_rollno = Request["roll_no"];
            string dpt_degree = Request["class"];
            Department dept = new Department
            {
                dep_name = dpt_name,
                dep_rollno = dpt_rollno,
                dep_degree = dpt_degree,
                dep_session = allottment.stay_duration,
                std_cnic = std.std_cnic
            };
            dc.Departments.InsertOnSubmit(dept);
            dc.SubmitChanges();
            return RedirectToAction("add_allotment");
        }*/
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Change()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
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
                return RedirectToAction("View_Allottment", "HostelClerk");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}