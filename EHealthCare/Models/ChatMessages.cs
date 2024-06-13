using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHealthCare.Models
{
    public class ChatMessages
    {
       
        public string Message { get; set; }
        public string FullName { get; set; }
        public string UserPhoto { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}