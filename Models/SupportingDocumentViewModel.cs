using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class SupportingDocumentViewModel
    {
        public SupportingDocument SupportingDocument { get; set; }
        public HttpPostedFileBase ImageToUpload { get; set; }
        public string userId { get; set; }
    }
}