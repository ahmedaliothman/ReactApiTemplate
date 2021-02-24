using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Api.Models.DB
{
    public partial class UserRole
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [JsonIgnore]

        public virtual SystemRole User { get; set; }
        [JsonIgnore]
       
        public virtual SystemUser UserNavigation { get; set; }
    }
}
