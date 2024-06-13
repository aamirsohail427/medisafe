using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EHealthCare.Models;
using System.ComponentModel.DataAnnotations;

namespace EHealthCare.Models
{
    public class PatientViewModel
    {
        public UserProfile UserProfile { get; set; }
        public User User { get; set; }
        public Patient Patient { get; set; }
        public Prescription Prescription { get; set; }
        public List<Prescription> PrescriptionList { get; set; }
        public List<Appointment> AppointmentList { get; set; }

        public List<LabTest> ParientLabTestList { get; set; }
        public List<City> CityList { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }
    }
}