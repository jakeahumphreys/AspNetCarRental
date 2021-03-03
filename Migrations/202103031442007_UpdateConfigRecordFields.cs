namespace EIRLSSAssignment1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateConfigRecordFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "SmtpEmailFrom", c => c.String());
            AddColumn("dbo.Configurations", "SmtpSenderUsername", c => c.String());
            DropColumn("dbo.Configurations", "SmtpSenderEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Configurations", "SmtpSenderEmail", c => c.String());
            DropColumn("dbo.Configurations", "SmtpSenderUsername");
            DropColumn("dbo.Configurations", "SmtpEmailFrom");
        }
    }
}
