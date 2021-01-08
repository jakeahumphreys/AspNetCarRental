using EIRLSSAssignment1.Models.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class ExtensionRequest
    {
        public int Id { get; set; }
        [Display(Name = "Request Status")]
        public ExtensionStatus extensionRequestStatus { get; set; }
        [Display(Name = "Booking Id")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        [Display(Name = "Requested End Date")]
        public DateTime EndDateRequest { get; set; }
    }
}