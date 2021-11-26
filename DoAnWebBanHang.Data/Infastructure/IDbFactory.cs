using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Infastructure
{
    public interface IDbFactory : IDisposable
    {
        //Tạo DbContext
        WebShopDbContext init();
    }
}
