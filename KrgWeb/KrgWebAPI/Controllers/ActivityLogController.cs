using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UtilityLayer;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityLogController : ControllerBase
    {
        private readonly IRecentActivityService _recentActivityService;
        public ActivityLogController(IRecentActivityService iRecentActivityService)
        {
            _recentActivityService = iRecentActivityService;
        }
        [HttpGet("getRecentActivityLogs")]
        public async Task<IActionResult> getRecentActivityLogs()
        {
            var result = await _recentActivityService.fetchActivityLogsList();

            return StatusCode(200, new ApiResponse<List<VactivityLog>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
    }
}