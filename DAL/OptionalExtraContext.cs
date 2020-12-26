
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public class OptionalExtraContext : IdentityDbContext<ApplicationUser>
    {
        public OptionalExtraContext() : base("DefaultConnection") { }

        public DbSet<OptionalExtra> OptionalExtras { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}