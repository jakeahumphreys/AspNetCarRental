namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagesToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DrivingLicenseImage", c => c.Binary());
            AddColumn("dbo.Bookings", "SupportingDocumentImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "SupportingDocumentImage");
            DropColumn("dbo.Bookings", "DrivingLicenseImage");
        }
    }
}
