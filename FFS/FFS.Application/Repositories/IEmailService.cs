using FFS.Application.DTOs.Email;

namespace FFS.Application.Repositories
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}
