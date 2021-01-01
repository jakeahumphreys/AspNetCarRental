namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOptionalExtras : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings");
            DropIndex("dbo.OptionalExtras", new[] { "bookingId" });
            AlterColumn("dbo.OptionalExtras", "bookingId", c => c.Int());
            CreateIndex("dbo.OptionalExtras", "bookingId");
            AddForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings", "Id");
            DropColumn("dbo.OptionalExtras", "QtyAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptionalExtras", "QtyAvailable", c => c.Int(nullable: false));
            DropForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings");
            DropIndex("dbo.OptionalExtras", new[] { "bookingId" });
            AlterColumn("dbo.OptionalExtras", "bookingId", c => c.Int(nullable: false));
            CreateIndex("dbo.OptionalExtras", "bookingId");
            AddForeignKey("dbo.OptionalExtras", "bookingId", "dbo.Bookings", "Id", cascadeDelete: true);
        }
    }
}
