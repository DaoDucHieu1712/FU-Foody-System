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
		private ILoggerManager _logger;

		private readonly IMapper _mapper;

		public AdminController(IReportRepository reportRepository, IHubContext<NotificationHub> hubContext, INotificationRepository notifyRepository, IUserRepository userRepository, IPostRepository postRepository, IOrderRepository orderRepository, IMapper mapper, ILoggerManager logger)
		{
			_reportRepository = reportRepository;
			_userRepository = userRepository;
			_postRepository = postRepository;
			_orderRepository = orderRepository;
			_mapper = mapper;
			_hubContext = hubContext;
			_notifyRepository = notifyRepository;
			_logger = logger;
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult GetReports([FromBody] ReportParameters reportParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to get reports...");

				IEnumerable<dynamic> data = _reportRepository.GetReports(reportParameters);

				_logger.LogInfo("Successfully retrieved reports.");

				return Ok(data);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting reports: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult CountGetReports([FromBody] ReportParameters reportParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to count reports...");

				int total = _reportRepository.CountGetReports(reportParameters);

				_logger.LogInfo("Successfully counted reports.");

				return Ok(total);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while counting reports: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult GetAccounts([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to get user accounts...");

				IEnumerable<dynamic> data = _userRepository.GetUsers(userParameters);
				int total = _userRepository.CountGetUsers(userParameters);
				var res = new dataReturn
				{
					data = data,
					total = total,
				};

				_logger.LogInfo("Successfully retrieved user accounts.");

				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting user accounts: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult GetPosts([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to get posts...");

				IEnumerable<dynamic> data = _userRepository.GetPosts(userParameters);
				int total = _userRepository.CountGetPosts(userParameters);
				var res = new dataReturn
				{
					data = data,
					total = total,
				};

				_logger.LogInfo("Successfully retrieved posts.");

				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting posts: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> ApprovePost([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to approve post...");
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
					_logger.LogInfo("Post approved successfully.");
					return Ok("Duyệt thành công!");
				}
				_logger.LogInfo("Post not found.");
				return BadRequest("Bài viết không tồn tại! Xin vui lòng thử lại sau");
			}
			catch (Exception)
			{
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult GetRoles()
		{
			try
			{
				_logger.LogInfo("Attempting to get roles...");

				IEnumerable<dynamic> data = _userRepository.GetRoles();

				_logger.LogInfo("Successfully retrieved roles.");

				return Ok(data);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting roles: {ex.Message}");
				throw;
			}
		}

		//[Authorize(Roles = "Admin")]
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

		//[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> ExportUser()
		{
			try
			{
				_logger.LogInfo("Attempting to export report...");
				var data = await _userRepository.ExportUser();
				string uniqueFileName = "ThongKe_NguoiDung_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
				_logger.LogInfo("Successfully exported report.");
				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while exporting report: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult GetRequestAccount([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo("Attempting to get request for creating account...");

				IEnumerable<dynamic> data = _userRepository.GetRequestCreateAccount(userParameters);
				int total = _userRepository.CountGetRequestCreateAccount(userParameters);
				var res = new dataReturn
				{
					data = data,
					total = total,
				};

				_logger.LogInfo("Successfully retrieved request for creating account.");

				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting request for creating account: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult BanAccount([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to ban account {userParameters.Username}...");

				string idBan = userParameters.id;
				_userRepository.BanAccount(idBan);

				_logger.LogInfo($"Successfully banned account {userParameters.Username}.");

				return Ok($"Khóa thành công tài khoản {userParameters.Username}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while banning account {userParameters.Username}: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult UnBanAccount([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to unban account {userParameters.Username}...");

				string idUnBan = userParameters.id;
				_userRepository.UnBanAccount(idUnBan);

				_logger.LogInfo($"Successfully unbanned account {userParameters.Username}.");

				return Ok($"Mở khóa thành công tài khoản {userParameters.Username}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while unbanning account {userParameters.Username}: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult ApproveUser([FromBody] UserParameters userParameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to approve user account {userParameters.Username}...");

				string id = userParameters.id;
				_userRepository.ApproveUser(id, userParameters.Action);

				_logger.LogInfo($"Successfully approved user account {userParameters.Username}.");

				return Ok($"Duyệt thành công tài khoản {userParameters.Username}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while approving user account {userParameters.Username}: {ex.Message}");
				throw;
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult AccountsStatistic()
		{
			try
			{
				_logger.LogInfo("Attempting to retrieve accounts statistics...");

				List<AccountStatistic> accountsStatistic = _userRepository.AccountsStatistic();
				int totalAccount = _userRepository.CountTotalUsers();

				_logger.LogInfo("Successfully retrieved accounts statistics.");

				return Ok(new
				{
					TotalAccount = totalAccount,
					AccountsStatistic = accountsStatistic
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving accounts statistics: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}


		[Authorize(Roles = "Admin")]
		[HttpGet("{year}")]
		public IActionResult ReportsStatistic(int year = default)
		{
			if (year == default)
			{
				year = DateTime.Now.Year;
			}

			try
			{
				_logger.LogInfo($"Attempting to retrieve reports statistics for year {year}...");

				List<ReportStatistic> reportsStatistic = _reportRepository.ReportStatistics(year);
				int totalReportYear = _reportRepository.CountAllReportInYear(year);

				_logger.LogInfo($"Successfully retrieved reports statistics for year {year}.");

				var result = new
				{
					TotalReportYear = totalReportYear,
					ReportsStatistic = reportsStatistic
				};

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving reports statistics for year {year}: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpGet]
		public IActionResult PostsStatistic()
		{
			try
			{
				_logger.LogInfo("Attempting to retrieve posts statistics...");

				List<PostStatistic> postStatistics = _postRepository.PostStatistics();
				int totalPost = _postRepository.CountAllPost();

				_logger.LogInfo("Successfully retrieved posts statistics.");

				return Ok(new
				{
					TotalPost = totalPost,
					PostsStatistic = postStatistics
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving posts statistics: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
	}

	public class dataReturn
	{
		public IEnumerable<dynamic> data { get; set; }
		public int total { get; set; }

	}
}
