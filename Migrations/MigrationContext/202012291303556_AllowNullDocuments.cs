namespace EIRLSSAssignment1.Migrations.MigrationContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullDocuments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses");
            DropForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments");
            DropIndex("dbo.AspNetUsers", new[] { "DrivingLicenseId" });
            DropIndex("dbo.AspNetUsers", new[] { "SupportingDocumentId" });
            AlterColumn("dbo.AspNetUsers", "DrivingLicenseId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "SupportingDocumentId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "DrivingLicenseId");
            CreateIndex("dbo.AspNetUsers", "SupportingDocumentId");
            AddForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses", "Id");
            AddForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments");
            DropForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses");
            DropIndex("dbo.AspNetUsers", new[] { "SupportingDocumentId" });
            DropIndex("dbo.AspNetUsers", new[] { "DrivingLicenseId" });
            AlterColumn("dbo.AspNetUsers", "SupportingDocumentId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "DrivingLicenseId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "SupportingDocumentId");
            CreateIndex("dbo.AspNetUsers", "DrivingLicenseId");
            AddForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses", "Id", cascadeDelete: true);
        }
    }
}
