using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Infastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WebShopDbContext dbContext;
        public WebShopDbContext init()
        {
            //Dependency injection
            return dbContext ?? (dbContext = new WebShopDbContext());
        }

        protected override void DisposeCore()
        {
            if(dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
