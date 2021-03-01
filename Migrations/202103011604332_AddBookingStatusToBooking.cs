namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingStatusToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "BookingStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "BookingStatus");
        }
    }
}
