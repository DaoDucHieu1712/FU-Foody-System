using System.ComponentModel.DataAnnotations;
using System.Web;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using FFS.Application.Constant;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.DTOs.Post;
using FFS.Application.Entities;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthenticateController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAuthRepository _authRepository;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		private ILoggerManager _logger;


		public AuthenticateController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IAuthRepository authRepository, IMapper mapper, IEmailService emailService, ILoggerManager logger)
		{
			_db = db;
			_userManager = userManager;
			_authRepository = authRepository;
			_mapper = mapper;
			_emailService = emailService;
			_logger = logger;
		}
		[HttpPost()]
		public async Task<IActionResult> Login([FromBody] LoginDTO model)
		{
			// Kiểm tra xem email có tồn tại trong hệ thống hay không
			_logger.LogInfo($"Attempting to login user with email: {model.Email}");
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				_logger.LogInfo($"Email {model.Email} does not exist.");
				return BadRequest("Email không tồn tại");
			}

			// Kiểm tra mật khẩu
			var result = await _userManager.CheckPasswordAsync(user, model.Password);
			if (result)
			{
				_logger.LogInfo($"User with email {model.Email} successfully logged in.");
				var token = await _authRepository.GenerateToken(user);
				var roles = await _userManager.GetRolesAsync(user);
				return Ok(new UserClientDTO
				{
					UserId = user.Id,
					Email = user.Email,
					Role = roles[0],
					Token = token
				});
			}
			else
			{
				_logger.LogInfo($"Invalid password for user with email {model.Email}.");
				return BadRequest("Mật khẩu không đúng");
			}
		}


		[HttpPost]
		public async Task<IActionResult> LoginByEmail([FromBody] LoginDTO logindto)
		{
			try
			{
				_logger.LogInfo($"Attempting to login user by email: {logindto.Email}");

				if (!ModelState.IsValid)
				{
					_logger.LogInfo("Invalid login request.");
					return BadRequest("Lỗi đăng nhập !");
				}

				var UserClient = await _authRepository.Login(logindto.Email, logindto.Password);

				if (UserClient == null)
				{
					_logger.LogInfo("Invalid email or password.");
					return BadRequest("Email hoặc mật khẩu không hợp lệ !");
				}

				_logger.LogInfo($"User with email {logindto.Email} successfully logged in.");

				return Ok(new { UserClient });
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while logging in user by email {logindto.Email}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetCurrentUser()
		{
			var userId = User.FindFirst("UserId")?.Value;
			try
			{
				_logger.LogInfo($"Attempting to get current user with ID: {userId}");

				dynamic user = await _authRepository.GetUser(userId);
				if (user == null)
				{
					_logger.LogInfo($"User with ID {userId} not found.");
					return NotFound();
				}

				_logger.LogInfo($"Successfully retrieved current user with ID: {userId}");

				return Ok(user);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving current user with ID {userId}: {ex.Message}");
				return NotFound();
			}
		}

		[HttpGet("{UId}")]
		public async Task<IActionResult> GetUserInformation(string UId)
		{
			try
			{
				_logger.LogInfo($"Attempting to retrieve user information for user with ID: {UId}");
				var user = await _authRepository.GetUserInformation(UId);
				if (user == null)
				{
					_logger.LogInfo($"User with ID {UId} not found.");
					return NotFound();
				}
				var userInfo = _mapper.Map<UserInfoDTO>(user);
				userInfo.TotalPost = user.Posts.Count;
				DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
				userInfo.TotalRecentComments = user.Comments.Count(c => c.UpdatedAt >= sevenDaysAgo);
				userInfo.Posts = user.Posts.Select(post =>
				{
					var postDto = _mapper.Map<PostDTO>(post);
					postDto.LikeNumber = post.ReactPosts.Where(x => x.IsLike == true).Count();
					postDto.CommentNumber = post.Comments?.Count ?? 0;

					postDto.Comments = post.Comments.Select(comment =>
					{
						var commentDto = _mapper.Map<CommentPostDTO>(comment);
						return commentDto;
					}).ToList();
					return postDto;
				}).ToList();
				_logger.LogInfo($"Successfully retrieved user information for user with ID: {UId}");
				return Ok(userInfo);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving user information for user with ID {UId}: {ex.Message}");
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> LoginGoogle([FromBody] GoogleRequest googleRequest)
		{
			try
			{
				_logger.LogInfo("Attempting to log in with Google.");
				var payload = await GoogleJsonWebSignature.ValidateAsync(googleRequest.idToken, new GoogleJsonWebSignature.ValidationSettings());
				if (!CommonService.IsEmailFPT(payload.Email))
				{
					_logger.LogInfo("Google login failed. Email is not from FPT system.");
					throw new Exception("Email của bạn không thuộc hệ thống FPT! Vui lòng thử lại!");
				}
				var googleId = payload.Subject;
				UserRegisterDTO user = new UserRegisterDTO()
				{
					email = payload.Email,
					Avatar = payload.Picture,
				};

				var UserClient = await _authRepository.LoginWithFptMail(user);
				_logger.LogInfo("Successfully logged in with Google.");
				return Ok(new { UserClient });
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while logging in with Google: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> RegisterShipper(ShipperRegisterDTO shipperRegisterDTO)
		{
			try
			{
				await _authRepository.ShipperRegister(shipperRegisterDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> StoreRegister([FromBody] StoreRegisterDTO storeRegisterDTO)
		{
			try
			{
				_logger.LogInfo("Attempting to register a shipper.");
				await _authRepository.StoreRegister(storeRegisterDTO);
				_logger.LogInfo("Shipper registration successful.");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while registering a shipper: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		//[HttpPost("testsendmail")]
		//public async Task<APIResponseModel> TestSendMail(EmailModel emailModel)
		//{

		//	try
		//	{
		//		await _emailService.SendEmailAsync(emailModel);
		//		return new APIResponseModel()
		//		{
		//			Code = 200,
		//			Message = "OK",
		//			IsSucceed = true,
		//			Data = "Send email success"
		//		};

		//	}
		//	catch (Exception ex)
		//	{
		//		return new APIResponseModel()
		//		{
		//			Code = 400,
		//			Message = "Error: " + ex.Message,
		//			IsSucceed = false,
		//			Data = ex.ToString(),
		//		};
		//	}
		//}

		[HttpPost]
		[AllowAnonymous]
		public async Task<APIResponseModel> ForgotPassword([Required] string email)
		{
			try
			{
				_logger.LogInfo($"Attempting to initiate password reset for email: {email}");
				if (CommonService.IsEmailFPT(email))
				{
					_logger.LogInfo("Email is an FPT email.");
					return new APIResponseModel()
					{
						Code = 400,
						Message = "Email is an FPT email",
						IsSucceed = false,
						Data = "Email của bạn thuộc hệ thống FPT! Vui lòng đăng nhập với tài khoản Google để truy cập vào hệ thống!",
					};

				}
				var user = await _userManager.FindByEmailAsync(email);
				if (user != null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordLink = "http://127.0.0.1:5173/reset-password?token=" + HttpUtility.UrlEncode(token) + "&email=" + HttpUtility.UrlEncode(user.Email);
					var emailModel = await GetEmailForResetPassword(email, resetPasswordLink);
					try
					{
						await _emailService.SendEmailAsync(emailModel);
						_logger.LogInfo("Password reset email sent successfully.");
						return new APIResponseModel()
						{
							Code = 200,
							Message = "OK",
							IsSucceed = true,
							Data = "Email đã được gửi thành công"
						};

					}
					catch (Exception ex)
					{
						_logger.LogError($"An error occurred while sending password reset email: {ex.Message}");
						return new APIResponseModel()
						{
							Code = 400,
							Message = "Error: " + ex.Message,
							IsSucceed = false,
							Data = ex.ToString(),
						};
					}
				}
				return new APIResponseModel()
				{
					Code = 400,
					Message = "Error: email not found!",
					IsSucceed = false,
					Data = "Email không tồn tại trong hệ thống, vui lòng nhập lại!",
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		[HttpPost]
		[AllowAnonymous]
		[Route("reset-password")]
		public async Task<APIResponseModel> ResetPassword(ResetPasswordDTO model)
		{
			try
			{
				_logger.LogInfo($"Attempting to reset password for email: {model.Email}");
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
					if (!resetPassResult.Succeeded)
					{
						foreach (var error in resetPassResult.Errors)
						{
							ModelState.AddModelError(error.Code, error.Description);
						}
						_logger.LogInfo("Password reset token has expired.");
						return new APIResponseModel()
						{
							Code = 400,
							Message = "Token đã hết hạn",
							IsSucceed = false,
							Data = ModelState
						};
					}
					_logger.LogInfo("Password reset successful.");
					return new APIResponseModel
					{
						Code = 200,
						Message = "OK",
						IsSucceed = true,
						Data = "Đổi mật khẩu thành công!"
					};
				}
				_logger.LogInfo("Invalid reset password link.");
				return new APIResponseModel()
				{
					Code = 400,
					Message = "Link invalid",
					IsSucceed = false,
					Data = "Link invalid",
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while resetting password for email {model.Email}: {ex.Message}");
				throw new Exception(ex.Message);
			}

		}


		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
		{
			try
			{
				_logger.LogInfo("Attempting to change password.");
				await _authRepository.ChangePassword(changePasswordDTO);
				_logger.LogInfo("Password change successful.");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while changing password: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		private async Task<EmailModel> GetEmailForResetPassword(string emailReceive, string resetpasswordLink)
		{
			EmailModel result = new EmailModel();
			List<string> emailTos = new List<string>();
			emailTos.Add(emailReceive);
			result.Subject = EmailTemplateSubjectConstant.ResetPasswordSubject;
			string bodyEmail = string.Format(EmailTemplateBodyConstant.ResetPasswordBody, emailReceive, resetpasswordLink);
			result.Body = bodyEmail + EmailTemplateBodyConstant.SignatureFooter;
			result.To = emailTos;
			return await Task.FromResult(result);
		}

		[HttpGet]
		public async Task<IActionResult> Profile(string email)
		{
			try
			{
				_logger.LogInfo($"Retrieving profile for email: {email}");
				ApplicationUser user = await _authRepository.Profile(email);
				_logger.LogInfo($"Profile retrieved successfully for email: {email}");
				return Ok(user);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving profile for email {email}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}


		[HttpPut]
		public async Task<IActionResult> Profile(string email, [FromBody] UserCommandDTO userCommandDTO)
		{
			try
			{
				_logger.LogInfo($"Updating profile for email: {email}");
				await _authRepository.ProfileEdit(email, userCommandDTO);
				_logger.LogInfo($"Profile updated successfully for email: {email}");
				return Ok("update thanh cong");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating profile for email {email}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetShipperById(string userId)
		{
			try
			{
				_logger.LogInfo($"Retrieving shipper by ID: {userId}");
				var user = await _authRepository.GetShipperById(userId);
				_logger.LogInfo($"Shipper retrieved successfully for ID: {userId}");
				return Ok(user);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving shipper by ID {userId}: {ex.Message}");
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRoleByUser(string id)
		{
			try
			{
				return Ok(await _authRepository.GetRoleWithUser(id));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving role by user ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}
	}
}
