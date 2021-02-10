using APICore.Common.DTO.Request;
using APICore.Data.Entities;
using System.Threading.Tasks;

namespace APICore.Services
{
    public interface ISettingService
    {
        Task<Setting> SetSettingAsync(SettingRequest settingRequest);

        Task<string> GetSettingAsync(string settingKey);
    }
}