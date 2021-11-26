using AutoMapper;
using DoAnWebBanHang.Common;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanHang.WebApp.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;
        ISystemConfigRepository _system;

        public HomeController(IProductCategoryService productCategoryService,
            ISystemConfigRepository system,
            IProductService productService,
            ICommonService commonService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _system = system;
            _productService = productService;
        }
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var homeViewModel = new HomeViewModel();
            homeViewModel.Slides = slideView;

            var lastestProductModel = _productService.GetLastest(3);
            var topSaleProductModel = _productService.GetHotProduct(3);
            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);
            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.TopSaleProducts = topSaleProductViewModel;

            try
            {
                homeViewModel.Title = _commonService.getSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.getSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.getSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch
            {

            }

            return View(homeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryModel);
        }
    }
}