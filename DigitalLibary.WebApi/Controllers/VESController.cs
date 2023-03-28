using AutoMapper;
using DigitalLibary.Data.Data;
using DigitalLibary.Data.Entity;
using DigitalLibary.Service.Common.FormatApi;
using DigitalLibary.Service.Dto;
using DigitalLibary.Service.Repository.IRepository;
using DigitalLibary.WebApi.Common;
using DigitalLibary.WebApi.Helper;
using DigitalLibary.WebApi.Payload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace DigitalLibary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VESController : Controller
    {
        #region Variables
        private readonly IVESRepository _VESRepository;
        private readonly AppSettingModel _appSettingModel;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        private readonly IUnitRepository _iUnitRepository;
        private readonly SaveToDiary _saveToDiary;
        private readonly ILogger<VESController> _logger;
        #endregion

        #region Contructor
        public VESController(IVESRepository VESRepository,
        IOptionsMonitor<AppSettingModel> optionsMonitor,
        ILogger<VESController> logger,
        IMapper mapper,
        SaveToDiary saveToDiary,
        JwtService jwtService,
        IUserRepository userRepository,
        DataContext context,
        IUnitRepository iUnitRepository)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _mapper = mapper;
            _VESRepository = VESRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _context = context;
            _logger = logger;
            _iUnitRepository = iUnitRepository;
            _saveToDiary = saveToDiary;
        }
        #endregion

        #region MeThod
        // HttpGet: api/VES/GetAllCategoryVesByVesSound
        [HttpGet]
        [Route("GetAllCategoryVesByVesSound")]
        public IEnumerable<CategoryVes> GetAllCategoryVesByVesSound(int pageNumber, int pageSize)
        {
            try
            {
                var result = _VESRepository.GetAllCategoryVesByVesSound(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // HttpGet: api/VES/GetAllCategoryVesByVesVideo
        [HttpGet]
        [Route("GetAllCategoryVesByVesVideo")]
        public IEnumerable<CategoryVes> GetAllCategoryVesByVesVideo(int pageNumber, int pageSize)
        {
            try
            {
                var result = _VESRepository.GetAllCategoryVesByVideo(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // HttpPost: api/VES/GetAllVESByMediaType
        [HttpPost]
        [Route("GetAllVESByMediaType")]
        public IEnumerable<VESDto> GetAllVESByMediaType(int pageNumber, int pageSize, int[] mediaType)
        {
            try
            {
                var result = _VESRepository.GetAllVESByMediaType(pageNumber, pageSize, mediaType);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/VES/GetAllVESByIdGroup
        [HttpGet]
        [Route("GetAllVESByIdGroup")]
        public IEnumerable<VESDto> GetAllVESByIdGroup(int pageNumber, int pageSize, Guid IdGroup)
        {
            try
            {
                var result = _VESRepository.GetAllVESByIdGroupVes(pageNumber, pageSize, IdGroup);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/VES/GetAllVES
        [HttpGet]
        [Route("GetAllVES")]
        public IEnumerable<VESDto> GetAllVES(int pageNumber, int pageSize)
        {
            try
            {
                var result = _VESRepository.GetAllVES(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/VES/GetAllVESAvailable
        [HttpGet]
        [Route("GetAllVESAvailable")]
        public IEnumerable<VESDto> GetAllVESAvailable(int pageNumber, int pageSize)
        {
            try
            {
                var result = _VESRepository.GetAllVESAvailable(pageNumber, pageSize);
                _logger.LogInformation("Lấy danh sách thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy danh sách không thành công: {message}", ex.Message);
                throw;
            }
        }
        // GET: api/VES/GetVESById
        [HttpGet]
        [Route("GetVESById")]
        public VESDto GetVESById(Guid IdVES)
        {
            try
            {
                var result = _VESRepository.GetVESById(IdVES);
                _logger.LogInformation("Lấy thành công !");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Lấy không thành công: {message}", ex.Message);
                throw;
            }
        }
        // HttpPost: api/VES/InsertVES
        [HttpPost]
        [Route("InsertVES")]
        public IActionResult InsertVES([FromForm] VESModel VESModel)
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

                if (!Directory.Exists(_appSettingModel.ServerFileVes))
                {
                    Directory.CreateDirectory(_appSettingModel.ServerFileVes);
                }
                if (!Directory.Exists(_appSettingModel.ServerFileVesAvatar))
                {
                    Directory.CreateDirectory(_appSettingModel.ServerFileVesAvatar);
                }

                var VESDto = _mapper.Map<VESDto>(VESModel);
                VESDto.Status = 0;
                VESDto.CreatedDate = DateTime.Now;
                VESDto.IsHide = false;
                VESDto.Id = Guid.NewGuid();

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileContentType = file.ContentType;
                        var IdFile = VESDto.Id.ToString() + $".{fileContentType.Split('/')[1]}";
                        VESDto.FileNameDocument = file.FileName;
                        VESDto.FileNameExtention = fileContentType.Split('/')[1];

                        // prepare path to save file image
                        string pathTo = _appSettingModel.ServerFileVes;

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(IdFile));

                        // save file
                        using (var stream = System.IO.File.Create(filename))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                var result = _VESRepository.InsertVES(VESDto);

                return Ok(new
                {
                    Success = true,
                    Fail = false,
                    Message = "Thêm mới thành công !",
                    IdVes = VESDto.Id
                });
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
        // HttpPut: api/VES/UpdateVES
        [HttpPut]
        [Route("UpdateVES")]
        public IActionResult UpdateVES([FromForm] VESModel VESModel)
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

                var VESDto = _mapper.Map<VESDto>(VESModel);

                if (!Directory.Exists(_appSettingModel.ServerFileVes))
                {
                    Directory.CreateDirectory(_appSettingModel.ServerFileVes);
                }
                if (!Directory.Exists(_appSettingModel.ServerFileVesAvatar))
                {
                    Directory.CreateDirectory(_appSettingModel.ServerFileVesAvatar);
                }

                if (VESModel.IdFile is null)
                {
                    string IdFile = VESDto.Id.ToString() + "." + VESDto.FileNameExtention;

                    // set file path to save file
                    var filename = Path.Combine(_appSettingModel.ServerFileVes, Path.GetFileName(IdFile));
                    //delete file before save
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileContentType = file.ContentType;
                        var IdFile = VESDto.Id.ToString() + $".{fileContentType.Split('/')[1]}";
                        VESDto.FileNameDocument = file.FileName;
                        VESDto.FileNameExtention = fileContentType.Split('/')[1];

                        // prepare path to save file image
                        string pathTo = _appSettingModel.ServerFileVes;

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(IdFile));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        // save file
                        using (var stream = System.IO.File.Create(filename))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                var result = _VESRepository.UpdateVES(VESDto.Id, VESDto);

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
        // HttpDelete: api/VES/DeleteVESByList
        [HttpDelete]
        [Route("DeleteVESByList")]
        public IActionResult DeleteVESByList(List<Guid> IdVES)
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

                var result = _VESRepository.DeleteVESByList(IdVES);

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
        // HttpDelete: api/VES/HideVESByList
        [HttpPut]
        [Route("HideVESByList")]
        public IActionResult HideVESByList(List<Guid> IdVES, bool IsHide)
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

                var result = _VESRepository.HideVESByList(IdVES, IsHide);

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
        // HttpPost: api/VES/UploadImage
        [HttpPost]
        [Route("UploadImage")]
        public IActionResult UploadImage([FromForm] Byte File)
        {
            if (!Directory.Exists(_appSettingModel.ServerFileVes))
            {
                Directory.CreateDirectory(_appSettingModel.ServerFileVes);
            }

            try
            {
                string IdFile = Guid.NewGuid().ToString();

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileContentType = file.ContentType;
                        IdFile += $".{fileContentType.Split('/')[1]}";

                        // prepare path to save file image
                        string pathTo = _appSettingModel.ServerFileVes;

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(IdFile));

                        // save file
                        using (var stream = System.IO.File.Create(filename))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                return Ok(IdFile);
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    message = "Thêm hình ảnh không thành công"
                });
            }
        }
        // HttpGet: api/VES/GetFile
        [HttpGet]
        [Route("GetFile")]
        public IActionResult GetFile(string fileNameId)
        {
            try
            {
                var temp = fileNameId.Split('.');
                byte[] fileBytes = new byte[] { };
                string fileType = temp[temp.Length - 1].ToLower();

                if (fileType == "jpg" || fileType == "png" || fileType == "jpeg")
                {
                    fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_appSettingModel.ServerFileVes, fileNameId));
                    return File(fileBytes, "image/jpeg");
                }
                else if (fileType == "pdf")
                {
                    fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_appSettingModel.ServerFileVes, fileNameId));
                    return File(fileBytes, "application/pdf");
                }
                else if (fileType == "mpeg")
                {
                    fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_appSettingModel.ServerFileVes, fileNameId));
                    return File(fileBytes, "audio/mp3");
                }
                else if (fileType == "mp4")
                {
                    fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_appSettingModel.ServerFileVes, fileNameId));
                    return File(fileBytes, "video/mp4");
                }
                else
                {
                    return BadRequest("Unsupported file type");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // HttpPost: api/VES/InsertImageAvatar
        [HttpPost]
        [Route("InsertImageAvatar")]
        public IActionResult InsertImageAvatar([FromForm] Byte File, Guid IdVes)
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

            if (!Directory.Exists(_appSettingModel.ServerFileVesAvatar))
            {
                Directory.CreateDirectory(_appSettingModel.ServerFileVesAvatar);
            }

            try
            {
                string IdFile = IdVes.ToString();
                string fileName = "";
                string fileNameExtention = "";

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileContentType = file.ContentType;
                        IdFile += $".{fileContentType.Split('/')[1]}";
                        fileName = file.FileName;
                        fileNameExtention = fileContentType.Split('/')[1];

                        // prepare path to save file image
                        string pathTo = _appSettingModel.ServerFileVesAvatar;

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(IdFile));

                        // save file
                        using (var stream = System.IO.File.Create(filename))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                var result = _VESRepository.UpdateVESWhenInsertImageAvatar(IdVes, fileNameExtention, fileName);
                return Ok(new
                {
                    Success = result.Success,
                    Fail = result.Fail,
                    Message = "Thêm mới thành công !"
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Success = false,
                    Fail = true,
                    Message = "Thêm mới không thành công !"
                });
            }
        }
        // HttpPut: api/VES/UpdateVESImageAvatar
        [HttpPut]
        [Route("UpdateVESImageAvatar")]
        public IActionResult UpdateVESImageAvatar([FromForm] Byte File, Guid IdVes)
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

                if (!Directory.Exists(_appSettingModel.ServerFileVesAvatar))
                {
                    Directory.CreateDirectory(_appSettingModel.ServerFileVesAvatar);
                }

                string IdFile = IdVes.ToString();
                string fileName = "";
                string fileNameExtention = "";

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileContentType = file.ContentType;
                        IdFile += $".{fileContentType.Split('/')[1]}";
                        fileName = file.FileName;
                        fileNameExtention = fileContentType.Split('/')[1];

                        // prepare path to save file image
                        string pathTo = _appSettingModel.ServerFileVesAvatar;

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(IdFile));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        // save file
                        using (var stream = System.IO.File.Create(filename))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                var result = _VESRepository.UpdateVESWhenInsertImageAvatar(IdVes, fileNameExtention, fileName);
                return Ok(new
                {
                    Success = result.Success,
                    Fail = result.Fail,
                    Message = "Cập nhật thành công !"
                });
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
        #endregion
    }
}
