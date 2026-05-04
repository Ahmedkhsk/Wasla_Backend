namespace Wasla_Backend.Helpers.EmailSender
{
    public interface IEmailSenderHelper
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
