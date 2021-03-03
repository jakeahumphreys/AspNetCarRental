using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class DrivingLicense
    {
        public int Id { get; set; }
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; }
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Display(Name = "Forenames")]
        public string Forenames { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Year of Issue")]
        public DateTime YearOfIssue { get; set; }
        [Display(Name = "Expires")]
        public DateTime Expires { get; set; }
        [Display(Name = "Issuing Authority")]
        public string IssuingAuthority { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}