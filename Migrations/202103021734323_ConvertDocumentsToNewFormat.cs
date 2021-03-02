namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertDocumentsToNewFormat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses");
            DropForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments");
            DropIndex("dbo.AspNetUsers", new[] { "DrivingLicenseId" });
            DropIndex("dbo.AspNetUsers", new[] { "SupportingDocumentId" });
            AddColumn("dbo.DrivingLicenses", "FamilyName", c => c.String());
            AddColumn("dbo.DrivingLicenses", "YearOfIssue", c => c.DateTime(nullable: false));
            AddColumn("dbo.DrivingLicenses", "Expires", c => c.DateTime(nullable: false));
            AddColumn("dbo.DrivingLicenses", "IssuingAuthority", c => c.String());
            AddColumn("dbo.DrivingLicenses", "Status", c => c.String());
            AddColumn("dbo.DrivingLicenses", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.SupportingDocuments", "FamilyName", c => c.String());
            AddColumn("dbo.SupportingDocuments", "Forenames", c => c.String());
            AddColumn("dbo.SupportingDocuments", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.SupportingDocuments", "AddressOfClaim", c => c.String());
            AddColumn("dbo.SupportingDocuments", "DateOfClaim", c => c.DateTime(nullable: false));
            AddColumn("dbo.SupportingDocuments", "InsurerCode", c => c.String());
            DropColumn("dbo.AspNetUsers", "DrivingLicenseId");
            DropColumn("dbo.AspNetUsers", "SupportingDocumentId");
            DropColumn("dbo.DrivingLicenses", "Surname");
            DropColumn("dbo.DrivingLicenses", "PlaceOfBirth");
            DropColumn("dbo.DrivingLicenses", "DateOfIssue");
            DropColumn("dbo.DrivingLicenses", "DateOfExpiry");
            DropColumn("dbo.DrivingLicenses", "IssuedBy");
            DropColumn("dbo.DrivingLicenses", "Image");
            DropColumn("dbo.SupportingDocuments", "DocumentType");
            DropColumn("dbo.SupportingDocuments", "DateRecieved");
            DropColumn("dbo.SupportingDocuments", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SupportingDocuments", "Image", c => c.Binary());
            AddColumn("dbo.SupportingDocuments", "DateRecieved", c => c.DateTime(nullable: false));
            AddColumn("dbo.SupportingDocuments", "DocumentType", c => c.String(nullable: false));
            AddColumn("dbo.DrivingLicenses", "Image", c => c.Binary());
            AddColumn("dbo.DrivingLicenses", "IssuedBy", c => c.String());
            AddColumn("dbo.DrivingLicenses", "DateOfExpiry", c => c.DateTime(nullable: false));
            AddColumn("dbo.DrivingLicenses", "DateOfIssue", c => c.DateTime(nullable: false));
            AddColumn("dbo.DrivingLicenses", "PlaceOfBirth", c => c.String());
            AddColumn("dbo.DrivingLicenses", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "SupportingDocumentId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DrivingLicenseId", c => c.Int());
            DropColumn("dbo.SupportingDocuments", "InsurerCode");
            DropColumn("dbo.SupportingDocuments", "DateOfClaim");
            DropColumn("dbo.SupportingDocuments", "AddressOfClaim");
            DropColumn("dbo.SupportingDocuments", "DateOfBirth");
            DropColumn("dbo.SupportingDocuments", "Forenames");
            DropColumn("dbo.SupportingDocuments", "FamilyName");
            DropColumn("dbo.DrivingLicenses", "Date");
            DropColumn("dbo.DrivingLicenses", "Status");
            DropColumn("dbo.DrivingLicenses", "IssuingAuthority");
            DropColumn("dbo.DrivingLicenses", "Expires");
            DropColumn("dbo.DrivingLicenses", "YearOfIssue");
            DropColumn("dbo.DrivingLicenses", "FamilyName");
            CreateIndex("dbo.AspNetUsers", "SupportingDocumentId");
            CreateIndex("dbo.AspNetUsers", "DrivingLicenseId");
            AddForeignKey("dbo.AspNetUsers", "SupportingDocumentId", "dbo.SupportingDocuments", "Id");
            AddForeignKey("dbo.AspNetUsers", "DrivingLicenseId", "dbo.DrivingLicenses", "Id");
        }
    }
}
