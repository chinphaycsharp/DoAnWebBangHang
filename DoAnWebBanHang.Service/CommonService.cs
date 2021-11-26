using DoAnWebBanHang.Common;
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
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlides();
        SystemConfig getSystemConfig(string code);
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        IUnitOfWork _unitOfWork;
        ISystemConfigRepository _system;
        ISlideRepository _slideRepository;

        public CommonService(IFooterRepository footerRepository, IUnitOfWork unitOfWork, ISlideRepository slideRepository, ISystemConfigRepository system)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _system = system;
            _slideRepository = slideRepository;
        }
        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status);
        }

        public SystemConfig getSystemConfig(string code)
        {
            return _system.GetSingleByCondition(x=>x.Code == code);
        }
    }
}
