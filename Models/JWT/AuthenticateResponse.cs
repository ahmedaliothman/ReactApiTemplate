using Api.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Models.JWT
{
    public class AuthenticateResponse
    {
        public object userInfo { set; get; }
        public string JwtToken { get; set; }




        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(string jwtToken, string refreshToken, RefreshToken _refreshToken = null, object _user = null)
        {
            if (_refreshToken != null)
            {
                userInfo = _user;
                JwtToken = jwtToken;
                RefreshToken = refreshToken;
            }
            else if (_user != null)
            {
                userInfo = _user;
                 JwtToken = jwtToken;
                RefreshToken = refreshToken;
            }

        }
    }
}
