using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EIRLSSAssignment1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Additional fields

        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name="Blacklisted")]
        public  bool IsBlackListed { get; set; }
        [Display(Name = "Trusted")]
        public bool IsTrustedCustomer { get; set; }
        [Display(Name = "Account Locked Until")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        override public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Account can be locked")]
        override public bool LockoutEnabled { get;set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<OptionalExtra> OptionalExtras { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DrivingLicense> DrivingLicenses { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<ExtensionRequest> ExtensionRequests { get; set; }

    }
}