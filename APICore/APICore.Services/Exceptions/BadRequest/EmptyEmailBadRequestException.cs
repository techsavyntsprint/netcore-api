using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class EmptyEmailBadRequestException : BaseBadRequestException
    {
        public EmptyEmailBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400011;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}