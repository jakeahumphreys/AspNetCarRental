namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfigurations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OpeningTime = c.String(),
                        ClosingTime = c.String(),
                        IsOpenForRentals = c.Boolean(nullable: false),
                        LateReturnEligibility = c.Int(nullable: false),
                        IsConfigurationActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.SupportingDocuments", "DocumentType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SupportingDocuments", "DocumentType", c => c.String());
            DropTable("dbo.Configurations");
        }
    }
}
