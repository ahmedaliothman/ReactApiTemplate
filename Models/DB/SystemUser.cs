using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Api.Models.DB
{
    public partial class SystemUser
    {
        public SystemUser()
        {
            RefreshTokens = new HashSet<RefreshToken>();
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
