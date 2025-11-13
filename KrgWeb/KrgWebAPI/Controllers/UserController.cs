using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public UserController(IAuthenticationService iAuthenticationService)
        {
            _authenticationService = iAuthenticationService;
        }
        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login([FromBody] VMAuthReq reqUser)
        {
            var erroResponse = new ApiErrorResponse();
            var result = await _authenticationService.ValidateUser(reqUser);
            if (result.isAuthenticated == false)
            {
                return StatusCode(401, new ApiErrorResponse
                {
                    Message = "UnAuthorized",
                    Errors = new Dictionary<string, string[]>
                    {
                        { "UnAuthorized", new[] { "Invalid Username or Password" } }
                    }
                });
            }
            Response.Cookies.Append(HeaderConstants.JwtCookie, result.token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(15)
            });
            result.token = "";
            return StatusCode(200, new ApiResponse<VMAuthResponse>
            {
                Code = 200,
                Message = "Authorized",
                Data = result
            });
        }
        [Authorize] // or AllowAnonymous if you prefer
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append(HeaderConstants.JwtCookie, "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1) // Past date = delete
            });

            return StatusCode(200, new ApiResponse<string>
            {
                Code = 200,
                Message = "Successfully Logged Out",
                Data = ""
            });
        }
        [Authorize] // or AllowAnonymous if you prefer
        [HttpGet("Validate")]
        public IActionResult ValidateAuthentication()
        {

            return Ok();
        }
    }
}
