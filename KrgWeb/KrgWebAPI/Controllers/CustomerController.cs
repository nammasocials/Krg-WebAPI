using DBLayer.Models;
using DBLayer.Service;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService iCustomerService)
        {
            _customerService = iCustomerService;
        }

        [HttpGet("getAllCustomerList")]
        public async Task<IActionResult> getAllCustomerList()
        {
            var result = await _customerService.fetchCustomerList();

            return StatusCode(200, new ApiResponse<List<Vcustomer>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
    }
}
