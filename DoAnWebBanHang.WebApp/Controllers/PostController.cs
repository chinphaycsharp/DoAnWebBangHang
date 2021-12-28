using AutoMapper;
using DoAnWebBanHang.Common;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.WebApp.Infastructure.Core;
using DoAnWebBanHang.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanHang.WebApp.Controllers
{
    public class PostController : Controller
    {
        IPostService _postService;
        IPostCategoryService _postCategoryService;
        public PostController( IPostService postService, IPostCategoryService postCategoryService)
        {
            this._postService = postService;
            this._postCategoryService = postCategoryService;
        }
        // GET: Post
        public ActionResult Index(int CateId = 0,int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var postModel = _postService.GetAllByCategoryPaging(CateId,page,pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.postCategory = _postCategoryService.GetAll();
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult Detail(int id)
        {
            var postModel = _postService.GetById(id);
            var viewModel = Mapper.Map<Post, PostViewModel>(postModel);
            return View(viewModel);
        }
    }
}