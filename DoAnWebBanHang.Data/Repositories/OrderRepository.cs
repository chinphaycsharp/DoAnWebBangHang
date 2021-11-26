using DoAnWebBanHang.Common.ViewModels;
using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(int fromDate, int toDate);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(int fromDate, int toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistic @fromDate,@toDate", parameters);
        }
    }
}
