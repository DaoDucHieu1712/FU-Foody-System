using FFS.Application.Constant;
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

namespace FFS.Application.Repositories.Impls
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSetting _appSettings;
        private readonly IEmailService _emailService;

        public AuthRepository(UserManager<ApplicationUser> userManager, IOptionsMonitor<AppSetting> optionsMonitor, IEmailService emailService)
        {
            _userManager = userManager;
            _appSettings = optionsMonitor.CurrentValue;
            _emailService = emailService;   
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
    }
}
