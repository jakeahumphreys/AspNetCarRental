using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class DrivingLicenseViewModel
    {
        public DrivingLicense License { get; set; }
        public HttpPostedFileBase ImageToUpload { get; set; }
        public string userId { get; set; }
    }
}