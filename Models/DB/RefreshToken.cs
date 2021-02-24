using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Api.Models.DB
{
    public partial class RefreshToken
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplaceByToken { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]

        public virtual SystemUser User { get; set; }
    }
}
