namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBookingCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "BookingCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "BookingCost");
        }
    }
}
