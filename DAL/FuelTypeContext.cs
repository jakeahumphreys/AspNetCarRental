
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public class FuelTypeContext : IdentityDbContext<ApplicationUser>
    {
        public FuelTypeContext() : base("DefaultConnection") { }

        public DbSet<FuelType> FuelTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}