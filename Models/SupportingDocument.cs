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
        [Required]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }
        [Required]
        [Display(Name = "Date Recieved")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateRecieved { get; set; }
        public byte[] Image { get; set; }
    }
}