using DoAnWebBanHang.Common.ViewModels;
using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetAll(string keyword);
        bool Create(Order order, List<OrderDetail> orderDetails);
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistics(int toDate, int fromDate);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }
        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
            else
                return _orderRepository.GetAll();
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistics(int toDate, int fromDate)
        {
            return _orderRepository.GetRevenueStatistic(toDate, fromDate);
        }
    }
}
