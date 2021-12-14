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
        Order Delete(int id);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAll(string keyword);
        Order GetById(int id);
        IEnumerable<Order> GetListOrder(string keyword);
        IEnumerable<OrderDetail> GetListOrderDetails();
        bool Create(Order order, List<OrderDetail> orderDetails);
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistics(int toDate, int fromDate);
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int id);
        void Save();
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

        public Order Delete(int id)
        {
            var orderDetails = _orderDetailRepository.GetOrderDetailsByOrderId(id);
            foreach (var item in orderDetails.ToList())
            {
                _orderDetailRepository.Delete(item);
            }
            return _orderRepository.Delete(id);
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

        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public IEnumerable<Order> GetListOrder(string keyword)
        {
            IEnumerable<Order> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword));
            else
                query = _orderRepository.GetAll();
            return query;
        }

        public IEnumerable<OrderDetail> GetListOrderDetails()
        {
            return _orderDetailRepository.GetAll();
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int id)
        {
            return _orderDetailRepository.GetOrderDetailsByOrderId(id);
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistics(int toDate, int fromDate)
        {
            return _orderRepository.GetRevenueStatistic(toDate, fromDate);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
