using ADIRA.Server.Authentication;
using ADIRA.Shared.BusinessDataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ADIRA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserAccountService _userAccountService;
        public AccountController(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager(_userAccountService);
            var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.UserName, loginRequest.Password);
            if (userSession is null)
            {
                // Check if the user is blocked
                if (_userAccountService.AuthenticateUser(loginRequest.UserName,loginRequest.Password))
                {
                    // Set the X-Blocked-User header in the response
                    Response.Headers.Add("X-Blocked-User", "true");
                    return Unauthorized(new { message = "User is blocked. Please try again later." });
                }

                return Unauthorized(new { message = "Invalid Username or Password" });

            }
           
            else
            {
                _userAccountService.ResetLoginAttempts(userSession.UserName);
               // _userAccountService.UnBlockUser(userSession.UserName);
                return userSession;
            }
               
        }
    }
}
