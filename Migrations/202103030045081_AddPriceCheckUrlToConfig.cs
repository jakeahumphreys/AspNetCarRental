namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceCheckUrlToConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "PriceCheckUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "PriceCheckUrl");
        }
    }
}
