using DigitalLibary.Data.Entity;
using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using System;
using System.Collections.Generic;

namespace DigitalLibary.Service.Repository.IRepository
{
    public interface IVESRepository
    {
        IEnumerable<VESDto> GetAllVES(int pageNumber, int pageSize);
        IEnumerable<VESDto> GetAllVESByIdGroupVes(int pageNumber, int pageSize, Guid IdGroupVes);
        IEnumerable<VESDto> GetAllVESAvailable(int pageNumber, int pageSize);
        IEnumerable<VESDto> GetAllVESByMediaType(int pageNumber, int pageSize, int[] MediaType);
        IEnumerable<CategoryVes> GetAllCategoryVesByVideo(int pageNumber, int pageSize);
        IEnumerable<CategoryVes> GetAllCategoryVesByVesSound(int pageNumber, int pageSize);
        VESDto GetVESById(Guid IdVES);
        Response InsertVES(VESDto VESDto);
        Response UpdateVES(Guid IdVES, VESDto VESDto);
        Response UpdateVESWhenInsertImageAvatar(Guid IdVes, string fileNameExtention, string fileNameAvatar);
        Response DeleteVESByList(List<Guid> IdVES);
        Response HideVESByList(List<Guid> IdVES, bool IsHide);
    }
}
