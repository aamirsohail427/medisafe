using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EHealthCare.Models
{
    class ModelValidation
    {
    }
    [MetadataType(typeof(Staff.Metadata))]
    public partial class Staff
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "First Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Phone Number is Required.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Gender is Required.")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Join Date is Required.")]
            public string JoiningDate { get; set; }
        }
    }
    [MetadataType(typeof(User.Metadata))]
    public partial class User
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "User Name is Required.")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Password is Required.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", ErrorMessage = "Password must have atleast 6 characters length, atleast 1 letter in Upper Case,1 letters in Lower Case")]
            public string Password { get; set; }
        }
    }
    [MetadataType(typeof(UserProfile.Metadata))]
    public partial class UserProfile
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "First Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is Required.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Gender is Required.")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Date of Birth is Required.")]
            public string DOB { get; set; }
        }
    }

    [MetadataType(typeof(Doctor.Metadata))]
    public partial class Doctor
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "First Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is Required.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Gender is Required.")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Date of Birth is Required.")]
            public string DOB { get; set; }

            [Required(ErrorMessage = "Phone is Required.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Join Date is Required.")]
            public string JoiningDate { get; set; }
        }
    }

    [MetadataType(typeof(Patient.Metadata))]
    public partial class Patient
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "First Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required.")]
            [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Only alphabets are allowed")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is Required.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Gender is Required.")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Date of Birth is Required.")]
            public string DOB { get; set; }

            [Required(ErrorMessage = "Phone is Required.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Join Date is Required.")]
            public string JoinDate { get; set; }

            [Required(ErrorMessage = "Age is Required.")]
            public string Age { get; set; }
        }
    }

    [MetadataType(typeof(Prescription.Metadata))]
    public partial class Prescription
    {
        public sealed class Metadata
        {
            [Required(ErrorMessage = "Medicine Name is Required.")]
            public int MedicineID { get; set; }

            [Required(ErrorMessage = "Patient Name is Required.")]
            public int PatientID { get; set; }

            [Required(ErrorMessage = "Reason is Required.")]
            public string Reason { get; set; }

            [Required(ErrorMessage = "Dose is Required.")]
            public string Dose { get; set; }

            [Required(ErrorMessage = "Unit is Required.")]
            public string Unit { get; set; }

            [Required(ErrorMessage = "Start Date is Required.")]
            public string StartDate { get; set; }

            [Required(ErrorMessage = "End Date is Required.")]
            public string EndDate { get; set; }
        }
    }
    [MetadataType(typeof(LabTest.Metadata))]
    public partial class LabTest
    {
        public sealed class Metadata
        {

            [Required(ErrorMessage = "Patient Name is Required.")]
            public int PatientID { get; set; }

            [Required(ErrorMessage = "Test Name is Required.")]
            public string TestName { get; set; }

            [Required(ErrorMessage = "Test Name is Required.")]
            public string TestDate { get; set; }

        }
    }

    [MetadataType(typeof(RoomAllotment.Metadata))]
    public partial class RoomAllotment
    {
        public sealed class Metadata
        {


            [Required(ErrorMessage = "Patient Name is Required.")]
            public int PatientID { get; set; }

            [Required(ErrorMessage = "Allotment Date is Required.")]
            public string Dated { get; set; }

            [Required(ErrorMessage = "Discharge Date is Required.")]
            public string DischargeDate { get; set; }

        }
    }
    
}