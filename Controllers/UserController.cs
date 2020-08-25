using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApiJWT.JWT;
using SecureApiJWT.Models;

namespace SecureApiJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJWTAuthManager _jWTAuthManager;
        public UserController(IJWTAuthManager jWTAuthManager) 
        {
            _jWTAuthManager = jWTAuthManager;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpPost("AuthUser")]
        public IActionResult AuthUser([FromBody] UserAuth userAuth)
        {
            var token=_jWTAuthManager.Auth(userAuth.UserName,userAuth.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
