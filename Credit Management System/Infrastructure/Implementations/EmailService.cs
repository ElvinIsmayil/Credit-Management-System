using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Credit_Management_System.Infrastructure.Configurations;
using Credit_Management_System.Infrastructure.Interfaces;

namespace Credit_Management_System.Infrastructure.Implementations
{
    public sealed class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value ?? throw new ArgumentNullException(nameof(smtpOptions));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient(_smtpSettings.Host)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            using var mail = new MailMessage(_smtpSettings.Username, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            try
            {
                await smtp.SendMailAsync(mail, cancellationToken);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}