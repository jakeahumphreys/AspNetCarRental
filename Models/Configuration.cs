using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public bool IsOpenForRentals { get; set; }
        public int LateReturnEligibility { get; set; }
        public bool IsConfigurationActive { get; set; }
    }
}