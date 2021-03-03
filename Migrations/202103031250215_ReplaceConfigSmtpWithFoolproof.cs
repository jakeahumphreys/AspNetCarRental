namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceConfigSmtpWithFoolproof : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Configurations", "SmtpUrl", c => c.String());
            AlterColumn("dbo.Configurations", "SmtpSenderEmail", c => c.String());
            AlterColumn("dbo.Configurations", "SmtpSenderPassword", c => c.String());
            AlterColumn("dbo.Configurations", "SmtpRecipientEmail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Configurations", "SmtpRecipientEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "SmtpSenderPassword", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "SmtpSenderEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Configurations", "SmtpUrl", c => c.String(nullable: false));
        }
    }
}
