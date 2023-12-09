using AutoMapper;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AdminController : ControllerBase
	{

		private readonly IReportRepository _reportRepository;
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IAuthRepository _authRepository;
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly INotificationRepository _notifyRepository;

		private readonly IMapper _mapper;

		public AdminController(IReportRepository reportRepository, IHubContext<NotificationHub> hubContext, INotificationRepository notifyRepository, IUserRepository userRepository, IPostRepository postRepository, IOrderRepository orderRepository, IMapper mapper)
		{
			_reportRepository = reportRepository;
			_userRepository = userRepository;
			_postRepository = postRepository;
			_orderRepository = orderRepository;
			_mapper = mapper;
			_hubContext = hubContext;
			_notifyRepository = notifyRepository;
		}

		[HttpPost]
		[Authorize(Roles = $"Admin")]
		public IActionResult GetReports([FromBody] ReportParameters reportParameters)
		{
			try
			{
				IEnumerable<dynamic> data = _reportRepository.GetReports(reportParameters);
				return Ok(data);
			}
			catch (Exception)
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
			catch (Exception)
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
			catch (Exception)
			{
				throw;
			}
		}



		[Authorize]
		[HttpPost]
		public IActionResult GetPosts([FromBody] UserParameters userParameters)
		{
			try
			{
				IEnumerable<dynamic> data = _userRepository.GetPosts(userParameters);
				int total = _userRepository.CountGetPosts(userParameters);
				var res = new
				{
					data = data,
					total = total,
				};
				return Ok(res);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ApprovePost([FromBody] UserParameters userParameters)
		{
			try
			{
				int idPost = Convert.ToInt32(userParameters.IdPost);
				Post post = await _postRepository.FindById(idPost, null);
				if (post != null)
				{
					if (userParameters.Status == 2)
					{
						post.Status = Entities.Enum.StatusPost.Accept;

					}
					if (userParameters.Status == 3)
					{
						post.Status = Entities.Enum.StatusPost.Reject;
					}
					await _postRepository.Update(post);

					if (post.UserId != null)
					{
						var notification = new Notification
						{
							CreatedAt = DateTime.Now,
							UpdatedAt = DateTime.Now,
							IsDelete = false,
							UserId = post.UserId,
							Title = "Phê duyệt bài viết",
							Content = $"Bài viết {post.Title} {(post.Status == Entities.Enum.StatusPost.Accept ? "đã được phê duyệt" : "bị từ chối")}."
						};

						await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
						await _notifyRepository.Add(notification);
					}
					return Ok("Duyệt thành công!");
				}
				return BadRequest("Bài viết không tồn tại! Xin vui lòng thử lại sau");
			}
			catch (Exception)
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
			catch (Exception)
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
			catch (Exception)
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
			catch (Exception)
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
			catch (Exception)
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
			catch (Exception)
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
			}
			catch (Exception)
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
					TotalReportYear = TotalReportYear,
					ReportsStatistic = reportsStatistic
				};

				return Ok(result);
			}
			catch (Exception)
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
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
