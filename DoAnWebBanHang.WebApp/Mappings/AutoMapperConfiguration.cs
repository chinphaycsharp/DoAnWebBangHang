using AutoMapper;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWebBanHang.WebApp.Mappings
{

    public class AutoMapperConfiguration
    {

        public static void config()
        {
            Mapper.CreateMap<Post, PostViewModel>();
            Mapper.CreateMap<PostCategory, PostCategoryViewModel>();
            Mapper.CreateMap<Tag, TagViewModel>();
            Mapper.CreateMap<Order, OrderViewModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryViewModel>();
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ProductTag, ProductTagViewModel>();
            Mapper.CreateMap<Footer, FooterViewModel>();
            Mapper.CreateMap<Slide, SlideViewModel>();
            Mapper.CreateMap<Page, PageViewModel>();
            Mapper.CreateMap<ContactDetail, ContactDetailViewModel>();

            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
            Mapper.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
        }
    }
}