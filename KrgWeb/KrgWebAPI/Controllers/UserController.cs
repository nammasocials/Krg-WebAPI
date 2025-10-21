using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KrgWebAPI.Controllers
{
    [Route("api/[controller]")]
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
        [HttpPost]
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
            return StatusCode(200, new ApiResponse<VMAuthResponse>
            {
                Code = 401,
                Message = "UnAuthorized",
                Data = result
            });
        }
    }
}
