namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOptionalExtraFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptionalExtras", "serialNumber", c => c.String());
            AddColumn("dbo.OptionalExtras", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OptionalExtras", "Remarks");
            DropColumn("dbo.OptionalExtras", "serialNumber");
        }
    }
}
