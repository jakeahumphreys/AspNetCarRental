namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMinMaxRentalHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "MinRentalHours", c => c.Int(nullable: false));
            AddColumn("dbo.Configurations", "MaxRentalHours", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "MaxRentalHours");
            DropColumn("dbo.Configurations", "MinRentalHours");
        }
    }
}
