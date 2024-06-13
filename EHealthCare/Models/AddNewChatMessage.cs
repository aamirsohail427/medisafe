using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHealthCare.Models
{
    public class AddNewChatMessage
    {
        public ChatMessage Chat { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoto { get; set; }
    }
}