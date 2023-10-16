using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.Entities;
using Microsoft.AspNetCore.Identity;
﻿using FFS.Application.Constant;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.Entities;
using FFS.Application.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.EntityFrameworkCore;
using FFS.Application.Entities.Constant;

namespace FFS.Application.Repositories.Impls
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSetting _appSettings;
        private readonly IEmailService _emailService;

        public AuthRepository(UserManager<ApplicationUser> userManager, IOptionsMonitor<AppSetting> optionsMonitor, IEmailService emailService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _appSettings = optionsMonitor.CurrentValue;
            _emailService = emailService;
            _context = context;
        }

        public async Task<string> Login(string email, string password)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            // User not found
            if (user == null)
            {
                throw new Exception("Email không tồn tại !");
            }

            // Verify the password
            var result = await _userManager.CheckPasswordAsync(user, password);
            // Password is incorrect
            if (!result)
            {
                throw new Exception("Mật khẩu không đúng !");
            }

            // If the email and password are valid, generate a JWT token
            var token = await GenerateToken(user);

            return token;
        }

        public async Task StoreRegister(StoreRegisterDTO storeRegisterDTO)
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                ApplicationUser _user = await _userManager.FindByEmailAsync(storeRegisterDTO.Email);
                if (_user != null)
                    throw new Exception("Email đã tồn tại , Vui lòng thử lại !");
                if (storeRegisterDTO.Password != storeRegisterDTO.PasswordConfirm)
                    throw new Exception("Vui lòng kiểm tra lại mật khẩu !");

                var NewUser = new ApplicationUser
                {
                    FirstName = storeRegisterDTO.FirstName,
                    LastName = storeRegisterDTO.LastName,
                    Avatar = storeRegisterDTO.Avatar,
                    Allow = storeRegisterDTO.Allow,
                    Gender = storeRegisterDTO.Gender,
                    BirthDay = storeRegisterDTO.BirthDay,
                    UserName = storeRegisterDTO.Email,
                    Email = storeRegisterDTO.Email,
                };

                var result = await _userManager.CreateAsync(NewUser, storeRegisterDTO.Password);
                if (result.Succeeded == false)
                {
                    var specificErrors = result.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                var role_rs = await _userManager.AddToRoleAsync(NewUser, "StoreOwner");
                if (role_rs.Succeeded == false)
                    throw new Exception("Đã có lỗi xảy ra");

                var _newuser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == NewUser.Email);

                var NewStore = new Store
                {
                    UserId = _newuser.Id,
                    StoreName = storeRegisterDTO.StoreName,
                    AvatarURL = storeRegisterDTO.AvatarURL,
                    Description = storeRegisterDTO.Description,
                    Address = storeRegisterDTO.Address,
                    TimeStart = storeRegisterDTO.TimeStart,
                    TimeEnd = storeRegisterDTO.TimeEnd,
                    PhoneNumber = storeRegisterDTO.PhoneNumber,
                };

                await _context.Stores.AddAsync(NewStore);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                //transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GenerateToken(ApplicationUser us)
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
                var userIdentity = await _userManager.FindByIdAsync(us.Id.ToString());
                var roles = await _userManager.GetRolesAsync(userIdentity);
                string role = roles[0].ToString();
                var tokenDecription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.Name, us.UserName),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, us.Email),
                new Claim("UserId", us.Id.ToString()),
                new Claim("TokenId", Guid.NewGuid().ToString())}),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = jwtTokenHandler.CreateToken(tokenDecription);
                string accessToken = jwtTokenHandler.WriteToken(token);
                return accessToken;
            }
        public async Task<bool> ResetPassword(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return false;
                string passwordGen = CommonService.GeneratePassword(8);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordGen);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var emailModel = await GetEmailForResetPassword(email, passwordGen);
                    await _emailService.SendEmailAsync(emailModel);
                    return true;
                }
                return false;
            }
        private async Task<EmailModel> GetEmailForResetPassword(string emailReceive, string newPassword)
            {
                EmailModel result = new EmailModel();
                List<string> emailTos = new List<string>();
                emailTos.Add(emailReceive);
                result.Subject = EmailTemplateSubjectConstant.ResetPasswordSubject;
                string bodyEmail = string.Format(EmailTemplateBodyConstant.ResetPasswordBody, emailReceive, newPassword);
                result.Body = bodyEmail + EmailTemplateBodyConstant.SignatureFooter;
                result.To = emailTos;
                return await Task.FromResult(result);

            }
        public async Task ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, changePasswordDTO.OldPassword))
                {
                    throw new Exception("Email hoặc mật khẩu không đúng, vui lòng thử lại !!!");
                }

                if (changePasswordDTO.ConfirmPassword != changePasswordDTO.NewPassword) throw new Exception("Xin vui lòng kiểm tra lại mật khẩu xác nhận !!");
                var rs = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UserRegister(UserRegisterDTO userRegisterDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!CommonService.IsEmailFPT(userRegisterDTO.email))
                {
                    throw new Exception("Email không hợp lệ!");
                }

                ApplicationUser userExist = await _userManager.FindByEmailAsync(userRegisterDTO.email);
                if (userExist != null)
                {
                    throw new Exception("Email đã tồn tại! Xin vui lòng thử lại");
                }

                if (!CommonService.IsStrongPassword(userRegisterDTO.password))
                {
                    throw new Exception("Mật khẩu phải nhiều hơn 8 kí tư, có chữ in hoa, chữ thường, số và kí tự đặc biệt!");
                }
                var user = new ApplicationUser()
                {
                    Email = userRegisterDTO.email,
                    UserName = CommonService.ExtractUsername(userRegisterDTO.email)
                };

                IdentityResult check = await _userManager.CreateAsync(user, userRegisterDTO.password);
                if (check.Succeeded == false)
                {
                    var specificErrors = check.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                IdentityRole? role = await _context.Roles.FirstOrDefaultAsync(role => role.NormalizedName == Role.USER);
                if (role == null)
                {
                    throw new Exception("Có lỗi xảy ra vui lòng liên hệ Admin!");
                }
                IdentityUserRole<string> userRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task ShipperRegister(ShipperRegisterDTO shipperRegisterDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!CommonService.IsEmail(shipperRegisterDTO.email))
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
                    UserName = CommonService.ExtractUsername(shipperRegisterDTO.email)
                };

                IdentityResult check = await _userManager.CreateAsync(shipper, shipperRegisterDTO.password);
                if (check.Succeeded == false)
                {
                    var specificErrors = check.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                IdentityRole? role = await _context.Roles.FirstOrDefaultAsync(role => role.NormalizedName == Role.SHIPPER);
                if (role == null)
                {
                    throw new Exception("Có lỗi xảy ra vui lòng liên hệ Admin!");
                }
                IdentityUserRole<string> userRole = new IdentityUserRole<string>
                {
                    UserId = shipper.Id,
                    RoleId = role.Id
                };
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public  async Task<ApplicationUser> Profile(string email)
        {
            try
            {
                ApplicationUser _user = await _userManager.FindByEmailAsync(email);
                if (_user == null) throw new Exception("Đã có lỗi bất định xảy ra !");
                return _user;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task ProfileEdit(string email, UserCommandDTO userCommandDTO)
        {
            try
            {
                ApplicationUser _user = await _userManager.FindByEmailAsync(email);
                if (_user == null) throw new Exception("Đã có lỗi bất định xảy ra !");
                _user.BirthDay = userCommandDTO.BirthDay;
                _user.FirstName = userCommandDTO.FirstName;
                _user.LastName = userCommandDTO.LastName;
                _user.Gender = userCommandDTO.Gender;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
