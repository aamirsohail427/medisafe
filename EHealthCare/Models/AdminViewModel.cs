using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EHealthCare.Models
{
    public class AdminViewModel
    {
        public List<Prescription> PrescriptionList { get; set; }
        public List<Patient> PatientList { get; set; }
        public List<City> CityList { get; set; }
        public List<Doctor> DoctorList { get; set; }
        public List<Medicine> MedicineList { get; set; }
        public List<Staff> StaffList { get; set; }
        public List<LabTest> TestList { get; set; }
        public List<Appointment> AppointmentsList { get; set; }
        public List<Payment> PaymentList { get; set; }

        public Staff Staff { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public Prescription Prescription { get; set; }
        public RoomAllotment RoomAllotment { get; set; }
        public User User { get; set; }
        public UserProfile UserProfile { get; set; }
        public LabTest LabTest { get; set; }
        public Payment Payment { get; set; }
        public Appointment Appointment { get; set; }

        public string ToTime { get; set; }
        public string FromTime { get; set; }
        public string AppointmentDate { get; set; }

        public int DoctorCount { get; set; }
        public int StaffCount { get; set; }
        public int TotalRoomCount { get; set; }
        public int EarningCount { get; set; }
        public int PatientCount { get; set; }
        public int AppointmentCount { get; set; }
        public int UserCount { get; set; }
        public int PrescriptionCount { get; set; }

        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }

    }
}