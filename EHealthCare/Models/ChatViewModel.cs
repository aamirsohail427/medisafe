using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHealthCare.Models
{
    public class ChatViewModel
    {
        public int ReceiverID { get; set; }
        public int ChatID { get; set; }

        public string SenderName { get; set; }
        public string UserPhoto { get; set; }


        public int SenderID { get; set; }

        public List<ChatMessages> AllMessages { get; set; }

        public string Layout { get; set; }

    }
}