using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IApplicationGroupRepository : IRepository<ApplicationGroup>
    {
        IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId);
        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId);
        IEnumerable<ApplicationUser> GetListUserByGroupIdNotAdmin(int groupId);
        ApplicationGroup GetGroupByName(string groupName);

    }
    public class ApplicationGroupRepository : RepositoryBase<ApplicationGroup>, IApplicationGroupRepository
    {
        public ApplicationGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public ApplicationGroup GetGroupByName(string groupName)
        {
            var query = DbContext.ApplicationGroups.Where(x => x.Name == groupName).Single();
            return query;
        }

        public IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.ID equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query;
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int GruopId)
        {
            var parameter = new SqlParameter[]
            {
                new SqlParameter("@GruopId",GruopId)
            };

            return DbContext.Database.SqlQuery<ApplicationUser>("GetUserIsAdmin @GroupId", parameter);
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupIdNotAdmin(int GruopId)
        {
            var parameter = new SqlParameter[]
            {
                new SqlParameter("@GruopId",GruopId),
                new SqlParameter("@isMember",true),
            };

            return DbContext.Database.SqlQuery<ApplicationUser>("GetUserNotAdmin @GroupId,@isMember", parameter);
        }
    }
}
