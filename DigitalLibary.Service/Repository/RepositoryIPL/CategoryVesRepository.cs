using AutoMapper;
using DigitalLibary.Data.Data;
using DigitalLibary.Data.Entity;
using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using DigitalLibary.Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalLibary.Service.Repository.RepositoryIPL
{
    public class CategoryVesRepository: ICategoryVesRepository
    {
        #region Variables
        private readonly IMapper _mapper;
        public DataContext _DbContext;
        #endregion

        #region Constructors
        public CategoryVesRepository(DataContext DbContext, IMapper mapper)
        {
            _DbContext = DbContext;
            _mapper = mapper;
        }
        #endregion

        #region METHOD
        public IEnumerable<CategoryVesDto> GetAllCategoryVesByELecture(int pageNumber, int pageSize)
        {
            var CategoryVess = new List<CategoryVes>();
            CategoryVess = _DbContext.CategoryVes.Where(e => e.Status == 1).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                CategoryVess = CategoryVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<CategoryVesDto>();
            result = _mapper.Map<List<CategoryVesDto>>(CategoryVess);

            return result;
        }
        public IEnumerable<CategoryVesDto> GetAllCategoryVesByVideo(int pageNumber, int pageSize)
        {
            var CategoryVess = new List<CategoryVes>();
            CategoryVess = _DbContext.CategoryVes.Where(e => e.Status == 2).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                CategoryVess = CategoryVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<CategoryVesDto>();
            result = _mapper.Map<List<CategoryVesDto>>(CategoryVess);

            return result;
        }
        public IEnumerable<CategoryVesDto> GetAllCategoryVesBySound(int pageNumber, int pageSize)
        {
            var CategoryVess = new List<CategoryVes>();
            CategoryVess = _DbContext.CategoryVes.Where(e => e.Status == 3).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                CategoryVess = CategoryVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<CategoryVesDto>();
            result = _mapper.Map<List<CategoryVesDto>>(CategoryVess);

            return result;
        }
        public Response DeleteCategoryVesByList(List<Guid> IdCategoryVes)
        {
            var CategoryVess = _DbContext.CategoryVes.Where(ar => IdCategoryVes.Contains(ar.Id)).ToList();
            if (!CategoryVess.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần xóa !"
                };
            }
            var groupVes = _DbContext.GroupVes.Where(ar => IdCategoryVes.Contains((Guid)ar.IdcategoryVes)).ToList();
            if (groupVes.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Danh mục đã có nhóm không thể xóa !"
                };
            }

            _DbContext.CategoryVes.RemoveRange(CategoryVess);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Xóa thành công !" };
        }

        public IEnumerable<CategoryVesDto> GetAllCategoryVes(int pageNumber, int pageSize)
        {
            var CategoryVess = new List<CategoryVes>();
            CategoryVess = _DbContext.CategoryVes.OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                CategoryVess = CategoryVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<CategoryVesDto>();
            result = _mapper.Map<List<CategoryVesDto>>(CategoryVess);

            return result;
        }

        public IEnumerable<CategoryVesDto> GetAllCategoryVesAvailable(int pageNumber, int pageSize)
        {
            var CategoryVess = new List<CategoryVes>();
            CategoryVess = _DbContext.CategoryVes.Where(e => e.IsHide == false).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                CategoryVess = CategoryVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<CategoryVesDto>();
            result = _mapper.Map<List<CategoryVesDto>>(CategoryVess);

            return result;
        }

        public CategoryVesDto GetAllCategoryVesById(Guid IdCategoryVes)
        {
            var CategoryVess = new CategoryVes();
            CategoryVess = _DbContext.CategoryVes.Where(e => e.Id == IdCategoryVes).FirstOrDefault();

            var result = new CategoryVesDto();
            result = _mapper.Map<CategoryVesDto>(CategoryVess);

            return result;
        }

        public Response HideCategoryVesByList(List<Guid> IdCategoryVes, bool IsHide)
        {
            var CategoryVess = _DbContext.CategoryVes.Where(ar => IdCategoryVes.Contains(ar.Id)).ToList();
            if (!CategoryVess.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần chỉnh sửa !"
                };
            }

            var groupVes = _DbContext.GroupVes.Where(ar => IdCategoryVes.Contains((Guid)ar.IdcategoryVes)).ToList();
            if (groupVes.Any() && IsHide)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Danh mục đã có nhóm không thể ẩn !"
                };
            }

            CategoryVess.ForEach(category => category.IsHide = IsHide);

            _DbContext.CategoryVes.UpdateRange(CategoryVess);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = $"{(IsHide ? "Khóa" : "Hủy khóa")} thành công !" };
        }

        public Response InsertCategoryVes(CategoryVesDto CategoryVesDto)
        {
            var CategoryVes = _mapper.Map<CategoryVes>(CategoryVesDto);

            _DbContext.CategoryVes.Add(CategoryVes);
            _DbContext.SaveChanges();

            return new Response()
            {
                Success = true,
                Fail = false,
                Message = "Thêm mới thành công !"
            };
        }

        public Response UpdateCategoryVes(Guid IdCategoryVes, CategoryVesDto CategoryVesDto)
        {
            var CategoryVes = _DbContext.CategoryVes.Find(IdCategoryVes);
            if (CategoryVes == null)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy thông tin !"
                };
            }
            CategoryVes.CategoryVesName = CategoryVesDto.CategoryVesName;
            CategoryVes.Status = CategoryVesDto.Status;

            _DbContext.CategoryVes.Update(CategoryVes);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Cập nhật thành công !" };
        }
        #endregion
    }
}
