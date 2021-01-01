namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReferenceToBooking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptionalExtras", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.OptionalExtras", new[] { "Booking_Id" });
            RenameColumn(table: "dbo.OptionalExtras", name: "Booking_Id", newName: "bookingId");
            AlterColumn("dbo.OptionalExtras", "bookingId", c => c.Int(nullable: false));
            CreateIndex("dbo.OptionalExtras", "bookingId");
            AddForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings");
            DropIndex("dbo.OptionalExtras", new[] { "bookingId" });
            AlterColumn("dbo.OptionalExtras", "bookingId", c => c.Int());
            RenameColumn(table: "dbo.OptionalExtras", name: "bookingId", newName: "Booking_Id");
            CreateIndex("dbo.OptionalExtras", "Booking_Id");
            AddForeignKey("dbo.OptionalExtras", "Booking_Id", "dbo.Bookings", "Id");
        }
    }
}
