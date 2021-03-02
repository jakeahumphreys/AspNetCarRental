using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class DvlaImportedLicense
    {
        public string LicenseNumber { get; set; }
        public string FamilyName { get; set; }
        public string Forenames { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime YearOfIssue { get; set; }
        public DateTime Expires { get; set; }
        public string IssuingAuthority { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}