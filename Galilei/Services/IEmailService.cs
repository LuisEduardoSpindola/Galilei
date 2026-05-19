namespace Galilei.Services
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string toEmail, string subject, string body);
    }
}
