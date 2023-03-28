using AutoMapper;
using DigitalLibary.Data.Data;
using DigitalLibary.Data.Entity;
using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using DigitalLibary.Service.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitalLibary.Service.Repository.RepositoryIPL
{
    public class VESRepository : IVESRepository
    {
        #region Variables
        private readonly IMapper _mapper;
        public DataContext _DbContext;
        #endregion

        #region Constructors
        public VESRepository(DataContext DbContext, IMapper mapper)
        {
            _DbContext = DbContext;
            _mapper = mapper;
        }
        #endregion

        #region METHOD
        public Response DeleteVESByList(List<Guid> IdVES)
        {
            var VESs = _DbContext.VES.Where(ar => IdVES.Contains(ar.Id)).ToList();
            if (!VESs.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần xóa !"
                };
            }

            _DbContext.VES.RemoveRange(VESs);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Xóa thành công !" };
        }

        public IEnumerable<VESDto> GetAllVES(int pageNumber, int pageSize)
        {
            var VESs = new List<VES>();
            VESs = _DbContext.VES.OrderBy(e => e.Number).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                VESs = VESs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<VESDto>();
            result = _mapper.Map<List<VESDto>>(VESs);

            return result;
        }

        public IEnumerable<VESDto> GetAllVESByMediaType(int pageNumber, int pageSize, int[] mediaType)
        {
            var vesQuery = _DbContext.VES
            .Where(e => e.MediaType != null && mediaType.Contains((int)e.MediaType))
            .OrderBy(e => e.Number).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                vesQuery = vesQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<VESDto>();
            result = _mapper.Map<List<VESDto>>(vesQuery);

            return result;
        }

        public IEnumerable<VESDto> GetAllVESAvailable(int pageNumber, int pageSize)
        {
            var VESs = new List<VES>();
            VESs = _DbContext.VES.Where(e => e.IsHide == false).OrderBy(e => e.Number).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                VESs = VESs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<VESDto>();
            result = _mapper.Map<List<VESDto>>(VESs);

            return result;
        }

        public IEnumerable<VESDto> GetAllVESByIdGroupVes(int pageNumber, int pageSize, Guid IdGroupVes)
        {
            var VESs = new List<VES>();
            VESs = _DbContext.VES.Where(e => e.IsHide == false && e.IdGroupVes == IdGroupVes).OrderBy(e => e.Number).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                VESs = VESs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new List<VESDto>();
            result = _mapper.Map<List<VESDto>>(VESs);

            return result;
        }

        public VESDto GetVESById(Guid IdVES)
        {
            var VESs = new VES();
            VESs = _DbContext.VES.Where(e => e.Id == IdVES).FirstOrDefault();

            var result = new VESDto();
            result = _mapper.Map<VESDto>(VESs);

            return result;
        }

        public Response HideVESByList(List<Guid> IdVES, bool IsHide)
        {
            var VESs = _DbContext.VES.Where(ar => IdVES.Contains(ar.Id)).ToList();
            if (!VESs.Any())
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy dữ liệu cần chỉnh sửa !"
                };
            }

            VESs.ForEach(category => category.IsHide = IsHide);

            _DbContext.VES.UpdateRange(VESs);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = $"{(IsHide ? "Khóa" : "Hủy khóa")} thành công !" };
        }

        public Response InsertVES(VESDto VESDto)
        {
            var VES = _mapper.Map<VES>(VESDto);

            _DbContext.VES.Add(VES);
            _DbContext.SaveChanges();

            return new Response()
            {
                Success = true,
                Fail = false,
                Message = "Thêm mới thành công !"
            };
        }

        public Response UpdateVES(Guid IdVES, VESDto VESDto)
        {
            var VES = _DbContext.VES.Find(IdVES);
            if (VES == null)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy thông tin !"
                };
            }

            VES.MediaTitle = VESDto.MediaTitle;
            VES.MediaPath = VESDto.MediaPath;
            VES.MediaDescription = VESDto.MediaDescription;
            VES.MediaLinkType = VESDto.MediaLinkType;
            VES.MediaType = VESDto.MediaType;
            VES.IdGroupVes = VESDto.IdGroupVes;
            VES.Number = VESDto.Number;

            if (VESDto.IdFile is null)
            {
                if (VESDto.FileNameDocument is null)
                {
                    VES.FileNameDocument = null;
                    VES.FileNameExtention = null;
                }
                else
                {
                    VES.FileNameDocument = VESDto.FileNameDocument;
                    VES.FileNameExtention = VESDto.FileNameExtention;
                }
            }

            _DbContext.VES.Update(VES);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "Cập nhật thành công !" };
        }

        public Response UpdateVESWhenInsertImageAvatar(Guid IdVes, string fileNameExtention, string fileNameAvatar)
        {
            var VES = _DbContext.VES.Find(IdVes);
            if (VES == null)
            {
                return new Response()
                {
                    Success = false,
                    Fail = true,
                    Message = "Không tìm thấy thông tin !"
                };
            }

            VES.Avatar = fileNameAvatar;
            VES.FileAvatarExtention = fileNameExtention;

            _DbContext.VES.Update(VES);
            _DbContext.SaveChanges();

            return new Response() { Success = true, Fail = false, Message = "" };
        }

        public IEnumerable<CategoryVes> GetAllCategoryVesByVideo(int pageNumber, int pageSize)
        {
            var VESs = _DbContext.VES.Where(e => e.IsHide == false && e.MediaType == 0).Select(e => e.IdGroupVes).ToList();
            if (!VESs.Any())
            {
                return Enumerable.Empty<CategoryVes>();
            }
            var groupVes = _DbContext.GroupVes.Where(e => e.IsHide == false && VESs.Contains(e.Id)).Select(e => e.IdcategoryVes).ToList();
            if (!VESs.Any())
            {
                return Enumerable.Empty<CategoryVes>();
            }
            var categoryVes = _DbContext.CategoryVes.Where(e => e.IsHide == false && groupVes.Contains(e.Id)).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                categoryVes = categoryVes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return categoryVes;
        }

        public IEnumerable<CategoryVes> GetAllCategoryVesByVesSound(int pageNumber, int pageSize)
        {
            var VESs = _DbContext.VES.Where(e => e.IsHide == false && e.MediaType == 3 || e.MediaType == 4).Select(e => e.IdGroupVes).ToList();
            if (!VESs.Any())
            {
                return Enumerable.Empty<CategoryVes>();
            }
            var groupVes = _DbContext.GroupVes.Where(e => e.IsHide == false && VESs.Contains(e.Id)).Select(e => e.IdcategoryVes).ToList();
            if (!VESs.Any())
            {
                return Enumerable.Empty<CategoryVes>();
            }
            var categoryVes = _DbContext.CategoryVes.Where(e => e.IsHide == false && groupVes.Contains(e.Id)).ToList();

            if (pageNumber != 0 && pageSize != 0)
            {
                if (pageNumber < 0) { pageNumber = 1; }
                categoryVes = categoryVes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return categoryVes;
        }
        #endregion
    }
}
