using EHealthCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;

namespace EHealthCare.Controllers
{
    public class AdminDashboardController : Controller
    {
        private EHealthCareEntities db = new EHealthCareEntities();
        // GET: Dashboard
        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> Dashboard()
        {
            AdminViewModel model = new AdminViewModel();
            GenericCrud<Doctor> Doclist = new GenericCrud<Doctor>();
            GenericCrud<Patient> Ptlist = new GenericCrud<Patient>();
            GenericCrud<Appointment> app = new GenericCrud<Appointment>();
            GenericCrud<Staff> stf = new GenericCrud<Staff>();
            GenericCrud<RoomAllotment> altr = new GenericCrud<RoomAllotment>();
            GenericCrud<AppointmentPayment> payment = new GenericCrud<AppointmentPayment>();
            GenericCrud<Prescription> pre = new GenericCrud<Prescription>();
            GenericCrud<User> user = new GenericCrud<User>();

            var doctor = await Doclist.GetAllAsync();
            var patient = await Ptlist.GetAllAsync();
            var staff = await stf.GetAllAsync();
            var room = await altr.GetAllAsync();
            var appoint = await app.GetAllAsync();
            var totalpayment = await payment.GetAllAsync();
            var totaluser = await user.GetAllAsync();
            var patientPre = await pre.GetAllAsync();

            model.DoctorCount = doctor.Count();
            model.PatientCount = patient.Count();
            model.StaffCount = staff.Count();
            model.TotalRoomCount = room.Count();
            model.AppointmentCount = appoint.Count();
            model.EarningCount = totalpayment.Count();
            model.UserCount = totaluser.Count();
            model.PrescriptionCount = patientPre.Count();


            model.AppointmentsList = new List<Appointment>();
            model.AppointmentsList = db.Appointments.ToList();

            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> BookAppointment(AdminViewModel model)
        {
            GenericCrud<Doctor> Doclist = new GenericCrud<Doctor>();
            GenericCrud<Patient> Ptlist = new GenericCrud<Patient>();
            var data = await Doclist.GetAllAsync();
            model.DoctorList = data.ToList();
            var data2 = await Ptlist.GetAllAsync();
            model.PatientList = data2.ToList();
            return View(model);
        }
        public async Task<ActionResult> BookPatientAppointment(AdminViewModel model)
        {
            Appointment appointment = new Appointment();
            if (model.Appointment.AppointmentID == 0)
            {
                GenericCrud<Appointment> app = new GenericCrud<Appointment>();

                appointment.PatientID = model.Appointment.PatientID;
                appointment.DoctorID = model.Appointment.DoctorID;
                appointment.AppointmentDate = Convert.ToDateTime(model.AppointmentDate);
                appointment.ToTime = Convert.ToDateTime(model.ToTime);
                appointment.FromTime = Convert.ToDateTime(model.FromTime);
                var addnew = await app.CreateAsync(appointment);
            }
            else
            {
                GenericCrud<Appointment> updapp = new GenericCrud<Appointment>();
                int Rid = model.Appointment.AppointmentID;
                appointment = model.Appointment;
                appointment.ToTime = Convert.ToDateTime(model.ToTime);
                appointment.FromTime = Convert.ToDateTime(model.FromTime);
                appointment.AppointmentDate = Convert.ToDateTime(model.AppointmentDate);
                var updateStaff = await updapp.UpdateEntityAsync(appointment, Rid);
            }

            return RedirectToAction("ViewAllAppointments", "AdminDashboard");

        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> ViewAllAppointments(AdminViewModel model)
        {
            GenericCrud<Appointment> Appointmentlist = new GenericCrud<Appointment>();
            GenericCrud<Doctor> Doclist = new GenericCrud<Doctor>();
            GenericCrud<Patient> Ptlist = new GenericCrud<Patient>();
            var data = await Doclist.GetAllAsync();
            model.DoctorList = data.ToList();
            var data2 = await Ptlist.GetAllAsync();
            model.PatientList = data2.ToList();
            model.AppointmentsList = new List<Appointment>();
            model.AppointmentsList = db.Appointments.ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditAppointmentById(int AppointId)
        {
            GenericCrud<Appointment> updateapp = new GenericCrud<Appointment>();
            Appointment app = new Appointment();
            try
            {
                app = await updateapp.GetSingleAsync(AppointId);
                return Json(app, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult> DeleteAppointmentById(int AppointId)
        {
            GenericCrud<Appointment> deleteAppointment = new GenericCrud<Appointment>();
            string msg = "";
            if (AppointId != 0)
            {
                var data = await deleteAppointment.DeleteAsync(AppointId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [EhealthcareAutorizedAttributes]
        public ActionResult ManageAppointment()
        {
            return View();
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> ViewAllDoctors(AdminViewModel model)
        {
            GenericCrud<City> CityList = new GenericCrud<City>();
            GenericCrud<Doctor> Doclist = new GenericCrud<Doctor>();
            var data = await Doclist.GetAllAsync();
            model.DoctorList = data.ToList();
            var data1 = await CityList.GetAllAsync();
            model.CityList = data1.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AddDoctor(AdminViewModel model)
        {
            GenericCrud<City> CityList = new GenericCrud<City>();
            var data = await CityList.GetAllAsync();
            model.CityList = data.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AddorEditDoctor(AdminViewModel model)
        {
            Doctor doc = new Doctor();
            User user = new User();
            UserProfile UserProfile = new UserProfile();
            int userId = Convert.ToInt32(Session["UserId"]);
            if (model.Doctor.DoctorID == 0)
            {
                GenericCrud<Doctor> addDoc = new GenericCrud<Doctor>();
                GenericCrud<User> addUser = new GenericCrud<User>();

                GenericCrud<UserProfile> addUserPro = new GenericCrud<UserProfile>();

                string resoucesDir = Server.MapPath("/Uploads/");
                CreateDir(resoucesDir);
                string productDir = Server.MapPath("/Uploads/Images/");
                CreateDir(productDir);
                var filePath = "";
                if (Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = Request.Files;
                    if (httpPostedFile != null)
                    {
                        for (int i = 0; i < httpPostedFile.Count; i++)
                        {
                            HttpPostedFileBase postedFile = httpPostedFile[i];
                            string extension = System.IO.Path.GetExtension(postedFile.FileName);
                            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                            var fullFileName = fileNameWithoutExtension + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            var contentType = postedFile.ContentType;
                            var contentLength = postedFile.ContentLength;
                            var fileSavePath = Path.Combine(Server.MapPath("/Uploads/Images/"), fullFileName);
                            postedFile.SaveAs(fileSavePath);

                            filePath += "/Uploads/Images/" + fullFileName;
                            Session.Add("UserPic", filePath);
                        }
                    }
                }

                user.UserName = model.User.UserName;
                user.Password = model.User.Password;
                user.UserType = model.User.UserType;
                var addnewUser = await addUser.CreateAsync(user);
                doc.Image = filePath;
                doc.FirstName = model.Doctor.FirstName;
                doc.LastName = model.Doctor.LastName;
                doc.Gender = model.Doctor.Gender;
                doc.DOB = model.Doctor.DOB;
                doc.Email = model.Doctor.Email;
                doc.JoiningDate = model.Doctor.JoiningDate;
                doc.Phone = model.Doctor.Phone;
                doc.Address = model.Doctor.Address;
                doc.UserID = addnewUser.UserID;
                var addnew = await addDoc.CreateAsync(doc);


                UserProfile.FirstName = model.Doctor.FirstName;
                UserProfile.LastName = model.Doctor.LastName;
                UserProfile.Gender = model.Doctor.Gender;
                UserProfile.DOB = model.Doctor.DOB;
                UserProfile.Email = model.Doctor.Email;
                UserProfile.Phone = model.Doctor.Phone;
                UserProfile.City = model.Doctor.City;
                UserProfile.UserID = addnewUser.UserID;
                UserProfile.UserName = model.User.UserName;
                UserProfile.Password = model.User.Password;

                var addnewPro = await addUserPro.CreateAsync(UserProfile);
                return RedirectToAction("ViewAllDoctors");
            }
            else
            {
                GenericCrud<Doctor> updDoc = new GenericCrud<Doctor>();
                GenericCrud<User> updUser = new GenericCrud<User>();
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = Server.MapPath("~/Uploads/Images/");

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

                        model.Doctor.Image = "/Uploads/Images/" + fileName;
                        Session.Add("UserPic", model.Doctor.Image);
                    }
                }
                int Did = model.Doctor.DoctorID;
                doc.Image = model.Doctor.Image;
                doc = model.Doctor;
                var updateDoc = await updDoc.UpdateEntityAsync(doc, Did);

                //User updateUser = db.Users.Where(x => x.UserID == userId).FirstOrDefault();
                //updateUser.UserName = model.User.UserName;
                //updateUser.Password = model.User.Password;
                //updateUser.UserType = model.User.UserType;
                //db.SaveChanges();

            }
            return RedirectToAction("ViewAllDoctors");
        }


        [HttpPost]
        public async Task<ActionResult> DeleteDoctorById(int DocId)
        {
            GenericCrud<Doctor> deletedoc = new GenericCrud<Doctor>();
            string msg = "";
            if (DocId != 0)
            {
                var data = await deletedoc.DeleteAsync(DocId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> EditDoctorById(int DoctorId)
        {
            GenericCrud<Doctor> updatedoc = new GenericCrud<Doctor>();
            Doctor doc = new Doctor();
            try
            {
                doc = await updatedoc.GetSingleAsync(DoctorId);
                return Json(doc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> ViewAllPatients(AdminViewModel model)
        {
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            List<Patient> listpatient = new List<Patient>();
            GenericCrud<Doctor> doclist = new GenericCrud<Doctor>();
            GenericCrud<City> cityList = new GenericCrud<City>();
            var data = await doclist.GetAllAsync();
            var data2 = await cityList.GetAllAsync();
            var data3 = await Patientlist.GetAllAsync();
            model.CityList = data2.ToList();
            model.DoctorList = data.ToList();
            model.PatientList = data3.ToList();
            return View(model);

        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AddPatient(AdminViewModel model)
        {
            GenericCrud<Doctor> doclist = new GenericCrud<Doctor>();
            GenericCrud<City> cityList = new GenericCrud<City>();
            var data = await doclist.GetAllAsync();
            var data2 = await cityList.GetAllAsync();
            model.CityList = data2.ToList();
            model.DoctorList = data.ToList();
            return View(model);
        }


        [EhealthcareAutorizedAttributes]
        [HttpPost]
        public async Task<ActionResult> AddorEditPatient(AdminViewModel model)
        {
            Patient pt = new Patient();
            User user = new User();
            if (model.Patient.PatientID == 0)
            {
                GenericCrud<Patient> addPatient = new GenericCrud<Patient>();
                GenericCrud<User> addUser = new GenericCrud<User>();
                string resoucesDir = Server.MapPath("/Uploads/");
                CreateDir(resoucesDir);
                string productDir = Server.MapPath("/Uploads/Images/");
                CreateDir(productDir);
                var filePath = "";
                if (Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = Request.Files;
                    if (httpPostedFile != null)
                    {
                        for (int i = 0; i < httpPostedFile.Count; i++)
                        {
                            HttpPostedFileBase postedFile = httpPostedFile[i];
                            string extension = System.IO.Path.GetExtension(postedFile.FileName);
                            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                            var fullFileName = fileNameWithoutExtension + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            var contentType = postedFile.ContentType;
                            var contentLength = postedFile.ContentLength;
                            var fileSavePath = Path.Combine(Server.MapPath("/Uploads/Images/"), fullFileName);
                            postedFile.SaveAs(fileSavePath);

                            filePath += "/Uploads/Images/" + fullFileName;
                            Session.Add("UserPic", filePath);
                        }
                    }
                }

                user.UserName = model.User.UserName;
                user.Password = model.User.Password;
                user.UserType = model.User.UserType;
                var addnewUser = await addUser.CreateAsync(user);

                pt.UserID = addnewUser.UserID;
                pt.CreatedDate = DateTime.Now;
                pt.FirstName = model.Patient.FirstName;
                pt.LastName = model.Patient.LastName;
                pt.Image = filePath;
                pt.Gender = model.Patient.Gender;
                pt.DOB = model.Patient.DOB;
                pt.Email = model.Patient.Email;
                pt.Phone = model.Patient.Phone;
                pt.City = model.Patient.City;
                pt.Age = model.Patient.Age;
                pt.Address = model.Patient.Address;
                pt.MariedStatus = model.Patient.MariedStatus;
                pt.BloodGroup = model.Patient.BloodGroup;
                pt.BloodPressureStatus = model.Patient.BloodPressureStatus;
                pt.JoinDate = model.Patient.JoinDate;
                pt.Condtion = model.Patient.Condtion;
                pt.ConsultingRoom = model.Patient.ConsultingRoom;
                pt.AssignedDoc = model.Patient.AssignedDoc;
                pt.Status = model.Patient.Status;
                var addnew = await addPatient.CreateAsync(pt);
            }
            else
            {

                GenericCrud<Patient> updPatient = new GenericCrud<Patient>();
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = Server.MapPath("~/Resources/Users/");

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

                        model.Patient.Image = "/Resources/Users/" + fileName;
                        Session.Add("UserPic", model.Patient.Image);
                    }
                }

                int Pid = model.Patient.PatientID;
                pt.Image = model.Patient.Image;
                pt = model.Patient;
                var updatePatient = await updPatient.UpdateEntityAsync(pt, Pid);
            }
            return RedirectToAction("ViewAllPatients");
        }
        [HttpPost]
        public async Task<ActionResult> DeletePatientById(int PId)
        {
            GenericCrud<Patient> deletePatient = new GenericCrud<Patient>();
            string msg = "";
            if (PId != 0)
            {
                var data = await deletePatient.DeleteAsync(PId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> EditPatientById(int PatientId)
        {
            GenericCrud<Patient> updatePat = new GenericCrud<Patient>();
            Patient pt = new Patient();
            try
            {
                pt = await updatePat.GetSingleAsync(PatientId);
                return Json(pt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AllotedRoom()
        {
            GenericCrud<RoomAllotment> allotlist = new GenericCrud<RoomAllotment>();
            List<RoomAllotment> listroom = new List<RoomAllotment>();
            listroom = await allotlist.GetAllAsync();
            return View(listroom);
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> NewRoomAllotment(AdminViewModel model)
        {
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        [HttpPost]
        public async Task<ActionResult> AddorEditRooms(AdminViewModel model)
        {
            RoomAllotment alt = new RoomAllotment();
            if (model.RoomAllotment.RoomAllotmentID == 0)
            {
                GenericCrud<RoomAllotment> addroom = new GenericCrud<RoomAllotment>();

                alt.RoomID = model.RoomAllotment.RoomID;
                alt.PatientID = model.RoomAllotment.PatientID;
                alt.Duration = model.RoomAllotment.Duration;
                alt.Dated = model.RoomAllotment.Dated;
                alt.DischargeDate = model.RoomAllotment.DischargeDate;
                var addnew = await addroom.CreateAsync(alt);
            }
            else
            {
                GenericCrud<RoomAllotment> updstf = new GenericCrud<RoomAllotment>();
                int Rid = model.RoomAllotment.RoomAllotmentID;
                alt = model.RoomAllotment;
                var updateStaff = await updstf.UpdateEntityAsync(alt, Rid);
            }
            return RedirectToAction("AllotedRoom");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteRoomById(int roomId)
        {
            GenericCrud<RoomAllotment> deleteroom = new GenericCrud<RoomAllotment>();
            string msg = "";
            if (roomId != 0)
            {
                var data = await deleteroom.DeleteAsync(roomId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> EditRoomById(int roomId)
        {
            GenericCrud<RoomAllotment> updateroom = new GenericCrud<RoomAllotment>();
            RoomAllotment ra = new RoomAllotment();
            try
            {
                ra = await updateroom.GetSingleAsync(roomId);
                return Json(ra, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult Payment()
        {
            AdminViewModel model = new AdminViewModel();
            model.PaymentList = new List<Payment>();
            model.PaymentList = db.Payments.ToList();
            return View(model);
        }
        public async Task<ActionResult> AddPayment()
        {
            AdminViewModel model = new AdminViewModel();
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            return View(model);
        }
        public async Task<ActionResult> AddPatientPayment(AdminViewModel model)
        {
            Payment pt = new Payment();
            if (model.Payment.PaymentId == 0)
            {
                GenericCrud<Payment> addpt = new GenericCrud<Payment>();

                pt.PatientID = model.Payment.PatientID;
                pt.PaymentStatus = model.Payment.PaymentStatus;
                pt.TotalPayment = model.Payment.TotalPayment;
                pt.PaymentDate = model.Payment.PaymentDate;
                pt.PaymentType = model.Payment.PaymentType;
                var addnew = await addpt.CreateAsync(pt);
            }
            return RedirectToAction("Payment");
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AllStaff(AdminViewModel model)
        {
            GenericCrud<Staff> Stafflist = new GenericCrud<Staff>();
            GenericCrud<City> cityList = new GenericCrud<City>();
            var data = await Stafflist.GetAllAsync();
            model.StaffList = data.ToList();
            var data2 = await cityList.GetAllAsync();
            model.CityList = data2.ToList();

            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        [HttpGet]
        public async Task<ActionResult> AddStaff(AdminViewModel model)
        {
            GenericCrud<City> cityList = new GenericCrud<City>();
            var data2 = await cityList.GetAllAsync();
            model.CityList = data2.ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> AddorEditStaff(AdminViewModel model)
        {
            Staff stf = new Staff();
            if (model.Staff.StaffId == 0)
            {
                GenericCrud<Staff> addstf = new GenericCrud<Staff>();

                string resoucesDir = Server.MapPath("/Uploads/");
                CreateDir(resoucesDir);
                string productDir = Server.MapPath("/Uploads/Images/");
                CreateDir(productDir);
                var filePath = "";
                if (Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = Request.Files;
                    if (httpPostedFile != null)
                    {
                        for (int i = 0; i < httpPostedFile.Count; i++)
                        {
                            HttpPostedFileBase postedFile = httpPostedFile[i];
                            string extension = System.IO.Path.GetExtension(postedFile.FileName);
                            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                            var fullFileName = fileNameWithoutExtension + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            var contentType = postedFile.ContentType;
                            var contentLength = postedFile.ContentLength;
                            var fileSavePath = Path.Combine(Server.MapPath("/Uploads/Images/"), fullFileName);
                            postedFile.SaveAs(fileSavePath);

                            filePath += "/Uploads/Images/" + fullFileName;
                            Session.Add("UserPic", filePath);
                        }
                    }
                }
                stf.Image = filePath;
                stf.Title = model.Staff.Title;
                stf.FirstName = model.Staff.FirstName;
                stf.LastName = model.Staff.LastName;
                stf.Designation = model.Staff.Designation;
                stf.Gender = model.Staff.Gender;
                stf.DOB = model.Staff.DOB;
                stf.Email = model.Staff.Email;
                stf.Phone = model.Staff.Phone;
                stf.Address = model.Staff.Address;
                var addnew = await addstf.CreateAsync(stf);
            }

            else
            {
                GenericCrud<Staff> updstf = new GenericCrud<Staff>();
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = Server.MapPath("~/Upload/Images/");

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

                        model.Staff.Image = "~/Upload/Images/" + fileName;
                        Session.Add("UserPic", model.Staff.Image);
                    }
                }
                int Sid = model.Staff.StaffId;
                stf.Image = model.Staff.Image;
                stf = model.Staff;
                var updateStaff = await updstf.UpdateEntityAsync(stf, Sid);
            }
            return RedirectToAction("AllStaff");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStaffById(int StaffId)
        {
            GenericCrud<Staff> deletestf = new GenericCrud<Staff>();
            string msg = "";
            if (StaffId != 0)
            {
                var data = await deletestf.DeleteAsync(StaffId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> EditStaffById(int StaffId)
        {
            GenericCrud<Staff> updatestf = new GenericCrud<Staff>();
            Staff staff = new Staff();
            try
            {
                staff = await updatestf.GetSingleAsync(StaffId);
                return Json(staff, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> Prescriptions()
        {
            AdminViewModel model = new AdminViewModel();
            model.PrescriptionList = new List<Prescription>();
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            GenericCrud<Medicine> Medilist = new GenericCrud<Medicine>();
            GenericCrud<Prescription> Prelist = new GenericCrud<Prescription>();
            int userId = Convert.ToInt32(Session["PatientId"]);
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            var data3 = await Medilist.GetAllAsync();
            model.MedicineList = data3.ToList();
            model.PrescriptionList = db.Prescriptions.ToList();

            return View(model);
        }

        public async Task<ActionResult> AddorEditPrescription(AdminViewModel model)
        {
            Prescription pre = new Prescription();
            if (model.Prescription.PatientPrescriptionID == 0)
            {
                GenericCrud<Prescription> addPre = new GenericCrud<Prescription>();
                pre.MedicineID = model.Prescription.MedicineID;
                pre.PatientID = model.Prescription.PatientID;
                pre.Medicine = model.Prescription.Medicine;
                pre.Patient = model.Prescription.Patient;
                pre.Dose = model.Prescription.Dose;
                pre.Unit = model.Prescription.Unit;
                pre.StartDate = model.Prescription.StartDate;
                pre.EndDate = model.Prescription.EndDate;
                pre.Reason = model.Prescription.Reason;
                pre.Comments = model.Prescription.Comments;
                var addnew = await addPre.CreateAsync(pre);
            }
            else
            {
                GenericCrud<Prescription> updPre = new GenericCrud<Prescription>();

                int Preid = model.Prescription.PatientPrescriptionID;

                pre = model.Prescription;
                var updateStaff = await updPre.UpdateEntityAsync(pre, Preid);
            }
            return RedirectToAction("Prescriptions");
        }

        [HttpPost]
        public async Task<ActionResult> EditPrescriptionById(int PreId)
        {
            GenericCrud<Prescription> updatePre = new GenericCrud<Prescription>();
            Prescription pre = new Prescription();
            try
            {
                pre = await updatePre.GetSingleAsync(PreId);
                return Json(pre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AddPrescription(AdminViewModel model)
        {
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            GenericCrud<Medicine> Medilist = new GenericCrud<Medicine>();
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            var data3 = await Medilist.GetAllAsync();
            model.MedicineList = data3.ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> DeletePrescriptionById(int PreId)
        {
            GenericCrud<Prescription> deletePre = new GenericCrud<Prescription>();
            string msg = "";
            if (PreId != 0)
            {
                var data = await deletePre.DeleteAsync(PreId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> PatientTests(AdminViewModel model)
        {
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            GenericCrud<LabTest> testlist = new GenericCrud<LabTest>();
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            var data3 = await testlist.GetAllAsync();
            model.TestList = data3.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        public async Task<ActionResult> AddLabTest(AdminViewModel model)
        {
            GenericCrud<Patient> Patientlist = new GenericCrud<Patient>();
            var data = await Patientlist.GetAllAsync();
            model.PatientList = data.ToList();
            return View(model);
        }

        [EhealthcareAutorizedAttributes]
        [HttpPost]
        public async Task<ActionResult> AddorEditLabTest(AdminViewModel model)
        {
            LabTest alt = new LabTest();
            if (model.LabTest.LabTestID == 0)
            {
                GenericCrud<LabTest> addTest = new GenericCrud<LabTest>();
                string resoucesDir = Server.MapPath("/Uploads/");
                CreateDir(resoucesDir);
                string productDir = Server.MapPath("/Uploads/Files/");
                CreateDir(productDir);
                var filePath = "";
                if (Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = Request.Files;
                    if (httpPostedFile != null)
                    {
                        for (int i = 0; i < httpPostedFile.Count; i++)
                        {
                            HttpPostedFileBase postedFile = httpPostedFile[i];
                            string extension = System.IO.Path.GetExtension(postedFile.FileName);
                            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                            var fullFileName = fileNameWithoutExtension + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            var contentType = postedFile.ContentType;
                            var contentLength = postedFile.ContentLength;
                            var fileSavePath = Path.Combine(Server.MapPath("/Uploads/Files/"), fullFileName);
                            postedFile.SaveAs(fileSavePath);

                            filePath += "/Uploads/Files/" + fullFileName;
                        }
                    }
                }
                alt.PatientID = model.LabTest.PatientID;
                alt.TestName = model.LabTest.TestName;
                alt.TestDate = model.LabTest.TestDate;
                alt.Status = model.LabTest.Status;
                alt.Remarks = model.LabTest.Remarks;
                alt.Report = filePath;
                var addnew = await addTest.CreateAsync(alt);
            }
            else
            {
                GenericCrud<LabTest> updtest = new GenericCrud<LabTest>();
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = Server.MapPath("~/Upload/Files/");

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

                        model.LabTest.Report = "~/Upload/Files/" + fileName;
                    }
                }
                int Rid = model.LabTest.LabTestID;
                alt = model.LabTest;
                alt.Report = model.LabTest.Report;
                var updatetest = await updtest.UpdateEntityAsync(alt, Rid);
            }
            return RedirectToAction("PatientTests");
        }
        [HttpPost]
        public async Task<ActionResult> DeletePatientTestById(int testId)
        {
            GenericCrud<LabTest> deletetest = new GenericCrud<LabTest>();
            string msg = "";
            if (testId != 0)
            {
                var data = await deletetest.DeleteAsync(testId);
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> EditTestById(int TId)
        {
            GenericCrud<LabTest> updatePre = new GenericCrud<LabTest>();
            LabTest pre = new LabTest();
            try
            {
                pre = await updatePre.GetSingleAsync(TId);
                return Json(pre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

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