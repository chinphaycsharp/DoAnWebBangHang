using AutoMapper;
using DoAnWebBanHang.Common;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.Service.Models;
using DoAnWebBanHang.WebApp.App_Start;
using DoAnWebBanHang.WebApp.Infastructure.Core;
using DoAnWebBanHang.WebApp.Infastructure.Extensions;
using DoAnWebBanHang.WebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DoAnWebBanHang.WebApp.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {

        IProductService _productService;
        ApplicationUserManager _userManager;
        IOrderService _orderService;
        IOrderDetailService _OrderDetailService;
        public ShoppingCartController(IProductService productService, ApplicationUserManager userManager,IOrderService orderService,IOrderDetailService OrderDetailService)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
            this._OrderDetailService = OrderDetailService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);

            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            decimal price = 0;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price += item.Product.Price * item.Quantity;
                orderDetails.Add(detail);
                price += detail.Price;
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }
            if(isEnough)
            {
                _orderService.Create(orderNew, orderDetails);
                _productService.Save();
                string paymentMethod = "";
                string mess = "Thông báo thanh toán";
                if (order.PaymentMethod == "ATM_ONLINE")
                {
                    paymentMethod = "Bạn đã đăng ký thanh toán qua thẻ ngân hàng";
                    string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/pay.html"));
                    content = content.Replace("{{Name}}", order.CustomerName);
                    content = content.Replace("{{Payment}}", paymentMethod);
                    content = content.Replace("{{NumberCreditCash}}", order.BankNumber);
                    content = content.Replace("{{BankName}}", order.BankName);
                    content = content.Replace("{{Message}}", mess);
                    var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                    content = content.Replace("{{adminEmail}}", adminEmail);
                    content = content.Replace("{{Price}}", price.ToString());
                    MailHelper.SendMail(order.CustomerEmail, "Thanh toán đơn hàng", content);
                }
                else
                {
                    paymentMethod = "Bạn đã đăng ký thanh toán qua tiền mặt. Chúng tôi sẽ liên hệ bên thứ ba để vận chuyển";
                    string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/pay2.html"));
                    content = content.Replace("{{Name}}", order.CustomerName);
                    content = content.Replace("{{Price}}", price.ToString());
                    content = content.Replace("{{Payment}}", paymentMethod);
                    content = content.Replace("{{Message}}", mess);
                    var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                    content = content.Replace("{{adminEmail}}", adminEmail);
                    MailHelper.SendMail(order.CustomerEmail, "Thanh toán đơn hàng", content);
                }


                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng"
                });
            }
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        [Route("OrderHistory/{customerId}")]
        public ActionResult OrderHistory(string customerId, string SearchString, int page = 1)
        {
            int pageSize = 3;
            int totalRow = 0;
            var result = _OrderDetailService.GetUserOrderHistoryByUsers(customerId, SearchString, page,pageSize, out totalRow);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var a = result.ToList();
            var paginationSet = new PaginationSet<UserOrderHistoryByCustomer>()
            {
                Items = result,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            ViewBag.customerId = customerId;
            return View(paginationSet);
        }
    }
}