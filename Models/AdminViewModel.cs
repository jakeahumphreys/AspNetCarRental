using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class AdminViewModel
    {
        public List<Booking> Bookings { get; set; }
        public List<Configuration> Configurations { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<VehicleType> VehicleTypes { get; set; }
        public List<FuelType> FuelTypes { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}