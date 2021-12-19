using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class ListOrderDetail
    {
        public string CustomerName { set; get; }
        public string CustomerAddress { set; get; }
        public string CustomerEmail { set; get; }
        public string CustomerMobile { set; get; }
        public string PaymentMethod { set; get; }
        public decimal TotalPrice { get; set; }
        public DateTime? date { get; set; }
        public List<OrderDetailViewModel> orderDetailViewModels { get; set; }
    }
}