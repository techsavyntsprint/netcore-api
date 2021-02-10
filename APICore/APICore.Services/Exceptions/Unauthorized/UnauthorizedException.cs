using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class UnauthorizedException : BaseUnauthorizedException
    {
        public UnauthorizedException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 401001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}