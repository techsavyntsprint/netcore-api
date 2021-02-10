using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using APICore.Services.Exceptions;
using Microsoft.Extensions.Localization;

namespace APICore.Services.Impls
{
    public class EmailService :IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<EmailService> _localizer;

        public EmailService(IConfiguration configuration,IStringLocalizer<EmailService> localizer)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }
            public async Task SendEmailResponseAsync(string subject, string message,string email)
        {

            var apiKey = _configuration.GetValue<string>("Sendgrid:SendGridKey");
            string sandBoxMode= _configuration.GetValue<string>("Sendgrid:UseSandbox");
            string userSendgrid= _configuration.GetValue<string>("Sendgrid:SendGridUser");

          
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(userSendgrid);
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject,"", message);

                if (sandBoxMode=="true")
                {
                    var MailSettings = new MailSettings();
                    MailSettings.SandboxMode = new SandboxMode();
                    MailSettings.SandboxMode.Enable = true;
                    msg.MailSettings = MailSettings;
                }
             
            var response = await client.SendEmailAsync(msg);
       
            if (response.StatusCode.ToString() != "Accepted")
            {
                throw new SendEmailBadRequestException(_localizer);
            }

        }

       

    }
}
