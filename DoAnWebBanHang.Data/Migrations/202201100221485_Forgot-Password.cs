namespace DoAnWebBanHang.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "VerificationToken", c => c.String());
            AddColumn("dbo.ApplicationUsers", "PasswordVerificationToken", c => c.String());
            AddColumn("dbo.ApplicationUsers", "DateValidateToken", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "DateValidateToken");
            DropColumn("dbo.ApplicationUsers", "PasswordVerificationToken");
            DropColumn("dbo.ApplicationUsers", "VerificationToken");
        }
    }
}
