using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Models
{
    public class HomeViewModel
    {
        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }

        public IEnumerable<SlideViewModel> Slides { set; get; }
        public IEnumerable<ProductViewModel> LastestProducts { set; get; }
        public IEnumerable<ProductViewModel> TopSaleProducts { set; get; }
    }
}