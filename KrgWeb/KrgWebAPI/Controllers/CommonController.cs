using DBLayer.Models;
using DBLayer.Service;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        public CommonController(ICommonService iCommonService)
        {
            _commonService = iCommonService;
        }

        [HttpGet("getProductUnitType")]
        public async Task<IActionResult> getAllProductList()
        {
            var result = await _commonService.getProductUnitTypeAsync();

            return StatusCode(200, new ApiResponse<List<Vconstant>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
    }
}
