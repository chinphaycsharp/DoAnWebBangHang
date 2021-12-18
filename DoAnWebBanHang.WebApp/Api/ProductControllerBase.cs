using System.Web.Http;

namespace DoAnWebBanHang.WebApp.Api
{
    [Authorize, RoutePrefix("api/product")]
    public class ProductControllerBase
    {
    }
}