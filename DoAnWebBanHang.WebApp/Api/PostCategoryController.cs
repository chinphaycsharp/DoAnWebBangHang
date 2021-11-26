using AutoMapper;
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
    [RoutePrefix("api/postcategory")]
    [Authorize]
    public class PostCategoryController : ApiControllerBase
    {
        IPostCategoryService _postCategoryService;

        public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService) :
            base(errorService)
        {
            this._postCategoryService = postCategoryService;
        }


        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
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
            //return CreateHttpResponse(request, () =>
            //{
            //    var listCategory = _postCategoryService.GetAll();

            //    var listPostCategoryVm = Mapper.Map<List<PostCategoryViewModel>>(listCategory);

            //    HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listPostCategoryVm);

            //    return response;
            //});
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, PostCategoryViewModel postCategory)
        {
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