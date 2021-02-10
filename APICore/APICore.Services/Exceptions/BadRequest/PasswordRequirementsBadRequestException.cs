using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class PasswordRequirementsBadRequestException : BaseBadRequestException
    {
        public PasswordRequirementsBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}