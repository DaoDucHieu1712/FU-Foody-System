using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

using AutoMapper;

using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.Entities;
using FFS.Application.Repositories;
using FFS.Application.Entities.Constant;
using FFS.Application.Helper;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using FFS.Application.Constant;

using FFS.Application.DTOs.Auth;

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

        [HttpPost]
        public async Task<IActionResult> RegisterShipper(ShipperRegisterDTO shipperRegisterDTO)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(shipperRegisterDTO.email))
                {
                    throw new Exception("Email không hợp lệ!");
                }

                ApplicationUser user = await _userManager.FindByEmailAsync(shipperRegisterDTO.email);
                if (user != null)
                {
                    throw new Exception("Email đã tồn tại! Xin vui lòng thử lại");
                }

                if (!CommonService.IsStrongPassword(shipperRegisterDTO.password))
                {
                    throw new Exception("Mật khẩu phải nhiều hơn 8 kí tư, có chữ in hoa, chữ thường, số và kí tự đặc biệt!");
                }
                var shipper = new ApplicationUser()
                {
                    Email = shipperRegisterDTO.email,
                    UserName = ExtractUsername(shipperRegisterDTO.email)
                };

                IdentityResult check = await _userManager.CreateAsync(shipper, shipperRegisterDTO.password);
                if (check.Succeeded == false)
                {
                    var specificErrors = check.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                IdentityRole? role = await _db.Roles.FirstOrDefaultAsync(role => role.NormalizedName == Role.SHIPPER);
                if (role == null)
                {
                    throw new Exception("Có lỗi xảy ra vui lòng liên hệ Admin!");
                }
                IdentityUserRole<string> userRole = new IdentityUserRole<string>
                {
                    UserId = shipper.Id,
                    RoleId = role.Id
                };
                await _db.UserRoles.AddAsync(userRole);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok("Đăng kí thành công tài khoản shipper");

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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

        [HttpGet]
        public IActionResult GoogleSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleSignInCallback))
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleSignInCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                return BadRequest("Failed to sign in with Google.");
            }

            // Here, you can access user information from authenticateResult.Principal
            var userId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Implement your user registration or authentication logic here
            // Example: Check if the user exists, create a new user, or issue JWT tokens

            // Redirect or return a response to your client app
            return Ok(new { UserId = userId, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(ShipperRegisterDTO shipperRegisterDTO)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(shipperRegisterDTO.email))
                {
                    throw new Exception("Email không hợp lệ!");
                }

                ApplicationUser user = await _userManager.FindByEmailAsync(shipperRegisterDTO.email);
                if (user != null)
                {
                    throw new Exception("Email đã tồn tại! Xin vui lòng thử lại");
                }

                if (!CommonService.IsStrongPassword(shipperRegisterDTO.password))
                {
                    throw new Exception("Mật khẩu phải nhiều hơn 8 kí tư, có chữ in hoa, chữ thường, số và kí tự đặc biệt!");
                }
                var shipper = new ApplicationUser()
                {
                    Email = shipperRegisterDTO.email,
                    UserName = ExtractUsername(shipperRegisterDTO.email)
                };

                IdentityResult check = await _userManager.CreateAsync(shipper, shipperRegisterDTO.password);
                if (check.Succeeded == false)
                {
                    var specificErrors = check.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                IdentityRole? role = await _db.Roles.FirstOrDefaultAsync(role => role.NormalizedName == Role.SHIPPER);
                if (role == null)
                {
                    throw new Exception("Có lỗi xảy ra vui lòng liên hệ Admin!");
                }
                IdentityUserRole<string> userRole = new IdentityUserRole<string>
                {
                    UserId = shipper.Id,
                    RoleId = role.Id
                };
                await _db.UserRoles.AddAsync(userRole);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok("Đăng kí thành công tài khoản shipper");

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink = Url.Action("ResetPassword", "Authenticate", new { token, email = user.Email }, Request.Scheme);
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

        [HttpGet("reset-password")]
        public async Task<APIResponseModel> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDTO { Token = token, Email = email };
            return new APIResponseModel()
            {
                Code = 200,
                Message = "OK",
                IsSucceed = true,
                Data = model
            };
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<APIResponseModel> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if(!resetPassResult.Succeeded)
                {
                    foreach(var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return new APIResponseModel() {
                        Code = 200,
                        Message = "Error",
                        IsSucceed = true,
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
                Message = "Error: email not found!",
                IsSucceed = false,
                Data = "Email không tồn tại trong hệ thống!",
            };
        }



        string ExtractUsername(string emailAddress)
        {
            int atIndex = emailAddress.IndexOf("@");

            if (atIndex != -1)
            {
                return emailAddress.Substring(0, atIndex);
            }
            else
            {
                return emailAddress;
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
    }
}
