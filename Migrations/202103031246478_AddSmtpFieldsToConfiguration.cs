namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSmtpFieldsToConfiguration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "SmtpShouldSendEmail", c => c.Boolean(nullable: false));
            AddColumn("dbo.Configurations", "SmtpUrl", c => c.String(nullable: false));
            AddColumn("dbo.Configurations", "SmtpPort", c => c.Int(nullable: false));
            AddColumn("dbo.Configurations", "SmtpSenderEmail", c => c.String(nullable: false));
            AddColumn("dbo.Configurations", "SmtpSenderPassword", c => c.String(nullable: false));
            AddColumn("dbo.Configurations", "SmtpShouldUseSsl", c => c.Boolean(nullable: false));
            AddColumn("dbo.Configurations", "SmtpRecipientEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "DvlaImportUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "AbiImportUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "PriceCheckUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Configurations", "PriceCheckUrl", c => c.String());
            AlterColumn("dbo.Configurations", "AbiImportUrl", c => c.String());
            AlterColumn("dbo.Configurations", "DvlaImportUrl", c => c.String());
            DropColumn("dbo.Configurations", "SmtpRecipientEmail");
            DropColumn("dbo.Configurations", "SmtpShouldUseSsl");
            DropColumn("dbo.Configurations", "SmtpSenderPassword");
            DropColumn("dbo.Configurations", "SmtpSenderEmail");
            DropColumn("dbo.Configurations", "SmtpPort");
            DropColumn("dbo.Configurations", "SmtpUrl");
            DropColumn("dbo.Configurations", "SmtpShouldSendEmail");
        }
    }
}
