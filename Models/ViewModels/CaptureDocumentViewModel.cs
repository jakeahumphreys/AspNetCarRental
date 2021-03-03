using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models.ViewModels
{
    public class CaptureDocumentViewModel
    {
        public int BookingId { get; set; }
        public HttpPostedFileBase DocumentImage { get; set; }
        public SupportingDocument SupportingDocument { get; set; }
    }
}