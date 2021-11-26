using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Repositories
{
    public interface IPostCategoryRepository : IRepository<PostCategory>
    {
        
    }

    public class PostCategoryRepository : RepositoryBase<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


    }
}
