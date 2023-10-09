using FFS.Application.DTOs.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FFS.Application.Repositories.Impls
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingModel _emailSetting;
        public EmailService(IOptionsMonitor<EmailSettingModel> emailSetting)
        {
            _emailSetting = emailSetting.CurrentValue;
        }
        public async Task SendEmailAsync(EmailModel emailModel)
        {
            using (var emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail);
                emailMessage.From.Add(emailFrom);
                foreach (var receiver in emailModel.To)
                {
                    MailboxAddress emailTo = new MailboxAddress("Receiver", receiver);
                    emailMessage.To.Add(emailTo);
                }

                emailMessage.Subject = emailModel.Subject;
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = emailModel.Body;
                emailMessage.Body = bodyBuilder.ToMessageBody();
                using (var mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_emailSetting.Server, _emailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_emailSetting.UserName, _emailSetting.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

        }
    }
}
