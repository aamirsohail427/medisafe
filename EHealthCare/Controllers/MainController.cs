using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EHealthCare.Models;

namespace EHealthCare.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        EHealthCareEntities db = new EHealthCareEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FeedBack(string Name, string Email, string Subject, string Message)
        {
            Contact contact = new Contact();
            contact.Name = Name;
            contact.Email = Email;
            contact.Subject = Subject;
            contact.Message = Message;
            db.Contacts.Add(contact);
            db.SaveChanges();
            string msg = "submit";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}