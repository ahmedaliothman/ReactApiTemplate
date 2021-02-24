using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DB;
using Api.Models.JWT;

namespace Api.Services.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [EnableCors("all")]
    [ApiController]
    [Route("[controller]")]
    public class JWTController : ControllerBase
    {

        private IjwtServices _jwtServices;
        private commonUserInfo commonUserInfo_;
        private readonly ApiDBContext _context;
        public ResponseRequest ResponseRequest_;

        public JWTController(IjwtServices jwtServices, ApiDBContext context)
        {
            _context = context;
            _jwtServices = jwtServices;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        async public Task<IActionResult> register([FromBody] SystemUser model)
        {
            ResponseRequest_ = new ResponseRequest();
           

            var Exists = _context.SystemUsers.Where(r => r.Email == model.Email);
            if (Exists.Count() > 0)
            {
                ResponseRequest_.Status = 1;
                ResponseRequest_.Message = "هذا الاميل موجود من قبل بالفعل او الرقم المدنى  ";
                return BadRequest(ResponseRequest_);
            }

            SystemUser user_ = _jwtServices.Register(model);
            var response = user_;
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم انشاء الحساب بنجاح برجاء الانتظار حتى الموافقه علية";
            ResponseRequest_.Response = response;
            return Ok(ResponseRequest_);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        async public Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _jwtServices.Authenticate(model, ipAddress());

            if (response == null)
                return BadRequest(new {status=0 , message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        async public Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _jwtServices.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }


        [HttpPost("revoke-token")]
        async public Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _jwtServices.RevokeToken(token, ipAddress());

            if (response == null)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

    }
}
