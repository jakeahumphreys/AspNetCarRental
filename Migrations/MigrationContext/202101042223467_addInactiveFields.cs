namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInactiveFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptionalExtras", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vehicles", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.FuelTypes", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.VehicleTypes", "IsInactive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.VehicleTypes", "IsInactive");
            DropColumn("dbo.FuelTypes", "IsInactive");
            DropColumn("dbo.Vehicles", "IsInactive");
            DropColumn("dbo.OptionalExtras", "IsInactive");
        }
    }
}
