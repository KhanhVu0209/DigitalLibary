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
    public class GroupVesController: Controller
    {
        #region Variables
        private readonly IGroupVesRepository _GroupVesRepository;
        private readonly AppSettingModel _appSettingModel;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly SaveToDiary _saveToDiary;
        private readonly ILogger<GroupVesController> _logger;
        #endregion

        #region Contructor
        public GroupVesController(IGroupVesRepository GroupVesRepository,
        IOptionsMonitor<AppSettingModel> optionsMonitor,
        IMapper mapper,
        SaveToDiary saveToDiary,
        JwtService jwtService, IUserRepository userRepository, ILogger<GroupVesController> logger)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _mapper = mapper;
            _GroupVesRepository = GroupVesRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _saveToDiary = saveToDiary;
            _logger = logger;
        }
        #endregion

        #region METHOD
        // GET: api/GroupVes/GetAllGroupVes
        [HttpGet]
        [Route("GetAllGroupVes")]
        public IEnumerable<GroupVesDto> GetAllGroupVes(int pageNumber, int pageSize)
        {
            try
            {
                var result = _GroupVesRepository.GetAllGroupVes(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/GroupVes/GetAllGroupVesAvailable
        [HttpGet]
        [Route("GetAllGroupVesAvailable")]
        public IEnumerable<GroupVesDto> GetAllGroupVesAvailable(int pageNumber, int pageSize)
        {
            try
            {
                var result = _GroupVesRepository.GetAllGroupVesAvailable(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/GroupVes/GetAllGroupVesByIdcategoryVes
        [HttpGet]
        [Route("GetAllGroupVesByIdcategoryVes")]
        public IEnumerable<GroupVesDto> GetAllGroupVesByIdcategoryVes(int pageNumber, int pageSize, Guid IdcategoryVes)
        {
            try
            {
                var result = _GroupVesRepository.GetAllGroupVesByIdcategoryVes(pageNumber, pageSize, IdcategoryVes);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/GroupVes/GetGroupVesById
        [HttpGet]
        [Route("GetGroupVesById")]
        public GroupVesDto GetGroupVesById(Guid IdGroupVes)
        {
            try
            {
                var result = _GroupVesRepository.GetAllGroupVesById(IdGroupVes);
                _logger.LogInformation("Lấy thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy không thành công: {message}", ex.Message);
                throw;
            }
        }
        // HttpPost: api/GroupVes/InsertGroupVes
        [HttpPost]
        [Route("InsertGroupVes")]
        public IActionResult InsertGroupVes(GroupVesModel GroupVesModel)
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

                var GroupVesDto = _mapper.Map<GroupVesDto>(GroupVesModel);
                GroupVesDto.Status = 0;
                GroupVesDto.CreatedDate = DateTime.Now;
                GroupVesDto.IsHide = false;
                GroupVesDto.GroupCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                GroupVesDto.Id = Guid.NewGuid();

                var result = _GroupVesRepository.InsertGroupVes(GroupVesDto);

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
        // HttpPut: api/GroupVes/UpdateGroupVes
        [HttpPut]
        [Route("UpdateGroupVes")]
        public IActionResult UpdateGroupVes(GroupVesModel GroupVesModel)
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

                var GroupVesDto = _mapper.Map<GroupVesDto>(GroupVesModel);
                var result = _GroupVesRepository.UpdateGroupVes(GroupVesDto.Id, GroupVesDto);

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
        // HttpDelete: api/GroupVes/DeleteGroupVesByList
        [HttpDelete]
        [Route("DeleteGroupVesByList")]
        public IActionResult DeleteGroupVesByList(List<Guid> IdGroupVes)
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

                var result = _GroupVesRepository.DeleteGroupVesByList(IdGroupVes);

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
        // HttpDelete: api/GroupVes/HideGroupVesByList
        [HttpPut]
        [Route("HideGroupVesByList")]
        public IActionResult HideGroupVesByList(List<Guid> IdGroupVes, bool IsHide)
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

                var result = _GroupVesRepository.HideGroupVesByList(IdGroupVes, IsHide);

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
