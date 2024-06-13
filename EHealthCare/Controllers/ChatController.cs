using EHealthCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHealthCare.Controllers
{
    
    public class ChatController : Controller
    {
        EHealthCareEntities db = new EHealthCareEntities();
        // GET: Chat
        public ActionResult ChatBox(int ?docid,int ? ChatIDD)
        {
           
            ChatViewModel Chatmodel = new ChatViewModel();
            Doctor DocModel = new Doctor();
            int ChatID = 0;
            int doctorid = 0;
            int patientid = 0;
            if(ChatIDD!=null)
            {
                Chat ChatData = new Chat();
                ChatData = db.Chats.FirstOrDefault(a => a.ChatId == ChatIDD);
                patientid = Convert.ToInt32(ChatData.FirstChatUserID);
                doctorid= Convert.ToInt32(ChatData.SecondChatUserID);
                DocModel = db.Doctors.FirstOrDefault(a => a.UserID == doctorid);
                Chatmodel.SenderID = Convert.ToInt32(Session["UserId"]);
                Patient PatientModel = new Patient();
                PatientModel = db.Patients.FirstOrDefault(a => a.UserID == patientid);
                Chatmodel.SenderName = DocModel.FirstName;
                Chatmodel.ReceiverID = patientid;
                Chatmodel.ChatID = Convert.ToInt32(ChatIDD);
                Chatmodel.UserPhoto = DocModel.Image;
                Chatmodel.Layout = "_DoctorLayout.cshtml";
                var AllChatMessages = db.ChatMessages.Where(a => a.ChatId == ChatIDD).ToList();
                Chatmodel.AllMessages = new List<ChatMessages>();

                foreach (var item in AllChatMessages)
                {
                    ChatMessages Addmessages = new ChatMessages();
                    User GetUser = new User();
                    GetUser = db.Users.FirstOrDefault(a => a.UserID == item.MessageBy);
                    if (GetUser!=null && GetUser.UserType==3)
                    {
                        Doctor GetDoctor = new Doctor();
                        GetDoctor = db.Doctors.FirstOrDefault(a => a.UserID == item.MessageBy);
                        Addmessages.FullName = GetDoctor.FirstName;
                        Addmessages.Message = item.Message;
                        Addmessages.UserPhoto = GetDoctor.Image;
                        Addmessages.CreatedDate = item.CreatedDate;
                        Chatmodel.AllMessages.Add(Addmessages);

                    }
                    else if(GetUser != null && GetUser.UserType == 2)
                    {
                        Patient GetPatient = new Patient();
                        GetPatient = db.Patients.FirstOrDefault(a => a.UserID == item.MessageBy);
                        Addmessages.FullName = GetPatient.FirstName;
                        Addmessages.Message = item.Message;
                        Addmessages.UserPhoto = GetPatient.Image;
                        Addmessages.CreatedDate = item.CreatedDate;
                        Chatmodel.AllMessages.Add(Addmessages);

                    }



                }
            }
            else
            {
                patientid= Convert.ToInt32(Session["UserId"]);
                doctorid = Convert.ToInt32(docid);
                DocModel = db.Doctors.FirstOrDefault(a => a.UserID == doctorid);
                Chatmodel.SenderID = Convert.ToInt32(Session["UserId"]);
                Patient PatientModel = new Patient();
                PatientModel = db.Patients.FirstOrDefault(a => a.UserID == patientid);
                Chatmodel.SenderName = PatientModel.FirstName;
                Chatmodel.ReceiverID = doctorid;
                Chat ChatExist = db.Chats.FirstOrDefault(a => (a.FirstChatUserID == Chatmodel.SenderID && a.SecondChatUserID == Chatmodel.ReceiverID) || (a.FirstChatUserID == Chatmodel.ReceiverID && a.SecondChatUserID == Chatmodel.SenderID));

                if (ChatExist != null && ChatExist.ChatId > 0)
                {
                    ChatID = Convert.ToInt32(ChatExist.ChatId);
                }
                else
                {
                    Chat StartChat = new Chat();
                    StartChat.IsActive = true;
                    StartChat.FirstChatUserID = Chatmodel.SenderID;
                    StartChat.SecondChatUserID = Chatmodel.ReceiverID;
                    db.Chats.Add(StartChat);
                    db.SaveChanges();
                    ChatID = Convert.ToInt32(StartChat.ChatId);

                }
                Chatmodel.ChatID = Convert.ToInt32(ChatID);
                Chatmodel.UserPhoto = PatientModel.Image;
                Chatmodel.Layout = "_PatientLayout.cshtml";
                var AllChatMessages = db.ChatMessages.Where(a => a.ChatId == Chatmodel.ChatID).ToList();
                Chatmodel.AllMessages = new List<ChatMessages>();

                foreach (var item in AllChatMessages)
                {
                    ChatMessages Addmessages = new ChatMessages();
                    User GetUser = new User();
                    GetUser = db.Users.FirstOrDefault(a => a.UserID == item.MessageBy);
                    if (GetUser != null && GetUser.UserType == 3)
                    {
                        Doctor GetDoctor = new Doctor();
                        GetDoctor = db.Doctors.FirstOrDefault(a => a.UserID == item.MessageBy);
                        Addmessages.FullName = GetDoctor.FirstName;
                        Addmessages.Message = item.Message;
                        Addmessages.UserPhoto = GetDoctor.Image;
                        Addmessages.CreatedDate = item.CreatedDate;
                        Chatmodel.AllMessages.Add(Addmessages);

                    }
                    else if (GetUser != null && GetUser.UserType == 2)
                    {
                        Patient GetPatient = new Patient();
                        GetPatient = db.Patients.FirstOrDefault(a => a.UserID == item.MessageBy);
                        Addmessages.FullName = GetPatient.FirstName;
                        Addmessages.Message = item.Message;
                        Addmessages.UserPhoto = GetPatient.Image;
                        Addmessages.CreatedDate = item.CreatedDate;
                        Chatmodel.AllMessages.Add(Addmessages);

                    }



                }


            }

        
           
        
      

          


            return View(Chatmodel);
        }

        public ActionResult GetChatUsers()
        {
            List<Doctor> DoctorsList = new List<Doctor>();
            DoctorsList = db.Doctors.ToList();
            return PartialView("_ChatDoctors", DoctorsList);

        }
        public ActionResult AddChatMessages(string Message, string SenderName, string SenderPhoto, int ChatID, string receiverID)
        {
         
            ChatMessage AddChatMessage = new ChatMessage();
            AddNewChatMessage AddMessageModel = new AddNewChatMessage();
            AddChatMessage.ChatId = ChatID;
            AddChatMessage.MessageBy = Convert.ToInt32(Session["UserId"]);
            AddChatMessage.Message = Message;
            AddChatMessage.IsRead = false;
            AddChatMessage.IsActive = true;
            AddChatMessage.CreatedBy = Convert.ToInt32(Session["UserId"]); ;
            AddChatMessage.CreatedDate = DateTime.UtcNow;
            db.ChatMessages.Add(AddChatMessage);
            db.SaveChanges();
            AddMessageModel.Chat = AddChatMessage;
            AddMessageModel.SenderName = SenderName;
            AddMessageModel.SenderPhoto = SenderPhoto;
          
            return Json(new { error = true, message = RenderRazorViewToString("_AddNewMessagePartialView", AddMessageModel), chatmessageid = AddChatMessage.ChatMessageId }, JsonRequestBehavior.AllowGet);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}