﻿using DoAnWebBanHang.Common;
using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service.Models;
using DoAnWebBanHang.WebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetAll();
        void Save();
        IEnumerable<ApplicationUser> GetAll(string keyword);
        void UpDate(ApplicationUser user);

        IEnumerable<TopUserOrdersViewModel> GetTopUserOrders();

        ApplicationUser getCurrentUser(string username);
        ApplicationUser getCurrentUserById(string id);

        IEnumerable<ApplicationUser> GetUserNotAdmin();

        IEnumerable<ApplicationUser> GetUserIsAdmin();

    }

    public class UserService : IUserService
    {
        private IApplicationUserRepository _applicationUserRepository;
        private IApplicationGroupRepository _applicationGruopRepository;
        private IOrderDetailService _orderDetailService;
        private IOrderService _orderService;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _applicationUser;

        public UserService( IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository, IOrderDetailService orderDetailService, IOrderService orderService, IApplicationUserRepository applicationUser, IApplicationGroupRepository applicationGroupRepository)
        {
            this._applicationUserRepository = applicationUserRepository;
            this._applicationGruopRepository = applicationGroupRepository;
            this._unitOfWork = unitOfWork;
            this._orderDetailService = orderDetailService;
            this._orderService = orderService;
            this._applicationUser = applicationUser;
        }


        public IEnumerable<ApplicationUser> GetAll()
        {
            return _applicationUserRepository.GetAll();
        }

        public IEnumerable<ApplicationUser> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _applicationUserRepository.GetMulti(x => x.FullName.Contains(keyword) || x.Email.Contains(keyword));
            else
                return _applicationUserRepository.GetAll();
        }

        public ApplicationUser getCurrentUser(string username)
        {
            var users = _applicationUserRepository.GetAll();
            return users.Single(x => x.UserName.Equals(username) );
        }

        public ApplicationUser getCurrentUserById(string id)
        {
            var users = _applicationUserRepository.GetAll();
            return users.Single(x => x.Id == id);
        }

        public IEnumerable<TopUserOrdersViewModel> GetTopUserOrders()
        {
            var users = _applicationUserRepository.GetAll();
            var orders = _orderService.GetAll();
            var orderDetails = _orderDetailService.GetAll();

            IEnumerable<TopUserOrdersViewModel> result = from u in users
                         join o in orders on u.Id equals o.CustomerId
                         join od in orderDetails on o.ID equals od.OrderID
                         orderby od.Price descending
                         select new TopUserOrdersViewModel
                         {
                             CustomerName = u.FullName,
                             CreateDate = o.CreatedDate,
                             Price = od.Price
                         };
            return result;
        }

        public IEnumerable<ApplicationUser> GetUserIsAdmin()
        {
            var adminGroup = _applicationGruopRepository.GetGroupByName(CommonConstants.Admin);
            var userNotAdmin = _applicationGruopRepository.GetListUserByGroupId(adminGroup.ID);
            return userNotAdmin;
        }

        public IEnumerable<ApplicationUser> GetUserNotAdmin()
        {
            var adminGroup = _applicationGruopRepository.GetGroupByName(CommonConstants.Admin);
            var userNotAdmin = _applicationGruopRepository.GetListUserByGroupIdNotAdmin(adminGroup.ID);
            return userNotAdmin;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpDate(ApplicationUser user)
        {
            _applicationUser.Update(user);
        }
    }
}
