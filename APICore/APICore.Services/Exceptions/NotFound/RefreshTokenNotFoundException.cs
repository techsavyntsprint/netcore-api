using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class RefreshTokenNotFoundException : BaseNotFoundException
    {
        public RefreshTokenNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404003;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}