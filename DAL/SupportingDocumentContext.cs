
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public class SupportingDocumentContext : IdentityDbContext<ApplicationUser>
    {
        public SupportingDocumentContext() : base("DefaultConnection") { }

        public DbSet<SupportingDocument> SupportingDocuments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}