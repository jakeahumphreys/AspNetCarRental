using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Models
{
    public class DrivingLicenseViewModel
    {
        public DrivingLicense License { get; set; }
        public HttpPostedFileBase ImageToUpload { get; set; }
        [Display(Name ="User")]
        public string userId { get; set; }
        public SelectList Users { get; set; }
        
    }
}