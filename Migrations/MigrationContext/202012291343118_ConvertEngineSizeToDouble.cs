namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertEngineSizeToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "EngineSize", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "EngineSize", c => c.Int());
        }
    }
}
