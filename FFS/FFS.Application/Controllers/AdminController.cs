using AutoMapper;

using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities.Constant;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase {

        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public AdminController(IReportRepository reportRepository, IMapper mapper, IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize(Roles = $"StoreOwner")]
        public IActionResult GetReports([FromBody]ReportParameters reportParameters)
        {
            try
            {
                IEnumerable<dynamic> data =  _reportRepository.GetReports(reportParameters);
                return Ok(data);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountGetReports([FromBody] ReportParameters reportParameters)
        {
            try
            {
                int total = _reportRepository.CountGetReports(reportParameters);
                return Ok(total);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult GetAccounts([FromBody] UserParameters userParameters)
        {
            try
            {
                IEnumerable<dynamic> data = _userRepository.GetUsers(userParameters);
                int total = _userRepository.CountGetUsers(userParameters);
                var res = new
                {
                    data = data,
                    total = total,
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetRoles()
        {
            try
            {
                IEnumerable<dynamic> data = _userRepository.GetRoles();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExportReport()
        {
            try
            {
                var data = await _reportRepository.ExportReport();
                string uniqueFileName = "ThongKe_BaoCao_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExportUser()
        {
            try
            {
                var data = await _userRepository.ExportUser();
                string uniqueFileName = "ThongKe_NguoiDung_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetRequestAccount([FromBody] UserParameters userParameters)
        {
            try
            {
                IEnumerable<dynamic> data = _userRepository.GetRequestCreateAccount(userParameters);
                int total = _userRepository.CountGetRequestCreateAccount(userParameters);
                var res = new
                {
                    data = data,
                    total = total,
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult BanAccount([FromBody] UserParameters userParameters)
        {
            try
            {
                string idBan = userParameters.id;
                _userRepository.BanAccount(idBan);
                return Ok($"Khóa thành công tài khoản {userParameters.Username}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult UnBanAccount([FromBody] UserParameters userParameters)
        {
            try
            {
                string idUnBan = userParameters.id;
                _userRepository.UnBanAccount(idUnBan);
                return Ok($"Mở khóa thành công tài khoản {userParameters.Username}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult ApproveUser([FromBody] UserParameters userParameters)
        {
            try
            {
                string id = userParameters.id;
                _userRepository.ApproveUser(id, userParameters.Action);
                return Ok($"Duyệt thành công tài khoản {userParameters.Username}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        



    }
}
