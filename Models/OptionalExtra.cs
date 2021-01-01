using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class OptionalExtra
    {
        public int Id { get; set; }
        [Display(Name = "Equipment Name")]
        public string Name { get; set; }
        [Display(Name = "Serial Number")]
        public string serialNumber { get; set; }
        public string Remarks { get; set; }
        //EF6 References
        public int? bookingId { get; set; }
        public Booking booking { get; set; }
        public string DisplayString
        {
            get
            {
                return Name + " (" + serialNumber + ")";
            }
        }
    }
}