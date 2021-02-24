using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Models.Config;
using Api.Models.DB;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api.Models.JWT {
    public interface IjwtServices {
        Task<AuthenticateResponse> Authenticate (AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken (string token, string ipAddress);
        public SystemUser Register (SystemUser model);
        Task<object> RevokeToken (string token, string ipAddress);
    }
  
   public class jwtServices : IjwtServices {
        private readonly ApiDBContext _context;
        private readonly AppSettings _appSettings;
        public jwtServices (IOptions<AppSettings> appSettings, ApiDBContext context) {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public SystemUser Register (SystemUser model) {
            SystemUser user_ = model;
            _context.SystemUsers.Add(user_);
            _context.SaveChanges();
            return user_;
        }
        public SystemUser AuthenticateUser (AuthenticateRequest model) {
            
            SystemUser user_ = _context.SystemUsers.Where(r=>r.Email==model.Username&&r.Password==model.Password).FirstOrDefault();

            return user_;

        }
        async public Task<AuthenticateResponse> Authenticate (AuthenticateRequest model, string ipAddress) {
            try {
                SystemUser user_ = AuthenticateUser (model);
                // return null if user not found
                if (user_ == null) return null;

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken (user: user_);
                var refreshToken = generateRefreshToken (ipAddress);

                // save refresh token
                _context.RefreshTokens.Add (new RefreshToken { UserId = user_.UserId, Token = refreshToken.Token, Expires = (DateTime) refreshToken.Expires, CreatedByIp = ipAddress });
                _context.SaveChanges ();
                return new AuthenticateResponse (_user: user_, jwtToken: jwtToken, refreshToken: refreshToken.Token);
            } catch (System.Exception ex) {
                throw;
            }

        }

        async public Task<AuthenticateResponse> RefreshToken (string token, string ipAddress) {
            //var user = _context.Users.SingleOrDefault (u => u.RefreshTokens.Any (t => t.Token == token));
            //List<RefreshTokens> refreshtokens = await _context.getRefreshTokens(actionId: 2, token: token);
            List<RefreshToken> refreshtokens = _context.RefreshTokens.Where (r => r.Token == token).ToList<RefreshToken> ();
            // return null if no user found with token
            if (refreshtokens == null) return null;

            RefreshToken refreshToken = refreshtokens.FirstOrDefault ();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken (ipAddress);
            refreshToken.ReplaceByToken = newRefreshToken.Token;
            refreshToken.RevokedByIp = ipAddress;
            var user_ = new SystemUser();
            _context.RefreshTokens.Add (new RefreshToken { UserId = refreshToken.UserId, Token = newRefreshToken.Token, Expires = (DateTime) refreshToken.Expires, CreatedByIp = ipAddress });
            _context.SaveChanges ();

            // generate new jwt
            var jwtToken = generateJwtToken (refreshToken: refreshToken);
            return new AuthenticateResponse (_refreshToken: refreshToken, jwtToken: jwtToken, refreshToken: newRefreshToken.Token, _user: user_);
        }

        async public Task<object> RevokeToken (string token, string ipAddress) {
            List<RefreshToken> refreshtokens = _context.RefreshTokens.Where (r => r.Token == token).ToList<RefreshToken> ();

            // return null if no user found with token
            if (refreshtokens.Count == 0) return null;

            RefreshToken refreshToken = refreshtokens.FirstOrDefault ();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.Revoked = DateTime.Now;
            _context.SaveChanges ();
            return true;
        }

        private string generateJwtToken (RefreshToken refreshToken = null, SystemUser user = null) {
            var tokenHandler = new JwtSecurityTokenHandler ();
            string a = "hello ahmed ali osman secret key for my api guys guys guys ";
            var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                user == null ? new Claim (ClaimTypes.Name, refreshToken.UserId.ToString ()) : new Claim (ClaimTypes.Name, user.UserId.ToString ())
                }),
                Expires = DateTime.UtcNow.AddMinutes (600),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            return tokenHandler.WriteToken (token);
        }
        private RefreshToken generateRefreshToken (string ipAddress) {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider ()) {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes (randomBytes);
                return new RefreshToken {
                    Token = Convert.ToBase64String (randomBytes),
                        Expires = DateTime.UtcNow.AddDays (7),
                        Created = DateTime.UtcNow,
                        CreatedByIp = ipAddress
                };
            }
        }

    }
}