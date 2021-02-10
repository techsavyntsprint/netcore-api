using System;

namespace APICore.Data.Entities
{
    public class UserToken
    {
        public int Id { get; set; }

        public string AccessToken { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        public int UserId { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string OS { get; set; }
        public string OSPlatform { get; set; }
        public string OSVersion { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public string ClientVersion { get; set; }
        public virtual User User { get; set; }
    }
}