using AutoMapper;
using DoAnWebBanHang.Common;
using DoAnWebBanHang.Data;
using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.Service.Models;
using DoAnWebBanHang.WebApp.App_Start;
using DoAnWebBanHang.WebApp.Infastructure.Extensions;
using DoAnWebBanHang.WebApp.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DoAnWebBanHang.WebApp.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;
        ISystemConfigRepository _system;
        IUserService _userService;
        private ApplicationUserManager _userManager;
        private IUnitOfWork _unitOfWork;

        public HomeController(IProductCategoryService productCategoryService, ApplicationUserManager userManager,
            IUserService userService, 
            ISystemConfigRepository system,
            IProductService productService,
            ICommonService commonService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _system = system;
            _userService = userService;
            _productService = productService;
            _userManager = userManager;
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
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryModel);
        }

        [HttpGet]
        public ActionResult GetInformationUser()
        {
            var currentuser = _userService.getCurrentUserById(User.Identity.GetUserId());
            var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(currentuser);
            return View(applicationUserViewModel);
        }

        [HttpPost]
        public async Task<IdentityResult> UpdateInformation(ApplicationUserViewModel user)
        {
            var appUser = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            appUser.UpdateUser(user);
            //_userManager.ChangePassword(appUser.Result, appUser.Result.PasswordHash)
            return await _userManager.UpdateAsync(appUser);
        }

        [HttpGet]
        public ActionResult UpdatePasword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePasword(ChangePasswordModel user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(),user.CurrentPassword,user.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.Message = "Đổi mật khẩu thất bại";
                    return View(user);
                }
            }
            return View(user);
        }
    }
}