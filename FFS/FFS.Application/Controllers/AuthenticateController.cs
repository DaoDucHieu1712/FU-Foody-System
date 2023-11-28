using System.ComponentModel.DataAnnotations;
using System.Web;
using AutoMapper;
using FFS.Application.Constant;
using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.DTOs.Post;
using FFS.Application.Entities;
using FFS.Application.Helper;
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


		public AuthenticateController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IAuthRepository authRepository, IMapper mapper, IEmailService emailService)
		{
			_db = db;
			_userManager = userManager;
			_authRepository = authRepository;
			_mapper = mapper;
			_emailService = emailService;
		}
		[HttpPost()]
		public async Task<IActionResult> Login([FromBody] LoginDTO model)
		{
			// Kiểm tra xem email có tồn tại trong hệ thống hay không
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return BadRequest("Email không tồn tại");
			}

			// Kiểm tra mật khẩu
			var result = await _userManager.CheckPasswordAsync(user, model.Password);
			if (result)
			{
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
				return BadRequest("Mật khẩu không đúng");
			}
		}


		[HttpPost]
		public async Task<IActionResult> LoginByEmail([FromBody] LoginDTO logindto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest("Lỗi đăng nhập !");
				}
				var UserClient = await _authRepository.Login(logindto.Email, logindto.Password);

				if (UserClient == null)
				{
					return BadRequest("Email hoặc mật khẩu không hợp lệ !");
				}

				return Ok(new { UserClient });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetCurrentUser()
		{
			var userId = User.FindFirst("UserId")?.Value;
			try
			{
				dynamic user = await _authRepository.GetUser(userId);
				if (user == null)
				{
					return NotFound();
				}
				return Ok(user);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}

		[HttpGet("{UId}")]
		public async Task<IActionResult> GetUserInformation(string UId)
		{
			try
			{
				var user = await _authRepository.GetUserInformation(UId);
				if (user == null)
				{
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
				return Ok(userInfo);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> LoginGoogle([FromBody] GoogleRequest googleRequest)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(googleRequest.idToken, new GoogleJsonWebSignature.ValidationSettings());
				if (!CommonService.IsEmailFPT(payload.Email))
				{
					throw new Exception("Email của bạn không thuộc hệ thống FPT! Vui lòng thử lại!");
				}
				var googleId = payload.Subject;
				UserRegisterDTO user = new UserRegisterDTO()
				{
					email = payload.Email,
					Avatar = payload.Picture,
				};

				var UserClient = await _authRepository.LoginWithFptMail(user);
				return Ok(new { UserClient });
			}
			catch (Exception ex)
			{
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
				await _authRepository.StoreRegister(storeRegisterDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("testsendmail")]
		public async Task<APIResponseModel> TestSendMail(EmailModel emailModel)
		{

			try
			{
				await _emailService.SendEmailAsync(emailModel);
				return new APIResponseModel()
				{
					Code = 200,
					Message = "OK",
					IsSucceed = true,
					Data = "Send email success"
				};

			}
			catch (Exception ex)
			{
				return new APIResponseModel()
				{
					Code = 400,
					Message = "Error: " + ex.Message,
					IsSucceed = false,
					Data = ex.ToString(),
				};
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<APIResponseModel> ForgotPassword([Required] string email)
		{
			try
			{
				if (CommonService.IsEmailFPT(email))
				{
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
						return new APIResponseModel()
						{
							Code = 400,
							Message = "Token đã hết hạn",
							IsSucceed = false,
							Data = ModelState
						};
					}
					return new APIResponseModel
					{
						Code = 200,
						Message = "OK",
						IsSucceed = true,
						Data = "Đổi mật khẩu thành công!"
					};
				}
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
				throw new Exception(ex.Message);
			}

		}


		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
		{
			try
			{
				await _authRepository.ChangePassword(changePasswordDTO);
				return Ok();
			}
			catch (Exception ex)
			{
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
				ApplicationUser user = await _authRepository.Profile(email);
				return Ok(user);
			}
			catch (Exception ex)
			{

				return StatusCode(500, ex.Message);
			}
		}


		[HttpPut]
		public async Task<IActionResult> Profile(string email, [FromBody] UserCommandDTO userCommandDTO)
		{
			try
			{
				await _authRepository.ProfileEdit(email, userCommandDTO);
				return Ok("update thanh cong");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetShipperById(string userId)
		{
			try
			{
				var user = await _authRepository.GetShipperById(userId);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
