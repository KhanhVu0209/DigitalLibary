using DigitalLibary.Service.Common;
using DigitalLibary.Service.Dto;
using System.Collections.Generic;
using System;

namespace DigitalLibary.Service.Repository.IRepository
{
    public interface ICategoryVesRepository
    {
        IEnumerable<CategoryVesDto> GetAllCategoryVesByELecture(int pageNumber, int pageSize);
        IEnumerable<CategoryVesDto> GetAllCategoryVesByVideo(int pageNumber, int pageSize);
        IEnumerable<CategoryVesDto> GetAllCategoryVesBySound(int pageNumber, int pageSize);

        IEnumerable<CategoryVesDto> GetAllCategoryVes(int pageNumber, int pageSize);
        IEnumerable<CategoryVesDto> GetAllCategoryVesAvailable(int pageNumber, int pageSize);
        CategoryVesDto GetAllCategoryVesById(Guid IdCategoryVes);
        Response InsertCategoryVes(CategoryVesDto CategoryVesDto);
        Response UpdateCategoryVes(Guid IdCategoryVes, CategoryVesDto CategoryVesDto);
        Response DeleteCategoryVesByList(List<Guid> IdCategoryVes);
        Response HideCategoryVesByList(List<Guid> IdCategoryVes, bool IsHide);
    }
}
