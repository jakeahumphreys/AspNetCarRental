namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtensions : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.ExtensionRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        extensionRequestStatus = c.Int(nullable: false),
                        BookingId = c.Int(nullable: false),
                        EndDateRequest = c.DateTime(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtensionRequests", "BookingId", "dbo.Bookings");
            DropIndex("dbo.ExtensionRequests", new[] { "BookingId" });
            DropTable("dbo.ExtensionRequests");
        }
    }
}
