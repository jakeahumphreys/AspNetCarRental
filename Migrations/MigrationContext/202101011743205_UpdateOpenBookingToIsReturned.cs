namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOpenBookingToIsReturned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "IsReturned", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "IsOpenBooking");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "IsOpenBooking", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "IsReturned");
        }
    }
}
