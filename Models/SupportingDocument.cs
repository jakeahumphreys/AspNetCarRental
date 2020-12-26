using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class SupportingDocument
    {
        public int Id { get; set; }
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }
        [Display(Name = "Date Recieved")]
        public DateTime DateRecieved { get; set; }
        public byte[] Image { get; set; }
    }
}