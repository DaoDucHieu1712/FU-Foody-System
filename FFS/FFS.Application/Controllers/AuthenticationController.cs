using System.Net.Mail;
using System.Text.RegularExpressions;

using AutoMapper;

using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.Entities;
using FFS.Application.Entities.Constant;
using FFS.Application.Helper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthenticateController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

                if (!ValidatePassword.IsStrongPassword(shipperRegisterDTO.password))
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

                if (!ValidatePassword.IsStrongPassword(shipperRegisterDTO.password))
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
    }
}
