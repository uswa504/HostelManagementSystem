using Online_Hostel_Management_System.Models;
using System;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.Mvc.Html;
using System.Web;
using System.Drawing;
using System.IO;

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
        public ActionResult View_Allottment()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int hostel = (int)Session["hostel"];
                var a = dc.View_Allottments.Where(s=> s.hostel_id == hostel && s.allotte_activeStatus == "active").ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult Add_allotment()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int hostel = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                model.a = dc.Rooms.Where(x=> x.hostel_id == hostel).ToList();
                model.b = dc.Departments.ToList();
                return View(model);
            }
            else return RedirectToAction("Index", "Home"); ;
        }
        public ActionResult Add(int id)
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int hostel = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Students.First(x => x.std_id == id);
                
                decimal cnic = (decimal)a.std_cnic;
                model.a = dc.Departments.ToList();
                model.b = dc.Educations.Where(x => x.std_cnic == cnic).ToList();
                model.c = dc.Sessions.Where(x => x.std_cnic == cnic && x.session_activeStatus == "active");
                model.d = dc.Students.Where(x => x.std_cnic == cnic);
                model.e = dc.Rooms.Where(x => x.hostel_id == hostel).ToList();
                model.f = dc.Users.Where(b=> b.user_id == a.user_id);
               
                return View(model);
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
        public ActionResult View_Students()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.View_Students.ToList();
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
                    room_no = roomno,
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

        public ActionResult student_info()
        {
            string username = Request["stud_username"];
            string passwd = Request["stud_passwd"];
            System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
            byte[] passwordArray = encryptpwd.GetBytes(passwd);
            int? hostel = null;
            User user = new User
            {
                user_name = username,
                user_passwd = passwordArray,
                user_role = "student",
                user_addedBy = (int)Session["user_id"],
                time_of_addition = DateTime.Now,
                user_activeStatus = "active",
                hostel_id = hostel
            };
            dc.Users.InsertOnSubmit(user);
            dc.SubmitChanges();
            string std_name = Request["name"];
            decimal std_cnic = decimal.Parse(Request["cnic"]);
            string std_fatherName = Request["fname"];
            string std_fatherOccupation = Request["occupation"];
            decimal std_fatherIncome = decimal.Parse(Request["income"]);
            string std_presentAddress = Request["paddress"];
            string std_permanentAddress = Request["permanentaddress"];
            string district = Request["district"];
            decimal std_phone = decimal.Parse(Request["stu_num"]);
            decimal std_parentPhone = decimal.Parse(Request["grd_num"]);
            string std_bloodGroup = Request["bloodGroup"];
            string std_HBSAg_report = Request["hbsag"];
            string std_antiCV_report = Request["hcv"];
            string std_nationality = Request["nationality"];
            HttpPostedFileBase file = Request.Files["ImageData"];
            var allowedExtensions = new[] {".Jpg", ".png", ".jpg", "jpeg"};
            var fileName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(ext))
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                string myfile = name + "_" + std_cnic + ext;
                var path = Path.Combine(Server.MapPath("~/Content/img"), myfile);
                var path2 ="~/Content/img/"+ myfile;
                file.SaveAs(path);
                Student std = new Student
                {
                    std_cnic = std_cnic,
                    std_name = std_name,
                    std_fatherName = std_fatherName,
                    std_fatherOccupation = std_fatherOccupation,
                    std_fatherIncome = std_fatherIncome,
                    std_presentAddress = std_presentAddress,
                    std_permanentAddress = std_permanentAddress,
                    std_phone = std_phone,
                    std_parentPhone = std_parentPhone,
                    std_district = district,
                    std_bloodGroup = std_bloodGroup,
                    std_HBSAg_report = std_HBSAg_report,
                    std_antiHCV_report = std_antiCV_report,
                    std_nationality = std_nationality,
                    std_activeStatus = "active",
                    std_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now,
                    user_id = user.user_id,
                    std_img = path2
                };
                dc.Students.InsertOnSubmit(std);
                dc.SubmitChanges();
                int room_number = int.Parse(Request["room_number"]);
                Allottment allottment = new Allottment
                {
                    room_id = room_number,
                    hostel_id = (int)Session["hostel"],
                    allotte_type = "regular",
                    allotte_activeStatus = "active",
                    time_of_addition = DateTime.Now,
                    allotte_addedBy = (int)Session["user_id"],
                    std_cnic = std.std_cnic
                };
                dc.Allottments.InsertOnSubmit(allottment);
                dc.SubmitChanges();
                int dpt_id = int.Parse(Request["dep_name"]);
                string rollN0 = Request["roll_no"];
                string degree = Request["class"];
                int batch = int.Parse(Request["session"]);
                DateTime session_start = DateTime.Parse(Request["session_start"]);
                DateTime session_end = DateTime.Parse(Request["session_end"]);
                string start = session_start.ToString("yyyy");
                string end = session_end.ToString("yyyy");
                int duration = int.Parse(end) - int.Parse(start);
                Session session = new Session
                {
                    dep_id = dpt_id,
                    std_cnic = std.std_cnic,
                    session_rollno = rollN0,
                    session_degree = degree,
                    session_startDate = session_start,
                    session_endDate = session_end,
                    session_duration = duration.ToString(),
                    session_activeStatus = "active",
                    session_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now,
                    session_batch = batch
                };
                dc.Sessions.InsertOnSubmit(session);
                dc.SubmitChanges();
                for (int i = 0; i < duration; i++)
                {
                    Due dues = new Due()
                    {
                        dues_type = "annual",
                        dues_amount = null,
                        dues_session_month = (batch + i).ToString(),
                        dues_lastDate = null,
                        dues_paidDate = null,
                        dues_paidStatus = null,
                        dues_recipt_no = null,
                        allottee_id = allottment.allottee_id,
                        dues_addedBy = null,
                        time_of_addition = DateTime.Now,
                    };
                    dc.Dues.InsertOnSubmit(dues);
                    dc.SubmitChanges();
                }
                string edu_deg0 = Request["edu_name0"];
                int marks_obt0 = int.Parse(Request["marks_obt0"]);
                int marks_total0 = int.Parse(Request["total_marks0"]);
                int edu_session0 = int.Parse(Request["session0"]);
                string board0 = Request["board0"];
                Education education0 = new Education
                {
                    std_cnic = std.std_cnic,
                    edu_degree = edu_deg0,
                    edu_marksObt = marks_obt0,
                    edu_totalMarks = marks_total0,
                    edu_board_uni = board0,
                    edu_session = edu_session0,
                    edu_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now
                };
                dc.Educations.InsertOnSubmit(education0);
                dc.SubmitChanges();
                string n = Request["education_no"];
                int key = int.Parse(n);
                for (int i = 1; i <= key; i++)
                {
                    string edu_deg = Request["edu_name" + i];
                    int marks_obt = int.Parse(Request["marks_obt" + i]);
                    int marks_total = int.Parse(Request["total_marks" + i]);
                    int edu_session = int.Parse(Request["session" + i]);
                    string board = Request["board" + i];
                    Education education = new Education
                    {
                        std_cnic = std.std_cnic,
                        edu_degree = edu_deg,
                        edu_marksObt = marks_obt,
                        edu_totalMarks = marks_total,
                        edu_board_uni = board,
                        edu_session = edu_session,
                        edu_addedBy = (int)Session["user_id"],
                        time_of_addition = DateTime.Now
                    };
                    dc.Educations.InsertOnSubmit(education);
                    dc.SubmitChanges();
                }
            }
            return RedirectToAction("add_allotment");
        }
        public ActionResult Assign(int id)
        {
            string passwd = Request["stud_passwd"];
            System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
            byte[] passwordArray = encryptpwd.GetBytes(passwd);
            var d = dc.Users.First(x => x.user_id == id);
            if (d != null)
            {
                d.user_passwd = passwordArray;
                d.user_activeStatus = "active";
                d.hostel_id = null;
                dc.SubmitChanges();
            }
            string std_name = Request["name"];
            decimal std_cnic = decimal.Parse(Request["cnic"]);
            string std_fatherName = Request["fname"];
            string std_fatherOccupation = Request["occupation"];
            decimal std_fatherIncome = decimal.Parse(Request["income"]);
            string std_presentAddress = Request["paddress"];
            string std_permanentAddress = Request["permanentaddress"];
            string district = Request["district"];
            decimal std_phone = decimal.Parse(Request["stu_num"]);
            decimal std_parentPhone = decimal.Parse(Request["grd_num"]);
            string std_bloodGroup = Request["bloodGroup"];
            string std_HBSAg_report = Request["hbsag"];
            string std_antiCV_report = Request["hcv"];
            string std_nationality = Request["nationality"];
            int? uid = d.user_id;
            var v = dc.Students.First(x => x.user_id == uid);
            if (v != null)
            {
                v.std_name = std_name;
                v.std_fatherName = std_fatherName;
                v.std_fatherOccupation = std_fatherOccupation;
                v.std_fatherIncome = std_fatherIncome;
                v.std_presentAddress = std_presentAddress;
                v.std_permanentAddress = std_permanentAddress;
                v.std_phone = std_phone;
                v.std_parentPhone = std_parentPhone;
                v.std_district = district;
                v.std_bloodGroup = std_bloodGroup;
                v.std_HBSAg_report = std_HBSAg_report;
                v.std_antiHCV_report = std_antiCV_report;
                v.std_nationality = std_nationality;
                v.std_activeStatus = "active";
                dc.SubmitChanges();
            }
            int room = int.Parse(Request["room_number"]);
            Allottment allottment = new Allottment
            {
                room_id = room,
                hostel_id = (int)Session["hostel"],
                allotte_type = "regular",
                allotte_activeStatus = "active",
                time_of_addition = DateTime.Now,
                allotte_addedBy = (int)Session["user_id"],
                std_cnic = v.std_cnic
            };
            dc.Allottments.InsertOnSubmit(allottment);
            dc.SubmitChanges();
            int dpt_id = int.Parse(Request["dep_name"]);
            string rollN0 = Request["roll_no"];
            string degree = Request["class"];
            int batch = int.Parse(Request["session"]);
            DateTime session_start = DateTime.Parse(Request["session_start"]);
            DateTime session_end = DateTime.Parse(Request["session_end"]);
            string start = session_start.ToString("yyyy");
            string end = session_end.ToString("yyyy");
            int duration = int.Parse(end) - int.Parse(start);
            Session session = new Session
            {
                dep_id = dpt_id,
                std_cnic = v.std_cnic,
                session_rollno = rollN0,
                session_degree = degree,
                session_startDate = session_start,
                session_endDate = session_end,
                session_duration = duration.ToString(),
                session_activeStatus = "active",
                session_addedBy = (int)Session["user_id"],
                time_of_addition = DateTime.Now,
                session_batch = batch
            };
            dc.Sessions.InsertOnSubmit(session);
            dc.SubmitChanges();
            string edu_deg0 = Request["edu_name0"];
            int marks_obt0 = int.Parse(Request["marks_obt0"]);
            int marks_total0 = int.Parse(Request["total_marks0"]);
            int edu_session0 = int.Parse(Request["session0"]);
            string board0 = Request["board0"];
            Education education0 = new Education
            {
                std_cnic = v.std_cnic,
                edu_degree = edu_deg0,
                edu_marksObt = marks_obt0,
                edu_totalMarks = marks_total0,
                edu_board_uni = board0,
                edu_session = edu_session0,
                edu_addedBy = (int)Session["user_id"],
                time_of_addition = DateTime.Now
            };
            dc.Educations.InsertOnSubmit(education0);
            dc.SubmitChanges();
            string n = Request["education_no"];
            int key = int.Parse(n);
            for (int i = 1; i <= key; i++)
            {
                string edu_deg = Request["edu_name" + i];
                int marks_obt = int.Parse(Request["marks_obt" + i]);
                int marks_total = int.Parse(Request["total_marks" + i]);
                int edu_session = int.Parse(Request["session" + i]);
                string board = Request["board" + i];
                Education education = new Education
                {
                    std_cnic = v.std_cnic,
                    edu_degree = edu_deg,
                    edu_marksObt = marks_obt,
                    edu_totalMarks = marks_total,
                    edu_board_uni = board,
                    edu_session = edu_session,
                    edu_addedBy = (int)Session["user_id"],
                    time_of_addition = DateTime.Now
                };
                dc.Educations.InsertOnSubmit(education);
                dc.SubmitChanges();
                /*TempData["msg"] = "<script>alert('Allottment added');</script>";
                @Html.Raw(TempData["msg"]);*/
            }
                return RedirectToAction("add_allotment");
        }
        public ActionResult Change_password()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_Info(int id)
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.First(x => x.allottee_id == id);
                decimal cnic = (decimal)a.std_cnic;
                model.b = dc.Educations.Where(x => x.std_cnic == cnic).ToList();
                model.c = dc.Sessions.Where(x => x.std_cnic == cnic && x.session_activeStatus == "active");
                var t = dc.Sessions.First(x => x.std_cnic == cnic && x.session_activeStatus == "active");
                var z = dc.Departments.First(b => b.dep_id == t.dep_id);
                ViewBag.dep = z.dep_name;
                model.d = dc.Students.Where(x => x.std_cnic == cnic);
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_MessDues()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "mess");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Manage_AnnualDues()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "annual").ToList();
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        
        public ActionResult View_StudentsMess()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                var a = dc.View_StudentsMesses.ToList();
                return View(a);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult View_AnnualRecords()
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                int id = (int)Session["hostel"];
                dynamic model = new ExpandoObject();
                var a = dc.Allottments.Where(x => x.hostel_id == id && x.allotte_activeStatus == "active");
                foreach (var x in a)
                {
                    var s = dc.Students.First(p => p.std_cnic == x.std_cnic);
                    ViewBag.user = s.std_name;
                    model.b = dc.Dues.Where(d => d.allottee_id == x.allottee_id && d.dues_type == "annual");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateAnnual(int id)
        {
            if (Session["user_role"].ToString() == "superitendant" || Session["user_role"].ToString() == "admin")
            {
                var a = dc.Dues.First(s => s.allottee_id == id);
                if (a != null)
                {
                    dc.SubmitChanges();
                    return RedirectToAction("Manage_AnnualDues");
                }
                else return RedirectToAction("Manage_AnnualDues");
            }
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Update(int id)
        {
            if (Session["user_role"].ToString() == "hostel_clerk" || Session["user_role"].ToString() == "admin")
            {
                string status = Request["status"];
                var a = dc.Allottments.First(s => s.allottee_id == id);
                var t = dc.Students.First(d => d.std_cnic == a.std_cnic);
                var z = dc.Users.First(f => f.user_id == t.user_id);
                if (a != null)
                {
                    a.allotte_activeStatus = "inactive";
                    var b = dc.Sessions.First(x=> x.std_cnic == a.std_cnic);
                    b.session_activeStatus = "inactive";
                    z.user_activeStatus = "inactive";
                    t.std_activeStatus = "inactive";
                    dc.SubmitChanges();
                    return RedirectToAction("View_Allottment");
                }
                else
                    return RedirectToAction("View_Allottment");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}