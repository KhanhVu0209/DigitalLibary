using AutoMapper;
using DigitalLibary.Data.Data;
using DigitalLibary.Data.Entity;
using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using DigitalLibary.Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitalLibary.Service.Repository.RepositoryIPL
{
    public class GroupVesRepository: IGroupVesRepository
    {
        #region Variables
        private readonly IMapper _mapper;
        public DataContext _DbContext;
        #endregion

        #region Constructors
        public GroupVesRepository(DataContext DbContext, IMapper mapper)
        {
            _DbContext = DbContext;
            _mapper = mapper;
        }
        #endregion

        #region METHOD
        public Response DeleteGroupVesByList(List<Guid> IdGroupVes)
        {
            var GroupVess = _DbContext.GroupVes.Where(ar => IdGroupVes.Contains(ar.Id)).ToList();
            if (!GroupVess.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần xóa !"
                };
            }
            var ves = _DbContext.VES.Where(ar => IdGroupVes.Contains((Guid)ar.IdGroupVes)).ToList();
            if (ves.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Nhóm đã có dữ liệu không thể xóa !"
                };
            }

            _DbContext.GroupVes.RemoveRange(GroupVess);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Xóa thành công !" };
        }

        public IEnumerable<GroupVesDto> GetAllGroupVes(int pageNumber, int pageSize)
        {
            var GroupVess = new List<GroupVes>();
            GroupVess = _DbContext.GroupVes.OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                GroupVess = GroupVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<GroupVesDto>();
            result = _mapper.Map<List<GroupVesDto>>(GroupVess);

            return result;
        }

        public IEnumerable<GroupVesDto> GetAllGroupVesByIdcategoryVes(int pageNumber, int pageSize, Guid IdcategoryVes)
        {
            var GroupVess = new List<GroupVes>();
            GroupVess = _DbContext.GroupVes.Where(e => e.IdcategoryVes == IdcategoryVes && e.IsHide == false).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                GroupVess = GroupVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<GroupVesDto>();
            result = _mapper.Map<List<GroupVesDto>>(GroupVess);

            return result;
        }

        public IEnumerable<GroupVesDto> GetAllGroupVesAvailable(int pageNumber, int pageSize)
        {
            var GroupVess = new List<GroupVes>();
            GroupVess = _DbContext.GroupVes.Where(e => e.IsHide == false).OrderByDescending(e => e.CreatedDate).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                GroupVess = GroupVess.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<GroupVesDto>();
            result = _mapper.Map<List<GroupVesDto>>(GroupVess);

            return result;
        }

        public GroupVesDto GetAllGroupVesById(Guid IdGroupVes)
        {
            var GroupVess = new GroupVes();
            GroupVess = _DbContext.GroupVes.Where(e => e.Id == IdGroupVes).FirstOrDefault();

            var result = new GroupVesDto();
            result = _mapper.Map<GroupVesDto>(GroupVess);

            return result;
        }

        public Response HideGroupVesByList(List<Guid> IdGroupVes, bool IsHide)
        {
            var GroupVess = _DbContext.GroupVes.Where(ar => IdGroupVes.Contains(ar.Id)).ToList();
            if (!GroupVess.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần chỉnh sửa !"
                };
            }

            var ves = _DbContext.VES.Where(ar => IdGroupVes.Contains((Guid)ar.IdGroupVes)).ToList();
            if (ves.Any() && IsHide)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Nhóm đã có dữ liệu không thể ẩn !"
                };
            }

            GroupVess.ForEach(category => category.IsHide = IsHide);

            _DbContext.GroupVes.UpdateRange(GroupVess);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = $"{(IsHide ? "Khóa" : "Hủy khóa")} thành công !" };
        }

        public Response InsertGroupVes(GroupVesDto GroupVesDto)
        {
            var GroupVes = _mapper.Map<GroupVes>(GroupVesDto);

            _DbContext.GroupVes.Add(GroupVes);
            _DbContext.SaveChanges();

            return new Response()
            {
                Success = true,
                Fail = false,
                Message = "Thêm mới thành công !"
            };
        }

        public Response UpdateGroupVes(Guid IdGroupVes, GroupVesDto GroupVesDto)
        {
            var GroupVes = _DbContext.GroupVes.Find(IdGroupVes);
            if (GroupVes == null)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy thông tin !"
                };
            }
            GroupVes.GroupName = GroupVesDto.GroupName;
            GroupVes.IdcategoryVes = GroupVesDto.IdcategoryVes;

            _DbContext.GroupVes.Update(GroupVes);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Cập nhật thành công !" };
        }
        #endregion
    }
}
