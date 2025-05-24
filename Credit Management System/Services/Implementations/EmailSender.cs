using System.Net;
using System.Net.Mail;
using IEmailSender = Credit_Management_System.Services.Interfaces.IEmailSender;


namespace Credit_Management_System.Services.Implementations
{

    public sealed class EmailSender : IEmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly bool _enableSsl;

        public EmailSender(string smtpHost, int smtpPort, string smtpUser, string smtpPass, bool enableSsl = true)
        {
            _smtpHost = smtpHost ?? throw new ArgumentNullException(nameof(smtpHost));
            _smtpPort = smtpPort;
            _smtpUser = smtpUser ?? throw new ArgumentNullException(nameof(smtpUser));
            _smtpPass = smtpPass ?? throw new ArgumentNullException(nameof(smtpPass));
            _enableSsl = enableSsl;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = _enableSsl
            };

            using var mail = new MailMessage(_smtpUser, email, subject, htmlMessage)
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
