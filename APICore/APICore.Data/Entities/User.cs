using APICore.Data.Entities.Enums;
using System;
using System.Collections.Generic;

namespace APICore.Data.Entities
{
    public class User
    {
        public User()
        {
            UserTokens = new HashSet<UserToken>();
        }

        public int Id { get; set; }
        public string Identity { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullName { get; set; }
        public GenderEnum Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTimeOffset? LastLoggedIn { get; set; }
        public string Avatar { get; set; }
        public string AvatarMimeType { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}