using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> getAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Post> getPostByCateId(int CateId, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> getAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags
                        on p.ID equals pt.PostID
                        where pt.TagID == tag && p.Status == true
                        orderby p.CreatedDate descending
                        select p;
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Post> getPostByCateId(int CateId, int pageIndex, int pageSize, out int totalRow)
        {
            if(CateId == 0)
            {
                
                var query = from p in DbContext.Posts
                            orderby p.CreatedDate descending
                            select p;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                var query = from p in DbContext.Posts
                            where p.CategoryID == CateId
                            orderby p.CreatedDate descending
                            select p;
                totalRow = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }
    }
}
