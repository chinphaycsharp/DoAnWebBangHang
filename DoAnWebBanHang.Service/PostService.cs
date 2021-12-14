using DoAnWebBanHang.Data.Infastructure;
using DoAnWebBanHang.Data.Repositories;
using DoAnWebBanHang.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Service
{
    public interface IPostService
    {
        void Add(Post post);

        void Update(Post post);

        void Delete(Post post);

        void Delete(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Post GetById(int id);

        IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);

        void SaveChanges();

    }

    public class PostService : IPostService
    {
        IPostRepository _postReposiotry;
        IUnitOfWork _unitOfWork;
        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            this._postReposiotry = postRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Add(Post post)
        {
            _postReposiotry.Add(post);
        }

        public void Delete(int id)
        {
            _postReposiotry.Delete(id);
        }

        public void Delete(Post post)
        {
            _postReposiotry.Delete(post);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postReposiotry.GetAll();
        }

        public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _postReposiotry.GetMultiPaging(x => x.Status == true && x.CategoryID == categoryId, out totalRow, page, pageSize, new string[] { "PostCategory"});
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {
            return _postReposiotry.getAllByTag(tag, page, pageSize, out totalRow);
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _postReposiotry.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Post GetById(int id)
        {
            return _postReposiotry.GetSingleById(id);
        }


        public void SaveChanges()
        {
            _unitOfWork.Commit(); 
        }

        public void Update(Post post)
        {
            _postReposiotry.Update(post);
        }
    }
}
