using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class InvalidRefreshTokenBadRequestException : BaseBadRequestException
    {
        public InvalidRefreshTokenBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 4000010;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}