using System.Threading.Tasks;

namespace APICore.Services
{
    public interface IEmailService
    {
        Task SendEmailResponseAsync(string subject, string htmlMessage,string email);
    }
}
