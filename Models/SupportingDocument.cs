using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class SupportingDocument
    {
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Display(Name = "Forenames")]
        public string Forenames { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
    }

}