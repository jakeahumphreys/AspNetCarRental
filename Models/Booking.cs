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
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Starts")]
        public DateTime BookingStart { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Ends")]
        public DateTime BookingFinish { get; set; }
        [Display(Name = "Late return arranged?")]
        public bool IsLateReturn { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public IList<OptionalExtra> OptionalExtras { get; set; }
        public string Remarks { get; set; }
    }
}