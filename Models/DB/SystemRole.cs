using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Api.Models.DB
{
    public partial class SystemRole
    {
        public SystemRole()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
