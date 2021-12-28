using DoAnWebBanHang.Common;
using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser FindUser(string userName, string password);
    }
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public ApplicationUser FindUser(string userName, string password)
        {
            var p = StringHelper.ConvertStringtoMD5(password);
            var query = from u in DbContext.Users
                        where u.UserName == userName && u.PasswordHash == p
                        select u;
            
            return query.FirstOrDefault();
        }
    }
}
