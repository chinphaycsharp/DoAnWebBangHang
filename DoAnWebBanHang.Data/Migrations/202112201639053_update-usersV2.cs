namespace DoAnWebBanHang.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateusersV2 : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetUserNotAdmin", p => new {
                GruopId = p.Int(),
                isMember = p.Boolean()
            }, @"select u.* from ApplicationUsers as u
                left join ApplicationUserGroups as ug on ug.UserId = u.Id
                left join ApplicationGroups as g on g.ID = ug.GroupId 
                where g.ID != @GruopId and u.isMember = @isMember");

            CreateStoredProcedure("GetUserIsAdmin", p => new {
                GruopId = p.Int()
            }, @"select u.* from ApplicationUsers as u
                left join ApplicationUserGroups as ug on ug.UserId = u.Id
                left join ApplicationGroups as g on g.ID = ug.GroupId 
                where g.ID = @GruopId");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetUserNotAdmin");
            DropStoredProcedure("dbo.GetUserIsAdmin");
        }
    }
}
