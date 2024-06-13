using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHealthCare.Models
{
    public class AppointmentViewModel
    {
        public List<Doctor> DoctorsList { get; set; }

        public Appointment Appointment { get; set; }
    }
}