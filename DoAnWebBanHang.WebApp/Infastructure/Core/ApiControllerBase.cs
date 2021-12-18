using DoAnWebBanHang.Model.Models;
using DoAnWebBanHang.Service;
using System;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DoAnWebBanHang.Common;
using DoAnWebBanHang.Data.Repositories;

namespace DoAnWebBanHang.WebApp.Infastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        private IErrorService _errorService;
        private IApplicationGroupRepository _applicationGruopRepository;
        private IApplicationRoleRepository _applicationRoleRepository;
        public ApiControllerBase(IErrorService errorService, IApplicationRoleRepository applicationRoleRepository, IApplicationGroupRepository applicationGroupRepository)
        {
            this._errorService = errorService;
            this._applicationGruopRepository = applicationGroupRepository;
            this._applicationRoleRepository = applicationRoleRepository;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;
            try
            {
                response = function.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                LogError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (DbUpdateException dbEx)
            {
                LogError(dbEx);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, dbEx.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return response;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch
            {
            }
        }

        protected bool checkAdmin(string role)
        {
            var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
            var listGroup = applicationGroupService.GetListGroupByUserId(User.Identity.GetUserId());
            foreach (var item in listGroup)
            {
                var groups = _applicationGruopRepository.GetGroupByName(item.Name);
                var roles = _applicationRoleRepository.GetListRoleByGroupId(groups.ID);
                if (roles.Any(x => x.Name == role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}