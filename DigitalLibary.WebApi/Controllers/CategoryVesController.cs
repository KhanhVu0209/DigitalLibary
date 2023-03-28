using AutoMapper;
using DigitalLibary.Service.Dto;
using DigitalLibary.Service.Repository.IRepository;
using DigitalLibary.WebApi.Common;
using DigitalLibary.WebApi.Helper;
using DigitalLibary.WebApi.Payload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

namespace DigitalLibary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryVesController: Controller
    {
        #region Variables
        private readonly ICategoryVesRepository _CategoryVesRepository;
        private readonly AppSettingModel _appSettingModel;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly SaveToDiary _saveToDiary;
        private readonly ILogger<CategoryVesController> _logger;
        #endregion

        #region Contructor
        public CategoryVesController(ICategoryVesRepository CategoryVesRepository,
        IOptionsMonitor<AppSettingModel> optionsMonitor,
        IMapper mapper,
        SaveToDiary saveToDiary,
        JwtService jwtService, IUserRepository userRepository, ILogger<CategoryVesController> logger)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _mapper = mapper;
            _CategoryVesRepository = CategoryVesRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _saveToDiary = saveToDiary;
            _logger = logger;
        }
        #endregion

        #region METHOD
        // GET: api/CategoryVes/GetAllCategoryVesByELecture
        [HttpGet]
        [Route("GetAllCategoryVesByELecture")]
        public IEnumerable<CategoryVesDto> GetAllCategoryVesByELecture(int pageNumber, int pageSize)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVesByELecture(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/CategoryVes/GetAllCategoryVesByVideo
        [HttpGet]
        [Route("GetAllCategoryVesByVideo")]
        public IEnumerable<CategoryVesDto> GetAllCategoryVesByVideo(int pageNumber, int pageSize)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVesByVideo(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/CategoryVes/GetAllCategoryVesBySound
        [HttpGet]
        [Route("GetAllCategoryVesBySound")]
        public IEnumerable<CategoryVesDto> GetAllCategoryVesBySound(int pageNumber, int pageSize)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVesBySound(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/CategoryVes/GetAllCategoryVes
        [HttpGet]
        [Route("GetAllCategoryVes")]
        public IEnumerable<CategoryVesDto> GetAllCategoryVes(int pageNumber, int pageSize)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVes(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/CategoryVes/GetAllCategoryVesAvailable
        [HttpGet]
        [Route("GetAllCategoryVesAvailable")]
        public IEnumerable<CategoryVesDto> GetAllCategoryVesAvailable(int pageNumber, int pageSize)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVesAvailable(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/CategoryVes/GetCategoryVesById
        [HttpGet]
        [Route("GetCategoryVesById")]
        public CategoryVesDto GetCategoryVesById(Guid IdCategoryVes)
        {
            try
            {
                var result = _CategoryVesRepository.GetAllCategoryVesById(IdCategoryVes);
                _logger.LogInformation("Lấy thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy không thành công: {message}", ex.Message);
                throw;
            }
        }
        // HttpPost: api/CategoryVes/InsertCategoryVes
        [HttpPost]
        [Route("InsertCategoryVes")]
        public IActionResult InsertCategoryVes(CategoryVesModel CategoryVesModel)
        {
            try
            {
                //check role admin
                Request.Headers.TryGetValue("Authorization", out var headerValue);
                if (headerValue.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }
                CheckRoleSystem checkRoleSystem = new CheckRoleSystem(_jwtService, _userRepository);
                CheckAdminModel checkModel = checkRoleSystem.CheckAdmin(headerValue);

                if (!checkModel.check)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }

                var CategoryVesDto = _mapper.Map<CategoryVesDto>(CategoryVesModel);
                CategoryVesDto.CreatedDate = DateTime.Now;
                CategoryVesDto.IsHide = false;
                CategoryVesDto.CategoryVesCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                CategoryVesDto.Id = Guid.NewGuid();

                var result = _CategoryVesRepository.InsertCategoryVes(CategoryVesDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Thêm mới không thành công: {message}", ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Fail = true,
                    Message = "Thêm mới không thành công !"
                });
            }
        }   
        // HttpPut: api/CategoryVes/UpdateCategoryVes
        [HttpPut]
        [Route("UpdateCategoryVes")]
        public IActionResult UpdateCategoryVes(CategoryVesModel CategoryVesModel)
        {
            try
            {
                //check role admin
                Request.Headers.TryGetValue("Authorization", out var headerValue);
                if (headerValue.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }
                CheckRoleSystem checkRoleSystem = new CheckRoleSystem(_jwtService, _userRepository);
                CheckAdminModel checkModel = checkRoleSystem.CheckAdmin(headerValue);

                if (!checkModel.check)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }

                var CategoryVesDto = _mapper.Map<CategoryVesDto>(CategoryVesModel);
                var result = _CategoryVesRepository.UpdateCategoryVes(CategoryVesDto.Id, CategoryVesDto);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Cập nhật không thành công: {message}", ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Fail = true,
                    Message = "Cập nhật không thành công !"
                });
            }
        }
        // HttpDelete: api/CategoryVes/DeleteCategoryVesByList
        [HttpDelete]
        [Route("DeleteCategoryVesByList")]
        public IActionResult DeleteCategoryVesByList(List<Guid> IdCategoryVes)
        {
            try
            {
                //check role admin
                Request.Headers.TryGetValue("Authorization", out var headerValue);
                if (headerValue.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }
                CheckRoleSystem checkRoleSystem = new CheckRoleSystem(_jwtService, _userRepository);
                CheckAdminModel checkModel = checkRoleSystem.CheckAdmin(headerValue);

                if (!checkModel.check)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }

                var result = _CategoryVesRepository.DeleteCategoryVesByList(IdCategoryVes);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Xóa không thành công: {message}", ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Fail = true,
                    Message = "Xóa không thành công !"
                });
            }
        }
        // HttpDelete: api/CategoryVes/HideCategoryVesByList
        [HttpPut]
        [Route("HideCategoryVesByList")]
        public IActionResult HideCategoryVesByList(List<Guid> IdCategoryVes, bool IsHide)
        {
            try
            {
                //check role admin
                Request.Headers.TryGetValue("Authorization", out var headerValue);
                if (headerValue.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }
                CheckRoleSystem checkRoleSystem = new CheckRoleSystem(_jwtService, _userRepository);
                CheckAdminModel checkModel = checkRoleSystem.CheckAdmin(headerValue);

                if (!checkModel.check)
                {
                    return BadRequest(new
                    {
                        message = "Bạn cần đăng nhập tài khoản Admin"
                    });
                }

                var result = _CategoryVesRepository.HideCategoryVesByList(IdCategoryVes, IsHide);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Xóa không thành công: {message}", ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Fail = true,
                    Message = "Xóa không thành công !"
                });
            }
        }
        #endregion
    }
}
