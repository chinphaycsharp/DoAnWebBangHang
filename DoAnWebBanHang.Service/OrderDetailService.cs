using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service
{
    public interface IOrderDetailService
    {
        IEnumerable<OrderDetail> GetAll();
        decimal GetTotalPriceOrder();
        IEnumerable<UserOrderHistoryByCustomer> GetUserOrderHistoryByUsers(string customerId, string value ,int page, int pageSize, out int totalRow);
        
    }
    public class OrderDetailService : IOrderDetailService
    {
        IOrderDetailRepository _orderDetailRepository;
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IUnitOfWork _unitOfWork;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetailRepository.GetAll();
        }

        public decimal GetTotalPriceOrder()
        {
            return _orderDetailRepository.GetAll().Sum(x => x.Price);
        }

        public IEnumerable<UserOrderHistoryByCustomer> GetUserOrderHistoryByUsers(string customerId, string value ,int page, int pageSize, out int totalRow)
        {
            var order = _orderRepository.GetAll();
            var orderDetail = _orderDetailRepository.GetAll();
            var product = _productRepository.GetAll();
            var result = from o in order
                         join od in orderDetail
                         on o.ID equals od.OrderID
                         join p in product
                         on od.ProductID equals p.ID
                         where o.CustomerId == customerId
                         select new UserOrderHistoryByCustomer
                         {
                             datePurchaseString = Convert.ToDateTime(o.CreatedDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                             Image = p.Image,
                             Name= p.Name,
                             Quantity = od.Quantity,
                             PaymentMethod = o.PaymentMethod,
                             PaymentStatus = o.Status == true ? "Đã giao xong" : "Đang giao",
                             Price = od.Price,
                         };
            if(value != null)
            {

                result = result.Where(x => x.Name.Contains(value));
            }
            
            totalRow = result.Count();
            return result.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
