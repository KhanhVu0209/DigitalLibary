using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using System.Collections.Generic;
using System;

namespace DigitalLibary.Service.Repository.IRepository
{
    public interface IGroupVesRepository
    {
        IEnumerable<GroupVesDto> GetAllGroupVes(int pageNumber, int pageSize);
        IEnumerable<GroupVesDto> GetAllGroupVesByIdcategoryVes(int pageNumber, int pageSize, Guid IdcategoryVes);
        IEnumerable<GroupVesDto> GetAllGroupVesAvailable(int pageNumber, int pageSize);
        GroupVesDto GetAllGroupVesById(Guid IdGroupVes);
        Response InsertGroupVes(GroupVesDto GroupVesDto);
        Response UpdateGroupVes(Guid IdGroupVes, GroupVesDto GroupVesDto);
        Response DeleteGroupVesByList(List<Guid> IdGroupVes);
        Response HideGroupVesByList(List<Guid> IdGroupVes, bool IsHide);
    }
}
