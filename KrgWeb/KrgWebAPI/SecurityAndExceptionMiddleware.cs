using DBLayer.Models;
using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace KrgWebAPI
{
    public class SecurityAndExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityAndExceptionMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IAuthenticationService _authenticationService;
        //private readonly IUserClaimsService _userClaimsService;

        public SecurityAndExceptionMiddleware(RequestDelegate next, 
            ILogger<SecurityAndExceptionMiddleware> logger,
            IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopeFactory;
            //_authenticationService = iAuthenticationService;
            //_userClaimsService = iUserClaimsService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var _authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                    var _userClaimsService = scope.ServiceProvider.GetRequiredService<IUserClaimsService>();

                    var endpoint = context.GetEndpoint();
                    var hasAllowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null;
                    if (!hasAllowAnonymous)
                    {
                        var user = _userClaimsService.GetUserClaims();
                        var token = _authenticationService.GenerateToken(user);

                        // Add to response header
                        context.Response.Headers["X-New-JWT"] = token;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new ApiErrorResponse
                {
                    Message = "Business logic error.",
                    Errors = new Dictionary<string, string[]>
                    {
                        { "General", new[] { ex.Message } }
                    }
                };

                var json = JsonSerializer.Serialize(errorResponse);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);

            }
        }
    }
}
