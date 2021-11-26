using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service.Models
{
    public class UserOrderHistoryByCustomer
    {
        public DateTime? datePurchase { get; set; }
        public string datePurchaseString { get; set; }
        public string Image { set; get; }
        public string Name { set; get; }
        public int Quantity { set; get; }
        public string PaymentMethod { get; set; }
        public bool status { set; get; }
        public decimal Price { set; get; }
        public string CustomerId { get; set; }
        public string PaymentStatus { get; set; }
    }
}
