using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    internal class FileInvalidTypeBadRequestException : BaseBadRequestException
    {
        public FileInvalidTypeBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400007;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}