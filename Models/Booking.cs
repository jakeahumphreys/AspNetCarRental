using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class Booking
    {
        [Display(Name = "Booking ID")]
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booking Starts")]
        public DateTime BookingStart { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booking Ends")]
        public DateTime BookingFinish { get; set; }
        [Display(Name = "Late return arranged?")]
        public bool IsLateReturn { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [Display(Name = "Optional Extras")]
        public ICollection<OptionalExtra> OptionalExtras { get; set; }
        public string Remarks { get; set; }
        public bool IsReturned { get; set; }
    }
}