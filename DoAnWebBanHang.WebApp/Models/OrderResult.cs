using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class OrderResult
    {
        public bool status { get; set; }
        public string price { get; set; }
        public string code { get; set; }
        public string method { get; set; }
        public DateTime? time { get; set; }
    }
}