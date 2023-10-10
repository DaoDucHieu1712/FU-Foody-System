using System.ComponentModel.DataAnnotations;

namespace FFS.Application.DTOs.Auth
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp.")]
        public string ConfirmPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
