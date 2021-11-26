using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Infastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly IDbFactory dbFactory;
        private WebShopDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public WebShopDbContext DbContext
        {
            get
            {
                return dbContext ?? (dbContext = dbFactory.init());
            }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
