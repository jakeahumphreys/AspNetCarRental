namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlsToConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "DvlaImportUrl", c => c.String());
            AddColumn("dbo.Configurations", "AbiImportUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "AbiImportUrl");
            DropColumn("dbo.Configurations", "DvlaImportUrl");
        }
    }
}
