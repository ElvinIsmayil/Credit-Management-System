using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using IEmailSender = Credit_Management_System.Services.Interfaces.IEmailSender;


namespace Credit_Management_System.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var smtp = new SmtpClient("smtp.yourserver.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("you@domain.com", "your-password"),
                EnableSsl = true
            };

            var mail = new MailMessage("you@domain.com", email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
