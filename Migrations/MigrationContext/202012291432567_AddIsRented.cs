namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRented : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "IsRented", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "IsRented");
        }
    }
}
