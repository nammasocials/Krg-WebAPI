using DBLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBLayer
{
    public class SecurityAndExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityAndExceptionMiddleware> _logger;

        public SecurityAndExceptionMiddleware(RequestDelegate next, ILogger<SecurityAndExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
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
