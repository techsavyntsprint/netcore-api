using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;


namespace APICore.Services.Exceptions { 

    public class SendEmailBadRequestException : BaseBadRequestException
    {
        public SendEmailBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400008;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
