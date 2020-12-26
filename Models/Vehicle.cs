using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Display(Name = "Vehicle Registration Mark")]
        public string VRM { get; set; }
        [Display(Name = "Vehicle Identification Number")]
        public string VIN { get; set; }
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }
        [Display(Name = "Fuel Type")]
        public FuelType FuelType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        [Display(Name = "Engine Size (Litres)")]
        public int? EngineSize { get; set; }
        public string Transmission { get; set; }
        [Display(Name = "Rental Cost")]
        public double RentalCost { get; set; }
        [Display(Name = "Rental Minimum Age")]
        public int MinimumAgeToRent { get; set; }
        public string Remarks { get; set; }
    }
}