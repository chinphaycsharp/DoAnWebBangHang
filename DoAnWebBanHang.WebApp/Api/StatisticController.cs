using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Service;
using DoAnWebBanHang.WebApp.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DoAnWebBanHang.WebApp.Api
{
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        IStatisticService _statisticService;
        public StatisticController(IErrorService errorService, IApplicationRoleRepository applicationRoleRepository, IApplicationGroupRepository applicationGroupRepository, IStatisticService statisticService) : base(errorService, applicationRoleRepository, applicationGroupRepository)
        {
            _statisticService = statisticService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, int fromDate, int toDate)
        {

            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

    }
}