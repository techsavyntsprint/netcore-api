using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APICore.Services
{
    public interface IEmailService
    {
        Task SendEmailResponseAsync(string subject, string htmlMessage,string email);
    }
}
