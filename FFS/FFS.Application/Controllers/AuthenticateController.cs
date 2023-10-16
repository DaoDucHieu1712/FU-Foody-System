﻿using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

using AutoMapper;

using FFS.Application.Data;
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

namespace FFS.Application.Controllers {
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
        public IActionResult LoginByEmail([FromBody] LoginDTO logindto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Lỗi đăng nhập !");
            }

            var token = _authRepository.Login(logindto.Email, logindto.Password);

            if (token == null)
            {
                return Unauthorized("Email hoặc mật khẩu không hợp lệ !");
            }

            return Ok(new { token });
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

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                await _authRepository.UserRegister(userRegisterDTO);
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
        public async Task<IActionResult> Profile(string email, [FromBody]UserCommandDTO userCommandDTO)
        {
            try
            {
                await _authRepository.ProfileEdit(email, userCommandDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
