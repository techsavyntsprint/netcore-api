using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class FileInvalidSizeBadRequestException : BaseBadRequestException
    {
        public FileInvalidSizeBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400006;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}