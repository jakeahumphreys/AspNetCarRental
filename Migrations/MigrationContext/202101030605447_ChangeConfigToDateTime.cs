namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeConfigToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Configurations", "OpeningTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Configurations", "ClosingTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Configurations", "ClosingTime", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "OpeningTime", c => c.String(nullable: false));
        }
    }
}
