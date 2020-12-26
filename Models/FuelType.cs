using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class FuelType
    {
        public int Id { get; set; }
        [Display(Name = "Fuel Type")]
        public string Value { get; set; }
    }
}