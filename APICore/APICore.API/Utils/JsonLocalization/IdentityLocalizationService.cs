using Microsoft.Extensions.Localization;
using System.Reflection;

namespace APICore.API.Utils.JsonLocalization
{
    public class IdentityLocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public IdentityLocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return _localizer[key];
        }

        public LocalizedString GetLocalizedHtmlString(string key, string parameter)
        {
            return _localizer[key, parameter];
        }
    }

    public class IdentityResource
    {
    }
}