using AutoMapper;
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
    public class PageController : Controller
    {
        // GET: Page
        IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        // GET: Page
        public ActionResult Index(string alias)
        {
            var page = _pageService.GetByAlias(alias);
            var model = Mapper.Map<Page, PageViewModel>(page);
            return View(model);
        }
    }
}