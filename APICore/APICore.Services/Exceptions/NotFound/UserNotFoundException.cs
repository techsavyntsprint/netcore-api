using Microsoft.Extensions.Localization;

namespace APICore.Services.Exceptions
{
    public class UserNotFoundException : BaseNotFoundException
    {
        public UserNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}