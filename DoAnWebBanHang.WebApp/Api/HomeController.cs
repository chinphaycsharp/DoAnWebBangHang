using AutoMapper;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.WebApp.Infastructure.Core;
using DoAnWebBanHang.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoAnWebBanHang.WebApp.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
        private IProductService _productService;
        private IOrderService _orderService;
        private IUserService _userService;
        private IOrderDetailService _orderDetailService;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleGroupRepository _applicationRoleGroupRepository;

        public HomeController(IErrorService errorService, IProductService productService, IOrderService orderService, IUserService userService, IOrderDetailService orderDetailService ,IApplicationGroupService appGroupService
            , IApplicationRoleGroupRepository applicationRoleGroupRepository) : base(errorService)
        {
            this._errorService = errorService;
            this._productService = productService;
            this._orderService = orderService;
            this._userService = userService;
            this._orderDetailService = orderDetailService;
            this._appGroupService = appGroupService;
            this._applicationRoleGroupRepository = applicationRoleGroupRepository;
        }

        [Route("getallquantities")]
        [HttpGet]
        public HttpResponseMessage GetAllQuantities(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                List<int> quanties = new List<int>();
                var modelProduct = _productService.GetAll();
                var responseProduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(modelProduct);
                var quantityP = responseProduct.ToList().Count();
                quanties.Add(quantityP);

                var modelOrder = _orderService.GetAll();
                var responseOrder = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(modelOrder);
                var quantityO = responseOrder.ToList().Count();
                quanties.Add(quantityO);

                var modelCustomer = _userService.GetAll();
                var responseCustomer = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(modelCustomer);
                var quantityC = responseCustomer.ToList().Count();
                quanties.Add(quantityC);

                var modelOrderDetail = _orderDetailService.GetTotalPriceOrder();
                //var responseOrderDetail = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(modelCustomer);
                var quantityOD = Convert.ToInt32(modelOrderDetail);
                quanties.Add(quantityOD);

                var response = request.CreateResponse(System.Net.HttpStatusCode.OK, quanties);
                return response;
            };
            return CreateHttpResponse(request, func);
        }


        [HttpGet]
        [Route("getcurrentuser")]
        public HttpResponseMessage getCurrentUser(HttpRequestMessage request, string userName)
        {
            return CreateHttpResponse(request, () =>
            {
                var user = _userService.getCurrentUser(userName);
                var gruops = _appGroupService.GetListGroupByUserId(user.Id);
                IEnumerable <ApplicationGroup>  a = gruops.ToList();
                IEnumerable<ApplicationRole> roles = null;
                foreach (var item in a)
                {
                    roles = _applicationRoleGroupRepository.getRolesUser(item.ID);
                }                
                var response = request.CreateResponse(HttpStatusCode.OK, roles);
                return response;
            });
        }
    }
}