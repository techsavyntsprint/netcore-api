using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class FileNullBadRequestException : BaseBadRequestException
    {
        public FileNullBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400005;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}