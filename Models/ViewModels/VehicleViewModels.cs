using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Models.ViewModels
{
    public class VehicleViewModel
    {
        public Vehicle vehicle { get; set; }
        public SelectList FuelTypes { get; set; }
        public SelectList VehicleTypes { get; set; }
    }
}