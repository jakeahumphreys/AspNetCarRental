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
        [Required]
        [Display(Name = "DVLA 15 Digit Reference")]
        [StringLength(15)]
        public string DvlaReference { get; set; }
        [Display(Name = "DVLA Import URL")]
        public string DvlaImportUrl { get; set; }
        [Display(Name = "ABI Import URL")]
        public string AbiImportUrl { get; set; }
        [Display(Name = "Price Check URL")]
        public string PriceCheckUrl { get; set; }
        [Display(Name ="Active?")]
        public bool IsConfigurationActive { get; set; }
    }
}