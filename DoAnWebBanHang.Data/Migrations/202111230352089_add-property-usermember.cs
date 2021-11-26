namespace DoAnWebBanHang.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpropertyusermember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "isMember", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "isMember");
        }
    }
}
