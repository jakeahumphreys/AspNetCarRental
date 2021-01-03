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
        public string OpeningTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string ClosingTime { get; set; }
        [Required]
        public bool IsOpenForRentals { get; set; }
        [Required]
        public int LateReturnEligibility { get; set; }
        public bool IsConfigurationActive { get; set; }
    }
}