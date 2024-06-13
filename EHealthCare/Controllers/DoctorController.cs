using EHealthCare.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EHealthCare.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        EHealthCareEntities db = new EHealthCareEntities();
        public ActionResult DoctorDashboard()
        {
            return View();
        }
        public ActionResult PatientGraph()
        {
            string monthName = "";
            AdminViewModel model = new AdminViewModel();
            List<string> months = new List<string>();
            var patientmonth = GetAllPatientByMonthWise();

            var monthsss = patientmonth.Select(a => a.Months).ToList();

            var patientcount = patientmonth.Select(a => a.NumberOfPatients).ToList();

            foreach (var item in monthsss)
            {
                if (item == 1)
                {
                    monthName = "January";
                }
                else if (item == 2)
                {
                    monthName = "February";
                }
                else if (item == 3)
                {
                    monthName = "March";
                }
                else if (item == 4)
                {
                    monthName = "April";
                }
                else if (item == 5)
                {
                    monthName = "May";
                }
                else if (item == 6)
                {
                    monthName = "June";
                }

                else if (item == 7)
                {
                    monthName = "July";
                }
                else if (item == 8)
                {
                    monthName = "August";
                }
                else if (item == 9)
                {
                    monthName = "September";
                }
                else if (item == 10)
                {
                    monthName = "October";
                }
                else if (item == 11)
                {
                    monthName = "November";
                }
                else if (item == 12)
                {
                    monthName = "December";
                }
                months.Add(monthName);
            }
            return Json(new { months, patientcount }, JsonRequestBehavior.AllowGet);
        }


        public List<GetPatientMonthWise_Result> GetAllPatientByMonthWise()
        {
            return db.GetPatientMonthWise().ToList();
        }
        [HttpGet]
        public async Task<ActionResult> ManageProfile()
        {
            AdminViewModel model = new AdminViewModel();
            int userId = Convert.ToInt32(Session["UserId"]);
            GenericCrud<City> cityList = new GenericCrud<City>();
            GenericCrud<User> userdata = new GenericCrud<User>();
            GenericCrud<UserProfile> profiledata = new GenericCrud<UserProfile>();
            model.User = await userdata.GetSingleAsync(userId);
            model.UserProfile = db.UserProfiles.Where(x => x.UserID == userId).FirstOrDefault();
            var data = await cityList.GetAllAsync();
            model.CityList = data.ToList();
            model.ConfirmPassword = model.User.Password;
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        [HttpPost]
        public ActionResult ManageDoctorProfile(AdminViewModel model)
        {
            GenericCrud<User> getuser = new GenericCrud<User>();
            GenericCrud<UserProfile> getuserProfile = new GenericCrud<UserProfile>();
            int userId = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            UserProfile userProfile = new UserProfile();
            Patient patientProfile = new Patient();

            string resoucesDir = Server.MapPath("/Uploads/");
            CreateDir(resoucesDir);
            string productDir = Server.MapPath("/Uploads/Images/");
            CreateDir(productDir);
            var filePath = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                string fileName = "";
                string path = Server.MapPath("~/Uploads/Images");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var extension = Path.GetExtension(file.FileName);
                    fileName = Path.GetFileName(file.FileName);
                    fileName = DateTime.UtcNow.ToString("mmddyyyyhhmmss") + fileName;
                    fileName = fileName.Replace(" ", "");
                    var patha = Path.Combine(path, fileName);
                    file.SaveAs(patha);

                    filePath = "/Uploads/Images" + fileName;
                    Session.Add("UserPic", filePath);
                }
            }
            model.UserProfile.ProfilePic = filePath;
            Session.Add("UserPic", filePath);

            User updateUser = db.Users.Where(x => x.UserID == userId).FirstOrDefault();
            updateUser.UserName = model.User.UserName;
            updateUser.Password = model.User.Password;
            updateUser.UserType = model.User.UserType;
            db.SaveChanges();

            UserProfile updateProfile = db.UserProfiles.Where(x => x.UserID == userId).FirstOrDefault();
            updateProfile.UserID = userId;
            updateProfile.FirstName = model.UserProfile.FirstName;
            updateProfile.LastName = model.UserProfile.LastName;
            updateProfile.Phone = model.UserProfile.Phone;
            updateProfile.City = model.UserProfile.City;
            updateProfile.Gender = model.UserProfile.Gender;
            updateProfile.DOB = model.UserProfile.DOB;
            updateProfile.Email = model.UserProfile.Email;
            updateProfile.UserName = model.User.UserName;
            updateProfile.Password = model.User.Password;
            updateProfile.ProfilePic = filePath;

            Doctor doctor = db.Doctors.Where(x => x.UserID == userId).FirstOrDefault();
            doctor.User.UserName = model.User.UserName;
            doctor.User.Password = model.User.Password;
            doctor.FirstName = model.UserProfile.FirstName;
            doctor.LastName = model.UserProfile.LastName;
            doctor.DOB = model.UserProfile.DOB;
            doctor.Gender = model.UserProfile.Gender;
            doctor.Phone = model.UserProfile.Phone;
            doctor.Email = model.UserProfile.Email;
            doctor.City = model.UserProfile.City;
            doctor.Image = filePath;
            doctor.UserID = userId;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Appointments", "Patient");
        }
        public void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}