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
        [Display(Name ="Equipment Name")]
        public string Name { get; set; }
        [Display(Name = "Quantity Available")]
        public int QtyAvailable { get; set; }
    }
}