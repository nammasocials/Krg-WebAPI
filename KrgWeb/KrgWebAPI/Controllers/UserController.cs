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
        private IAuthenticationService _authenticationService;
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
            throw new Exception("For testing custom exception");
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
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10)
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
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append(HeaderConstants.JwtCookie, "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1) // Past date = delete
            });

            return Ok(new { message = "Logged out" });
        }
        [Authorize] // or AllowAnonymous if you prefer
        [HttpGet("Test")]
        public IActionResult TestAuthentication()
        {

            return Ok(new { message = "Working" });
        }
    }
}
