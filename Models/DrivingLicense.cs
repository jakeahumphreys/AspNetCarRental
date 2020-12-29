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
        public string Surname { get; set; }
        public string Forenames { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Place of Birth")]
        public String PlaceOfBirth { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Issue")]
        public DateTime DateOfIssue { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Expiry")]
        public DateTime DateOfExpiry { get; set; }
        [Display(Name = "Issued By")]
        public string IssuedBy { get; set; }
        [Display(Name = "License #")]
        public string LicenseNumber { get; set; }
        public string Address { get; set; }
        public byte[] Image { get; set; }
    }
}