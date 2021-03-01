namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDvlaReference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "DvlaReference", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "DvlaReference");
        }
    }
}
