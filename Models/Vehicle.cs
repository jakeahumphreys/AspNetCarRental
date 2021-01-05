using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Display(Name = "VRM")]
        public string VRM { get; set; }
        [Display(Name = "VIN")]
        public string VIN { get; set; }
        public int VehicleTypeId { get; set; }
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }
        public int FuelTypeId { get; set; }
        [Display(Name = "Fuel")]
        public FuelType FuelType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        [Display(Name = "Engine Size (Litres)")]
        public double EngineSize { get; set; }
        public string Transmission { get; set; }
        [Display(Name = "Rental Cost")]
        public double RentalCost { get; set; }
        [Display(Name = "Rental Minimum Age")]
        public int MinimumAgeToRent { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Currently Rented?")]
        public bool IsRented { get; set; }
        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }
        [Display(Name ="Vehicle")]
        public string DisplayString
        {
            get
            {
                return Make + " " + Model + " (" + VRM + ") " + "[" + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + RentalCost + " p.d]";
            }
        }

    }
}