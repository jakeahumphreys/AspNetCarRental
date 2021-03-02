namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeSupportingDocuments : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SupportingDocuments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SupportingDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FamilyName = c.String(),
                        Forenames = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        AddressOfClaim = c.String(),
                        DateOfClaim = c.DateTime(nullable: false),
                        InsurerCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
