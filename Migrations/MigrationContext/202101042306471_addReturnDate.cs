namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReturnDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "ReturnDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "ReturnDate");
        }
    }
}
