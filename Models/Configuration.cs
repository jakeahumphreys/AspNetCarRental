using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name="Opening Time")]
        public DateTime OpeningTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Closing Time")]
        public DateTime ClosingTime { get; set; }
        [Required]
        [Display(Name = "Open for Rentals?")]
        public bool IsOpenForRentals { get; set; }
        [Required]
        [Display(Name = "Auto LR Eligibility")]
        public int LateReturnEligibility { get; set; }
        [Required]
        [Display(Name = "Minimum Rental Period (Hours)")]
        public int MinRentalHours { get; set; }
        [Required]
        [Display(Name = "Maximum Rental Period (Hours)")]
        public int MaxRentalHours { get; set; }
        public bool IsConfigurationActive { get; set; }
    }
}