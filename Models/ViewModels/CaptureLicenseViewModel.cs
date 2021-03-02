using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models.ViewModels
{
    public class CaptureLicenseViewModel
    {
        public int BookingId { get; set; }
        public DrivingLicense License { get; set; }
        public HttpPostedFileBase LicenseImage { get; set; }
    }
}