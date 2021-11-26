using DoAnWebBanHang.Common.ViewModels;
using DoAnWebBanHang.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(int fromDate, int toDate);

    }
    public class StatisticService : IStatisticService
    {
        IOrderRepository _orderRepository;
        public StatisticService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(int fromDate, int toDate)
        {
            return _orderRepository.GetRevenueStatistic(fromDate, toDate);
        }
    }
}
