﻿using AutoMapper;
using FFS.Application.DTOs.Admin;
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
		private readonly IPostRepository _postRepository;

		private readonly IMapper _mapper;

		public AdminController(IReportRepository reportRepository, IUserRepository userRepository, IPostRepository postRepository, IMapper mapper)
		{
			_reportRepository = reportRepository;
			_userRepository = userRepository;
			_postRepository = postRepository;
			_mapper = mapper;
		}

		[HttpPost]
        [Authorize(Roles = $"Admin")]
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

		[HttpGet]
		public IActionResult AccountsStatistic()
		{
			try
			{
				List<AccountStatistic> AccountsStatistic = _userRepository.AccountsStatistic();
				return Ok(new
				{
					TotalAccount = _userRepository.CountTotalUsers(),
					AccountsStatistic = AccountsStatistic
				});
			} catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
			
		}

		[HttpGet("{year}")]
		public IActionResult ReportsStatistic(int year = default)
		{
			if (year == default)
			{
				year = DateTime.Now.Year;
			}

			try
			{
				List<ReportStatistic> reportsStatistic = _reportRepository.ReportStatistics(year);
				int TotalReportYear = _reportRepository.CountAllReportInYear(year);

				var result = new
				{
					TotalAccount = TotalReportYear,
					ReportsStatistic = reportsStatistic
				};

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpGet]
		public IActionResult PostsStatistic()
		{
			try
			{
				List<PostStatistic> postStatistics = _postRepository.PostStatistics();
				return Ok(new
				{
					TotalPost = _postRepository.CountAllPost(),
					PostsStatistic = postStatistics
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}

	}
}
