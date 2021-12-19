using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);

    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            var result = from or in DbContext.OrderDetails where or.OrderID == orderId select or;
            return result;
        }
    }
}
