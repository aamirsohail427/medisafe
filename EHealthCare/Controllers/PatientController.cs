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
    public class PatientController : Controller
    {
        //chat ka sara view thk kr lena jaha se start hoti vgara vgara. or jaha me koi b doctor patient staff add ho ra h. vaha pehle entry user k table me ho gi phr us k apny table me. nai to chat nai chly gi.. or userid b laazmi dalni h un k tables me.

        private EHealthCareEntities db = new EHealthCareEntities();
        // GET: Patient
        [HttpGet]
        public async Task<ActionResult> ManageProfile()
        {
            PatientViewModel model = new PatientViewModel();
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
        public ActionResult ManageUserProfile(PatientViewModel model)
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
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            UserProfile updateProfile = db.UserProfiles.Where(x => x.UserID == userId).FirstOrDefault();
            updateProfile.FirstName = model.UserProfile.FirstName;
            updateProfile.LastName = model.UserProfile.LastName;
            updateProfile.Phone = model.UserProfile.Phone;
            updateProfile.City = model.UserProfile.City;
            updateProfile.Gender = model.UserProfile.Gender;
            updateProfile.DOB = model.UserProfile.DOB;
            updateProfile.UserName = model.User.UserName;
            updateProfile.Password = model.User.Password;
            updateProfile.ProfilePic = filePath;

            Patient patient = db.Patients.Where(x => x.UserID == userId).FirstOrDefault();
            patient.UserName = model.User.UserName;
            patient.Password = model.User.Password;
            patient.FirstName = model.UserProfile.FirstName;
            patient.LastName = model.UserProfile.LastName;
            patient.DOB = model.UserProfile.DOB;
            patient.Gender = model.UserProfile.Gender;
            patient.Phone = model.UserProfile.Phone;
            patient.Email = model.UserProfile.Email;
            patient.City = model.UserProfile.City;
            patient.Image = filePath;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Appointments", "Patient");
        }

        [EhealthcareAutorizedAttributes]
        public ActionResult Appointments(PatientViewModel model)
        {
            int userId = Convert.ToInt32(Session["PatientId"]);
            var data = db.Appointments.Where(x => x.PatientID == userId);
            model.AppointmentList = new List<Appointment>();
            model.AppointmentList = data.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        public ActionResult RequestAppointment()
        {
            var stripePublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            ViewBag.StripePublishKey = stripePublishKey;
            AppointmentViewModel model = new AppointmentViewModel();
            model.DoctorsList = new List<Doctor>();
            
            model.DoctorsList = db.Doctors.ToList();

            return View(model);
        }

        public async Task<ActionResult> BookAppointment(AppointmentViewModel modle,string stripeEmail, string stripeToken)
        {
            //bookappointment ka form sara thk kr lena abi mene time me static values dali h.tum time valy textbox lga k form se values lana.pehle appointment k table me entry ho gi phr payment k.payment ka option save py click krny se aye ga.


            PaymentController pay = new PaymentController();
            GenericCrud<Appointment> app = new GenericCrud<Appointment>();
            Appointment AppModle = new Appointment();
            modle.Appointment.FromTime = DateTime.Now;
            modle.Appointment.ToTime = DateTime.Now;
            modle.Appointment.UsedID = Convert.ToInt32(Session["UserId"]);


            modle.Appointment.PatientID = Convert.ToInt32(Session["UserId"]);
            AppModle = modle.Appointment;
           var newappointment= await app.CreateAsync(AppModle);
            
          await  pay.Charge(stripeEmail, stripeToken,newappointment.AppointmentID);

            //jaha mrzi redirect kr lena issy
            return RedirectToAction("Appointments","Patient");
        }

        [EhealthcareAutorizedAttributes]
        public ActionResult Prescriptions(PatientViewModel model)
        {
            int userId = Convert.ToInt32(Session["PatientId"]);
            var data = db.Prescriptions.Where(x => x.PatientID == userId).ToList();
            model.PrescriptionList = new List<Prescription>();
            model.PrescriptionList = data.ToList();
            return View(model);
        }
        [EhealthcareAutorizedAttributes]

        public ActionResult LaboratoryTest(PatientViewModel model)
        {
            int userId = Convert.ToInt32(Session["PatientId"]);
            var data = db.LabTests.Where(x => x.PatientID == userId);
            model.ParientLabTestList = new List<LabTest>();
            model.ParientLabTestList = data.ToList();
            return View(model);
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