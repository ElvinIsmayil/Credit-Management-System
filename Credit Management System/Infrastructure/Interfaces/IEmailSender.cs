namespace Credit_Management_System.Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
    }
}
