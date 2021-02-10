using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class EmailInUseBadRequestException : BaseBadRequestException
    {
        public EmailInUseBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}