
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public class VehicleTypeContext : IdentityDbContext<ApplicationUser>
    {
        public VehicleTypeContext() : base("DefaultConnection") { }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}