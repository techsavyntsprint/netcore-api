using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class SettingNotFoundException : BaseNotFoundException
    {
        public SettingNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}