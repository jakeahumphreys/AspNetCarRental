namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        BookingStart = c.DateTime(nullable: false),
                        BookingFinish = c.DateTime(nullable: false),
                        IsLateReturn = c.Boolean(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        Remarks = c.String(),
                        IsReturned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.OptionalExtras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        serialNumber = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IsBlackListed = c.Boolean(nullable: false),
                        IsTrustedCustomer = c.Boolean(nullable: false),
                        DrivingLicenseId = c.Int(),
                        SupportingDocumentId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DrivingLicenses", t => t.DrivingLicenseId)
                .ForeignKey("dbo.SupportingDocuments", t => t.SupportingDocumentId)
                .Index(t => t.DrivingLicenseId)
                .Index(t => t.SupportingDocumentId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DrivingLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Surname = c.String(),
                        Forenames = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        PlaceOfBirth = c.String(),
                        DateOfIssue = c.DateTime(nullable: false),
                        DateOfExpiry = c.DateTime(nullable: false),
                        IssuedBy = c.String(),
                        LicenseNumber = c.String(),
                        Address = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SupportingDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentType = c.String(nullable: false),
                        DateRecieved = c.DateTime(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VRM = c.String(),
                        VIN = c.String(),
                        VehicleTypeId = c.Int(nullable: false),
                        FuelTypeId = c.Int(nullable: false),
                        Make = c.String(),
                        Model = c.String(),
                        EngineSize = c.Double(nullable: false),
                        Transmission = c.String(),
                        RentalCost = c.Double(nullable: false),
                        MinimumAgeToRent = c.Int(nullable: false),
                        Remarks = c.String(),
                        IsRented = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FuelTypes", t => t.FuelTypeId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.VehicleTypeId)
                .Index(t => t.FuelTypeId);
            
            CreateTable(
                "dbo.FuelTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        OpeningTime = c.String(nullable: false),
                        ClosingTime = c.String(nullable: false),
                        IsOpenForRentals = c.Boolean(nullable: false),
                        LateReturnEligibility = c.Int(nullable: false),
                        IsConfigurationActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.OptionalExtraBookings",
                c => new
                    {
                        OptionalExtra_Id = c.Int(nullable: false),
                        Booking_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OptionalExtra_Id, t.Booking_Id })
                .ForeignKey("dbo.OptionalExtras", t => t.OptionalExtra_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id, cascadeDelete: true)
                .Index(t => t.OptionalExtra_Id)
                .Index(t => t.Booking_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "FuelTypeId", "dbo.FuelTypes");
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OptionalExtraBookings", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.OptionalExtraBookings", "OptionalExtra_Id", "dbo.OptionalExtras");
            DropIndex("dbo.OptionalExtraBookings", new[] { "Booking_Id" });
            DropIndex("dbo.OptionalExtraBookings", new[] { "OptionalExtra_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Vehicles", new[] { "FuelTypeId" });
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "SupportingDocumentId" });
            DropIndex("dbo.AspNetUsers", new[] { "DrivingLicenseId" });
            DropIndex("dbo.Bookings", new[] { "VehicleId" });
            DropIndex("dbo.Bookings", new[] { "UserId" });
            DropTable("dbo.OptionalExtraBookings");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Configurations");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.FuelTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.SupportingDocuments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.DrivingLicenses");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OptionalExtras");
            DropTable("dbo.Bookings");
        }
    }
}
