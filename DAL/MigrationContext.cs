
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public class MigrationContext : IdentityDbContext<ApplicationUser>
    {
        public MigrationContext() : base("DefaultConnection") { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<OptionalExtra> OptionalExtras { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DrivingLicense> DrivingLicenses { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}