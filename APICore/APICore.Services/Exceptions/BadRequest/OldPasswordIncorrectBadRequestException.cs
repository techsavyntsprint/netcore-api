using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class OldPasswordIncorrectBadRequestException : BaseBadRequestException
    {
        public OldPasswordIncorrectBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400004;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}