namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpenBookingField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "IsOpenBooking", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "IsOpenBooking");
        }
    }
}
