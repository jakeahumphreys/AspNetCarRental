
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace EIRLSSAssignment1.DAL
{
    public class VehicleTypeContext : IdentityDbContext<ApplicationUser>
    {
        public VehicleTypeContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}