using EHealthCare.Models;
using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EHealthCare.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        EHealthCareEntities db = new EHealthCareEntities();

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["UserType"] = null;
            Session["UserPic"] = null;
            Session["PatientId"] = null;
            Session["PatientName"] = null;
            return RedirectToAction("Index", "Main");
        }
        public ActionResult Login()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["UserType"] = null;
            Session["UserPic"] = null;
            Session["PatientId"] = null;
            Session["PatientName"] = null;
            Session["profileId"] = null;

            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("CustomError", TempData["CustomError"].ToString());
            }
            return View();
        }
        public ActionResult UserLogin(AdminViewModel model)
        {

            var user = db.Users.FirstOrDefault(a => a.UserName.ToLower() == model.User.UserName.ToLower() && a.Password.ToLower() == model.UserPassword.ToLower());
            if (user != null)
            {
                UserProfile userProfile = new UserProfile();
                string token = "";
                DateTime tokenExpiry = DateTime.UtcNow;
                double inOneDay = (DateTime.UtcNow.Add(TimeSpan.FromDays(1)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                OpenTok Open = new OpenTok(CommonHelper.OpenTokApiKey, CommonHelper.secretKey);
                token = Open.GenerateToken(CommonHelper.OpenTokSessionId, role: Role.PUBLISHER, expireTime: inOneDay, data: "0");
                Session["token"] = token;
                Session.Add("UserId", user.UserID);

                if (user.UserType == 1)
                {
                    Session.Add("UserId", user.UserID);
                    Session.Add("UserName", user.UserName);
                    Session.Add("UserType", user.UserType);
                    if (userProfile.ProfilePic != null)
                    {
                        Session.Add("UserPic", userProfile.ProfilePic);
                    }
                    else
                    {
                        Session.Add("UserPic", "../WebAssets/img/avtar.png");
                    }
                    return Redirect("/AdminDashboard/Dashboard");
                }
                else if (user.UserType == 2)
                {

                    Session.Add("UserId", user.UserID);
                    Session.Add("UserName", user.UserName);
                    Session.Add("UserType", user.UserType);
                    Session.Add("UserName", user.UserName);
                    Session.Add("NewPatientId", user.UserID);
                    int id = Convert.ToInt32(Session["NewPatientId"]);

                    var pId = db.Patients.FirstOrDefault(x => x.UserID == id);
                    if (pId != null)
                    {
                        Session.Add("PatientId", pId.PatientID);
                    }
                    if (userProfile.ProfilePic != null)
                    {
                        Session.Add("UserProfile", userProfile.ProfilePic);
                    }
                    else
                    {
                        Session.Add("UserProfile", "../WebAssets/img/avtar.png");
                    }
                    return Redirect("/Patient/ManageProfile");

                }
                else if (user.UserType == 3)
                {
                    Session.Add("UserId", user.UserID);
                    Session.Add("UserName", user.UserName);
                    Session.Add("UserType", user.UserType);
                    if (userProfile.ProfilePic != null)
                    {
                        Session.Add("UserPic", userProfile.ProfilePic);
                    }
                    else
                    {
                        Session.Add("UserPic", "../WebAssets/img/avtar.png");
                    }
                    return Redirect("/AdminDashboard/Dashboard");
                }
                else
                {
                    Session.Add("UserId", user.UserID);
                    Session.Add("UserName", user.UserName);
                    Session.Add("UserType", user.UserType);
                    if (userProfile.ProfilePic != null)
                    {
                        Session.Add("UserPic", userProfile.ProfilePic);
                    }
                    else
                    {
                        Session.Add("UserPic", "../WebAssets/img/avtar.png");
                    }
                }

            }

            ModelState.AddModelError("CustomError", "Invalid Username or Password");
            return View("Login");

        }

        public ActionResult Registration()
        {
            if (TempData["exist"] != null)
            {
                ViewBag.message = "Username already exists.";

            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UserRegistration(PatientViewModel model)
        {
            User user = new User();
            UserProfile userProfile = new UserProfile();
            Patient patient = new Patient();
            if (model.User.UserID == 0 && model.UserProfile.UserProfileId == 0)
            {
                int? count = ExistUser(model.User.UserName);
                if (count == 0)
                {
                    GenericCrud<User> addUser = new GenericCrud<User>();
                    GenericCrud<UserProfile> addUserProfile = new GenericCrud<UserProfile>();
                    GenericCrud<Patient> addPatient = new GenericCrud<Patient>();
                    user.Password = model.User.Password;
                    user.UserType = model.User.UserType;
                    user.UserName = model.User.UserName;
                    var userdata = await addUser.CreateAsync(user);

                    userProfile.UserID = userdata.UserID;
                    userProfile.FirstName = model.UserProfile.FirstName;
                    userProfile.LastName = model.UserProfile.LastName;
                    userProfile.Email = model.UserProfile.Email;
                    var data1 = await addUserProfile.CreateAsync(userProfile);

                    patient.UserID = userdata.UserID;
                    patient.FirstName = model.UserProfile.FirstName;
                    patient.LastName = model.UserProfile.LastName;
                    patient.Email = model.UserProfile.Email;
                    patient.UserName = model.User.UserName;
                    patient.Password = model.User.Password;
                    patient.CreatedDate = DateTime.Now;
                    var data2 = await addPatient.CreateAsync(patient);
                }
                else
                {
                    TempData["exist"] = "Username already exists.";
                    return View("Registration");
                }
            }

            return RedirectToAction("Login", "Login");
        }

        public int? ExistUser(string username)
        {
            int namecount;
            namecount = db.Users.Where(x => x.UserName == username).Count();
            return namecount;
        }
    }
}