using EIRLSSAssignment1.Customisations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EIRLSSAssignment1.RepeatLogic;

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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Starts")]
        public DateTime BookingStart { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ends")]
        public DateTime BookingFinish { get; set; }
        [Display(Name = "Late Return")]
        public bool IsLateReturn { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [Display(Name = "Optional Extras")]
        public ICollection<OptionalExtra> OptionalExtras { get; set; }
        public string Remarks { get; set; }
        public bool IsReturned { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate {get;set;}
        [Display(Name = "Cost of Booking")]
        public double BookingCost { get; set; }
        [Display(Name = "Booking Status")]
        [EnumDataType(typeof (BookingStatus))]
        public BookingStatus BookingStatus { get; set; }
        [Display(Name = "Booking")]
        public string DisplayString
        {
            get
            {
                return "Booking #" + Id + " (" + BookingStart.ToString("d") + " to " + BookingFinish.ToString("d") + ")";
            }
        }

    }
}