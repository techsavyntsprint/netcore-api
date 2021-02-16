using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class EmptyEmailBadRequestExceptioncs : BaseBadRequestException
    {
        public EmptyEmailBadRequestExceptioncs(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400011;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}