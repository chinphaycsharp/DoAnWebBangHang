using AutoMapper;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.WebApp.Infastructure.Core;
using DoAnWebBanHang.WebApp.Infastructure.Extensions;
using DoAnWebBanHang.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoAnWebBanHang.WebApp.Api
{
    [RoutePrefix("api/postcategories")]
    [Authorize]
    public class PostCategoryController : ApiControllerBase
    {
        IPostCategoryService _postCategoryService;

        public PostCategoryController(IErrorService errorService, IApplicationRoleRepository applicationRoleRepository, IApplicationGroupRepository applicationGroupRepository, IPostCategoryService postCategoryService) :
             base(errorService, applicationRoleRepository, applicationGroupRepository)
        {
            this._postCategoryService = postCategoryService;
        }


        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            //if(!checkAdmin())
            //{
            //    return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập"); 
            //}
            return CreateHttpResponse(request, () =>
            {
                var model = _postCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request, string keyWord, int page, int pageSize = 20)
        {
            //if (!checkAdmin())
            //{
            //    return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập");
            //}
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _postCategoryService.GetAll(keyWord);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<PostCategoryViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            //if (!checkAdmin())
            //{
            //    return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập");
            //}
            return CreateHttpResponse(request, () =>
            {
                var model = _postCategoryService.GetById(id);

                var responseData = Mapper.Map<PostCategory, PostCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, PostCategoryViewModel postCategory)
        {
            //if (!checkAdmin())
            //{
            //    return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập");
            //}
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newPostCategory = new PostCategory();
                    newPostCategory.UpdatePostCategory(postCategory);
                    newPostCategory.CreatedDate = DateTime.Now;
                    _postCategoryService.Add(newPostCategory);
                    _postCategoryService.Save();

                    respone = request.CreateResponse(HttpStatusCode.Created, postCategory);
                }
                return respone;
            });
        }

        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, PostCategory postCategory)
        {
            //if (!checkAdmin(""))
            //{
            //    return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập");
            //}
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (ModelState.IsValid)
                {
                    respone = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _postCategoryService.Update(postCategory);
                    _postCategoryService.Save();

                    respone = request.CreateResponse(HttpStatusCode.OK, postCategory);
                }
                return respone;
            });
        }

        [Route("remove")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            if (!checkAdmin("ViewProductCategory"))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "Bạn không có quyền truy cập");
            }
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (ModelState.IsValid)
                {
                    respone = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _postCategoryService.Delete(id);
                    _postCategoryService.Save();

                    respone = request.CreateResponse(HttpStatusCode.OK);
                }
                return respone;
            });
        }
    }
}