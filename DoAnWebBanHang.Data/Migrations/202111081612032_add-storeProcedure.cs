namespace DoAnWebBanHang.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstoreProcedure : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetRevenueStatistic",p=>new {
                toDate = p.Int(),
                fromDate = p.Int()
            }, @"select
DATEPART(mm, o.CreatedDate) as month,
sum(od.Quantity*od.Price) as Revenues,
sum((od.Quantity*od.Price)-(od.Quantity*p.OriginalPrice)) as Benefit
from Orders o
inner join OrderDetails od
on o.ID = od.OrderId
inner join Products p
on od.ProductID  = p.ID
where DATEPART(mm,o.CreatedDate) <= @toDate and DATEPART(mm,o.CreatedDate) >= @fromDate
group by DATEPART(mm,o.CreatedDate)");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevenueStatistic");
        }
    }
}
