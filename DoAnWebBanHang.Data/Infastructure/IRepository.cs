using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWebBanHang.Data.Infastructure
{
    //interface generic có thể sử dụng tất cả các lớp trong dự án
    public interface IRepository<T> where T : class
    {
        //Tạo mới bản ghi
        T Add(T entity);

        //Chỉnh sửa bản ghi
        void Update(T entity);

        //Xóa bản ghi
        T Delete(T entity);

        T Delete(int id);

        //Xóa nhiều bản ghi
        void DeleteMulti(Expression<Func<T, bool>> where);

        //Lấy theo id
        T GetSingleById(int id);

        //Lấy theo điều kiện
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        //Lấy tất cả
        IEnumerable<T> GetAll(string[] includes = null);

        //Lấy nhiều bản ghi
        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        //Lấy nhiều bản ghi có phân trang
        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        //Đếm bản ghi
        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
    }
}
