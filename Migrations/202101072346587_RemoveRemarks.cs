namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRemarks : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ExtensionRequests", "Remarks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExtensionRequests", "Remarks", c => c.String());
        }
    }
}
