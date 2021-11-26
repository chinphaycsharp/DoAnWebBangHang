using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class TopUserOrdersViewModel
    {
        public string CustomerName { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal Price { get; set; }
    }
}