using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IApplicationRoleGroupRepository : IRepository<ApplicationRoleGroup>
    {
        IEnumerable<ApplicationRole> getRolesUser(int groupId);
    }
    public class ApplicationRoleGroupRepository : RepositoryBase<ApplicationRoleGroup>, IApplicationRoleGroupRepository
    {
        public ApplicationRoleGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<ApplicationRole> getRolesUser(int groupId)
        {
            var query = from g in DbContext.ApplicationRoles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;

            return query;
        }
    }
}
